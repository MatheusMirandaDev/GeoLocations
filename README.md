# 🗺️ GeoLocations API

API REST desenvolvida em .NET 8 (C#) com PostgreSQL e extensão PostGIS, containerizada com Docker. Permite o gerenciamento completo de locais geográficos com suporte a coordenadas e categorias.

---

## 📖 Projeto
API para gerenciamento de dados geográficos, utilizando .NET 8, Entity Framework Core e PostgreSQL com PostGIS. Desenvolvida para ser escalável, com consultas espaciais eficientes. O ambiente roda via Docker para fácil deploy e desenvolvimento.

---

## 📌 Tecnologias Utilizadas
- **Backend:** .NET 8 (C#)
- **Banco de Dados:** PostgreSQL com extensão PostGIS
- **ORM:** Entity Framework Core
- **Geodados:** NetTopologySuite
- **Mapeamento de Objetos:** AutoMapper
- **Documentação da API:** Swagger (OpenAPI)
- **Containerização:** Docker + Docker Compose
- **Registro de Imagem Docker:** [Meu Docker Hub](https://hub.docker.com/r/matheusmirandadev/geolocations-api)

---

## 🔗 Funcionalidades

A API permite:
- Criar um local informando nome, categoria e coordenadas (latitude/longitude).
- Listar todos os locais cadastrados.
- Listar todos os locais cadastrados no formato GeoJSON.
- Buscar um local específico pelo seu ID.
- Atualizar os dados de um local já cadastrado.
- Excluir um local específico pelo seu ID.

As coordenadas são armazenadas no formato `Point` (SRID 4326) utilizando NetTopologySuite, compatível com PostgreSQL/PostGIS.

---

## 💻 Documentação da API
Todos os endpoints estão sob o prefixo `/api/Locais`. Para detalhes completos sobre os requests e responses, consulte a documentação Swagger em `/swagger` quando a API estiver em execução.

### 🔹 Endpoints

- **`POST /api/Locais`** → Permite a criação de um local com nome, categoria e coordenadas geográficas.
    - Corpo da requisição: `CreateLocalDto` (JSON)
        ```json
        {
          "nome": "Nome do Local", // Ex: "Giraffas"
          "categoria": 1, // Ex: Restaurante (veja todas opções em: enum CategoriaLocal)
          "latitude": -28.750290, // Intervalo: (-90.00 até 90.00)
          "longitude": -167.623308 // Intervalo: (-180.00 até 180.00)
        }
        ```

* **`GET /api/Locais`** → Lista todos os locais cadastrados.
* **`GET /api/Locais/geojson`** → Lista todos os locais cadastrados no formato GeoJSON.
* **`GET /api/Locais/{id}`** → Busca um local pelo ID.
* **`PUT /api/Locais/{id}`** → Atualiza um local existente.
    * Corpo da Requisição → `UpdateLocalDto` (Simular ao CreateLocalDto)
* **`DELETE /api/Locais/{id}`** → Exclui um local.

### Categorias de Local (`CategoriaLocal` Enum)

* `0` = Farmácia
* `1` = Restaurante
* `2` = Hospital
* `3` = Supermercado
* `4` = Posto de Combustível
* `5` = Escola
* `6` = Parque
* `7` = Shopping
* `8` = Outro

---

## 🧪 Testes

Para garantir a qualidade e o correto funcionamento da API, foram realizados os seguintes testes:

* **Testes Manuais da API:** Foram executados testes manuais em todos os endpoints da API utilizando o **Swagger UI** (para exploração e validação rápida) e o **Postman** (para cenários mais complexos e validação de requisições e respostas).
* **Validação de Persistência de Dados:** Após as operações de criação e atualização via API, a persistência e integridade dos dados no banco de dados **PostgreSQL** foram verificadas diretamente através do **PgAdmin**. Isso assegura que os dados estão sendo salvos corretamente e no formato esperado.

---

## 🚀 Como executar

### ⚙️ Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (ou Docker Engine e Docker Compose CLI)
- Um cliente Git para clonar o repositório (ex: [Git SCM](https://git-scm.com/downloads))

### 🐳 Execução com Docker Compose (Recomendado)

Esta é a forma mais simples de executar o projeto. O `docker-compose.yml` configura tanto a API quanto o banco de dados com PostGIS, utilizando a imagem publicada no Docker Hub:  
 [matheusmirandadev/geolocations-api](https://hub.docker.com/r/matheusmirandadev/geolocations-api)

 **Importante:** Antes de executar os comandos abaixo, **garanta que o Docker Desktop esteja aberto e completamente inicializado** em seu computador.

#### 1. Clone o repositório:

**Opção A:** Clonar o repositório (com o código fonte completo):
```bash
  git clone https://github.com/matheusmirandadev/geolocations.git
  cd geolocations
```

**Opção B:** Baixar apenas o `docker-compose.yml`:
Você pode baixar apenas este arquivo e colocá-lo em qualquer pasta.

#### 2. Execute o Docker Compose:
```bash
  docker-compose up --build -d
```

#### 3. Acesse a API:
- `http://localhost:8080` (API)
- `http://localhost:8080/swagger` (Swagger UI)

As migrations do EF Core são aplicadas automaticamente ao iniciar.

---

### 🛠️ Execução Local (API localmente e banco via Docker)

Se quiser rodar a API com o .NET SDK e utilizar o banco de dados via Docker:

1.  **Clone o repositório (se ainda não o fez):**
```bash
  git clone https://github.com/matheusmirandadev/geolocations.git
  cd geolocations
```
2.  **Inicie o serviço do banco de dados com Docker Compose:**
  Na raiz do projeto (`cd geolocations`):
  ```bash
    docker-compose up -d db
  ```
  Isso garantirá que o PostgreSQL com PostGIS esteja rodando na porta `5432`.

3.  **Execute o projeto da API:**
  Ainda na raiz do projeto (`geolocations`):
  ```bash
    dotnet run --project GeoLocations.API --launch-profile https
  ```
Isso garante que as configurações do perfil https serão usadas

4.  **Acesse a API:**
  * A API estará disponível em `http://localhost:5168` e `https://localhost:7163`.
  * A documentação do Swagger UI estará disponível em `http://localhost:5168/swagger` ou `https://localhost:7163/swagger`.

---
