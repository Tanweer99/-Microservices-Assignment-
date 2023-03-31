using ProductInventoryService.Models;

namespace ProductInventoryService.Service
{
    public class ServiceProductInventory : IProductInventoryService
    {
        //using in-memory db like - list of products.
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Teddy Bear", Description = "A cuddly and soft teddy bear with a red bowtie.", StockQuantity = 10 },
            new Product { Id = 2, Name = "Lego Technic Porsche 911", Description = "A realistic replica of the Porsche 911 sports car that can be built from Lego bricks.", StockQuantity = 5 },
            new Product { Id = 3, Name = "Barbie Dreamhouse", Description = "A colorful and interactive dollhouse that comes with multiple rooms, accessories, and furniture.", StockQuantity = 3 }
        };

        public IEnumerable<Product> Get()
        {
            return _products;
        }

        public Product? Get(int id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return null;
            }
            return product;
        }

        public Product Add(Product product)
        {
            var newProduct = new Product
            {
                Id = _products.Count + 1,
                Name = product.Name,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
            };
            _products.Add(newProduct);
            return newProduct;
        }

        public bool Delete(int id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return false;
            }
            return _products.Remove(product);
        }
    }
}
