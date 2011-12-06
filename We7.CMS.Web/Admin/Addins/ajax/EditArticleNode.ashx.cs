using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using We7.Framework;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.Addins.ajax
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class EditArticleNode : IHttpHandler
    {
        /// <summary>
        /// 文章业务助手
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.Instance.GetHelper<ArticleHelper>(); }
        }

        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleID
        {
            get { return HttpContext.Current.Request["id"]; }
        }

        public string ParentID
        {
            get { return HttpContext.Current.Request["ref"]; }
        }

        string Index
        {
            get { return HttpContext.Current.Request["position"]; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(ArticleID) && !string.IsNullOrEmpty(ParentID))
            {
                MoveArticleNode(ArticleID, ParentID,Index);
            }
            context.Response.Write( "{ \"status\" : 1, \"id\" :\""+ArticleID+"\" }");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        void MoveArticleNode(string id, string parentID,string index)
        {
            Article a = ArticleHelper.GetArticle(id);
            if (a != null)
            {
                int seq = 0;
                if (!string.IsNullOrEmpty(index))
                    seq = int.Parse(index);
                a.Index = seq;
                a.ParentID = parentID;
                ArticleHelper.UpdateArticle(a, new string[] { "ParentID", "Index" });
            }
        }
    }
}
