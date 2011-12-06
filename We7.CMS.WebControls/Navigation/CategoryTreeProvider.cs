using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.Framework.Util;
using System.Web.UI.HtmlControls;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;

namespace We7.CMS.WebControls
{
    public class CategoryTreeProvider : BaseWebControl
    {
        private string queryKey = "cat";

        #region 属性面板参数
        public string CssClass { get; set; }

        public string QueryKey
        {
            get { return queryKey; }
            set { queryKey = value; }
        }

        public string Url { get; set; }


        protected bool EnableFolder { get; set; }
        /// <summary>
        /// 部门类业务助手
        /// </summary>
        protected CategoryHelper CategoryHelper
        {
            get { return HelperFactory.GetHelper<CategoryHelper>(); }
        }

        /// <summary>
        /// 获取当前部门对象
        /// </summary>
        public Category CurrentCategory
        {
            get
            {
                string id = Request["cat"];
                if (string.IsNullOrEmpty(id))
                {
                    id = We7Helper.EmptyGUID;
                }
                Category ch = CategoryHelper.GetCategory(id);
                return ch;
            }
        }

        private string keyword;
        /// <summary>
        /// 类别关键字
        /// </summary>
        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }


        List<Category> lsCategorys = new List<Category>();
        /// <summary>
        /// 当前栏目层级关系
        /// </summary>
        /// <param name="currentCategory"></param>
        public void InitCategorys(Category currentCategory)
        {
            if (currentCategory != null)
            {
                lsCategorys.Add(currentCategory);
                Category tempChanel = CategoryHelper.GetCategory(currentCategory.ParentID);
                InitCategorys(tempChanel);
            }
        }

        string parentID;
        /// <summary>
        /// 上级部门ID
        /// </summary>
        public string ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        bool showParentName;
        /// <summary>
        /// 显示父部门
        /// </summary>
        public bool ShowParentName
        {
            get { return showParentName; }
            set { showParentName = value; }
        }


        int titleMaxLength = 0;
        /// <summary>
        /// 标题最大字数
        /// </summary>
        public int TitleMaxLength
        {
            get { return titleMaxLength; }
            set { titleMaxLength = value; }
        }
        string tag;
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        string showToolTip;
        /// <summary>
        /// 鼠标提示
        /// </summary>
        public string ShowToolTip
        {
            get { return showToolTip; }
            set { showToolTip = value; }
        }

        public string HtmlData;
        #endregion
        protected void GetAllChildCategory(StringBuilder sb, string parentID)
        {
            ParentID = parentID;
            List<Category> childCategorys = CategoryHelper.GetCategoryByParentID(parentID);
            //CategoryHelper.GetCategorys(parentID,true);
            if (childCategorys != null)
            {
                sb.Append("<ul>");
                for (int j = 0; j < childCategorys.Count; j++)
                {
                    List<Category> CategoryList = CategoryHelper.GetCategoryByParentID(childCategorys[j].ID);
                    if (CategoryList != null && CategoryList.Count > 0)
                    {
                        bool isFind = false;
                        for (int k = 0; k < lsCategorys.Count; k++)
                        {
                            if (lsCategorys[k].ID == childCategorys[j].ID)
                            {
                                isFind = true;
                                break;
                            }
                        }
                        if (isFind)
                        {
                            if (CurrentCategory != null && childCategorys[j].ID == CurrentCategory.ID)
                            {
                                sb.Append("<li><span class=\"folder TreeMenuTwoProviderCurrent\">" + GetHref(childCategorys[j].KeyWord, childCategorys[j].Name,true) + "</span>");
                            }
                            else
                            {
                                sb.Append("<li><span class=\"folder\">" + GetHref(childCategorys[j].KeyWord, childCategorys[j].Name,true) + "</span>");
                            }
                        }
                        else
                        {
                            sb.Append("<li class=\"closed\" ><span class=\"folder\">" + GetHref(childCategorys[j].KeyWord, childCategorys[j].Name,true) + "</span>");
                        }
                        GetAllChildCategory(sb, childCategorys[j].ID);
                    }
                    else
                    {
                        if (CurrentCategory != null && childCategorys[j].ID == CurrentCategory.ID)
                        {
                            sb.Append("<li><span class=\"file TreeMenuTwoProviderCurrent\">" + GetHref(childCategorys[j].KeyWord, childCategorys[j].Name,false) + "</span>");
                        }
                        else
                        {
                            sb.Append("<li><span class=\"file\">" + GetHref(childCategorys[j].KeyWord, childCategorys[j].Name,false) + "</span>");
                        }
                    }
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }
        }

        /// <summary>
        /// 绑定树
        /// </summary>
        private void BindTree()
        {
            string tempKeyword = Keyword;
            if (string.IsNullOrEmpty(Keyword))
            {
                tempKeyword = Request["cat"];
            }
            HtmlData = CategoryHelper.CacheRecord.GetInstance<string>("We7.CMS.WebControls.CategoryTreeProvider_" + tempKeyword, () =>
            {
                StringBuilder sb = new StringBuilder("");
                CategoryCollection Categorys = null;
                if (String.IsNullOrEmpty(Keyword))
                {
                    Category cat = CategoryHelper.GetTopAndSiblingByKeyword(Request["cat"]);
                    Categorys = new CategoryCollection();
                    Categorys.Add(cat);
                }
                else
                {
                    Categorys = new CategoryCollection();
                    Categorys.Add(CategoryHelper.GetCategoryByKeyword(Keyword));
                }
                bool displayParent = false;
                for (int i = 0; i < Categorys.Count; i++)
                {
                    Category ch = Categorys[i];
                    List<Category> CategoryList = CategoryHelper.GetCategoryByParentID(ch.ID);

                    if (CategoryList != null && CategoryList.Count > 0)
                    {
                        bool isFind = false;
                        for (int j = 0; j < lsCategorys.Count; j++)
                        {
                            if (lsCategorys[j].ID == ch.ID)
                            {
                                isFind = true;
                                break;
                            }
                        }
                        if (isFind)
                        {
                            if (CurrentCategory != null && ch.ID == CurrentCategory.ID)
                            {
                                sb.Append("<li><span class=\"folder TreeMenuTwoProviderCurrent\">" + GetHref(ch.KeyWord, ch.Name, true) + "</span>");
                            }
                            else
                            {

                                sb.Append("<li><span class=\"folder\">" + GetHref(ch.KeyWord, ch.Name, true) + "</span>");
                            }
                        }
                        else
                        {
                            sb.Append("<li><span class=\"folder\">" + GetHref(ch.KeyWord, ch.Name, true) + "</span>");
                        }
                        GetAllChildCategory(sb, ch.ID);
                    }
                    else
                    {
                        //if (CurrentCategory != null && ch.ID == CurrentCategory.ID)
                        //{
                        //    sb.Append("<li><span class=\"file TreeMenuTwoProviderCurrent\"><a href='" + ch.FullUrl + "'>" + ch.Name + "</a></span>");
                        //}
                        //else
                        //{
                        sb.Append("<li><span class=\"file\">" + GetHref(ch.KeyWord, ch.Name, false) + "</span>");
                        //}
                    }
                    sb.Append("</li>");
                }
                if (displayParent)
                {
                    sb.Append("</ul></li>");
                }
                sb.Append("<script>$(function(){$('#" + this.ClientID + "').treeview({unique:true});});</script>").ToString();
                return sb.ToString();

        });
            
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitCategorys(CurrentCategory);
            IncludeJavaScript();
            JavaScriptManager.Include("/Admin/Ajax/jquery/jquery.cookie.js",
                                      "/Admin/Ajax/jquery/jquery.treeview.min.js");
            //HtmlGenericControl ctr = new HtmlGenericControl("link");
            //ctr.Attributes["href"] = "/Admin/Ajax/jquery/css/jquery.treeview.css";
            //ctr.Attributes["type"] = "text/css";
            //ctr.Attributes["rel"] = "Stylesheet";
            //Page.Header.Controls.Add(ctr);
            BindTree();
        }

        /// <summary>
        /// 得到转向url
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        private string GetHref(string keyword, string CategoryName,bool isFolder)
        {
            if (!isFolder || EnableFolder)
            {
                if (!String.IsNullOrEmpty(Url))
                {
                    string u=Url+(Url.Contains("?") ? "&" : "?") + String.Format("{0}={1}", QueryKey, keyword);
                    return "<a href='"+u+"'>" + CategoryName + "</a>";
                }
                else
                {
                    return "<a href='#' onclick=\"return CatDirect('" + keyword + "');\">" + CategoryName + "</a>";
                }
            }
            else
                return CategoryName;
        }
    }
}
