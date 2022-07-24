using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace WindowsFormsApp1
{
    internal class NhanVienBLL
    {
        NhanVienDAO dalNV;
        public NhanVienBLL(string connStr)
        {
            dalNV = new NhanVienDAO(connStr);
        }

        public DataTable getallNhanVien()
        {
            return dalNV.GetAllNhanVien();
        }

        public bool InsertNhanVien(NhanVien nv)
        {
            return dalNV.InsertNhanVien(nv);
        }
    }
}
