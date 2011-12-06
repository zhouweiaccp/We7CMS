using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace We7.CMS.Accounts
{
    [Serializable]
    public class SSORequest : MarshalByRefObject
    {
        /// <summary>
        /// 各独立站点标识ID
        /// </summary>
        public string SiteID { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// 各独立站点的访问地址
        /// </summary>
        public string AppUrl { get; set; }
        /// <summary>
        /// 各独立站点的 Token
        /// </summary>
        public string Authenticator { get; set; }
        /// <summary>
        /// 账号名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 账号ID
        /// </summary>
        public string AccountID { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 操作：signin,signout,authenticate
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 代表已经验证过的Url
        /// </summary>
        public string FromUrls { get; set; }

        /// <summary>
        /// 代表将要验证的Url
        /// </summary>
        public string ToUrls { get; set; }

        //为ssresponse对象做准备
        public string ErrorDescription = "认证失败";   //用户认证通过,认证失败,包数据格式不正确,数据校验不正确
        public int Result = -1;

        public SSORequest()
        {
            TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            AppUrl =HttpContext.Current.Request.Url.Scheme+"://"+HttpContext.Current.Request.Url.Host+":"+HttpContext.Current.Request.Url.Port+HttpContext.Current.Request.RawUrl;
        }


        /// <summary>
        /// 获取当前页面上的SSORequest对象
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <returns></returns>
        public static SSORequest GetRequest(HttpContext CurrentPage)
        {
            SSORequest request = new SSORequest();
            request.IPAddress = CurrentPage.Request.UserHostAddress;
            request.Action = CurrentPage.Request["Action"];
            request.SiteID = CurrentPage.Request["SiteID"];
            request.AccountID = CurrentPage.Request["AccountID"];
            request.UserName = CurrentPage.Request["UserName"];
            request.Password = CurrentPage.Request["Password"];
            request.AppUrl = CurrentPage.Request["AppUrl"];
            request.Authenticator = CurrentPage.Request["Authenticator"];
            request.TimeStamp = CurrentPage.Request["TimeStamp"];
            request.ToUrls = CurrentPage.Request["ToUrls"];
            request.FromUrls = CurrentPage.Request["FromUrls"];
            return request;
        }
    }
}