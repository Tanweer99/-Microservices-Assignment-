using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace ProductService.Service
{
    public interface IServiceProduct
    {
        Task<Product> GetProduct(int productId);
        Task<IEnumerable<ProductListModel>> GetProductList();
    }
}