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
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    public partial class frmLogin : Form
    {
        public string connectionString;

        public frmLogin(string connectionString)
        {
            InitializeComponent();

            this.connectionString = connectionString;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private DataTable execQuery(string query, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(query, connection);
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            adapter.Fill(data);

            return data;
        }

        public static string toSHA1(string str)
        {
            string result = "";
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
            buffer = SHA1.ComputeHash(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                result += buffer[i].ToString("x2");
            }

            return result;
        }

        public static string toMD5(string str)
        {
            string result = "";
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                result += buffer[i].ToString("x2");
            }

            return result;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txbUsername.Text;
            string password = txbPassword.Text;

            using(SqlConnection connection = new SqlConnection(connectionString))
            { 
                connection.Open();

                string queryNV = "exec SP_CHECK_LOGIN '" + username + "'" + ", '0x" + toSHA1(password).ToUpper() + "';";
                string querySV = "exec SP_CHECK_LOGIN '" + username + "'" + ", '0x" + toMD5(password).ToUpper() + "';";
               
                DataTable dataNV = execQuery(queryNV, connection);
                DataTable dataSV = execQuery(querySV, connection);

                
                if (dataNV.Rows.Count == 1 || dataSV.Rows.Count == 1)
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmNVList form = new frmNVList(this.connectionString);
                    this.Hide();
                    form.ShowDialog();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                connection.Close();
            }    
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if(MessageBox.Show("Bạn có thực sự muốn thoát chương trình", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            //{
            //    e.Cancel = true;
            //}
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
