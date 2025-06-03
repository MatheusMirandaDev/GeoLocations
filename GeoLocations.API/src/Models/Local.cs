using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace GeoLocations.API.src.Models;

/// <summary>
/// Representa as categorias de locais disponíveis. | 0 - Farmácias | 1 - Restaurantes | 2 - Hospitais | 3 - Supermercados | 4 - Postos de Combustível | 5 - Escolas | 6 - Parques | 7 - Shoppings | 8 - Outros.|
/// </summary>
public enum CategoriaLocal
{
    /// <summary>
    /// 0 - Farmácias.
    /// </summary>
    Farmacia,

    /// <summary>
    /// 1 - Restaurantes.
    /// </summary>
    Restaurante,

    /// <summary>
    /// 2 - Hospitais.
    /// </summary>
    Hospital,

    /// <summary>
    /// 3 - Supermercados.
    /// </summary>
    Supermercado,

    /// <summary>
    /// 4 - Combustível.
    /// </summary>
    PostoCombustivel,

    /// <summary>
    /// 5 - Escolas.
    /// </summary>
    Escola,

    /// <summary>
    /// 6 - Parques.
    /// </summary>
    Parque,

    /// <summary>
    /// 7 - Shoppings.
    /// </summary>
    Shopping,

    /// <summary>
    /// 8 - Outros.
    /// </summary>
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
    [Required(ErrorMessage = "O nome do local é um campo obrigatório.")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Categoria (enum) do local
    /// </summary>
    [Required(ErrorMessage = "A categoria do local é um campo obrigatória.")]
    public CategoriaLocal Categoria { get; set; }

    /// <summary>
    /// Coordenada geográfica do local (Point SRID 4326).
    /// </summary>
    [Required(ErrorMessage = "A coordenada do local é um campo obrigatória.")]
    public required Point Coordenada { get; set; }
}
