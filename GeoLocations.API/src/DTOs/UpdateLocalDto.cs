using GeoLocations.API.src.Models;
using System.ComponentModel.DataAnnotations;

namespace GeoLocations.API.src.DTOs;

/// <summary>
/// DTO responsável pela atualização (PUT) de um novo local geográfico.
/// </summary>
public class UpdateLocalDto
{
    /// <summary>
    /// Nome do local.
    /// </summary>
    [Required(ErrorMessage = "O nome do local é um campo obrigatório.")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Categoria (enum) do local
    /// </summary>
    [Required(ErrorMessage = "A categoria do local é um campo obrigatória.")]
    [Range(0, 8, ErrorMessage = "As opções devem ser entre 0 e 8 (para mais informações, acesse CreateLocalDto no final da pagina!)")]
    public CategoriaLocal Categoria { get; set; }

    /// <summary>
    /// Latitude da coordenada geográfica do local.
    /// Deve estar entre -90 e 90.
    /// </summary>
    [Required(ErrorMessage = "A latitude do local é um campo obrigatório.")]
    [Range(-90, 90, ErrorMessage = "A latitude deve estar entre -90 e 90")]
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude da coordenada geográfica do local.
    /// Deve estar entre -180 e 180.
    /// </summary> 
    [Required(ErrorMessage = "A longitude do local é um campo obrigatório.")]
    [Range(-180, 180, ErrorMessage = "A longitude deve estar entre -180 e 180")]
    public double Longitude { get; set; }

}
