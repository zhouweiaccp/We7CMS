using System;
using System.IO;
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

using Thinkment.Data;
using System.Text;
using We7.CMS.Common.Enum;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.DataControlUI
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
            string selectQuery = this.FieldDropDownList.SelectedItem.ToString();
            string query = SearchTextBox.Text.Trim();
            List<DataControlInfo> ds = DataControlHelper.GetControls(query, selectQuery);

            ModeDataList.DataSource = ds;
            ModeDataList.DataBind();

            if (ds.Count == 0)
            {
                ShowMessage("没有符合条件的控件。");
            }
            else
            {
                ShowMessage(String.Format("总共 {0} 个控件。", ds.Count));
            }
        }

        protected void QueryButton_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        protected void Sort(string sortName)
        {
            string sortText = sortName;
            List<DataControlInfo> ds = DataControlHelper.GetControls(sortName);

            ModeDataList.DataSource = ds;
            ModeDataList.DataBind();

            if (ds.Count == 0)
            {
                ShowMessage("没有符合条件的控件。");
            }
            else
            {
                ShowMessage(String.Format("总共 {0} 个控件。", ds.Count));
            }
        }

        void ShowMessage(string s)
        {
            MessageLabel.Text = s;
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

        protected string GetControlFileName(Object o)
        {
            DataControl dc=GetDefaultControl(o);
            if (dc == null)
                return "";
            else
                return dc.Control;
        }

        protected string GetDemoUrl(Object o)
        {
            DataControl dc = GetDefaultControl(o);
            if (dc == null)
                return "We7Controls/Resources/s.jpg";
            return dc.DemoUrl;
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