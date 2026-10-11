using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.Business.Interfaces;
using SistemaReservaHotel.Business.Services;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.DataAccess.Seed;
using SistemaReservaHotel.DataAccess.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// ═══ SERVICIOS MVC ═══════════════════════════════════════════════
builder.Services.AddControllersWithViews();

// ═══ BASE DE DATOS Y MIGRACIONES ═════════════════════════════════
// ═══ BASE DE DATOS Y MIGRACIONES ═════════════════════════════════
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, b =>
        b.MigrationsAssembly("SistemaReservaHotel.DataAccess"))); // <--- SOLUCIÓN AL ERROR DE MIGRACIONES

// ═══ UNIT OF WORK Y REPOSITORIOS ═══════════════════════════════
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ═══ SERVICIOS DE NEGOCIO ═══════════════════════════════════════
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();

// 🔹 REGISTRO DE SERVICIOS NUEVOS
builder.Services.AddScoped<IHabitacionService, HabitacionService>();
// builder.Services.AddScoped<IReservaService, ReservaService>();
// builder.Services.AddScoped<IDisponibilidadService, DisponibilidadService>();

// ═══ AUTENTICACIÓN CON COOKIES ═══════════════════════════════════
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

builder.Services.AddAuthorization();

// ═══ SESIONES ════════════════════════════════════════════════════
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

// ═══ MIDDLEWARE PIPELINE ═════════════════════════════════════════
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ═══ SEED DE DATOS INICIALES ═══════════════════════════════════
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await DbInitializer.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al ejecutar seed de datos");
    }
}
app.Run();