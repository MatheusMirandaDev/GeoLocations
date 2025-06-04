using GeoLocations.API.src.DTOs;
using GeoLocations.API.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoLocations.API.src.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas aos locais geográficos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LocaisController : ControllerBase
{
    private readonly ILocalService _localService;

    /// <summary>
    /// Construtor do controlador.
    /// </summary>
    /// <param name="localService">Contexto da camada de serviço</param>
    public LocaisController(ILocalService localService)
    {
        _localService = localService;
    }

    /// <summary>
    /// Cria um novo local geográfico.
    /// </summary>
    /// <param name="localDto">Dados do local a ser criado</param>
    /// <returns>Retorna o local criado</returns>
    /// <response code="201">Local criado com sucesso e retornado no corpo da resposta.</response>
    /// <response code="400">Erro ao tentar criar, dados inválidos.</response>
    /// <response code="500">Erro interno inesperado no servidor.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateLocal([FromBody] CreateLocalDto localDto)
    {
        try
        {
            // Chama o serviço para criar o local geográfico
            var responseDto = await _localService.CreateLocal(localDto);

            // Retorna o local criado com o status 201 Created e a localização do novo recurso
            return CreatedAtAction(
                nameof(GetLocalById),
                new { id = responseDto.Id },
                responseDto
            );
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado ao criar o local.");
        }
    }

    /// <summary>
    /// Mostra a lista com todos os locais geográficos cadastrados.
    /// </summary>
    /// <returns>Retorna todos os locais geográficos cadastrados </returns>
    /// <response code="200">Lista a lista com todos os locais geográficos cadastrados.</response>
    /// <response code="500">Erro interno inesperado no servidor.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<LocalResponseDto>>> GetAllLocais()
    {
        try
        {
            // Chama o serviço para obter todos os locais geográficos   
            return Ok(await _localService.GetAllLocais()); 
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado ao listar os locais.");
        }

    }

    /// <summary>
    /// Mostra todos os locais geográficos cadastrados no formato GeoJSON.
    /// </summary>
    /// <returns> Retorna os locais geográficos no formato GeoJson </returns>
    /// /// <response code="200">Lista a lista com todos os locais geográficos cadastrados.</response>
    /// <response code="500">Erro interno inesperado no servidor.</response>
    [HttpGet("geojson")]
    [Produces("application/geo+json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetLocaisAsGeoJson()
    {
        try
        {
            // Chama o serviço para obter os locais geográficos no formato GeoJSON
            return Content(await _localService.GetLocaisAsGeoJson(), "application/geo+json");
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado ao listar os locais.");
        }
    }

    /// <summary>
    /// Busca um local geográfico já cadastrado pelo seu ID.
    /// </summary>
    /// <param name="id"> Identificador do local geográfico </param>
    /// <returns> Retorna o local geografico referente ao ID</returns>
    /// <response code="200">Local encontrado com sucesso e retornado no corpo da resposta.</response>
    /// <response code="404">Local não encontrado</response>
    /// <response code="500">Erro interno inesperado no servidor.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LocalResponseDto>> GetLocalById(int id)
    {
        try
        {
            // Chama o serviço para buscar o local geográfico pelo ID
            return await _localService.GetLocalById(id) switch
            {
                LocalResponseDto local => Ok(local), // Retorna o local encontrado com status 200 OK
                null => NotFound() // Retorna 404 Not Found se o local não for encontrado
            };
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado ao buscar o local.");
        }
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
    /// <response code="500">Erro inesperado ao tentar listar os locais.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateLocal(int id, [FromBody] UpdateLocalDto localDto) 
    {
        try
        {
            var responseDto = await _localService.UpdateLocal(id, localDto);
            if (responseDto == null)
            {
                return NotFound(); // Retorna 404 Not Found se o local não for encontrado
            }
            return Ok(responseDto); // Retorna o local atualizado com status 200 OK
        } 
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado ao atualizar o local.");
        }
    }

    /// <summary>
    /// Exclui um local geográfico existente pelo seu ID.
    /// </summary>
    /// <param name="id"> Identificador do local geográfico </param>
    /// <returns>  Retorna um 204 No Content sinalizando que a exclusão foi concluida com sucesso</returns>
    /// <response code="204">Local excluído com sucesso.</response>
    /// <response code="404">Local não encontrado</response>
    /// <response code="500">Erro interno inesperado no servidor.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteLocal(int id)
    {
        try
        {   
            // Chama o serviço para excluir o local geográfico
            var success = await _localService.DeleteLocal(id);
            if (!success)
            {
                return NotFound(); // Retorna 404 Not Found se o local não for encontrado
            }
            return NoContent(); // Retorna 204 No Content se a exclusão for bem-sucedida
        }
        catch (Exception ex) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado.");
        }
    }
}
