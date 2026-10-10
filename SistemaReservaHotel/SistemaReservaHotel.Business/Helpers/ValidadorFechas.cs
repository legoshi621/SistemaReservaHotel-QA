using System;

namespace SistemaReservaHotel.Business.Helpers
{
    public static class ValidadorFechas
    {
        public static bool FechasValidas(DateTime entrada, DateTime salida)
        {
            return entrada < salida && entrada >= DateTime.Today;
        }
    }
}