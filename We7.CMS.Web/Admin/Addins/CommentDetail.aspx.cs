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
using System.Text;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.Addins
{
    public partial class CommentDetail : BasePage
    {

        string CommentID
        {
            get { return Request["id"]; }
        }

        protected override void Initialize()
        {
            if (!We7Helper.IsEmptyID(CommentID))
            {
                Comments cm = CommentsHelper.GetComment(CommentID, null);

                AuthorLiteral.Text = cm.Author;
                IPLiteral.Text = cm.IP;
                ContentLiteral.Text = cm.Content;
                TimeLiteral.Text = cm.Created.ToString();
                StateLiteral.Text = cm.AuditText;

                try
                {
                    SummaryLabel.Text = String.Format("管理文章：“{0}”的评论", ArticleHelper.GetArticle(cm.ArticleID, new string[] { "Title" }).Title);
                }
                catch
                {
                    string chID = ChannelHelper.GetChannelIDByOnlyName(cm.ArticleID);
                    Channel ch = ChannelHelper.GetChannel(chID, new string[] { "FullPath" });
                    if (ch != null)
                    {
                        SummaryLabel.Text = String.Format("管理栏目：“{0}”的评论", ch.FullPath);
                    }
                }

                RefreshHyperLink.NavigateUrl = String.Format("CommentDetail.aspx?id={0}",CommentID);
            }
            else
            {
                MessageLabel.Text = "没有任何评论";
            }
        }
    }
}
