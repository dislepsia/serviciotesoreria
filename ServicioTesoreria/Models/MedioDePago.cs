using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioTesoreria.Models
{
    public class MedioDePago
    {
        public long Id { get; set; }
        public long cuota_id { get; set; }
        public long activida_id { get; set; }
        public int TipoMedioPago { get; set; }
        public string TipoMedioPagoDescripcion { get; set; }
        public int Estado { get; set; }
        public string EstadoDescripcion { get; set; }
        public decimal Importe { get; set; }
        public DateTime fechaCreado { get; set; }
        public DateTime fechaVerificar { get; set; }
        public DateTime fechaVerificado { get; set; }
        public DateTime fechaGenerado { get; set; }
        public DateTime fechaImportadoPago { get; set; }
        public string codigoGeneracion{ get; set; }
        public string codigoImportacion { get; set; }
   }
}