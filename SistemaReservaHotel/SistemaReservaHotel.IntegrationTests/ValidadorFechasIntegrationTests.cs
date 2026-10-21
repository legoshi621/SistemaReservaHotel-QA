using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;
using SistemaReservaHotel.IntegrationTests.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SistemaReservaHotel.IntegrationTests
{
    public class ValidadorFechasIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public ValidadorFechasIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostReserva_ConFechasValidas_Retorna200OkYPersisteReserva()
        {
            // Arrange (US-16 #1040 - Caso Rango Futuro Válido)
            var nuevaReserva = new Reserva
            {
                HabitacionId = 1,
                NombreCliente = "Cliente Fechas Validas",
                FechaEntrada = DateTime.Today.AddDays(20),
                FechaSalida = DateTime.Today.AddDays(25),
                PrecioTotal = 300m,
                Estado = "Activa"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/Reserva/Crear", nuevaReserva);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var reservaCreada = await response.Content.ReadFromJsonAsync<Reserva>();
            reservaCreada.Should().NotBeNull();
            reservaCreada!.Id.Should().BeGreaterThan(0);
            reservaCreada.NombreCliente.Should().Be("Cliente Fechas Validas");

            // Validar persistencia en BD en memoria
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var guardada = await db.Reservas.FindAsync(reservaCreada.Id);
            guardada.Should().NotBeNull();
            guardada!.Estado.Should().Be("Activa");
        }

        [Fact]
        public async Task PostReserva_ConFechaEntradaMayorQueSalida_Retorna400BadRequest()
        {
            // Arrange (US-16 #1040 - Caso Rango Invertido)
            var reservaInvertida = new Reserva
            {
                HabitacionId = 1,
                NombreCliente = "Cliente Invertido",
                FechaEntrada = DateTime.Today.AddDays(25),
                FechaSalida = DateTime.Today.AddDays(20),
                PrecioTotal = 150m
            };

            // Act
            var response = await _client.PostAsJsonAsync("/Reserva/Crear", reservaInvertida);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostReserva_ConFechaEntradaIgualASalida_Retorna400BadRequest()
        {
            // Arrange (US-16 #1040 - Caso Estancia Cero Noches)
            var reservaMismoDia = new Reserva
            {
                HabitacionId = 1,
                NombreCliente = "Cliente Mismo Dia",
                FechaEntrada = DateTime.Today.AddDays(5),
                FechaSalida = DateTime.Today.AddDays(5),
                PrecioTotal = 100m
            };

            // Act
            var response = await _client.PostAsJsonAsync("/Reserva/Crear", reservaMismoDia);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostReserva_ConFechaEntradaEnElPasado_Retorna400BadRequest()
        {
            // Arrange (US-16 #1040 - Check-in en el pasado)
            var reservaPasada = new Reserva
            {
                HabitacionId = 1,
                NombreCliente = "Cliente Pasado",
                FechaEntrada = DateTime.Today.AddDays(-2),
                FechaSalida = DateTime.Today.AddDays(3),
                PrecioTotal = 200m
            };

            // Act
            var response = await _client.PostAsJsonAsync("/Reserva/Crear", reservaPasada);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostReserva_ConAmbasFechasEnElPasado_Retorna400BadRequest()
        {
            // Arrange (US-16 #1040 - Rango de fechas caducado)
            var reservaCaducada = new Reserva
            {
                HabitacionId = 1,
                NombreCliente = "Cliente Caducado",
                FechaEntrada = DateTime.Today.AddDays(-10),
                FechaSalida = DateTime.Today.AddDays(-5),
                PrecioTotal = 250m
            };

            // Act
            var response = await _client.PostAsJsonAsync("/Reserva/Crear", reservaCaducada);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
