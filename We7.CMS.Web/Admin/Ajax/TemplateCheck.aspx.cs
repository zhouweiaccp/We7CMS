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
using System.Text;
using We7.CMS.Common.Enum;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.Ajax
{
    public partial class TemplateCheck :BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Command)
            {
                case "styleuc":
                    ResponseStyleUsageCounter();
                    break;
                case "delstyle":
                    ResponseDelStyle();
                    break;
                case "delctr":
                    ResponseDelControl();
                    break;
                case "reload":
                    ResponseReload();
                    break;
            }
        }

        void ResponseReload()
        {
            DataControlInfo info = DataControlHelper.GetDataControlInfo(Control);
            ResponseResult(info.ToJson());
        }

        void ResponseStyleUsageCounter()
        {
            StringBuilder sb = new StringBuilder("{");

            TemplateProcessor processor = new TemplateProcessor(TemplateGroup);
            int counter=processor.StyleUsageCounter(Control, Style);
            sb.AppendFormat("used:{0},",counter);

            processor = new TemplateProcessor(TemplateGroupCopy);
            counter = processor.StyleUsageCounter(Control, Style);
            sb.AppendFormat("copy:{0}", counter);
            sb.Append("}");
            ResponseResult(sb.ToString());
        }

        void ResponseDelStyle()
        {
            TemplateProcessor processor = new TemplateProcessor(TemplateGroup);
            processor.DelCss(Control, Style);

            processor = new TemplateProcessor(TemplateGroupCopy);
            processor.DelCss(Control, Style);

            DataControlHelper.DeleteCssFile(Control, Style);
            ResponseResult("success");
        }

        void ResponseDelControl()
        {
            DataControlHelper.DelControl(Control);
            ResponseResult("success");
        }

        void ResponseResult(object result)
        {
            Response.Clear();
            Response.Write(result);
            Response.Flush();
            Response.End();
        }

        string Command
        {
            get
            {
                return Request["cmd"];
            }
        }

        string Control
        {
            get
            {
                return Request["ctr"];
            }
        }

        string TemplateGroup
        {
            get
            {
                if (!String.IsNullOrEmpty(Request["gp"]))
                    return Request["gp"];
                else
                {
                    string tmpfolder = CDHelper.Config.DefaultTemplateGroupFileName;
                    tmpfolder = tmpfolder.Remove(tmpfolder.IndexOf("."));
                    return tmpfolder;
                }
            }
        }
        string TemplateGroupCopy
        {
            get
            {
                return String.Format("~{0}",TemplateGroup.TrimStart('~'));
            }
        }

        string Style
        {
            get
            {
                return Request["style"];
            }
        }
    }
}
