using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common.Enum;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.Ajax
{
    public partial class CategoryRepeatCheck : BasePage
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
            ProcessRequest();
        }

        private void ProcessRequest()
        {
            bool result = false;
            Response.ContentType = "text/html";
            string key = Request.QueryString["key"];
            string type = Request.QueryString["type"];
            string catType = Request.QueryString["cattype"];

            if (!String.IsNullOrEmpty(key))
            {
                CategoryHelper helper=HelperFactory.GetHelper<CategoryHelper>();
                if (type == "1")
                {
                    //result=helper.FindCatetory(key, catType)!=null;
                }
                else
                {
                    //Category cat = helper.GetCategorys(catType);
                    //result=GetCatByName(cat, key) != null;
                    //TODO::检查名称是否重复
                }
            }
            Response.Clear();
            Response.Write(result.ToString().ToLower());
        }

        Category GetCatByName(Category cat, string name)
        {
            if (cat.Name == name)
                return cat;
            foreach (Category c in cat.Children)
            {
                Category cc = GetCatByName(c, name);
                if (cc != null)
                    return cc;
            }
            return null;
        }
    }
}
