

using Library.Api.Middleware;
using Library.Application;
using Library.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


// Registers OpenAPI, which auto-generates a JSON description of all your endpoints (routes, parameters, responses). Swagger and Scalar read this to build a test page.
builder.Services.AddOpenApi();


//Register the layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); //generates an API description
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//MapIdentityApi:generates ready-made endpoints.
//MapGroup("/api/auth"): puts them all under a common prefix, so /register becomes /api/auth/register, matching your other routes.
app.MapGroup("/api/auth").MapIdentityApi<IdentityUser>().WithTags("Auth");

app.Run();
