using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioTesoreria.Logica
{
    public static class Constantes
    {
        public const string CONN = "Tesoreria";
        public const string WHERE = @"{WHERE}";
        public const int ERROR = -1;
        public const string A_Z = "A-Z";

        public const string PPT_CURRENCY_PESOS = @"ARS";
        public const string PPT_DNI = @"DNI_ARG";
        public const string PPT_COUNTRY = @"ARG";

    }

    public enum EstadosPago
    {
        Creado = 1,
        Generado = 2,
        Importado = 3,
        Verificar = 4,
        Verificado = 5,
    }

    public enum TiposMedioPago
    {
        PagoFacil = 1,
        Banelco = 2
    }

    public enum EstadoBanelco
    {
        NoGenerar = 0,
        Generar = 1,
        Generado = 2
    }
}