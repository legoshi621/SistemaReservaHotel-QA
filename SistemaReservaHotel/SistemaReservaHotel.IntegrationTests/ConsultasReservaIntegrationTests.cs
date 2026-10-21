using FluentAssertions;
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
    public class ConsultasReservaIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ConsultasReservaIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetObtenerPorDia_ConFechaConReservas_Retorna200OkYListaReservas()
        {
            // Arrange (US-14 #1034 - La reserva semilla 1 abarca del día +10 al día +15)
            var fechaConsulta = DateTime.Today.AddDays(12).ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/Reserva/ObtenerPorDia?fecha={fechaConsulta}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var reservas = await response.Content.ReadFromJsonAsync<List<Reserva>>();
            reservas.Should().NotBeNull();
            reservas.Should().Contain(r => r.Id == 1);
        }

        [Fact]
        public async Task GetObtenerPorDia_ConFechaSinReservas_Retorna200OkYListaVacia()
        {
            // Arrange (US-14 #1034 - Fecha futura lejana sin reservas registradas)
            var fechaSinReservas = DateTime.Today.AddDays(60).ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/Reserva/ObtenerPorDia?fecha={fechaSinReservas}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var reservas = await response.Content.ReadFromJsonAsync<List<Reserva>>();
            reservas.Should().NotBeNull();
            reservas.Should().BeEmpty();
        }

        [Fact]
        public async Task GetDetalles_ConIdExistente_Retorna200OkYReservaCorrespondiente()
        {
            // Arrange (US-14 #1034 - Reserva Semilla con Id 1)
            int idExistente = 1;

            // Act
            var response = await _client.GetAsync($"/Reserva/Detalles/{idExistente}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var reserva = await response.Content.ReadFromJsonAsync<Reserva>();
            reserva.Should().NotBeNull();
            reserva!.Id.Should().Be(idExistente);
            reserva.NombreCliente.Should().Be("Carlos Miranda");
            reserva.HabitacionId.Should().Be(1);
        }

        [Fact]
        public async Task GetDetalles_ConIdInexistente_Retorna404NotFound()
        {
            // Arrange (US-14 #1034 - ID que no existe en la base de datos)
            int idInexistente = 9999;

            // Act
            var response = await _client.GetAsync($"/Reserva/Detalles/{idInexistente}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
