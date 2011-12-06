using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Thinkment.Data;
using We7.CMS;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using System.Data;
using We7.Model.Core;
using We7.CMS.Common;
using System.IO;
using We7.Model.UI.Data;
using System.Text;
using System.Xml;
using System.Reflection;
using We7.Framework.Util;
using We7.Framework.Zip;
using We7.CMS.Common.Enum;

namespace We7.Model.UI
{
    public partial class Export : BasePage
    {
        protected global::System.Web.UI.WebControls.TextBox txtTable;
        protected void bttnExport_Click(object sender, EventArgs args)
        {
            string table=txtTable.Text.Trim();
            DataBaseHelper helper = new DataBaseHelper();
            DataTable dt=helper.Query("SELECT * FROM [" + table + "]");

            DataSet ds = new DataSet();
            ds.DataSetName = "We7DataSet";
            dt.TableName = "Design";
            ds.Tables.Add(dt);


            string tempDir = Server.MapPath("~/_temp/Data");
            string tempXml = Server.MapPath("~/_temp/Data/Data.xml");
            string tempSchema = Server.MapPath("~/_temp/Data/Schema.xsd");
            DirectoryInfo di = new DirectoryInfo(tempDir);

            if (!di.Exists)
                di.Create();

            ds.WriteXml(tempXml);
            ds.WriteXmlSchema(tempSchema);


            Response.Clear();
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", "attachment;filename=Data.zip");
            ZipUtils.CreateZip(tempDir, Response.OutputStream);
            Response.End();
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }
    }
}
