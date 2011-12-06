using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace We7.CMS.Accounts
{
    public class CryptoHelper
    {
        /// <summary>
        /// 复合 Hash：string --> byte[] --> hashed byte[] --> base64 string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ComputeHashString(string s)
        {
            return ToBase64String(ComputeHash(ConvertStringToByteArray(s)));
        }


        public static byte[] ComputeHash(byte[] buf)
        {
            //return ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(buf);
            return SHA1.Create().ComputeHash(buf);

        }

        /// <summary>
        /// //System.Convert.ToBase64String
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static string ToBase64String(byte[] buf)
        {
            return System.Convert.ToBase64String(buf);
        }


        public static byte[] FromBase64String(string s)
        {
            return System.Convert.FromBase64String(s);
        }

        /// <summary>
        /// //Encoding.UTF8.GetBytes(s)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] ConvertStringToByteArray(String s)
        {
            return Encoding.UTF8.GetBytes(s);//gb2312
        }


        public static string ConvertByteArrayToString(byte[] buf)
        {
            //return System.Text.Encoding.GetEncoding("utf-8").GetString(buf);

            return Encoding.UTF8.GetString(buf);
        }


        /// <summary>
        /// 字节数组转换为十六进制字符串
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] buf)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < buf.Length; i++)
            {
                sb.Append(buf[i].ToString("X").Length == 2 ? buf[i].ToString("X") : "0" + buf[i].ToString("X"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 十六进制字符串转换为字节数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string s)
        {
            Byte[] buf = new byte[s.Length / 2];
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(Char2Hex(s.Substring(i * 2, 1)) * 0x10 + Char2Hex(s.Substring(i * 2 + 1, 1)));
            }
            return buf;
        }


        private static byte Char2Hex(string chr)
        {
            switch (chr)
            {
                case "0":
                    return 0x00;
                case "1":
                    return 0x01;
                case "2":
                    return 0x02;
                case "3":
                    return 0x03;
                case "4":
                    return 0x04;
                case "5":
                    return 0x05;
                case "6":
                    return 0x06;
                case "7":
                    return 0x07;
                case "8":
                    return 0x08;
                case "9":
                    return 0x09;
                case "A":
                    return 0x0a;
                case "B":
                    return 0x0b;
                case "C":
                    return 0x0c;
                case "D":
                    return 0x0d;
                case "E":
                    return 0x0e;
                case "F":
                    return 0x0f;
            }
            return 0x00;
        }
    }

    public class CryptoService
    {
        /// <summary>
        /// 加密的密钥
        /// </summary>
        string sKey = "22362E7A9285DD53A0BBC2932F9733C505DC04EDBFE00D70";
        string sIV = "1E7FA9231E7FA923";

        byte[] byteKey;
        byte[] byteIV;

        /// <summary>
        /// 加密向量
        /// </summary>
        static byte[] bIV = { 1, 2, 3, 4, 5, 6, 7, 8 };

        public CryptoService()
        { }

        public CryptoService(string key, string IV)
        {
            sKey = key;
            sIV = IV;

            byteKey = CryptoHelper.HexStringToByteArray(sKey);
            byteIV = CryptoHelper.HexStringToByteArray(sIV);
        }



        /// <summary>
        /// 将明文加密，返回密文
        /// </summary>
        /// <param name="Data">要加密的字串</param>
        /// <returns></returns>
        public byte[] Encrypt(string Data)
        {
            try
            {
                byte[] ret;

                using (MemoryStream mStream = new MemoryStream())
                using (CryptoStream cStream = new CryptoStream(mStream,
                    new TripleDESCryptoServiceProvider().CreateEncryptor(byteKey, byteIV),
                    CryptoStreamMode.Write))
                {

                    byte[] toEncrypt = new ASCIIEncoding().GetBytes(Data);

                    // Write the byte array to the crypto stream and flush it.
                    cStream.Write(toEncrypt, 0, toEncrypt.Length);
                    cStream.FlushFinalBlock();

                    // Get an array of bytes from the 
                    // MemoryStream that holds the 
                    // encrypted data.
                    ret = mStream.ToArray();

                }

                return ret;
            }
            catch (CryptographicException e)
            {
                //Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }

        }


        /// <summary>
        /// 将明文加密，返回密文
        /// </summary>
        /// <param name="toEncrypt">明文</param>
        /// <param name="encrypted">密文</param>
        /// <returns></returns>
        public bool Encrypt(byte[] toEncrypt, out byte[] encrypted)
        {
            encrypted = null;
            try
            {
                // Create a new MemoryStream using the passed 
                // array of encrypted data.
                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                using (MemoryStream mStream = new MemoryStream())
                using (CryptoStream cStream = new CryptoStream(mStream,
                    new TripleDESCryptoServiceProvider().CreateEncryptor(byteKey, byteIV),
                    CryptoStreamMode.Write))
                {

                    // Write the byte array to the crypto stream and flush it.
                    cStream.Write(toEncrypt, 0, toEncrypt.Length);
                    cStream.FlushFinalBlock();

                    // Get an array of bytes from the 
                    // MemoryStream that holds the 
                    // encrypted data.
                    encrypted = mStream.ToArray();
                }

                return true;
            }
            catch (CryptographicException e)
            {
                //Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return false;
            }

        }



        /// <summary>
        /// 将明文加密，返回 Base64 字符串
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string EncryptToString(string Data)
        {
            try
            {
                string base64String = string.Empty;

                using (MemoryStream mStream = new MemoryStream())
                using (CryptoStream cStream = new CryptoStream(mStream,
                    new TripleDESCryptoServiceProvider().CreateEncryptor(byteKey, byteIV),
                    CryptoStreamMode.Write))
                {

                    byte[] toEncrypt = new ASCIIEncoding().GetBytes(Data);

                    cStream.Write(toEncrypt, 0, toEncrypt.Length);
                    cStream.FlushFinalBlock();

                    byte[] ret = mStream.ToArray();

                    base64String = Convert.ToBase64String(ret);
                }

                return base64String;
            }
            catch (CryptographicException e)
            {
                return null;
            }

        }


        /// <summary>
        /// 将密文解密，返回明文
        /// </summary>
        /// <param name="Data">密文</param>
        /// <returns>明文</returns>
        public bool Decrypt(byte[] Data, out string decrypted)
        {
            decrypted = string.Empty;
            try
            {

                using (MemoryStream msDecrypt = new MemoryStream(Data))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    new TripleDESCryptoServiceProvider().CreateDecryptor(byteKey, byteIV),
                    CryptoStreamMode.Read))
                {

                    byte[] fromEncrypt = new byte[Data.Length];

                    // Read the decrypted data out of the crypto stream
                    // and place it into the temporary buffer.
                    csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                    decrypted = Encoding.UTF8.GetString(fromEncrypt);//new ASCIIEncoding().GetString(fromEncrypt);

                    return true;
                }
            }
            catch (CryptographicException e)
            {
                return false;
            }
        }

    }

}