using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using We7.Model.Core;
using We7.Model.Core.UI;
using We7.Framework.Util;
using System.Web;
using Thinkment.Data;
using We7.Framework;
using We7.CMS.Helpers;

namespace We7.CMS.WebControls
{
    public class SearchBar : BaseWebControl
    {
        UIHelper UIHelper;
        PanelContext ctx = new PanelContext();
        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// 面板名称
        /// </summary>
        public string PanelName { get; set; }
        /// <summary>
        /// 控件集合
        /// </summary>
        public We7ControlCollection Items { get; set; }





        private string searchPageUrl = "/gsearch.aspx";
        public string SearchPageUrl
        {
            get { return searchPageUrl; }
            set { searchPageUrl = value; }
        }

        /// <summary>
        /// 是否是当前页
        /// </summary>
        public bool CurrentPage { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            InitParams();
            InitControl();
            bttnQuery.Click += new EventHandler(bttnQuery_Click);

            if (CurrentPage && String.IsNullOrEmpty(Request["queryed"]))

            {
                CreateQueryParams();
            }
        }

        protected void bttnQuery_Click(object sender, EventArgs e)
        {
            CreateQueryParams();
        }
        void bttnQuery_Clickimg(object sender, EventArgs e)
        {
            CreateQueryParams();
        }
        void InitParams()
        {
            ctx.Model = ModelHelper.GetModelInfo(ModelName);
            if (ctx.Model == null)
                throw new Exception("当前模型不存在");

            ctx.Panel = ctx.Model.Layout.Panels[PanelName];
            if (ctx.Panel == null)
                throw new Exception("当前模型没有查询项");

            UIHelper = new UIHelper(Page, ctx);
            Items = ctx.Panel.ConditionInfo.Controls;
        }

        void InitControl()
        {
            foreach (We7Control ctr in Items)
            {
                string value = HttpUtility.UrlDecode(Request[GetMapping(ctr.Name)]);
                if (!String.IsNullOrEmpty(value))
                {
                    ctx.Row[ctr.Name] = value;
                }
                FieldControl fieldCtr = UIHelper.GetControl(ctr);
                Control c = Utils.FindControl("_" + ctr.ID, this);
                if (c != null)
                {
                    c.Controls.Clear();
                    c.Controls.Add(fieldCtr);
                }
            }
        }

        void CreateQueryParams()
        {
            QueryEntity query = new QueryEntity();
            foreach (We7Control ctr in Items)
            {
                FieldControl fieldCtr = UIHelper.GetControl(ctr);
                FieldControl c = UIHelper.GetControl(ctr.ID, this) as FieldControl;
                OperationType op = ModelHelper.GetOperation(ctr.Params["operater"]);
                CriteriaType quryType = GetCriteriaType(op);
                if (c != null)
                {
                    object o = c.GetValue();
                    if (o is IDictionary<string, object>)
                    {
                        IDictionary<string, object> dic = o as IDictionary<string, object>;
                        foreach (string key in dic.Keys)
                        {
                            query.QueryParams.Add(new QueryParam() { ColumnKey = GetMapping(key), ColumnValue = dic[key], CriteriaType = quryType });
                        }
                    }
                    else
                    {
                        query.QueryParams.Add(new QueryParam() { ColumnKey = GetMapping(ctr.Name), ColumnValue = o, CriteriaType = quryType });
                    }
                }
            }
            query.ModelName = ctx.ModelName;
            Response.Redirect(CreateUrl(query));
        }

        string CreateUrl(QueryEntity query)
        {

            StringBuilder sb = new StringBuilder();
            string url = CurrentPage ? Request.RawUrl : SearchPageUrl;
            foreach (QueryParam param in query.QueryParams)

            {
                url = We7Helper.AddParamToUrl(url, param.ColumnKey, HttpUtility.UrlEncode(Convert.ToString(param.ColumnValue)));
                sb.Append(param.ColumnKey).Append(",");
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            url = We7Helper.AddParamToUrl(url, "QueryFields", HttpUtility.UrlEncode(sb.ToString()));
            url = We7Helper.AddParamToUrl(url, "queryed", HttpUtility.UrlEncode("1"));
            url = We7Helper.AddParamToUrl(url, "ModelName", HttpUtility.UrlEncode(query.ModelName));
            url = We7Helper.AddParamToUrl(url, "PageIndex", HttpUtility.UrlEncode("1"));
            return url;
        }

        string GetMapping(string key)
        {
            We7DataColumn dc = ctx.Model.DataSet.Tables[0].Columns[key];
            return dc != null && !string.IsNullOrEmpty(dc.Mapping) ? dc.Mapping : key;
        }

        CriteriaType GetCriteriaType(OperationType op)
        {
            switch (op)
            {
                case OperationType.EQUER:
                    return CriteriaType.Equals;
                case OperationType.NOTEQUER:
                    return CriteriaType.NotEquals;
                case OperationType.LIKE:
                    return CriteriaType.Like;
                case OperationType.LESSTHAN:
                    return CriteriaType.LessThan;
                case OperationType.MORETHAN:
                    return CriteriaType.MoreThan;
                case OperationType.LESSTHANEQURE:
                    return CriteriaType.LessThanEquals;
                case OperationType.MORETHANEQURE:
                    return CriteriaType.MoreThanEquals;
                default:
                    return CriteriaType.Equals;
            }
        }

        #region 注册控件
        protected global::System.Web.UI.WebControls.Button bttnQuery;
        protected global::System.Web.UI.WebControls.Button btnSub;

       
        #endregion
}
    }
