using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.Framework.Util;
using System.Web.UI.HtmlControls;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using We7.Framework.Cache;
using We7.CMS.Accounts;
using We7.Framework.Config;

namespace We7.CMS.WebControls
{
    public class DepartmentTreeProvider : BaseWebControl
    {
        private string queryKey = "DepartID";

        #region 属性面板参数
        public string CssClass { get; set; }

        public string QueryKey
        {
            get { return queryKey; }
            set { queryKey = value; }
        }

        /// <summary>
        /// 导航Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 权限业务对象
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// 获取当前部门对象
        /// </summary>
        public Department CurrentDepartment
        {
            get
            {
                string id = Request["DepartID"];
                if (string.IsNullOrEmpty(id))
                {
                    id = We7Helper.EmptyGUID;
                }
                Department ch = AccountHelper.GetDepartment(id, new string[] { "FromSiteID", "ParentID", "ID", "Name", "Index", "Created", "FullName" });
                return ch;
            }
        }

        List<Department> lsDepartments = new List<Department>();
        /// <summary>
        /// 当前栏目层级关系
        /// </summary>
        /// <param name="currentDepartment"></param>
        public void InitDepartments(Department currentDepartment)
        {
            if (currentDepartment != null)
            {
                lsDepartments.Add(currentDepartment);
                Department tempChanel = AccountHelper.GetDepartment(currentDepartment.ParentID, new string[] { "FromSiteID", "ParentID", "ID", "Name", "Index", "Created", "FullName" });
                InitDepartments(tempChanel);
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

        string SiteID
        {
            get
            {
                return SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
            }
        }

        protected void GetAllChildDepartment(StringBuilder sb, string parentID)
        {
            ParentID = parentID;
            List<Department> childDepartments = AccountHelper.GetOrderDepartments(SiteID,parentID);
            //DepartmentHelper.GetDepartments(parentID,true);
            if (childDepartments != null)
            {
                sb.Append("<ul>");
                for (int j = 0; j < childDepartments.Count; j++)
                {
                    List<Department> DepartmentList = AccountHelper.GetOrderDepartments(SiteID,childDepartments[j].ID);
                    if (DepartmentList != null && DepartmentList.Count > 0)
                    {
                        bool isFind = false;
                        for (int k = 0; k < lsDepartments.Count; k++)
                        {
                            if (lsDepartments[k].ID == childDepartments[j].ID)
                            {
                                isFind = true;
                                break;
                            }
                        }
                        if (isFind)
                        {
                            if (CurrentDepartment != null && childDepartments[j].ID == CurrentDepartment.ID)
                            {
                                sb.Append("<li><span class=\"folder TreeMenuTwoProviderCurrent\">" + GetHref(childDepartments[j].ID, childDepartments[j].Name) + "</span>");
                            }
                            else
                            {
                                sb.Append("<li><span class=\"folder\">" + GetHref(childDepartments[j].ID, childDepartments[j].Name) + "</span>");
                            }
                        }
                        else
                        {
                            sb.Append("<li class=\"closed\" ><span class=\"folder\">" + GetHref(childDepartments[j].ID, childDepartments[j].Name) + "</span>");
                        }
                        GetAllChildDepartment(sb, childDepartments[j].ID);
                    }
                    else
                    {
                        if (CurrentDepartment != null && childDepartments[j].ID == CurrentDepartment.ID)
                        {
                            sb.Append("<li><span class=\"file TreeMenuTwoProviderCurrent\">" + GetHref(childDepartments[j].ID, childDepartments[j].Name) + "</span>");
                        }
                        else
                        {
                            sb.Append("<li><span class=\"file\">" + GetHref(childDepartments[j].ID, childDepartments[j].Name) + "</span>");
                        }
                    }
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }
        }

        #region
        ///// <summary>
        ///// 格式化部门数据
        ///// </summary>
        ///// <param name="list">部门列表</param>
        ///// <returns>部门列表</returns>
        //protected virtual List<Department> FormatDepartmentsData(List<Department> list)
        //{
        //    DateTime now = DateTime.Now;
        //    foreach (Department ch in list)
        //    {
        //        if (ShowToolTip != null && ShowToolTip.Length > 0)
        //            ch.Title = ch.Name;
        //        if (TitleMaxLength > 0 && ch.Name.Length > TitleMaxLength)
        //        {
        //            ch.Name = ch.Name.Substring(0, TitleMaxLength) + "...";
        //        }
        //        if (We7Helper.GetDepartmentUrlFromUrl(Request.RawUrl).ToLower().StartsWith(ch.FullUrl.ToLower()))
        //            ch.MenuIsSelected = true;
        //    }
        //    return list;
        //}
        #endregion

        /// <summary>
        /// 绑定树
        /// </summary>
        private void BindTree()
        {
            HtmlData = CacheRecord.Create(typeof(AccountLocalHelper)).GetInstance<string>("We7.CMS.WebControls.DepartmentTreeProvider_" + ParentID, () =>
            {
                StringBuilder sb = new StringBuilder("");
                List<Department> Departments = AccountHelper.GetOrderDepartments(SiteID,ParentID);
                bool displayParent = false;
                if (ShowParentName)
                {
                    string parentID = "";
                    if (ParentID != null && ParentID.Length > 0)
                    {
                        parentID = ParentID;
                    }
                    else
                    {
                        parentID = We7.We7Helper.EmptyGUID;
                    }
                    Department parentDepartment = AccountHelper.GetDepartment(parentID, new string[] { "FromSiteID", "ParentID", "ID", "Name", "Index", "Created", "FullName" });
                    if (parentDepartment != null)
                    {
                        displayParent = true;
                        sb.Append("<li><span class=\"folder\">" + GetHref(parentDepartment.ID, parentDepartment.Name) + "</span><ul>");
                    }
                }
                for (int i = 0; i < Departments.Count; i++)
                {
                    Department ch = Departments[i];
                    List<Department> DepartmentList = AccountHelper.GetOrderDepartments(null,ch.ID);

                    if (DepartmentList != null && DepartmentList.Count > 0)
                    {
                        bool isFind = false;
                        for (int j = 0; j < lsDepartments.Count; j++)
                        {
                            if (lsDepartments[j].ID == ch.ID)
                            {
                                isFind = true;
                                break;
                            }
                        }
                        if (isFind)
                        {
                            if (CurrentDepartment != null && ch.ID == CurrentDepartment.ID)
                            {
                                sb.Append("<li><span class=\"folder TreeMenuTwoProviderCurrent\">" + GetHref(ch.ID, ch.Name) + "</span>");
                            }
                            else
                            {

                                sb.Append("<li><span class=\"folder\">" + GetHref(ch.ID, ch.Name) + "</span>");
                            }
                        }
                        else
                        {
                            sb.Append("<li class=\"closed\" ><span class=\"folder\">" + GetHref(ch.ID, ch.Name) + "</span>");
                        }
                        GetAllChildDepartment(sb, ch.ID);
                    }
                    else
                    {
                        //if (CurrentDepartment != null && ch.ID == CurrentDepartment.ID)
                        //{
                        //    sb.Append("<li><span class=\"file TreeMenuTwoProviderCurrent\"><a href='" + ch.FullUrl + "'>" + ch.Name + "</a></span>");
                        //}
                        //else
                        //{
                        sb.Append("<li><span class=\"file\">" + GetHref(ch.ID, ch.Name) + "</span>");
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
            InitDepartments(CurrentDepartment);
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
        /// <param name="departmentId"></param>
        /// <returns></returns>
        private string GetHref(string departmentId, string departmentName)
        {
            if (!String.IsNullOrEmpty(Url))
            {
                string url=Url+(Url.Contains("?")?"&":"?")+String.Format("{0}={1}",QueryKey,departmentId);
                return "<a href='" + url + "'>" + departmentName + "</a>";
            }
            else
            {
                return "<a href='#' onclick=\"return DepartDirect('" + departmentId + "');\">" + departmentName + "</a>";
            }
        }


    }
}
