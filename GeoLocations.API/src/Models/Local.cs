using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace GeoLocations.API.src.Models;

/// <summary>
/// Representa as categorias de locais disponíveis.
/// 0 - Farmácia
/// 1 - Restaurante
/// 2 - Hospital
/// 3 - Supermercado
/// 4 - Posto de Combustível
/// 5 - Escola
/// 6 - Parque
/// 7 - Shopping
/// 8 - Outro
/// </summary>
public enum CategoriaLocal
{ 
    Farmacia,
    Restaurante,
    Hospital,
    Supermercado,
    PostoCombustivel,
    Escola,
    Parque,
    Shopping,
    Outro
};

/// <summary>
/// Representa um local geográfico com suas propriedades e categoria.
/// </summary>
public class Local
{
    /// <summary>
    /// Identificador único do local.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Nome do local.
    /// </summary>
    [Required(ErrorMessage = "O nome do local é obrigatório.")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Categoria (enum) do local
    /// </summary>
    [Required(ErrorMessage = "A categoria do local é obrigatória.")]
    public CategoriaLocal Categoria { get; set; }

    /// <summary>
    /// Coordenada geográfica do local (Point SRID 4326).
    /// </summary>
    [Required(ErrorMessage = "A coordenada do local é obrigatória.")]
    public required Point Coordenada { get; set; }
}
