using Common;
using MassTransit;
using PriceService.Models;
using PriceService.Service;

namespace PriceService.Consumer
{
    public class PriceAddConsumer : IConsumer<AddPrice>
    {
        private readonly IPriceService _priceService;
        public PriceAddConsumer(IPriceService priceService)
        {
            _priceService = priceService;
        }
        public Task Consume(ConsumeContext<AddPrice> context)
        {
            var priceMessage = context.Message;
            Price price = new Price
            {
                ProductId = priceMessage.ProductId,
                Value = priceMessage.PriceValue,
            };
            //Add price value of product via price service.
            _priceService.Add(price);   
            return Task.CompletedTask;
        }
    }
}
