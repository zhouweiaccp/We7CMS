using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.controls
{
    public partial class FloatMenuControl : BaseUserControl
    {
        string ColumnID
        {
            get
            {
                return ChannelHelper.GetChannelIDFromURL();
            }
        }
        string SeSearchWord
        {
            get { return Request["sekeyword"]; }
        }
        string ColumnMode
        {
            get
            {
                if (Request["mode"] != null)
                    return Request["mode"];
                else
                {
                    if (ArticleID != null && ArticleID != "")
                    {
                        string channelID = ArticleHelper.GetArticle(ArticleID).OwnerID;
                        if (channelID != "")
                        {
                            Channel ch = this.ChannelHelper.GetChannel(channelID, new string[] { "EnumState" });
                            //string type = StateManagement.GetStateName(ch.EnumState, UserEnumLibrary.Business.ArticleType).ToString();
                            EnumLibrary.ArticleType type = (EnumLibrary.ArticleType)StateMgr.GetStateValueEnum(ch.EnumState, EnumLibrary.Business.ArticleType);
                            if (type == EnumLibrary.ArticleType.Product)
                            {
                                return "productDetail";
                            }
                            else
                            {
                                return "detail";
                            }
                        }
                        else
                        { return string.Empty; }
                    }

                    else
                        return string.Empty;
                }
            }
        }
        //string ColumnMode
        //{
        //    get
        //    {
        //        if (Request["mode"] != null)
        //            return Request["mode"];
        //        else
        //        {
        //            if (ArticleID != null && ArticleID != "")
        //                return "detail";
        //            else
        //                return string.Empty;
        //        }
        //    }
        //}

        string ArticleID
        {
            get
            {
                return ArticleHelper.GetArticleIDFromURL();
            }
        }

        string SearchWord
        {
            get { return Request["keyword"]; }
        }

        public string MenuItems
        {
            get
            {
                if (CurrentAccount == We7Helper.EmptyGUID)
                {
                    return BuildFloatMenu();
                }
                return "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        string BuildFloatMenu()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            string TemplatePath = TemplateHelper.GetThisPageTemplate(ColumnMode, ColumnID, SearchWord,SeSearchWord);
            string tmpRedirect = "<p style='margin:3px;'><a href='/Compose.aspx?file={0}&folder={1}' target='_blank'>编辑模板</a></p>";
            string fileName = TemplatePath.Remove(TemplatePath.LastIndexOf("/")).Substring(TemplatePath.Remove(TemplatePath.LastIndexOf("/")).LastIndexOf("/"));
            sb.Append(string.Format(tmpRedirect, TemplatePath.Substring(TemplatePath.LastIndexOf('/') + 1), Path.GetFileNameWithoutExtension(fileName)));

            if (!We7Helper.IsEmptyID(ArticleID))
            {
                string strArticle = "<p style='margin:3px;'><a href='/addins/articleEdit.aspx?id={0}' target='_blank'>编辑本文内容</a></p>";
                sb.Append(string.Format(strArticle, ArticleID));
            }
            else if (!We7Helper.IsEmptyID(ColumnID))
            {
                string strChannnel = "<p style='margin:3px;'><a href='/Channels.aspx?id={0}' target='_blank'>编辑本栏目属性</a></p>";
                sb.Append(string.Format(strChannnel, ColumnID));

                string strArticles = "<p style='margin:3px;'><a href='/addins/Articles.aspx?oid={0}' target='_blank'>编辑本栏目文章</a></p>";
                sb.Append(string.Format(strArticles, ColumnID));
            }


            return sb.ToString();
        }
    }
}