using System.Data;
using System.Data.SqlClient;
using ProductSales.DTOs;

namespace ProductSales.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly string _connectionString;

        public SaleRepository(IConfiguration configuration)
        {
            var cadena = configuration.GetConnectionString("DefaultConnection");
            if (cadena != null)
            {
                _connectionString = cadena;
            }
            else
                _connectionString = "";
        }
        public async Task<int> AddSaleAsync(SaleDTO saleDTO)
        {
            int saleID;
            var saleDetailsTable = new DataTable();

            saleDetailsTable.Columns.Add("ProductID", typeof(int));
            saleDetailsTable.Columns.Add("Quantity", typeof(int));
            saleDetailsTable.Columns.Add("Price", typeof(decimal));

            foreach (var detail in saleDTO.SaleDetails)
            {
                var productPrice = await GetProductPriceAsync(detail.ProductID);
                saleDetailsTable.Rows.Add(detail.ProductID, detail.Quantity, productPrice);
            }

            saleDTO.Total = saleDetailsTable.AsEnumerable().Sum(x => x.Field<decimal>("Price") * x.Field<int>("Quantity"));

            using (SqlConnection con = new(_connectionString))
            {
                using SqlCommand cmd = new("sp_InsertSale", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date", saleDTO.Date);
                cmd.Parameters.AddWithValue("@Total", saleDTO.Total);

                var param = cmd.Parameters.AddWithValue("@SaleDetails", saleDetailsTable);
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "SaleDetailsType";

                await con.OpenAsync();
                saleID = Convert.ToInt32(await cmd.ExecuteScalarAsync());

            }
            return saleID;
        }

        private async Task<decimal> GetProductPriceAsync(int productID)
        {
            decimal price = 0;

            using (SqlConnection con = new(_connectionString))
            {
                using SqlCommand cmd = new("sp_GetProductPrice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductID", productID);
                await con.OpenAsync();
                price = Convert.ToDecimal(await cmd.ExecuteScalarAsync());
            }
            return price;
        }

        public async Task DeleteSaleAsync(int id)
        {
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("sp_DeleteSale", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SaleID", id);
            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<SaleDTO>> GetAllSalesAsync()
        {
            var sales = new List<SaleDTO>();
            using SqlConnection con = new(_connectionString);
            using (SqlCommand cmd = new("sp_GetAllSales", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await con.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    sales.Add(new SaleDTO
                    {
                        SaleID = reader.GetInt32(reader.GetOrdinal("SaleID")),
                        Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                        Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                        SaleDetails = []
                    });
                }

                if (await reader.NextResultAsync())
                {
                    foreach (var sale in sales)
                    {
                        while (await reader.ReadAsync() && sale.SaleID == reader.GetInt32(reader.GetOrdinal("SaleID")))
                        {
                            sale.SaleDetails.Add(new SaleDetailDTO
                            {
                                SaleDetailID = reader.GetInt32(reader.GetOrdinal("SaleDetailsID")),
                                SaleID = reader.GetInt32(reader.GetOrdinal("SaleID")),
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                            });
                        }
                    }
                }
            }
            return sales;
        }

        public async Task<SaleDTO> GetSaleByIdAsync(int id)
        {
            SaleDTO sale = new();
            using SqlConnection con = new(_connectionString);
            using (SqlCommand cmd = new("sp_GetSaleByID", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SaleID", id);
                await con.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    sale = new SaleDTO
                    {
                        SaleID = reader.GetInt32(reader.GetOrdinal("SaleID")),
                        Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                        Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                        SaleDetails = []
                    };
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        sale.SaleDetails.Add(new SaleDetailDTO
                        {
                            SaleDetailID = reader.GetInt32(reader.GetOrdinal("SaleDetailsID")),
                            SaleID = reader.GetInt32(reader.GetOrdinal("SaleID")),
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                        });
                    }

                }
            }
            return sale;
        }

        public async Task UpdateSaleAsync(SaleDTO saleDTO)
        {
            using SqlConnection con = new(_connectionString);
            await con.OpenAsync();

            using var transaction = con.BeginTransaction();
            try
            {
                var deleteDetailsCommand = new SqlCommand("sp_DeleteSaleDetails", con, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };

                deleteDetailsCommand.Parameters.AddWithValue("@SaleID", saleDTO.SaleID);
                await deleteDetailsCommand.ExecuteNonQueryAsync();

                foreach (var item in saleDTO.SaleDetails)
                {
                    var insertDetailsCommand = new SqlCommand("sp_InsertSaleDetail", con,transaction)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    insertDetailsCommand.Parameters.AddWithValue("@SaleID", saleDTO.SaleID);
                    insertDetailsCommand.Parameters.AddWithValue("@ProductID", item.ProductID);
                    insertDetailsCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                    insertDetailsCommand.Parameters.AddWithValue("@Price", item.Price);
                    await insertDetailsCommand.ExecuteNonQueryAsync();
                }
                var updateSaleCommand = new SqlCommand("sp_UpdateSale", con, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };
                updateSaleCommand.Parameters.AddWithValue("@SaleID", saleDTO.SaleID);
                updateSaleCommand.Parameters.AddWithValue("@Date", saleDTO.Date);
                updateSaleCommand.Parameters.AddWithValue("@Total", saleDTO.Total);
                await updateSaleCommand.ExecuteNonQueryAsync();

                transaction.Commit();

            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}