using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioTesoreria.Models
{
    public class Cupon
    {
        public long Id { get; set; }
        public int CodPlan { get; set; }
        public long Dni { get; set; }
        public int NroCuota { get; set; }
        public int TotalCuota { get; set; }
        public decimal Importe { get; set; }
        public string fechaVencimiento { get; set; }
        public DateTime FechaVto { get; set; }
        public decimal Importe2 { get; set; }
        public string fechaVencimiento2 { get; set; }
        public DateTime FechaVto2 { get; set; }
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
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string CursoNombre { get; set; }
        public string fecha { get; set; }
        public byte[] barcode { get; set; }

        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }
        //public  { get; set; }

    }
}