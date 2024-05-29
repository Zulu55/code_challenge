using Dapper;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using System.Data;

namespace ProductApi.Infrastructure.Data;

public class ProductRepository : IProductRepository
{
    private readonly IDbConnection _dbConnection;

    public ProductRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var sql = "SELECT * FROM Products";
        return await _dbConnection.QueryAsync<Product>(sql);
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Products WHERE Id = @Id";
        return await _dbConnection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
    }

    public async Task AddAsync(Product product)
    {
        var sql = "INSERT INTO Products (Name, Brand, Price_Amount, Price_Currency) VALUES (@Name, @Brand, @Price_Amount, @Price_Currency)";
        await _dbConnection.ExecuteAsync(sql, new
        {
            product.Name,
            product.Brand,
            Price_Amount = product.Price.Amount,
            Price_Currency = product.Price.Currency
        });
    }

    public async Task UpdateAsync(Product product)
    {
        var sql = "UPDATE Products SET Name = @Name, Brand = @Brand, Price_Amount = @Price_Amount, Price_Currency = @Price_Currency WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, new
        {
            product.Name,
            product.Brand,
            Price_Amount = product.Price.Amount,
            Price_Currency = product.Price.Currency,
            product.Id
        });
    }

    public async Task DeleteAsync(int id)
    {
        var sql = "DELETE FROM Products WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, new { Id = id });
    }
}