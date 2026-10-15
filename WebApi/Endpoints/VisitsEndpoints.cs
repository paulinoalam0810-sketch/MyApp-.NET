using Application.UseCases.Visits;
using Application.UseCases.Persons;
using System.Net.WebSockets;
using Application.DTOs.Visits;

namespace WebApi.Endpoints
{
    public static class VisitsEndpoints
    {
        public static void MapVisitsEnpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/visits")
                .WithTags("Visits");

            group.MapGet("/", async (GetAllVisitsUseCase useCase) =>
            {
                try
                {
                    var visits = await useCase.ExecuteAsync();
                    return Results.Ok(visits);
                }
                catch(Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).WithName("GetAllVisits")
                .WithSummary("Get all visits")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError);

            group.MapPost("/entry", async (RegisterEntryDto dto, RegisterEntryUseCase useCase) =>
            {
                try 
                {
                    var visit = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/visits/{visit.Id}", visit);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).WithName("RegisterEntry")
                .WithSummary("Register a new visit entry")
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/active", async (GetActiveVisitsUseCase useCase) =>
            {
                try
                {
                    var visits = await useCase.ExecuteAsync();
                    return Results.Ok(visits);
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).WithName("GetActiveVisits")
                .WithSummary("Get all active visits")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/person/{personId:guid}", async (Guid personId, GetVisitsByPersonUseCase useCase) =>
            {
                try
                {
                    var visits = await useCase.ExecuteAsync(personId);
                    return Results.Ok(visits);
                }
                catch (Exception ex)
                { 
                    return Results.InternalServerError(ex.Message);
                }
            }).WithName("GetVisitsByPerson")
                .WithSummary("Get visits by person ID")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError);

            group.MapPost("/exit", async (RegisterExitDto dto, RegisterExitUseCase useCase) =>
            {
                try
                { 
                    var visit = await useCase.ExecuteAsync(dto);
                    return Results.Ok(visit);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).WithName("RegisterExit")
                .WithSummary("Register a visit exit")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
