using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using System.Collections.Generic;
using Wuqi.Webdiyer;
using System.Text;
using We7.CMS.WebControls.Core;
using System.IO;

namespace We7.CMS.Web.Admin.DataControlUI
{
    public partial class DataControlListEx : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        protected override void Initialize()
        {
            if (!IsPostBack)
            {
                //if (Directory.Exists(Constants.We7ControlPhysicalPath))
                //{
                    BindCategory();
                    BindData();
                //}
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnCreateIndex_Click(object sender, EventArgs e)
        {

            try
            {
                BaseControlHelper Helper = new BaseControlHelper();
                Helper.CreateWidegetsIndex();   //重建部件索引
                Helper.CreateIntegrationIndexConfig();   //创建控件、模型、插件的索引 
                ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('重建索引成功!')", true);
                BindCategory();
                BindData();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('重建索引失败:" + ex.Message + "')", true);
            }
        }

        void BindData()
        {
            BaseControlHelper Helper = new BaseControlHelper();
            List<DataControlInfo> infoList = Helper.GetDataControlsInfos();
            List<DataControlInfo> result = new List<DataControlInfo>();
            if (result != null)
            {
                if (ddlType.SelectedValue != "ALL")
                {

                    foreach (DataControlInfo dci in infoList)
                    {
                        if (!result.Contains(dci) && dci.Group == ddlType.SelectedValue && (dci.Name.Contains(txtKeyWord.Text.Trim()) || dci.Desc.Contains(txtKeyWord.Text.Trim()) || dci.DefaultControl.Description.Contains(txtKeyWord.Text.Trim())))
                            result.Add(dci);
                    }
                }
                else
                {
                    foreach (DataControlInfo dci in infoList)
                    {
                        if (!result.Contains(dci) && (dci.Name.Contains(txtKeyWord.Text.Trim()) || dci.Desc.Contains(txtKeyWord.Text.Trim())))
                            result.Add(dci);
                    }
                }
                PagedDataSource pds = new PagedDataSource();
                pds.DataSource = result;
                pds.PageSize = Pager.PageSize;
                pds.AllowPaging = true;
                Pager.RecordCount = result.Count;
                pds.CurrentPageIndex = Pager.CurrentPageIndex - 1;

                if (pds.CurrentPageIndex < 0)
                    pds.CurrentPageIndex = 0;
                ModeDataList.DataSource = pds;
                ModeDataList.DataBind();
            }
        }

        void BindCategory()
        {
            List<DataControlGroup> list = DataControlHelper.GetDataControlGroups();
            ddlType.DataSource = list;
            ddlType.DataTextField = "Label";
            ddlType.DataValueField = "Name";
            ddlType.DataBind();
            ddlType.Items.Add(new ListItem("系统部件", "系统部件"));
            ddlType.Items.Insert(0, new ListItem("所有", "ALL"));
        }

        protected void Pager_PageChanging(object src, PageChangingEventArgs e)
        {
            Pager.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        protected string GetControlFileName(Object o)
        {
            DataControlInfo info = o as DataControlInfo;
            if (info != null)
            {
                 DataControl dc= info.DefaultControl;
                 if (dc == null)
                     return "";
                 else
                 {
                     if (info.Group == "系统部件")
                         return dc.Description;
                     else
                         return dc.FileName;
                 }
            }
            return "";
          
        }

        protected string GetTitle(object o)
        {
            DataControlInfo info = o as DataControlInfo;
            if (info.Group == "系统部件")
            {
                return info.GroupDesc;
            }
            else
                return string.Format(@"<a href='javascript:onSelectThis(""{0}"");'>{1}</a>", info.DefaultControl.FileName,info.Desc);

        }

        protected DataControl GetDefaultControl(Object o)
        {
            DataControlInfo info = o as DataControlInfo;
            if (info != null)
            {
                return info.DefaultControl;
            }
            return null;
        }

        protected string GetDemoUrl(Object o)
        {
            DataControl dc = GetDefaultControl(o);
            if (dc == null)
                return "We7Controls/Resources/s.jpg";
            return dc.DemoUrl;
        }

        protected string GetGroup(object o)
        {
            DataControlInfo info = o as DataControlInfo;
            return info != null ? (String.IsNullOrEmpty(info.Group) ? "System" : info.Group) : String.Empty;
        }

        protected string GetControls(Object o)
        {
            StringBuilder sb = new StringBuilder();
            
            DataControlInfo info = o as DataControlInfo;
            if (info != null)
            {
                sb.Append("部件：<br/>");
                string pattern = @"【<a href='javascript:onSelectThis(""{0}"");'>{1}</a>】<br/>";
                if (info.Group == "系统部件")
                {
                    foreach (DataControl dc in info.Controls)
                    {
                        sb.AppendFormat(pattern, dc.FileName, dc.Description);
                    }
                }
                else
                {
                    sb.Append("外观：<br/>");
                    foreach (DataControl dc in info.Controls)
                    {
                        //sb.AppendFormat(pattern, dc.Control, dc.Name);
                        sb.AppendFormat(pattern, dc.FileName, dc.Name);
                    }
                }
            }
            return sb.ToString();
        }
    }
}
