using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Plum
{
    public static class SecurityExtensions
    {
        private static readonly string DesKey = "a3f3bc6d43e7f10d";

        /// <summary>
        /// Encrypts the by DES.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string EncryptDes(this string text)
        {
            if (text.IsNullOrEmpty()) return string.Empty;
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(DesKey.Substring(0, 8)),
                IV = Encoding.ASCII.GetBytes(DesKey.Substring(0, 8))
            };
            byte[] bytes = Encoding.Default.GetBytes(text);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            foreach (byte num in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num);
            }
            stream.Close();
            return builder.ToString();
        }

        /// <summary>
        /// Decrypts the by DES.
        /// </summary>
        /// <param name="ciphertext">The ciphertext.</param>
        /// <returns></returns>
        public static string DecryptDes(this string ciphertext)
        {
            if (ciphertext.IsNullOrEmpty()) return string.Empty;
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(DesKey.Substring(0, 8)),
                IV = Encoding.ASCII.GetBytes(DesKey.Substring(0, 8))
            };
            byte[] buffer = new byte[ciphertext.Length / 2];
            for (int i = 0; i < (ciphertext.Length / 2); i++)
            {
                int num2 = Convert.ToInt32(ciphertext.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)num2;
            }
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            stream.Close();
            return Encoding.Default.GetString(stream.ToArray());
        }
    }
}