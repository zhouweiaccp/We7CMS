using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Collections;


using We7.CMS;
using We7;
using We7.CMS.Controls;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using Thinkment.Data;
using We7.Framework.Config;



namespace We7.CMS.WebControls
{
    /// <summary>
    /// 查询控件继承控件基类
    /// </summary>
    public class QueryList : BaseWebControl
    {
        public string CssClass { get; set; }
        public int ColumnCount { get; set; }
        private string bindColumnID;
        /// <summary>
        /// 缩略图类型
        /// </summary>
        public string ThumbnailTag { get; set; }
        /// <summary>
        ///最大长度
        /// </summary>
        public int MaxLength { get; set; }

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
        
        /// <summary>
        /// 控件页面要展示的列表实例
        /// </summary>
        public List<Article> list = new List<Article>();
       /// <summary>
       /// 显示的文章集合
       /// </summary>
        public List<Article> Items
        {
            get
            {
                if (DesignHelper.IsDesigning)
                {
                    DesignHelper.FillItems<Article>(out list, PageSize);
                }
                else
                {
                    if (list.Count == 0)
                    {
                        QueryEntity queryEntity = new QueryEntity();
                        queryEntity.Orders = new Order[] { new Order("Updated", OrderMode.Desc) };
                        queryEntity.ModelName = HttpUtility.UrlDecode(Request["ModelName"]);
                        string fields = HttpUtility.UrlDecode(Request["QueryFields"]);
                        if (!String.IsNullOrEmpty(fields))
                        {
                            string[] ss = fields.Split(',');
                            foreach (string s in ss)
                            {
                                queryEntity.QueryParams.Add(new QueryParam() { ColumnKey = s, ColumnValue = HttpUtility.UrlDecode(Request[s]), CriteriaType = CriteriaType.Like });
                            }
                        }
                        RecordCount = ArticleHelper.QueryArtilceModelCountByParameter(queryEntity);
                        list = ArticleHelper.QueryArticles(queryEntity, StartItem, PageItemsCount, null);
                    }
                }
                return list;
            }
        }     
        

        /// <summary>
        /// 格式化列表中的数据
        /// </summary>
        /// <param name="list">文章列表</param>
        List<Article> FormatArticlesData(List<Article> list)
        {
            //此处进行格式化页面操作
            return list;
        }

        /// <summary>
        /// 获得文章帮助类的事例
        /// </summary>
        ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
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
            return String.Format("{0}{1}.{2}", ChannelMap[BindColumnID].FullUrl, We7Helper.GUIDToFormatString(ToStr(id)), ext);
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
            return String.Format("{0}{1}.{2}", ChannelMap[strChannelID].FullUrl, We7Helper.GUIDToFormatString(ToStr(id)), ext);
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

    }
}
