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
using We7.Framework.Common;
using We7.Model.Core.Data;

namespace We7.Model.UI
{
    public partial class Test : System.Web.UI.Page
    {
        //测试2
        protected override void OnLoad(EventArgs e)
        {
            ModelDBHelper helper = ModelDBHelper.Create("Subjects.Paper");
            List<Order> os = new List<Order>();
            os.Add(new Order("ID"));
            DataTable dt = helper.Query(new Criteria(CriteriaType.MoreThan, "Created", DateTime.MinValue),os, 0, 0);

            ArticleHelper ah = HelperFactory.Instance.GetHelper<ArticleHelper>();
            foreach (DataRow row in dt.Rows)
            {
                Article a = new Article();
                a.Icon = row["ID"].ToString();
                a.OwnerID = row["OwnerID"].ToString();
                a.State = 1;
                a.Title = row["Title"].ToString();
                ah.AddArticle(a);
            }
        }
    }
}
