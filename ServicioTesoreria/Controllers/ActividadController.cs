using ServicioTesoreria.Logica;
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
    public class ActividadController : ApiController
    {
        // GET api/actividad
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/actividad/5
        public Actividad Get(int id)
        {
            return ActividadData.LeerUno(id);
        }

        public Actividad ObtenerActividad(int id)
        {
            return ActividadData.LeerUno(id);
        }

        // POST api/actividad
        public void Post([FromBody]string value)
        {
        }

        // PUT api/actividad/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/actividad/5
        public void Delete(int id)
        {
            ActividadData.Delete(id);
        }

        public long GenerarActividad(long? Dni = null
                                        , string ApeYNom= ""
                                        , string origen = ""
                                        , int? CodCon = null
                                        , decimal? Importe = null
                                        , int? control = null
                                        , bool? generaPagoFacil = null
                                        , bool? generaBanelco = null
                                        , string codigoClienteBanelco = "")
        {
            var value = new Actividad();
            if (!String.IsNullOrEmpty(origen))
                value.Origen = origen;
            if (!String.IsNullOrEmpty(ApeYNom))
                value.ApeYNom = ApeYNom;
            if (Dni.HasValue)
                value.Dni = Dni.Value;
            if (Importe.HasValue)
                value.Importe = Importe.Value;
            if (CodCon.HasValue)
                value.CodCon = CodCon.Value;
            if (control.HasValue)
                value.Control = control.Value;
            
            long id = ActividadData.Insert(value);

            if (generaPagoFacil.HasValue)
            {
                var medio = new MedioDePago();
                medio.activida_id = id;
                medio.Estado = (int)EstadosPago.Creado;
                medio.fechaCreado = DateTime.Now;
                medio.TipoMedioPago = (int)TiposMedioPago.PagoFacil;
                MedioDePagoData.Insert(medio);
            }
            if (generaBanelco.HasValue)
            {
                var medio = new MedioDePago();
                medio.activida_id = id;
                medio.Estado = (int)EstadosPago.Creado;
                medio.fechaCreado = DateTime.Now;
                medio.TipoMedioPago = (int)TiposMedioPago.Banelco;
                if (!string.IsNullOrEmpty(medio.codigoGeneracion))
                    medio.codigoGeneracion = "111111";
                else
                    medio.codigoGeneracion = codigoClienteBanelco;
                MedioDePagoData.Insert(medio);
            }

            return id;
        }

        public bool ModificarCuota(long id
                                        ,long? Dni = null
                                        , string ApeYNom= ""
                                        , string origen = ""
                                        , int? CodCon = null
                                        , decimal? Importe = null
                                        , int? nroRec = null
                                        , DateTime? fecha = null
                                        , int? control = null
                                        , bool? generaPagoFacil = null
                                        , bool? generaBanelco = null
                                        , string codigoClienteBanelco = "")
        {
            var value = new Actividad();
            if (!String.IsNullOrEmpty(origen))
                value.Origen = origen;
            if (!String.IsNullOrEmpty(ApeYNom))
                value.ApeYNom = ApeYNom;
            if (Dni.HasValue)
                value.Dni = Dni.Value;
            if (Importe.HasValue)
                value.Importe = Importe.Value;
            if (CodCon.HasValue)
                value.CodCon = CodCon.Value;
            if (control.HasValue)
                value.Control = control.Value;
            if (fecha.HasValue)
                value.fecha = fecha.Value;
            if (nroRec.HasValue)
                value.NroRec = nroRec.Value;

            ActividadData.Update(value);

            return true;
        }

        public IEnumerable<Actividad> ObtenerActividades(string origen = ""
                                        , long? Dni = null
                                        , decimal? Importe = null
                                        , DateTime? Fecha= null
                                        , int? NroRec = null
                                        , int? CodCon = null
                                        , int? Control = null
                                        , decimal? ImporteDesde = null
                                        , decimal? ImporteHasta = null
                                        , DateTime? FechaDesde = null
                                        , DateTime? FechaHasta = null
                                        )
        {
            return ActividadData.Buscar(origen
                                        , Dni
                                        , Importe
                                        , Fecha
                                        , NroRec
                                        , CodCon
                                        , Control
                                        , ImporteDesde
                                        , ImporteHasta
                                        , FechaDesde
                                        , FechaHasta);
        }


        public string ObtenerEstadoActividad(long id)
        {

            var actividad = ActividadData.LeerUno(id);
            if (actividad != default(Actividad))
            {
                if (actividad.NroRec == 0)
                    return "0";
                else
                    return "P";
            }
            else
            {
                return "N/A";
            }
        }

        public bool ImputarPagoActividad(long id)
        {
            var actividad = ActividadData.LeerUno(id);
            if (actividad != default(Actividad))
            {
                actividad.NroRec = 1;
                ActividadData.Update(actividad);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
