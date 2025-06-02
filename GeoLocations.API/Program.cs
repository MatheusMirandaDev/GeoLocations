using Microsoft.EntityFrameworkCore; // Essencial para AddDbContext
using Npgsql.EntityFrameworkCore.PostgreSQL; // Para UseNpgsql
using Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite; // Para UseNetTopologySuite
using GeoLocations.API.src.DataAccess; // Para GeoLocationsContext


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GeoLocationsContext>(options =>
    options.UseNpgsql(connectionString,
    x => x.UseNetTopologySuite())
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
