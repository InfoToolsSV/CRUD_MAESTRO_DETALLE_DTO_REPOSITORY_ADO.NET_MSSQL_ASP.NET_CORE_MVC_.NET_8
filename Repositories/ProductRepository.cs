using System.Data.SqlClient;
using System.Data;
using ProductSales.DTOs;

namespace ProductSales.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            var cadena = configuration.GetConnectionString("DefaultConnection");
            if (cadena != null)
            {
                _connectionString = cadena;
            }
            else
                _connectionString = "";
        }


        public async Task<int> AddProductAsync(ProductDTO productDTO)
        {
            int productID;

            using (SqlConnection con = new(_connectionString))
            {
                using SqlCommand cmd = new("sp_InsertProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", productDTO.Name);
                cmd.Parameters.AddWithValue("@Price", productDTO.Price);
                await con.OpenAsync();
                productID = Convert.ToInt32(await cmd.ExecuteScalarAsync());

            }
            return productID;
        }

        public async Task DeleteProductAsync(int id)
        {
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("sp_DeleteProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", id);
            await con.OpenAsync();
            await cmd.ExecuteScalarAsync();
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = new List<ProductDTO>();

            using (SqlConnection con = new(_connectionString))
            {
                using SqlCommand cmd = new("sp_GetAllProducts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                await con.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    products.Add(new ProductDTO
                    {
                        ProductID = (int)reader["ProductID"],
                        Name = (string)reader["Name"],
                        Price = (decimal)reader["Price"]
                    });
                }
            }
            return products;
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            ProductDTO product = new();

            using (SqlConnection con = new(_connectionString))
            {
                using SqlCommand cmd = new("sp_GetProductByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductID", id);
                await con.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    product = new ProductDTO
                    {
                        ProductID = (int)reader["ProductID"],
                        Name = (string)reader["Name"],
                        Price = (decimal)reader["Price"]
                    };
                }
            }
            return product;
        }

        public async Task UpdateProductAsync(ProductDTO productDTO)
        {
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("sp_UpdateProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", productDTO.ProductID);
            cmd.Parameters.AddWithValue("@Name", productDTO.Name);
            cmd.Parameters.AddWithValue("@Price", productDTO.Price);
            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}