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
using System.Text;
using We7.Model.Core;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.DataControlUI
{
    public partial class DataControlModelEditor : BasePage
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
        private DataControlInfo _DataControlInfo;
        protected DataControlInfo DataControlInfo
        {
            get 
            {
                if (_DataControlInfo == null)
                {
                    _DataControlInfo = DataControlHelper.GetDataControlInfo(ControlFile);
                }
                return _DataControlInfo;
            }
        }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName
        {
            get
            {
                return DataControlInfo.Model;
            }
        }


        public string Data
        {
            get
            {
                StringBuilder data = new StringBuilder();
                data.Append("[");
                if (!String.IsNullOrEmpty(ModelName))
                {
                    BuildModelField(data);
                }
                data.Append("]");
                return data.ToString();
            }
        }

        void BuildModelField(StringBuilder sb)
        {
            ModelInfo model = ModelHelper.GetModelInfo(ModelName);

            foreach (We7DataColumn dc in model.DataSet.Tables[0].Columns)
            {
                if(dc.Direction==ParameterDirection.Input||dc.Direction==ParameterDirection.InputOutput)
                    AppendData(sb, dc.Label, dc.Name);
            }
            Utils.TrimEndStringBuilder(sb, ",");
        }

        void AppendData(StringBuilder sb,string key, string value)
        {
            sb.Append("{");
            if (String.IsNullOrEmpty(DataControlInfo.CtrType) || String.Compare(DataControlInfo.CtrType, "text", true) == 0)
            {
                sb.AppendFormat("label:'{0}',exp:'<%=Item[\"{1}\"] %>'", key, value);
            }
            else if(String.Compare(DataControlInfo.CtrType, "placeholder", true)==0)
            {
                sb.AppendFormat("label:'{0}',exp:'<asp:PlaceHolder ID=\"_{1}\" runat=\"server\"></asp:PlaceHolder>'", key, value);
            }
            sb.Append("},");
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
                    tmpfolder = Request["gp"];
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
            DataControlHelper.EditControl(GroupCopy, ControlFile, CtrCodeTextBox.Text.Trim());
            string script = "window.close()";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "returnValue", script, true);
        }

        protected void StoreButton_Click(object sender, EventArgs e)
        {
            if (hdKey.Value == "1")
            {
                string[] vs = hdValue.Value.Split('|');
                string name = vs[0];
                string key = vs[1];
                string desc = vs[2];
                string ctr = DataControlHelper.SaveControl(ControlFile, CtrCodeTextBox.Text.Trim(), name, key, desc);
                DataControl dc = DataControlHelper.GetDataControl(ctr);
                string ctrName = dc != null ? (string.IsNullOrEmpty(dc.Name) ? "(空)" : dc.Name) : "(空)";
                string script = "window.returnValue={key:'" + ctrName + "',value:'" + ctr + "'};window.close()";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "returnValue", script, true);
            }
            else
            {
                try
                {
                    DataControlHelper.EditControl(ControlFile, CtrCodeTextBox.Text.Trim());
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
