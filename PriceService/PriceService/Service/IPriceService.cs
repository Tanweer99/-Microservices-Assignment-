using PriceService.Models;

namespace PriceService.Service
{
    public interface IPriceService
    {
        void Add(Price price);
        IEnumerable<Price> Get();
        Price? Get(int productId);
        bool Remove(int productId);
    }
}