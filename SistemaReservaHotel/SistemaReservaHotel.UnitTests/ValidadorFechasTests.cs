using SistemaReservaHotel.Business.Helpers;
using System;
using Xunit;

namespace SistemaReservaHotel.UnitTests
{
    public class ValidadorFechasTests
    {
        [Fact]
        public void FechasValidas_EntradaFuturaYMenorQueSalida_RetornaTrue()
        {
            // Arrange
            var entrada = DateTime.Today.AddDays(2);
            var salida = DateTime.Today.AddDays(5);

            // Act
            var resultado = ValidadorFechas.FechasValidas(entrada, salida);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void FechasValidas_EntradaHoyYSalidaManana_RetornaTrue()
        {
            // Arrange
            var entrada = DateTime.Today;
            var salida = DateTime.Today.AddDays(1);

            // Act
            var resultado = ValidadorFechas.FechasValidas(entrada, salida);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void FechasValidas_EntradaMayorQueSalida_RetornaFalse()
        {
            // Arrange
            var entrada = DateTime.Today.AddDays(5);
            var salida = DateTime.Today.AddDays(2);

            // Act
            var resultado = ValidadorFechas.FechasValidas(entrada, salida);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void FechasValidas_EntradaIgualASalida_RetornaFalse()
        {
            // Arrange
            var entrada = DateTime.Today.AddDays(3);
            var salida = DateTime.Today.AddDays(3);

            // Act
            var resultado = ValidadorFechas.FechasValidas(entrada, salida);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void FechasValidas_EntradaEnElPasado_RetornaFalse()
        {
            // Arrange
            var entrada = DateTime.Today.AddDays(-1);
            var salida = DateTime.Today.AddDays(2);

            // Act
            var resultado = ValidadorFechas.FechasValidas(entrada, salida);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void FechasValidas_EntradaYSalidaEnElPasado_RetornaFalse()
        {
            // Arrange
            var entrada = DateTime.Today.AddDays(-5);
            var salida = DateTime.Today.AddDays(-2);

            // Act
            var resultado = ValidadorFechas.FechasValidas(entrada, salida);

            // Assert
            Assert.False(resultado);
        }
    }
}
