using ControleVendas.DAL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ControleVendas
{
    public partial class FormVendas : Form
    {
        private List<ItemVenda> itens = new List<ItemVenda>();

        private Label? lblCliente, lblProduto, lblQuantidade, lblTotal;
        private ComboBox? cmbCliente, cmbProduto;
        private TextBox? txtQuantidade;
        private Button? btnAdicionarItem, btnFinalizarVenda, btnLimpar;
        private DataGridView? gridItens, gridVendas;

        public FormVendas()
        {
            InitializeComponent();
            MontarLayout();
            CarregarCombos();
            CarregarGridVendas();
        }

        private void MontarLayout()
        {
            this.Text = "Vendas";
            this.Size = new System.Drawing.Size(900, 650);

            lblCliente = new Label { Text = "Cliente:", Left = 20, Top = 20, Width = 70 };
            cmbCliente = new ComboBox { Left = 95, Top = 18, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            lblProduto = new Label { Text = "Produto:", Left = 20, Top = 55, Width = 70 };
            cmbProduto = new ComboBox { Left = 95, Top = 53, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            lblQuantidade = new Label { Text = "Qtd:", Left = 360, Top = 55, Width = 40 };
            txtQuantidade = new TextBox { Left = 400, Top = 53, Width = 60, Text = "1" };

            btnAdicionarItem = new Button { Text = "Adicionar Item", Left = 480, Top = 52, Width = 120, Height = 26 };
            btnFinalizarVenda = new Button { Text = "Finalizar Venda", Left = 620, Top = 52, Width = 130, Height = 26 };
            btnLimpar = new Button { Text = "Limpar", Left = 760, Top = 52, Width = 80, Height = 26 };

            lblTotal = new Label { Text = "Total: R$ 0,00", Left = 20, Top = 90, Width = 300, Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold) };

            gridItens = new DataGridView
            {
                Left = 20,
                Top = 120,
                Width = 840,
                Height = 180,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            gridVendas = new DataGridView
            {
                Left = 20,
                Top = 320,
                Width = 840,
                Height = 270,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAdicionarItem.Click += (s, e) =>
            {
                if (cmbProduto!.SelectedItem == null) { MessageBox.Show("Selecione um produto!"); return; }
                if (!int.TryParse(txtQuantidade!.Text, out int qtd) || qtd <= 0) { MessageBox.Show("Quantidade inválida!"); return; }

                var produto = (Produto)cmbProduto.SelectedItem;

                if (qtd > produto.Estoque) { MessageBox.Show($"Estoque insuficiente! Disponível: {produto.Estoque}"); return; }

                itens.Add(new ItemVenda
                {
                    IdProduto = produto.Id,
                    NomeProduto = produto.Nome,
                    Quantidade = qtd,
                    PrecoUnitario = produto.Preco
                });

                AtualizarGridItens();
                AtualizarTotal();
            };

            btnFinalizarVenda.Click += (s, e) =>
            {
                if (cmbCliente!.SelectedItem == null) { MessageBox.Show("Selecione um cliente!"); return; }
                if (itens.Count == 0) { MessageBox.Show("Adicione pelo menos um item!"); return; }

                var cliente = (Cliente)cmbCliente.SelectedItem;
                decimal total = 0;
                foreach (var item in itens) total += item.Subtotal;

                var venda = new Venda
                {
                    IdCliente = cliente.Id,
                    DataVenda = DateTime.Now,
                    Total = total
                };

                VendaDAO.Inserir(venda, itens);
                MessageBox.Show("Venda finalizada com sucesso!");
                LimparVenda();
                CarregarCombos();
                CarregarGridVendas();
            };

            btnLimpar.Click += (s, e) => LimparVenda();

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblCliente, cmbCliente, lblProduto, cmbProduto,
                lblQuantidade, txtQuantidade, btnAdicionarItem,
                btnFinalizarVenda, btnLimpar, lblTotal,
                gridItens, gridVendas
            });
        }

        private void CarregarCombos()
        {
            var clientes = ClienteDAO.Listar();
            cmbCliente!.DataSource = clientes;
            cmbCliente.DisplayMember = "Nome";
            cmbCliente.ValueMember = "Id";

            var produtos = ProdutoDAO.Listar();
            cmbProduto!.DataSource = produtos;
            cmbProduto.DisplayMember = "Nome";
            cmbProduto.ValueMember = "Id";
        }

        private void AtualizarGridItens()
        {
            gridItens!.DataSource = null;
            gridItens.DataSource = new System.ComponentModel.BindingList<ItemVenda>(itens);
        }

        private void AtualizarTotal()
        {
            decimal total = 0;
            foreach (var item in itens) total += item.Subtotal;
            lblTotal!.Text = $"Total: R$ {total:F2}";
        }

        private void CarregarGridVendas()
        {
            gridVendas!.DataSource = VendaDAO.Listar();
        }

        private void LimparVenda()
        {
            itens.Clear();
            AtualizarGridItens();
            AtualizarTotal();
        }
    }
}