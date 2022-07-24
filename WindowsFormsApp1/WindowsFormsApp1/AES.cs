using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WindowsFormsApp1
{
    internal class AES
    {
        public static byte[] salt = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        public static byte[] iv = new byte[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        public static string pass = "19120478";
        private const int KEYSIZE = 256;
        public static byte[] Encrypt(byte[] data, string password, byte[] salt, byte[] iv)
        {
            var rij = new RijndaelManaged()
            {
                KeySize = KEYSIZE,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            var rfc = new Rfc2898DeriveBytes(password, salt);
            rij.Key = rfc.GetBytes(KEYSIZE / 8);
            rij.IV = iv;

            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, rij.CreateEncryptor(), CryptoStreamMode.Write);

            using (var bw = new BinaryWriter(cs))
            {
                bw.Write(data);
            }

            return ms.ToArray();
        }
        public static byte[] Decrypt(byte[] data, string password, byte[] salt, byte[] iv)
        {
            var rij = new RijndaelManaged()
            {
                KeySize = KEYSIZE,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            var rfc = new Rfc2898DeriveBytes(password, salt);
            rij.Key = rfc.GetBytes(KEYSIZE / 8);
            rij.IV = iv;

            var ms = new MemoryStream(data);
            var cs = new CryptoStream(ms, rij.CreateDecryptor(), CryptoStreamMode.Read);

            var br = new BinaryReader(cs);
            return br.ReadBytes(data.Length);
        }
    }
}
