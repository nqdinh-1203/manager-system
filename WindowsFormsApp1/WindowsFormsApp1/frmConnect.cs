using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class frmConnect : Form
    {
        public frmConnect()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=" + txbServer.Text + ";Initial Catalog=" + txbDatabase.Text + ";Integrated Security=True";

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    frmLogin f = new frmLogin(connectionString);
                    this.Hide();
                    f.ShowDialog();
                    this.Show();

                    connection.Close();
                }
                catch (SqlException)
                {
                    MessageBox.Show("Kết nối thất bại!", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmConnect_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thực sự muốn thoát chương trình", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
