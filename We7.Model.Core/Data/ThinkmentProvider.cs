using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using We7.Framework.Util;
using System.Text.RegularExpressions;
using System.Data;
using We7.Framework.Config;
using We7.CMS;
using We7.CMS.Accounts;

namespace We7.Model.Core.Data
{
    class ThinkmentProvider : IDbProvider
    {
        Regex regex = new Regex(@"\bDESC\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        Regex regextrim = new Regex(@"\bDESC\b|\bASC\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #region IDbProvider 成员

        public bool Insert(PanelContext data)
        {
            ModelDBHelper helper = ModelDBHelper.Create(data.ModelName);
            if (data.Row.IndexByMapping("AccountID")==null)
                data.Row["AccountID"] = Security.CurrentAccountID;
            helper.Insert(data.Row);
            return true;
        }

        public bool Update(PanelContext data)
        {
            ModelDBHelper helper = ModelDBHelper.Create(data.ModelName);
            helper.Update(data.Row, CreatePKCriteria(data.DataKey));
            return true;
        }

        public bool Delete(PanelContext data)
        {
            ModelDBHelper helper = ModelDBHelper.Create(data.ModelName);
            helper.Delete(CreatePKCriteria(data.DataKey));
            return true;
        }

        public System.Data.DataTable Query(PanelContext data, out int recordcount, ref int pageindex)
        {
            
            ModelDBHelper helper = ModelDBHelper.Create(data.ModelName);
            Criteria ct = CreateQueryCriteria(data.QueryFields,data);

            recordcount = helper.Count(ct);

            int startindex,itemscount;
            Utils.BuidlPagerParam(recordcount, data.PageSize, ref pageindex, out startindex, out itemscount);
            
            return helper.Query(ct, CreateOrders(data.Orders), startindex, itemscount);
        }

        public System.Data.DataRow Get(PanelContext data)
        {
            ModelDBHelper helper = ModelDBHelper.Create(data.ModelName);
            DataTable dt=helper.Query(CreatePKCriteria(data.DataKey), CreateOrders(data.Orders), 0, 0);
            return dt.Rows.Count > 0 ? dt.Rows[0] : dt.NewRow();
        }

        public int GetCount(PanelContext data)
        {
            ModelDBHelper helper = ModelDBHelper.Create(data.ModelName);
            Criteria ct = CreateQueryCriteria(data.QueryFields, data);
            return helper.Count(ct);
        }

        #endregion

        Criteria CreateQueryCriteria(QueryFieldCollection qfc,PanelContext data)
        {
            Criteria ct = new Criteria(CriteriaType.None);
            //if (data.Model.Type == ModelType.ARTICLE && !data.Model.AuthorityType &&
            //    Security.CurrentAccountID != We7Helper.EmptyGUID)
            ////if (!GeneralConfigs.GetConfig().ShowAllInfo && Security.CurrentAccountID != We7Helper.EmptyGUID && !String.IsNullOrEmpty(Security.CurrentAccountID))
            //{
            //    ct.Add(CriteriaType.Equals, "AccountID", Security.CurrentAccountID);
            //}

            foreach (QueryField qf in qfc)
            {
                if (qf.Operator == OperationType.LIKE)
                {
                    ct.Add(ModelDBHelper.ConvertOperationType(qf.Operator), qf.Column.Name, "%"+qf.Value+"%");
                }
                else
                {
                    ct.Add(ModelDBHelper.ConvertOperationType(qf.Operator), qf.Column.Name, qf.Value);
                }                
            }
            return ct;
        }

        Criteria CreatePKCriteria(DataKey keys)
        {
            Criteria ct = new  Criteria(CriteriaType.None);
            foreach (string s in keys.Values.Keys)
            {
                ct.Add(CriteriaType.Equals, s, keys[s]);
            }
            return ct;
        }

        List<Order> CreateOrders(string orderstr)
        {
            List<Order> orderList = new List<Order>();
            string[] ss=orderstr.Split(',');
            foreach (string s in ss)
            {
                Order order = new Order(regextrim.Replace(s, "").Trim(), regex.IsMatch(s)?OrderMode.Desc:OrderMode.Asc);
                orderList.Add(order);
            }
            return orderList;
        }
    }
}
