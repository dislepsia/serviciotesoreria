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
    public class ActividadData
    {
        private static readonly string conexion = ConfigurationManager.ConnectionStrings[Constantes.CONN].ConnectionString;

        const string QUERY = @"
            SELECT A.Origen
                  ,A.ApeyNom
                  ,A.Dni
                  ,A.Control
                  ,A.Fecha
                  ,A.CodCon
                  ,A.NroRec
                  ,A.Id
                  ,A.Importe,
              FROM Actividad A
            {WHERE} 
            ";
        
        public static List<Actividad> LeerTodo()
        {
            var query = QUERY.Replace(Constantes.WHERE, "");
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Actividad>(query).ToList();
            }
        }

        public static List<Actividad> Buscar( string origen = ""
                                        ,long? Dni = null
                                        , decimal? Importe = null
                                        , DateTime? Fecha = null
                                        ,int? NroRec = null
                                        ,int? CodCon = null
                                        ,int? Control = null
                                        , decimal? ImporteDesde = null
                                        , decimal? ImporteHasta = null
                                        , DateTime? FechaDesde = null
                                        , DateTime? FechaHasta = null
                                        
            
            )
        {
            string where = @"WHERE 1 = 1";
            DynamicParameters param = new DynamicParameters();

            if (string.IsNullOrEmpty(origen))
            {
                param.Add("@origen", dbType: DbType.String, value: origen);
                where += " AND A.origen = @origen ";
            } 
            if (Dni.HasValue)
            {
                param.Add("@Dni", dbType: DbType.Int64, value: Dni.Value);
                where += " AND A.Dni = @Dni ";
            }
            if (Control.HasValue)
            {
                param.Add("@Control", dbType: DbType.Int32, value: Control.Value);
                where += " AND A.Control = @Control ";
            }
            
            if (Importe.HasValue)
            {
                param.Add("@Importe", dbType: DbType.Decimal, value: Importe.Value);
                where += " AND A.Importe = @Importe ";
            }
            if (Fecha.HasValue)
            {
                param.Add("@Fecha", dbType: DbType.DateTime, value: Fecha.Value);
                where += " AND A.Fecha = @Fecha ";
            }
           
            if (NroRec.HasValue)
            {
                param.Add("@NroRec", dbType: DbType.Int32, value: NroRec.Value);
                where += " AND A.NroRec = @NroRec ";
            }
            if (CodCon.HasValue)
            {
                param.Add("@CodCon", dbType: DbType.Int32, value: CodCon.Value);
                where += " AND A.CodCon = @CodCon ";
            }
            if (ImporteDesde.HasValue)
            {
                param.Add("@ImporteDesde", dbType: DbType.Decimal, value: ImporteDesde.Value);
                where += " AND A.Importe > @ImporteDesde";
            }
            if (ImporteHasta.HasValue)
            {
                param.Add("@ImporteHasta", dbType: DbType.Decimal, value: ImporteHasta.Value);
                where += " AND A.Importe < @ImporteHasta";
            }
            if (FechaDesde.HasValue)
            {
                param.Add("@FechaDesde", dbType: DbType.DateTime, value: FechaDesde.Value);
                where += " AND A.FechaVto > @FechaDesde";
            }
            if (FechaHasta.HasValue)
            {
                param.Add("@FechaHasta", dbType: DbType.DateTime, value: FechaHasta.Value);
                where += " AND A.FechaVto < @FechaHasta";
            }
            
            string query = QUERY.Replace(Constantes.WHERE, where);
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Actividad>(query, param).ToList();
            }
        }

        public static Actividad LeerUno(long id)
        {
            var param = new DynamicParameters();
            param.Add("@Id", dbType: DbType.Int64, value: id);
            string query = QUERY.Replace(Constantes.WHERE, @"
                WHERE
                    A.Id = @Id 
            ");
            using (var db = new SqlConnection(conexion))
            {
                Actividad act = db.Query<Actividad>(query, param).FirstOrDefault();
                return act;
            }
        }

        public static long Insert(Actividad activida)
        {
            var param = new DynamicParameters();
            param.Add("@CodCon", dbType: DbType.Int32, value: activida.CodCon);
            param.Add("@Control", dbType: DbType.Int32, value: activida.Control);
            param.Add("@Dni", dbType: DbType.Int64, value: activida.Dni);
            param.Add("@fecha", dbType: DbType.DateTime, value: activida.fecha);
            param.Add("@Importe", dbType: DbType.Decimal, value: activida.Importe);
            param.Add("@NroRec", dbType: DbType.Int32, value: activida.NroRec);
            param.Add("@Origen", dbType: DbType.String, value: activida.Origen);
            param.Add("@ApeYNom", dbType: DbType.String, value: activida.ApeYNom);
            param.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
           
            const string SQL_QUERY = @"
                INSERT INTO Actividad
                       (Origen
                       ,ApeyNom
                       ,Dni
                       ,Control
                       ,Fecha
                       ,CodCon
                       ,NroRec
                       ,Importe)
                 VALUES
                       (@Origen
                       ,@ApeyNom
                       ,@Dni
                       ,@Control
                       ,@Fecha
                       ,@CodCon
                       ,@NroRec
                       ,@Importe);
                SET @ID = SCOPE_IDENTITY() AS BIGINT
            ";
            using (var db = new SqlConnection(conexion))
            {
                db.Execute(SQL_QUERY, param);
                return (param.Get<long>("@ID"));
                
            }
        }

        public static void Update(Actividad activida)
        {
            var param = new DynamicParameters();
            param.Add("@CodCon", dbType: DbType.Int32, value: activida.CodCon);
            param.Add("@Control", dbType: DbType.Int32, value: activida.Control);
            param.Add("@Dni", dbType: DbType.Int64, value: activida.Dni);
            param.Add("@fecha", dbType: DbType.DateTime, value: activida.fecha);
            param.Add("@Importe", dbType: DbType.Decimal, value: activida.Importe);
            param.Add("@NroRec", dbType: DbType.Int32, value: activida.NroRec);
            param.Add("@Origen", dbType: DbType.String, value: activida.Origen);
            param.Add("@ApeYNom", dbType: DbType.String, value: activida.ApeYNom);
            param.Add("@Id", dbType: DbType.Int64, value: activida.Id);
            
            const string SQL_QUERY = @"
               UPDATE Actividad
               SET Origen = @Origen
                  ,ApeyNom = @ApeyNom
                  ,Dni = @Dni
                  ,Control = @Control
                  ,Fecha = @Fecha
                  ,CodCon = @CodCon
                  ,NroRec = @NroRec
                  ,Importe = @Importe
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
                Actividad
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