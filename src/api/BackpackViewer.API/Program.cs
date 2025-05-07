using BackpackViewer.Core;
using BackpackViewer.Core.Caching;
using BackpackViewer.Core.Services;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "www"
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddTransient<ITf2BackpackLoader, Tf2BackpackLoader>();
builder.Services.AddTransient<IMockTf2BackpackLoader, MockTf2BackpackLoader>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient(typeof(ICache<>), typeof(Cache<>));
builder.Services.AddTransient<IBackpackCache, BackpackCache>();
builder.Services.AddTransient<IItemSchemaCache, ItemSchemaCache>();
builder.Services.AddMemoryCache();

// Enable CORS
// https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0

var corsPolicyName = "CORS-LocalReact";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName, policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173");
    });
});

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseAuthorization();
app.MapControllers();

app.Run();

