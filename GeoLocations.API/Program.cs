using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite;
using GeoLocations.API.src.DataAccess;


var builder = WebApplication.CreateBuilder(args);

// Obtém a string de conexão do banco de dados a partir das configurações (appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra o GeoLocationsContext (seu DbContext) para injeção de dependência.
// Configura-o para usar PostgreSQL (Npgsql) com a string de conexão especificada
// e habilita o suporte a tipos de dados geoespaciais do NetTopologySuite.
builder.Services.AddDbContext<GeoLocationsContext>(options =>
    options.UseNpgsql(connectionString,
    x => x.UseNetTopologySuite())
);

// Adiciona os serviços dos controllers (endpoints)
builder.Services.AddControllers();

// Confiura o uso do Swagger/OpenAPI 
builder.Services.AddEndpointsApiExplorer(); // Permite que o Swagger descubra os endpoints
builder.Services.AddSwaggerGen(); // Gera a documentação Swagger

var app = builder.Build();

// Configura o middleware do Swagger para uso apenas em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita o middleware para servir o documento Swagger JSON
    app.UseSwaggerUI(); // Habilita o middleware para servir a interface de usuário do Swagger (UI)
}

// Redireciona requisições HTTP para HTTPS para segurança
app.UseHttpsRedirection();

// Adiciona o middleware de autorização (se houver regras de segurança)
app.UseAuthorization();

// Mapeia os controladores da API para as rotas HTTP
app.MapControllers();

// Inicia a execução da aplicação
app.Run();
