using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.CMS.Config;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class ClickRecordPage : BasePage
    {
        string RequestUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["url"]))
                    return Request["url"];
                else
                    return string.Empty;
            }
        }
        string Action
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["action"]))
                    return Request["action"];
                else
                    return string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //清空缓存
                Response.CacheControl = "no-cache";
                Response.Expires = 0;

                if (Action == "add" && !string.IsNullOrEmpty(RequestUrl))
                {
                    AddClick(RequestUrl);
                }

                Response.Clear();
                Response.Write("ok");
                Response.End();
            }
        }

        /// <summary>
        /// 添加记录过程
        /// </summary>
        /// <param name="url">文章url</param>
        protected void AddClick(string url)
        {
            //从URL获取文章SN
            string ArticleID = ArticleHelper.GetArticleIDFromURL(url);
            //string ArticleID = ArticleHelper.GetArticleIDBySN(SN);
            if (!string.IsNullOrEmpty(ArticleID))
            {
                //获取文章实体
                Article article = ArticleHelper.GetArticle(ArticleID);
                if (article != null)
                {
                    //保存日记录
                    ClickRecords cr = new ClickRecords();
                    cr.ObjectID = article.ID;
                    cr.VisitDate = ClickRecordHelper.ConvertIntegerFromDate(DateTime.Now);
                    cr.ObjectType =String.IsNullOrEmpty(article.ModelName)? "Article" :article.ModelName;
                    ClickRecordHelper.SaveClickRecord(cr);

                    //获取点击量报表
                    Dictionary<string, int> dictReports
                        = ClickRecordHelper.GetObjectClickReport(cr.ObjectType, article.ID);
                    //通过工厂获取具体的Helper
                    IObjectClickHelper helper = ClickHelperFactory.Create(article.ModelName);
                    helper.UpdateClicks(article.ModelName, article.ID, dictReports);
                }
                else
                {
                    AdviceInfo advice = AdviceFactory.Create().GetAdvice(ArticleID);
                    if (advice != null)
                    {
                        //保存日记录
                        ClickRecords cr = new ClickRecords();
                        cr.ObjectID = advice.ID;
                        cr.VisitDate = ClickRecordHelper.ConvertIntegerFromDate(DateTime.Now);
                        cr.ObjectType = "Advice";
                        ClickRecordHelper.SaveClickRecord(cr);

                        //获取点击量报表
                        Dictionary<string, int> dictReports
                            = ClickRecordHelper.GetObjectClickReport(cr.ObjectType, advice.ID);

                        IObjectClickHelper helper = ClickHelperFactory.Create(cr.ObjectType);
                        helper.UpdateClicks(cr.ObjectType, cr.ObjectID, dictReports);
                    }
                }
            }
        }
    }
}
