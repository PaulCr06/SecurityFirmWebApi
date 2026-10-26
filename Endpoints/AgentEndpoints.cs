using Microsoft.AspNetCore.Http.HttpResults;
using SecurityApi.Contracts;
using SecurityApi.Services;

namespace SecurityApi.Endpoints;

public static class AgentEndpoints
{
    public static IEndpointRouteBuilder MapAgentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var agents = endpoints.MapGroup("/agenti").WithTags("Agents");

        agents.MapGet("", async Task<Ok<IReadOnlyList<AgentResponse>>> (
            IAgentService service,
            CancellationToken cancellationToken) =>
        {
            var result = await service.GetAllAsync(cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithName("GetAgents")
        .WithSummary("Get all security guards")
        .WithDescription("Returns all security guards ordered by their ID.")
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        agents.MapGet("/{id:int:min(1)}", async Task<Results<Ok<AgentResponse>, NotFound>> (
            int id,
            IAgentService service,
            CancellationToken cancellationToken) =>
        {
            var agent = await service.GetByIdAsync(id, cancellationToken);
            return agent is null ? TypedResults.NotFound() : TypedResults.Ok(agent);
        })
        .WithName("GetAgentById")
        .WithSummary("Get a security guard by ID")
        .WithDescription("Returns the security guard with the specified ID.")
        .Produces<AgentResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        agents.MapPost("", async Task<Created<AgentResponse>> (
            CreateAgentRequest request,
            IAgentService service,
            CancellationToken cancellationToken) =>
        {
            var agent = await service.CreateAsync(request, cancellationToken);
            return TypedResults.Created($"/agenti/{agent.Id}", agent);
        })
        .WithName("CreateAgent")
        .WithSummary("Create a security guard")
        .WithDescription("Creates a security guard and returns its resource URL.")
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        agents.MapPut("/{id:int:min(1)}", async Task<Results<Ok<AgentResponse>, NotFound>> (
            int id,
            UpdateAgentRequest request,
            IAgentService service,
            CancellationToken cancellationToken) =>
        {
            var agent = await service.UpdateAsync(id, request, cancellationToken);
            return agent is null ? TypedResults.NotFound() : TypedResults.Ok(agent);
        })
        .WithName("UpdateAgent")
        .WithSummary("Update a security guard")
        .WithDescription("Replaces the details of the security guard with the specified ID.")
        .Produces<AgentResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        agents.MapDelete("/{id:int:min(1)}", async Task<Results<NoContent, NotFound>> (
            int id,
            IAgentService service,
            CancellationToken cancellationToken) =>
        {
            var deleted = await service.DeleteAsync(id, cancellationToken);
            return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteAgent")
        .WithSummary("Delete a security guard")
        .WithDescription("Deletes the security guard with the specified ID.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return endpoints;
    }
}
