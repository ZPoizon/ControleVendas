using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace ControleVendas.DAL
{
    public class Venda
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; } = "";
        public DateTime DataVenda { get; set; }
        public decimal Total { get; set; }
    }

    public class ItemVenda
    {
        public int IdProduto { get; set; }
        public string NomeProduto { get; set; } = "";
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;
    }

    public class VendaDAO
    {
        public static List<Venda> Listar()
        {
            var lista = new List<Venda>();
            using (var con = Conexao.ObterConexao())
            {
                con.Open();
                var cmd = new SqlCommand(@"
                    SELECT v.Id, v.IdCliente, c.Nome as NomeCliente, v.DataVenda, v.Total
                    FROM Vendas v
                    INNER JOIN Clientes c ON v.IdCliente = c.Id
                    ORDER BY v.DataVenda DESC", con);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Venda
                    {
                        Id = (int)reader["Id"],
                        IdCliente = (int)reader["IdCliente"],
                        NomeCliente = reader["NomeCliente"].ToString()!,
                        DataVenda = (DateTime)reader["DataVenda"],
                        Total = (decimal)reader["Total"]
                    });
                }
            }
            return lista;
        }

        public static int Inserir(Venda v, List<ItemVenda> itens)
        {
            using (var con = Conexao.ObterConexao())
            {
                con.Open();
                var transaction = con.BeginTransaction();
                try
                {
                    var cmdVenda = new SqlCommand(
                        "INSERT INTO Vendas (IdCliente, DataVenda, Total) VALUES (@IdCliente, @DataVenda, @Total); SELECT SCOPE_IDENTITY();",
                        con, transaction);
                    cmdVenda.Parameters.AddWithValue("@IdCliente", v.IdCliente);
                    cmdVenda.Parameters.AddWithValue("@DataVenda", v.DataVenda);
                    cmdVenda.Parameters.AddWithValue("@Total", v.Total);
                    int idVenda = Convert.ToInt32(cmdVenda.ExecuteScalar());

                    foreach (var item in itens)
                    {
                        var cmdItem = new SqlCommand(
                            "INSERT INTO ItensVenda (IdVenda, IdProduto, Quantidade, PrecoUnitario) VALUES (@IdVenda, @IdProduto, @Quantidade, @PrecoUnitario)",
                            con, transaction);
                        cmdItem.Parameters.AddWithValue("@IdVenda", idVenda);
                        cmdItem.Parameters.AddWithValue("@IdProduto", item.IdProduto);
                        cmdItem.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                        cmdItem.Parameters.AddWithValue("@PrecoUnitario", item.PrecoUnitario);
                        cmdItem.ExecuteNonQuery();

                        var cmdEstoque = new SqlCommand(
                            "UPDATE Produtos SET Estoque = Estoque - @Qtd WHERE Id = @Id",
                            con, transaction);
                        cmdEstoque.Parameters.AddWithValue("@Qtd", item.Quantidade);
                        cmdEstoque.Parameters.AddWithValue("@Id", item.IdProduto);
                        cmdEstoque.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return idVenda;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}