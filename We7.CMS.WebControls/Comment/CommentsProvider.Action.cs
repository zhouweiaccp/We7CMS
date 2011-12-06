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
using We7.CMS.Accounts;
using System.Web;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 评论数据处理对象
    /// </summary>
    public class AddCommentAction : BaseAction
    {
        /// <summary>
        /// 登陆名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 是否需要登陆
        /// </summary>
        public bool IsLogin { get; set; }
        /// <summary>
        /// 是否允许匿名
        /// </summary>
        public bool IsAnony { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public new string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidateCode { get; set; }
        /// <summary>
        /// 评论的文章ID
        /// </summary>
        public string ArticleID { get; set; }
        /// <summary>
        /// 评论的栏目ID
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// 转向的ArticleID
        /// </summary>
        public string ArticleIDByRedirect { get; set; }
        /// <summary>
        /// 当前用户
        /// </summary>
        public string CurrentAccount { get; set; }
        /// <summary>
        /// 是否需要登陆
        /// </summary>
        public bool IsSignin { get; set; }
        /// <summary>
        /// 是否需要验证码
        /// </summary>
        public bool ISValidate { get; set; }
        /// <summary>
        /// 是否登陆
        /// </summary>
        public bool ISSiginSelect { get; set; }


        /// <summary>
        /// 执行当前操作
        /// </summary>
        public override void Execute()
        {
            if (IsLogin)
            {
                Authenticate();
            }
            else
            {
                if (CheckLogin() && CheckValidateCode())
                    SubmitComment();
            }
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        void Authenticate()
        {
            IAccountHelper AccountHelper = AccountFactory.CreateInstance();
            Account act = AccountHelper.GetAccountByLoginName(Name);
            if (act == null)
            {
                Message = "该用户不存在!";
                return;
            }
            if (!AccountHelper.IsValidPassword(act, Password))
            {
                Message = "密码不正确!";
                return;
            }
            Security.SetAccountID(act.ID);
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        protected void SubmitComment()
        {
            try
            {
                SiteSettingHelper CDHelper = HelperFactory.GetHelper<SiteSettingHelper>();
                IAccountHelper AccountHelper = AccountFactory.CreateInstance();
                CommentsHelper CommentsHelper = HelperFactory.GetHelper<CommentsHelper>();

                Comments cm = new Comments();
                DateTime Createdtime = DateTime.Now;
                if (ArticleIDByRedirect != "")
                { cm.ArticleID = ArticleIDByRedirect; }
                else
                {
                    cm.ArticleID = ArticleID;
                }

                if (CDHelper.Config.IsAuditComment)
                {
                    cm.State = 0;
                }
                else
                {
                    cm.State = 1;
                }
                if (IsSignin)
                {
                    string actID = CurrentAccount;
                    if (We7Helper.IsEmptyID(actID))
                    {
                        cm.Author = "系统管理员";
                    }
                    else
                    {
                        Account act = AccountHelper.GetAccount(CurrentAccount, new string[] { "FirstName", "LastName", "LoginName" });
                        cm.Author = String.Format("{0} {1}({2})",
                            act.LastName, act.FirstName, act.LoginName);
                    }
                    cm.AccountID = actID;
                }
                else
                {
                    cm.Author = Author;
                    cm.AccountID = "";
                }
                cm.Content = Content;
                cm.Created = Createdtime;
                cm.ID = We7Helper.CreateNewID();
                cm.ArticleName = Title;
                CommentsHelper.AddComments(cm);
                Message = CDHelper.Config.IsAuditComment ? "评论发表成功,等待系统审核！" : "发表成功！";

                Content = "";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        /// <summary>
        /// 查看是否有非法评论
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        bool HasIllegalContent(string s)
        {
            bool flag = false;

            KeywordHelper keywordHelper = new KeywordHelper();
            KeyWordGroup keyWordGroup = keywordHelper.GetKeyWordGroup();
            int i = 0;
            string keys = "";
            foreach (KeyWordGroup.Item it in keyWordGroup.Items)
            {
                if (s.IndexOf(it.Words) >= 0)
                {
                    i++;
                    keys = it.Words;
                }
            }
            if (i == 0)
            {
                flag = false;
            }
            else
            {
                Message = "不能包括" + keys + "关键字!";
                flag = true;
            }
            if (s == null || s.Trim().Length == 0)
            {
                flag = false;
            }
            if (s.IndexOf("<script") > -1 || s.IndexOf("&#60script") > -1 || s.IndexOf("&60script") > -1 || s.IndexOf("%60script") > -1)
            {
                flag = true;
            }
            if (flag)
                Message = "评论内容非法。";
            return flag;
        }

        /// <summary>
        /// 检测验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CheckValidateCode()
        {
            if (ISValidate && ValidateCode != Request.Cookies["AreYouHuman"].Value)
            {
                Message = "验证码出错";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检测是否登陆
        /// </summary>
        /// <returns></returns>
        bool CheckLogin()
        {
            if (ISSiginSelect && !IsSignin)
            {
                Message = "请您先登录！";
                return false;
            }
            return true;
        }
    }


    /// <summary>
    /// 取得数据列表
    /// </summary>
    public class ListCommentAction : ListAction<Comments>, IHttpHandler
    {
        private int recordCount;
        /// <summary>
        /// ArticleID
        /// </summary>
        public string ArticleIDByRedirect { get; set; }
        /// <summary>
        /// 栏目ID
        /// </summary>
        public string ArticleID { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        public override void Execute()
        {
            CommentsHelper CommentsHelper = HelperFactory.GetHelper<CommentsHelper>();

            if (!string.IsNullOrEmpty(ArticleIDByRedirect))
            {
                Records = CommentsHelper.ArticleIDQueryComments(ArticleIDByRedirect, this.StartIndexs, PageSize, null, true);
            }
            else
            {
                Records = CommentsHelper.ArticleIDQueryComments(ArticleID, this.StartIndexs, PageSize, null, true);
            }
        }

        /// <summary>
        /// 记录总条数
        /// </summary>
        public override int RecordCount
        {
            get
            {
                if (recordCount == 0)
                {
                    CommentsHelper CommentsHelper = HelperFactory.GetHelper<CommentsHelper>();
                    if (ArticleIDByRedirect != "")
                    {
                        recordCount = CommentsHelper.ArticleIDQueryCommentsCount(ArticleIDByRedirect);
                    }
                    else
                    {
                        recordCount = CommentsHelper.ArticleIDQueryCommentsCount(ArticleID);
                    }
                }
                return recordCount;
            }
        }

        bool IHttpHandler.IsReusable
        {
            get
            {
                return false;
            }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ArticleID = context.Request.Params["ArticleID"];
            PageSize = int.Parse(context.Request.Params["PageSize"]);
            this.StartIndexs = int.Parse(context.Request.Params["PageIndex"]);
            Execute();
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (We7.CMS.Common.Comments c in Records)
            {
                sb.Append("{Author:\"" + c.Author + "\",Updated:\"" + c.Updated.ToString("yyyy-dd-MM HH:mm:ss") + "\",Content:\"" + c.Content + "\"},");
            }
            string value = sb.ToString().TrimEnd(',').ToString() + "]";
            context.Response.Write(value);
            context.Response.Flush();
        }
        private int startIndex;
        /// <summary>
        /// 开始页
        /// </summary>
        public int StartIndexs
        {
            get
            {
                startIndex = startIndex < 1 ? 1 : startIndex;
                return (startIndex * PageSize) - (PageSize - 1);
            }
            set { startIndex = value; }
        }
        private int pageSize;

        public new int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

    }
}
