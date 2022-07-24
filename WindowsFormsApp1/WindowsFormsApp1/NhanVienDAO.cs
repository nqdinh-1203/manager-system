using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    internal class NhanVienDAO
    {
        string connectionString;

        public NhanVienDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DataTable GetAllNhanVien()
        {
            DataTable dt = new DataTable();
            string query = "EXEC SP_SEL_ENCRYPT_NHANVIEN";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(dt);

                foreach(DataRow row in dt.Rows)
                {
                    string salary_enc = row[3].ToString();
                    byte[] salary = AES.Encrypt(Encoding.UTF8.GetBytes(salary_enc), AES.pass, AES.salt, AES.iv);
                    row[3] = salary;
                }
            }

            return dt;
        }

        public bool InsertNhanVien(NhanVien nv)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "SP_INS_ENCRYPT_NHANVIEN";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.Parameters.Add("@MaNV", SqlDbType.VarChar).Value = nv.MANV;
                cmd.Parameters.Add("@HoTen", SqlDbType.VarChar).Value = nv.HOTEN;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = nv.EMAIl;
                int luong = Convert.ToInt32(nv.LUONG);
                byte[] luongct = AES.Encrypt(Encoding.UTF8.GetBytes(nv.LUONG), AES.pass, AES.salt, AES.iv);
                cmd.Parameters.Add("@Luong", SqlDbType.VarBinary).Value = luongct;
                cmd.Parameters.Add("@TenDN", SqlDbType.VarChar).Value = nv.TENDN;
                string mkct = SHA1.toSHA1(nv.MATKHAU);
                cmd.Parameters.Add("@MatKhau", SqlDbType.VarBinary).Value = Encoding.UTF8.GetBytes(mkct);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                con.Close();
            }            
            return true;
        }
    }
}
