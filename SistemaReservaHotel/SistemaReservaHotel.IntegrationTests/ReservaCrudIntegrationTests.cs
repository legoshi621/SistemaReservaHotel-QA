using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;
using SistemaReservaHotel.IntegrationTests.Infrastructure;
using SistemaReservaHotel.WebMVC.Controllers;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SistemaReservaHotel.IntegrationTests
{
    public class ReservaCrudIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public ReservaCrudIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostCrearReserva_PersisteEntidadEnBaseDeDatosInMemory()
        {
            // Arrange (US-SEC-01 #1043 - Creación y Persistencia)
            var nuevaReserva = new Reserva
            {
                HabitacionId = 1,
                NombreCliente = "Camila Calderon Test",
                FechaEntrada = DateTime.Today.AddDays(30),
                FechaSalida = DateTime.Today.AddDays(35),
                PrecioTotal = 450m,
                Estado = "Activa"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/Reserva/Crear", nuevaReserva);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var reservaCreada = await response.Content.ReadFromJsonAsync<Reserva>();
            reservaCreada.Should().NotBeNull();
            reservaCreada!.Id.Should().BeGreaterThan(0);
            reservaCreada.NombreCliente.Should().Be("Camila Calderon Test");

            // Verificar persistencia directa en el contexto de BD en memoria
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var entidadPersistida = await db.Reservas.FindAsync(reservaCreada.Id);

            entidadPersistida.Should().NotBeNull();
            entidadPersistida!.NombreCliente.Should().Be("Camila Calderon Test");
            entidadPersistida.PrecioTotal.Should().Be(450m);
            entidadPersistida.Estado.Should().Be("Activa");
        }

        [Fact]
        public async Task PostCancelarReserva_ActualizaEstadoEnBaseDeDatosInMemory()
        {
            // Arrange (US-SEC-01 #1043 - Cancelación y Ciclo de Vida)
            // 1. Crear una reserva inicial
            var reservaInicial = new Reserva
            {
                HabitacionId = 1,
                NombreCliente = "Cliente Para Cancelar",
                FechaEntrada = DateTime.Today.AddDays(40),
                FechaSalida = DateTime.Today.AddDays(45),
                PrecioTotal = 350m,
                Estado = "Activa"
            };

            var createResponse = await _client.PostAsJsonAsync("/Reserva/Crear", reservaInicial);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var reservaCreada = await createResponse.Content.ReadFromJsonAsync<Reserva>();
            reservaCreada.Should().NotBeNull();

            // Act: Cancelar la reserva creada
            var cancelarDto = new CancelarReservaDto { Id = reservaCreada!.Id };
            var cancelResponse = await _client.PostAsJsonAsync("/Reserva/Cancelar", cancelarDto);

            // Assert
            cancelResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verificar persistencia y transición de estado a "Cancelada" en la BD en memoria
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var entidadActualizada = await db.Reservas.FindAsync(reservaCreada.Id);

            entidadActualizada.Should().NotBeNull();
            entidadActualizada!.Estado.Should().Be("Cancelada");
        }

        [Fact]
        public async Task PostCancelarReserva_ConIdInexistente_Retorna404NotFound()
        {
            // Arrange
            var cancelarInexistente = new CancelarReservaDto { Id = 88888 };

            // Act
            var response = await _client.PostAsJsonAsync("/Reserva/Cancelar", cancelarInexistente);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostCrearReserva_EnMismasFechasQueReservaActiva_RechazaPorColision()
        {
            // Arrange (US-SEC-01 #1043 - Control de Colisión)
            // La reserva semilla 1 ya ocupa la habitación 1 del día +10 al día +15
            var reservaEnColision = new Reserva
            {
                HabitacionId = 1,
                NombreCliente = "Intento Sobreventa",
                FechaEntrada = DateTime.Today.AddDays(11),
                FechaSalida = DateTime.Today.AddDays(14),
                PrecioTotal = 200m
            };

            // Act
            var response = await _client.PostAsJsonAsync("/Reserva/Crear", reservaEnColision);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
