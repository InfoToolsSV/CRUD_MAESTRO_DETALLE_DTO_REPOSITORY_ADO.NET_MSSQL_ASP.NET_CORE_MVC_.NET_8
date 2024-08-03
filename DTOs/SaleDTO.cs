namespace ProductSales.DTOs
{
    public class SaleDTO
    {
        public int SaleID { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public List<SaleDetailDTO> SaleDetails { get; set; } = [];
    }
}