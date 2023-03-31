using Common;
using MassTransit;
using ProductDetailService.Models;
using ProductDetailService.Service;

namespace ProductDetailService.Consumer
{
    public class ProductDetailConsumer : IConsumer<ProductDetailRequest>
    {
        private readonly IProductDetailsService _productService;
        public ProductDetailConsumer(IProductDetailsService productService)
        {
            _productService = productService;
        }
        public async Task Consume(ConsumeContext<ProductDetailRequest> context)
        {
            var productId = context.Message.ProductId;
            var productDetail = RetrieveProductDetail(productId);
            var productDetailResponse = new ProductDetailResponse
            {
                Size = productDetail.Size,
                Design = productDetail.Design,
            };
            await context.RespondAsync(productDetailResponse);
        }

        private ProductDetail RetrieveProductDetail(int productId)
        {
            // Retrieve the price for the specified product ID from the PriceService.cs
            var productDetail = _productService.Get(productId);
            if (productDetail == null)
            {
                throw new ArgumentException($"ProductDetail not available for product ID {productId}");
            }
            return productDetail;
        }
    }
}
