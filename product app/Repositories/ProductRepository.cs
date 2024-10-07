using Dapper;
using Microsoft.Data.SqlClient;
using product_app.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class ProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        using (IDbConnection db = new SqlConnection(_connectionString))
        {
            return await db.QueryAsync<Product>("SELECT * FROM Products");
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var product = await connection.QueryFirstOrDefaultAsync<Product>(
                "SELECT * FROM Products WHERE Id = @Id", new { Id = id });
            return product; // This can be null if not found
        }
    }


    public async Task AddProductAsync(Product product)
    {
        using (IDbConnection db = new SqlConnection(_connectionString))
        {
            string sqlQuery = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";
            await db.ExecuteAsync(sqlQuery, product);
        }
    }

    public async Task UpdateProductAsync(Product product)
    {
        using (IDbConnection db = new SqlConnection(_connectionString))
        {
            string sqlQuery = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
            await db.ExecuteAsync(sqlQuery, product);
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        using (IDbConnection db = new SqlConnection(_connectionString))
        {
            await db.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { Id = id });
        }
    }
}
