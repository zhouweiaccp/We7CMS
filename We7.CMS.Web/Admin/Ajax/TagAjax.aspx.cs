using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework;
using We7.Framework.Util;
using Newtonsoft.Json;
using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin.Ajax
{
    public partial class TagAjax : BasePage
    {
        string ObjectID
        {
            get
            {
                return RequestHelper.Get<string>("id");
            }
        }

        string TagType
        {
            get
            {
                return RequestHelper.Get<string>("type");
            }
        }
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        string TagName
        {

            get
            {
                return Server.UrlDecode(RequestHelper.Get<string>("name"));
            }
        }
        /// <summary>
        /// result(json)
        /// </summary>
        protected string result;
        /// <summary>
        /// page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = RequestHelper.Get<string>("op");
            string result = "{\"success\":false,\"msg\":\"错误的访问路径!\"}";
            switch (action)
            {
                case "add": result = AddTags(); break;
                case "del": result = DelTags(); break;
                case "list": result = GetTags(); break;
            }
            Response.Write(result);

        }

        #region Add

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        protected string AddTags()
        {
            switch (TagType)
            {
                case "article":
                    return AddArticleTag();
                case "channel":
                    return AddChannelTag();
            }
            return "{\"success\":false,\"msg\":\"错误的访问路径!\"}";
        }
        string AddArticleTag()
        {

            //检查是否已经存在改标签
            List<string> tagsList = ArticleHelper.GetTags(ObjectID);

            if (tagsList != null && tagsList.Contains(TagName))
            {
                return "{\"success\":false,\"msg\":\"该标签已经存在!\"}";
            }
            else
            {
                Article a = ArticleHelper.GetArticle(ObjectID);
                if (a != null)
                {
                    //更新文章标签
                    a.Tags = a.Tags + "'" + TagName + "'";
                    ArticleHelper.UpdateArticle(a, new string[] { "Tags" });

                    //更新标签使用频率
                    TagsHelper.Add(TagName);

                    //记录日志
                    AddLog("编辑文章", string.Format("为文章【{0}】增加标签【{1}】", a.Title, TagName));


                    return "{\"success\":true,\"msg\":\"添加成功!\"}";
                }
                return "{\"success\":false,\"msg\":\"文章已被删除!\"}";
            }
            //string tagLi = "<LI><IMG class=Icon height=16 src=\"/admin/images/icon_globe.gif\" width=16>{0}<A class=Del title=\"删除标签 {0}?\"   href=\"javascript:removeTag('{0}');\"  >[x]</A> </LI>";

            //return string.Format(tagLi, TagName);
        }

        string AddChannelTag()
        {


            //检查是否已经存在改标签
            List<string> tagsList = ChannelHelper.GetTags(ObjectID);

            if (tagsList != null && tagsList.Contains(TagName))
            {
                return "{\"success\":false,\"msg\":\"该标签已经存在!\"}";
            }
            else
            {
                Channel a = ChannelHelper.GetChannelById(ObjectID);
                if (a != null)
                {
                    //更新文章标签
                    a.Tags = a.Tags + "'" + TagName + "'";

                    try
                    {
                        ChannelHelper.UpdateChannel(a, new string[] { "Tags" });

                        //更新标签使用频率

                        TagsHelper.Add(TagName);

                    }
                    catch (Exception ex)
                    {
                        return "{\"success\":false,\"msg\":\""+ex.Message+"\"}";
                    }

                    //记录日志
                    AddLog("编辑文章", string.Format("为栏目【{0}】增加标签【{1}】", a.Title, TagName));


                    return "{\"success\":true,\"msg\":\"添加成功!\"}";
                }
                return "{\"success\":false,\"msg\":\"文章已被删除!\"}";
            }

            return "";
        }

        #endregion

        #region Del

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <returns></returns>
        protected string DelTags()
        {
            switch (TagType)
            {
                case "article":
                    return DeleteArticleTag();
                case "channel":
                    return DeleteChannelTag();

            }
            return "{\"success\":false,\"msg\":\"错误的访问路径!\"}";
        }

        /// <summary>
        /// 删除栏目标签
        /// </summary>
        /// <returns></returns>
        string DeleteChannelTag()
        {
            Channel c = ChannelHelper.GetChannel(ObjectID, new string[] { "Tags", "ID" });
            if (c != null && c.Tags != null)
            {
                c.Tags = c.Tags.Replace("'" + TagName + "'", "");
                ChannelHelper.UpdateChannel(c, new string[] { "Tags" });

                //记录日志
                AddLog("编辑栏目", string.Format("删除了栏目【{0}】的标签【{1}】", c.Name, TagName));

                return "{\"success\":true,\"msg\":\"操作成功!\"}";
            }

            return "{\"success\":false,\"msg\":\"操作失败，请刷新后再试!\"}";
        }

        /// <summary>
        /// 删除文章标签
        /// </summary>
        /// <returns></returns>
        string DeleteArticleTag()
        {
            Article a = ArticleHelper.GetArticle(ObjectID);
            if (a != null && a.Tags != null)
            {
                a.Tags = a.Tags.Replace("'" + TagName + "'", "");
                ArticleHelper.UpdateArticle(a, new string[] { "Tags" });

                //记录日志
                AddLog("编辑文章", string.Format("删除了文章【{0}】的标签【{1}】", a.Title, TagName));

                return "{\"success\":true,\"msg\":\"操作成功!\"}";

            }
            return "{\"success\":false,\"msg\":\"操作失败，请刷新后再试!\"}";

        }
        #endregion

        #region GetTags

        string GetTags()
        {

            int pageIndex = We7Request.GetInt("pi", 1);
            int pageSize = We7Request.GetInt("ps", 10);
            List<Tags> list = TagsHelper.GetTags(pageIndex, pageSize);
            if (list != null && list.Count > 0)
            {
                string json = JavaScriptConvert.SerializeObject(list).Replace("null", "\"\"");

                return "{\"success\":true,\"msg\":\"操作成功!\",\"Data\":" + json + "}";
            }
            else
            {
                return "{\"success\":true,\"msg\":\"无数据!\"}";
            }

        }
        /// <summary>
        /// 构建分页数据
        /// </summary>
        /// <param name="recordcount">总记录数</param>
        /// <param name="pagesize">页记录数</param>
        /// <param name="pageindex">当前页记录</param>
        /// <param name="startindex">开始记录行号</param>
        /// <param name="itemscount">当前分页记录数</param>
        static void BuidlPagerParam(int recordcount, int pagesize, ref int pageindex, out int startindex, out int itemscount)
        {
            if (pagesize <= 0)
                pagesize = 1;

            int totalpagecount = recordcount / pagesize;
            if (recordcount % pagesize != 0)
                totalpagecount++;
            if (pageindex > totalpagecount)
                pageindex = totalpagecount;
            if (pageindex <= 0)
                pageindex = 1;
            if (pageindex < totalpagecount)
            {
                itemscount = pagesize;
            }
            else if (pageindex == totalpagecount)
            {
                itemscount = recordcount - (pageindex - 1) * pagesize;
            }
            else
            {
                itemscount = 0;
            }
            startindex = (pageindex - 1) * pagesize;
        }

        #endregion
    }
}