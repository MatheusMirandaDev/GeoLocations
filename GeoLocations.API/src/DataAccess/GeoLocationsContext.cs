using GeoLocations.API.src.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace GeoLocations.API.src.DataAccess;

/// <summary>
/// Classe de contexto do Entity Framework Core para a aplicação GeoLocations.
/// Faz o mapeamento (ORM) entre as entidades do modelo e o banco de dados PostgreSQL com suporte a PostGIS.
/// </summary>
public class GeoLocationsContext : DbContext
{
    /// <summary>
    /// Construtor que recebe as opções de configuração do contexto.
    /// </summary>
    /// <param name="options">As opções de configuração para este contexto de banco de dados.</param>
    public GeoLocationsContext(DbContextOptions<GeoLocationsContext> options)
        : base(options) { }

    /// <summary>
    /// Define a tabela Locais no contexto do banco de dados.
    /// O DbSet permite realizar operações CRUD na tabela Locais.
    /// </summary>
    public DbSet<Locais> Locais { get; set; }

    /// <summary>
    /// Método responsavel por configurar o modelo do Entity Framework Core.
    /// As configurações de extensões e tipos de dados específicos do PostGIS são aplicadas aqui.
    /// </summary>
    /// <param name="modelBuilder">O construtor de modelo usado para configurar o esquema do banco de dados.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // garante que as convenções padrão do EF Core sejam aplicadas.
        base.OnModelCreating(modelBuilder);

        // Registra a extensão PostGIS no modelo, permitindo o uso de tipos geoespaciais.
        modelBuilder.HasPostgresExtension("postgis");

        // Configura a entidade Locais para usar o tipo de dados geográfico do PostGIS.
        modelBuilder.Entity<Locais>().Property(l => l.Coordenada)
            .HasColumnType("geography(Point, 4326)"); 
    }
}