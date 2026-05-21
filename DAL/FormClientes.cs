using ControleVendas.DAL;
using System;
using System.Windows.Forms;

namespace ControleVendas
{
    public partial class FormClientes : Form
    {
        private int idSelecionado = 0;

        public FormClientes()
        {
            InitializeComponent();
            MontarLayout();
            CarregarGrid();
        }

        Label lblNome, lblCPF, lblTelefone, lblEmail;
        TextBox txtNome, txtCPF, txtTelefone, txtEmail;
        Button btnSalvar, btnEditar, btnExcluir, btnLimpar;
        DataGridView grid;

        void MontarLayout()
        {
            this.Text = "Clientes";
            this.Size = new System.Drawing.Size(800, 600);

            lblNome = new Label { Text = "Nome:", Left = 20, Top = 20, Width = 80 };
            txtNome = new TextBox { Left = 100, Top = 18, Width = 200 };

            lblCPF = new Label { Text = "CPF:", Left = 20, Top = 55, Width = 80 };
            txtCPF = new TextBox { Left = 100, Top = 53, Width = 200 };

            lblTelefone = new Label { Text = "Telefone:", Left = 20, Top = 90, Width = 80 };
            txtTelefone = new TextBox { Left = 100, Top = 88, Width = 200 };

            lblEmail = new Label { Text = "Email:", Left = 20, Top = 125, Width = 80 };
            txtEmail = new TextBox { Left = 100, Top = 123, Width = 200 };

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

            btnSalvar.Click += BtnSalvar_Click;
            btnEditar.Click += BtnEditar_Click;
            btnExcluir.Click += BtnExcluir_Click;
            btnLimpar.Click += BtnLimpar_Click;
            grid.CellClick += Grid_CellClick;

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblNome, txtNome, lblCPF, txtCPF,
                lblTelefone, txtTelefone, lblEmail, txtEmail,
                btnSalvar, btnEditar, btnExcluir, btnLimpar, grid
            });
        }

        void CarregarGrid()
        {
            grid.DataSource = null;
            grid.DataSource = ClienteDAO.Listar();
        }

        void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNome.Text)) { MessageBox.Show("Informe o nome!"); return; }
                ClienteDAO.Inserir(new Cliente
                {
                    Nome = txtNome.Text,
                    CPF = txtCPF.Text,
                    Telefone = txtTelefone.Text,
                    Email = txtEmail.Text
                });
                MessageBox.Show("Cliente salvo!");
                LimparCampos();
                CarregarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message);
            }
        }

        void BtnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idSelecionado == 0) { MessageBox.Show("Selecione um cliente!"); return; }
                if (string.IsNullOrEmpty(txtNome.Text)) { MessageBox.Show("Informe o nome!"); return; }

                var c = new Cliente
                {
                    Id = idSelecionado,
                    Nome = txtNome.Text,
                    CPF = txtCPF.Text,
                    Telefone = txtTelefone.Text,
                    Email = txtEmail.Text
                };
                ClienteDAO.Atualizar(c);
                MessageBox.Show("Cliente atualizado!");
                LimparCampos();
                CarregarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar: " + ex.Message);
            }
        }

        void BtnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (idSelecionado == 0) { MessageBox.Show("Selecione um cliente!"); return; }
                var res = MessageBox.Show("Confirma exclusão?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes) return;
                ClienteDAO.Excluir(idSelecionado);
                MessageBox.Show("Cliente excluído!");
                LimparCampos();
                CarregarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir: " + ex.Message);
            }
        }

        void BtnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = grid.Rows[e.RowIndex];
            try
            {
                if (row.Cells["Id"].Value != null)
                    idSelecionado = Convert.ToInt32(row.Cells["Id"].Value);
                else
                    idSelecionado = Convert.ToInt32(row.Cells[0].Value);

                txtNome.Text = row.Cells["Nome"].Value?.ToString();
                txtCPF.Text = row.Cells["CPF"].Value?.ToString();
                txtTelefone.Text = row.Cells["Telefone"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
            }
            catch
            {
                // ignore parsing issues
            }
        }

        void LimparCampos()
        {
            idSelecionado = 0;
            txtNome.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            grid.ClearSelection();
        }
    }
}