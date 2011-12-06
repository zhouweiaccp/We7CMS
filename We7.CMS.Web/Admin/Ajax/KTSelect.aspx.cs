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
using System.Xml;
using System.IO;

namespace We7.CMS.Web.Admin.Ajax
{
    public partial class KTSelect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Write(GetText());
            Response.End();
        }

        public string GetText()
        {
            string filename=Request["f"];
            string key=Request["k"];
            if(!String.IsNullOrEmpty(filename)&&!String.IsNullOrEmpty(key))
            {
                string path = Server.MapPath(String.Format("~/Config/Dictionary/{0}.xml", filename));
                if (File.Exists(path))
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(path);

                        XmlElement node = doc.DocumentElement.SelectSingleNode(String.Format("item[@name='{0}']", key)) as XmlElement;
                        if (node != null)
                        {
                            return node.InnerText.Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
            return " ";
         }
    }
}
