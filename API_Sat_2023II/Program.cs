using API_Sat_2023II.DAL.Entities;
using API_Sat_2023II.Domain.Interfaces;
using API_Sat_2023II.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Esta l�nea me crea el contexto de la BD a la hora de correr esta API.
//Funciones an�nimas (x => x...) Arrow Functions - Lambda Functions (an�nimas porque no se deben crear para llamarlas)

builder.Services.AddDbContext<DataBaseContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
/*'o' de option, se le est� indicando que se usar� SQL Server*/


builder.Services.AddScoped<ICountryService, CountryService>();
//Por cada nuevo servicio/interfaz que yo creo en mi API, debo agregar aqu� esa nueva dependencia

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
