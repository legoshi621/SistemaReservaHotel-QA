using Moq;
using SistemaReservaHotel.Business.Services;
using SistemaReservaHotel.DataAccess.Repositories;
using SistemaReservaHotel.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SistemaReservaHotel.UnitTests
{
    public class DisponibilidadServiceTests
    {
        private readonly Mock<IHabitacionRepository> _habitacionRepoMock;
        private readonly Mock<IReservaRepository> _reservaRepoMock;
        private readonly DisponibilidadService _service;

        public DisponibilidadServiceTests()
        {
            _habitacionRepoMock = new Mock<IHabitacionRepository>();
            _reservaRepoMock = new Mock<IReservaRepository>();
            _service = new DisponibilidadService(_habitacionRepoMock.Object, _reservaRepoMock.Object);
        }

        [Fact]
        public async Task ObtenerDisponiblesAsync_SinReservas_RetornaTodasLasHabitacionesActivas()
        {
            // Arrange
            var habitaciones = new List<Habitacion>
            {
                new Habitacion { Id = 1, Numero = 101, Activa = true },
                new Habitacion { Id = 2, Numero = 102, Activa = true }
            };

            _habitacionRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(habitaciones);
            _reservaRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(new List<Reserva>());

            var entrada = DateTime.Today.AddDays(1);
            var salida = DateTime.Today.AddDays(4);

            // Act
            var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);

            // Assert
            Assert.Equal(2, disponibles.Count);
            Assert.Contains(disponibles, h => h.Id == 1);
            Assert.Contains(disponibles, h => h.Id == 2);
        }

        [Fact]
        public async Task ObtenerDisponiblesAsync_HabitacionInactiva_NoSeIncluyeEnDisponibles()
        {
            // Arrange
            var habitaciones = new List<Habitacion>
            {
                new Habitacion { Id = 1, Numero = 101, Activa = true },
                new Habitacion { Id = 2, Numero = 102, Activa = false }
            };

            _habitacionRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(habitaciones);
            _reservaRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(new List<Reserva>());

            var entrada = DateTime.Today.AddDays(1);
            var salida = DateTime.Today.AddDays(4);

            // Act
            var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);

            // Assert
            Assert.Single(disponibles);
            Assert.Equal(1, disponibles[0].Id);
        }

        [Fact]
        public async Task ObtenerDisponiblesAsync_ConReservaActivaEnConflicto_ExcluyeHabitacionOcupada()
        {
            // Arrange
            var habitaciones = new List<Habitacion>
            {
                new Habitacion { Id = 1, Numero = 101, Activa = true },
                new Habitacion { Id = 2, Numero = 102, Activa = true }
            };

            var reservas = new List<Reserva>
            {
                new Reserva
                {
                    Id = 1,
                    HabitacionId = 1,
                    FechaEntrada = DateTime.Today.AddDays(2),
                    FechaSalida = DateTime.Today.AddDays(5),
                    Estado = "Activa"
                }
            };

            _habitacionRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(habitaciones);
            _reservaRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(reservas);

            var entrada = DateTime.Today.AddDays(3);
            var salida = DateTime.Today.AddDays(6);

            // Act
            var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);

            // Assert
            Assert.Single(disponibles);
            Assert.Equal(2, disponibles[0].Id);
        }

        [Fact]
        public async Task ObtenerDisponiblesAsync_ConReservaCanceladaEnMismoRango_NoExcluyeHabitacion()
        {
            // Arrange
            var habitaciones = new List<Habitacion>
            {
                new Habitacion { Id = 1, Numero = 101, Activa = true }
            };

            var reservas = new List<Reserva>
            {
                new Reserva
                {
                    Id = 1,
                    HabitacionId = 1,
                    FechaEntrada = DateTime.Today.AddDays(2),
                    FechaSalida = DateTime.Today.AddDays(5),
                    Estado = "Cancelada"
                }
            };

            _habitacionRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(habitaciones);
            _reservaRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(reservas);

            var entrada = DateTime.Today.AddDays(3);
            var salida = DateTime.Today.AddDays(4);

            // Act
            var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);

            // Assert
            Assert.Single(disponibles);
            Assert.Equal(1, disponibles[0].Id);
        }

        [Fact]
        public async Task ObtenerDisponiblesAsync_ReservaFinalizaElMismoDiaDeEntrada_HabitacionDisponible()
        {
            // Arrange
            var habitaciones = new List<Habitacion>
            {
                new Habitacion { Id = 1, Numero = 101, Activa = true }
            };

            var reservas = new List<Reserva>
            {
                new Reserva
                {
                    Id = 1,
                    HabitacionId = 1,
                    FechaEntrada = DateTime.Today.AddDays(1),
                    FechaSalida = DateTime.Today.AddDays(4),
                    Estado = "Activa"
                }
            };

            _habitacionRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(habitaciones);
            _reservaRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(reservas);

            var entrada = DateTime.Today.AddDays(4);
            var salida = DateTime.Today.AddDays(7);

            // Act
            var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);

            // Assert
            Assert.Single(disponibles);
            Assert.Equal(1, disponibles[0].Id);
        }

        [Fact]
        public async Task ObtenerDisponiblesAsync_ReservaIniciaElMismoDiaDeSalida_HabitacionDisponible()
        {
            // Arrange
            var habitaciones = new List<Habitacion>
            {
                new Habitacion { Id = 1, Numero = 101, Activa = true }
            };

            var reservas = new List<Reserva>
            {
                new Reserva
                {
                    Id = 1,
                    HabitacionId = 1,
                    FechaEntrada = DateTime.Today.AddDays(5),
                    FechaSalida = DateTime.Today.AddDays(8),
                    Estado = "Activa"
                }
            };

            _habitacionRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(habitaciones);
            _reservaRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(reservas);

            var entrada = DateTime.Today.AddDays(2);
            var salida = DateTime.Today.AddDays(5);

            // Act
            var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);

            // Assert
            Assert.Single(disponibles);
            Assert.Equal(1, disponibles[0].Id);
        }
    }
}
