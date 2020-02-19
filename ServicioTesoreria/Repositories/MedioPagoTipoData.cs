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
    public class MedioPagoTipoData
    {
        private static readonly string conexion = ConfigurationManager.ConnectionStrings[Constantes.CONN].ConnectionString;

        const string QUERY = @"
            SELECT
                  T.id
                  ,T.Descripcion
                  FROM
                MedioPagoTipo T
            {WHERE} 
            ";
        
        public static List<MedioPagoTipo> LeerTodo()
        {
            var query = QUERY.Replace(Constantes.WHERE, "");
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<MedioPagoTipo>(query).ToList();
            }
        }

    }
}