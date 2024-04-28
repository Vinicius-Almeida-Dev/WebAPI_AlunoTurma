using AlunoTurma.Application;
using AlunoTurma.Application.Interfaces;
using AlunoTurma.Domain.Services;
using AlunoTurma.Infrastructure.Context;
using AlunoTurma.Infrastructure.Services;
using APICatalogo.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => 
                                                    options.JsonSerializerOptions.
                                                        ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? sqlServerConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>  options.UseSqlServer(sqlServerConnection, builder => builder.MigrationsAssembly("APIAlunoTurma")));

builder.Services.AddScoped<IAlunosApplication, AlunosApplication>();
builder.Services.AddScoped<IAlunosServices, AlunosServices>();

var app = builder.Build();

// Configure the HTTP request pipeline -- Por enquanto apenas ambiente de Desenv -- Para produção podemos criar métodos de tratamentos de erros diferentes.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Adicionado Middleware /  Extension Method >> Sendo implementado no controller "******AlunosTurmas********".
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
