
using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.DataAccess.Repositories;
using SistemaReservaHotel.Business.Interfaces;
using SistemaReservaHotel.Business.Services;
using SistemaReservaHotel.UI.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE SERVICIOS DE BLAZOR (.NET 8)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. CONFIGURACIÓN DE LA BASE DE DATOS
// Usamos "AppDbContext" que es el nombre que unificamos en el paso anterior
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. REGISTRO DE REPOSITORIOS (Capa de Datos)
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<ReservaRepository>();
builder.Services.AddScoped<HabitacionRepository>();
// Si tienes repositorios para Comodidades o Tipos, agrégalos aquí de la misma forma

// 4. REGISTRO DE SERVICIOS DE NEGOCIO (Capa Business)
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<IDisponibilidadService, DisponibilidadService>();
builder.Services.AddScoped<IHabitacionService, HabitacionService>();
builder.Services.AddScoped<IReservaService, ReservaService>();

// 5. CONFIGURACIÓN DE HTTP CLIENT (Opcional, pero útil si consumes APIs externas)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["BaseAddress"] ?? "https://localhost:7000")
});

var app = builder.Build();

// 6. CONFIGURACIÓN DEL PIPELINE DE HTTP (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Manejo de errores 404
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// 7. RENDERIZADO DE COMPONENTES
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();