namespace ProductSales.DTOs
{
    public class SaleDetailDTO
    {
        public int SaleDetailID { get; set; }
        public int SaleID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}