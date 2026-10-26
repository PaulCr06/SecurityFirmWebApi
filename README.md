# SecurityApi

API REST pentru gestionarea agenților de pază, realizat cu ASP.NET Core Minimal APIs și SQL Server.

## Cerințe

- .NET SDK 10
- SQL Server accesibil din aplicație
- O bază de date numită `FirmaSecuritate`, cu tabelul `Agenti_de_Paza`

Tabelul trebuie să conțină coloanele `ID_Agent`, `Nume_Complet`, `Telefon` și `Salariu`. `ID_Agent` este identificatorul folosit de API.

## Configurare

1. Copiază `appsettings.example.json` în `appsettings.json`.
2. Înlocuiește valoarea `Server=localhost` din `ConnectionStrings:DefaultConnection` cu serverul tău SQL Server.
3. Configurează autentificarea potrivită mediului tău. Exemplul folosește autentificarea Windows (`Trusted_Connection=True`).
4. Creează baza de date și tabelul necesar înainte de a porni API-ul.

Fișierele locale `appsettings.json` și `appsettings.Development.json` nu sunt incluse în Git, pentru a evita publicarea configurațiilor specifice mediului.

## Pornire

Din directorul `SecurityApi`:

```powershell
dotnet restore
dotnet run
```

În profilul local HTTP, API-ul este disponibil la `http://localhost:5185`. În mediul de dezvoltare, documentația Swagger este disponibilă la `/swagger`.

## Endpoint-uri

| Metodă | Rută | Descriere |
| --- | --- | --- |
| `GET` | `/agenti` | Returnează lista agenților. |
| `POST` | `/agenti` | Adaugă un agent. |
| `PUT` | `/agenti/{id}` | Actualizează datele agentului cu ID-ul indicat. |
| `DELETE` | `/agenti/{id}` | Șterge agentul cu ID-ul indicat. |

### Adăugarea unui agent

Trimite o cerere `POST /agenti` cu un corp JSON precum:

```json
{
  "nume_Complet": "Exemplu Nume",
  "telefon": "0712345678",
  "salariu": 4500
}
```

Pentru `PUT /agenti/{id}`, folosește același format JSON.

## Pachete principale

- `Microsoft.Data.SqlClient` pentru accesul la SQL Server
- `Swashbuckle.AspNetCore` pentru documentația Swagger în dezvoltare
