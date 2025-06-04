# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base 
# Define o diretório de trabalho padrão para o contêiner.
USER $APP_UID
# Define o diretório de trabalho padrão para o contêiner.
WORKDIR /app
# Expõe as portas HTTP e HTTPS padrão do Kestrel
EXPOSE 8080
EXPOSE 8081

# Esta fase é usada para compilar o projeto de serviço
# Imagem base para o SDK do .NET(compilação)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build  
# Configuração de compilação padrão
ARG BUILD_CONFIGURATION=Release 
# Define o diretório de trabalho para /src, onde os arquivos do projeto serão copiados
WORKDIR /src 
# Copia o arquivo de projeto do serviço para o diretório de trabalho
COPY ["GeoLocations.API/GeoLocations.API.csproj", "GeoLocations.API/"] 
# Restaura as dependências do projeto de serviço
RUN dotnet restore "./GeoLocations.API/GeoLocations.API.csproj" 
# Copia todos os arquivos do serviço para o diretório de trabalho
COPY . .
# Define o diretório de trabalho para o projeto de serviço
WORKDIR "/src/GeoLocations.API"
# Compila o projeto de serviço com a configuração especificada (padrão é Release)
RUN dotnet build "./GeoLocations.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish 
# Configuração de compilação padrão para a fase de publicação
ARG BUILD_CONFIGURATION=Release
# Define o diretório de trabalho para o projeto de serviço
RUN dotnet publish "./GeoLocations.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
# Define o diretório de trabalho final para a execução da aplicação.
WORKDIR /app
# Define a variável de ambiente para o ASP.NET Core
COPY --from=publish /app/publish .
# Define a variável de ambiente para o ASP.NET Core
ENTRYPOINT ["dotnet", "GeoLocations.API.dll"]