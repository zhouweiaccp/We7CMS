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

namespace We7.CMS.Web.Admin.VisualTemplate
{
    public partial class DataControlList : BasePage
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
                BindCategory();
                BindData();
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
                DataControlHelper dchelper = HelperFactory.GetHelper<DataControlHelper>();
                dchelper.CreateDataControlIndex();
                ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('重建索引成功!')", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('重建索引失败:" + ex.Message + "')", true);
            }
        }

        void BindData()
        {
            List<DataControlInfo> result = DataControlHelper.GetDataControlsInfos();
            if (ddlType.SelectedValue != "ALL")
            {
                result = result.FindAll(delegate(DataControlInfo dci)
                {
                    return dci.Group == ddlType.SelectedValue && (dci.Name.Contains(txtKeyWord.Text.Trim()) || dci.Desc.Contains(txtKeyWord.Text.Trim()));
                });
            }
            else
            {
                result = result.FindAll(delegate(DataControlInfo dci)
                {
                    return dci.Name.Contains(txtKeyWord.Text.Trim()) || dci.Desc.Contains(txtKeyWord.Text.Trim());
                });
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

        void BindCategory()
        {
            List<DataControlGroup> list = DataControlHelper.GetDataControlGroups();
            ddlType.DataSource = list;
            ddlType.DataTextField = "Label";
            ddlType.DataValueField = "Name";
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("所有", "ALL"));
        }

        protected void Pager_PageChanging(object src, PageChangingEventArgs e)
        {
            Pager.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        protected string GetControlFileName(Object o)
        {
            DataControl dc = GetDefaultControl(o);
            if (dc == null)
                return "";
            else
                return dc.Control;
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
                string pattern = @"【<a href='javascript:onSelectThis(""{0}"");'>{1}</a>】";
                foreach (DataControl dc in info.Controls)
                {
                    sb.AppendFormat(pattern, dc.Control, dc.Name);
                }
            }
            return sb.ToString();
        }
    }
}
