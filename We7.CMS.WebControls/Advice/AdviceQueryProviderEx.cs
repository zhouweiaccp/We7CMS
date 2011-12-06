using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using Thinkment.Data;
using We7.Framework.Util;
using System.IO;
using System.Xml;

namespace We7.CMS.WebControls
{
    public class AdviceQueryProviderEx : BaseWebControl
    {
        private List<AdviceInfo> items;
        private string adviceTypeID;
        private string queryKey = "advicetype";

        /// <summary>
        /// Css样式
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryKey
        {
            get { return queryKey; }
            set { queryKey = value; }
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
                    string query = Request[QueryKey];
                    if (!String.IsNullOrEmpty(query))
                    {
                        query = query.Trim().ToLower();
                        string path = We7Utils.GetMapPath("/Config/advicemapping.xml");
                        if (File.Exists(path))
                        {
                            XmlNode node = XmlHelper.GetXmlNode(path, "//item[@key='" + query + "']");
                            if (node != null)
                            {
                                XmlElement xe = node as XmlElement;
                                adviceTypeID = (xe.GetAttribute("value") ?? "").Trim();
                            }
                        }
                    }
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
        /// 导航栏目
        /// </summary>
        public string OwnerID { get; set; }

        /// <summary>
        /// 安全查询
        /// </summary>
        public bool SecurityQuery { get; set; }

        public string ErrorMessage { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<AdviceInfo> Items
        {
            get
            {
                if (items == null)
                {
                    items = GetItems();
                }
                return items;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        private List<AdviceInfo> GetItems()
        {
            List<AdviceInfo> items = null;
            RecordCount = HelperFactory.Assistant.Count<AdviceInfo>(CreateListCriteria());
            items = HelperFactory.Assistant.List<AdviceInfo>(CreateListCriteria(), CreateOrders(), StartItem, PageItemsCount);
            return items != null ? items : new List<AdviceInfo>();
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <returns></returns>
        protected virtual Criteria CreateListCriteria()
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", AdviceTypeID);
            if (!Html.IsPostBack) //如果是安全查询，则默认不显示信息
            {
                c.Add(CriteriaType.NotEquals, "TypeID", AdviceTypeID);
            }

            if (Html.IsPostBack)
            {
                string sn = Html.Request<string>("SN");
                string keyword = Html.Request<string>("KeyWord");
                string password = Html.Request<string>("Password");

                if (SecurityQuery)
                {
                    c.Add(CriteriaType.Equals, "SN", sn);
                    c.Add(CriteriaType.Equals, "MyQueryPwd", (password??"").Trim());
                }
                else
                {
                    Criteria subC = new Criteria(CriteriaType.None);
                    subC.Mode = CriteriaMode.Or;
                    subC.AddOr(CriteriaType.Equals, "SN", sn);
                    subC.AddOr(CriteriaType.Like, "Title", "%"+keyword+"%");
                    c.Criterias.Add(subC);
                }
            }

            return c;
        }

        /// <summary>
        /// 创建Order集合
        /// </summary>
        /// <returns></returns>
        protected virtual Order[] CreateOrders()
        {
            return new Order[] { new Order("Created", OrderMode.Desc) };
        }

        /// <summary>
        /// 根据ID取得Url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string GetUrl(string id)
        {
            return UrlHelper.GetUrl(OwnerID, id);
        }
    }
}
