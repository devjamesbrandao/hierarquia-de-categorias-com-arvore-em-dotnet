using System.Text.Json.Serialization;
using Arvore.Data;
using Arvore.Repository;
using Arvore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(c => c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<CategoriaContext>(x => x.UseSqlServer("Server=localhost,1433;Database=Estudos;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True;"));

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
