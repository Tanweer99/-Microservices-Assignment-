using MassTransit;
using PriceService.Service;
using Common;

namespace PriceService.Consumer
{
    public class PriceRequestConsumer : IConsumer<PriceRequest>
    {
        private readonly IPriceService _priceService;

        public PriceRequestConsumer(IPriceService priceService)
        {
            _priceService = priceService;
        }

        public async Task Consume(ConsumeContext<PriceRequest> context)
        {
            // Retrieve the price for the specified product ID from the database
            var receivedMessage = context.Message;
            int productId = receivedMessage.ProductId;
            decimal price = RetrievePriceForProductId(productId);
            // Send the price back to the product details service
            await context.RespondAsync(new PriceResponse { Price = price });
        }

        private decimal RetrievePriceForProductId(int productId)
        {
            // Retrieve the price for the specified product ID from the PriceService.cs
            var price = _priceService.Get(productId);
            if (price == null)
            {
                throw new ArgumentException($"Price not available for product ID {productId}");
            }

            return price.Value;
        }
    }
}
