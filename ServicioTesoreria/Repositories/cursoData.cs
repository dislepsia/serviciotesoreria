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
    public class CursoData
    {
        private static readonly string conexion = ConfigurationManager.ConnectionStrings[Constantes.CONN].ConnectionString;

        const string QUERY = @"
              SELECT C.Id
                  ,C.Abreviada
                  ,C.Descripcion
                  ,C.CodCon
                  ,C.Origen
                  ,C.NroCurso
              FROM Curso C
            {WHERE} 
            ";
        
        public static List<Curso> LeerTodo()
        {
            var query = QUERY.Replace(Constantes.WHERE, "");
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Curso>(query).ToList();
            }
        }

        public static List<Curso> Buscar(long? Id = null
                                        , string descripcion = ""
                                        , string origen = ""
                                        ,int? CodCon= null
                                        , int? NroCurso = null)
        {
            string where = @"WHERE 1 = 1";
            DynamicParameters param = new DynamicParameters();

            if (Id.HasValue)
            {
                param.Add("@Id", dbType: DbType.Int64, value: Id.Value);
                where += " AND C.Id = @Id ";
            }
            if (CodCon.HasValue)
            {
                param.Add("@CodCon", dbType: DbType.Int32, value: CodCon.Value);
                where += " AND C.CodCon = @CodCon ";
            }
            if (NroCurso.HasValue)
            {
                param.Add("@NroCurso", dbType: DbType.Int32, value: NroCurso.Value);
                where += " AND C.NroCurso = @NroCurso ";
            }
            if (!string.IsNullOrEmpty(descripcion))
            {
                param.Add("@descripcion", dbType: DbType.String, value: descripcion);
                where += " AND C.descripcion LIKE '%@descripcion%' ";
            }
            if (!string.IsNullOrEmpty(origen))
            {
                param.Add("@origen", dbType: DbType.String, value: origen);
                where += " AND C.origen = @origen ";
            }
            
            string query = QUERY.Replace(Constantes.WHERE, where);
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Curso>(query, param).ToList();
            }
        }

        public static Curso LeerUno(long id)
        {
            var param = new DynamicParameters();
            param.Add("@Id", dbType: DbType.Int64, value: id);
            string query = QUERY.Replace(Constantes.WHERE, @"
                WHERE
                    C.Id = @Id 
            ");
            using (var db = new SqlConnection(conexion))
            {
                Curso act = db.Query<Curso>(query, param).FirstOrDefault();
                return act;
            }
        }

        public static Curso LeerUno(int nroCurso, string origen)
        {
            var param = new DynamicParameters();
            param.Add("@nroCurso", dbType: DbType.Int32, value: nroCurso);
            param.Add("@origen", dbType: DbType.String, value: origen);
            string query = QUERY.Replace(Constantes.WHERE, @"
                WHERE
                    C.NroCurso = @NroCurso AND C.Origen = @origen 
            ");
            using (var db = new SqlConnection(conexion))
            {
                Curso act = db.Query<Curso>(query, param).FirstOrDefault();
                return act;
            }
        }

        public static long Insert(Curso curso)
        {
            var param = new DynamicParameters();
            param.Add("@Descripcion", dbType: DbType.String, value: curso.Descripcion);
            param.Add("@Origen", dbType: DbType.String, value: curso.Origen);
            param.Add("@Abreviada", dbType: DbType.String, value: curso.Abreviada);
            param.Add("@CodCon", dbType: DbType.Int32, value: curso.CodCon);
            param.Add("@NroCurso", dbType: DbType.Int32, value: curso.NroCurso);
            param.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
            
            const string SQL_QUERY = @"
                INSERT INTO Curso
                       (Descripcion
                       ,Origen
                       ,Abreviada
                       ,CodCon
                       ,NroCurso)
                 VALUES
                       (@Descripcion
                       ,@Origen
                       ,@Abreviada
                       ,@CodCon
                       ,@NroCurso);
                SET @ID = SCOPE_IDENTITY()
            ";
            using (var db = new SqlConnection(conexion))
            {
                db.Execute(SQL_QUERY, param);
                return (param.Get<long>("@ID"));
            }
        }

        public static void Update(Curso curso)
        {
            var param = new DynamicParameters();
            param.Add("@Id", dbType: DbType.Int64, value: curso.Id);
            param.Add("@Descripcion", dbType: DbType.String, value: curso.Descripcion);
            param.Add("@Origen", dbType: DbType.String, value: curso.Origen);
            param.Add("@Abreviada", dbType: DbType.String, value: curso.Abreviada);
            param.Add("@CodCon", dbType: DbType.Int32, value: curso.CodCon);
            param.Add("@NroCurso ", dbType: DbType.Int32, value: curso.NroCurso);
           
            const string SQL_QUERY = @"
               UPDATE dbo.Alumno
               SET 
                  Descripcion = @Descripcion
                  ,Abreviada = @Abreviada
                  ,Origen = @Origen
                  ,CodCon = @CodCon
                  ,NroCurso = @NroCurso 
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
                Curso
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