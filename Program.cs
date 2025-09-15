using CleanArch.Infrastructure.Persistence;
using InfrastructureLayer;
using InfrastructureLayer.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Database connection
var cs = builder.Configuration.GetConnectionString("Default") ?? "Data Source=./AppData/app.db";

builder.Services.AddInfrastructure(cs);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Ensure AppData folder exists
Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "AppData"));

// Ensure database created (demo only – use migrations in production)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();

    DbSeeder.Seed(db);
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
