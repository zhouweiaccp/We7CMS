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
using We7.CMS.Config;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin.DataControlUI
{
    public partial class DataControlEditor : BasePage
    {
        string controlfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        string ControlFile
        {
            get
            {
                if (String.IsNullOrEmpty(controlfile))
                {
                    controlfile = Request["ctr"];
                    if (String.IsNullOrEmpty(controlfile))
                        throw new Exception("控件参数不能为空");
                }
                return controlfile;
            }
        }


        bool IsEdit
        {
            get
            {
                if (ViewState["WE$IsEditor"] == null)
                {
                    string cmd = Request["cmd"];
                    return String.IsNullOrEmpty(cmd) ? false : cmd.ToLower() == "edit";
                }
                return (bool)ViewState["WE$IsEditor"];
            }
            set
            {
                ViewState["WE$IsEditor"] = value;
            }
        }
        string Group
        {
            get
            {
                string tmpfolder = String.Empty;
                if (!String.IsNullOrEmpty(Request["gp"]))
                    tmpfolder= Request["gp"];
                else
                {
                    tmpfolder = CDHelper.Config.DefaultTemplateGroupFileName;
                    tmpfolder = tmpfolder.Remove(tmpfolder.IndexOf("."));
                }
                return tmpfolder.TrimStart('~');
            }
        }

        string GroupCopy
        {
            get
            {
                GeneralConfigInfo config = GeneralConfigs.GetConfig();
                if (config.SiteBuildState == "run")
                {
                    return String.Format("~{0}", Group);
                }
                else
                {
                    return Group;
                }
            }
        }

        void BindData()
        {
            BindContent();
        }

        void BindContent()
        {
            CtrCodeTextBox.Text = DataControlHelper.GetControlCode(GroupCopy, ControlFile);
        }

        protected void OriControlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindContent();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            DataControlHelper.EditControl(GroupCopy,ControlFile, CtrCodeTextBox.Text.Trim());
            string script = "window.close()";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "returnValue", script, true);
        }

        protected void StoreButton_Click(object sender, EventArgs e)
        {
            if (hdKey.Value == "1")
            {
                string[] vs=hdValue.Value.Split('|');
                string name = vs[0];
                string key = vs[1];
                string desc = vs[2];
                string path = DataControlHelper.SaveControl(ControlFile, CtrCodeTextBox.Text.Trim(),name,key,desc);
                DataControl dc = DataControlHelper.GetDataControlByPath(path);
                string ctrName = dc != null ? (string.IsNullOrEmpty(dc.Name) ? "(空)" : dc.Name) : "(空)";
                string script = "window.returnValue={key:'" + ctrName + "',value:'" + path + "'};window.close()";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "returnValue", script, true);
            }
            else
            {
                try
                {
                    DataControlHelper.EditControl2(ControlFile, CtrCodeTextBox.Text.Trim());
                    string script = "window.close()";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "returnValue", script, true);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    Response.End();
                }
            }
            DataControlHelper.EditControl(GroupCopy, ControlFile, CtrCodeTextBox.Text.Trim());
        }
    }
}
