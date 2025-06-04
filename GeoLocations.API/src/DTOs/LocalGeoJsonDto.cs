namespace GeoLocations.API.src.DTOs;

/// <summary>
/// Dto para representar um local em formato GeoJSON.
/// </summary>
public class LocalGeoJsonDto
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
    /// Categoria do local.
    /// </summary>
    public string Categoria { get; set; }
}
