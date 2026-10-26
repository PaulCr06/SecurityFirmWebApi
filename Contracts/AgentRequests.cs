using System.ComponentModel.DataAnnotations;

namespace SecurityApi.Contracts;

/// <summary>Payload used to create a security guard.</summary>
public sealed record CreateAgentRequest
{
    [Required, StringLength(150, MinimumLength = 2)]
    public required string Nume_Complet { get; init; }

    [Required, Phone, StringLength(30)]
    public required string Telefon { get; init; }

    [Range(typeof(decimal), "0", "99999999.99")]
    public required decimal Salariu { get; init; }
}

/// <summary>Payload used to replace a security guard's details.</summary>
public sealed record UpdateAgentRequest
{
    [Required, StringLength(150, MinimumLength = 2)]
    public required string Nume_Complet { get; init; }

    [Required, Phone, StringLength(30)]
    public required string Telefon { get; init; }

    [Range(typeof(decimal), "0", "99999999.99")]
    public required decimal Salariu { get; init; }
}
