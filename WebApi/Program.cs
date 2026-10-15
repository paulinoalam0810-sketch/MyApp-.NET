using Application.UseCases.Persons;
using Application.UseCases.Visits;
using Data;
using System.Text.RegularExpressions;
using WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("You must connect to the database.");

builder.Services.AddData(connectionString);

builder.Services.AddScoped<CreatePersonUseCase>();
builder.Services.AddScoped<GetPersonByIdUseCase>();
builder.Services.AddScoped<GetPersonByCodeUseCase>();
builder.Services.AddScoped<GetAllPersonsUseCase>();
builder.Services.AddScoped<UpdatePersonUseCase>();
builder.Services.AddScoped<DeletePersonUseCase>();

builder.Services.AddScoped<GetActiveVisitsUseCase>();
builder.Services.AddScoped<GetAllVisitsUseCase>();
builder.Services.AddScoped<GetVisitsByPersonUseCase>();
builder.Services.AddScoped<RegisterEntryUseCase>();
builder.Services.AddScoped<RegisterExitUseCase>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPersonsEndpoints();
app.MapVisitsEnpoints();

app.Run();
