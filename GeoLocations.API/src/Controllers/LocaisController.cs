using AutoMapper;
using GeoLocations.API.src.DataAccess;
using GeoLocations.API.src.DTOs;
using GeoLocations.API.src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Features;
using NetTopologySuite.IO.Converters;
using System.Text.Json;


namespace GeoLocations.API.src.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas aos locais geográficos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LocaisController : ControllerBase
{
    private readonly GeoLocationsContext _dbContext; // Contexto do banco de dados
    private readonly IMapper _mapper; // Mapeador de objetos para DTOs

    /// <summary>
    /// Construtor do controlador.
    /// </summary>
    /// <param name="dbContext">Contexto do banco de dados.</param>
    /// <param name="mapper">Mapeador de objetos para DTOs.</param>
    public LocaisController(GeoLocationsContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <summary>
    /// Cria um novo local geográfico.
    /// </summary>
    /// <param name="localDto">Dados do local a ser criado</param>
    /// <returns>Retorna o local criado</returns>
    /// <response code="201">Local criado com sucesso e retornado no corpo da resposta.</response>
    /// <response code="400">Erro ao tentar criar, dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLocal([FromBody] CreateLocalDto localDto)
    {
        var local = _mapper.Map<Local>(localDto); // Mapeia o DTO para a entidade Local
        await _dbContext.Locais.AddAsync(local); // Adiciona o local ao contexto do banco de dados
        await _dbContext.SaveChangesAsync(); // Salva as alterações no banco de dados

        var responseDto = _mapper.Map<LocalResponseDto>(local);

        // Retorna o local criado com o status 201 Created e a localização do novo recurso
        return CreatedAtAction(
            nameof(GetLocalById),
            new { id = responseDto.Id },
            responseDto
        );
    }

    /// <summary>
    /// Mostra a lista com todos os locais geográficos cadastrados.
    /// </summary>
    /// <returns>Retorna todos os locais geográficos cadastrados </returns>
    /// <response code="200">Lista a lista com todos os locais geográficos cadastrados.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LocalResponseDto>>> GetAllLocais()
    {
        var locais = await _dbContext.Locais.ToListAsync(); // Busca todos os locais do banco de dados
        return _mapper.Map<List<LocalResponseDto>>(locais); // Mapeia a lista de locais para DTOs de resposta
    }

    /// <summary>
    /// Mostra todos os locais geográficos cadastrados no formato GeoJSON.
    /// </summary>
    /// <returns> Retorna os locais geográficos no formato GeoJson </returns>
    /// /// <response code="200">Lista a lista com todos os locais geográficos cadastrados.</response>
    [HttpGet("geojson")]
    [Produces("application/geo+json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetLocaisAsGeoJson()
    {
        // Busca todos os locais do banco de dados sem rastreamento
        var locais = await _dbContext.Locais.AsNoTracking().ToListAsync();

        // Cria uma lista de features 
        var featureList = new List<Feature>();

        // Itera sobre cada local e cria uma feature GeoJSON
        foreach (var local in locais)
        {


            // Cria uma tabela de atributos para a feature
            var attributes = new AttributesTable();
            attributes.Add("id", local.Id);
            attributes.Add("nome", local.Nome);
            attributes.Add("categoria", local.Categoria.ToString());

            // Cria uma feature GeoJSON com a coordenada do local e os atributos
            var feature = new Feature(local.Coordenada, attributes);

            // Adiciona a feature à lista de features
            featureList.Add(feature);
        }

        // Cria uma FeatureCollection e adiciona todas as features
        var featureCollection = new FeatureCollection(); 
        foreach (var feature in featureList)
        {
            featureCollection.Add(feature); 
        }

        // Configura as opções de serialização para GeoJSON
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
            WriteIndented = true 
        };

        // Adiciona o conversor GeoJsonConverterFactory para serializar corretamente as coordenadas geográficas
        serializerOptions.Converters.Add(new GeoJsonConverterFactory());

        // Serializa a FeatureCollection para uma string GeoJSON
        var geoJsonString = JsonSerializer.Serialize(featureCollection, serializerOptions);

        // Retorna o GeoJSON como conteúdo adequado
        return Content(geoJsonString, "application/geo+json"); 
    }

    /// <summary>
    /// Busca um local geográfico já cadastrado pelo seu ID.
    /// </summary>
    /// <param name="id"> Identificador do local geográfico </param>
    /// <returns> Retorna o local geografico referente ao ID</returns>
    /// <response code="200">Local encontrado com sucesso e retornado no corpo da resposta.</response>
    /// <response code="404">Local não encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LocalResponseDto>> GetLocalById(int id)
    {
        var local = await _dbContext.Locais.AsNoTracking() // Desabilita o rastreamento de alterações para melhorar a performance
            .FirstOrDefaultAsync(l => l.Id == id); // Busca o local pelo ID no banco de dados

        if (local == null) return NotFound();

        var response = _mapper.Map<LocalResponseDto>(local); // Mapeia o local encontrado para o DTO de resposta

        return Ok(response);
    }

    /// <summary>
    /// Atualiza um local geográfico existente pelo seu ID.
    /// </summary>
    /// <param name="id"> Identificador do local geográfico </param>
    /// <param name="localDto"> Dados do local a ser atualizado</param>
    /// <returns>Retorna o local atualizado</returns>
    /// <response code="200">Local atualizado com sucesso e retornado no corpo da resposta.</response>
    /// <response code="400">Erro ao tentar atualizar local, dados inválidos.</response>
    /// <response code="404">Local não encontrado</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLocal(int id, [FromBody] UpdateLocalDto localDto) 
    {
        // Busca o local pelo ID no banco de dados
        var local = await _dbContext.Locais.FindAsync(id); 

        if (local == null) return NotFound();

        _mapper.Map(localDto, local); // Mapeia os dados do DTO para a entidade Local existente
        await _dbContext.SaveChangesAsync(); // Salva as alterações no banco de dados
        var responseDto = _mapper.Map<LocalResponseDto>(local); // Mapeia o local atualizado para o DTO de resposta
        return Ok(responseDto);
    }


    /// <summary>
    /// Exclui um local geográfico existente pelo seu ID.
    /// </summary>
    /// <param name="id"> Identificador do local geográfico </param>
    /// <returns>  Retorna um 204 No Content sinalizando que a exclusão foi concluida com sucesso</returns>
    /// <response code="204">Local excluído com sucesso.</response>
    /// <response code="404">Local não encontrado</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLocal(int id)
    {
        var local = await _dbContext.Locais.FindAsync(id);

        if (local == null) return NotFound();

        _dbContext.Locais.Remove(local); // Remove o local do contexto do banco de dados
        await _dbContext.SaveChangesAsync(); // Salva as alterações no banco de dados
        return NoContent(); // Retorna 204 No Content indicando que a operação foi bem-sucedida, mas não há conteúdo para retornar
    }
}
