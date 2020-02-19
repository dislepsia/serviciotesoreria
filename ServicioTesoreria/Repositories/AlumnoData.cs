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
    public class AlumnoData
    {
        private static readonly string conexion = ConfigurationManager.ConnectionStrings[Constantes.CONN].ConnectionString;

        const string QUERY = @"
            SELECT A.Dni
                  ,A.ApeyNom
                  ,A.Sexo
                  ,A.FechaNac
                  ,A.Domicilio
                  ,A.CodPostal
                  ,A.Telefono
                  ,A.Activo
                  ,A.Academico
              FROM Alumno A
            {WHERE} 
            ";
        
        public static List<Alumno> LeerTodo()
        {
            var query = QUERY.Replace(Constantes.WHERE, "");
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Alumno>(query).ToList();
            }
        }

        public static List<Alumno> Buscar(long? Dni = null
                                        , string ApeYNom = ""
                                        , string Sexo = ""
                                        , DateTime? FechaNac = null
                                        ,string Domicilio = ""
                                        ,int? CodPostal = null
                                        ,string telefono = ""
                                        , bool? activo = null
                                        , bool? academico = null            
            )
        {
            string where = @"WHERE 1 = 1";
            DynamicParameters param = new DynamicParameters();

            if (Dni.HasValue)
            {
                param.Add("@Dni", dbType: DbType.Int64, value: Dni.Value);
                where += " AND A.Dni = @Dni ";
            }
            if (string.IsNullOrEmpty(ApeYNom))
            {
                param.Add("@ApeYNom", dbType: DbType.String, value: ApeYNom);
                where += " AND A.ApeYNom LIKE '%@ApeYNom%' ";
            }
            if (string.IsNullOrEmpty(Sexo))
            {
                param.Add("@Sexo", dbType: DbType.String, value: Sexo);
                where += " AND A.Sexo = @Sexo ";
            }
            
            
            if (FechaNac.HasValue)
            {
                param.Add("@FechaNac", dbType: DbType.DateTime, value: FechaNac.Value);
                where += " AND A.FechaNac = @FechaNac ";
            }

            if (string.IsNullOrEmpty(Domicilio))
            {
                param.Add("@Domicilio", dbType: DbType.String, value: Domicilio);
                where += " AND A.Domicilio LIKE '%@Domicilio%' ";
            }
            if (CodPostal.HasValue)
            {
                param.Add("@CodPostal", dbType: DbType.Int32, value: CodPostal.Value);
                where += " AND A.CodPostal = @CodPostal ";
            }
            if (string.IsNullOrEmpty(telefono))
            {
                param.Add("@telefono", dbType: DbType.String, value: telefono);
                where += " AND A.telefono LIKE '%@telefono%' ";
            }
            if (activo.HasValue)
            {
                param.Add("@activo", dbType: DbType.Boolean, value: activo.Value);
                where += " AND A.activo = @activo";
            }
            if (academico.HasValue)
            {
                param.Add("@academico", dbType: DbType.Boolean, value: academico.Value);
                where += " AND A.academico = @academico";
            }
            
            string query = QUERY.Replace(Constantes.WHERE, where);
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<Alumno>(query, param).ToList();
            }
        }

        public static Alumno LeerUno(long id)
        {
            var param = new DynamicParameters();
            param.Add("@Dni", dbType: DbType.Int64, value: id);
            string query = QUERY.Replace(Constantes.WHERE, @"
                WHERE
                    A.Dni = @Dni 
            ");
            using (var db = new SqlConnection(conexion))
            {
                Alumno act = db.Query<Alumno>(query, param).FirstOrDefault();
                return act;
            }
        }

        public static void Insert(Alumno alumno)
        {
            var param = new DynamicParameters();
            param.Add("@Dni", dbType: DbType.Int64, value: alumno.Dni);
            param.Add("@ApeyNom", dbType: DbType.String, value: alumno.ApeyNom);

            //param.Add("@Sexo", dbType: DbType.String, value: alumno.Sexo);
            //param.Add("@FechaNac", dbType: DbType.DateTime, value: alumno.FechaNac);
            //param.Add("@Domicilio", dbType: DbType.String, value: alumno.Domicilio);
            //param.Add("@CodPostal", dbType: DbType.Int32, value: alumno.CodPostal);
            //param.Add("@Telefono", dbType: DbType.String, value: alumno.Telefono);
            //param.Add("@Activo", dbType: DbType.Boolean, value: alumno.Activo);
            //param.Add("@Academico", dbType: DbType.Boolean, value: alumno.Academico);
            const string SQL_QUERY = @"
                INSERT INTO Alumno
                       (Dni
                       ,ApeyNom)
                 VALUES
                       (@Dni
                       ,@ApeyNom)
            ";
            //const string SQL_QUERY = @"
            //    INSERT INTO Alumno
            //           (Dni
            //           ,ApeyNom
            //           ,Sexo
            //           ,FechaNac
            //           ,Domicilio
            //           ,CodPostal
            //           ,Telefono
            //           ,Activo
            //           ,Academico)
            //     VALUES
            //           (@Dni
            //           ,@ApeyNom
            //           ,@Sexo
            //           ,@FechaNac
            //           ,@Domicilio
            //           ,@CodPostal
            //           ,@Telefono
            //           ,@Activo
            //           ,@Academico)
            //";
            using (var db = new SqlConnection(conexion))
            {
                db.Execute(SQL_QUERY, param);
            }
        }

        public static void Update(Alumno alumno)
        {
            var param = new DynamicParameters();
            param.Add("@Dni", dbType: DbType.Int64, value: alumno.Dni);
            param.Add("@ApeyNom", dbType: DbType.String, value: alumno.ApeyNom);
            param.Add("@Sexo", dbType: DbType.String, value: alumno.Sexo);
            param.Add("@FechaNac", dbType: DbType.DateTime, value: alumno.FechaNac);
            param.Add("@Domicilio", dbType: DbType.String, value: alumno.Domicilio);
            param.Add("@CodPostal", dbType: DbType.Int32, value: alumno.CodPostal);
            param.Add("@Telefono", dbType: DbType.String, value: alumno.Telefono);
            param.Add("@Activo", dbType: DbType.Boolean, value: alumno.Activo);
            param.Add("@Academico", dbType: DbType.Boolean, value: alumno.Academico);
            
            const string SQL_QUERY = @"
               UPDATE dbo.Alumno
               SET 
                  ApeyNom = @ApeyNom
                  ,Sexo = @Sexo
                  ,FechaNac = @FechaNac
                  ,Domicilio = @Domicilio
                  ,CodPostal = @CodPostal
                  ,Telefono = @Telefono
                  ,Activo = @Activo
                  ,Academico = @Academico
                WHERE
	                 Dni = @Dni
            ";
            using (var db = new SqlConnection(conexion))
            {
                db.Execute(SQL_QUERY, param);
            }
        }

        public static void Delete(long id)
        {
            var param = new DynamicParameters();
            param.Add("@Dni", dbType: DbType.Int64, value: id);
            const string SQL_QUERY = @"
            DELETE
                Alumno
            WHERE
	            Dni = @Dni
            ";
            using (var db = new SqlConnection(conexion))
            {
                db.Execute(SQL_QUERY, param);
            }
        }
    }
}