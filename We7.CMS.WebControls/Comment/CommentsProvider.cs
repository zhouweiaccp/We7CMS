using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS;
using System.Web.UI.WebControls;
using We7;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;
using System.IO;

namespace We7.CMS.WebControls
{

    /// <summary>
    /// 评论属性
    /// </summary>
    public class CommentsProvider:ListWebControl<Comments>
    {
        /// <summary>
        /// 评论业务助手
        /// </summary>
        protected CommentsHelper CommentsHelper
        {
            get { return HelperFactory.GetHelper<CommentsHelper>(); }
        }
        /// <summary>
        /// 文章类业务助手
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        /// <summary>
        /// 文章ID
        /// </summary>
        protected string ArticleID
        {
            get
            {
                return ArticleHelper.GetArticleIDFromURL();
            }
        }
        /// <summary>
        /// 通过导航取得文章ID
        /// </summary>
        protected string ArticleIDByRedirect
        {
            get
            {
                if (Session["ArticleComment"] != null)
                {
                    string[] listString = Session["ArticleComment"] as string[];
                    if (listString[0] == "ArticleCommentID")
                    {
                        return listString[1].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                { return ""; }
            }
        }


        /// <summary>
        /// 栏目ID
        /// </summary>
        protected string ChannelID
        {
            get { return ChannelHelper.GetChannelIDFromURL(); }
        }

        string cssClass;
        /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }
        string bindColumnID;
        /// <summary>
        ///　绑定栏目
        /// </summary>
        public string BindColumnID
        {
            get { return bindColumnID; }
            set { bindColumnID = value; }
        }

        bool iSValidate;
        /// <summary>
        /// 是否验证
        /// </summary>
        public bool ISValidate
        {
            get { return iSValidate; }
            set { iSValidate = value; }
        }
        bool iSSiginSelect;
        /// <summary>
        /// 是否需要登陆
        /// </summary>
        public bool ISSiginSelect
        {
            get { return iSSiginSelect; }
            set { iSSiginSelect = value; }
        }
        /// <summary>
        /// 选择验证
        /// </summary>
        public string SelectValidate
        {
            get
            {
                return ISValidate ? "" : "none";
            }
        }
        /// <summary>
        /// 是否显示名称
        /// </summary>
        public string NameSelect
        {
            get
            {
                if (ArticleID != null && BindColumnID != null && ArticleID != "" && BindColumnID != "")
                {
                    return "";//显示
                }
                else
                {
                    return "none";//不显示
                }
            }
        }
        /// <summary>
        /// 登陆名
        /// </summary>
        public string LoginName
        {
            get
            {
                return ISSiginSelect ? "none" : "";
            }
        }

        /// <summary>
        /// 栏目名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 登陆选择
        /// </summary>
        public string SiginSelect
        {
            get
            {
                if (ISSiginSelect)
                {
                    if (IsSignin)
                    {
                        return "none";//不显示
                    }
                    else
                    {
                        return "";//显示
                    }
                }
                else
                {
                    return "none";//不显示
                }
            }
        }

        private bool isShowCount = true;
        /// <summary>
        /// 是否显示记录条数
        /// </summary>
        public bool IsShowCount
        {
            get { return isShowCount; }
            set { isShowCount = value; }
        }

        /// <summary>
        /// 是否显示记录条数
        /// </summary>
        protected string ShowCount
        {
            get
            {
                return IsShowCount ? "" : "none";
            }
        }
        /// <summary>
        /// 栏目Url
        /// </summary>
        public string ChannelUrl
        {
            get
            {
                string channelUrl = "";
                if (BindColumnID != null && ArticleID != null && BindColumnID != "" && ArticleID != "")
                {
                    channelUrl = String.Format("{0}{1}", ChannelHelper.GetFullUrl(BindColumnID), We7Helper.GUIDToFormatString(ArticleID) + ".html");
                }
                return channelUrl;
            }
        }

        /// <summary>
        /// 评论的条数
        /// </summary>
        public int CommentsCount
        {
            get
            {
                return CommentsHelper.ArticleIDQueryCommentsCount(ArticleID);
            }
        }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title
        {
            get
            {
                if (ArticleIDByRedirect != null && ArticleIDByRedirect != "")
                {
                    return ArticleHelper.GetArticleName(ArticleIDByRedirect);
                }
                return String.Empty;
            }
        }

        ListCommentAction action;
        /// <summary>
        /// 取得评论Action
        /// </summary>
        /// <returns>评论Action</returns>
        public override IListAction GetListAction()
        {
            if (action == null)
            {
                action = GetAction<ListCommentAction>();
                if (action == null)
                {
                    action = new ListCommentAction();
                    action.PageSize = PageSize;
                    action.PageIndex = 1;
                    action.ArticleIDByRedirect = ArticleIDByRedirect;
                    action.ArticleID = ArticleID;
                    action.Execute();
                }
            }
            return action;
        }

        /// <summary>
        /// 验证当前控件
        /// </summary>
        protected virtual void ValidateControl()
        {
            if (ArticleID != null && ArticleID != "")
            {
                Visible = ArticleHelper.GetArticle(ArticleID).AllowComments == 1;
            }
            else
            {
                if (ArticleIDByRedirect != null && ArticleIDByRedirect != "")
                {
                    Visible = true;
                }
                else
                {
                    Visible = false;
                }
            }
        }
        /// <summary>
        /// 评论声明
        /// </summary>
        public string Rule
        {
            get
            {
                return File.ReadAllText(Server.MapPath("~/Widgets/CommentsList/Page/Resource/rule.txt"));
            }
        }

        public string AccountID
        {
            get
            {
                return We7Helper.EmptyGUID;
            }
        }

        public string CurrentAccount
        {
            get
            {
                return We7Helper.EmptyGUID;
            }
        }
    }
}
