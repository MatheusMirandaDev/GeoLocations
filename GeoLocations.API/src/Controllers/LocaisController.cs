using AutoMapper;
using GeoLocations.API.src.DataAccess;
using GeoLocations.API.src.DTOs;
using GeoLocations.API.src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    /// <returns>Retorma o local criado</returns>
    /// <response code="201">Local criado com sucesso.</response>
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
    /// <response code="200">Lista de locais geográficos retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LocalResponseDto>>> GetAllLocais() 
    {
        var locais = await _dbContext.Locais.ToListAsync(); // Busca todos os locais do banco de dados
        return _mapper.Map<List<LocalResponseDto>>(locais); // Mapeia a lista de locais para DTOs de resposta
    }

    /// <summary>
    /// Busca um local geográfico já cadastrado pelo seu ID.
    /// </summary>
    /// <param name="id"> Identificador do local geográfico </param>
    /// <returns> Retorna o local geografico referente ao ID</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LocalResponseDto>> GetLocalById(int id) 
    {
        var local = await _dbContext.Locais.FirstOrDefaultAsync(l => l.Id == id); // Busca o local pelo ID no banco de dados
        if (local == null)
        {
            return NotFound();
        }

        var response = _mapper.Map<LocalResponseDto>(local); // Mapeia o local encontrado para o DTO de resposta

        return Ok(response);
    }
}
