using System.Data;
using Microsoft.Data.SqlClient;
using SecurityApi.Contracts;

namespace SecurityApi.Services;

public sealed class AgentService(string connectionString) : IAgentService
{
    public async Task<IReadOnlyList<AgentResponse>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT ID_Agent, Nume_Complet, Telefon, Salariu
            FROM dbo.Agenti_de_Paza
            ORDER BY ID_Agent;
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var agents = new List<AgentResponse>();
        while (await reader.ReadAsync(cancellationToken))
        {
            agents.Add(ReadAgent(reader));
        }

        return agents;
    }

    public async Task<AgentResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT ID_Agent, Nume_Complet, Telefon, Salariu
            FROM dbo.Agenti_de_Paza
            WHERE ID_Agent = @Id;
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        return await reader.ReadAsync(cancellationToken) ? ReadAgent(reader) : null;
    }

    public async Task<AgentResponse> CreateAsync(
        CreateAgentRequest request,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO dbo.Agenti_de_Paza (Nume_Complet, Telefon, Salariu)
            OUTPUT INSERTED.ID_Agent
            VALUES (@Nume, @Telefon, @Salariu);
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddAgentParameters(command, request.Nume_Complet, request.Telefon, request.Salariu);

        var id = (int)(await command.ExecuteScalarAsync(cancellationToken)
            ?? throw new InvalidOperationException("The inserted agent ID was not returned."));

        return new AgentResponse(id, request.Nume_Complet, request.Telefon, request.Salariu);
    }

    public async Task<AgentResponse?> UpdateAsync(
        int id,
        UpdateAgentRequest request,
        CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE dbo.Agenti_de_Paza
            SET Nume_Complet = @Nume, Telefon = @Telefon, Salariu = @Salariu
            WHERE ID_Agent = @Id;
            """;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        AddAgentParameters(command, request.Nume_Complet, request.Telefon, request.Salariu);

        var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
        return rowsAffected > 0
            ? new AgentResponse(id, request.Nume_Complet, request.Telefon, request.Salariu)
            : null;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = "DELETE FROM dbo.Agenti_de_Paza WHERE ID_Agent = @Id;";

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    private static void AddAgentParameters(
        SqlCommand command,
        string name,
        string phone,
        decimal salary)
    {
        command.Parameters.Add("@Nume", SqlDbType.NVarChar, 150).Value = name;
        command.Parameters.Add("@Telefon", SqlDbType.NVarChar, 30).Value = phone;
        command.Parameters.Add(new SqlParameter("@Salariu", SqlDbType.Decimal)
        {
            Precision = 10,
            Scale = 2,
            Value = salary
        });
    }

    private static AgentResponse ReadAgent(SqlDataReader reader) =>
        new(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetDecimal(3));
}
