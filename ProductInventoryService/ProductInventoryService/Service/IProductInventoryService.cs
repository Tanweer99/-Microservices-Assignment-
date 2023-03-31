using ProductInventoryService.Models;

namespace ProductInventoryService.Service
{
    public interface IProductInventoryService
    {
        Product Add(Product product);
        bool Delete(int id);
        IEnumerable<Product> Get();
        Product? Get(int id);
    }
}