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
using System.Collections.Generic;
using We7.CMS;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.User
{
    public partial class ArticleProcess : UserBasePage
    {
        //private MessagePanel Messages=null;

        protected override bool NeedAnAccount
        {
            get
            {
                return false;
            }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Messages = (MessagePanel)Master.FindControl("Messages");
            if (!IsPostBack)
            {
                LoadArticles();
            }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            ////this.Master.SiteHeadTitle = SiteHeadTitle;
            //this.Master.TitleName = "文章审核管理";
        }
        #region 获取数据

        void LoadArticles()
        {
            //取出所有待审批文章，逐一判断是否具有权限
            ArticleQuery query = new ArticleQuery();
            query.State = ArticleStates.Checking;
            List<Article> GetAllArticles = ArticleHelper.QueryArtilcesByAll(query);
            List<Article> articles = new List<Article>();

            foreach (Article article in GetAllArticles)
            {
                try
                {
                    string curLayerNOText = ProcessHelper.GetCurLayerNOText(article.ID);
                    if (curLayerNOText != "") //文章当前审批进程：类似 Channel.FirstAudit
                    {
                        string channelID = ArticleHelper.GetArticle(article.ID).OwnerID;
                        List<string> contents = AccountHelper.GetPermissionContents(AccountID, channelID);
                        if (contents.Contains(curLayerNOText))
                        {
                            articles.Add(article);
                        }
                    }
                }
                catch
                { }
            }

            if (articles != null)
            { Pager.RecorderCount = articles.Count; }
            else
            {
                Pager.RecorderCount = 0;
            }
            if (Pager.Count < 0)
                Pager.PageIndex = 0;
            Pager.FreshMyself();
            if (Pager.Count <= 0)
            {
                DataGridView.DataSource = null;
                DataGridView.DataBind();
                return;
            }

            DataGridView.DataSource = articles.GetRange(Pager.Begin, Pager.Count);
            DataGridView.DataBind();

        }

        #endregion

        public string GetProcessState(string id)
        {
            Article a = ArticleHelper.GetArticle(id);
            Processing ap = ProcessHelper.GetArticleProcess(a);
            string processText = "草稿";
            if (ap != null)
                processText = ap.ProcessDirectionText + ap.ProcessText;
            return processText;
        }
        public string GetChannelText(string id)
        {
            return ChannelHelper.GetChannelName(ArticleHelper.GetArticle(id).OwnerID);
        }

        List<string> GetIDs()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < DataGridView.Rows.Count; i++)
            {
                if (((CheckBox)DataGridView.Rows[i].FindControl("chkItem")).Checked)
                {
                    list.Add(((Label)(DataGridView.Rows[i].FindControl("lblID"))).Text);
                }
            }
            return list;
        }
        protected void Pager_Fired(object sender, EventArgs e)
        {
            LoadArticles();
        }

        string GetChannelProcessLayerNO(string id)
        {
            string channelID = ArticleHelper.GetArticle(id).OwnerID;
            Channel ch = ChannelHelper.GetChannel(channelID, null);
            if (ch.ProcessLayerNO != null)
                return ch.ProcessLayerNO;
            else
                return "";
        }


    }
}
