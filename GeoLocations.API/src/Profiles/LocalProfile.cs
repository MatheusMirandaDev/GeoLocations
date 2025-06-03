using AutoMapper;
using GeoLocations.API.src.DTOs;
using GeoLocations.API.src.Models;
using NetTopologySuite.Geometries;

namespace GeoLocations.API.src.Profiles;

/// <summary>
/// Define o perfil de mapeamento (AutoMapper) para a entidade Local.
/// Configura o mapeamento entre o DTO CreateLocalDto e o Model Local.
/// </summary>
public class LocalProfile : Profile
{
    /// <summary>
    /// Construtor que configura todas as regras dp mapeamento entre CreateLocalDto e Local.
    /// </summary>
    public LocalProfile()
    {
        // Mapeamento do CreateLocalDto
        CreateMap<CreateLocalDto, Local>(). // Mapear CreateLocalDto para Local
            ForMember(dest => dest.Coordenada, opt => opt.MapFrom(src => // Converte Latitude e Longitude (DTO) para a prop. Coordenada (Model)
        new Point(src.Longitude, src.Latitude) { SRID = 4326 })); // Define o SRID como 4326 para coordenadas geográficas (Point)

        // Mapeamento do LocalResponseDto
        CreateMap<Local, LocalResponseDto>() // Mapear Local para LocalResponseDto
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Coordenada.Y)) // Mapeia a Latitude do ponto
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Coordenada.X)); // Mapeia a Longitude do ponto

        // Mapeamento do UpdateLocalDto
        CreateMap<UpdateLocalDto, Local>() // Mapear UpdateLocalDto para Local
            .ForMember(dest => dest.Coordenada, opt => opt.MapFrom(src => // Converte Latitude e Longitude (DTO) para a prop. Coordenada (Model)
            new Point(src.Longitude, src.Latitude) { SRID = 4326 })); // Define o SRID como 4326 para coordenadas geográficas (Point)
    }
}
