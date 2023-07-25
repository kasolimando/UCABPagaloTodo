using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public static class Encriptacion
{
    public static string EncriptarClave(string cadena)
    {

        using (SHA256 sha256 = new SHA256CryptoServiceProvider())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(cadena);
            byte[] hash = sha256.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}