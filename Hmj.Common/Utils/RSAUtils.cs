using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using static Hmj.Common.Utils.RSAExtensions;

namespace Hmj.Common.Utils
{
    public static class RSAUtils
    {

        private static string publicKeyPEM = "-----BEGIN PUBLIC KEY-----MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCl7ccmBDxTf0rhzEPgi8678WMKeTSea5twar08eDdl9y6bkLlYk1dINoRzEh3EqZwEKFFXkYgnFw0AOHmxKIgwB0Xz4HRhgViGsfQmhm4kJGjGQpzEzAbBlxufNeLowRDhrbljlM7Tx+bQ6/8iKm4sF3cSop8xRmo+UWiIoHYEPQIDAQAB-----END PUBLIC KEY-----";

        private static string privateKeyPEM = "-----BEGIN RSA PRIVATE KEY-----MIICXAIBAAKBgQCl7ccmBDxTf0rhzEPgi8678WMKeTSea5twar08eDdl9y6bkLlYk1dINoRzEh3EqZwEKFFXkYgnFw0AOHmxKIgwB0Xz4HRhgViGsfQmhm4kJGjGQpzEzAbBlxufNeLowRDhrbljlM7Tx+bQ6/8iKm4sF3cSop8xRmo+UWiIoHYEPQIDAQABAoGBAIFRiQV7BZ05tx21+izWKYBWyA7Qmg7h5No/hk7Ljrl8ZSm/KIT9CGhyfNXGB8dPPRYMaiDqfoegsnQ6j7Vf1or7qk6nnADOyhcfP92flZ3wHdnLzc123S51XxI74wmrNXsO4lJpNxPzmsuuYrN8cmcx2KmqcbJrrLuKWHewQ6DBAkEA7P4HYz0ZQ2RWXnN0mn2MdN+3EqViLjTlO5kyyZwW3DPj9sCwTGTjtheM3iN+8cEbFc0FBABh33Eo9DwV2xPdUQJBALM8qP4NWrjx4PBTE3f+vSgzihB1KTmrwzv9uMGs6ZHT+RtZyX5Yj2iwimRnafz32Q5Lmo7CrAfIlfuItbVLDS0CQBhwIYbkOASxBg77TNzZcXBj2Vb84uDs525737bWd60BVNKPEB7wkGKojwghFOgNB6P53jiJaY5G9vgocgCDTeECQGtQi4Io7sPDFsHti7+RxyG10hlOfNNp0ugtXpyfce19NC47ERhT3/F3mjTJcj0jDFOx0qVdS3ERmTNURC968zUCQDR6eOw6kopG7t8uBGgAu9l2KmaoZ7DO7IekBRhRilQwt5ib0Ij0exClKvpQgy+gx6ZSWD2lYuEmhB2RvVOorlo=-----END RSA PRIVATE KEY-----";


        private static string Siyao_publicKeyPEM = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCl7ccmBDxTf0rhzEPgi8678WMKeTSea5twar08eDdl9y6bkLlYk1dINoRzEh3EqZwEKFFXkYgnFw0AOHmxKIgwB0Xz4HRhgViGsfQmhm4kJGjGQpzEzAbBlxufNeLowRDhrbljlM7Tx+bQ6/8iKm4sF3cSop8xRmo+UWiIoHYEPQIDAQAB";

        private static string Siyao_privateKeyPEM = "MIICXAIBAAKBgQCl7ccmBDxTf0rhzEPgi8678WMKeTSea5twar08eDdl9y6bkLlYk1dINoRzEh3EqZwEKFFXkYgnFw0AOHmxKIgwB0Xz4HRhgViGsfQmhm4kJGjGQpzEzAbBlxufNeLowRDhrbljlM7Tx+bQ6/8iKm4sF3cSop8xRmo+UWiIoHYEPQIDAQABAoGBAIFRiQV7BZ05tx21+izWKYBWyA7Qmg7h5No/hk7Ljrl8ZSm/KIT9CGhyfNXGB8dPPRYMaiDqfoegsnQ6j7Vf1or7qk6nnADOyhcfP92flZ3wHdnLzc123S51XxI74wmrNXsO4lJpNxPzmsuuYrN8cmcx2KmqcbJrrLuKWHewQ6DBAkEA7P4HYz0ZQ2RWXnN0mn2MdN+3EqViLjTlO5kyyZwW3DPj9sCwTGTjtheM3iN+8cEbFc0FBABh33Eo9DwV2xPdUQJBALM8qP4NWrjx4PBTE3f+vSgzihB1KTmrwzv9uMGs6ZHT+RtZyX5Yj2iwimRnafz32Q5Lmo7CrAfIlfuItbVLDS0CQBhwIYbkOASxBg77TNzZcXBj2Vb84uDs525737bWd60BVNKPEB7wkGKojwghFOgNB6P53jiJaY5G9vgocgCDTeECQGtQi4Io7sPDFsHti7+RxyG10hlOfNNp0ugtXpyfce19NC47ERhT3/F3mjTJcj0jDFOx0qVdS3ERmTNURC968zUCQDR6eOw6kopG7t8uBGgAu9l2KmaoZ7DO7IekBRhRilQwt5ib0Ij0exClKvpQgy+gx6ZSWD2lYuEmhB2RvVOorlo=";

        #region 公钥加密，私钥解密
        static void EncryptByPublicKey()
        {
            var PlainText = "I need to be encrypted";
            var sign = RSAUtils.EncryptPEM("", PlainText);
            string decryptResult = RSAUtils.DecryptPEM("", sign);
        }

        /// <summary>Extracts the binary data from a PEM file.</summary>
        static byte[] GetDERFromPEM(string sPEM)
        {
            UInt32 dwSkip, dwFlags;
            UInt32 dwBinarySize = 0;

            if (!CryptStringToBinary(sPEM, (UInt32)sPEM.Length, CRYPT_STRING_FLAGS.CRYPT_STRING_BASE64HEADER, null, ref dwBinarySize, out dwSkip, out dwFlags))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            byte[] decodedData = new byte[dwBinarySize];
            if (!CryptStringToBinary(sPEM, (UInt32)sPEM.Length, CRYPT_STRING_FLAGS.CRYPT_STRING_BASE64HEADER, decodedData, ref dwBinarySize, out dwSkip, out dwFlags))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return decodedData;
        }

        /// <summary>Extension method which initializes an RSACryptoServiceProvider from a DER public key blob.</summary>
        public static void LoadPublicKeyDER(this RSACryptoServiceProvider provider, byte[] DERData)
        {
            byte[] RSAData = GetRSAFromDER(DERData);
            byte[] publicKeyBlob = GetPublicKeyBlobFromRSA(RSAData);
            provider.ImportCspBlob(publicKeyBlob);
        }

        /// <summary>
        /// RSA加密PEM秘钥
        /// </summary>
        /// <param name="publicKeyPEM"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncryptPEM(string publicKeyPEM, string data, string encoding = "UTF-8")
        {
            publicKeyPEM = RSAUtils.publicKeyPEM;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.LoadPublicKeyPEM(publicKeyPEM);

            //☆☆☆☆.NET 4.6以后特有☆☆☆☆
            //HashAlgorithmName hashName = new System.Security.Cryptography.HashAlgorithmName(hashAlgorithm);
            //RSAEncryptionPadding padding = RSAEncryptionPadding.OaepSHA512;//RSAEncryptionPadding.CreateOaep(hashName);//.NET 4.6以后特有               
            //cipherbytes = rsa.Encrypt(Encoding.GetEncoding(encoding).GetBytes(data), padding);
            //☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆

            //☆☆☆☆.NET 4.6以前请用此段代码☆☆☆☆
            cipherbytes = rsa.Encrypt(Encoding.GetEncoding(encoding).GetBytes(data), false);

            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privateKeyPEM"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string DecryptPEM(string privateKeyPEM, string data, string encoding = "UTF-8")
        {
            privateKeyPEM = RSAUtils.privateKeyPEM;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.LoadPrivateKeyPEM(privateKeyPEM);
            //☆☆☆☆.NET 4.6以后特有☆☆☆☆
            //RSAEncryptionPadding padding = RSAEncryptionPadding.CreateOaep(new System.Security.Cryptography.HashAlgorithmName(hashAlgorithm));//.NET 4.6以后特有        
            //cipherbytes = rsa.Decrypt(Encoding.GetEncoding(encoding).GetBytes(data), padding);
            //☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆

            //☆☆☆☆.NET 4.6以前请用此段代码☆☆☆☆
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(data), false);

            return Encoding.GetEncoding(encoding).GetString(cipherbytes);
        }

        #endregion

        #region 私钥加密、公钥解密

        static void EncryptByPrivateKey()
        {
            var PlainText = "I need to be encrypted";
            var sign = RSAUtils.RSAEncryptByPrivateKey("", PlainText);
            string decryptResult = RSAUtils.DecryptPEM("", sign);
        }
        #region 加签      
        /// <summary>
        /// RSA私钥格式转换，java->.net
        /// </summary>
        /// <param name="privateKey">java生成的RSA私钥</param>
        /// <returns></returns>
        public static string RSAPrivateKeyJava2DotNet(string privateKey)
        {
            var b = Convert.FromBase64String(privateKey);
            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(b);

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        /// <summary>用私钥给数据进行RSA加密
        /// 
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="m_strEncryptString">待加密数据</param>
        /// <returns>加密后的数据（Base64）</returns>
        public static string RSAEncryptByPrivateKey(string xmlPrivateKey, string strEncryptString)
        {
            //xmlPrivateKey = RSAPrivateKeyJava2DotNet(RSAUtils.Siyao_privateKeyPEM);
            xmlPrivateKey = "<RSAKeyValue><Modulus>pe3HJgQ8U39K4cxD4IvOu/FjCnk0nmubcGq9PHg3Zfcum5C5WJNXSDaEcxIdxKmcBChRV5GIJxcNADh5sSiIMAdF8+B0YYFYhrH0JoZuJCRoxkKcxMwGwZcbnzXi6MEQ4a25Y5TO08fm0Ov/IipuLBd3EqKfMUZqPlFoiKB2BD0=</Modulus><Exponent>AQAB</Exponent><P>7P4HYz0ZQ2RWXnN0mn2MdN+3EqViLjTlO5kyyZwW3DPj9sCwTGTjtheM3iN+8cEbFc0FBABh33Eo9DwV2xPdUQ==</P><Q>szyo/g1auPHg8FMTd/69KDOKEHUpOavDO/24wazpkdP5G1nJfliPaLCKZGdp/PfZDkuajsKsB8iV+4i1tUsNLQ==</Q><DP>GHAhhuQ4BLEGDvtM3NlxcGPZVvzi4OznbnvfttZ3rQFU0o8QHvCQYqiPCCEU6A0Ho/neOIlpjkb2+ChyAINN4Q==</DP><DQ>a1CLgijuw8MWwe2Lv5HHIbXSGU5802nS6C1enJ9x7X00LjsRGFPf8XeaNMlyPSMMU7HSpV1LcRGZM1REL3rzNQ==</DQ><InverseQ>NHp47DqSikbu3y4EaAC72XYqZqhnsM7sh6QFGFGKVDC3mJvQiPR7EKUq+lCDL6DHplJYPaVi4SaEHZG9U6iuWg==</InverseQ><D>gVGJBXsFnTm3HbX6LNYpgFbIDtCaDuHk2j+GTsuOuXxlKb8ohP0IaHJ81cYHx089FgxqIOp+h6CydDqPtV/WivuqTqecAM7KFx8/3Z+VnfAd2cvNzXbdLnVfEjvjCas1ew7iUmk3E/Oay65is3xyZzHYqapxsmusu4pYd7BDoME=</D></RSAKeyValue>";
            //加载私钥
            RSACryptoServiceProvider privateRsa = new RSACryptoServiceProvider();
            privateRsa.FromXmlString(xmlPrivateKey);

            //转换密钥
            AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetKeyPair(privateRsa);
            IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            
            // 参数与Java中加密解密的参数一致     
            //第一个参数为true表示加密，为false表示解密；第二个参数表示密钥
            c.Init(true, keyPair.Private);

            byte[] DataToEncrypt = Encoding.UTF8.GetBytes(strEncryptString);
            byte[] outBytes = c.DoFinal(DataToEncrypt);//加密
            string strBase64 = Convert.ToBase64String(outBytes);
            return strBase64;
        }

        #endregion
        
        #endregion
    }
}