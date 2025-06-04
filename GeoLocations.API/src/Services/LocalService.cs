using AutoMapper;
using GeoLocations.API.src.DataAccess;
using GeoLocations.API.src.DTOs;
using GeoLocations.API.src.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Features; 
using NetTopologySuite.IO.Converters; 
using System.Text.Json;

namespace GeoLocations.API.src.Services;

/// <summary>
/// Camada de serviço responsável por gerenciar as operações CRUD relacionadas aos locais geográficos.
/// </summary>
public class LocalService : ILocalService
{

    private readonly GeoLocationsContext _dbContext; // Contexto do Banco de Dados para acessar as entidades Local
    private readonly IMapper _mapper; // O AutoMapper para mapeamento entre entidades e DTOs

    /// <summary>
    /// Construtor do serviço LocalService.
    /// </summary>
    /// <param name="dbContext"> Contexto do Banco de Dados</param>
    /// <param name="mapper"> O AutoMapper para mapeamento entre entidades e DTOs. </param>
    public LocalService(GeoLocationsContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<LocalResponseDto> CreateLocal(CreateLocalDto localDto)
    {
        var local = _mapper.Map<Local>(localDto); // Mapeia o DTO para a entidade Local

        try 
        {
            await _dbContext.Locais.AddAsync(local); // Adiciona o local ao contexto do banco de dados
            await _dbContext.SaveChangesAsync(); // Salva as alterações no banco de dados
        }
        catch (DbUpdateException dbEx)
        {
            // Captura exceções de atualização do banco de dados e lança uma exceção personalizada
            throw new Exception("Erro de banco de dados ao criar o local: " + dbEx.Message, dbEx); 
        }
        return _mapper.Map<LocalResponseDto>(local);
    }


    /// <inheritdoc/>
    public async Task<IEnumerable<LocalResponseDto>> GetAllLocais()
    {
        var locais = await _dbContext.Locais.ToListAsync(); 
        return _mapper.Map<List<LocalResponseDto>>(locais);
    }

    /// <inheritdoc/>
    public async Task<string> GetLocaisAsGeoJson()
    {
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
        return JsonSerializer.Serialize(featureCollection, serializerOptions);
    }

    /// <inheritdoc/>
    public async Task<LocalResponseDto?> GetLocalById(int id)
    {
        var local = await _dbContext.Locais.AsNoTracking() // Desabilita o rastreamento de alterações para melhorar a performance
            .FirstOrDefaultAsync(l => l.Id == id); // Busca o local pelo ID no banco de dados

        return local == null ? null : _mapper.Map<LocalResponseDto>(local);
    }

    /// <inheritdoc/>
    public async Task<LocalResponseDto?> UpdateLocal(int id, UpdateLocalDto localDto)
    {
        var local = await _dbContext.Locais.FindAsync(id);
        if (local == null) return null; // Retorna nulo se o local não for encontrado
        
        try 
        {
            _mapper.Map(localDto, local); // Mapeia os dados do DTO para a entidade Local existente
            await _dbContext.SaveChangesAsync(); // Salva as alterações no banco de dados
        }
        catch (DbUpdateException dbEx)
        {
            // Captura exceções de atualização do banco de dados e lança uma exceção personalizada
            throw new Exception("Erro de banco de dados ao atualizar o local: " + dbEx.Message, dbEx);
        }
        // Busca o local pelo ID no banco de dados
        return _mapper.Map<LocalResponseDto>(local); // Mapeia o local atualizado para o DTO de resposta
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteLocal(int id)
    {
        var local = await _dbContext.Locais.FindAsync(id); 

        if (local == null) return false; // Item não encontrado

        _dbContext.Locais.Remove(local); 
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Lançar uma exceção para ser tratada pelo controller ou middleware global
            throw new Exception("Erro ao tentar excluir o local do banco de dados.", ex);
        }
        return true; // Sucesso na exclusão
    }
}
