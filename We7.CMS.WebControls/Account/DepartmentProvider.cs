using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.PF;
using We7.Framework.Util;
using System.Web;
using System.Xml;

namespace We7.CMS.WebControls
{
    public class DepartmentProvider : BaseWebControl
    {
        private string departCode = "DepartID";
        private Department item;
        private string departID;

        public string CssClass { get; set; }

        public string DepartCode
        {
            get { return departCode; }
            set { departCode = value; }
        }

        /// <summary>
        /// 关联的控件ID
        /// </summary>
        public string RelationCtrID { get; set; }

        public string DepartID
        {
            get
            {
                if (String.IsNullOrEmpty(departID))
                {
                    if (!String.IsNullOrEmpty(RelationCtrID))
                    {
                        DbModelProvider provider = We7Utils.FindControl(RelationCtrID, Page) as DbModelProvider;
                        if (provider != null && provider.Item != null && provider.Item.Table.Columns.Contains(DepartCode))
                        {
                            departID = provider.Item[DepartCode] as string;
                        }
                    }
                    else
                    {
                        departID = Request[DepartCode];
                    }
                }
                return departID;
            }
        }



        public Department Item
        {
            get
            {
                if (item == null)
                {
                    item = AccountFactory.CreateInstance().GetDepartment(DepartID, new string[] { "FromSiteID", "ParentID", "ID", "Name", "Index", "Created", "FullName" });
                    if (item == null)
                    {
                        item = new Department();
                    }
                }
                return item;
            }
        }
    }

    public class DepartUtil
    {
        /// <summary>
        /// 从Config/Departs.xml取得部门ID值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDepartIds(string key)
        {
            XmlNode node = XmlHelper.GetXmlNode(We7Utils.GetMapPath("/Config/Departs.xml"), "//item[@key='" + key + "']");
            return node != null ? node.InnerText : null;
        }
    }
}
