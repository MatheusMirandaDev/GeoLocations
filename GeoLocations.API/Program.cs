using GeoLocations.API.src.DataAccess;
using GeoLocations.API.src.Profiles;
using GeoLocations.API.src.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

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

builder.Services.AddAutoMapper(typeof(LocalProfile));

builder.Services.AddScoped<ILocalService, LocalService>();

// Adiciona os serviços dos controllers (endpoints)
builder.Services.AddControllers();

// Confiura o uso do Swagger/OpenAPI 
builder.Services.AddEndpointsApiExplorer(); // Permite que o Swagger descubra os endpoints

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "API de Gerenciador de Locais Geográficos",
            Description = """
                    API REST desenvolvida em .NET 8 (C#) utilizando PostgreSQL com PostGIS via Docker para gerenciar locais geográficos em uma cidade.

                    A API permite:

                    - Criar um local informando nome, categoria e coordenadas (latitude/longitude).
                    - Listar todos os locais cadastrados, além de existir um endpoint para retornar esses locais no formato GeoJSON.
                    - Buscar um local específico pelo seu ID.
                    - Atualizar os dados de um local já cadastrado.
                    - Excluir um local específico pelo seu ID.

                    As cordenadas são armazenadas no formato Point (SRID 4326) utilizando o NetTopologySuite, que é compatível com o PostgreSQL/PostGIS.
                    """,
            Contact = new OpenApiContact
            {
                Name = "Matheus Miranda Batista",
                Email = "matheusmiranda.batista@gmail.com",
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT"),
            },

        }
    );

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Verifica se o banco de dados foi criado e aplica as migrations pendentes
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GeoLocationsContext>();
    dbContext.Database.Migrate();
}

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
