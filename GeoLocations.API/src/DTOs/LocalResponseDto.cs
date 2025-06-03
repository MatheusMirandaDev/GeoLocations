using GeoLocations.API.src.Models;

namespace GeoLocations.API.src.DTOs;

/// <summary>
/// Dto para representar um local geográfico nas respostas da API.
/// </summary>
public class LocalResponseDto
{
    /// <summary>
    /// Identificador único do local.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Nome do local.
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    /// <summary>
    /// Categoria(enum) do local.
    /// </summary>
    public CategoriaLocal Categoria { get; set; }
    /// <summary>
    /// Latitude do local.
    /// </summary>
    public double Latitude { get; set; }
    /// <summary>
    /// Longitude do local.
    /// </summary>
    public double Longitude { get; set; }
}
