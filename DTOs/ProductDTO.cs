namespace ProductSales.DTOs
{
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }
}