using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;
using System;
using System.Linq;

namespace SistemaReservaHotel.IntegrationTests.Infrastructure
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _databaseName = $"HotelTestDb_{Guid.NewGuid()}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Limpiar cualquier descriptor previo de AppDbContext o DbContextOptions
                var descriptors = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType == typeof(AppDbContext)).ToList();

                foreach (var d in descriptors)
                {
                    services.Remove(d);
                }

                // Registrar AppDbContext usando InMemoryDatabase
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName);
                });
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                SeedIntegrationTestData(db);
            }

            return host;
        }

        private static void SeedIntegrationTestData(AppDbContext db)
        {
            // 1. Habitación activa de prueba (Id = 1)
            if (!db.Habitaciones.Any(h => h.Id == 1))
            {
                db.Habitaciones.Add(new Habitacion
                {
                    Id = 1,
                    Numero = 101,
                    Tipo = "Matrimonial",
                    Descripcion = "Habitación ejecutiva con vista al jardín.",
                    PrecioBase = 75.00m,
                    Capacidad = 2,
                    Tamaño = 25,
                    Activa = true,
                    Estado = "Disponible"
                });
            }

            // 2. Usuario de prueba (Id = 1)
            if (!db.Usuario.Any(u => u.Id == 1 || u.Email == "tester.qa@hotel.com"))
            {
                db.Usuario.Add(new Usuario
                {
                    Id = 1,
                    NombreCompleto = "Tester QA Integración",
                    Email = "tester.qa@hotel.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEJ4f8Z89xK89...",
                    Activo = true,
                    FechaCreacion = DateTime.Today
                });
            }

            // 3. Reserva de prueba (Id = 1)
            if (!db.Reservas.Any(r => r.Id == 1))
            {
                db.Reservas.Add(new Reserva
                {
                    Id = 1,
                    HabitacionId = 1,
                    NombreCliente = "Carlos Miranda",
                    FechaEntrada = DateTime.Today.AddDays(10),
                    FechaSalida = DateTime.Today.AddDays(15),
                    PrecioTotal = 375.00m,
                    Estado = "Activa",
                    UsuarioId = 1
                });
            }

            db.SaveChanges();
        }
    }
}
