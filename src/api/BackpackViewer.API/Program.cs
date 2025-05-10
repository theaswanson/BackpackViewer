using System.Threading.RateLimiting;
using BackpackViewer.Core.Caching;
using BackpackViewer.Core.Services;

namespace BackpackViewer.API;

internal static class Program
{
    public static async Task Main(string[] args) =>
        await BuildApp(args).RunAsync();

    private static WebApplication BuildApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            WebRootPath = "www"
        }).ConfigureServices();
        
        var app = builder.Build();
        
        return app.Configure();
    }

    private static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
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

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AppConstants.LocalCorsPolicyName,
                    policy => policy.WithOrigins("http://localhost:5173", "https://localhost:5173"));
            });
        }
        else
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AppConstants.CorsPolicyName,
                    policy => policy.WithOrigins("https://theaswanson.github.io"));
            });
        }

        return builder;
    }

    private static WebApplication Configure(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseCors(app.Environment.IsDevelopment()
            ? AppConstants.LocalCorsPolicyName
            : AppConstants.CorsPolicyName);
        app.UseAuthorization();
        app.UseRateLimiter();
        app.MapControllers();

        return app;
    }
}