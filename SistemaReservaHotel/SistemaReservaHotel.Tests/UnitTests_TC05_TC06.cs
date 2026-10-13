using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SistemaReservaHotel.Business.DTOs;
using SistemaReservaHotel.Business.Services;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.DataAccess.Repositories;
using SistemaReservaHotel.DataAccess.Seed;
using SistemaReservaHotel.DataAccess.UnitOfWork;
using SistemaReservaHotel.Entities;
using SistemaReservaHotel.Web.Controllers;
using SistemaReservaHotel.WebMVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SistemaReservaHotel.Tests
{
    public class TestDatabaseFixture : IAsyncLifetime
    {
        private static string? _resolvedConnectionString;

        public string ConnectionString
        {
            get
            {
                if (_resolvedConnectionString != null) return _resolvedConnectionString;

                var candidates = new[]
                {
                    "Server=localhost;Database=SistemaReservaHotel_QA;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
                    "Server=localhost\\SQLEXPRESS;Database=SistemaReservaHotel_QA;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
                    "Server=.\\SQLEXPRESS;Database=SistemaReservaHotel_QA;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
                    "Server=(localdb)\\mssqllocaldb;Database=SistemaReservaHotel_QA;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
                };

                foreach (var candidate in candidates)
                {
                    try
                    {
                        var masterConn = candidate.Replace("Database=SistemaReservaHotel_QA", "Database=master");
                        using var conn = new Microsoft.Data.SqlClient.SqlConnection(masterConn);
                        conn.Open();
                        _resolvedConnectionString = candidate;
                        return _resolvedConnectionString;
                    }
                    catch
                    {
                        // Intentar siguiente candidato de conexión
                    }
                }

                _resolvedConnectionString = candidates[0];
                return _resolvedConnectionString;
            }
        }

        public AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(ConnectionString, b => b.MigrationsAssembly("SistemaReservaHotel.DataAccess"))
                .Options;

            return new AppDbContext(options);
        }

        public async Task InitializeAsync()
        {
            using var context = CreateContext();
            // Asegurar que la base de datos SistemaReservaHotel_QA esté limpia y migrada
            await context.Database.EnsureDeletedAsync();

            // Aplicar migraciones hasta AddSecurityEntities (que define el esquema completo del sistema)
            var migrator = context.Database.GetService<IMigrator>();
            await migrator.MigrateAsync("20260313215551_AddSecurityEntities");

            // Registrar en __EFMigrationsHistory la migración Inicial para sincronizar EF Core
            await context.Database.ExecuteSqlRawAsync(
                "INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES ('20260314022016_Inicial', '10.0.3')");

            await DbInitializer.SeedAsync(context);

            // Sembrar datos sintéticos de prueba para Habitaciones y Temporadas
            if (!context.TipoHabitaciones.Any())
            {
                var tipoSuite = new TipoHabitacion
                {
                    Nombre = "Suite Deluxe",
                    Descripcion = "Habitación suite de lujo"
                };
                context.TipoHabitaciones.Add(tipoSuite);
                await context.SaveChangesAsync();

                var habitacion1 = new Habitacion
                {
                    Numero = 101,
                    Tipo = "Suite Deluxe",
                    Descripcion = "Suite de prueba",
                    PrecioBase = 100.00m,
                    Capacidad = 2,
                    Tamaño = 35,
                    Activa = true,
                    Estado = "Disponible"
                };
                context.Habitaciones.Add(habitacion1);

                var temporadaPrecio = new TemporadaPrecio
                {
                    Precio = 120.00m,
                    TipoHabitacionId = tipoSuite.TipoHabitacionId,
                    Temporada = new Temporada
                    {
                        Nombre = "Temporada Alta"
                    }
                };
                context.TemporadasPrecios.Add(temporadaPrecio);
                await context.SaveChangesAsync();
            }

            // Sembrar usuario estándar para pruebas de permisos (TC-08)
            var usuarioRol = await context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Usuario");
            if (usuarioRol != null && !context.Usuario.Any(u => u.Email == "juan.perez@testqa.com"))
            {
                using var sha256 = SHA256.Create();
                var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes("UserPassword123!")));

                var normalUser = new Usuario
                {
                    NombreCompleto = "Juan Perez QA",
                    Email = "juan.perez@testqa.com",
                    PasswordHash = hash,
                    Telefono = "7000-0000",
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };
                context.Usuario.Add(normalUser);
                await context.SaveChangesAsync();

                context.UsuariosRoles.Add(new UsuarioRol
                {
                    UsuarioId = normalUser.Id,
                    RolId = usuarioRol.Id,
                    FechaAsignacion = DateTime.Now
                });
                await context.SaveChangesAsync();
            }

            // Sembrar reserva base para pruebas de consulta y anti-colisión
            if (!context.Reservas.Any(r => r.NombreCliente == "Cliente Existente QA"))
            {
                context.Reservas.Add(new Reserva
                {
                    HabitacionId = 1,
                    NombreCliente = "Cliente Existente QA",
                    FechaEntrada = new DateTime(2026, 11, 10),
                    FechaSalida = new DateTime(2026, 11, 15),
                    PrecioTotal = 500.00m,
                    Estado = "Activa"
                });
                await context.SaveChangesAsync();
            }
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }

    public class UnitTests_TC01_TC08 : IClassFixture<TestDatabaseFixture>
    {
        private readonly TestDatabaseFixture _fixture;

        public UnitTests_TC01_TC08(TestDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task TC_01_Autenticacion_LoginAsync_AdministradorAutorizado()
        {
            using var context = _fixture.CreateContext();
            var uow = new UnitOfWork(context);
            var authService = new AuthService(uow);

            // 1. Login exitoso con credenciales de administrador
            var loginDto = new LoginDto
            {
                Email = "admin@facturacion.com",
                Password = "Admin123!"
            };
            var (success, message, usuario) = await authService.LoginAsync(loginDto);

            Assert.True(success, "El inicio de sesión debió ser exitoso.");
            Assert.NotNull(usuario);
            Assert.Equal("admin@facturacion.com", usuario.Email);
            Assert.Contains("Administrador", usuario.Roles);

            // 2. Intento fallido con contraseña incorrecta
            var (failSuccess, failMessage, failUser) = await authService.LoginAsync(new LoginDto
            {
                Email = "admin@facturacion.com",
                Password = "PasswordInvalida999!"
            });

            Assert.False(failSuccess, "El login con credenciales inválidas debe fallar.");
            Assert.Null(failUser);
        }

        [Fact]
        public async Task TC_02_ConsultaReservas_ObtenerPorDiaAsync_FechaEspecifica()
        {
            using var context = _fixture.CreateContext();
            var repo = new ReservaRepository(context);
            var reservaService = new ReservaService(repo);

            // 1. Consulta para fecha específica donde existe reserva activa (2026-11-10)
            var reservas = await reservaService.ObtenerPorDiaAsync(new DateTime(2026, 11, 10));

            Assert.NotNull(reservas);
            Assert.NotEmpty(reservas);
            Assert.Contains(reservas, r => r.NombreCliente == "Cliente Existente QA");

            // 2. Consulta para fecha sin reservas (2026-10-01)
            var reservasVacias = await reservaService.ObtenerPorDiaAsync(new DateTime(2026, 10, 1));
            Assert.Empty(reservasVacias);
        }

        [Fact]
        public async Task TC_03_CreacionYPersistenciaReserva_ReservarAsync()
        {
            using var context = _fixture.CreateContext();
            var repo = new ReservaRepository(context);
            var reservaService = new ReservaService(repo);

            var nuevaReserva = new Reserva
            {
                HabitacionId = 1,
                NombreCliente = "Huesped_Unit_Creacion_TC03",
                FechaEntrada = new DateTime(2026, 12, 1),
                FechaSalida = new DateTime(2026, 12, 4),
                PrecioTotal = 300.00m,
                Estado = "Activa"
            };

            var creada = await reservaService.ReservarAsync(nuevaReserva);

            Assert.NotNull(creada);
            Assert.True(creada.Id > 0, "La reserva creada debe tener un Id asignado por la base de datos.");

            var persistida = await context.Reservas.FindAsync(creada.Id);
            Assert.NotNull(persistida);
            Assert.Equal("Huesped_Unit_Creacion_TC03", persistida.NombreCliente);
            Assert.Equal(new DateTime(2026, 12, 1), persistida.FechaEntrada);
            Assert.Equal(new DateTime(2026, 12, 4), persistida.FechaSalida);
        }

        [Fact]
        public async Task TC_04_ReporteRangoFechas_InclusionesYExclusiones()
        {
            using var context = _fixture.CreateContext();

            // Asegurar que exista una reserva en noviembre y otra en diciembre
            if (!context.Reservas.Any(r => r.NombreCliente == "Huesped_Diciembre_TC04"))
            {
                context.Reservas.Add(new Reserva
                {
                    HabitacionId = 1,
                    NombreCliente = "Huesped_Diciembre_TC04",
                    FechaEntrada = new DateTime(2026, 12, 10),
                    FechaSalida = new DateTime(2026, 12, 15),
                    PrecioTotal = 600.00m,
                    Estado = "Activa"
                });
                await context.SaveChangesAsync();
            }

            var inicio = new DateTime(2026, 11, 1);
            var fin = new DateTime(2026, 11, 30);

            // Ejecución del filtro tal como lo hace ReportesController.Reservas(inicio, fin)
            var query = context.Reservas
                .Where(r => r.FechaEntrada >= inicio && r.FechaSalida <= fin)
                .ToList();

            // Inclusión: reserva de noviembre presente
            Assert.Contains(query, r => r.NombreCliente == "Cliente Existente QA");

            // Exclusión: reserva de diciembre excluida
            Assert.DoesNotContain(query, r => r.NombreCliente == "Huesped_Diciembre_TC04");
        }

        [Fact]
        public async Task TC_05_ValidarAnticolision_DisponibilidadService_Y_ReservaService()
        {
            using var context = _fixture.CreateContext();
            var repo = new ReservaRepository(context);
            var reservaService = new ReservaService(repo);

            // Asegurar aislamiento: solo la reserva base del 10 al 15 en Habitación 1
            var extras = context.Reservas.Where(r => r.HabitacionId == 1 && r.NombreCliente != "Cliente Existente QA").ToList();
            if (extras.Any())
            {
                context.Reservas.RemoveRange(extras);
                await context.SaveChangesAsync();
            }

            // Caso 1: Solapamiento frontal (entrada 8 a salida 12 con reserva del 10 al 15) -> false
            bool dispFrontal = await reservaService.ValidarDisponibilidadAsync(1, new DateTime(2026, 11, 8), new DateTime(2026, 11, 12));
            Assert.False(dispFrontal, "El solapamiento frontal debió retornar false.");

            // Caso 2: Solapamiento trasero (entrada 13 a salida 17) -> false
            bool dispTrasero = await reservaService.ValidarDisponibilidadAsync(1, new DateTime(2026, 11, 13), new DateTime(2026, 11, 17));
            Assert.False(dispTrasero, "El solapamiento trasero debió retornar false.");

            // Caso 3: Solapamiento interno (entrada 11 a salida 14) -> false
            bool dispInterno = await reservaService.ValidarDisponibilidadAsync(1, new DateTime(2026, 11, 11), new DateTime(2026, 11, 14));
            Assert.False(dispInterno, "El solapamiento interno debió retornar false.");

            // Caso 4: Rango libre (entrada 25 a salida 28) -> true
            bool dispLibre = await reservaService.ValidarDisponibilidadAsync(1, new DateTime(2026, 11, 25), new DateTime(2026, 11, 28));
            Assert.True(dispLibre, "El rango libre debió retornar true.");
        }

        [Fact]
        public async Task TC_06_ValidarCalculoDinamicoTarifaTemporadaAlta_3Noches_X_120()
        {
            using var context = _fixture.CreateContext();
            var controller = new ReservasAdminController(context);

            var tipoSuite = await context.TipoHabitaciones.FirstOrDefaultAsync();
            Assert.NotNull(tipoSuite);

            // Reserva solicitada para 3 noches (del 2026-11-20 al 2026-11-23)
            var nuevaReserva = new Reserva
            {
                HabitacionId = 1,
                NombreCliente = "Huésped Temporada Alta QA",
                FechaEntrada = new DateTime(2026, 11, 20),
                FechaSalida = new DateTime(2026, 11, 23),
                Estado = "Activa"
            };

            // Ejecución del controlador real en WebMVC
            var resultado = controller.Crear(nuevaReserva, tipoSuite.TipoHabitacionId);

            // Consultar la reserva persistida en la BD QA
            var reservaGuardada = await context.Reservas
                .FirstOrDefaultAsync(r => r.NombreCliente == "Huésped Temporada Alta QA");

            Assert.NotNull(reservaGuardada);
            Assert.Equal(3, reservaGuardada.Noches);

            // Según el criterio de aceptación del caso TC-06 de Azure (#1051):
            // "Validar cálculo dinámico de tarifa por temporada alta (3 noches x $120.00 = $360.00)"
            const decimal tarifaPorNoche = 120.00m;
            decimal totalEsperado = 3 * tarifaPorNoche; // $360.00

            // Comprobación real: Falla debido al defecto en ReservasAdminController (multiplicación por número de noches omitida)
            Assert.Equal(totalEsperado, reservaGuardada.PrecioTotal);
        }

        [Fact]
        public async Task TC_07_ValidarPasswordAsync_VerificacionHashSHA256()
        {
            using var context = _fixture.CreateContext();
            var uow = new UnitOfWork(context);
            var authService = new AuthService(uow);

            // Escenario 1: Credenciales válidas del administrador sembrado
            bool passValida = await authService.ValidarPasswordAsync("admin@facturacion.com", "Admin123!");
            Assert.True(passValida, "La contraseña válida 'Admin123!' debió ser validada como true por ValidarPasswordAsync.");

            // Escenario 2: Contraseña errónea con correo existente
            bool passErronea = await authService.ValidarPasswordAsync("admin@facturacion.com", "ClaveIncorrecta999!");
            Assert.False(passErronea, "Una contraseña inválida debió retornar false.");

            // Escenario 3: Correo inexistente en el sistema
            bool passInexistente = await authService.ValidarPasswordAsync("noexiste@hotel.com", "Admin123!");
            Assert.False(passInexistente, "Un usuario inexistente debió retornar false.");
        }

        [Fact]
        public async Task TC_08_RestriccionAccesoAdministrativo_UsuarioSinPermisos()
        {
            using var context = _fixture.CreateContext();
            var uow = new UnitOfWork(context);
            var authService = new AuthService(uow);

            // 1. Validar autenticación de usuario sin rol Administrador
            var (success, message, usuario) = await authService.LoginAsync(new LoginDto
            {
                Email = "juan.perez@testqa.com",
                Password = "UserPassword123!"
            });

            Assert.True(success, "El usuario 'juan.perez@testqa.com' debe poder autenticarse.");
            Assert.NotNull(usuario);
            Assert.DoesNotContain("Administrador", usuario.Roles);
            Assert.Contains("Usuario", usuario.Roles);

            // 2. Verificar que el controlador administrativo esté protegido con atributo [Authorize(Roles = "Administrador")]
            var authorizeAttr = typeof(AdminController).GetCustomAttribute<AuthorizeAttribute>();
            Assert.NotNull(authorizeAttr);
            Assert.Equal("Administrador", authorizeAttr.Roles);
        }
    }
}
