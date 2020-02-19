
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
    public class CuponData
    {
        private static readonly string conexion = ConfigurationManager.ConnectionStrings[Constantes.CONN].ConnectionString;

        const string QUERY = @"
            SELECT
                     				  C.id
                  ,C.Dni
                  ,C.CodPlan
                  ,C.NroCuota
                  ,C.TotalCuota
                  ,C.Importe
                  ,C.FechaVto
                  ,C.FechaVto2
                  ,CONVERT(VARCHAR(10), C.FechaVto, 111) as fechaVencimiento 
                  ,C.Importe2
                  ,CONVERT(VARCHAR(10), C.FechaVto2, 111) as fechaVencimiento2 
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
				  ,A.ApeyNom Nombre 
				  ,A.Domicilio Domicilio 
				  ,'-' Localidad 
				  ,C.descripcionCuota CursoNombre 
				  ,CONVERT(VARCHAR(10), GETDATE(), 103) as Fecha
            FROM cuota C
				  LEFT JOIN Alumno A on A.Dni = C.Dni
				  LEFT JOIN Curso CU on CU.NroCurso = C.NroCurso
            {WHERE} 
            ";

        const string QUERYestadosCuotas = @"
            SELECT
                     				  C.id
                  ,C.cuota_id
                  ,C.estado
                  ,C.fecha
                  
            FROM EstadosCuotas C
				  {WHERE} 
            ";

        public static List<Cupon> LeerTodo()
        {
            var query = QUERY.Replace(Constantes.WHERE, "");
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Cupon>(query).ToList();
            }
        }

        public static List<Cupon> Buscar(string origen
                                        , string NroFactura
            )
        {
            string where = @"WHERE 1 = 1 AND Estado = '0' ";
            DynamicParameters param = new DynamicParameters();

            if (!string.IsNullOrEmpty(origen))
            {
                param.Add("@origen", dbType: DbType.String, value: origen);
                where += " AND C.origen = @origen ";
            }
            if (!String.IsNullOrEmpty(NroFactura))
            {
                param.Add("@NroFactura", dbType: DbType.String, value: NroFactura);
                where += " AND C.NroFactura = @NroFactura ";
            }

            string query = QUERY.Replace(Constantes.WHERE, where);
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Cupon>(query, param).ToList();
            }
        }

        public static List<Cupon> BuscarEstadosCuotas(string id)
        {
            string where = @"WHERE 1 = 1 ";
            DynamicParameters param = new DynamicParameters();

            if (!string.IsNullOrEmpty(id))
            {
                param.Add("@id", dbType: DbType.String, value: id);
                where += " AND C.cuota_id = @id ";
            }

            string query = QUERYestadosCuotas.Replace(Constantes.WHERE, where);
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Cupon>(query, param).ToList();
            }
        }

        public static List<Cupon> Buscar(long idCuota)
        {
            string where = @"WHERE 1 = 1";
            DynamicParameters param = new DynamicParameters();


            param.Add("@idCuota", dbType: DbType.Int64, value: idCuota);
            where += " AND C.id = @idCuota ";


            string query = QUERY.Replace(Constantes.WHERE, where);
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Cupon>(query, param).ToList();
            }
        }
        //public static Cuota LeerUno(long id)
        //{
        //    var param = new DynamicParameters();
        //    param.Add("@Id", dbType: DbType.Int64, value: id);
        //    string query = QUERY.Replace(Constantes.WHERE, @"
        //        WHERE
        //            C.Id = @Id 
        //    ");
        //    using (var db = new SqlConnection(conexion))
        //    {
        //        Cuota cuota = db.Query<Cuota>(query, param).FirstOrDefault();
        //        return cuota;
        //    }
        //}

        //public static Cuota LeerUno(string NroFactura, string origen)
        //{
        //    var param = new DynamicParameters();
        //    param.Add("@NroFactura", dbType: DbType.String, value: NroFactura);
        //    param.Add("@origen", dbType: DbType.String, value: origen);
        //    string query = QUERY.Replace(Constantes.WHERE, @"
        //        WHERE
        //            C.NroFactura = @NroFactura AND C.origen = @origen 
        //    ");
        //    using (var db = new SqlConnection(conexion))
        //    {
        //        Cuota cuota = db.Query<Cuota>(query, param).FirstOrDefault();
        //        return cuota;
        //    }
        //}

        //public static bool TieneDeuda(long dni)
        //{
        //    var param = new DynamicParameters();
        //    param.Add("@Dni", dbType: DbType.Int64, value: dni);
        //    string query = QUERY.Replace(Constantes.WHERE, @"
        //        WHERE
        //            C.Dni = @Dni
        //        AND C.Estado = '0' 
        //    ");
        //    using (var db = new SqlConnection(conexion))
        //    {
        //        var cuotas = db.Query<Cuota>(query, param).ToList();
        //        return (cuotas.Count > 0);
        //    }
        //}

        //public static bool TieneDeudaVencida(long dni, DateTime Fecha, bool? SegundoVencimiento = null)
        //{
        //    string where = @"WHERE 1 = 1";
        //    var param = new DynamicParameters();
        //    param.Add("@Dni", dbType: DbType.Int64, value: dni);
        //    param.Add("@Fecha", dbType: DbType.DateTime, value: Fecha);
        //    if (SegundoVencimiento.HasValue && SegundoVencimiento.Value == true)
        //    {
        //        where += " AND C.FechaVto2 < @Fecha";
        //    }

        //    where += @"
        //        WHERE
        //            C.Dni = @Dni
        //        AND C.Estado = '0'
        //        AND C.FechaVto < @Fecha
        //    ";

        //    string query = QUERY.Replace(Constantes.WHERE, where);

        //    using (var db = new SqlConnection(conexion))
        //    {
        //        var cuotas = db.Query<Cuota>(query, param).ToList();
        //        return (cuotas.Count > 0);
        //    }
        //}
        //public static long Insert(Cuota cuota)
        //{
        //    var param = new DynamicParameters();
        //    param.Add("@CodCon", dbType: DbType.Int32, value: cuota.CodCon);
        //    param.Add("@CodPlan", dbType: DbType.Int32, value: cuota.CodPlan);
        //    param.Add("@fechaBaja", dbType: DbType.DateTime, value: cuota.fechaBaja);
        //    param.Add("@fechaPago", dbType: DbType.DateTime, value: cuota.fechaPago);
        //    param.Add("@fechavto", dbType: DbType.DateTime, value: cuota.fechavto);
        //    param.Add("@fechavto2", dbType: DbType.DateTime, value: cuota.fechavto2);
        //    param.Add("@Importe", dbType: DbType.Decimal, value: cuota.Importe);
        //    param.Add("@Importe2", dbType: DbType.Decimal, value: cuota.Importe2);
        //    param.Add("@Motivo", dbType: DbType.String, value: cuota.Motivo);
        //    param.Add("@NroFactura", dbType: DbType.String, value: cuota.NroFactura);

        //    param.Add("@NroComision", dbType: DbType.Int32, value: cuota.NroComision);
        //    param.Add("@NroCuota", dbType: DbType.Int32, value: cuota.NroCuota);
        //    param.Add("@NroCurso", dbType: DbType.Int32, value: cuota.NroCurso);
        //    param.Add("@NroRec", dbType: DbType.Int32, value: cuota.NroRec);
        //    param.Add("@TotalCuota", dbType: DbType.Int32, value: cuota.TotalCuota);
        //    param.Add("@Dni", dbType: DbType.Int32, value: cuota.Dni);
        //    param.Add("@Estado", dbType: DbType.String, value: cuota.Estado);
        //    param.Add("@Origen", dbType: DbType.String, value: cuota.Origen);
        //    param.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
        //    const string SQL_QUERY = @"
        //        INSERT dbo.Cuota(
        //                Dni
        //               ,CodPlan
        //               ,NroCuota
        //               ,TotalCuota
        //               ,Importe
        //               ,FechaVto
        //               ,Importe2
        //               ,FechaVto2
        //               ,FechaPago
        //               ,Estado
        //               ,NroRec
        //               ,FechaBaja
        //               ,Motivo
        //               ,CodCon
        //               ,NroComision
        //               ,NroCurso
        //               ,Origen
        //               ,NroFactura
        //            )
        //        VALUES(
        //            @Dni
        //           ,@CodPlan
        //           ,@NroCuota
        //           ,@TotalCuota
        //           ,@Importe
        //           ,@FechaVto
        //           ,@Importe2
        //           ,@FechaVto2
        //           ,@FechaPago
        //           ,@Estado
        //           ,@NroRec
        //           ,@FechaBaja
        //           ,@Motivo
        //           ,@CodCon
        //           ,@NroComision
        //           ,@NroCurso
        //           ,@Origen
        //           ,@NroFactura
        //        );
        //        SET @ID = SCOPE_IDENTITY() AS BIGINT
        //    ";
        //    using (var db = new SqlConnection(conexion))
        //    {
        //        db.Execute(SQL_QUERY, param);
        //        return (param.Get<long>("@ID"));

        //    }
        //}

        //public static void Update(Cuota cuota)
        //{
        //    var param = new DynamicParameters();
        //    param.Add("@CodCon", dbType: DbType.Int32, value: cuota.CodCon);
        //    param.Add("@CodPlan", dbType: DbType.Int32, value: cuota.CodPlan);
        //    param.Add("@fechaBaja", dbType: DbType.DateTime, value: cuota.fechaBaja);
        //    param.Add("@fechaPago", dbType: DbType.DateTime, value: cuota.fechaPago);
        //    param.Add("@fechavto", dbType: DbType.DateTime, value: cuota.fechavto);
        //    param.Add("@fechavto2", dbType: DbType.DateTime, value: cuota.fechavto2);
        //    param.Add("@Importe", dbType: DbType.Decimal, value: cuota.Importe);
        //    param.Add("@Importe2", dbType: DbType.Decimal, value: cuota.Importe2);
        //    param.Add("@Motivo", dbType: DbType.String, value: cuota.Motivo);
        //    param.Add("@NroFactura", dbType: DbType.String, value: cuota.NroFactura);
        //    param.Add("@NroComision", dbType: DbType.Int32, value: cuota.NroComision);
        //    param.Add("@NroCuota", dbType: DbType.Int32, value: cuota.NroCuota);
        //    param.Add("@NroCurso", dbType: DbType.Int32, value: cuota.NroCurso);
        //    param.Add("@NroRec", dbType: DbType.Int32, value: cuota.NroRec);
        //    param.Add("@TotalCuota", dbType: DbType.Int32, value: cuota.TotalCuota);
        //    param.Add("@Dni", dbType: DbType.Int32, value: cuota.Dni);
        //    param.Add("@Estado", dbType: DbType.String, value: cuota.Estado);
        //    param.Add("@Id", dbType: DbType.Int64, value: cuota.Id);
        //    param.Add("@Origen", dbType: DbType.String, value: cuota.Origen);

        //    const string SQL_QUERY = @"
        //        UPDATE
        //            dbo.Cuota
        //        SET 
        //        Dni = @Dni
        //          ,CodPlan = @CodPlan
        //          ,NroCuota = @NroCuota
        //          ,TotalCuota = @TotalCuota
        //          ,Importe = @Importe
        //          ,FechaVto = @FechaVto
        //          ,Importe2 = @Importe2
        //          ,FechaVto2 = @FechaVto2
        //          ,FechaPago = @FechaPago
        //          ,Estado = @Estado
        //          ,NroRec = @NroRec
        //          ,FechaBaja = @FechaBaja
        //          ,Motivo = @Motivo
        //          ,CodCon = @CodCon
        //          ,NroComision = @NroComision
        //          ,NroCurso = @NroCurso
        //          ,Origen = @Origen
        //          ,NroFactura = @NroFactura
        //        WHERE
        //          Id = @Id
        //    ";
        //    using (var db = new SqlConnection(conexion))
        //    {
        //        db.Execute(SQL_QUERY, param);
        //    }
        //}

        //public static void Delete(long id)
        //{
        //    var param = new DynamicParameters();
        //    param.Add("@Id", dbType: DbType.Int64, value: id);
        //    const string SQL_QUERY = @"
        //    DELETE
        //        dbo.Cuota
        //    WHERE
        //     Id = @Id
        //    ";
        //    using (var db = new SqlConnection(conexion))
        //    {
        //        db.Execute(SQL_QUERY, param);
        //    }
        //}
    }
}