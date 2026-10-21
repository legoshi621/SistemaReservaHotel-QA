using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SistemaReservaHotel.DataAccess;
using SistemaReservaHotel.Entities;
using SistemaReservaHotel.IntegrationTests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SistemaReservaHotel.IntegrationTests
{
    public class DisponibilidadIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public DisponibilidadIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetDisponibilidad_EnRangoSinConflicto_Retorna200OkYHabitacionDisponible()
        {
            // Arrange (US-15 #1037 - Rango libre de fechas: hoy a hoy + 3 días)
            var entrada = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            var salida = DateTime.Today.AddDays(3).ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/Disponibilidad?entrada={entrada}&salida={salida}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var disponibles = await response.Content.ReadFromJsonAsync<List<Habitacion>>();
            disponibles.Should().NotBeNull();
            disponibles.Should().Contain(h => h.Id == 1 && h.Activa);
        }

        [Fact]
        public async Task GetDisponibilidad_EnRangoConReservaActiva_ExcluyeHabitacionOcupada()
        {
            // Arrange (US-15 #1037 - Conflicto con la reserva semilla existente del día 10 al 15)
            var entrada = DateTime.Today.AddDays(11).ToString("yyyy-MM-dd");
            var salida = DateTime.Today.AddDays(14).ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/Disponibilidad?entrada={entrada}&salida={salida}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var disponibles = await response.Content.ReadFromJsonAsync<List<Habitacion>>();
            disponibles.Should().NotBeNull();
            disponibles.Should().NotContain(h => h.Id == 1);
        }

        [Fact]
        public async Task GetDisponibilidad_HabitacionInactiva_NoSeIncluyeEnDisponibles()
        {
            // Arrange: Agregar una habitación inactiva en la BD en memoria
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!db.Habitaciones.Any(h => h.Id == 99))
                {
                    db.Habitaciones.Add(new Habitacion
                    {
                        Id = 99,
                        Numero = 999,
                        Tipo = "Mantenimiento",
                        Descripcion = "Habitación fuera de servicio",
                        PrecioBase = 100m,
                        Activa = false,
                        Estado = "En Reparacion"
                    });
                    await db.SaveChangesAsync();
                }
            }

            var entrada = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            var salida = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/Disponibilidad?entrada={entrada}&salida={salida}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var disponibles = await response.Content.ReadFromJsonAsync<List<Habitacion>>();
            disponibles.Should().NotBeNull();
            disponibles.Should().NotContain(h => h.Id == 99);
        }
    }
}
