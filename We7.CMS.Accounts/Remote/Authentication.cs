using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Config;
using We7.Framework.Util;

namespace We7.CMS.Accounts
{
    /// <summary>
    /// 安全验证类
    /// </summary>
    public class Authentication
    {
        static readonly string cookieName = "EACToken";
        static readonly string hashSplitter = "|";

        public Authentication()
        {
        }

        public static string GetAppKey(int appID)
        {
            //string cmdText = @"select * from ";
            return string.Empty;
        }

        public static string GetAppKey()
        {
            return "22362E7A9285DD53A0BBC2932F9733C505DC04EDBFE00D70";
        }

        public static string GetAppIV()
        {
            return "1E7FA9231E7FA923";
        }

        /// <summary>
        /// 取得加密服务
        /// </summary>
        /// <returns></returns>
        static CryptoService GetCryptoService()
        {
            string key = GetAppKey();
            string IV = GetAppIV();

            CryptoService cs = new CryptoService(key, IV);
            return cs;
        }

        /// <summary>
        /// 创建各分站发往认证中心的 Token
        /// </summary>
        /// <param name="ssoRequest"></param>
        /// <returns></returns>
        public static bool CreateAppToken(SSORequest ssoRequest)
        {
            string OriginalAuthenticator = ssoRequest.SiteID + ssoRequest.TimeStamp + ssoRequest.AppUrl;
            string AuthenticatorDigest = CryptoHelper.ComputeHashString(OriginalAuthenticator);
            string sToEncrypt = OriginalAuthenticator + AuthenticatorDigest;
            byte[] bToEncrypt = CryptoHelper.ConvertStringToByteArray(sToEncrypt);

            CryptoService cs = GetCryptoService();

            byte[] encrypted;

            if (cs.Encrypt(bToEncrypt, out encrypted))
            {
                ssoRequest.Authenticator = CryptoHelper.ToBase64String(encrypted);

                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 验证从各分站发送过来的 Token
        /// </summary>
        /// <param name="ssoRequest"></param>
        /// <returns></returns>
        public static bool ValidateAppToken(SSORequest ssoRequest)
        {
            string Authenticator = ssoRequest.Authenticator;

            string OriginalAuthenticator = ssoRequest.SiteID + ssoRequest.TimeStamp + ssoRequest.AppUrl;
            string AuthenticatorDigest = CryptoHelper.ComputeHashString(OriginalAuthenticator);
            string sToEncrypt = OriginalAuthenticator + AuthenticatorDigest;
            byte[] bToEncrypt = CryptoHelper.ConvertStringToByteArray(sToEncrypt);

            CryptoService cs = GetCryptoService();
            byte[] encrypted;

            if (cs.Encrypt(bToEncrypt, out encrypted))
            {
                return Authenticator == CryptoHelper.ToBase64String(encrypted);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 创建认证中心发往各分站的 Token
        /// </summary>
        /// <param name="ssoRequest"></param>
        /// <returns></returns>
        public static bool CreateEACToken(SSORequest ssoRequest)
        {
            string OriginalAuthenticator = ssoRequest.AccountID + ssoRequest.SiteID + ssoRequest.TimeStamp + ssoRequest.AppUrl;
            string AuthenticatorDigest = CryptoHelper.ComputeHashString(OriginalAuthenticator);
            string sToEncrypt = OriginalAuthenticator + AuthenticatorDigest;
            byte[] bToEncrypt = CryptoHelper.ConvertStringToByteArray(sToEncrypt);

            CryptoService cs = GetCryptoService();
            byte[] encrypted;

            if (cs.Encrypt(bToEncrypt, out encrypted))
            {
                ssoRequest.Authenticator = CryptoHelper.ToBase64String(encrypted);

                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 验证从认证中心发送过来的 Token
        /// </summary>
        /// <param name="ssoRequest"></param>
        /// <returns></returns>
        public static bool ValidateEACToken(SSORequest ssoRequest)
        {
            string Authenticator = ssoRequest.Authenticator;

            string OriginalAuthenticator = ssoRequest.AccountID + ssoRequest.SiteID + ssoRequest.TimeStamp + ssoRequest.AppUrl;
            string AuthenticatorDigest = CryptoHelper.ComputeHashString(OriginalAuthenticator);
            string sToEncrypt = OriginalAuthenticator + AuthenticatorDigest;
            byte[] bToEncrypt = CryptoHelper.ConvertStringToByteArray(sToEncrypt);

            string EncryCurrentAuthenticator = string.Empty;
            CryptoService cs = GetCryptoService();
            byte[] encrypted;

            if (cs.Encrypt(bToEncrypt, out encrypted))
            {
                EncryCurrentAuthenticator = CryptoHelper.ToBase64String(encrypted);

                return Authenticator == EncryCurrentAuthenticator;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 发送Token信息到验证服务器或子站
        /// </summary>
        /// <param name="ssoRequest"></param>
        /// <param name="url"></param>
        public static void Post(SSORequest ssoRequest, string url)
        {
            PostService ps = new PostService();
            ps.Url = url;
            ps.Add("Action", ssoRequest.Action);
            if (!string.IsNullOrEmpty(ssoRequest.SiteID))
                ps.Add("SiteID", ssoRequest.SiteID);
            if (!string.IsNullOrEmpty(ssoRequest.AccountID))
                ps.Add("AccountID", ssoRequest.AccountID);
            if (!string.IsNullOrEmpty(ssoRequest.UserName))
                ps.Add("UserName", ssoRequest.UserName);
            if (!string.IsNullOrEmpty(ssoRequest.Password))
                ps.Add("Password", ssoRequest.Password);
            ps.Add("TimeStamp", ssoRequest.TimeStamp);
            ps.Add("AppUrl", ssoRequest.AppUrl);
            ps.Add("Authenticator", ssoRequest.Authenticator);

            ps.Post();
        }

        /// <summary>
        /// 按需要进行验证的Url进行验证
        /// </summary>
        /// <param name="ssoRequest"></param>
        public static void PostChains(SSORequest ssoRequest)
        {
            string leaveToUrls = String.Empty;
            string url = GetCurrentUrl(ssoRequest.ToUrls, ref leaveToUrls);
            ssoRequest.ToUrls = leaveToUrls;

            if (!String.IsNullOrEmpty(url))
            {
                PostService ps = new PostService();
                ps.Url = url;
                ps.Add("Action", ssoRequest.Action);
                ps.Add("ToUrls", ssoRequest.ToUrls);
                if (!string.IsNullOrEmpty(ssoRequest.UserName))
                    ps.Add("UserName", ssoRequest.UserName);
                if (!string.IsNullOrEmpty(ssoRequest.Password))
                    ps.Add("Password", ssoRequest.Password);
                ps.Add("AppUrl", ssoRequest.AppUrl);

                ps.Post();
            }
            else
            {
                HttpContext.Current.Response.Redirect(ssoRequest.AppUrl);
            }
        }

        private static string GetCurrentUrl(string toUrls,ref string leaveUrl)
        {
            string url = String.Empty;

            if (!String.IsNullOrEmpty(toUrls) && !String.IsNullOrEmpty(toUrls.Trim()))
            {
                string[] urls = toUrls.Split(';');
                url = urls[0].Trim();
                StringBuilder sb=new StringBuilder();
                for(int i=1;i<urls.Length;i++)
                {
                    sb.Append(urls[i]).Append(";");
                }
                Utils.TrimEndStringBuilder(sb, ";");
                leaveUrl = sb.ToString();
            }

            return url;
        }
    }
}