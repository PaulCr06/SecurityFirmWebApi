namespace SecurityApi.Contracts;

/// <summary>Represents a security guard returned by the API.</summary>
public sealed record AgentResponse(
    int Id,
    string Nume,
    string Telefon,
    decimal Salariu);
