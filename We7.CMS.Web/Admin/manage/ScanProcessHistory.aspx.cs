using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using We7.CMS.Controls;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
    public partial class ScanProcessHistory : BasePage
    {
        /// <summary>
        /// 是否判断用户权限
        /// </summary>
        //protected override bool NeedAnPermission
        //{
        //    get
        //    {
        //        if (AccountHelper.GetAccountUserType(AccountID))
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //}

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        public string ArticleID
        {
            get
            {
                if (Request["id"] != null && Request["id"] != "")
                {
                    return Request["id"].ToString();
                }
                else
                    return "";
            }
        }
        public string AdviceID
        {
            get
            {
                if (Request["adviceID"] != null && Request["adviceID"] != "")
                {
                    return Request["adviceID"].ToString();
                }
                else
                    return "";
            }
        }

        public string Titles
        {
            get
            {
                if (ArticleID != null || ArticleID != "")
                {
                    Article a = ArticleHelper.GetArticle(ArticleID);
                    if (a != null)
                        return a.Title;
                    else
                        return "";
                }
                if (AdviceID != null || AdviceID != "")
                {
                    Advice advice = AdviceHelper.GetAdvice(AdviceID);
                    if (advice != null)
                    {
                        return advice.Title;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                    return "";
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //this.Master.SiteHeadTitle = SiteHeadTitle;
            //this.Master.TitleName = "查看历史";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TitleLabel.Text = Titles;
            }
        }
      
       
    }
}
