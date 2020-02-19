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
    public class CursoController : ApiController
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

        public Curso ObtenerCurso(long id)
        {
            return CursoData.LeerUno(id);
        }
        public Curso ObtenerCurso(int NroCurso, string origen)
        {
            return CursoData.LeerUno(NroCurso, origen);
        }

        public bool crearCurso(string Descripcion = "", string Abreviado = "", int? CodCon = null, string Origen = "", int? NroCurso = null)
        {
            var value = new Curso();
            if (!String.IsNullOrEmpty(Descripcion))
                value.Descripcion = Descripcion;
            if (!String.IsNullOrEmpty(Abreviado))
                value.Abreviada = Abreviado;
            if (!String.IsNullOrEmpty(Origen))
                value.Origen = Origen ;
            if (CodCon.HasValue)
                value.CodCon = CodCon.Value;
            if (NroCurso.HasValue)
                value.NroCurso = NroCurso.Value;
            
            CursoData.Insert(value);

            return true;
        }

        public bool editarCurso(long? id=null, string Descripcion = "", string Abreviado = "", int? CodCon = null, string Origen = "", int? NroCurso = null)
        {
            var value = new Curso();
            if (!String.IsNullOrEmpty(Descripcion))
                value.Descripcion = Descripcion;
            if (!String.IsNullOrEmpty(Abreviado))
                value.Abreviada = Abreviado;
            if (!String.IsNullOrEmpty(Origen))
                value.Origen = Origen;
            if (CodCon.HasValue)
                value.CodCon = CodCon.Value;
            if (NroCurso.HasValue)
                value.NroCurso = NroCurso.Value;
            if (id.HasValue)
                value.Id = id.Value;
            else
            {
                if (!String.IsNullOrEmpty(Origen) && NroCurso.HasValue)
                {
                    var curso = CursoData.LeerUno(NroCurso.Value, Origen);

                    if (curso != default(Curso))
                    {
                        value.Id = curso.Id;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                    return false;
            }

            CursoData.Update(value);

            return true;
        }

        public bool crearAlumno(Curso curso)
        {
            CursoData.Insert(curso);

            return true;
        }

        public bool editarAlumno(Curso curso)
        {
            if (curso.Id == null)
                if (String.IsNullOrEmpty(curso.Origen) && curso.NroCurso != null)
                {
                    var curso1 = CursoData.LeerUno(curso.NroCurso, curso.Origen);

                    if (curso1 != default(Curso))
                    {
                        curso.Id = curso.Id;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                    return false;
                        
            CursoData.Update(curso);

            return true;
        }


    }
}
