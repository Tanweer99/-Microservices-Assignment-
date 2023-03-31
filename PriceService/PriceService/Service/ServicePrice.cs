using MassTransit.Futures.Contracts;
using PriceService.Models;

namespace PriceService.Service
{
    public class ServicePrice : IPriceService
    {
        private static List<Price> _priceList = new List<Price>()
        {
             (new Price { Id = 1, ProductId = 1, Value = 20.0m }),
             (new Price { Id = 2, ProductId = 2, Value = 15.0m }),
             (new Price { Id = 3, ProductId = 3, Value = 10.0m }),
        };

        public IEnumerable<Price> Get()
        {
            return _priceList;
        }

        public Price? Get(int productId)
        {
            var price = _priceList.FirstOrDefault(x => x.ProductId == productId);
            if (price == null)
            {
                return null;
            }
            return price;
        }

        public void Add(Price price)
        {
            var newPrice = new Price
            {
                Id = _priceList.Last().Id + 1,
                ProductId = price.ProductId,
                Value = price.Value,
            };
            _priceList.Add(newPrice);
        }

        public bool Remove(int productId)
        {
            var result = _priceList.FirstOrDefault(x => x.ProductId == productId);
            if (result == default)
            {
                return false;
            }
            return _priceList.Remove(result);
        }
    }
}
