using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.WebControls;
using System.Data;
using We7.CMS.WebControls.Core;
using We7.Model.Core.Data;
using Thinkment.Data;
using We7.CMS.Common;
using System.Web;
using We7.Model.Core;
using We7.Framework.Util;
using We7.Framework.Common;

namespace We7.CMS.UI.Widget
{
    public abstract class BaseWidgetList : ThinkmentDataControl
    {
        private int pageSize = 10;
        private int titleMaxLength = 30;
        private int summaryMaxLength = 200;
        private string dateFormat = "yyyy-MM-dd";
        private string cssClass;
        private Channel channel;

        [Parameter(Title = "栏目", Type = "Channel", Description = "如果按栏目查询则只查询指定栏目中的数据，如果没有选中按栏目查询，则指定栏目做为导航使用")]
        public string OwnerID { get; set; }

        [Parameter(Title = "页记录数", Type = "Number", Required = true, DefaultValue = "10")]
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        [Parameter(Title = "按栏目查询", Type = "Boolean", Required = true)]
        public bool QueryByColumn { get; set; }

        [Parameter(Title = "包含子栏目信息", Type = "Boolean")]
        public bool IncludeChildren { get; set; }



        [Parameter(Title = "置顶信息", Type = "Boolean")]
        public bool ShowAtHome { get; set; }

        [Parameter(Title = "查询标签", Type = "String")]
        public string Tag { get; set; }

        [Parameter(Title = "查询字符串", Type = "String", Description = "查询只符串不能包含子查询")]
        public string QueryParam { get; set; }

        [Parameter(Title = "Url中的查询信息是否编码", Type = "Boolean")]
        public bool EncodeQueryParam { get; set; }

        [Parameter(Title = "标签最大长度", Type = "Number", DefaultValue = "30")]
        public int TitleMaxLength
        {
            get { return titleMaxLength; }
            set { titleMaxLength = value; }
        }

        [Parameter(Title = "简介最大长度", Type = "Number", DefaultValue = "200")]
        public int SummaryMaxLength
        {
            get { return summaryMaxLength; }
            set { summaryMaxLength = value; }
        }

        [Parameter(Title = "时间", Type = "String", DefaultValue = "yyyy-MM-dd")]
        public string DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        [Parameter(Title = "缩略图标签", Type = "String")]
        public string ThumbnailTag { get; set; }

        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "area")]
        public virtual string CssClass
        {
            get { return String.IsNullOrEmpty(cssClass) ? "area" : cssClass; }
            set { cssClass = value; }
        }

        /// <summary>
        /// 上边距10像素
        /// </summary>
        [Parameter(Title = "上边距10像素", Type = "Boolean", DefaultValue = "1")]
        public bool MarginTop10 { get; set; }

        /// <summary>
        /// 下边距10像素
        /// </summary>
        [Parameter(Title = "左边距10像素", Type = "Boolean", DefaultValue = "1")]
        public bool MarginLeft10 { get; set; }

        /// <summary>
        /// 附加的Css样式
        /// </summary>
        protected virtual string MarginCss
        {
            get { return (MarginTop10 ? " mtop10" : "") + (MarginLeft10 ? " mleft10" : ""); }
        }

        /// <summary>
        /// 模型名称
        /// </summary>
        public virtual string ModelName { get; set; }

        /// <summary>
        /// 输出的查询数据
        /// </summary>
        public DataRowCollection Items { get; set; }

        /// <summary>
        /// 生成列表查询条件
        /// </summary>
        /// <returns></returns>
        protected abstract Criteria CreateCriteria();

        protected virtual List<Order> CreateOrders()
        {
            return new List<Order>
            {
                new Order ("Index"),
				new Order("Updated",OrderMode.Desc),
                new Order("ID",OrderMode.Desc)
            };
        }

        /// <summary>
        /// 处理查询字符串
        /// </summary>
        /// <param name="c"></param>
        protected void ProcessQueryParam(Criteria c)
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
                            ProcessOrCriteria(c, kvp[0], kvp[1]);
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
                                ProcessOrCriteria(c, kvp[0], v.Trim());
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

        /// <summary>
        /// 处理一个查询组的字符串信息
        /// </summary>
        /// <param name="c"></param>
        /// <param name="k"></param>
        /// <param name="v"></param>
        protected void ProcessOrCriteria(Criteria c, string k, string v)
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


        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// 当前栏目信息
        /// </summary>
        protected Channel Channel
        {
            get
            {
                if (channel == null)
                {
                    channel = ChannelHelper.GetChannel(OwnerID, null) ?? new Channel();
                }
                return channel;
            }
        }

        /// <summary>
        /// 将字段转为值
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 得到文章的URL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string GetUrl(object id)
        {
            return UrlHelper.GetUrl(OwnerID, id as string);
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
        /// 取得图片信息集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IIC GetIIC(object json)
        {
            string s = json as string;
            return !String.IsNullOrEmpty(s) ? new IIC(s) : new IIC();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (string.IsNullOrEmpty(OwnerID))
            {
                OwnerID = ChannelHelper.GetChannelIDFromURL();
            }
        }

    }
}
