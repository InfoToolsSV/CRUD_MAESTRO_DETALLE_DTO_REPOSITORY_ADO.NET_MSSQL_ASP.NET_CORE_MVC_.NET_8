using ProductSales.DTOs;

namespace ProductSales.Repositories
{
    public interface ISaleRepository
    {
        Task<IEnumerable<SaleDTO>> GetAllSalesAsync();
        Task<SaleDTO> GetSaleByIdAsync(int id);
        Task<int> AddSaleAsync(SaleDTO saleDTO);
        Task UpdateSaleAsync(SaleDTO saleDTO);
        Task DeleteSaleAsync(int id);
    }
}