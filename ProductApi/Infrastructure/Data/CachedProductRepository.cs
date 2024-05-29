using Microsoft.Extensions.Caching.Memory;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Data;

public class CachedProductRepository : IProductRepository
{
    private readonly IProductRepository _inner;
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public CachedProductRepository(IProductRepository inner, IMemoryCache cache)
    {
        _inner = inner;
        _cache = cache;
        _cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        if (!_cache.TryGetValue("all_products", out IEnumerable<Product> products))
        {
            products = await _inner.GetAllAsync();
            _cache.Set("all_products", products, _cacheOptions);
        }
        return products!;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var cacheKey = $"product_{id}";
        if (!_cache.TryGetValue(cacheKey, out Product product))
        {
            product = await _inner.GetByIdAsync(id);
            _cache.Set(cacheKey, product, _cacheOptions);
        }
        return product!;
    }

    public Task AddAsync(Product product) => _inner.AddAsync(product);

    public Task UpdateAsync(Product product) => _inner.UpdateAsync(product);

    public Task DeleteAsync(int id) => _inner.DeleteAsync(id);
}