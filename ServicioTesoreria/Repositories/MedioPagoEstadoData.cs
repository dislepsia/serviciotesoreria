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
    public class MedioPagoEstadoData
    {
        private static readonly string conexion = ConfigurationManager.ConnectionStrings[Constantes.CONN].ConnectionString;

        const string QUERY = @"
            SELECT
                  E.id
                  ,E.Descripcion
                  FROM
                MedioPagoEstado E
            {WHERE} 
            ";
        
        public static List<MedioPagoEstado> LeerTodo()
        {
            var query = QUERY.Replace(Constantes.WHERE, "");
            using (var db = new SqlConnection(conexion))
            {
                return db.Query<MedioPagoEstado>(query).ToList();
            }
        }

    }
}