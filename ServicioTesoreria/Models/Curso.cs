using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioTesoreria.Models
{
    public class Curso
    {
        public long Id { get; set; }
        public string Abreviada { get; set; }
        public string Descripcion { get; set; }
        public int CodCon { get; set; }
        public int NroCurso { get; set; }
        public string Origen { get; set; }

        }
}