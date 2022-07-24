using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmNVList : Form
    {
        private string connectionString;
        NhanVienBLL bllNV;

        public frmNVList(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            bllNV = new NhanVienBLL(connectionString);
        }

        public void ShowallNV()
        {
            DataTable dt = bllNV.getallNhanVien();
            dgvList.DataSource = dt;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            NhanVien nv = new NhanVien();
            nv.MANV = txtMaNV.Text;
            nv.HOTEN = txtName.Text;
            nv.EMAIl = txtEmail.Text;
            nv.LUONG = txtSalary.Text;
            nv.TENDN = txtUsername.Text;
            nv.MATKHAU = txtPassword.Text;
            if (bllNV.InsertNhanVien(nv))
            {
                ShowallNV();
            }
            else
            {
                MessageBox.Show("Đã có lỗi xảy ra, thử lại sau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmNVList_Load(object sender, EventArgs e)
        {
            ShowallNV();
        }
    }
}
