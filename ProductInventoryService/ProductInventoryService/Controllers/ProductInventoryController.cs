using Microsoft.AspNetCore.Mvc;
using ProductInventoryService.Models;
using ProductInventoryService.Service;

namespace ProductInventoryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductInventoryController : ControllerBase
    {
        private readonly IProductInventoryService _productService;

        public ProductInventoryController(IProductInventoryService productService)
        {
            _productService = productService;
        }
        // GET api/ProductInventory
        [HttpGet("GetProduct")]
        public ActionResult<IEnumerable<Product>> GetProductItems()
        {
            // Return a list of Product objects
            return _productService.Get().ToList();
        }

        // GET api/ProductInventory/5
        [HttpGet("GetProduct/{id}")]
        public ActionResult<ProductDTO> GetProduct(int id)
        {
            // Retrieve the product with the given ID from the in-memory database
            // Return a Product object
            var product = _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            return ProductToDto(product);
        }

        // POST api/ProductInventory
        [HttpPost("AddProduct")]
        public ActionResult<ProductDTO> Post([FromBody] ProductDTO productDto)
        {
            // Add the new product to the in-memory database
            var productItem = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                StockQuantity = productDto.StockQuantity
            };

            var newProduct = _productService.Add(productItem);

            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, ProductToDto(newProduct));

        }

        [HttpDelete("RemoveProduct/{id}")]
        public IActionResult Delete(int id)
        {
            // Remove the product with the given ID from the in-memory database
            var result = _productService.Delete(id);
            if (result == false)
            {
                return NotFound();
            }
            
            return Ok();
        }

        private static ActionResult<ProductDTO> ProductToDto(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
            };
        }
    }
}