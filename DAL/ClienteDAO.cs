using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace ControleVendas.DAL
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }

    public class ClienteDAO
    {
        public static List<Cliente> Listar()
        {
            var lista = new List<Cliente>();
            using (var con = Conexao.ObterConexao())
            {
                con.Open();
                var cmd = new SqlCommand("SELECT * FROM Clientes", con);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Cliente
                    {
                        Id = (int)reader["Id"],
                        Nome = reader["Nome"].ToString(),
                        CPF = reader["CPF"].ToString(),
                        Telefone = reader["Telefone"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }
            return lista;
        }

        public static void Inserir(Cliente c)
        {
            using (var con = Conexao.ObterConexao())
            {
                con.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Clientes (Nome, CPF, Telefone, Email) VALUES (@Nome, @CPF, @Telefone, @Email)", con);
                cmd.Parameters.AddWithValue("@Nome", c.Nome);
                cmd.Parameters.AddWithValue("@CPF", c.CPF);
                cmd.Parameters.AddWithValue("@Telefone", c.Telefone);
                cmd.Parameters.AddWithValue("@Email", c.Email);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Atualizar(Cliente c)
        {
            using (var con = Conexao.ObterConexao())
            {
                con.Open();
                var cmd = new SqlCommand(
                    "UPDATE Clientes SET Nome=@Nome, CPF=@CPF, Telefone=@Telefone, Email=@Email WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Nome", c.Nome);
                cmd.Parameters.AddWithValue("@CPF", c.CPF);
                cmd.Parameters.AddWithValue("@Telefone", c.Telefone);
                cmd.Parameters.AddWithValue("@Email", c.Email);
                cmd.Parameters.AddWithValue("@Id", c.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Excluir(int id)
        {
            using (var con = Conexao.ObterConexao())
            {
                con.Open();
                var cmd = new SqlCommand("DELETE FROM Clientes WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}