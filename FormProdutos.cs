using ControleVendas.DAL;
using System;
using System.Windows.Forms;

namespace ControleVendas
{
    public partial class FormProdutos : Form
    {
        private int idSelecionado = 0;

        private Label? lblNome, lblDescricao, lblPreco, lblEstoque;
        private TextBox? txtNome, txtDescricao, txtPreco, txtEstoque;
        private Button? btnSalvar, btnEditar, btnExcluir, btnLimpar;
        private DataGridView? grid;

        public FormProdutos()
        {
            InitializeComponent();
            MontarLayout();
            CarregarGrid();
        }

        private void MontarLayout()
        {
            this.Text = "Produtos";
            this.Size = new System.Drawing.Size(800, 600);

            lblNome = new Label { Text = "Nome:", Left = 20, Top = 20, Width = 80 };
            txtNome = new TextBox { Left = 100, Top = 18, Width = 200 };

            lblDescricao = new Label { Text = "Descrição:", Left = 20, Top = 55, Width = 80 };
            txtDescricao = new TextBox { Left = 100, Top = 53, Width = 200 };

            lblPreco = new Label { Text = "Preço:", Left = 20, Top = 90, Width = 80 };
            txtPreco = new TextBox { Left = 100, Top = 88, Width = 200 };

            lblEstoque = new Label { Text = "Estoque:", Left = 20, Top = 125, Width = 80 };
            txtEstoque = new TextBox { Left = 100, Top = 123, Width = 200 };

            btnSalvar = new Button { Text = "Salvar", Left = 20, Top = 165, Width = 80 };
            btnEditar = new Button { Text = "Editar", Left = 110, Top = 165, Width = 80 };
            btnExcluir = new Button { Text = "Excluir", Left = 200, Top = 165, Width = 80 };
            btnLimpar = new Button { Text = "Limpar", Left = 290, Top = 165, Width = 80 };

            grid = new DataGridView
            {
                Left = 20,
                Top = 210,
                Width = 740,
                Height = 330,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnSalvar.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtNome!.Text)) { MessageBox.Show("Informe o nome!"); return; }
                if (!decimal.TryParse(txtPreco!.Text, out decimal preco)) { MessageBox.Show("Preço inválido!"); return; }
                if (!int.TryParse(txtEstoque!.Text, out int estoque)) { MessageBox.Show("Estoque inválido!"); return; }

                ProdutoDAO.Inserir(new Produto
                {
                    Nome = txtNome.Text,
                    Descricao = txtDescricao!.Text,
                    Preco = preco,
                    Estoque = estoque
                });
                MessageBox.Show("Produto salvo!");
                LimparCampos();
                CarregarGrid();
            };

            btnEditar.Click += (s, e) =>
            {
                if (idSelecionado == 0) { MessageBox.Show("Selecione um produto!"); return; }
                if (!decimal.TryParse(txtPreco!.Text, out decimal preco)) { MessageBox.Show("Preço inválido!"); return; }
                if (!int.TryParse(txtEstoque!.Text, out int estoque)) { MessageBox.Show("Estoque inválido!"); return; }

                ProdutoDAO.Atualizar(new Produto
                {
                    Id = idSelecionado,
                    Nome = txtNome!.Text,
                    Descricao = txtDescricao!.Text,
                    Preco = preco,
                    Estoque = estoque
                });
                MessageBox.Show("Produto atualizado!");
                LimparCampos();
                CarregarGrid();
            };

            btnExcluir.Click += (s, e) =>
            {
                if (idSelecionado == 0) { MessageBox.Show("Selecione um produto!"); return; }
                if (MessageBox.Show("Confirma exclusão?", "Atenção", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProdutoDAO.Excluir(idSelecionado);
                    LimparCampos();
                    CarregarGrid();
                }
            };

            btnLimpar.Click += (s, e) => LimparCampos();

            grid.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var row = grid.Rows[e.RowIndex];
                idSelecionado = (int)row.Cells["Id"].Value;
                txtNome!.Text = row.Cells["Nome"].Value.ToString();
                txtDescricao!.Text = row.Cells["Descricao"].Value.ToString();
                txtPreco!.Text = row.Cells["Preco"].Value.ToString();
                txtEstoque!.Text = row.Cells["Estoque"].Value.ToString();
            };

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblNome, txtNome, lblDescricao, txtDescricao,
                lblPreco, txtPreco, lblEstoque, txtEstoque,
                btnSalvar, btnEditar, btnExcluir, btnLimpar, grid
            });
        }

        private void CarregarGrid()
        {
            grid!.DataSource = ProdutoDAO.Listar();
        }

        private void LimparCampos()
        {
            idSelecionado = 0;
            txtNome!.Text = txtDescricao!.Text = txtPreco!.Text = txtEstoque!.Text = "";
        }
    }
}