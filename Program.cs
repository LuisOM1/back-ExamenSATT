using ExamenSATT.Data;
using ExamenSATT.Data.Repositories;
using ExamenSATT.Interfaces;
using ExamenSATT.Mappings;
using ExamenSATT.Services;
using ExamenSATT.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// CONFIG DE SERILOG
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Nivel mínimo a loguear
    .Enrich.FromLogContext()
    .WriteTo.Console()          // Loguear en consola para desarrollo
    .WriteTo.File("Logs/log-.txt",
        rollingInterval: RollingInterval.Day, // Crea un archivo nuevo cada día
        retainedFileCountLimit: 7)            // Solo guarda los últimos 7 días
    .CreateLogger();

builder.Host.UseSerilog();   // Decirle a .NET que use serilog
// Al usar serilog implica poner el resto dentro de 1 try catch

// AGREGAR EL SERVICIO DE CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // El puerto de tu front de Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

try
{
    // Add services to the container.

    Log.Information("Iniciando la Web API...");

    // Configuración de DB con Connection Resiliency (Pro)
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure())); // Reintenta si la DB parpadea

    // Registrar AutoMapper en Program.cs para que la app busque los perfiles de mapeo al iniciar
    builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

    builder.Services.AddFluentValidationAutoValidation(); // Habilita validación automática en Controllers
    builder.Services.AddValidatorsFromAssemblyContaining<EmpleadoValidator>();

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // En los controladores todas las fechas de la API usaran el convertidor
            options.JsonSerializerOptions.Converters.Add(new ExamenSATT.Utils.DateTimeConverter());
        });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    // Inyección de dependencias
    builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
    builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

    var app = builder.Build();
    // Middleware para loguear cada petición HTTP automáticamente
    app.UseSerilogRequestLogging();
    // Usar el Middleware Global de Excepciones
    app.UseMiddleware<ExamenSATT.Middleware.ExceptionMiddleware>();

    // Activar el middleware de CORS
    app.UseCors("AllowAngular");


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

} catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    Log.CloseAndFlush();
}