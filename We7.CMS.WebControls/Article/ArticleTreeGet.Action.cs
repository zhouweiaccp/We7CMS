using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS;
using We7;
using We7.CMS.Common;
using We7.CMS.Common.PF;
using System.Web;
using System.Web.SessionState;
using We7.CMS.Common.Enum;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 文章树节点加载
    /// </summary>
    public class ArticleTreeGetAction :  IHttpHandler, IRequiresSessionState  
    {
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// 文章业务助手
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.Instance.GetHelper<ArticleHelper>(); }
        }

        /// <summary>
        /// 栏目类业务助手
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.Instance.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleID
        {
            get { return HttpContext.Current.Request["ArticleID"]; }
        }

        /// <summary>
        /// 栏目ID
        /// </summary>
        public string OwnerID
        {
            get { return HttpContext.Current.Request["ChannelID"]; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string parentID  = context.Request["id"];
            if (!string.IsNullOrEmpty(parentID))
                context.Response.Write(GetArticleChildrenJasonString(parentID,null,0));
            else
                context.Response.Write(GetInitArticleTreeRoot());
        }

        /// <summary>
        /// 根节点初始化
        /// </summary>
        /// <returns></returns>
        string GetInitArticleTreeRoot()
        {
            string jasonText = @"[
	    {{ ""data"" : ""{0}"", ""children"" : {1}, ""state"" : ""open"" }}
        ]";
            Channel ch = ChannelHelper.GetChannel(OwnerID, null);

            string root = ch == null ? "所有栏目" : ch.Name;

            List<Article> paths = GetParentArticlePath();

            jasonText = string.Format(jasonText, root, GetArticleChildrenJasonString("", paths, 0));

            return jasonText.Replace("{{", "{").Replace("}}", "}");
        }

        /// <summary>
        /// 节点展开获取子列表
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="paths"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        string GetArticleChildrenJasonString(string parentID,List<Article> paths,int level)
        {
            List<Article> list = GetArticleListFromDB(parentID);
            list=FormatArticlesData(list);
            string jasonString = @"
    {{ ""data"" :  {{ 
						""title"" : ""{0}"", 
						""attr"" : {{ ""href"" : ""{1}"",""class"":""{2}"",""title"":""{0}"" }} 
					}} ,
    ""state"" : ""{5}"",
    ""attr"" : {{ ""id"" : ""{3}"" }}  {4}
    }} ";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string childString=@", ""children"" :{0}";
                    string childJason="";
                    string stateString = "closed";
                    string classString = "";
                    if (paths != null && level < paths.Count  && paths[level].ID == list[i].ID)
                    {
                        childJason = string.Format(childString, GetArticleChildrenJasonString(list[i].ID, paths, level + 1));
                        stateString = "open";
                        classString = "active";
                    }
                    string oneLine = string.Format(jasonString, list[i].Title, list[i].LinkUrl, classString, list[i].ID, childJason, stateString);
                    sb.AppendLine(oneLine.Replace("{{", "{").Replace("}}", "}"));
                    if (i < list.Count - 1) sb.AppendLine(",");
                }
            }
            sb.AppendLine("]");
            return sb.ToString();
        }

        protected List<Article> FormatArticlesData(List<Article> list)
        {
            DateTime now = DateTime.Now;
            foreach (Article a in list)
            {
                if (a.ContentType == (int)TypeOfArticle.LinkArticle)
                {
                    a.LinkUrl = a.ContentUrl;
                }
                else
                {
                    a.LinkUrl = String.Format("{0}{1}", a.ChannelFullUrl, a.FullUrl);
                }
                a.FullTitle = a.Title;
            }
            return list;
        }
        /// <summary>
        /// 取得数据列表
        /// </summary>
        /// <returns>文章列表</returns>
        List<Article> GetArticleListFromDB(string parentID)
        {
            ArticleQuery query = new ArticleQuery();

            query.State = ArticleStates.Started;
            query.OrderKeys = "Index|Asc";

            if (!string.IsNullOrEmpty(parentID))
                query.ArticleParentID = parentID;
            else
            {
                query.ArticleParentID = We7Helper.EmptyGUID;
                if (!string.IsNullOrEmpty(OwnerID))
                    query.ChannelID = OwnerID;
            }

            List<Article> list = ArticleHelper.QueryArtilcesByAll(query, 0, 100, null);
            return list;
        }

        /// <summary>
        /// 获取父文章路径
        /// </summary>
        /// <returns></returns>
        List<Article> GetParentArticlePath()
        {
            List<Article> list = new List<Article>();
            if (!string.IsNullOrEmpty(ArticleID))
            {
                Article a = ArticleHelper.GetArticle(ArticleID, null);
                while (a != null)
                {
                    list.Insert(0, a);
                    if (!We7Helper.IsEmptyID(a.ParentID))
                        a = ArticleHelper.GetArticle(a.ParentID, null);
                    else
                        break;
                }
            }
            return list;
        }
    }
}
