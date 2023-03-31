using Common;
using MassTransit;
using ProductService.Controllers;
using ProductService.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace ProductService.Service
{
    public class ServiceProduct : IServiceProduct
    {
        private readonly IRequestClient<ProductListRequest> _productListRequestclient;
        private readonly IRequestClient<ProductRequest> _productRequestClient;
        private readonly IRequestClient<PriceRequest> _priceRequestClient;
        private readonly IRequestClient<ProductDetailRequest> _productDetailRequestClient;

        public ServiceProduct(
            IRequestClient<ProductListRequest> productListRequestclient,
            IRequestClient<ProductRequest> productRequestClient,
            IRequestClient<PriceRequest> priceRequestClient,
            IRequestClient<ProductDetailRequest> productDetailRequestClient,
            ILogger<HomeController> logger)
        {
            _productListRequestclient = productListRequestclient;
            _productRequestClient = productRequestClient;
            _priceRequestClient = priceRequestClient;
            _productDetailRequestClient = productDetailRequestClient;
        }

        public async Task<Product> GetProduct(int productId)
        {
            Product product = new Product();
            //Get Product from Inventory service.
            var productRequestHandle = _productRequestClient.Create(new ProductRequest { ProductId = productId });
            var productResponse = await productRequestHandle.GetResponse<ProductResponse>();
            var productDataResponse = productResponse.Message;
            product.Id = productId;
            product.Name = productDataResponse.Name;
            product.Description = productDataResponse.Description;
            product.StockQuantity = productDataResponse.StockQuantity;
            //Get Product from Price service.
            var priceRequestHandle = _priceRequestClient.Create(new PriceRequest { ProductId = productId });
            var priceResponse = await priceRequestHandle.GetResponse<PriceResponse>();
            var priceDataResponse = priceResponse.Message;
            product.Price = priceDataResponse.Price;
            //Get Product from ProductDetail service.
            var productDetailRequestHandle = _productDetailRequestClient.Create(new ProductDetailRequest { ProductId = productId });
            var productDetailResponse = await productDetailRequestHandle.GetResponse<ProductDetailResponse>();
            var productDetailDataResponse = productDetailResponse.Message;
            product.Size = productDetailDataResponse.Size;
            product.Design = productDetailDataResponse.Design;

            return product;
        }

        public async Task<IEnumerable<ProductListModel>> GetProductList()
        {
            //Send request to Product Inventory service to get product list.
            var requestHandle = _productListRequestclient.Create(new ProductListRequest { productList = "GetProductListFromInventoryService" });
            var response = await requestHandle.GetResponse<ProductListResponse>();
            var dataResponse = response.Message.Data;
            var productListResponse = JsonSerializer.Deserialize<List<ProductListModel>>(dataResponse);

            //List<ProductListModel> productList = new List<ProductListModel>();
            //foreach (var product in productListResponse)
            //{
            //    productList.Add(new ProductListModel
            //    {
            //        Name = product.Name,
            //        Description = product.Description,
            //        StockQuantity = product.StockQuantity,
            //    });
            //} 
            return productListResponse;
        }
    }
}
