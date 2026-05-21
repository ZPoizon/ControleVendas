using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControleVendas.DAL
{
    public class Conexao
    {
        private static string _connectionString =
            "Server=localhost\\SQLEXPRESS;Database=ControleVendasDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection ObterConexao()
        {
            return new SqlConnection(_connectionString);
        }
    }
}