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
using System.Text;

namespace We7.CMS.Web.Admin.DataControlUI.FieldControl
{
    public partial class TagsSelector : System.Web.UI.Page
    {
        public string Tags="[]";
        const string Config = "Config/Dictionary/Tags.xml";
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadTags();
        }

        void LoadTags()
        {
            string path = Server.MapPath("~/" + Config);
            if (!File.Exists(path))
                return;

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            XmlNodeList nl=doc.SelectNodes("//Item");
            foreach (XmlElement xe in nl)
            {
                sb.AppendFormat("'{0}',", xe.GetAttribute("words"));
            }
            if (sb.Length > 1)
                sb.Remove(sb.Length - 1,1);
            sb.Append("]");
            Tags = sb.ToString();
        }
        
    }
}
