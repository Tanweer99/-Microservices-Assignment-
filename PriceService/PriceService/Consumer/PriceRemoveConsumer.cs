using Common;
using MassTransit;
using PriceService.Service;

namespace PriceService.Consumer
{
    public class PriceRemoveConsumer : IConsumer<RemovePrice>
    {
        private readonly IPriceService _priceService;

        public PriceRemoveConsumer(IPriceService priceService)
        {
            _priceService = priceService;
        }

        public Task Consume(ConsumeContext<RemovePrice> context)
        {
            // Retrieve the price for the specified product ID from the database
            var receivedMessage = context.Message;
            int productId = receivedMessage.ProductId;
            RetrievePriceForProductId(productId);
            return Task.CompletedTask;
        }

        private void RetrievePriceForProductId(int productId)
        {
            // Retrieve the price for the specified product ID from the PriceService.cs
            var result = _priceService.Remove(productId);
            if (!result)
            {
                throw new ArgumentException($"Price not found for product ID {productId}");
            }
        }
    }
}
