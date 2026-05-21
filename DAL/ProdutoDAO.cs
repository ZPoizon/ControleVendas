using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace ControleVendas.DAL
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Descricao { get; set; } = "";
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
    }

    public class ProdutoDAO
    {
        public static List<Produto> Listar()
        {
            var lista = new List<Produto>();
            using (var con = Conexao.ObterConexao())
            {
                con.Open();
                var cmd = new SqlCommand("SELECT * FROM Produtos", con);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Produto
                    {
                        Id = (int)reader["Id"],
                        Nome = reader["Nome"].ToString()!,
                        Descricao = reader["Descricao"].ToString()!,
                        Preco = (decimal)reader["Preco"],
                        Estoque = (int)reader["Estoque"]
                    });
                }
            }
            return lista;
        }

        public static void Inserir(Produto p)
        {
            using (var con = Conexao.ObterConexao())
            {
                con.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Produtos (Nome, Descricao, Preco, Estoque) VALUES (@Nome, @Descricao, @Preco, @Estoque)", con);
                cmd.Parameters.AddWithValue("@Nome", p.Nome);
                cmd.Parameters.AddWithValue("@Descricao", p.Descricao);
                cmd.Parameters.AddWithValue("@Preco", p.Preco);
                cmd.Parameters.AddWithValue("@Estoque", p.Estoque);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Atualizar(Produto p)
        {
            using (var con = Conexao.ObterConexao())
            {
                con.Open();
                var cmd = new SqlCommand(
                    "UPDATE Produtos SET Nome=@Nome, Descricao=@Descricao, Preco=@Preco, Estoque=@Estoque WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Nome", p.Nome);
                cmd.Parameters.AddWithValue("@Descricao", p.Descricao);
                cmd.Parameters.AddWithValue("@Preco", p.Preco);
                cmd.Parameters.AddWithValue("@Estoque", p.Estoque);
                cmd.Parameters.AddWithValue("@Id", p.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Excluir(int id)
        {
            using (var con = Conexao.ObterConexao())
            {
                con.Open();
                var cmd = new SqlCommand("DELETE FROM Produtos WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}