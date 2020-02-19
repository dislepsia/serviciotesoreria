using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioTesoreria.Models
{
    public class Alumno
    {
        public long Dni { get; set; }
        public string ApeyNom { get; set; }
        public string Sexo { get; set; }
        public string Domicilio { get; set; }
        public DateTime FechaNac{ get; set; }
        public int CodPostal { get; set; }
        public string Telefono { get; set; }
        public bool Activo { get; set; }
        public bool Academico { get; set; }
    }
}