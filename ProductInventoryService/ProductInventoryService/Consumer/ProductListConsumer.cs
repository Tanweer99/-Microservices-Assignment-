using Common;
using MassTransit;
using ProductInventoryService.Service;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ProductInventoryService.Consumer
{
    public class ProductListConsumer : IConsumer<ProductListRequest>
    {
        private readonly IProductInventoryService _productService;

        public ProductListConsumer(IProductInventoryService productService)
        {
            _productService = productService;
        }

        public async Task Consume(ConsumeContext<ProductListRequest> context)
        {
            var received = context.Message;
            // get list of products and return it to product-microservice.
            var productResult = _productService.Get();

            string jsonData = JsonSerializer.Serialize(productResult);
            var responseMessage  = new ProductListResponse { Data = jsonData };
            await context.RespondAsync(responseMessage); 
        }
    }
}
