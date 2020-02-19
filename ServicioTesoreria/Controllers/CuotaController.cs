using cnrl.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServicioTesoreria.Logica;
using ServicioTesoreria.Models;
using ServicioTesoreria.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace ServicioTesoreria.Controllers
{
    public class CuotaController : ApiController
    {
        // GET api/cuota
        //public IEnumerable<Cuota> Get()
        //{
        //    return CuotaData.LeerTodo();
        //}
        //generarTicket($dni, $apellido, $nombre, $importe, $fechaVencimiento, $importe2, 
        //$fechaVencimiento2, $descripcionCuota, $numeroCuota, $cantidadCuotas, $concepto, $mail,$origen,$numeroFactura, $enviarMail);

        #region WebServiceViejo
        [HttpGet]
        public long GenerarCuota(long Dni
        , string Apellido
        , string Nombre
        , decimal Importe
        , DateTime FechaVto
        , decimal Importe2
        , DateTime FechaVto2
        , string DescripcionCuota
        , int NroCuota
        , int TotalCuota
        , int CodCon
        , string Email
        , string Origen
        , string NumeroFactura
        , int NroCurso
        , bool? generaBanelco = false
        , string banelcoDescripcionPantalla = ""
        , string banelcoDescripcionTicket = ""
        , string codigoClienteBanelco = "")
        {
            var cuotas = CuotaData.Buscar(Origen, Dni, null, null, null, null, null, null, null, null, null, null, null, null, NumeroFactura);

            if (cuotas.Count == 1)
            {
                foreach (var c in cuotas)
                {
                    c.Importe = Importe;
                    c.Importe2 = Importe2;
                    c.fechavto = FechaVto;
                    c.fechavto2 = FechaVto2;
                    CuotaData.Update(c);
                    return c.Id;
                }
            }

            var value = new Cuota();
            if (!String.IsNullOrEmpty(Origen))
                value.Origen = Origen;

            value.Dni = Dni;

            value.NroCuota = NroCuota;
            value.TotalCuota = TotalCuota;
            value.Importe = Importe;
            value.Importe2 = Importe2;
            value.fechavto = FechaVto;
            value.fechavto2 = FechaVto2;
            value.CodCon = CodCon;
            value.Estado = "0";
            value.NroFactura = NumeroFactura;
            value.Origen = Origen;
            value.DescripcionCuota = DescripcionCuota;

            if (generaBanelco.HasValue && generaBanelco.Value)
            {
                value.banelco = (int)EstadoBanelco.Generar;
                value.banelcoDescripcionPantalla = banelcoDescripcionPantalla;
                value.banelcoDescripcionTicket = banelcoDescripcionTicket;
                value.banelcoCodigo = codigoClienteBanelco;
            }
            else
            {
                value.banelco = (int)EstadoBanelco.NoGenerar;
            }

            var alumno = AlumnoData.LeerUno(Dni);

            if (alumno == default(Alumno))
            {
                alumno = new Alumno();
                alumno.ApeyNom = Apellido + " " + Nombre;
                alumno.Dni = Dni;

                AlumnoData.Insert(alumno);
            }

            long id = CuotaData.Insert(value);

            return id;
        }
        #endregion

        #region CRUD
        public void Post([FromBody]Cuota value)
        {
            CuotaData.Insert(value);
        }

        public void GenerarCuota(Cuota value)
        {
            try
            {
                CuotaData.Insert(value);
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(string.Format("Exception = {0}", "test")),//ex.Message)),
                    ReasonPhrase = "Error1"
                };
                throw new HttpResponseException(resp);
            }
        }

        public void GenerarIdCuota(EstadosCuotas value)
        {
            try
            {
                CuotaData.Insert(value);
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(string.Format("Exception = {0}", "test")),//ex.Message)),
                    ReasonPhrase = "Error1"
                };
                throw new HttpResponseException(resp);
            }
        }

        public bool ModificarCuota(long Dni
                                        , string origen
                                        , string nroFacura
                                        , int CodCon
                                        , int NroCuota
                                        , int TotalCuota
                                        , decimal Importe
                                        , DateTime FechaVto
                                        , decimal Importe2
                                        , DateTime FechaVto2
                                        , int NroCurso
                                        , long? id = null
                                        , int? CodPlan = null
                                        , int? NroComision = null
                                        , string Estado = ""
                                        , DateTime? FechaPago = null
                                        , int? NroRec = null
                                        , DateTime? FechaBaja = null
                                        , String Motivo = "")
        {
            var value = new Cuota();

            if (id.HasValue)
                value.Id = id.Value;
            else
            {
                var c = CuotaData.LeerUno(nroFacura, origen);
                if (c == default(Cuota))
                    return false;
                else
                    value.Id = c.Id;
            }
            if (!String.IsNullOrEmpty(origen))
                value.Origen = origen;
            value.Dni = Dni;
            if (CodPlan.HasValue)
                value.CodPlan = CodPlan.Value;
            value.NroCuota = NroCuota;
            value.TotalCuota = TotalCuota;
            value.Importe = Importe;
            value.Importe2 = Importe2;
            value.fechavto = FechaVto;
            value.fechavto2 = FechaVto2;
            if (!String.IsNullOrEmpty(Estado))
                value.Estado = Estado;
            if (FechaPago.HasValue)
                value.fechaPago = FechaPago.Value;
            if (NroRec.HasValue)
                value.NroRec = NroRec.Value;
            if (FechaBaja.HasValue)
                value.fechaBaja = FechaBaja.Value;
            if (!String.IsNullOrEmpty(Motivo))
                value.Motivo = Motivo;
            value.CodCon = CodCon;
            if (NroComision.HasValue)
                value.NroComision = NroComision.Value;
            value.NroCurso = NroCurso;

            var alumno = AlumnoData.LeerUno(Dni);
            if (alumno == default(Alumno))
            {
                return false;
            }

            var curso = CursoData.LeerUno(NroCurso, origen);

            if (curso == default(Curso))
            {
                return false;
            }

            CuotaData.Update(value);

            return true;
        }

        public bool UpdateCuota(Cuota cuota)
        {
            try
            {
                CuotaData.Update(cuota);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        // PUT api/cuota/5
        public void Put(int id, [FromBody]Cuota value)
        {
            value.Id = id;
            CuotaData.Update(value);
        }

        // DELETE api/cuota/5
        public void Delete(int id)
        {
            CuotaData.Delete(id);
        }

        #endregion

        #region Cambios de estado
        public bool BajaCuota(string motivo
                                               , string origen
                                               , long Dni
                                               , int CodPlan
                                               , int NroCuota
                                               , int TotalCuota
                                               , int CodCon
                                               , int NroComision
                                               , int Anio
                                               , int NroCurso
                                               )
        {
            var cuotas = CuotaData.Buscar(origen
                                        , Dni
                                        , CodPlan
                                        , NroCuota
                                        , TotalCuota
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , CodCon
                                        , NroComision
                                        , Anio
                                        , NroCurso);
            if (cuotas.Count == 1)
            {
                var cuota = cuotas.First();
                cuota.Estado = "B";
                cuota.Motivo = motivo;
                cuota.fechaBaja = DateTime.Now;
                CuotaData.Update(cuota);
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        public bool BajaCuota(long id, string motivo)
        {
            var cuota = CuotaData.LeerUno(id);
            if (cuota != default(Cuota))
            {
                cuota.Estado = "B";
                cuota.Motivo = motivo;
                cuota.fechaBaja = DateTime.Now;
                CuotaData.Update(cuota);
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        public bool BajaCuotaPorCursada(string origen, int id, string motivo = "Baja de Cursada", string usuario = " ")
        {
            var cuotas = CuotaData.LeerPorCursada(id, origen);
            if (cuotas != null && cuotas.Count > 0)
            {
                foreach (var cuota in cuotas)
                {
                    if (cuota.Estado != "P")
                    {
                        cuota.Estado = "B";
                        cuota.Motivo = motivo;
                        cuota.fechaBaja = DateTime.Now;
                        cuota.Usuario = usuario;
                        CuotaData.Update(cuota);
                    }
                }

            }
            return true;
        }

        [HttpGet]
        public bool BajaCuotaPorCursadaRestaurar(string origen, int id, string motivo = "Baja de Cursada Restaurada", string usuario = " ")
        {
            var cuotas = CuotaData.LeerPorCursada(id, origen);
            if (cuotas != null && cuotas.Count > 0)
            {
                foreach (var cuota in cuotas)
                {
                    if (cuota.Estado == "B")
                    {
                        cuota.Estado = "0";
                        cuota.Motivo = motivo;
                        cuota.fechaBaja = DateTime.Now;
                        cuota.Usuario = usuario;
                        CuotaData.Update(cuota);
                    }
                }

            }
            return true;
        }

        [HttpGet]
        public bool ImputarPagoCuota(long id, string motivo = "Imputacion Manual", int nroRecibo = 99)
        {
            var cuota = CuotaData.LeerUno(id);
            if (cuota != default(Cuota))
            {
                cuota.Estado = "P";
                cuota.fechaPago = DateTime.Now;
                cuota.Motivo = motivo;
                cuota.NroRec = nroRecibo;
                CuotaData.Update(cuota);
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        public bool DesvincularPagoCuota(long id, string motivo = "Desvinculacion Manual")
        {
            var cuota = CuotaData.LeerUno(id);
            if (cuota != default(Cuota))
            {
                cuota.Estado = "0";
                //cuota.fechaPago = DateTime.Now;
                cuota.Motivo = motivo;
                CuotaData.Update(cuota);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ImputarPagoCuota(string motivo
                                        , string origen
                                        , long Dni
                                        , int CodPlan
                                        , int NroCuota
                                        , int TotalCuota
                                        , int CodCon
                                        , int NroComision
                                        , int Anio
                                        , int NroCurso
                                        )
        {
            var cuotas = CuotaData.Buscar(origen
                                        , Dni
                                        , CodPlan
                                        , NroCuota
                                        , TotalCuota
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , CodCon
                                        , NroComision
                                        , Anio
                                        , NroCurso);
            if (cuotas.Count == 1)
            {
                var cuota = cuotas.First();
                cuota.Estado = "P";
                cuota.fechaPago = DateTime.Now;
                cuota.Motivo = motivo;
                CuotaData.Update(cuota);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Consultas
        [HttpGet]
        public IEnumerable<Cuota> ObtenerCuotas(string origen = ""
                                        , long? Dni = null
                                        , int? CodPlan = null
                                        , int? NroCuota = null
                                        , int? TotalCuota = null
                                        , decimal? Importe = null
                                        , DateTime? FechaVto = null
                                        , decimal? Importe2 = null
                                        , DateTime? FechaVto2 = null
                                        , string Estado = ""
                                        , DateTime? FechaPago = null
                                        , int? NroRec = null
                                        , DateTime? FechaBaja = null
                                        , String Motivo = ""
                                        , String NroFactura = ""
                                        , int? CodCon = null
                                        , int? NroComision = null
                                        , int? Anio = null
                                        , int? NroCurso = null
                                        , int? NroCuotaDesde = null
                                        , int? NroCuotaHasta = null
                                        , decimal? ImporteDesde = null
                                        , decimal? ImporteHasta = null
                                        , decimal? Importe2Desde = null
                                        , decimal? Importe2Hasta = null
                                        , DateTime? FechaVtoDesde = null
                                        , DateTime? FechaVtoHasta = null
                                        , DateTime? FechaVto2Desde = null
                                        , DateTime? FechaVto2Hasta = null
                                        , DateTime? FechaPagoDesde = null
                                        , DateTime? FechaPagoHasta = null
                                        , DateTime? FechaBajaDesde = null
                                        , DateTime? FechaBajaHasta = null
                                        , string listadoComisiones = ""
                                        , string listadoCuotas = ""
                                        , DateTime? fechaImportadoDesde = null
                                        , DateTime? fechaImportadoHasta = null
            )
        {
            var cuotas = CuotaData.Buscar(origen
                                        , Dni
                                        , CodPlan
                                        , NroCuota
                                        , TotalCuota
                                        , Importe
                                        , FechaVto
                                        , Importe2
                                        , FechaVto2
                                        , Estado
                                        , FechaPago
                                        , NroRec
                                        , FechaBaja
                                        , Motivo
                                        , NroFactura
                                        , CodCon
                                        , NroComision
                                        , Anio
                                        , NroCurso
                                        , NroCuotaDesde
                                        , NroCuotaHasta
                                        , ImporteDesde
                                        , ImporteHasta
                                        , Importe2Desde
                                        , Importe2Hasta
                                        , FechaVtoDesde
                                        , FechaVtoHasta
                                        , FechaVto2Desde
                                        , FechaVto2Hasta
                                        , FechaPagoDesde
                                        , FechaPagoHasta
                                        , FechaBajaDesde
                                        , FechaBajaHasta
                                        , listadoComisiones
                                        , listadoCuotas
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , fechaImportadoDesde
                                        , fechaImportadoHasta
                                        );

            return cuotas; //JsonConvert.SerializeObject(cuotas);
        }

        [HttpGet]
        public IEnumerable<Cuota> ObtenerCuotasMenorIgual(string origen = ""
                                        , long? Dni = null
                                        , int? CodPlan = null
                                        , int? NroCuota = null
                                        , int? TotalCuota = null
                                        , decimal? Importe = null
                                        , DateTime? FechaVto = null
                                        , decimal? Importe2 = null
                                        , DateTime? FechaVto2 = null
                                        , string Estado = ""
                                        , DateTime? FechaPago = null
                                        , int? NroRec = null
                                        , DateTime? FechaBaja = null
                                        , String Motivo = ""
                                        , String NroFactura = ""
                                        , int? CodCon = null
                                        , int? NroComision = null
                                        , int? Anio = null
                                        , int? NroCurso = null
                                        , int? NroCuotaDesde = null
                                        , int? NroCuotaHasta = null
                                        , decimal? ImporteDesde = null
                                        , decimal? ImporteHasta = null
                                        , decimal? Importe2Desde = null
                                        , decimal? Importe2Hasta = null
                                        , DateTime? FechaVtoDesde = null
                                        , DateTime? FechaVtoHasta = null
                                        , DateTime? FechaVto2Desde = null
                                        , DateTime? FechaVto2Hasta = null
                                        , DateTime? FechaPagoDesde = null
                                        , DateTime? FechaPagoHasta = null
                                        , DateTime? FechaBajaDesde = null
                                        , DateTime? FechaBajaHasta = null
                                        , string listadoComisiones = ""
                                        , string listadoCuotas = ""
                                        , DateTime? fechaImportadoDesde = null
                                        , DateTime? fechaImportadoHasta = null
            )
        {
            var cuotas = CuotaData.BuscarMenor(origen
                                        , Dni
                                        , CodPlan
                                        , NroCuota
                                        , TotalCuota
                                        , Importe
                                        , FechaVto
                                        , Importe2
                                        , FechaVto2
                                        , Estado
                                        , FechaPago
                                        , NroRec
                                        , FechaBaja
                                        , Motivo
                                        , NroFactura
                                        , CodCon
                                        , NroComision
                                        , Anio
                                        , NroCurso
                                        , NroCuotaDesde
                                        , NroCuotaHasta
                                        , ImporteDesde
                                        , ImporteHasta
                                        , Importe2Desde
                                        , Importe2Hasta
                                        , FechaVtoDesde
                                        , FechaVtoHasta
                                        , FechaVto2Desde
                                        , FechaVto2Hasta
                                        , FechaPagoDesde
                                        , FechaPagoHasta
                                        , FechaBajaDesde
                                        , FechaBajaHasta
                                        , listadoComisiones
                                        , listadoCuotas
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , fechaImportadoDesde
                                        , fechaImportadoHasta
                                        );

            return cuotas; //JsonConvert.SerializeObject(cuotas);
        }

        public string ObtenerEstadoCuota(string origen
                                        , long Dni
                                        , int CodPlan
                                        , int NroCuota
                                        , int TotalCuota
                                        , int CodCon
                                        , int NroComision
                                        , int Anio
                                        , int NroCurso
                                        )
        {
            var cuotas = CuotaData.Buscar(origen
                                        , Dni
                                        , CodPlan
                                        , NroCuota
                                        , TotalCuota
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , CodCon
                                        , NroComision
                                        , Anio
                                        , NroCurso);
            if (cuotas.Count > 0)
            {
                var cuota = cuotas.First();
                return cuota.Estado;
            }
            else
            {
                return "N/A";
            }
        }

        public string ObtenerEstadoCuota(string origen
                                        , string nroFactura
                                        )
        {
            var cuota = CuotaData.LeerUno(nroFactura, origen);
            if (cuota != default(Cuota))
            {
                return cuota.Estado;
            }
            else
            {
                return "N/A";
            }
        }

        public string ObtenerEstadoCuota(long id)
        {

            var cuota = CuotaData.LeerUno(id);
            if (cuota != default(Cuota))
            {
                return cuota.Estado;
            }
            else
            {
                return "N/A";
            }
        }
        [HttpGet]
        public Cuota Get(int id)
        {
            return CuotaData.LeerUno(id);
        }

        [HttpGet]
        public Cuota GetCuota(string NroFactura)
        {
            return CuotaData.LeerUno(NroFactura);
        }


        [HttpGet]
        public EstadosCuotas GetEstadosCuota(int id)
        {
            return CuotaData.LeerUnoEstadosCuotas(id);
        }

        public Cuota ObtenerCuota(int id)
        {
            return CuotaData.LeerUno(id);
        }

        [HttpGet]
        public decimal RecaudacionPorComision(string comision, string origen)
        {
            var cuotas = CuotaData.LeerPorComision(int.Parse(comision), origen);
            var cuo = new Decimal();
            //return cuotas.Where(x => x.Estado == "P").Sum(x => x.Importe);
            cuo += cuotas.Where(x => x.Estado == "P" && x.fechaPago <= x.fechavto).Sum(x => x.Importe);
            cuo += cuotas.Where(x => x.Estado == "P" && x.fechaPago > x.fechavto).Sum(x => x.Importe2);

            return cuo;
        }


        #endregion

        #region Cupones de pago
        //public HttpResponseMessage GetCuota(string origen, string nroFactura)
        //{
        //    var cupon = CuponData.Buscar(origen, nroFactura);
        //    var respuesta = Request.CreateResponse(HttpStatusCode.OK, cupon);
        //    return respuesta;
        //}

        public HttpResponseMessage GetCuota(string origen, string nroFactura, string nombre = "", string curso = "", bool vencimientoRelativo = false, int diasVencimientoRelativo = 0, bool tipoCurso = false)
        {
            var cupon = CuponData.Buscar(origen, nroFactura);
            if (!string.IsNullOrEmpty(nombre))
            {
                foreach (var c in cupon)
                {
                    c.Nombre = nombre;
                }
            }
            else
            {
                foreach (var c in cupon)
                {
                    var alumno = AlumnoData.LeerUno(c.Dni);
                    c.Nombre = alumno.ApeyNom;
                }
            }
            if (!string.IsNullOrEmpty(curso))
            {
                foreach (var c in cupon)
                {
                    c.CursoNombre = curso;
                }
            }

            if (cupon.First().FechaVto2.DayOfYear < DateTime.Now.DayOfYear && tipoCurso == true)
            {
                foreach (var c in cupon)
                {
                    c.FechaVto2 = DateTime.Now.AddDays(10);
                    c.fechaVencimiento2 = DateTime.Now.AddDays(10).ToString("yyyy/MM/dd");
                }
            }

            if (vencimientoRelativo == true && DateTime.Parse(cupon.First().FechaVto.ToShortDateString()) < DateTime.Parse(DateTime.Now.ToShortDateString()))
            {
                var fecha = DateTime.Now.AddDays(diasVencimientoRelativo);
                foreach (var c in cupon)
                {
                    c.FechaVto = fecha;
                    c.fechaVencimiento = fecha.ToString("yyyy/MM/dd");

                    if (origen.Equals("INGE"))
                    {
                        c.FechaVto2 = fecha.AddDays(10);
                        c.fechaVencimiento2 = fecha.AddDays(10).ToString("yyyy/MM/dd");
                    }
                    else {
                        c.FechaVto2 = fecha.AddDays(1);
                        c.fechaVencimiento2 = fecha.AddDays(1).ToString("yyyy/MM/dd");
                    }
                    c.Importe = c.Importe2;
                }
            }

            if (vencimientoRelativo == false && DateTime.Parse(cupon.First().FechaVto.ToShortDateString()) < DateTime.Parse(DateTime.Now.ToShortDateString()))
            {
                var fecha = DateTime.Now.AddDays(0);
                foreach (var c in cupon)
                {
                    //c.FechaVto = fecha;
                    //c.fechaVencimiento = fecha.ToString("dd/MM/yyyy");
                    //c.FechaVto2 = fecha.AddDays(1);
                    //c.fechaVencimiento2 = fecha.AddDays(1).ToString("dd/MM/yyyy");
                    //c.Importe = c.Importe2;
                }
            }



            var respuesta = Request.CreateResponse(HttpStatusCode.OK, cupon);
            return respuesta;
        }

        public string ToRfc822(DateTime date)
        {
            int offset = TimeZone.CurrentTimeZone.GetUtcOffset(date).Hours;

            string timeZone = "+" + offset.ToString().PadLeft(2, '0');

            if (offset < 0)
            {
                int i = offset * -1;
                timeZone = "-" + i.ToString().PadLeft(2, '0');
            }

            string utc = date.ToString("yyyy-MM-dd'T'HH:mm:ss" + timeZone.PadRight(5, '0'));
            //int length = utc.Length;
            //return utc.Substring(0, length - 3) + timeZone.PadRight(5, '0');
            utc = utc.Replace("00:00:00", "23:59:00");
            return utc;
        }

        public async System.Threading.Tasks.Task<HttpResponseMessage> CuotaPayPerTic(string origen, string nroFactura, string username, string password, string clientId, string clientSecret, string nombre = "", string curso = "", bool vencimientoRelativo = false, int diasVencimientoRelativo = 0, string email = "")
        {
            var cupon = CuponData.Buscar(origen, nroFactura);
            List<Cupon> cuponEstadoCuota = new List<Cupon>();
            if (cupon.Count() != 0)
            {
                cuponEstadoCuota = CuponData.BuscarEstadosCuotas(cupon.First().Id.ToString()).OrderByDescending(x=>x.Id).ToList();
            }

            if (!string.IsNullOrEmpty(nombre))
            {
                foreach (var c in cupon)
                {
                    c.Nombre = nombre;
                }
            }
            else
            {
                foreach (var c in cupon)
                {
                    var alumno = AlumnoData.LeerUno(c.Dni);
                    c.Nombre = alumno.ApeyNom;
                    c.Dni = alumno.Dni;
                }
            }
            if (!string.IsNullOrEmpty(curso))
            {
                foreach (var c in cupon)
                {
                    c.CursoNombre = curso;
                }
            }

            if (cupon.First().FechaVto2 <= DateTime.Now)
            {
                foreach (var c in cupon)
                {
                    c.FechaVto2 = DateTime.Now.AddDays(10);
                    c.fechaVencimiento2 = DateTime.Now.AddDays(10).ToString("dd/MM/yyyy");
                }
            }

            if (vencimientoRelativo == true && cupon.First().FechaVto > DateTime.Now)
            {
                var fecha = DateTime.Now.AddDays(diasVencimientoRelativo);
                foreach (var c in cupon)
                {
                    c.FechaVto = fecha;
                    c.fechaVencimiento = fecha.ToString("dd/MM/yyyy");
                    c.FechaVto2 = fecha.AddDays(1);
                    c.fechaVencimiento2 = fecha.AddDays(1).ToString("dd/MM/yyyy");
                    c.Importe2 = c.Importe;
                }
            }

            var cuponGenera = cupon.First();
            PayPerTicCrearPago pptPago = new PayPerTicCrearPago();
            IFormatProvider culture = new CultureInfo("en-US", true);
            try
            {
                // var fechas = DateTime.ParseExact(cuponGenera.FechaVto.Date.ToUniversalTime().ToString(), "yyyy-MM-dd'T'HH:mm:ss-ffff", culture);

                pptPago.currency_id = Constantes.PPT_CURRENCY_PESOS;

                pptPago.due_date = ToRfc822(cuponGenera.FechaVto); //2020-12-30T09:05:29-0300//"yyyy-MM-dd'T'HH:mm:ss-ffff"
                pptPago.last_due_date = ToRfc822(cuponGenera.FechaVto2);

                //pptPago.external_transaction_id = cuponGenera.Id.ToString();
                pptPago.external_transaction_id = cuponEstadoCuota.Count() != 0 ? cuponEstadoCuota.First().Id.ToString() : cuponGenera.Id.ToString();

                pptPago.payer = new PayPerTicPagador();
                pptPago.payer.email = email;
                pptPago.payer.name = nombre;
                pptPago.payer.identification = new PayPerTicIdentificacion();
                pptPago.payer.identification.country = Constantes.PPT_COUNTRY;
                pptPago.payer.identification.type = Constantes.PPT_DNI;
                pptPago.payer.identification.number = cuponGenera.Dni.ToString();

            }
            catch (Exception ex)
            {

                throw;
            }
            string accessToken = await GetTokenPayPerTic(username, password, clientId, clientSecret);
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServicioPayPerTic"]);
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            //        Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(ConfigurationManager.AppSettings["ServicioTesoUsuario"])));

            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            //        Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(accessToken)));

            //    /*
            //    Authorization:Bearer {{access_token}}
            //    Cache-Control:no-cache
            //    Content-Type:application/json
            //     */
            //    var content = new StringContent(JsonConvert.SerializeObject(pptPago));
            //    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = await client.PostAsync(ConfigurationManager.AppSettings["ServicioPayPerTic"] + "pagos", content);
            //    if (response.IsSuccessStatusCode == false)//error!
            //    {
            //        var error = await response.Content.ReadAsStringAsync();
            //        throw new Exception(error);
            //    }
            //    //var respuesta = responseGet.Content.ReadAsAsync<IEnumerable<Cuota>>().Result;
            //    else
            //    {
            //        var rta = await response.Content.ReadAsStringAsync();

            //        //cuotas = respuesta.ToList();

            //        //foreach (var cuota in cuotas)
            //        //{
            //        //    var cursoCuota = db.curso.Where(x => x.codCurso == cuota.NroCurso).FirstOrDefault();
            //        //    if (cursoCuota != null)
            //        //    {
            //        //        cuota.Curso = cursoCuota.descripcion;
            //        //        if (cursoCuota.precio != null)
            //        //            cuota.Concepto = cursoCuota.precio.descripcion;
            //        //    }
            //        //}
            //    }
            //}

            //var username = "38wivhraf4wrhuvn";
            //var password = "BNBmrkGQZwv3HVG9";

            //// Estos valores debes conocerlos de antemano para autenticar tu cliente
            //var clientId = "16465308-1844-4abe-abe6-f184149ee740";
            //var clientSecret = "a2d03fa3-f6c4-45e5-9792-dc0d8b51a25c";

            //var username = "5o6Y29gSzDQbQcBx";
            //var password = "bFptFXgSAjcDYNVP4f";
            //var clientId = "16465308-1844-4abe-abe6-f184149ee740";
            //var clientSecret = "a2d03fa3-f6c4-45e5-9792-dc0d8b51a25c";

            System.Net.ServicePointManager.Expect100Continue = false;
            var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ServicioPayPerTic"] + "pagos");


            //var postData = "currency_id=" + pptPago.currency_id + "&due_date=" + "2020-12-30T09:05:29-0300" + " & external_transaction_id=" + pptPago.external_transaction_id + "&details=[amount=" + cuponGenera.Importe.ToString() + "&concept_id=" + cuponGenera.CodCon.ToString() + "&concept_description=" + cuponGenera.CursoNombre.ToString() + "&external_reference=1]";
            //var data = System.Text.Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";

            request.KeepAlive = false;

            request.Headers.Add("Cache-Control", "no-cache");
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            //request.UserAgent = "Mozilla / 4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            //request.UseDefaultCredentials = true;

            request.PreAuthenticate = true;
            request.Credentials = new NetworkCredential(username, password);


            //request.ContentLength = data.Length;
            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                currency_id = pptPago.currency_id,
                due_date = pptPago.due_date,
                last_due_date = pptPago.last_due_date,
                notification_url = ConfigurationManager.AppSettings["NotificacionPayPerTic"].ToString(),

                external_transaction_id = int.Parse(pptPago.external_transaction_id),

                charge_delay = Math.Round(cuponGenera.Importe2 - cuponGenera.Importe, 2),

                details = new[]
                {
                   new {
                        external_reference = pptPago.external_transaction_id,// "1",
                        concept_id = cuponGenera.CodCon.ToString(),
                        concept_description = cuponGenera.CursoNombre.ToString(),
                        amount =Math.Round(cuponGenera.Importe,2)

                   }
                },

                payer = new
                {
                    name = pptPago.payer.name,
                    email = pptPago.payer.email,
                    identification = new
                    {
                        type = pptPago.payer.identification.type,
                        number = pptPago.payer.identification.number,
                        country = pptPago.payer.identification.country
                    }
                }
                //email = pptPago.payer.email,
                //name = pptPago.payer.name,

                //country = pptPago.payer.identification.country,
                //type = pptPago.payer.identification.type,
                //number = pptPago.payer.identification.number
            });
            //request.ContentLength = json.Length;
            try
            {
                using (StreamWriter streamRequest = new StreamWriter(request.GetRequestStream()))
                {
                    //dataStream.Write(arrData, 0, arrData.Length);

                    streamRequest.Write(json);
                }

                //  StreamWriter streamRequest = new StreamWriter(request.GetRequestStream());
                //streamRequest.Write(json);
                //streamRequest.Close();
            }
            catch (Exception ex)
            {

                throw;
            }
            HttpWebResponse response;
            string strRta = "";
            try
            {
                response = request.GetResponse() as HttpWebResponse;

                StreamReader dataStream = new StreamReader(response.GetResponseStream());

                strRta = dataStream.ReadToEnd();


                dataStream.Close();

                response.Close();
                var res = (JObject)JsonConvert.DeserializeObject(strRta);

                //var payment = res["payments"].FirstOrDefault();
                //var pago = payments.FirstOrDefault(x => (int)x["external_transaction_id"] == int.Parse(pptPago.external_transaction_id));
                if (res != null)
                {

                    string url = res["form_url"].ToString();
                    strRta = url.ToString();

                }
            }
            catch (WebException wex)
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ERROR:" + wex.Message + ". STATUS: " + wex.Status.ToString());

                if (wex.Status == WebExceptionStatus.ProtocolError)
                {
                    var responses = ((HttpWebResponse)wex.Response);
                    sb.AppendLine(string.Format("Status Code : {0}", responses.StatusCode));
                    sb.AppendLine(string.Format("Status Description : {0}", responses.StatusDescription));

                    try
                    {
                        StreamReader reader = new StreamReader(responses.GetResponseStream());
                        sb.AppendLine(reader.ReadToEnd());
                    }
                    catch (WebException ex) { throw; }
                }

                if (sb.ToString().Contains("\"extended_code\" : 4215") == true)
                {


                    //return Request.CreateResponse(HttpStatusCode.Conflict);

                    //request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ServicioPayPerTic"] + "pagos/listado?limit=100000");
                    //request.Method = "GET";

                    request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ServicioPayPerTic"] + "pagos/busqueda");
                    request.Method = "POST";

                    request.KeepAlive = false;

                    request.Headers.Add("Cache-Control", "no-cache");
                    request.ContentType = "application/json";
                    request.Headers.Add("Authorization", "Bearer " + accessToken);

                    request.PreAuthenticate = true;
                    request.Credentials = new NetworkCredential(username, password);

                    json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {

                        external_transaction_id = int.Parse(pptPago.external_transaction_id)

                    });
                    request.ContentLength = json.Length;

                    StreamWriter streamRequest = new StreamWriter(request.GetRequestStream());
                    streamRequest.Write(json);
                    streamRequest.Close();
                    try
                    {
                        response = request.GetResponse() as HttpWebResponse;

                        StreamReader dataStream = new StreamReader(response.GetResponseStream());

                        strRta = dataStream.ReadToEnd();


                        dataStream.Close();

                        response.Close();
                        var res = (JObject)JsonConvert.DeserializeObject(strRta);
                        //if (res != null)
                        //{
                        //    if (res["status"].ToString() == "pending")
                        //    {
                        //        string url = res["form_url"].ToString();
                        //        strRta = res.ToString();
                        //    }
                        //}
                        var payments = res["payments"].FirstOrDefault();
                        //var pago = payments.FirstOrDefault(x => (int)x["external_transaction_id"] == int.Parse(pptPago.external_transaction_id));
                        if (payments != null)
                        {
                            switch (payments["status"].ToString())
                            {
                                case "pending":
                                case "issued":
                                    string url = payments["form_url"].ToString();
                                    strRta = url.ToString();
                                    break;
                                case "overdue":
                                    strRta = "Vencido";
                                    break;
                                case "cancelled":
                                    strRta = "Cancelado";
                                    break;
                                case "rejected":
                                    strRta = "Rechazado";
                                    break;
                                default:
                                    strRta = "Aprobado";
                                    break;
                            }
                            //if (payments["status"].ToString() == "pending" || payments["status"].ToString() == "issued")
                            //{
                            //    string url = payments["form_url"].ToString();
                            //    strRta = url.ToString();
                            //}
                            //else
                            //{
                            //    if (payments["status"].ToString() == "overdue")
                            //    {
                            //        strRta = "Vencido";
                            //        //pptPago.external_transaction_id = cuponGenera.Id.ToString() + "|" + DateTime.Now.ToShortDateString();
                            //        //json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                            //        //{
                            //        //    currency_id = pptPago.currency_id,
                            //        //    due_date = pptPago.due_date,
                            //        //    notification_url = ConfigurationManager.AppSettings["NotificacionPayPerTic"].ToString(),

                            //        //    external_transaction_id = int.Parse(pptPago.external_transaction_id),

                            //        //    details = new[]
                            //        //            {
                            //        //                new {external_reference = "1",
                            //        //                     concept_id = cuponGenera.CodCon.ToString(),
                            //        //                     concept_description = cuponGenera.CursoNombre.ToString(),
                            //        //                     amount =Math.Round( cuponGenera.Importe,2)

                            //        //                    }
                            //        //            },

                            //        //    payer = new
                            //        //    {
                            //        //        name = pptPago.payer.name,
                            //        //        email = pptPago.payer.email,
                            //        //        identification = new
                            //        //        {
                            //        //            type = pptPago.payer.identification.type,
                            //        //            number = pptPago.payer.identification.number,
                            //        //            country = pptPago.payer.identification.country
                            //        //        }
                            //        //    }
                            //        //    //email = pptPago.payer.email,
                            //        //    //name = pptPago.payer.name,

                            //        //    //country = pptPago.payer.identification.country,
                            //        //    //type = pptPago.payer.identification.type,
                            //        //    //number = pptPago.payer.identification.number
                            //        //});
                            //        ////request.ContentLength = json.Length;
                            //        //try
                            //        //{
                            //        //    using (streamRequest = new StreamWriter(request.GetRequestStream()))
                            //        //    {
                            //        //        //dataStream.Write(arrData, 0, arrData.Length);

                            //        //        streamRequest.Write(json);
                            //        //    }

                            //        //    //  StreamWriter streamRequest = new StreamWriter(request.GetRequestStream());
                            //        //    //streamRequest.Write(json);
                            //        //    //streamRequest.Close();
                            //        //}
                            //        //catch (Exception ex)
                            //        //{

                            //        //    throw;
                            //        //}

                            //        //strRta = "";

                            //        //response = request.GetResponse() as HttpWebResponse;

                            //        //dataStream = new StreamReader(response.GetResponseStream());

                            //        //strRta = dataStream.ReadToEnd();


                            //        //dataStream.Close();

                            //        //response.Close();
                            //        //res = (JObject)JsonConvert.DeserializeObject(strRta);

                            //        ////var payment = res["payments"].FirstOrDefault();
                            //        ////var pago = payments.FirstOrDefault(x => (int)x["external_transaction_id"] == int.Parse(pptPago.external_transaction_id));
                            //        //if (res != null)
                            //        //{

                            //        //    string url = res["form_url"].ToString();
                            //        //    strRta = url.ToString();

                            //        //}


                            //    }
                            //    else
                            //    {
                            //        if (payments["status"].ToString() == "cancelled")
                            //        {
                            //            strRta = "Vencido";
                            //        }
                            //        else
                            //        {
                            //            strRta = "Aprobado";
                            //        }

                            //    }

                            //}
                        }
                    }
                    catch (Exception ex)
                    {

                        //throw;
                    }

                }
                else
                {
                    strRta = "Error";
                }
                //throw new Exception(sb.ToString(), wex);
            }
            //catch (Exception ex) { throw; }

            //string estado = (((HttpWebResponse)response).StatusDescription);
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);


            var respuesta = Request.CreateResponse(HttpStatusCode.OK, strRta);

            return respuesta;
        }

        private async System.Threading.Tasks.Task<string> GetTokenPayPerTic(string username, string password, string clientId, string clientSecret)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            // Estos valores los obtienes usando input del usuario
            //var username = "38wivhraf4wrhuvn";
            //var password = "BNBmrkGQZwv3HVG9";

            //// Estos valores debes conocerlos de antemano para autenticar tu cliente
            //var clientId = "16465308-1844-4abe-abe6-f184149ee740";
            //var clientSecret = "a2d03fa3-f6c4-45e5-9792-dc0d8b51a25c";

            //var username = "5o6Y29gSzDQbQcBx";
            //var password = "bFptFXgSAjcDYNVP4f";

            //var clientId = "16465308-1844-4abe-abe6-f184149ee740";
            //var clientSecret = "a2d03fa3-f6c4-45e5-9792-dc0d8b51a25c";

            var request = (HttpWebRequest)WebRequest.Create("https://a.paypertic.com/auth/realms/entidades/protocol/openid-connect/token/");

            var postData = "grant_type=password&username=" + username + "&password=" + password + "&client_id=" + clientId + "&client_secret=" + clientSecret;

            var data = System.Text.Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded;charset=utf8";
            request.KeepAlive = false;
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf8";
            request.Credentials = new NetworkCredential(username, password);
            request.ContentLength = data.Length;
            //request.Timeout = 0;
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var stream = request.GetRequestStream())
                {
                    //stream.WriteTimeout = 0;
                    stream.Write(data, 0, data.Length);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //   using (var client = new HttpClient())
            //   {
            //       client.BaseAddress = new Uri("https://a.paypertic.com");

            //       /*
            //       Authorization:Bearer {{access_token}}
            //       Cache-Control:no-cache
            //       Content-Type:application/json
            //        */

            //       /*
            //   username:38wivhraf4wrhuvn
            //   password:BNBmrkGQZwv3HVG9
            //   grant_type:password
            //   client_id:16465308-1844-4abe-abe6-f184149ee740
            //   client_secret:a2d03fa3-f6c4-45e5-9792-dc0d8b51a25c
            //*/
            //       PayPerTicAutorization autorization = new PayPerTicAutorization();

            //       autorization.username = "38wivhraf4wrhuvn";
            //       autorization.password = "BNBmrkGQZwv3HVG9";
            //       autorization.grant_type = "password";
            //       autorization.client_id = "16465308-1844-4abe-abe6-f184149ee740";
            //       autorization.client_secret = "a2d03fa3-f6c4-45e5-9792-dc0d8b51a25c";


            //       var content = new StringContent(JsonConvert.SerializeObject(autorization));



            //       content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            //       var response = await client.PostAsync("https://a.paypertic.com/auth/realms/entidades/protocol/openid-connect/token", content);
            //       if (response.IsSuccessStatusCode == false)//error!
            //       {
            //           var error = await response.Content.ReadAsStringAsync();
            //           throw new Exception(error);
            //       }
            //       //var respuesta = responseGet.Content.ReadAsAsync<IEnumerable<Cuota>>().Result;
            //       else
            //       {
            //           var rta = await response.Content.ReadAsStringAsync();
            //       }
            //   }

            JObject obj = (JObject)JsonConvert.DeserializeObject(responseString);
            return obj["access_token"].ToString();
        }

        public HttpResponseMessage GetCuota(int idCuota)
        {
            var cupon = CuponData.Buscar(idCuota);
            foreach (var c in cupon)
            {
                var alumno = AlumnoData.LeerUno(c.Dni);
                c.Nombre = alumno.ApeyNom;
            }

            //if (cupon.First().FechaVto2 <= DateTime.Now)
            //{
            //    foreach (var c in cupon)
            //    {
            //        c.FechaVto2 = DateTime.Now.AddDays(10);
            //        c.fechaVencimiento2 = DateTime.Now.AddDays(10).ToString("dd/MM/yyyy");
            //    }
            //}

            //if (vencimientoRelativo == true && cupon.First().FechaVto > DateTime.Now)
            //{
            //    var fecha = DateTime.Now.AddDays(diasVencimientoRelativo);
            //    foreach (var c in cupon)
            //    {
            //        c.FechaVto = fecha;
            //        c.fechaVencimiento = fecha.ToString("dd/MM/yyyy");
            //        c.FechaVto2 = fecha.AddDays(1);
            //        c.fechaVencimiento2 = fecha.AddDays(1).ToString("dd/MM/yyyy");
            //        c.Importe2 = c.Importe;
            //    }
            //}



            var respuesta = Request.CreateResponse(HttpStatusCode.OK, cupon);
            return respuesta;
        }

        //public HttpResponseMessage GetCuota(long Dni
        //    , string Apellido
        //    , string Nombre
        //    , decimal Importe
        //    , DateTime FechaVto
        //    , decimal Importe2
        //    , DateTime FechaVto2
        //    , string DescripcionCuota
        //    , int NroCuota
        //    , int TotalCuota
        //    , int CodCon
        //    , string Email
        //    , string Origen
        //    , string NumeroFactura
        //    , int NroCurso)
        //{

        //    var value = new Cuota();
        //    if (!String.IsNullOrEmpty(Origen))
        //        value.Origen = Origen;

        //    value.Dni = Dni;

        //    value.NroCuota = NroCuota;
        //    value.TotalCuota = TotalCuota;
        //    value.Importe = Importe;
        //    value.Importe2 = Importe2;
        //    value.fechavto = FechaVto;
        //    value.fechavto2 = FechaVto2;
        //    value.CodCon = CodCon;
        //    value.Estado = "0";
        //    value.NroFactura = NumeroFactura;
        //    value.Origen = Origen;
        //    value.DescripcionCuota = DescripcionCuota;
        //    var alumno = AlumnoData.LeerUno(Dni);

        //    if (alumno == default(Alumno))
        //    {
        //        alumno = new Alumno();
        //        alumno.ApeyNom = Apellido + " " + Nombre;
        //        alumno.Dni = Dni;

        //        AlumnoData.Insert(alumno);
        //    }

        //    long id = CuotaData.Insert(value);

        //    var cupon = CuponData.Buscar(id);
        //    foreach (var cu in cupon)
        //        {
        //            cu.Nombre = Apellido + " " + Nombre; 
        //        }
        //    foreach (var cu in cupon)
        //        {
        //            cu.CursoNombre = DescripcionCuota;
        //        }

        //    if (cupon.First().FechaVto2 <= DateTime.Now)
        //    {
        //        foreach (var cu in cupon)
        //        {
        //            cu.FechaVto2 = DateTime.Now.AddDays(10);
        //            cu.fechaVencimiento2 = DateTime.Now.AddDays(10).ToString("dd/MM/yyyy");
        //        }
        //    }

        //    var respuesta = Request.CreateResponse(HttpStatusCode.OK, cupon);
        //    return respuesta;
        //}


        //public HttpResponseMessage GetCuota(long dni, string apellido, string nombre,
        //            decimal importe, string fechaVencimiento, decimal importe2, string fechaVencimiento2,
        //            string descripcionCuota, int numeroCuota, int cantidadCuotas,
        //            string concepto, string origen, string nroFactura)
        //{//$respuesta = $oSoapClient->generarTicket($dni, $apellido, $nombre, $importe, $fechaVencimiento, 
        // //             $importe2, $fechaVencimiento2, $descripcionCuota, $numeroCuota, $cantidadCuotas, 
        // //             $concepto, $mail,$origen,$numeroFactura, $enviarMail);
        //    DateTime FechaVto = DateTime.ParseExact(fechaVencimiento, "dd/MM/yyyy",
        //                                  System.Globalization.CultureInfo.InvariantCulture);
        //    DateTime FechaVto2 = DateTime.ParseExact(fechaVencimiento2, "dd/MM/yyyy",
        //                                           System.Globalization.CultureInfo.InvariantCulture);

        //    var cupon = CuponData.Buscar(origen, nroFactura);
        //    if (cupon.Count == 0)
        //    {
        //        var cup = new Cupon();
        //        cupon.Add(cup);

        //        var cuota = new Cuota();
        //        cuota.CodCon = int.Parse(concepto);
        //        cuota.Dni = dni;
        //        cuota.fechavto = FechaVto;
        //        cuota.fechavto2 = FechaVto2;
        //        cuota.Importe = importe;
        //        cuota.Importe2 = importe2;
        //        cuota.NroCuota = numeroCuota;
        //        cuota.NroFactura = nroFactura;
        //        cuota.Origen = origen;
        //        cuota.TotalCuota = cantidadCuotas;
        //        //inserto la cuota
        //        CuotaData.Insert(cuota);
        //        var alumno = AlumnoData.LeerUno(dni);

        //        if (alumno == null)
        //        {
        //            alumno.ApeyNom = apellido + ", " + nombre;
        //            alumno.Dni = dni;
        //            AlumnoData.Insert(alumno);
        //        }
        //    }
        //    cupon[0].NroFactura = nroFactura;
        //    cupon[0].fecha = DateTime.Now.ToString("dd/MM/yyyy");
        //    cupon[0].Nombre = apellido + ", " + nombre;
        //    cupon[0].Domicilio = "-";
        //    cupon[0].Localidad = "-";
        //    cupon[0].CursoNombre = descripcionCuota;
        //    cupon[0].NroCuota = numeroCuota;
        //    cupon[0].TotalCuota = cantidadCuotas;
        //    cupon[0].Importe = importe;
        //    cupon[0].Importe2 = importe2;
        //    cupon[0].FechaVto = FechaVto;
        //    cupon[0].FechaVto2 = FechaVto2;
        //    cupon[0].fechaVencimiento = fechaVencimiento;
        //    cupon[0].fechaVencimiento2 = fechaVencimiento2;

        //    return Request.CreateResponse(HttpStatusCode.OK, cupon);
        //}

        #endregion


        // GET api/cuota/5

        // POST api/cuota


    }
}
