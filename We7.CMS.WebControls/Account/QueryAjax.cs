using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS;
using We7;
using We7.CMS.Common;
using We7.CMS.Common.PF;
using System.Text.RegularExpressions;
using We7.Framework;
using Thinkment.Data;
using System.Web;
using We7.CMS.Accounts;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 评论数据处理对象
    /// </summary>
    public partial class QueryAjax : IHttpHandler
    {
        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID]; }
        }

        /// <summary>
        /// 帐户对象
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        void CheckUserName()
        {            
            string userName = We7Helper.FilterHtmlChars(HttpContext.Current.Request["userName"]);
            HttpContext.Current.Response.Clear();
            int length = GetStrLen(userName);
            if (userName == "")
            {
                HttpContext.Current.Response.Write("false|用户名不能为空");
            }
            else if (length < 5 || length > 20)
            {
                HttpContext.Current.Response.Write("false|用户名必须是5-20位");
            }
            else if (!Regex.IsMatch(userName, @"^[\u4E00-\u9FA5a-zA-Z0-9]+$"))
            {
                HttpContext.Current.Response.Write("false|用户名必须是必须是字母、数字或组合");
            }
            else if (AccountHelper.ExistUserName(userName))
            {
                HttpContext.Current.Response.Write("false|该会员名已被使用");
            }
            else
            {
                HttpContext.Current.Response.Write("true|");
            }
            HttpContext.Current.Response.End();
        }

        void CheckEmail()
        {
            string email = We7Helper.FilterHtmlChars(HttpContext.Current.Request["email"]);
            HttpContext.Current.Response.Clear();
            if (email == "")
            {
                HttpContext.Current.Response.Write("false|Email不能为空");
            }
            else if (!Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                HttpContext.Current.Response.Write("false|Email格式不正确");
            }
            else if (AccountHelper.ExistEmail(email))
            {
                HttpContext.Current.Response.Write("false|该电子邮箱名已被使用");
            }
            else
            {
                HttpContext.Current.Response.Write("true|");
            }
            HttpContext.Current.Response.End();
        }

        void CheckValidateCode()
        {
            string validateCode = HttpContext.Current.Request["validateCode"];
            HttpContext.Current.Response.Clear();
            if (String.Compare(validateCode, HttpContext.Current.Request.Cookies["CheckCode"].Value, true) != 0)
            {
                HttpContext.Current.Response.Write("false");
            }
            else
            {
                HttpContext.Current.Response.Write("true");
            }
            HttpContext.Current.Response.End();         
        }

        //得到字符长度（一个汉字占两个字符）
        int GetStrLen(String ss)
        {
            Char[] cc = ss.ToCharArray();
            int intLen = ss.Length;
            int i;
            if ("中文".Length == 4)
            {
                //是非 中文 的 平台
                return intLen;
            }
            for (i = 0; i < cc.Length; i++)
            {
                if (Convert.ToInt32(cc[i]) > 255)
                {
                    intLen++;
                }
            }
            return intLen;
        }

        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        public void ProcessRequest(HttpContext context)
        {

            //清空缓存
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.Expires = 0;

            if (!string.IsNullOrEmpty(HttpContext.Current.Request["name"]) && HttpContext.Current.Request["name"] == "Register.Simple.ascx" && HttpContext.Current.Request["userName"] != null)
            {
                CheckUserName();
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request["name"]) && HttpContext.Current.Request["name"] == "Register.Simple.ascx" && HttpContext.Current.Request["email"] != null)
            {
                CheckEmail();
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request["name"]) && HttpContext.Current.Request["name"] == "Register.Simple.ascx" && HttpContext.Current.Request["validateCode"] != null)
            {
                CheckValidateCode();
            }
        }

        #endregion
    }
}