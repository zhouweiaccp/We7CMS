using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using We7.Framework.Util;
using System.Net;
using System.IO;
using We7.CMS.Accounts;
using We7.CMS.Common.PF;

namespace We7
{
    public class RemoteServiceHelper
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
         public string ConfigPath { get; set; }

         public RemoteServiceHelper(string config)
        {
            ConfigPath = config;
        }

        /// <summary>
        /// 检查远程用户方法
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
         public int CheckUserName(string username)
         {

             var error = 0;
             XmlDocument doc = new XmlDocument();
             doc.Load(ConfigPath);

             var nodes = doc.DocumentElement.SelectNodes("//item");

             for (int i = 0; i < nodes.Count; i++)
             {
                 try
                 {
                     var url = nodes[i].Attributes["url"].Value;
                     var captcha = nodes[i].Attributes["captcha"].Value;
                     url = RequestHelper.AddOrUpdateParam(url, "action", "check");
                     url = RequestHelper.AddOrUpdateParam(url, "username", username);
                     url = RequestHelper.AddOrUpdateParam(url, "captcha", captcha);

                     HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                     request.Timeout = 300000;
                     HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                     StreamReader srContent = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));

                     string result = srContent.ReadToEnd();
                     if (string.Compare(result, "0", true) != 0)
                     {
                         //添加成功
                         error++;
                     }

                 }
                 catch (Exception ex)
                 {
                     //LOG
                     error++;
                     continue;
                 }


             }

             return error;
         }
        /// <summary>
        /// 添加远程用户方法
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public int Add(string username, string pwd, string email)
        {
            var error = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load(ConfigPath);

           var nodes= doc.DocumentElement.SelectNodes("//item");

           for (int i = 0; i < nodes.Count; i++)
           {
               try
               {
                   var url = nodes[i].Attributes["url"].Value;
                   var captcha = nodes[i].Attributes["captcha"].Value;
                   url = RequestHelper.AddOrUpdateParam(url, "action", "add");
                   url = RequestHelper.AddOrUpdateParam(url, "username", username);
                   url = RequestHelper.AddOrUpdateParam(url, "pwd", pwd);
                   url = RequestHelper.AddOrUpdateParam(url, "email", email);
                   url = RequestHelper.AddOrUpdateParam(url, "captcha", captcha);

                   HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                   request.Timeout = 300000;
                   HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                   StreamReader srContent = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));

                   string result = srContent.ReadToEnd();
                   if (string.Compare(result,"0",true)!=0)
                   {
                       //添加成功
                       error++;
                   }

               }
               catch (Exception ex)
               {
                   //LOG
                   error++;
                   continue;
               }


           }

           return error;
        }
    }
}
namespace We7.CMS.Web.Install
{
    /// <summary>
    /// 远程调用API:返回-1,标示操作码不正确,0标示正常,1标示失败,2标示异常
    /// </summary>
    public partial class UserServiceAPI : System.Web.UI.Page
    {
        /// <summary>
        /// 当前站点远程调用User校验码
        /// </summary>
        private const string CurrentCaptcha = "Test";
        /// <summary>
        /// 操作类型:add为添加用户check为检查用户名是否存在
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 传递过来的校验码
        /// </summary>
        public string Captcha { get; set; }

        /// <summary>
        /// LOAD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
          
            //获取参数
            Action = Request["action"];
            Captcha = Request["captcha"];

            //检查校验码,区分大小写
            if (string.Compare(Captcha, CurrentCaptcha, false) != 0)
            {
                //验证码错误
                Response.Write("-1");
                Response.Flush();
                Response.End();
            }

            //添加操作
            if (string.Compare(Action, "add", true) == 0)
            {
                string username = Request["username"];
                string pwd = Request["pwd"];
                string email = Request["email"];

                AddUser(username, pwd, email);
            }
            //检查用户名
            else if (string.Compare(Action, "check", true) == 0)
            {
                string username = Request["username"];
                CheckUserExsit(username);
            }
            else
            {
                Response.Write("-1");
                Response.Flush();
                Response.End();
            }
        }

        /// <summary>
        /// 检查用户名是否存在,返回0标示存在,1标示不存在,2标示异常
        /// </summary>
        /// <param name="username">用户名</param>
        
        protected void CheckUserExsit(string username)
        {
            AccountRemoteHelper accountHelper = new AccountRemoteHelper();
            var exsit = accountHelper.ExistUserName(username);

            if (exsit)
            {
                Response.Write("0");
            }
            else
            {
                Response.Write("1");
            }
            Response.Flush();
            Response.End();
        }
        /// <summary>
        /// 添加用户返回,0标示添加成功,1标示失败,2标示异常
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <param name="email"></param>
        protected void AddUser(string username, string pwd, string email)
        {
            AccountRemoteHelper accountHelper = new AccountRemoteHelper();
            Account account = new Account();
            account.LastName = username;
            account.Password = pwd;
            account.Email = email;
            try
            {
                accountHelper.AddAccount(account);
                Response.Write("0");

            }
            catch
            {
                Response.Write("1");
            }

            Response.Flush();
            Response.End();
        }
    }
}
