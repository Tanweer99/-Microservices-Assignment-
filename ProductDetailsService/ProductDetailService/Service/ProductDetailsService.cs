using ProductDetailService.Models;

namespace ProductDetailService.Service
{
    public class ProductDetailsService : IProductDetailsService
    {
        //using in-memory db like - list of ProductDetails.
        private static List<ProductDetail> _productDetails = new List<ProductDetail>
        {// In-memory product details with price
            (new ProductDetail { ProductId = 1, Size = "Large", Design = "Red" }),
            (new ProductDetail { ProductId = 2, Size = "Medium", Design = "Blue" }),
            (new ProductDetail { ProductId = 3, Size = "Small", Design = "Green" })
        };

        public IEnumerable<ProductDetail> Get()
        {
            return _productDetails;
        }

        public ProductDetail? Get(int id)
        {
            var productDetail = _productDetails.FirstOrDefault(x => x.ProductId == id);
            if (productDetail == null)
            {
                return null;
            }
            return productDetail;
        }

        public ProductDetail Add(ProductDetail productDetail)
        {
            var newProductDetail = new ProductDetail
            {
                ProductId = productDetail.ProductId,
                Size = productDetail.Size,
                Design = productDetail.Design,
            };
            _productDetails.Add(newProductDetail);
            return newProductDetail;
        }

        public bool Delete(int id)
        {
            var productDetail = _productDetails.FirstOrDefault(x => x.ProductId == id);
            if (productDetail == null)
            {
                return false;
            }
            return _productDetails.Remove(productDetail);
        }
    }
}
