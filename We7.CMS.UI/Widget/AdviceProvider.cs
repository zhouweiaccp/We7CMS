using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.WebControls;
using We7.CMS.WebControls.Core;
using We7.CMS.Common;
using We7.Framework.Util;
using System.IO;
using System.Xml;
using Thinkment.Data;
using We7.CMS.Common.PF;
using We7.CMS.Accounts;

namespace We7.CMS.UI.Widget
{
    public abstract class AdviceProvider : ThinkmentDataControl
    {
        private List<AdviceInfo> items;
        private AdviceInfo item;
        private string adviceTypeID;
        private string queryKey = "SN";

        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        [Parameter(Title = "是否显示", Type = "Boolean", DefaultValue = "1")]
        public bool IsShow = true;

        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryKey
        {
            get { return queryKey; }
            set { queryKey = value; }
        }

        /// <summary>
        /// 当前反馈模型
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 导航栏目
        /// </summary>
        [Parameter(Title = "导航栏目", Type = "Channel", DefaultValue = "", Required = true)]
        public string OwnerID;

        /// <summary>
        /// 记录总条数
        /// </summary>
        public virtual int RecordCount { get; set; }

        private int pageSize = 10;
        /// <summary>
        /// 每页条数
        /// </summary>
        public virtual int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        /// <summary>
        ///　分页开始的条目
        /// </summary>
        public int StartItem
        {
            get
            {
                int start = (PageIndex - 1) * PageSize;
                return start < 0 ? 0 : start;
            }
        }

        /// <summary>
        ///  分页结束的条目
        /// </summary>
        public int EndItem
        {
            get
            {
                int end = PageIndex * PageSize - 1;
                return end >= RecordCount ? (RecordCount - 1) : end;
            }
        }

        /// <summary>
        /// 当前页面的记录数
        /// </summary>
        public int PageItemsCount
        {
            get
            {
                int count = EndItem - StartItem + 1;
                return count > RecordCount ? 0 : count;
            }
        }

        private int pageIndex = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public virtual int PageIndex
        {
            get
            {
                if (!DisablePager)
                {
                    try
                    {
                        string sp = Request != null ? Request["PageIndex"] : "0";
                        pageIndex = String.IsNullOrEmpty(sp) ? 1 : Convert.ToInt32(sp);
                    }
                    catch { pageIndex = 1; }
                }
                else
                {
                    pageIndex = 1;
                }
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        public bool Disable { get; set; }

        /// <summary>
        /// 禁止分页
        /// </summary>
        public virtual bool DisablePager
        {
            get { return Disable; }
            set { Disable = value; }
        }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<AdviceInfo> Items
        {
            get
            {
                if (items == null)
                {
                    try
                    {
                        items = GetItems();
                    }
                    catch
                    {
                        items = null;
                    }
                }
                return items;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AdviceInfo Item
        {
            get
            {
                if (item == null)
                {
                    item = GetItem();
                    //if (item != null)
                    //    item.ModelName = GetModelName();
                }
                return item;
            }
        }

        /// <summary>
        /// 反馈类型
        /// </summary>
        public virtual string AdviceTypeID
        {
            get
            {
                if (String.IsNullOrEmpty(adviceTypeID))
                {
                    adviceTypeID = Request[QueryKey];

                    if (String.IsNullOrEmpty(adviceTypeID))
                    {
                        throw new Exception("advicemapping.xml不存在对应的类型");
                    }
                }
                return adviceTypeID;
            }
            set { adviceTypeID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public string GetAccountName(string accountID)
        {
            Account act = AccountHelper.GetAccount(accountID, new[] { "FirstName", "MiddleName", "LastName" });
            if (act != null)
            {
                return String.Format("{0}{1}{2}", act.FirstName, act.MiddleName, act.LastName);
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        ///  JS文件注册；可以引用多个文件，如：
        ///  IncludeJavaScript("jquery.bgiframe.js", "jquery.dimensions.js",
        ///  "jquery.jdMenu.js", "SlideMenuReady.js");
        /// </summary>
        /// <param name="files"></param>
        public void IncludeJavaScript(params string[] files)
        {
            List<string> paths = new List<string>();
            foreach (string file in files)
            {
                if (file.StartsWith("/"))
                {
                    paths.Add(file);
                }
                else
                {
                    paths.Add(this.TemplateSourceDirectory + "/js/" + file);
                }
            }
            JavaScriptManager.Include(paths.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string GetUrl(string id)
        {
            return UrlHelper.GetUrl(OwnerID, id);
        }
        /// <summary>
        /// 取得当前反馈的模型名称
        /// </summary>
        /// <returns></returns>
        protected virtual string GetModelName()
        {
            if (String.IsNullOrEmpty(ModelName))
            {
                if (String.IsNullOrEmpty(AdviceTypeID))
                    throw new Exception("反馈类型为空");
                AdviceType adviceType = HelperFactory.GetHelper<AdviceTypeHelper>().GetAdviceType(AdviceTypeID);
                ModelName = adviceType != null ? adviceType.ModelName : String.Empty;
            }
            return ModelName;
        }

        /// <summary>
        /// 创建列表查询条件
        /// </summary>
        /// <returns></returns>
        protected virtual Criteria CreateListCriteria()
        {
            Criteria c = new Criteria(CriteriaType.Equals, "SN", AdviceTypeID);
            if (IsShow)
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            return c;
        }

        protected virtual Order[] CreateOrderArray()
        {
            return new Order[] { new Order("Created", OrderMode.Desc) };
        }

        private HtmlHelper2 htmlHelper2;
        /// <summary>
        /// 
        /// </summary>
        protected HtmlHelper2 Html
        {
            get { return htmlHelper2 ?? (htmlHelper2 = new HtmlHelper2(this)); }
        }

     

        /// <summary>
        /// 要据查询条件取得反馈列表
        /// </summary>
        /// <returns></returns>
        public virtual List<AdviceInfo> GetItems()
        {
            List<AdviceInfo> items = null;
            RecordCount = HelperFactory.Assistant.Count<AdviceInfo>(CreateListCriteria());
            items = HelperFactory.Assistant.List<AdviceInfo>(CreateListCriteria(), CreateOrderArray(), StartItem, PageItemsCount);
            return items != null ? items : new List<AdviceInfo>();
        }

        /// <summary>
        /// 取得当前记录
        /// </summary>
        /// <returns></returns>
        private AdviceInfo GetItem()
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", GetAdviceID());
            List<AdviceInfo> list = HelperFactory.Assistant.List<AdviceInfo>(c, null);
            return list != null && list.Count > 0 ? list[0] : new AdviceInfo();
        }

        /// <summary>
        /// 报得反馈ID
        /// </summary>
        /// <returns></returns>
        protected string GetAdviceID()
        {
            return HelperFactory.GetHelper<ArticleHelper>().GetArticleIDFromURL();
        }
    }
}
