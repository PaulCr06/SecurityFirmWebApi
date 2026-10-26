# SecurityFirmWebApi

ASP.NET Core Minimal API for managing security guards, backed by SQL Server.

## Requirements

- .NET 10 SDK
- SQL Server
- `sqlcmd` (to run the database setup script)

## Setup and run

Run these commands from the repository root:

```powershell
sqlcmd -S localhost -E -i .\schema.sql
Copy-Item .\appsettings.example.json .\appsettings.json
```

Update `ConnectionStrings:DefaultConnection` in `appsettings.json` for your SQL Server, then start the API:

```powershell
dotnet restore .\SecurityApi.csproj
dotnet run --project .\SecurityApi.csproj
```

The HTTP launch profile uses `http://localhost:5185`. In Development, Swagger UI is available at `http://localhost:5185/swagger`.

## Endpoints

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/agenti` | List all agents. |
| `GET` | `/agenti/{id}` | Get one agent. |
| `POST` | `/agenti` | Create an agent (`201 Created`, with a `Location` header). |
| `PUT` | `/agenti/{id}` | Replace an agent's details. |
| `DELETE` | `/agenti/{id}` | Delete an agent (`204 No Content`). |

Create and update requests use JSON properties `nume_Complet`, `telefon`, and `salariu`. Input is validated; errors use Problem Details. See [SecurityApi.http](./SecurityApi.http) for sample requests.

## Română

API REST pentru gestionarea agenților de pază, realizat cu ASP.NET Core Minimal APIs și SQL Server.

### Configurare și pornire

Rulează comenzile din rădăcina depozitului:

```powershell
sqlcmd -S localhost -E -i .\schema.sql
Copy-Item .\appsettings.example.json .\appsettings.json
```

Configurează `ConnectionStrings:DefaultConnection` în `appsettings.json` pentru serverul tău SQL Server, apoi pornește aplicația:

```powershell
dotnet restore .\SecurityApi.csproj
dotnet run --project .\SecurityApi.csproj
```

Profilul HTTP local folosește `http://localhost:5185`. În mediul Development, Swagger este disponibil la `/swagger`.

### Endpoint-uri

| Metodă | Rută | Descriere |
| --- | --- | --- |
| `GET` | `/agenti` | Returnează lista agenților. |
| `GET` | `/agenti/{id}` | Returnează agentul cu ID-ul indicat. |
| `POST` | `/agenti` | Adaugă un agent și returnează `201 Created`. |
| `PUT` | `/agenti/{id}` | Actualizează datele agentului. |
| `DELETE` | `/agenti/{id}` | Șterge agentul și returnează `204 No Content`. |

Corpul cererilor POST și PUT folosește câmpurile JSON `nume_Complet`, `telefon` și `salariu`. Datele sunt validate, iar erorile sunt returnate în format Problem Details. Exemplele de cereri sunt în [SecurityApi.http](./SecurityApi.http).
