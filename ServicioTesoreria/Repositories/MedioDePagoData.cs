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
    public class MedioDePagoData
    {
        private static readonly string conexion = ConfigurationManager.ConnectionStrings[Constantes.CONN].ConnectionString;

        const string QUERY = @"
            SELECT M.id
                  ,M.tipoMedioPago
                  ,M.estado
                  ,M.fechaCreado
                  ,M.fechaGenerado
                  ,M.fechaImportado
                  ,M.fechaVerificar
                  ,M.fechaVerificado
                  ,M.cuota_id
                  ,M.activida_id
                  ,M.codigoGeneracion
                  ,M.codigoImportacion
                  ,M.importe
	              ,E.descripcion as EstadoDescripcion
	              ,T.descripcion as TipoMedioPagoDescripcion
              FROM MedioDePago M
              INNER JOIN MedioPagoEstado E ON M.estado = E.id
              INNER JOIN MedioPagoTipo T ON M.tipoMedioPago = T.id
            {WHERE} 
            ";
        
        public static List<MedioDePago> LeerTodo()
        {
            var query = QUERY.Replace(Constantes.WHERE, "");
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<MedioDePago>(query).ToList();
            }
        }

        public static List<MedioDePago> Buscar(long? cuota = null
            , long? activida = null
            , int? tipoMedioPago = null
            , int? Estado = null
            , string codigoGeneracion = ""
            , string codigoImportacion = ""
            , DateTime? FechaGenerado = null
            , DateTime? FechaCreado= null
            , DateTime? FechaImportado = null
            , DateTime? FechaVerificar= null
            , DateTime? FechaVerificado = null
            , DateTime? FechaGeneradoDesde = null
            , DateTime? FechaGeneradoHasta = null
            , DateTime? FechaImportadoDesde = null
            , DateTime? FechaImportadoHasta = null
            , DateTime? FechaCreadoDesde = null
            , DateTime? FechaCreadoHasta = null
            , DateTime? FechaVerificarDesde = null
            , DateTime? FechaVerificarHasta = null
            , DateTime? FechaVerificadoDesde = null
            , DateTime? FechaVerificadoHasta = null
            )
        {
            string where = @"WHERE 1 = 1";
            DynamicParameters param = new DynamicParameters();

            if (cuota.HasValue)
            {
                param.Add("@cuota", dbType: DbType.Int64, value: cuota.Value);
                where += " AND M.cuota_id = @cuota ";
            }
            if (activida.HasValue)
            {
                param.Add("@activida", dbType: DbType.Int64, value: activida.Value);
                where += " AND M.activida_id = @activida ";
            }
            if (tipoMedioPago.HasValue)
            {
                param.Add("@tipoMedioPago", dbType: DbType.Int32, value: tipoMedioPago.Value);
                where += " AND M.tipoMedioPago = @tipoMedioPago ";
            }
            if (Estado.HasValue)
            {
                param.Add("@Estado", dbType: DbType.Int32, value: Estado.Value);
                where += " AND M.Estado = @Estado ";
            } 
            if (!string.IsNullOrEmpty(codigoGeneracion))
            {
                param.Add("@codigoGeneracion", dbType: DbType.String, value: codigoGeneracion);
                where += " AND M.codigoGeneracion = @codigoGeneracion ";
            }

            if (!string.IsNullOrEmpty(codigoImportacion))
            {
                param.Add("@codigoImportacion", dbType: DbType.String, value: codigoImportacion);
                where += " AND M.codigoImportacion = @codigoImportacion ";
            }

            if (FechaCreado.HasValue)
            {
                param.Add("@FechaCreado", dbType: DbType.DateTime, value: FechaCreado.Value);
                where += " AND M.FechaCreado = @FechaCreado";
            } 
            if (FechaGenerado.HasValue)
            {
                param.Add("@FechaGenerado", dbType: DbType.DateTime, value: FechaGenerado.Value);
                where += " AND M.FechaGenerado = @FechaGenerado";
            }
            if (FechaImportado.HasValue)
            {
                param.Add("@FechaImportadoPago", dbType: DbType.DateTime, value: FechaImportado.Value);
                where += " AND M.FechaImportadoPago = @FechaImportadoPago ";
            }
            if (FechaVerificado.HasValue)
            {
                param.Add("@FechaVerificado", dbType: DbType.DateTime, value: FechaVerificado.Value);
                where += " AND M.FechaVerificado = @FechaVerificado ";
            }
            if (FechaVerificar.HasValue)
            {
                param.Add("@FechaVerificar", dbType: DbType.DateTime, value: FechaVerificar.Value);
                where += " AND M.FechaVerificar = @FechaVerificar ";
            }
            
            if (FechaGeneradoDesde.HasValue)
            {
                param.Add("@FechaGeneradoDesde", dbType: DbType.DateTime, value: FechaGeneradoDesde.Value);
                where += " AND M.FechaGenerado > @FechaGeneradoDesde";
            }
            if (FechaGeneradoHasta.HasValue)
            {
                param.Add("@FechaGeneradoHasta", dbType: DbType.DateTime, value: FechaGeneradoHasta.Value);
                where += " AND M.FechaGenerado < @FechaGeneradoHasta";
            }
            if (FechaImportadoDesde.HasValue)
            {
                param.Add("@FechaImportadoDesde", dbType: DbType.DateTime, value: FechaImportadoDesde.Value);
                where += " AND M.FechaImportado > @FechaImportadoDesde";
            }
            if (FechaImportadoHasta.HasValue)
            {
                param.Add("@FechaImportadoHasta", dbType: DbType.DateTime, value: FechaImportadoHasta.Value);
                where += " AND M.FechaImportado < @FechaImportadoHasta";
            }

            if (FechaCreadoDesde.HasValue)
            {
                param.Add("@FechaCreadoDesde", dbType: DbType.DateTime, value: FechaCreadoDesde.Value);
                where += " AND M.FechaCreado > @FechaCreadoDesde";
            }
            if (FechaCreadoHasta.HasValue)
            {
                param.Add("@FechaCreadoHasta", dbType: DbType.DateTime, value: FechaCreadoHasta.Value);
                where += " AND M.FechaCreado < @FechaCreadoHasta";
            }

            if (FechaVerificadoDesde.HasValue)
            {
                param.Add("@FechaVerificadoDesde", dbType: DbType.DateTime, value: FechaVerificadoDesde.Value);
                where += " AND M.FechaVerificado > @FechaVerificadoDesde";
            }
            if (FechaVerificadoHasta.HasValue)
            {
                param.Add("@FechaVerificadoHasta", dbType: DbType.DateTime, value: FechaVerificadoHasta.Value);
                where += " AND M.FechaVerificado < @FechaVerificadoHasta";
            }

            if (FechaVerificarDesde.HasValue)
            {
                param.Add("@FechaVerificarDesde", dbType: DbType.DateTime, value: FechaVerificarDesde.Value);
                where += " AND M.FechaVerificar > @FechaVerificarDesde";
            }
            if (FechaVerificarHasta.HasValue)
            {
                param.Add("@FechaVerificarHasta", dbType: DbType.DateTime, value: FechaVerificarHasta.Value);
                where += " AND M.FechaVerificar < @FechaVerificarHasta";
            }
            
            string query = QUERY.Replace(Constantes.WHERE, where);
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<MedioDePago>(query, param).ToList();
            }
        }

        public static MedioDePago LeerUno(long id)
        {
            var param = new DynamicParameters();
            param.Add("@Id", dbType: DbType.Int64, value: id);
            string query = QUERY.Replace(Constantes.WHERE, @"
                WHERE
                    C.Id = @Id 
            ");
            using (var db = new SqlConnection(conexion))
            {
                MedioDePago medio = db.Query<MedioDePago>(query, param).FirstOrDefault();
                return medio;
            }
        }

        
        public static long Insert(MedioDePago medio)
        {
            var param = new DynamicParameters();
            param.Add("@codigoGeneracion", dbType: DbType.String, value: medio.codigoGeneracion);
            param.Add("@codigoImportacion", dbType: DbType.String, value: medio.codigoImportacion);
            param.Add("@Estado", dbType: DbType.Int32, value: medio.Estado);
            param.Add("@cuota_id", dbType: DbType.Int64, value: medio.cuota_id);
            param.Add("@activida_id", dbType: DbType.Int64, value: medio.activida_id);
            param.Add("@fechaGenerado", dbType: DbType.DateTime, value: medio.fechaGenerado);
            param.Add("@fechaImportadoPago", dbType: DbType.DateTime, value: medio.fechaImportadoPago);
            param.Add("@Importe", dbType: DbType.Decimal, value: medio.Importe);
            param.Add("@TipoMedioPago", dbType: DbType.Int32, value: medio.TipoMedioPago);
            
            param.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
            const string SQL_QUERY = @"
                INSERT INTO MedioDePago
                (
                   tipoMedioPago,
                   estado,
                   fechaGenerado,
                   fechaImportadoPago,
                   cuota_id,
                    activida_id, 
                   codigoGeneracion, 
                   codigoImportacion,
                   importe
                )
                VALUES
               (
                    @tipoMedioPago
                   ,@estado
                   ,@fechaGenerado
                   ,@fechaImportadoPago
                   ,@cuota_id
                    ,@activida_id
                   ,@codigoGeneracion
                   ,@codigoImportacion
                   ,@importe
               );
                SET @ID = SCOPE_IDENTITY() AS BIGINT
            ";
            using (var db = new SqlConnection(conexion))
            {
                db.Execute(SQL_QUERY, param);
                return (param.Get<long>("@ID"));
                
            }
        }

        public static void Update(MedioDePago medio)
        {
            var param = new DynamicParameters();
            param.Add("@codigoGeneracion", dbType: DbType.String, value: medio.codigoGeneracion);
            param.Add("@codigoImportacion", dbType: DbType.String, value: medio.codigoImportacion);
            param.Add("@Estado", dbType: DbType.Int32, value: medio.Estado);
            param.Add("@cuota_id", dbType: DbType.Int64, value: medio.cuota_id);
            param.Add("@activida_id", dbType: DbType.Int64, value: medio.activida_id);
            param.Add("@fechaGenerado", dbType: DbType.DateTime, value: medio.fechaGenerado);
            param.Add("@fechaImportadoPago", dbType: DbType.DateTime, value: medio.fechaImportadoPago);
            param.Add("@Importe", dbType: DbType.Decimal, value: medio.Importe);
            param.Add("@TipoMedioPago", dbType: DbType.Int32, value: medio.TipoMedioPago);
            param.Add("@Id", dbType: DbType.Int64, value: medio.Id);
            
            const string SQL_QUERY = @"
                UPDATE MedioDePago
                SET tipoMedioPago = @tipoMedioPago
                  ,estado = @estado
                  ,fechaGenerado = @fechaGenerado
                  ,fechaImportadoPago = @fechaImportadoPago
                  ,cuota_id = @cuota_id
                  ,activida_id = @activida_id
                  ,codigoGeneracion = @codigoGeneracion
                  ,codigoImportacion = @codigoImportacion
                  ,importe = @importe
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
                dbo.MedioDePago
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