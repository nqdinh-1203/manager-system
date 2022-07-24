using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    public class SHA1
    {
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
    }
}
