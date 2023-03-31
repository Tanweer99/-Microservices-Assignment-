using Common;
using MassTransit;
using ProductInventoryService.Models;
using ProductInventoryService.Service;

namespace ProductInventoryService.Consumer
{
    public class ProductConsumer : IConsumer<ProductRequest>
    {
        private readonly IProductInventoryService _productService;
        public ProductConsumer(IProductInventoryService productService)
        {
            _productService = productService;
        }
        public async Task Consume(ConsumeContext<ProductRequest> context)
        {
            var productId = context.Message.ProductId;
            //Get product info and send it to product-microservice.
            var productInfo = RetrieveProduct(productId);
            var product = new ProductResponse
            {
                Name = productInfo.Name,
                Description = productInfo.Description,
                StockQuantity = productInfo.StockQuantity
            };
            await context.RespondAsync(product);
        }

        private Product RetrieveProduct(int productId)
        {
            // Retrieve the price for the specified product ID from the PriceService.cs
            var product = _productService.Get(productId);
            if (product == null)
            {
                throw new ArgumentException($"Product not available for product ID {productId}");
            }
            return product;
        }
    }
}
