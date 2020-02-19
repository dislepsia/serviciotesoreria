using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioTesoreria.Models
{
    public class Actividad
    {
        public long Id { get; set; }
        public long Dni { get; set; }
        public decimal Importe { get; set; }
        public DateTime fecha { get; set; }
        public int NroRec { get; set; }
        public int CodCon { get; set; }
        public int Control { get; set; }
        public string Origen { get; set; }
        public string ApeYNom{ get; set; }
    }
}