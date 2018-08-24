using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Hmj.Common
{
    public class CryptographyUtils
    {
        //private readonly string _defaultLegalIV = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";



        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyManager"/> class.
        /// </summary>
        public CryptographyUtils()
        {
        }
        //public const string DEFAULT_KEY = "aslkjkljlsajsuaggasfklrjuisdhaie";

        public static string SHA1(string encryptingString)
        {
            return Encrypt(encryptingString, "sha1");
        }


        /// <summary>
        /// 单向加密方法，提供MD5、SHA1加密算法
        /// </summary>
        /// <param name="encryptingString">被加密的字符串</param>
        /// <param name="encryptFormat">加密算法，有"md5"、"sha1"、"clear"（明文，即不加密）等</param>
        /// <returns>加密后的字符串</returns>
        /// <remarks>
        /// 当参数<paramref name="encryptFormat" />不为"md5"、"sha1"、"clear"时，直接返回参数<paramref name="encryptingString" />
        /// </remarks>
        public static string Encrypt(string encryptingString, string encryptFormat)
        {
            if (string.Compare(encryptFormat, "md5", true) == 0 || string.Compare(encryptFormat, "sha1", true) == 0)
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(encryptingString, encryptFormat);
            }
            return encryptingString;
        }

        private static readonly string DESKey = "_ILOVEU_";
        /// <summary>  
        /// 执行DES加密  
        /// </summary>  
        public static string DesEncrypt(string encryptStr)
        {
            
            byte[] MyStr_E = Encoding.UTF8.GetBytes(encryptStr);
            byte[] MyKey_E = Encoding.UTF8.GetBytes(DESKey);

            DESCryptoServiceProvider MyDes_E = new DESCryptoServiceProvider();
            MyDes_E.Key = MyKey_E;
            MyDes_E.IV = MyKey_E;

            MemoryStream MyMem_E = new MemoryStream();

            CryptoStream MyCry_E = new CryptoStream(MyMem_E, MyDes_E.CreateEncryptor(), CryptoStreamMode.Write);
            MyCry_E.Write(MyStr_E, 0, MyStr_E.Length);
            MyCry_E.FlushFinalBlock();
            MyCry_E.Close();

            return Convert.ToBase64String(MyMem_E.ToArray());
        
        }

        /// <summary>  
        /// 执行DES解密  
        /// </summary>  
        public static string DesDecrypt(string decryptStr)
        {
       
            byte[] MyStr_D = Convert.FromBase64String(decryptStr);
            byte[] MyKey_D = Encoding.UTF8.GetBytes(DESKey);

            DESCryptoServiceProvider MyDes_D = new DESCryptoServiceProvider();
            MyDes_D.Key = MyKey_D;
            MyDes_D.IV = MyKey_D;

            MemoryStream MyMem_D = new MemoryStream();

            CryptoStream MyCry_D = new CryptoStream(MyMem_D, MyDes_D.CreateDecryptor(), CryptoStreamMode.Write);
            MyCry_D.Write(MyStr_D, 0, MyStr_D.Length);
            MyCry_D.FlushFinalBlock();
            MyCry_D.Close();

            return Encoding.UTF8.GetString(MyMem_D.ToArray());
           
        }  
    }
}
