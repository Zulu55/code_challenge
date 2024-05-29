using AspNetCoreRateLimit;
using Microsoft.Data.SqlClient;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using System.Data;

namespace ProductApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimit"));
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddControllers();
        services.AddSingleton<IDbConnection>(sp => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<ProductRepository>();
        services.AddScoped<IProductRepository, CachedProductRepository>();
        services.Decorate<IProductRepository, ProductRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseIpRateLimiting();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}