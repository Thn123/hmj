using System;
using System.Security.Cryptography;
using System.Text;

namespace Hmj.WebApp.Common.Pages
{
    public class CenshEncryption
    {
        public static string Encrypt3DES(string strString)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Key = Encoding.UTF8.GetBytes("censhdes");
            DES.Mode = CipherMode.ECB;
            DES.Padding = PaddingMode.Zeros;

            ICryptoTransform DESEncrypt = DES.CreateEncryptor();

            byte[] Buffer = Encoding.UTF8.GetBytes(strString);

            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }
    }
}