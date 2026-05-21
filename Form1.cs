using System.Windows.Forms;

namespace ControleVendas
{
    public partial class Form1 : Form
    {
        private Button? btnClientes, btnProdutos, btnVendas;

        public Form1()
        {
            InitializeComponent();
            MontarMenu();
        }

        private void MontarMenu()
        {
            this.Text = "Controle de Vendas";
            this.Size = new System.Drawing.Size(400, 350);

            btnClientes = new Button { Text = "Clientes", Left = 50, Top = 50, Width = 150, Height = 40 };
            btnProdutos = new Button { Text = "Produtos", Left = 50, Top = 110, Width = 150, Height = 40 };
            btnVendas = new Button { Text = "Vendas", Left = 50, Top = 170, Width = 150, Height = 40 };

            btnClientes.Click += (s, e) => new FormClientes().ShowDialog();
            btnProdutos.Click += (s, e) => new FormProdutos().ShowDialog();
            btnVendas.Click += (s, e) => new FormVendas().ShowDialog();

            this.Controls.AddRange(new System.Windows.Forms.Control[] { btnClientes, btnProdutos, btnVendas });
        }
    }
}