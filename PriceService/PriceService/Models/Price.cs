namespace PriceService.Models
{
    public class Price
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Value { get; set; }
    }
}
