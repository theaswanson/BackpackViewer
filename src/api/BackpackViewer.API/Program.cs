using System.Threading.RateLimiting;
using BackpackViewer.Core.Caching;
using BackpackViewer.Core.Services;

namespace BackpackViewer.API;

internal static class Program
{
    public static async Task Main(string[] args) =>
        await BuildApp(args, AppConstants.CorsPolicyName).RunAsync();

    private static WebApplication BuildApp(string[] args, string corsPolicyName)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            WebRootPath = "www"
        }).ConfigureServices(corsPolicyName);
        
        var app = builder.Build();
        
        return app.Configure(corsPolicyName);
    }

    private static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder, string corsPolicyName)
    {
        builder.Services.AddControllers();

        builder.Services
            .AddTransient<ITf2BackpackLoader, Tf2BackpackLoader>()
            .AddTransient<IMockTf2BackpackLoader, MockTf2BackpackLoader>()
            .AddTransient<IItemService, ItemService>()
            .AddTransient(typeof(ICache<>), typeof(Cache<>))
            .AddTransient<IBackpackCache, BackpackCache>()
            .AddTransient<IItemSchemaCache, ItemSchemaCache>();

        builder.Services.AddMemoryCache();
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ??
                                  httpContext.Connection.RemoteIpAddress?.ToString() ??
                                  httpContext.Request.Headers.Host.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 10,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1)
                    }));
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(corsPolicyName,
                policy => { policy.WithOrigins("http://localhost:5173", "https://localhost:5173"); });
        });

        return builder;
    }

    private static WebApplication Configure(this WebApplication app, string corsPolicyName)
    {
        app.UseHttpsRedirection();
        app.UseCors(corsPolicyName);
        app.UseAuthorization();
        app.UseRateLimiter();
        app.MapControllers();

        return app;
    }
}