using GeoLocations.API.src.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace GeoLocations.API.src.DataAccess;

public class GeoLocationsContext : DbContext
{
    public GeoLocationsContext(DbContextOptions<GeoLocationsContext> options)
        : base(options) { }

    public DbSet<Locais> Locais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<Locais>().Property(l => l.Coordenada)
            .HasColumnType("geography(Point, 4326)"); // O erro sumirá aqui
    }
}