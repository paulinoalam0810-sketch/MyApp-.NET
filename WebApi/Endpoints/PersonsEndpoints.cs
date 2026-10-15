using Application.DTOs.Persons;
using Application.UseCases.Persons;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Endpoints
{
    public static class PersonsEndpoints
    {
        public static void MapPersonsEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/persons")
                .WithTags("Persons");

            group.MapGet("/{id:guid}", async (Guid id, GetPersonByIdUseCase useCase) =>
            {
                try
                { 
                    var person = await useCase.ExecuteAsync(id);
                    return Results.Ok(person);
                }
                catch(InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            }).WithName("GetPersonById")
            .WithSummary("Get a person by their ID")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPost("/", async (CreatePersonDto dto, CreatePersonUseCase useCase) =>
            {
                try
                {
                    var person = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/persons/{person.Id}", person);
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
            }).WithName("CreatePerson")
            .WithSummary("Create a new person")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/", async (GetAllPersonsUseCase useCase) =>
            {
                try 
                { 
                    var persons = await useCase.ExecuteAsync();
                    return Results.Ok(persons);
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).WithName("GetAllPersons")
            .WithSummary("Get all persons")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:guid}", async (Guid id, UpdatePersonDto dto, UpdatePersonUseCase useCase) =>
            {

                if (id != dto.Id)
                {
                    return Results.BadRequest("The ID in the path does not match the ID in the request body.");
                }

                try
                {
                    var person = await useCase.ExecuteAsync(id, dto);
                    return Results.Ok(person);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).WithName("UpdatePerson")
            .WithSummary("Update an existing person")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id:guid}", async (Guid id, DeletePersonUseCase useCase) =>
            {
                try
                {
                    await useCase.ExecuteAsync(id);
                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError( new { error = ex.Message });
                }
            }).WithName("DeletePerson")
            .WithSummary("Delete an existing person")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/code/{code}", async (string code, GetPersonByCodeUseCase useCase) =>
            {
                try
                {
                    var person = await useCase.ExecuteAsync(code);
                    return Results.Ok(person);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }
            }).WithName("GetPersonByCode")
            .WithSummary("Get a person by their code")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        }
    }
}
