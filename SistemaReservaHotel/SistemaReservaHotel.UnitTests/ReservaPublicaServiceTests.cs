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
    public class ReservaPublicaServiceTests
    {
        private readonly Mock<IReservaRepository> _reservaRepoMock;
        private readonly ReservaService _service;

        public ReservaPublicaServiceTests()
        {
            _reservaRepoMock = new Mock<IReservaRepository>();
            _service = new ReservaService(_reservaRepoMock.Object);
        }

        #region Carlos Alonso Miranda Rivas - US-14 (#1034) - Consultas y Lectura

        [Fact]
        public async Task ObtenerPorDiaAsync_RetornaReservasDelDiaIndicado()
        {
            // Arrange
            var fecha = DateTime.Today;
            var reservasSimuladas = new List<Reserva>
            {
                new Reserva { Id = 1, NombreCliente = "Cliente 1", FechaEntrada = fecha, FechaSalida = fecha.AddDays(2) },
                new Reserva { Id = 2, NombreCliente = "Cliente 2", FechaEntrada = fecha, FechaSalida = fecha.AddDays(1) }
            };

            _reservaRepoMock.Setup(r => r.ObtenerPorDiaAsync(fecha)).ReturnsAsync(reservasSimuladas);

            // Act
            var resultado = await _service.ObtenerPorDiaAsync(fecha);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            _reservaRepoMock.Verify(r => r.ObtenerPorDiaAsync(fecha), Times.Once);
        }

        [Fact]
        public async Task ObtenerTodasAsync_RetornaListaDeReservas()
        {
            // Arrange
            var listaEsperada = new List<Reserva>
            {
                new Reserva { Id = 1, NombreCliente = "Juan Perez" },
                new Reserva { Id = 2, NombreCliente = "Maria Gomez" }
            };

            _reservaRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(listaEsperada);

            // Act
            var resultado = await _service.ObtenerTodasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Juan Perez", resultado[0].NombreCliente);
            _reservaRepoMock.Verify(r => r.ObtenerTodasAsync(), Times.Once);
        }

        [Fact]
        public async Task ObtenerPorIdAsync_IdExistente_RetornaReserva()
        {
            // Arrange
            var reservaEsperada = new Reserva { Id = 1, NombreCliente = "Carlos Lopez" };
            _reservaRepoMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(reservaEsperada);

            // Act
            var resultado = await _service.ObtenerPorIdAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Carlos Lopez", resultado.NombreCliente);
            _reservaRepoMock.Verify(r => r.ObtenerPorIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task ObtenerPorIdAsync_IdInexistente_RetornaNull()
        {
            // Arrange
            _reservaRepoMock.Setup(r => r.ObtenerPorIdAsync(999)).ReturnsAsync((Reserva?)null);

            // Act
            var resultado = await _service.ObtenerPorIdAsync(999);

            // Assert
            Assert.Null(resultado);
            _reservaRepoMock.Verify(r => r.ObtenerPorIdAsync(999), Times.Once);
        }

        #endregion

        #region Camila Antonia Calderón Cortez - US-SEC-01 (#1043) - Mutación y Validación de Colisión

        [Fact]
        public async Task ReservarAsync_CreaReservaYRetornaEntidad()
        {
            // Arrange
            var nuevaReserva = new Reserva
            {
                NombreCliente = "Ana Torres",
                HabitacionId = 1,
                FechaEntrada = DateTime.Today.AddDays(2),
                FechaSalida = DateTime.Today.AddDays(5),
                PrecioTotal = 150m,
                Estado = "Activa"
            };

            var reservaCreada = new Reserva
            {
                Id = 10,
                NombreCliente = "Ana Torres",
                HabitacionId = 1,
                FechaEntrada = nuevaReserva.FechaEntrada,
                FechaSalida = nuevaReserva.FechaSalida,
                PrecioTotal = 150m,
                Estado = "Activa"
            };

            _reservaRepoMock.Setup(r => r.CrearAsync(nuevaReserva)).ReturnsAsync(reservaCreada);

            // Act
            var resultado = await _service.ReservarAsync(nuevaReserva);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(10, resultado.Id);
            Assert.Equal("Ana Torres", resultado.NombreCliente);
            _reservaRepoMock.Verify(r => r.CrearAsync(nuevaReserva), Times.Once);
        }

        [Fact]
        public async Task ActualizarAsync_LlamaMetodoActualizarDelRepositorio()
        {
            // Arrange
            var reserva = new Reserva
            {
                Id = 5,
                NombreCliente = "Pedro Soto",
                Estado = "Cancelada"
            };

            _reservaRepoMock.Setup(r => r.ActualizarAsync(reserva)).Returns(Task.CompletedTask);

            // Act
            await _service.ActualizarAsync(reserva);

            // Assert
            _reservaRepoMock.Verify(r => r.ActualizarAsync(reserva), Times.Once);
        }

        [Fact]
        public async Task EliminarAsync_LlamaMetodoEliminarDelRepositorio()
        {
            // Arrange
            int reservaId = 7;
            _reservaRepoMock.Setup(r => r.EliminarAsync(reservaId)).Returns(Task.CompletedTask);

            // Act
            await _service.EliminarAsync(reservaId);

            // Assert
            _reservaRepoMock.Verify(r => r.EliminarAsync(reservaId), Times.Once);
        }

        [Fact]
        public async Task ValidarDisponibilidadAsync_CuandoEstaDisponible_RetornaTrue()
        {
            // Arrange
            int habitacionId = 1;
            var entrada = DateTime.Today.AddDays(2);
            var salida = DateTime.Today.AddDays(5);

            _reservaRepoMock.Setup(r => r.ValidarDisponibilidadAsync(habitacionId, entrada, salida)).ReturnsAsync(true);

            // Act
            var resultado = await _service.ValidarDisponibilidadAsync(habitacionId, entrada, salida);

            // Assert
            Assert.True(resultado);
            _reservaRepoMock.Verify(r => r.ValidarDisponibilidadAsync(habitacionId, entrada, salida), Times.Once);
        }

        [Fact]
        public async Task ValidarDisponibilidadAsync_CuandoNoEstaDisponible_RetornaFalse()
        {
            // Arrange
            int habitacionId = 1;
            var entrada = DateTime.Today.AddDays(2);
            var salida = DateTime.Today.AddDays(5);

            _reservaRepoMock.Setup(r => r.ValidarDisponibilidadAsync(habitacionId, entrada, salida)).ReturnsAsync(false);

            // Act
            var resultado = await _service.ValidarDisponibilidadAsync(habitacionId, entrada, salida);

            // Assert
            Assert.False(resultado);
            _reservaRepoMock.Verify(r => r.ValidarDisponibilidadAsync(habitacionId, entrada, salida), Times.Once);
        }

        #endregion
    }
}
