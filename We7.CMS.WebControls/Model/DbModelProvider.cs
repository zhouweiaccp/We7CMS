using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Thinkment.Data;
using We7.Model.UI.Data;
using We7.Model.Core.Data;
using We7.Model.Core;
using We7.CMS.Common;
using We7.Framework.Config;
using System.Web;
using We7.Framework.Common;
using We7.Framework.Util;
using We7.Framework;
using System.Xml;
using System.IO;

namespace We7.CMS.WebControls
{
    public class DbModelProvider : BaseWebControl
    {
        #region private members
        //&QueryFields=&queryed=1&ModelName=ShopPlugin.Product&PageIndex=1
        //&QueryFields=ListKeys%2cOwnerID&queryed=1&ModelName=ShopPlugin.Product&PageIndex=1&ListKeys=ewew&OwnerID={db7470a2-0ebd-4499-baeb-fc55e9216f01}
        private string bindColumnID;

        private DataRow item;

        private DataRowCollection items;

        #endregion

        #region 参数
        /// <summary>
        /// 模型参数
        /// </summary>
        public string ModelName { get; set; }
        public string CssClass { get; set; }
        public bool AllowPager { get; set; }
        public bool ShowAtHome { get; set; }
        public int ColumnCount { get; set; }
        public bool UseColumn { get; set; }
        public bool IncludeChildren { get; set; }
        public string QueryParam { get; set; }
        public bool EncodeQueryParam { get; set; }
        public string Tag { get; set; }


        int titleMaxLength = 30;
        public int TitleMaxLength
        {
            get { return titleMaxLength; }
            set { titleMaxLength = value; }
        }

        int summaryMaxLength = 200;
        public int SummaryMaxLength
        {
            get { return summaryMaxLength; }
            set { summaryMaxLength = value; }
        }

        private string dateFormat = "yyyy-MM-dd";
        /// <summary>
        /// 时间格式
        /// </summary>
        public string DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        /// <summary>
        /// 缩略图类型
        /// </summary>
        public string ThumbnailTag { get; set; }
        /// <summary>
        ///最大长度
        /// </summary>
        public int MaxLength { get; set; }
        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleID
        {
            get
            {
                return HelperFactory.GetHelper<ArticleHelper>().GetArticleIDFromURL();
            }
        }
        /// <summary>
        /// 控件绑定ID
        /// </summary>
        public string BindColumnID
        {
            get
            {
                if (String.IsNullOrEmpty(bindColumnID))
                {
                    bindColumnID = ChannelHelper.GetChannelIDFromURL();
                }
                return bindColumnID;
            }
            set { bindColumnID = value; }
        }

        /// <summary>
        /// 栏目映射关系
        /// </summary>
        private IDictionary<string, Channel> ChannelMap
        {
            get
            {
                if (HttpContext.Current.Items["$ChannelMap"] == null)
                {
                    HttpContext.Current.Items["$ChannelMap"] = new Dictionary<string, Channel>();
                }
                return HttpContext.Current.Items["$ChannelMap"] as IDictionary<string, Channel>;
            }
        }

        #endregion
        #region 公共接口
        /// <summary>
        /// 单行数据
        /// </summary>
        public DataRow Item
        {
            get
            {
                if (DesignHelper.IsDesigning)
                {
                    RecordCount = DesignHelper.FillItems(out items, ModelName, PageSize);
                }
                else if (item == null)
                {
                    ModelDBHelper helper = ModelDBHelper.Create(ModelName);
                    Criteria c = CreateEntryCriteria();
                    DataTable dt = helper.Query(c, CreateOrders(), 0, 0);
                    return dt.Rows.Count > 0 ? dt.Rows[0] : dt.NewRow();
                }
                return item;
            }
        }

        /// <summary>
        /// 数据列表
        /// </summary>
        public virtual DataRowCollection Items
        {
            get
            {
                if (items == null)
                {
                    if (DesignHelper.IsDesigning)
                    {
                        RecordCount = DesignHelper.FillItems(out items, ModelName, PageSize);
                    }
                    else
                    {
                        ModelDBHelper helper = ModelDBHelper.Create(ModelName);
                        Criteria c = CreateListCriteria();
                        RecordCount = helper.Count(c);

                        items = helper.Query(c, CreateOrders(), StartItem, PageItemsCount).Rows;
                    }
                }
                return items;
            }
        }

        /// <summary>
        /// 按ID取得详细页的Url
        /// </summary>
        /// <param name="id">条目ID</param>
        /// <returns></returns>
        public string GetUrl(object id)
        {
            if (!ChannelMap.ContainsKey(BindColumnID))
            {
                ChannelMap.Add(BindColumnID, ChannelHelper.GetChannel(BindColumnID, null));
            }
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            string ext = si.UrlFormat;
            return String.Format("{0}{1}.{2}", ChannelMap.ContainsKey(BindColumnID) && ChannelMap[BindColumnID] != null ? ChannelMap[BindColumnID].FullUrl : "", We7Helper.GUIDToFormatString(ToStr(id)), ext);
        }

        /// <summary>
        /// 按ID取得详细页的Url
        /// </summary>
        /// <param name="id">条目ID</param>
        /// <returns></returns>
        public string GetUrl(object id, object ChannelID)
        {
            if (ChannelID == null)
            {
                return "";
            }
            string strChannelID = ChannelID.ToString();
            if (!ChannelMap.ContainsKey(strChannelID))
            {
                ChannelMap.Add(strChannelID, ChannelHelper.GetChannel(strChannelID, null));
            }
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            string ext = si.UrlFormat;
            return String.Format("{0}{1}.{2}", ChannelMap.ContainsKey(strChannelID) && ChannelMap[strChannelID] != null ? ChannelMap[strChannelID].FullUrl : "", We7Helper.GUIDToFormatString(ToStr(id)), ext);
        }

        /// <summary>
        /// 取得第一张图片
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <param name="defImg"></param>
        /// <returns></returns>
        [Obsolete]
        public static string GetFirstImage(object json, string type, string defImg)
        {
            return GetImage(json, type, defImg);
        }

        public static string GetImage(object json, string type, string defImg)
        {
            IIC iic = GetIIC(json);
            foreach (ImageInfo info in iic)
            {
                ImageItem item = info.GetItemByType(type);
                if (item != null && !String.IsNullOrEmpty(item.Src))
                {
                    return item.Src;
                }
            }
            return defImg;
        }

        /// <summary>
        /// 取得某种类型的图片
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetImage(object json, string type)
        {
            return GetFirstImage(json, type, "");
        }

        /// <summary>
        /// 取得某种类型的图片
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string GetImage(object json)
        {
            return GetImage(json, "");
        }

        /// <summary>
        /// 取得图片信息集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IIC GetIIC(object json)
        {
            string s = json as string;
            return !String.IsNullOrEmpty(s) ? new IIC(s) : new IIC();
        }

        public static string RemoveHtml(object s)
        {
            string str = We7Helper.RemoveHtml(s != null ? s.ToString() : String.Empty);
            return str.Replace(" ", "").Replace("&nbsp;", "").Replace("　", "");
        }

        /// <summary>
        /// 通过取得类型的名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetCat(string keyword)
        {
            Category cat = HelperFactory.Instance.GetHelper<CategoryHelper>().GetCategoryByKeyword(keyword);
            return cat != null ? cat.Name : String.Empty;
        }

        /// <summary>
        /// 将数据转化为数据表
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataRowCollection  ConvertObjectToDataTable(object value,string modelname)
        {
            DataTable dt = ModelHelper.CreateDataset(modelname).Tables[0];

            if (value != null && value != DBNull.Value)
            {
                string xml = value as string;
                if (!String.IsNullOrEmpty(xml))
                {
                    try
                    {
                        TextReader reader=new StringReader(xml);
                        dt.ReadXml(reader);
                    }
                    catch { }
                }
            }

            return dt.Rows;
        }

        #endregion

        #region 私有方法

        protected virtual Criteria CreateListCriteria()
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (UseColumn)
            {

                if (IncludeChildren)
                {
                    Criteria c2 = new Criteria(CriteriaType.Equals, "OwnerID", BindColumnID);
                    c2.Mode = CriteriaMode.Or;
                    List<Channel> chs = ChannelHelper.GetChildren(BindColumnID);
                    if (chs != null)
                    {
                        foreach (Channel ch in chs)
                        {
                            c2.AddOr(CriteriaType.Equals, "OwnerID", ch.ID);
                        }
                    }
                    c.Criterias.Add(c2);
                }
                else
                {
                    c.Add(CriteriaType.Equals, "OwnerID", BindColumnID);
                }
            }
            if (ShowAtHome)
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            if (!String.IsNullOrEmpty(Tag))
            {
                c.Add(CriteriaType.Like, "Tags", "%" + Tag + "%");
            }
            c.Add(CriteriaType.Equals, "State", 1);
            ProcessQueryParam(c);

            return c;
        }

        void ProcessQueryParam(Criteria c)
        {
            if (!String.IsNullOrEmpty(QueryParam))
            {
                string[] groups = QueryParam.Split('&');
                foreach (string s in groups)
                {
                    string[] kvp = s.Split('=');
                    if (kvp.Length > 1)
                    {
                        if (kvp[1].Contains(","))
                        {
                            ProcessOrCriteria(c,kvp[0],kvp[1]);
                        }
                        else
                        {
                            c.Add(CriteriaType.Equals, kvp[0], kvp[1]);
                        }
                    }
                    else
                    {
                        string v = EncodeQueryParam ? HttpUtility.UrlDecode(Request[kvp[0]]) : Request[kvp[0]];
                        if (!String.IsNullOrEmpty(v) && !String.IsNullOrEmpty(v.Trim()))
                        {
                            if (v.Contains(","))
                            {
                                ProcessOrCriteria(c, kvp[0],v.Trim());
                            }
                            else
                            {
                                c.Add(CriteriaType.Equals, kvp[0], ConvertValue(ModelName, kvp[0], v.Trim()));
                            }
                        }
                    }
                }
            }
        }

        private static void ProcessOrCriteria(Criteria c, string k,string v)
        {
            string[] ss = v.Split(',');
            Criteria c2 = new Criteria(CriteriaType.None);
            c2.Mode = CriteriaMode.Or;
            foreach (string s in ss)
            {
                c2.Add(CriteriaType.Equals, k, s);
            }
            if (c2.Criterias.Count > 0)
            {
                c.Criterias.Add(c2);
            }
        }

        protected virtual Criteria CreateEntryCriteria()
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Add(CriteriaType.Equals, "ID", ArticleID);
            return c;
        }

        protected virtual List<Order> CreateOrders()
        {
            List<Order> orders = new List<Order>();
            orders.Add(new Order("IsShow", OrderMode.Desc));
            orders.Add(new Order("Updated", OrderMode.Desc));
            orders.Add(new Order("ID", OrderMode.Desc));
            return orders;
        }

        protected object ConvertValue(string modelName, string field, string value)
        {
            ModelInfo model = ModelHelper.GetModelInfo(modelName);
            if (model == null)
                throw new ArgumentException("当前模型[" + modelName + "]不存在。");

            object result = null;
            We7DataColumn dc = model.DataSet.Tables[0].Columns[field];
            if (dc != null)
            {
                result = TypeConverter.StrToObjectByTypeCode(value, dc.DataType);
            }
            return result;
        }

        #endregion
    }
}
