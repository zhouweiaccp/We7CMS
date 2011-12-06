using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using We7.Framework;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.Data;
using Thinkment.Data;
using We7.CMS.Accounts;

namespace We7.CMS.WebControls.AccountEx
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SearchCategory : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Cache.SetNoStore();
            context.Response.Clear();
            string action=context.Request["action"];
            if (!String.IsNullOrEmpty(action))
            {
                CategoryHelper helper = HelperFactory.Instance.GetHelper<CategoryHelper>();
                string value = context.Request["value"];
                action = action.Trim().ToLower();
                List<Category> lsTemp = null;
                if (action == "seachcategory")
                {
                    lsTemp = helper.GetChildrenListByKeyword(value);
                    if (lsTemp != null && lsTemp.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder("");
                        foreach(Category model in lsTemp)
                        {
                            sb.Append(model.KeyWord + "|" + model.Name + ",");
                        }
                        string result = "";
                        if(sb.Length > 1)
                        {
                            result =sb.ToString().TrimEnd(new char[] { ',' });
                        }
                        context.Response.Write(result);
                        return;
                    }
                }
                else if (action == "seachbsfw")
                {
                    DataTable dt = ModelDBHelper.Create("gov.bsfw").Query(new Criteria(CriteriaType.Equals, "yycj", value), new List<Order>() { new Order("ID") }, 0, 0);
                    if(dt != null && dt.Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder("");
                        foreach (DataRow row in dt.Rows)
                        {
                            sb.Append(UrlHelper.GetModelUrl("gov.bsfw", row["ID"].ToString()) + "|" + row["ywmc"] + ",");
                        }
                        string result = "";
                        if (sb.Length > 1)
                        {
                            result = sb.ToString().TrimEnd(new char[] { ',' });
                        }
                        context.Response.Write(result);
                        return; 
                    }
                }

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
