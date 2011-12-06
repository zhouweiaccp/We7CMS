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
    public class AdviceQueryProvider : BaseWebControl
    {
        private List<Advice> items;
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
                        string path=We7Utils.GetMapPath("/Config/advicemapping.xml");
                        if(File.Exists(path))
                        {
                            XmlNode node=XmlHelper.GetXmlNode(path, "//item[@key='" + query + "']");
                            if (node != null)
                            {
                                XmlElement xe = node as XmlElement;
                                adviceTypeID = (xe.GetAttribute("value")??"").Trim();
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
        /// 是否公开
        /// </summary>
        public bool IsShow { get; set; }

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
        public List<Advice> Items
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
        private List<Advice> GetItems()
        {
            List<Advice> items = null;
            RecordCount = HelperFactory.Assistant.Count<Advice>(CreateListCriteria());
            items = HelperFactory.Assistant.List<Advice>(CreateListCriteria(), CreateOrders(), StartItem, PageItemsCount);
            return items != null ? items : new List<Advice>();
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <returns></returns>
        protected virtual Criteria CreateListCriteria()
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", AdviceTypeID);
            if (!Html.IsPostBack && SecurityQuery)
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
                    long lngSn;
                    if (!long.TryParse(sn, out lngSn))
                    {
                        ErrorMessage = "查询编号不正确";
                        c.Add(CriteriaType.NotEquals, "TypeID", AdviceTypeID);
                        return c;
                    }
                    if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password.Trim()))
                    {
                        ErrorMessage = "密码不能为空";
                        c.Add(CriteriaType.NotEquals, "TypeID", AdviceTypeID);
                        return c;
                    }
                    c.Add(CriteriaType.Equals, "SN", lngSn);
                    c.Add(CriteriaType.Equals, "MyQueryPwd", password.Trim());

                }
                else
                {
                    if (IsShow)
                    {
                        c.Add(CriteriaType.Equals, "IsShow", 1);
                    }
                    if (!String.IsNullOrEmpty(sn) && !String.IsNullOrEmpty(keyword = sn.Trim()))
                    {
                        long lngSn;
                        if (long.TryParse(sn, out lngSn))
                        {
                            c.Add(CriteriaType.Equals, "SN", lngSn);
                        }
                    }
                    if (!String.IsNullOrEmpty(keyword) && !String.IsNullOrEmpty(keyword = keyword.Trim()))
                    {
                        c.Add(CriteriaType.Like, "Title", "%" + keyword + "%");
                    }
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
            return new Order[] { new Order("Updated", OrderMode.Desc), new Order("CreateDate", OrderMode.Desc) };
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
