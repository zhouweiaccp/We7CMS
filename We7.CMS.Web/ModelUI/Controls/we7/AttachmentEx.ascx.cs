using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework;
using System.Xml;
using System.Web.UI.HtmlControls;

namespace We7.Model.UI.Controls.we7
{
    public partial class AttachmentEx : We7FieldControl
    {
        public override void InitControl()
        {
            RegisterResource();
            InitAttachmentBuilder();
            hfValue.Value = Value != null ? Value.ToString() : "[]";
        }

        public override object GetValue()
        {
            return !String.IsNullOrEmpty(hfValue.Value) ? hfValue.Value : "[]";
        }

        public void RegisterResource()
        {
            if (HttpContext.Current.Items["jquery-ui-1.8.1.custom.css"] == null)
            {
                HtmlLink link = new HtmlLink();
                link.Attributes["type"] = "text/css";
                link.Attributes["rel"] = "Stylesheet";
                link.Attributes["media"] = "screen";
                link.Href = "/Admin/Ajax/jquery/ui1.8.1/themes/ui-lightness/jquery-ui-1.8.1.custom.css";
                Page.Header.Controls.Add(link);
                HttpContext.Current.Items["jquery-ui-1.8.1.custom.css"] = new object();
            }
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("JqueryUI"))
                Page.ClientScript.RegisterClientScriptInclude("JqueryUI", "/Admin/Ajax/jquery/ui1.8.1/jquery-ui-1.8.1.custom.min.js");
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("ModelCtrAttachment"))
                Page.ClientScript.RegisterClientScriptInclude("ModelCtrAttachment", "/ModelUI/Controls/Js/Attachment.js");
        }

        protected int FrameWidth
        {
            get
            {
                if (ViewState["$Width"] == null)
                {
                    ViewState["$Width"] = 500;
                }
                return (int)ViewState["$Width"];
            }
            set { ViewState["$Width"] = value; }
        }

        protected int FrameHeight
        {
            get
            {
                if (ViewState["$Height"] == null)
                {
                    ViewState["$Height"] = 410;
                }
                return (int)ViewState["$Height"];
            }
            set { ViewState["$Height"] = value; }
        }

        protected string Src
        {
            get
            {
                if (ViewState["$Src"] == null)
                {
                    ViewState["$Src"] = "/ModelUI/Controls/page/SimpleAttachment.aspx";
                }
                return ViewState["$Src"] as string;
            }
            set { ViewState["$Src"] = value; }
        }

        public void InitAttachmentBuilder()
        {
            if (!IsPostBack)
            {
                string uploader = Control.Params["uploader"];
                if (!String.IsNullOrEmpty(uploader))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(Server.MapPath("~/ModelUI/Controls/Js/AttachmentUploader.xml"));
                    XmlElement xe = doc.DocumentElement.SelectSingleNode("Item[@name='" + uploader + "']") as XmlElement;
                    if (xe != null)
                    {
                        int width, height;
                        FrameWidth = Int32.TryParse(xe.GetAttribute("width"), out width) ? width : 400;
                        FrameHeight = Int32.TryParse(xe.GetAttribute("height"), out height) ? height : 360;
                        Src = xe.GetAttribute("src");
                    }
                }
            }
        }
    }
}