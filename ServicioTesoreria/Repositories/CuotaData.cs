
using ServicioTesoreria.Logica;
using ServicioTesoreria.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;


namespace ServicioTesoreria.Repositories
{
    public class CuotaData
    {
        private static readonly string conexion = ConfigurationManager.ConnectionStrings[Constantes.CONN].ConnectionString;

        const string QUERY = @"
            SELECT DISTINCT
                  C.id
                  ,C.Dni
                  ,C.CodPlan
                  ,C.NroCuota
                  ,C.TotalCuota
                  ,C.Importe
                  ,C.FechaVto
                  ,C.Importe2
                  ,C.FechaVto2
                  ,C.FechaPago
                  ,C.Estado
                  ,C.NroRec
                  ,C.FechaBaja
                  ,C.Motivo
                  ,C.CodCon
                  ,C.NroComision
                  ,C.NroCurso
                  ,C.Origen
                  ,C.NroFactura
                  ,C.descripcionCuota
                  ,C.banelco
                  ,C.banelcoDescripcionPantalla
                  ,C.banelcoDescripcionTicket 
                  ,C.banelcoCodigo 
            FROM
                Cuota C
            LEFT JOIN
                MedioDePago M ON C.id = M.cuota_id
            {WHERE} 
            ORDER BY  C.id desc
            ";


        public static List<Cuota> LeerTodo()
        {
            var query = QUERY.Replace(Constantes.WHERE, "");
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Cuota>(query).ToList();
            }
        }

        public static List<Cuota> Buscar(string origen = ""
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
                                        , int? banelco = null
                                        , string banelcoDescripcionPantalla = ""
                                        , string banelcoDescripcionTicket = ""
                                        , string descripcionCuota = ""
                                        , string banelcoCodigo = ""
                                        , DateTime? fechaImportadoDesde = null
                                        , DateTime? fechaImportadoHasta = null
            )
        {
            string where = @"WHERE 1 = 1";
            DynamicParameters param = new DynamicParameters();

            if (!string.IsNullOrEmpty(origen))
            {
                param.Add("@origen", dbType: DbType.String, value: origen);
                where += " AND C.origen = @origen ";
            }
            if (Dni.HasValue)
            {
                param.Add("@Dni", dbType: DbType.Int64, value: Dni.Value);
                where += " AND C.Dni = @Dni ";
            }
            if (CodPlan.HasValue)
            {
                param.Add("@CodPlan", dbType: DbType.Int32, value: CodPlan.Value);
                where += " AND C.CodPlan = @CodPlan ";
            }
            if (NroCuota.HasValue)
            {
                param.Add("@NroCuota", dbType: DbType.Int32, value: NroCuota.Value);
                where += " AND C.NroCuota = @NroCuota ";
            }
            if (NroCuotaDesde.HasValue)
            {
                param.Add("@NroCuotaDesde", dbType: DbType.Int32, value: NroCuotaDesde.Value);
                where += " AND C.NroCuota >= @NroCuotaDesde ";
            }
            if (NroCuotaHasta.HasValue)
            {
                param.Add("@NroCuotaHasta", dbType: DbType.Int32, value: NroCuotaHasta.Value);
                where += " AND C.NroCuota <= @NroCuotaHasta ";
            }
            if (TotalCuota.HasValue)
            {
                param.Add("@TotalCuota", dbType: DbType.Int32, value: TotalCuota.Value);
                where += " AND C.TotalCuota = @TotalCuota ";
            }
            if (Importe.HasValue)
            {
                param.Add("@Importe", dbType: DbType.Decimal, value: Importe.Value);
                where += " AND C.Importe = @Importe ";
            }
            if (Anio.HasValue)
            {
                param.Add("@Anio", dbType: DbType.Decimal, value: Anio.Value);
                where += " AND YEAR(C.FechaVto) = @Anio ";
            }
            if (FechaVto.HasValue)
            {
                param.Add("@FechaVto", dbType: DbType.DateTime, value: FechaVto.Value);
                where += " AND C.FechaVto = @FechaVto ";
            }
            if (Importe2.HasValue)
            {
                param.Add("@Importe2", dbType: DbType.Decimal, value: Importe2.Value);
                where += " AND C.Importe2 = @Importe2 ";
            }
            if (FechaVto2.HasValue)
            {
                param.Add("@FechaVto2", dbType: DbType.DateTime, value: FechaVto2.Value);
                where += " AND C.FechaVto2 = @FechaVto2 ";
            }
            if (FechaPago.HasValue)
            {
                param.Add("@FechaPago", dbType: DbType.String, value: FechaPago.Value.ToShortDateString());
                where += " AND convert(varchar,c.FechaPago,103) = convert(varchar,convert(date,@FechaPago),101) ";
            }
            if (!string.IsNullOrEmpty(Estado))
            {
                param.Add("@Estado", dbType: DbType.String, value: Estado);
                where += " AND C.Estado = @Estado ";
            }
            if (NroRec.HasValue)
            {
                param.Add("@NroRec", dbType: DbType.Int32, value: NroRec.Value);
                where += " AND C.NroRec = @NroRec ";
            }
            if (FechaBaja.HasValue)
            {
                param.Add("@FechaBaja", dbType: DbType.DateTime, value: FechaBaja.Value);
                where += " AND C.FechaBaja = @FechaBaja ";
            }
            if (!String.IsNullOrEmpty(Motivo))
            {
                param.Add("@Motivo", dbType: DbType.String, value: Motivo);
                where += " AND C.Motivo LIKE '%@Motivo%' ";
            }
            if (!String.IsNullOrEmpty(NroFactura))
            {
                param.Add("@NroFactura", dbType: DbType.String, value: string.Format("%{0}%", NroFactura));
                where += " AND C.NroFactura LIKE @NroFactura";
            }
            if (!String.IsNullOrEmpty(NroFactura))
            {
                param.Add("@NroFactura", dbType: DbType.String, value: string.Format("%{0}%", NroFactura));
                where += " AND C.NroFactura LIKE @NroFactura";
            }
            if (!String.IsNullOrEmpty(descripcionCuota))
            {
                param.Add("@descripcionCuota", dbType: DbType.String, value: string.Format("%{0}%", descripcionCuota));
                where += " AND C.descripcionCuota LIKE @descripcionCuota";
            }
            if (!String.IsNullOrEmpty(banelcoDescripcionTicket))
            {
                param.Add("@banelcoDescripcionTicket", dbType: DbType.String, value: string.Format("%{0}%", banelcoDescripcionTicket));
                where += " AND C.banelcoDescripcionTicket LIKE @banelcoDescripcionTicket";
            }
            if (!String.IsNullOrEmpty(banelcoDescripcionPantalla))
            {
                param.Add("@banelcoDescripcionPantalla", dbType: DbType.String, value: string.Format("%{0}%", banelcoDescripcionPantalla));
                where += " AND C.banelcoDescripcionPantalla LIKE @banelcoDescripcionPantalla";
            }
            if (!String.IsNullOrEmpty(banelcoCodigo))
            {
                param.Add("@banelcoCodigo", dbType: DbType.String, value: string.Format("%{0}%", banelcoCodigo));
                where += " AND C.banelcoCodigo LIKE @banelcoCodigo";
            }

            if (banelco.HasValue)
            {
                param.Add("@banelco", dbType: DbType.Int32, value: banelco.Value);
                where += " AND C.banelco = @banelco ";
            }

            if (CodCon.HasValue)
            {
                param.Add("@CodCon", dbType: DbType.Int32, value: CodCon.Value);
                where += " AND C.CodCon = @CodCon ";
            }
            if (NroComision.HasValue)
            {
                param.Add("@NroComision", dbType: DbType.Int32, value: NroComision.Value);
                where += " AND C.NroComision = @NroComision ";
            }
            if (NroCurso.HasValue)
            {
                param.Add("@NroCurso", dbType: DbType.Int32, value: NroCurso.Value);
                where += " AND C.NroCurso = @NroCurso ";
            }
            if (ImporteDesde.HasValue)
            {
                param.Add("@ImporteDesde", dbType: DbType.Decimal, value: ImporteDesde.Value);
                where += " AND C.Importe > @ImporteDesde";
            }
            if (ImporteHasta.HasValue)
            {
                param.Add("@ImporteHasta", dbType: DbType.Decimal, value: ImporteHasta.Value);
                where += " AND C.Importe < @ImporteHasta";
            }
            if (Importe2Desde.HasValue)
            {
                param.Add("@Importe2Desde", dbType: DbType.Decimal, value: Importe2Desde.Value);
                where += " AND C.Importe2 > @Importe2Desde";
            }
            if (Importe2Hasta.HasValue)
            {
                param.Add("@Importe2Hasta", dbType: DbType.Decimal, value: Importe2Hasta.Value);
                where += " AND C.Importe < @Importe2Hasta";
            }
            if (FechaVtoDesde.HasValue)
            {
                param.Add("@FechaVtoDesde", dbType: DbType.DateTime, value: FechaVtoDesde.Value);
                where += " AND C.FechaVto > @FechaVtoDesde";
            }
            if (FechaVtoHasta.HasValue)
            {
                param.Add("@FechaVtoHasta", dbType: DbType.DateTime, value: FechaVtoHasta.Value);
                where += " AND C.FechaVto < @FechaVtoHasta";
            }
            if (FechaVto2Desde.HasValue)
            {
                param.Add("@FechaVto2Desde", dbType: DbType.DateTime, value: FechaVto2Desde.Value);
                where += " AND C.FechaVto2 > @FechaVto2Desde";
            }
            if (FechaVto2Hasta.HasValue)
            {
                param.Add("@FechaVto2Hasta", dbType: DbType.DateTime, value: FechaVto2Hasta.Value);
                where += " AND C.FechaVto2 < @FechaVto2Hasta";
            }
            if (FechaPagoDesde.HasValue)
            {
                param.Add("@FechaPagoDesde", dbType: DbType.DateTime, value: FechaPagoDesde.Value);
                where += " AND C.FechaPago > @FechaPagoDesde";
            }
            if (FechaPagoHasta.HasValue)
            {
                param.Add("@FechaPagoHasta", dbType: DbType.DateTime, value: FechaPagoHasta.Value);
                where += " AND C.FechPagoa < @FechaPagoHasta";
            }
            if (fechaImportadoDesde.HasValue)
            {
                param.Add("@fechaImportadoDesde", dbType: DbType.DateTime, value: fechaImportadoDesde.Value);
                where += " AND CAST(M.fechaImportado AS date) >= @fechaImportadoDesde";
            }
            if (fechaImportadoHasta.HasValue)
            {
                param.Add("@fechaImportadoHasta", dbType: DbType.DateTime, value: fechaImportadoHasta.Value);
                where += " AND CAST(M.fechaImportado AS date) <= @fechaImportadoHasta";
            }

            if (FechaBajaDesde.HasValue)
            {
                param.Add("@FechaBajaDesde", dbType: DbType.DateTime, value: FechaBajaDesde.Value);
                where += " AND C.FechaBaja >= @FechaBajaDesde";
            }
            if (FechaBajaHasta.HasValue)
            {
                param.Add("@FechaBajaHasta", dbType: DbType.DateTime, value: FechaBajaHasta.Value);
                where += " AND C.FechaBaja <= @FechaBajaHasta";
            }

            if (!string.IsNullOrEmpty(listadoComisiones))
            {
                var comisiones = listadoComisiones.Split('|');

                if (comisiones.Count() > 0)
                {
                    where += " AND C.NroComision IN (";

                    foreach (var comision in comisiones)
                    {
                        where += " " + comision;
                        if (comisiones.Last() != comision)
                        {
                            where += ", ";
                        }
                    }
                    where += " )";
                }
            }

            if (!string.IsNullOrEmpty(listadoCuotas))
            {
                var cuotas = listadoCuotas.Split('|');

                if (cuotas.Count() > 0)
                {
                    where += " AND C.Id IN (";

                    foreach (var cu in cuotas)
                    {
                        if (!string.IsNullOrEmpty(cu))
                        {
                            where += " " + cu;
                            if (cuotas.Last() != cu)
                            {
                                where += ", ";
                            }
                        }
                    }
                    where += " )";
                }
            }

            string query = QUERY.Replace(Constantes.WHERE, where);
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Cuota>(query, param).ToList();
            }
        }

        public static List<Cuota> BuscarMenor(string origen = ""
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
                                       , int? banelco = null
                                       , string banelcoDescripcionPantalla = ""
                                       , string banelcoDescripcionTicket = ""
                                       , string descripcionCuota = ""
                                       , string banelcoCodigo = ""
                                       , DateTime? fechaImportadoDesde = null
                                       , DateTime? fechaImportadoHasta = null
           )
        {
            string where = @"WHERE 1 = 1";
            DynamicParameters param = new DynamicParameters();

            if (!string.IsNullOrEmpty(origen))
            {
                param.Add("@origen", dbType: DbType.String, value: origen);
                where += " AND C.origen = @origen ";
            }
            if (Dni.HasValue)
            {
                param.Add("@Dni", dbType: DbType.Int64, value: Dni.Value);
                where += " AND C.Dni = @Dni ";
            }
            if (CodPlan.HasValue)
            {
                param.Add("@CodPlan", dbType: DbType.Int32, value: CodPlan.Value);
                where += " AND C.CodPlan = @CodPlan ";
            }
            if (NroCuota.HasValue)
            {
                param.Add("@NroCuota", dbType: DbType.Int32, value: NroCuota.Value);
                where += " AND C.NroCuota = @NroCuota ";
            }
            if (NroCuotaDesde.HasValue)
            {
                param.Add("@NroCuotaDesde", dbType: DbType.Int32, value: NroCuotaDesde.Value);
                where += " AND C.NroCuota >= @NroCuotaDesde ";
            }
            if (NroCuotaHasta.HasValue)
            {
                param.Add("@NroCuotaHasta", dbType: DbType.Int32, value: NroCuotaHasta.Value);
                where += " AND C.NroCuota <= @NroCuotaHasta ";
            }
            if (TotalCuota.HasValue)
            {
                param.Add("@TotalCuota", dbType: DbType.Int32, value: TotalCuota.Value);
                where += " AND C.TotalCuota = @TotalCuota ";
            }
            if (Importe.HasValue)
            {
                param.Add("@Importe", dbType: DbType.Decimal, value: Importe.Value);
                where += " AND C.Importe = @Importe ";
            }
            if (Anio.HasValue)
            {
                param.Add("@Anio", dbType: DbType.Decimal, value: Anio.Value);
                where += " AND YEAR(C.FechaVto) = @Anio ";
            }
            if (FechaVto.HasValue)
            {
                param.Add("@FechaVto", dbType: DbType.DateTime, value: FechaVto.Value);
                where += " AND C.FechaVto = @FechaVto ";
            }
            if (Importe2.HasValue)
            {
                param.Add("@Importe2", dbType: DbType.Decimal, value: Importe2.Value);
                where += " AND C.Importe2 = @Importe2 ";
            }
            if (FechaVto2.HasValue)
            {
                param.Add("@FechaVto2", dbType: DbType.DateTime, value: FechaVto2.Value);
                where += " AND C.FechaVto2 = @FechaVto2 ";
            }
            if (FechaPago.HasValue)
            {
                param.Add("@FechaPago", dbType: DbType.String, value: FechaPago.Value.ToShortDateString());
                where += " AND convert(varchar,c.FechaPago,103) <= convert(varchar,convert(date,@FechaPago),101) ";
            }
            if (!string.IsNullOrEmpty(Estado))
            {
                param.Add("@Estado", dbType: DbType.String, value: Estado);
                where += " AND C.Estado = @Estado ";
            }
            if (NroRec.HasValue)
            {
                param.Add("@NroRec", dbType: DbType.Int32, value: NroRec.Value);
                where += " AND C.NroRec = @NroRec ";
            }
            if (FechaBaja.HasValue)
            {
                param.Add("@FechaBaja", dbType: DbType.DateTime, value: FechaBaja.Value);
                where += " AND C.FechaBaja = @FechaBaja ";
            }
            if (!String.IsNullOrEmpty(Motivo))
            {
                param.Add("@Motivo", dbType: DbType.String, value: Motivo);
                where += " AND C.Motivo LIKE '%@Motivo%' ";
            }
            if (!String.IsNullOrEmpty(NroFactura))
            {
                param.Add("@NroFactura", dbType: DbType.String, value: string.Format("%{0}%", NroFactura));
                where += " AND C.NroFactura LIKE @NroFactura";
            }
            if (!String.IsNullOrEmpty(NroFactura))
            {
                param.Add("@NroFactura", dbType: DbType.String, value: string.Format("%{0}%", NroFactura));
                where += " AND C.NroFactura LIKE @NroFactura";
            }
            if (!String.IsNullOrEmpty(descripcionCuota))
            {
                param.Add("@descripcionCuota", dbType: DbType.String, value: string.Format("%{0}%", descripcionCuota));
                where += " AND C.descripcionCuota LIKE @descripcionCuota";
            }
            if (!String.IsNullOrEmpty(banelcoDescripcionTicket))
            {
                param.Add("@banelcoDescripcionTicket", dbType: DbType.String, value: string.Format("%{0}%", banelcoDescripcionTicket));
                where += " AND C.banelcoDescripcionTicket LIKE @banelcoDescripcionTicket";
            }
            if (!String.IsNullOrEmpty(banelcoDescripcionPantalla))
            {
                param.Add("@banelcoDescripcionPantalla", dbType: DbType.String, value: string.Format("%{0}%", banelcoDescripcionPantalla));
                where += " AND C.banelcoDescripcionPantalla LIKE @banelcoDescripcionPantalla";
            }
            if (!String.IsNullOrEmpty(banelcoCodigo))
            {
                param.Add("@banelcoCodigo", dbType: DbType.String, value: string.Format("%{0}%", banelcoCodigo));
                where += " AND C.banelcoCodigo LIKE @banelcoCodigo";
            }

            if (banelco.HasValue)
            {
                param.Add("@banelco", dbType: DbType.Int32, value: banelco.Value);
                where += " AND C.banelco = @banelco ";
            }

            if (CodCon.HasValue)
            {
                param.Add("@CodCon", dbType: DbType.Int32, value: CodCon.Value);
                where += " AND C.CodCon = @CodCon ";
            }
            if (NroComision.HasValue)
            {
                param.Add("@NroComision", dbType: DbType.Int32, value: NroComision.Value);
                where += " AND C.NroComision = @NroComision ";
            }
            if (NroCurso.HasValue)
            {
                param.Add("@NroCurso", dbType: DbType.Int32, value: NroCurso.Value);
                where += " AND C.NroCurso = @NroCurso ";
            }
            if (ImporteDesde.HasValue)
            {
                param.Add("@ImporteDesde", dbType: DbType.Decimal, value: ImporteDesde.Value);
                where += " AND C.Importe > @ImporteDesde";
            }
            if (ImporteHasta.HasValue)
            {
                param.Add("@ImporteHasta", dbType: DbType.Decimal, value: ImporteHasta.Value);
                where += " AND C.Importe < @ImporteHasta";
            }
            if (Importe2Desde.HasValue)
            {
                param.Add("@Importe2Desde", dbType: DbType.Decimal, value: Importe2Desde.Value);
                where += " AND C.Importe2 > @Importe2Desde";
            }
            if (Importe2Hasta.HasValue)
            {
                param.Add("@Importe2Hasta", dbType: DbType.Decimal, value: Importe2Hasta.Value);
                where += " AND C.Importe < @Importe2Hasta";
            }
            if (FechaVtoDesde.HasValue)
            {
                param.Add("@FechaVtoDesde", dbType: DbType.DateTime, value: FechaVtoDesde.Value);
                where += " AND C.FechaVto > @FechaVtoDesde";
            }
            if (FechaVtoHasta.HasValue)
            {
                param.Add("@FechaVtoHasta", dbType: DbType.DateTime, value: FechaVtoHasta.Value);
                where += " AND C.FechaVto < @FechaVtoHasta";
            }
            if (FechaVto2Desde.HasValue)
            {
                param.Add("@FechaVto2Desde", dbType: DbType.DateTime, value: FechaVto2Desde.Value);
                where += " AND C.FechaVto2 > @FechaVto2Desde";
            }
            if (FechaVto2Hasta.HasValue)
            {
                param.Add("@FechaVto2Hasta", dbType: DbType.DateTime, value: FechaVto2Hasta.Value);
                where += " AND C.FechaVto2 < @FechaVto2Hasta";
            }
            if (FechaPagoDesde.HasValue)
            {
                param.Add("@FechaPagoDesde", dbType: DbType.DateTime, value: FechaPagoDesde.Value);
                where += " AND C.FechaPago > @FechaPagoDesde";
            }
            if (FechaPagoHasta.HasValue)
            {
                param.Add("@FechaPagoHasta", dbType: DbType.DateTime, value: FechaPagoHasta.Value);
                where += " AND C.FechPagoa < @FechaPagoHasta";
            }
            if (fechaImportadoDesde.HasValue)
            {
                param.Add("@fechaImportadoDesde", dbType: DbType.DateTime, value: fechaImportadoDesde.Value);
                where += " AND CAST(M.fechaImportado AS date) >= @fechaImportadoDesde";
            }
            if (fechaImportadoHasta.HasValue)
            {
                param.Add("@fechaImportadoHasta", dbType: DbType.DateTime, value: fechaImportadoHasta.Value);
                where += " AND CAST(M.fechaImportado AS date) <= @fechaImportadoHasta";
            }

            if (FechaBajaDesde.HasValue)
            {
                param.Add("@FechaBajaDesde", dbType: DbType.DateTime, value: FechaBajaDesde.Value);
                where += " AND C.FechaBaja >= @FechaBajaDesde";
            }
            if (FechaBajaHasta.HasValue)
            {
                param.Add("@FechaBajaHasta", dbType: DbType.DateTime, value: FechaBajaHasta.Value);
                where += " AND C.FechaBaja <= @FechaBajaHasta";
            }

            if (!string.IsNullOrEmpty(listadoComisiones))
            {
                var comisiones = listadoComisiones.Split('|');

                if (comisiones.Count() > 0)
                {
                    where += " AND C.NroComision IN (";

                    foreach (var comision in comisiones)
                    {
                        where += " " + comision;
                        if (comisiones.Last() != comision)
                        {
                            where += ", ";
                        }
                    }
                    where += " )";
                }
            }

            if (!string.IsNullOrEmpty(listadoCuotas))
            {
                var cuotas = listadoCuotas.Split('|');

                if (cuotas.Count() > 0)
                {
                    where += " AND C.Id IN (";

                    foreach (var cu in cuotas)
                    {
                        if (!string.IsNullOrEmpty(cu))
                        {
                            where += " " + cu;
                            if (cuotas.Last() != cu)
                            {
                                where += ", ";
                            }
                        }
                    }
                    where += " )";
                }
            }

            string query = QUERY.Replace(Constantes.WHERE, where);
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Cuota>(query, param).ToList();
            }
        }

        public static List<Cuota> LeerPorCursada(int NroFactura, string origen)
        {
            string where = @"WHERE 1 = 1";
            DynamicParameters param = new DynamicParameters();

            //param.Add("@NroFactura", dbType: DbType.String, value: NroFactura);
            //where += " AND C.NroFactura LIKE '@NroFactura_%' ";

            //param.Add("@origen", dbType: DbType.String, value: origen);
            //where += " AND C.Origen = '@origen' ";


            where += "AND C.NroFactura LIKE '" + NroFactura.ToString() + "_%' AND C.Origen = '" + origen + "' ";
            string query = QUERY.Replace(Constantes.WHERE, where);
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Cuota>(query).ToList();
            }
        }

        public static List<Cuota> LeerPorComision(int NroComision, string origen)
        {
            string where = @"WHERE 1 = 1";
            DynamicParameters param = new DynamicParameters();

            param.Add("@NroComision", dbType: DbType.Int32, value: NroComision);
            where += " AND C.NroComision = @NroComision ";

            param.Add("@origen", dbType: DbType.String, value: origen);
            where += " AND C.Origen = @origen ";

            string query = QUERY.Replace(Constantes.WHERE, where);
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Cuota>(query, param).ToList();
            }
        }

        public static Cuota LeerUno(long id)
        {
            var param = new DynamicParameters();
            param.Add("@Id", dbType: DbType.Int64, value: id);
            string query = QUERY.Replace(Constantes.WHERE, @"
                WHERE
                    C.Id = @Id 
            ");
            using (var db = new SqlConnection(conexion))
            {
                Cuota cuota = db.Query<Cuota>(query, param).FirstOrDefault();
                return cuota;
            }
        }

        public static Cuota LeerUno(string id)
        {
            var param = new DynamicParameters();
            param.Add("@Id", dbType: DbType.String, value: id);
            string query = QUERY.Replace(Constantes.WHERE, @"
                WHERE
                    C.NroFactura = @Id 
            ");
            using (var db = new SqlConnection(conexion))
            {
                Cuota cuota = db.Query<Cuota>(query, param).FirstOrDefault();
                return cuota;
            }
        }

        public static Cuota LeerUno(string NroFactura, string origen)
        {
            var param = new DynamicParameters();
            param.Add("@NroFactura", dbType: DbType.String, value: NroFactura);
            param.Add("@origen", dbType: DbType.String, value: origen);
            string query = QUERY.Replace(Constantes.WHERE, @"
                WHERE
                    C.NroFactura = @NroFactura AND C.origen = @origen 
            ");
            using (var db = new SqlConnection(conexion))
            {
                Cuota cuota = db.Query<Cuota>(query, param).FirstOrDefault();
                return cuota;
            }
        }

        public static bool TieneDeuda(long dni)
        {
            var param = new DynamicParameters();
            param.Add("@Dni", dbType: DbType.Int64, value: dni);
            string query = QUERY.Replace(Constantes.WHERE, @"
                WHERE
                    C.Dni = @Dni
                AND C.Estado = '0' 
            ");
            using (var db = new SqlConnection(conexion))
            {
                var cuotas = db.Query<Cuota>(query, param).ToList();
                return (cuotas.Count > 0);
            }
        }

        public static bool TieneDeudaVencida(long dni, string origen, DateTime Fecha, bool? SegundoVencimiento = null)
        {
            string where = ""; // @"WHERE 1 = 1";
            var param = new DynamicParameters();
            param.Add("@Dni", dbType: DbType.Int64, value: dni);
            param.Add("@Fecha", dbType: DbType.DateTime, value: Fecha);


            where += @"
                WHERE
                    C.Dni = @Dni
                AND C.Estado = '0'
                AND C.FechaVto < @Fecha
            ";
            if (SegundoVencimiento.HasValue && SegundoVencimiento.Value == true)
            {
                where += " AND C.FechaVto2 < @Fecha";
            }
            if (!string.IsNullOrEmpty(origen))
            {
                param.Add("@Origen", dbType: DbType.String, value: origen);
                where += " AND C.Origen = @Origen";
            }

            string query = QUERY.Replace(Constantes.WHERE, where);

            using (var db = new SqlConnection(conexion))
            {
                var cuotas = db.Query<Cuota>(query, param).ToList();
                return (cuotas.Count > 0);
            }
        }
        public static long Insert(Cuota cuota)
        {
            var param = new DynamicParameters();
            param.Add("@CodCon", dbType: DbType.Int32, value: cuota.CodCon);
            param.Add("@CodPlan", dbType: DbType.Int32, value: cuota.CodPlan);
            param.Add("@fechaBaja", dbType: DbType.DateTime, value: (cuota.fechaBaja < new DateTime(1900, 1, 1)) ? new DateTime(1900, 1, 1) : cuota.fechaBaja);
            param.Add("@fechaPago", dbType: DbType.DateTime, value: (cuota.fechaPago < new DateTime(1900, 1, 1)) ? new DateTime(1900, 1, 1) : cuota.fechaPago);
            param.Add("@fechavto", dbType: DbType.DateTime, value: (cuota.fechavto < new DateTime(1900, 1, 1)) ? new DateTime(1900, 1, 1) : cuota.fechavto);
            param.Add("@fechavto2", dbType: DbType.DateTime, value: (cuota.fechavto2 < new DateTime(1900, 1, 1)) ? new DateTime(1900, 1, 1) : cuota.fechavto2);
            param.Add("@Importe", dbType: DbType.Decimal, value: cuota.Importe);
            param.Add("@Importe2", dbType: DbType.Decimal, value: cuota.Importe2);
            param.Add("@Motivo", dbType: DbType.String, value: cuota.Motivo);
            param.Add("@NroFactura", dbType: DbType.String, value: cuota.NroFactura);

            param.Add("@NroComision", dbType: DbType.Int32, value: cuota.NroComision);
            param.Add("@NroCuota", dbType: DbType.Int32, value: cuota.NroCuota);
            param.Add("@NroCurso", dbType: DbType.Int32, value: cuota.NroCurso);
            param.Add("@NroRec", dbType: DbType.Int32, value: cuota.NroRec);
            param.Add("@TotalCuota", dbType: DbType.Int32, value: cuota.TotalCuota);
            param.Add("@Dni", dbType: DbType.Int32, value: cuota.Dni);
            param.Add("@Estado", dbType: DbType.String, value: cuota.Estado);
            param.Add("@Origen", dbType: DbType.String, value: cuota.Origen);
            param.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

            param.Add("@Usuario", dbType: DbType.String, value: cuota.Usuario);
            param.Add("@FechaLog", dbType: DbType.DateTime, value: DateTime.Now);
            param.Add("@descripcionCuota", dbType: DbType.String, value: cuota.DescripcionCuota);

            param.Add("@banelco", dbType: DbType.Int32, value: cuota.banelco);
            param.Add("@banelcoDescripcionPantalla", dbType: DbType.String, value: cuota.banelcoDescripcionPantalla);
            param.Add("@banelcoDescripcionTicket", dbType: DbType.String, value: cuota.banelcoDescripcionTicket);
            param.Add("@banelcoCodigo", dbType: DbType.String, value: cuota.banelcoCodigo);


            const string SQL_QUERY = @"
                INSERT dbo.Cuota(
                        Dni
                       ,CodPlan
                       ,NroCuota
                       ,TotalCuota
                       ,Importe
                       ,FechaVto
                       ,Importe2
                       ,FechaVto2
                       ,FechaPago
                       ,Estado
                       ,NroRec
                       ,FechaBaja
                       ,Motivo
                       ,CodCon
                       ,NroComision
                       ,NroCurso
                       ,Origen
                       ,NroFactura
                       ,Usuario
                       ,FechaLog
                       ,descripcionCuota
                       ,banelco
                       ,banelcoDescripcionPantalla
                       ,banelcoDescripcionTicket
                       ,banelcoCodigo
                    )
                VALUES(
                    @Dni
                   ,@CodPlan
                   ,@NroCuota
                   ,@TotalCuota
                   ,@Importe
                   ,@FechaVto
                   ,@Importe2
                   ,@FechaVto2
                   ,@FechaPago
                   ,@Estado
                   ,@NroRec
                   ,@FechaBaja
                   ,@Motivo
                   ,@CodCon
                   ,@NroComision
                   ,@NroCurso
                   ,@Origen
                   ,@NroFactura
                   ,@Usuario
                   ,@FechaLog
                   ,@descripcionCuota
                   ,@banelco
                   ,@banelcoDescripcionPantalla
                   ,@banelcoDescripcionTicket
                   ,@banelcoCodigo
                );
                SET @ID = SCOPE_IDENTITY();
            ";
            using (var db = new SqlConnection(conexion))
            {
                db.Execute(SQL_QUERY, param);
                return (param.Get<long>("@Id"));

            }
        }
        
        public static void Update(Cuota cuota)
        {
            var param = new DynamicParameters();
            param.Add("@CodCon", dbType: DbType.Int32, value: cuota.CodCon);
            param.Add("@CodPlan", dbType: DbType.Int32, value: cuota.CodPlan);
            param.Add("@fechaBaja", dbType: DbType.DateTime, value: (cuota.fechaBaja < new DateTime(1900, 1, 1)) ? new DateTime(1900, 1, 1) : cuota.fechaBaja);
            param.Add("@fechaPago", dbType: DbType.DateTime, value: (cuota.fechaPago < new DateTime(1900, 1, 1)) ? new DateTime(1900, 1, 1) : cuota.fechaPago);
            param.Add("@fechavto", dbType: DbType.DateTime, value: (cuota.fechavto < new DateTime(1900, 1, 1)) ? new DateTime(1900, 1, 1) : cuota.fechavto);
            param.Add("@fechavto2", dbType: DbType.DateTime, value: (cuota.fechavto2 < new DateTime(1900, 1, 1)) ? new DateTime(1900, 1, 1) : cuota.fechavto2);
            param.Add("@Importe", dbType: DbType.Decimal, value: cuota.Importe);
            param.Add("@Importe2", dbType: DbType.Decimal, value: cuota.Importe2);
            param.Add("@Motivo", dbType: DbType.String, value: cuota.Motivo);
            param.Add("@NroFactura", dbType: DbType.String, value: cuota.NroFactura);
            param.Add("@NroComision", dbType: DbType.Int32, value: cuota.NroComision);
            param.Add("@NroCuota", dbType: DbType.Int32, value: cuota.NroCuota);
            param.Add("@NroCurso", dbType: DbType.Int32, value: cuota.NroCurso);
            param.Add("@NroRec", dbType: DbType.Int32, value: cuota.NroRec);
            param.Add("@TotalCuota", dbType: DbType.Int32, value: cuota.TotalCuota);
            param.Add("@Dni", dbType: DbType.Int32, value: cuota.Dni);
            param.Add("@Estado", dbType: DbType.String, value: cuota.Estado);
            param.Add("@Id", dbType: DbType.Int64, value: cuota.Id);
            param.Add("@Origen", dbType: DbType.String, value: cuota.Origen);
            param.Add("@Usuario", dbType: DbType.String, value: cuota.Usuario);
            param.Add("@FechaLog", dbType: DbType.DateTime, value: DateTime.Now);

            param.Add("@descripcionCuota", dbType: DbType.String, value: cuota.DescripcionCuota);
            param.Add("@banelco", dbType: DbType.Int16, value: cuota.banelco);
            param.Add("@banelcoDescripcionPantalla", dbType: DbType.String, value: cuota.banelcoDescripcionPantalla);
            param.Add("@banelcoDescripcionTicket", dbType: DbType.String, value: cuota.banelcoDescripcionTicket);
            param.Add("@banelcoCodigo", dbType: DbType.String, value: cuota.banelcoCodigo);


            const string SQL_QUERY = @"
                UPDATE
                    dbo.Cuota
                SET 
	               Dni = @Dni
                  ,CodPlan = @CodPlan
                  ,NroCuota = @NroCuota
                  ,TotalCuota = @TotalCuota
                  ,Importe = @Importe
                  ,FechaVto = @FechaVto
                  ,Importe2 = @Importe2
                  ,FechaVto2 = @FechaVto2
                  ,FechaPago = @FechaPago
                  ,Estado = @Estado
                  ,NroRec = @NroRec
                  ,FechaBaja = @FechaBaja
                  ,Motivo = @Motivo
                  ,CodCon = @CodCon
                  ,NroComision = @NroComision
                  ,NroCurso = @NroCurso
                  ,Origen = @Origen
                  ,NroFactura = @NroFactura
                  ,Usuario  = @Usuario
                  ,FechaLog = @FechaLog
                  ,banelco = @banelco
                  ,banelcoDescripcionPantalla = @banelcoDescripcionPantalla
                  ,banelcoDescripcionTicket = @banelcoDescripcionTicket
                  ,descripcionCuota = @descripcionCuota
                  ,banelcoCodigo = @banelcoCodigo
                WHERE
	                 Id = @Id
            ";
            using (var db = new SqlConnection(conexion))
            {
                db.Execute(SQL_QUERY, param);
            }
        }

        public static void Delete(long id)
        {
            var param = new DynamicParameters();
            param.Add("@Id", dbType: DbType.Int64, value: id);
            const string SQL_QUERY = @"
            DELETE
                dbo.Cuota
            WHERE
	            Id = @Id
            ";
            using (var db = new SqlConnection(conexion))
            {
                db.Execute(SQL_QUERY, param);
            }
        }
    }
}