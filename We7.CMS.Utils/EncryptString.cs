using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using We7.Framework.Config;

namespace We7.CMS
{
    public class EncryptString
    {
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="rgbKey">加密密钥,要求为8位</param>
        /// <param name="rgbIV">密钥向量</param>
        /// <returns>加密成功返回加密后的字串，失败返Null</returns>
        public byte[] DES_Encrypt(string encryptString, byte[] rgbKey, byte[] rgbIV)
        {
            try
            {
                if (encryptString != null && encryptString != "")
                {
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                    cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cryptoStream.FlushFinalBlock();
                    if (memoryStream != null)
                    {
                        return memoryStream.ToArray();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            {
                throw ex;
                //return null;
            }
        } 

        ///   <summary> 
        ///   DES解密字符串 
        ///   </summary> 
        ///   <param   name= "decryptString "> 待解密的字符串 </param> 
        ///   <param   name= "rgbKey "> 解密密钥,要求为8位,和加密密钥相同 </param> 
        ///   <param   name= "rgbIV "> 密钥向量 </param> 
        ///   <returns> 解密成功返回解密后的字符串，失败返源字符串 </returns> 
        public string DES_Decrypt(byte[] decryptByteArray, byte[] rgbKey, byte[] rgbIV)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cryptoStream.Write(decryptByteArray, 0, decryptByteArray.Length);
                cryptoStream.FlushFinalBlock();
                if (memoryStream != null)
                {
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
                else
                {
                    return "False";
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        
        private static  string skey = "xbdongli";
        public static string Encrypt(string pToEncrypt)
        {
            return pToEncrypt;
            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            if (ci != null && ci.JiaMiKey.Length == 8)
            {
                skey = ci.JiaMiKey;
            }            
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中  
            //原来使用的UTF8编码，我改成Unicode编码了，不行  
            if (pToEncrypt == null)
            {
                return "";
                throw new Exception();
            }
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[]  inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);  

            //建立加密对象的密钥和偏移量  
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法  
            //使得输入密码必须输入英文文本  
            des.Key = ASCIIEncoding.ASCII.GetBytes(skey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(skey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //Write  the  byte  array  into  the  crypto  stream  
            //(It  will  end  up  in  the  memory  stream)  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get  the  data  back  from  the  memory  stream,  and  into  a  string  
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                //Format  as  hex  
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        //解密方法  
        public static string Decrypt(string pToDecrypt)
        {
            return pToDecrypt;
            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            if (ci != null && ci.JiaMiKey.Length == 8)
            {
                skey = ci.JiaMiKey;
            }
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //Put  the  input  string  into  the  byte  array  
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                //建立加密对象的密钥和偏移量，此值重要，不能修改  
                des.Key = ASCIIEncoding.ASCII.GetBytes(skey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(skey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                //Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                //Get  the  decrypted  data  back  from  the  memory  stream  
                //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象  
                StringBuilder ret = new StringBuilder();

                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                //throw ex;
                System.Web.HttpContext.Current.Response.Redirect("/Nonexistence.htm");
                return "";
            }
        }

    }
}
