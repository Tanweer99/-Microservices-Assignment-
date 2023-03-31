using ProductDetailService.Models;

namespace ProductDetailService.Service
{
    public interface IProductDetailsService
    {
        ProductDetail Add(ProductDetail productDetail);
        bool Delete(int id);
        IEnumerable<ProductDetail> Get();
        ProductDetail? Get(int id);
    }
}