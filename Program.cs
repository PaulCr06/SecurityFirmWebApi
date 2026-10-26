using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Înregistrează serviciile necesare pentru explorarea și documentarea endpoint-urilor API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Activează documentația Swagger doar în mediul de dezvoltare, pentru a nu o expune în producție.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Citește din configurație șirul folosit pentru conectarea la baza de date.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// GET: Returnează toți agenții de pază împreună cu datele selectate din baza de date.
app.MapGet("/agenti", () =>
{
    // Pregătește lista care va conține rezultatele transformate în obiecte JSON.
    var agenti = new List<object>();
    // Deschide conexiunea în mod controlat; aceasta se închide automat la ieșirea din bloc.
    using var connection = new SqlConnection(connectionString);
    connection.Open();
    // Selectează doar coloanele necesare din tabelul agenților.
    var command = new SqlCommand("SELECT ID_Agent, Nume_Complet, Telefon, Salariu FROM Agenti_de_Paza", connection);
    // Execută interogarea și permite parcurgerea succesivă a rândurilor returnate.
    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        // Mapează coloanele fiecărui rând într-un obiect pe care ASP.NET îl serializează ca JSON.
        agenti.Add(new {
            Id = reader.GetInt32(0),
            Nume = reader.GetString(1),
            Telefon = reader.GetString(2),
            Salariu = reader.GetDecimal(3)
        });
    }
    return agenti;
});

// POST: Adaugă un agent nou folosind datele primite în corpul cererii.
app.MapPost("/agenti", (AgentCreateDto agentNou) =>
{
    // Deschide o conexiune nouă la baza de date pentru această operație.
    using var connection = new SqlConnection(connectionString);
    connection.Open();
    
    // Interogarea folosește parametri, astfel încât valorile primite să nu fie interpretate ca instrucțiuni SQL.
    string sql = "INSERT INTO Agenti_de_Paza (Nume_Complet, Telefon, Salariu) VALUES (@Nume, @Telefon, @Salariu)";
    using var command = new SqlCommand(sql, connection);
    // Asociază valorile DTO-ului parametrilor SQL corespunzători.
    command.Parameters.AddWithValue("@Nume", agentNou.Nume_Complet);
    command.Parameters.AddWithValue("@Telefon", agentNou.Telefon);
    command.Parameters.AddWithValue("@Salariu", agentNou.Salariu);
    
    // Execută inserarea și obține numărul de rânduri afectate pentru a stabili rezultatul cererii.
    int rowsAffected = command.ExecuteNonQuery();
    // Răspunde cu succes dacă s-a inserat un rând; în caz contrar, indică eșecul operației.
    return rowsAffected > 0 ? Results.Ok("Agent adăugat cu succes.") : Results.BadRequest("Eroare la adăugare.");
});

// DELETE: Șterge agentul identificat prin ID-ul primit în ruta cererii.
app.MapDelete("/agenti/{id}", (int id) =>
{
    // Deschide o conexiune nouă la baza de date pentru această operație.
    using var connection = new SqlConnection(connectionString);
    connection.Open();
    
    // Șterge doar înregistrarea al cărei ID corespunde parametrului @Id.
    string sql = "DELETE FROM Agenti_de_Paza WHERE ID_Agent = @Id";
    using var command = new SqlCommand(sql, connection);
    // Transmite ID-ul ca parametru, fără a-l concatena în textul interogării SQL.
    command.Parameters.AddWithValue("@Id", id);
    
    // Execută ștergerea și verifică dacă a fost găsită și eliminată o înregistrare.
    int rowsAffected = command.ExecuteNonQuery();
    // Returnează 200 la ștergere reușită sau 404 dacă nu există agentul cu ID-ul indicat.
    return rowsAffected > 0 ? Results.Ok($"Agentul cu ID {id} a fost șters.") : Results.NotFound("Agentul nu a fost găsit.");
});

// PUT: Actualizează datele agentului identificat prin ID-ul primit în rută.
app.MapPut("/agenti/{id}", (int id, AgentUpdateDto agentActualizat) =>
{
    // Deschide o conexiune nouă la baza de date pentru această operație.
    using var connection = new SqlConnection(connectionString);
    connection.Open();
    
    // Actualizează coloanele agentului numai pentru înregistrarea cu ID-ul indicat.
    string sql = "UPDATE Agenti_de_Paza SET Nume_Complet = @Nume, Telefon = @Telefon, Salariu = @Salariu WHERE ID_Agent = @Id";
    using var command = new SqlCommand(sql, connection);
    
    // Asociază ID-ul din rută și valorile din corpul cererii parametrilor interogării SQL.
    command.Parameters.AddWithValue("@Id", id);
    command.Parameters.AddWithValue("@Nume", agentActualizat.Nume_Complet);
    command.Parameters.AddWithValue("@Telefon", agentActualizat.Telefon);
    command.Parameters.AddWithValue("@Salariu", agentActualizat.Salariu);
    
    // Execută actualizarea și verifică dacă a fost modificată o înregistrare.
    int rowsAffected = command.ExecuteNonQuery();
    
    // Returnează succes dacă agentul a fost găsit; altfel, răspunde că ID-ul nu există.
    return rowsAffected > 0 ? Results.Ok($"Agentul cu ID {id} a fost actualizat.") : Results.NotFound("Agentul nu a fost găsit.");
});

app.Run();

// Definește forma datelor JSON acceptate de endpoint-ul POST pentru crearea unui agent.
public record AgentCreateDto(string Nume_Complet, string Telefon, decimal Salariu);

// Definește forma datelor JSON acceptate de endpoint-ul PUT pentru actualizarea unui agent.
public record AgentUpdateDto(string Nume_Complet, string Telefon, decimal Salariu);