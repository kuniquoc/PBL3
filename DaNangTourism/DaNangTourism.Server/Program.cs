using DaNangTourism.Server.DAL;
using DaNangTourism.Server.Services;

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
                      policy.WithOrigins("http://localhost:5173")
                              .AllowAnyHeader() // Allow all headers
                              .AllowAnyMethod(); // Allow all methods
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

builder.Services.AddScoped<IScheduleRepository>(provider =>
{
  var configuration = provider.GetRequiredService<IConfiguration>();
  var connectionString = configuration.GetConnectionString("DefaultConnection");
  if (string.IsNullOrEmpty(connectionString))
  {
    throw new Exception("DefaultConnection connection string is not configured in appsettings.json.");
  }
  return new ScheduleRepository(connectionString);
});

builder.Services.AddScoped<IScheduleDestinationRepository>(provider =>
{
  var configuration = provider.GetRequiredService<IConfiguration>();
  var connectionString = configuration.GetConnectionString("DefaultConnection");
  if (string.IsNullOrEmpty(connectionString))
  {
    throw new Exception("DefaultConnection connection string is not configured in appsettings.json.");
  }
  return new ScheduleDestinationRepository(connectionString);
});

builder.Services.AddScoped<IAccountRepository>(provider =>
{
  var configuration = provider.GetRequiredService<IConfiguration>();
  var connectionString = configuration.GetConnectionString("DefaultConnection");
  if (string.IsNullOrEmpty(connectionString))
  {
    throw new Exception("DefaultConnection connection string is not configured in appsettings.json.");
  }
  return new AccountRepository(connectionString);
});

builder.Services.AddScoped<IBlogRepository>(provider =>
{
  var configuration = provider.GetRequiredService<IConfiguration>();
  var connectionString = configuration.GetConnectionString("DefaultConnection");
  if (string.IsNullOrEmpty(connectionString))
  {
    throw new Exception("DefaultConnection connection string is not configured in appsettings.json.");
  }
  return new BlogRepository(connectionString);
});

// Register gmail password
builder.Services.AddScoped<IEmailService>(provider =>
{
  var configuration = provider.GetRequiredService<IConfiguration>();
  var fromPassword = configuration["GmailPassword"];
  if (string.IsNullOrEmpty(fromPassword))
  {
    throw new Exception("GmailPassword is not configured in appsettings.json.");
  }
  return new EmailService(fromPassword);
});

// Register scoped services here

builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBlogService, BlogService>();


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

// app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Map fallback to index.html
app.MapFallbackToFile("/index.html");

app.Run();