using ServicioTesoreria.Models;
using ServicioTesoreria.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServicioTesoreria.Controllers
{
    public class AlumnoController : ApiController
    {
        // GET api/alumno
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/alumno/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/alumno
        public void Post([FromBody]string value)
        {
        }

        // PUT api/alumno/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/alumno/5
        public void Delete(int id)
        {
        }

        public bool TieneDeudaAlumno(long id)
        {
            return CuotaData.TieneDeuda(id);
        }

        [HttpGet]
        public bool TieneDeudaAlumnoVencida(long id, string origen = "", DateTime? fecha = null, bool? SegundoVencimiento = null)
        {
            if (!fecha.HasValue)
                fecha = DateTime.Now;
            return CuotaData.TieneDeudaVencida(id, origen, fecha.Value, SegundoVencimiento);
        }

        public Alumno ObtenerAlumno(long id)
        {
            return AlumnoData.LeerUno(id);
        }

        public bool crearAlumno(long? Dni = null
                                        , string ApeYNom = ""
                                        , string Sexo = ""
                                        , DateTime? FechaNac = null
                                        , string Domicilio = ""
                                        , int? codigoPostal = null
                                        , string Telefono = ""
                                        , bool? activo = null
                                        , bool? academico = null)
        {
            var value = new Alumno();
            if (!String.IsNullOrEmpty(ApeYNom))
                value.ApeyNom = ApeYNom;
            if (!String.IsNullOrEmpty(Sexo))
                value.Sexo = Sexo;
            if (!String.IsNullOrEmpty(Domicilio))
                value.Domicilio = Domicilio;
            if (!String.IsNullOrEmpty(Telefono))
                value.Telefono = Telefono;
            if (Dni.HasValue)
                value.Dni = Dni.Value;
            if (FechaNac.HasValue)
                value.FechaNac = FechaNac.Value;
            if (codigoPostal.HasValue)
                value.CodPostal = codigoPostal.Value;
            if (activo.HasValue)
                value.Activo = activo.Value;
            if (academico.HasValue)
                value.Academico = academico.Value;
            
            AlumnoData.Insert(value);

            return true;
        }

        public bool editarAlumno(long? Dni = null
                                        , string ApeYNom = ""
                                        , string Sexo = ""
                                        , DateTime? FechaNac = null
                                        , string Domicilio = ""
                                        , int? codigoPostal = null
                                        , string Telefono = ""
                                        , bool? activo = null
                                        , bool? academico = null)
        {
            var value = new Alumno();
            if (!String.IsNullOrEmpty(ApeYNom))
                value.ApeyNom = ApeYNom;
            if (!String.IsNullOrEmpty(Sexo))
                value.Sexo = Sexo;
            if (!String.IsNullOrEmpty(Domicilio))
                value.Domicilio = Domicilio;
            if (!String.IsNullOrEmpty(Telefono))
                value.Telefono = Telefono;
            if (Dni.HasValue)
                value.Dni = Dni.Value;
            if (FechaNac.HasValue)
                value.FechaNac = FechaNac.Value;
            if (codigoPostal.HasValue)
                value.CodPostal = codigoPostal.Value;
            if (activo.HasValue)
                value.Activo = activo.Value;
            if (academico.HasValue)
                value.Academico = academico.Value;

            AlumnoData.Update(value);

            return true;
        }

        public bool crearAlumno(Alumno alumno)
        {
            AlumnoData.Insert(alumno);

            return true;
        }

        public bool editarAlumno(Alumno alumno)
        {
            AlumnoData.Update(alumno);

            return true;
        }


    }
}
