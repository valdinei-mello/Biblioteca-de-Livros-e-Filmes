# Biblioteca de Filmes Favoritos

API RESTful em C# para gerenciamento de filmes favoritos. A aplicação ASP.NET Core usa `ConcurrentDictionary` como armazenamento em memória, sem banco de dados ou ORM externo. Os dados são perdidos quando a aplicação é encerrada ou reiniciada.

## Pré-requisitos

- .NET 9 SDK
- cURL
- Terminal compatível com o .NET CLI

Confira a instalação do SDK:

```powershell
dotnet --version
```

## Estrutura da solução

- `BibliotecaFilmes.Api`: aplicação ASP.NET Core Web API.
- `BibliotecaFilmes.Tests`: testes automatizados com xUnit.
- `BibliotecaFilmes.sln`: solução que reúne os projetos.

## Compilar a solução

Na pasta raiz da solução:

```powershell
dotnet build BibliotecaFilmes.sln
```

## Executar os testes

Rode todos os testes automatizados e confira o resumo de aprovados e falhos:

```powershell
dotnet test BibliotecaFilmes.sln
```

## Iniciar a API

Inicie a aplicação usando o perfil HTTP de desenvolvimento:

```powershell
dotnet run --project BibliotecaFilmes.Api --launch-profile http
```

A API ficará disponível em `http://localhost:5122`. Mantenha esse terminal aberto durante os testes manuais. O Swagger UI, disponível no ambiente de desenvolvimento, pode ser acessado em:

```text
http://localhost:5122/swagger
```

O documento OpenAPI do Swagger está em `http://localhost:5122/swagger/v1/swagger.json`.

## Validar os endpoints com cURL

Os exemplos abaixo usam `curl.exe`, o executável cURL, em vez do alias `curl` de algumas versões do PowerShell. Execute-os em outro terminal enquanto a API estiver em execução.

### Criar um filme válido

```powershell
curl.exe -i -X POST "http://localhost:5122/api/movies" `
  -H "Content-Type: application/json" `
  -d '{"title":"A Chegada","director":"Denis Villeneuve","releaseYear":2016,"genre":"Ficção científica","rating":5}'
```

A resposta `201 Created` contém o `id` do filme criado. Copie esse valor para `$id` para os exemplos seguintes:

```powershell
$id = "COLE-O-ID-RETORNADO-PELA-API"
```

### Tentar criar um filme com Rating inválido

O valor `6` deve ser rejeitado com `400 Bad Request`, pois `Rating` precisa estar entre 1 e 5.

```powershell
curl.exe -i -X POST "http://localhost:5122/api/movies" `
  -H "Content-Type: application/json" `
  -d '{"title":"Filme inválido","director":"Diretor","releaseYear":2020,"genre":"Drama","rating":6}'
```

### Listar filmes, filtrar e paginar

Liste todos os filmes (por padrão, página 1 com até 10 itens):

```powershell
curl.exe -i "http://localhost:5122/api/movies"
```

Os parâmetros `title` e `genre` filtram os resultados; `page` e `pageSize` controlam a paginação (`pageSize` máximo: 100):

```powershell
curl.exe -i "http://localhost:5122/api/movies?title=chegada&genre=Fic%C3%A7%C3%A3o%20cient%C3%ADfica&page=1&pageSize=5"
```

### Buscar um filme por ID

```powershell
curl.exe -i "http://localhost:5122/api/movies/$id"
```

### Atualizar um filme

```powershell
curl.exe -i -X PUT "http://localhost:5122/api/movies/$id" `
  -H "Content-Type: application/json" `
  -d '{"title":"A Chegada (revisão)","director":"Denis Villeneuve","releaseYear":2016,"genre":"Ficção científica","rating":4}'
```

### Remover um filme

```powershell
curl.exe -i -X DELETE "http://localhost:5122/api/movies/$id"
```
