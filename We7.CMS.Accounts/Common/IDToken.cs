using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.PF
{
    /// <summary>
    /// 站群通行证令牌信息
    /// </summary>
    [Serializable]
    public class IDToken
    {
       private string passportID;
       private string userName;
       private string providerSiteID;
       private string providerSiteTitle;
       private string loginSiteID;
       private string loginSiteTitle;
       private int status;
       private string returnUrl;
       private string action;
       private List<string> existList;

        public IDToken()
        {
        }

        /// <summary>
        /// 护照ID
        /// </summary>
        public string PassportID
        {
            get { return passportID; }
            set { passportID = value; }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 所提供的站点ID
        /// </summary>
        public string ProviderSiteID
        {
            get { return providerSiteID; }
            set { providerSiteID = value; }
        }

        /// <summary>
        /// 所提供的站点名称
        /// </summary>
        public string ProviderSiteTitle
        {
            get { return providerSiteTitle; }
            set { providerSiteTitle = value; }
        }

        /// <summary>
        /// 注册的站点ID
        /// </summary>
        public string LoginSiteID
        {
            get { return loginSiteID; }
            set { loginSiteID = value; }
        }

        /// <summary>
        /// 注册的站点名称
        /// </summary>
        public string LoginSiteTitle
        {
            get { return loginSiteTitle; }
            set { loginSiteTitle = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// 站点跳转地址
        /// </summary>
        public string ReturnUrl
        {
            get { return returnUrl; }
            set { returnUrl = value; }
        }

        /// <summary>
        /// 所操作的动作
        /// </summary>
        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> ExistList
        {
            get { return existList; }
            set { existList = value; }
        }


    }
}
