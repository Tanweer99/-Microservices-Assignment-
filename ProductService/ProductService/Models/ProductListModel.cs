using Newtonsoft.Json;

namespace ProductService.Models
{
    public class ProductListModel
    {
        [JsonProperty("ProductId")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
    }
}
