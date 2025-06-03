using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite;
using GeoLocations.API.src.DataAccess;


var builder = WebApplication.CreateBuilder(args);

// Obt�m a string de conex�o do banco de dados a partir das configura��es (appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra o GeoLocationsContext (seu DbContext) para inje��o de depend�ncia.
// Configura-o para usar PostgreSQL (Npgsql) com a string de conex�o especificada
// e habilita o suporte a tipos de dados geoespaciais do NetTopologySuite.
builder.Services.AddDbContext<GeoLocationsContext>(options =>
    options.UseNpgsql(connectionString,
    x => x.UseNetTopologySuite())
);

// Adiciona os servi�os dos controllers (endpoints)
builder.Services.AddControllers();

// Confiura o uso do Swagger/OpenAPI 
builder.Services.AddEndpointsApiExplorer(); // Permite que o Swagger descubra os endpoints
builder.Services.AddSwaggerGen(); // Gera a documenta��o Swagger

var app = builder.Build();

// Configura o middleware do Swagger para uso apenas em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita o middleware para servir o documento Swagger JSON
    app.UseSwaggerUI(); // Habilita o middleware para servir a interface de usu�rio do Swagger (UI)
}

// Redireciona requisi��es HTTP para HTTPS para seguran�a
app.UseHttpsRedirection();

// Adiciona o middleware de autoriza��o (se houver regras de seguran�a)
app.UseAuthorization();

// Mapeia os controladores da API para as rotas HTTP
app.MapControllers();

// Inicia a execu��o da aplica��o
app.Run();
