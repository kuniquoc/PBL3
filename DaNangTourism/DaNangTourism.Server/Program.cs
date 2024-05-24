using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Service;
using DaNangTourism.Server.Helper;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllers();

// Register CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173");
                      });
});

// Register API Explorer service
builder.Services.AddEndpointsApiExplorer();

// Register Swagger generator service
builder.Services.AddSwaggerGen();


// Register connection string here
builder.Services.AddScoped<IDestinationRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new Exception("DefaultConnection connection string is not configured in appsettings.json.");
    }
    return new DestinationRepository(connectionString);
});

builder.Services.AddScoped<IFavoriteDestinationRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new Exception("DefaultConnection connection string is not configured in appsettings.json.");
    }
    return new FavoriteDestinationRepository(connectionString);
});

builder.Services.AddScoped<IReviewRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new Exception("DefaultConnection connection string is not configured in appsettings.json.");
    }
    return new ReviewRepository(connectionString);
});

// Register scoped services here

builder.Services.AddScoped<IDestinationService, DestinationService>();

// Register helper services here

builder.Services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();

// Register IHttpContextAccessor services here

builder.Services.AddHttpContextAccessor();

// Register IConfiguration here

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);



var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI and JSON endpoint in development environment
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Map fallback to index.html
app.MapFallbackToFile("/index.html");

app.Run();