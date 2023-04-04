using Common;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ProductDetailService.Models;
using ProductDetailService.Service;
using System.Collections;

namespace ProductDetailService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductDetailController : ControllerBase
    {
        private readonly IProductDetailsService _productService;
        private readonly IConfiguration _config;
        private readonly IBusControl _bus;
        private readonly IRequestClient<PriceRequest> _priceServiceClient;

        public ProductDetailController(IProductDetailsService productService, IConfiguration config,  IBusControl bus, IRequestClient<PriceRequest> priceServiceClient)
        {
            _productService = productService;
            _config = config;
            _bus = bus;
            _priceServiceClient = priceServiceClient;
        }

        [HttpGet("GetProductDetail/{productId}")]
        public async Task<ActionResult<ProductDetailDTO>> GetProductDetail(int productId)
        {
            var productDetail = _productService.Get(productId);
            if (productDetail == null)
            {
                return NotFound();
            }

            var priceValue = 0.0m;
            try
            {
                // Send request to price service to get price for product detail
                var request = _priceServiceClient.Create(new PriceRequest { ProductId = productDetail.ProductId });
                var response = await request.GetResponse<PriceResponse>();
                // Set price for product detail based on response from price service
                priceValue = response.Message.Price;
                
            }
            catch (RequestTimeoutException ex)
            {
                // Handle timeout exception from price service
                var errorMessage = $"Timeout exception occurred while requesting price for product detail {productDetail.ProductId} from price service. {ex.Message}";
                //_logger.LogError(errorMessage);
            }
            catch (Exception ex)
            {
                // Handle other exceptions from price service
                var errorMessage = $"An error occurred while requesting price for product detail {productDetail.ProductId} from price service. {ex.Message}";
                //_logger.LogError(errorMessage);
            }

            return ProductDetailToDto(productDetail, priceValue);
        }

        [HttpPost("AddProductDetail")]
        public async Task<ActionResult<ProductDetailDTO>> Post([FromBody] ProductDetailDTO productDetailDto)
        {
            var productDetail = new ProductDetail
            {
                ProductId = productDetailDto.ProductId,
                Size = productDetailDto.Size,
                Design = productDetailDto.Design,
                
            };

            //Add product detail into in-memory db
            ProductDetail productDetailResult = _productService.Add(productDetail);

            try
            {
                // notify price service to Add price of product as well.
                Uri uri = new Uri("rabbitmq://localhost/add-price");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(new AddPrice
                {
                    ProductId = productDetailResult.ProductId,
                    PriceValue = productDetailDto.Price
                });
            }
            catch (RequestTimeoutException ex)
            {
                // Handle timeout exception from price service
                var errorMessage = $"Timeout exception occurred while adding price for product {productDetailResult.ProductId} in price service. {ex.Message}";
                //_logger.LogError(errorMessage);
            }
            catch (Exception ex)
            {
                // Handle other exceptions from price service
                var errorMessage = $"An error occurred while adding price for product {productDetailResult.ProductId} in price service. {ex.Message}";
                //_logger.LogError(errorMessage);
            }

            return RedirectToAction(nameof(GetProductDetail), new { productId = productDetailResult.ProductId });
        }

        [HttpDelete("DeleteProductDetail/{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var productDetail = _productService.Get(productId);
            if (productDetail == null)
            {
                return NotFound();
            }
            _productService.Delete(productId);

            try
            {
                // notify price service to delete price of product as well.
                Uri uri = new Uri("rabbitmq://localhost/remove-price");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(new RemovePrice { ProductId = productDetail.ProductId });
            }
            catch (RequestTimeoutException ex)
            {
                // Handle timeout exception from price service
                var errorMessage = $"Timeout exception occurred while removing price for product {productDetail.ProductId} from price service. {ex.Message}";
                //_logger.LogError(errorMessage);
            }
            catch (Exception ex)
            {
                // Handle other exceptions from price service
                var errorMessage = $"An error occurred while removing price for product {productDetail.ProductId} from price service. {ex.Message}";
                //_logger.LogError(errorMessage);
            }

            return Ok();
        }

        private static ActionResult<ProductDetailDTO> ProductDetailToDto(ProductDetail productDetail, decimal price = 0.0m)
        {
            return new ProductDetailDTO
            {
                ProductId = productDetail.ProductId,
                Size = productDetail.Size,
                Price = price,  
                Design = productDetail.Design,
            };
        }
    }
}
