using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioTesoreria.Models
{
    public class Cuota
    {
        public long Id { get; set; }
        public int CodPlan { get; set; }
        public long Dni { get; set; }
        public int NroCuota { get; set; }
        public int TotalCuota { get; set; }
        public decimal Importe { get; set; }
        public DateTime fechavto { get; set; }
        public decimal Importe2 { get; set; }
        public DateTime fechavto2 { get; set; }
        public DateTime fechaPago { get; set; }
        public int NroRec { get; set; }
        public string Estado { get; set; }
        public DateTime fechaBaja { get; set; }
        public String Motivo { get; set; }
        public int CodCon { get; set; }
        public int NroComision { get; set; }
        public int NroCurso { get; set; }
        public string Origen { get; set; }
        public string NroFactura { get; set; }
        public string DescripcionCuota { get; set; }

        public int banelco { get; set; }
        public string banelcoDescripcionPantalla { get; set; }
        public string banelcoDescripcionTicket { get; set; }
        public string banelcoCodigo { get; set; }

        public string Usuario { get; set; }
        
    }
}