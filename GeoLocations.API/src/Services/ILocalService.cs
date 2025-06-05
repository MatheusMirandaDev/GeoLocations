using GeoLocations.API.src.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GeoLocations.API.src.Services;

/// <summary>
/// Contrato para o serviço de gerenciamento de locais geográficos
/// </summary>
public interface ILocalService
{
    /// <summary>
    /// Cria um novo local geográfico com base nos dados fornecidos.
    /// </summary>
    /// <param name="localDto">DTO contendo os dados para a criação do novo local.</param>
    /// <returns>Um <see cref="LocalResponseDto"/> DTO responsavel por exibir a resposta do local criado.</returns>
    /// <exception cref="Exception">Lançada se ocorrer um erro inesperado durante a persistência dos dados.</exception>
    Task<LocalResponseDto> CreateLocal(CreateLocalDto localDto);

    /// <summary>
    /// Obtém uma lista de todos os locais geográficos cadastrados.
    /// </summary>
    /// <returns>Uma coleção de <see cref="LocalResponseDto"/> representando todos os locais.</returns>
    Task<IEnumerable<LocalResponseDto>> GetAllLocais();

    /// <summary>
    /// Obtém todos os locais geográficos cadastrados no formato de string GeoJSON.
    /// </summary>
    /// <returns>Uma string contendo a representação GeoJSON (FeatureCollection) dos locais.</returns>
    Task<string> GetLocaisAsGeoJson();

    /// <summary>
    /// Busca um local geográfico específico pelo seu identificador único.
    /// </summary>
    /// <param name="id">O ID do local a ser buscado.</param>
    /// <returns>Um <see cref="LocalResponseDto"/> representando o local encontrado, ou nulo se nenhum local com o ID fornecido for encontrado.</returns>
    Task<LocalResponseDto?> GetLocalById(int id);

    /// <summary>
    /// Atualiza os dados de um local geográfico existente.
    /// </summary>
    /// <param name="id">O ID do local a ser atualizado.</param>
    /// <param name="localDto">DTO contendo os novos dados para o local.</param>
    /// <returns>Um <see cref="LocalResponseDto"/> representando o local atualizado, ou nulo se o local não for encontrado.</returns>
    /// <exception cref="Exception">Lançada se ocorrer um erro inesperado durante a persistência dos dados.</exception>
    Task<LocalResponseDto?> UpdateLocal(int id, UpdateLocalDto localDto);

    /// <summary>
    /// Exclui um local geográfico específico pelo seu identificador único.
    /// </summary>
    /// <param name="id">O ID do local a ser excluído.</param>
    /// <returns>Verdadeiro se o local foi excluído com sucesso, falso se o local não foi encontrado.</returns>
    /// <exception cref="Exception">Lançada se ocorrer um erro inesperado durante a exclusão.</exception>
    Task<bool> DeleteLocal(int id);
}
