using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer;
using pagina_personal.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Sobre la Base de Datos
builder.Services.AddDbContext<PersonalContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

// CORS - Las variables de cors
var misReglasCors = "ReglasCors";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: misReglasCors, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


var app = builder.Build();

// SE USA EN DESARROLLO
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


// SE USA EN DESPLIEGUE
app.UseSwagger();
app.UseSwaggerUI();


app.UseCors(misReglasCors);

// Configurar el servidor para utilizar wwwroot como la carpeta raíz web
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
