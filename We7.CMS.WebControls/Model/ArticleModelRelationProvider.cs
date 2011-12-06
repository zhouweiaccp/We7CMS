using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using Thinkment.Data;
using System.Web;
using We7.Model.Core;
using System.Data;

namespace We7.CMS.WebControls
{
    public class ArticleModelRelationProvider:BaseWebControl
    {
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }
        /// <summary>
        /// 映射字段
        /// </summary>
        public string RelationKey { get; set; }

        private int pageSize = 1;
        /// <summary>
        /// 每一页条数
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }


        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 关联控件
        /// </summary>
        public string RelationCtr { get; set; }

        private string relationValue;
        /// <summary>
        /// 映射值
        /// </summary>
        public string RelationValue
        {
            get
            {
                if (String.IsNullOrEmpty(relationValue))
                {
                    BaseWebControl ctr = null;
                    if (!String.IsNullOrEmpty(RelationCtr) && (ctr = Parent.FindControl(RelationCtr) as BaseWebControl) != null)
                    {
                        relationValue = ctr.RelationValue;
                    }
                    else
                    {
                        relationValue = ArticleHelper.GetArticleIDFromURL();
                    }
                }
                return relationValue;
            }
        }

        private List<Article> items;
        public List<Article> Items
        {
            get
            {
                if (items == null)
                {
                    ModelInfo info = ModelHelper.GetModelInfo(ModelName);
                    We7DataColumn col=info.DataSet.Tables[0].Columns[RelationKey];
                    if (col==null||(col.Direction != ParameterDirection.Output && col.Direction != ParameterDirection.InputOutput))
                        throw new Exception("关联控件没有设置映射字段");
                    string rk=string.IsNullOrEmpty(col.Mapping)?col.Name:col.Mapping;
                    Criteria c = new Criteria(CriteriaType.Equals, rk, RelationValue);
                    if (!String.IsNullOrEmpty(ModelName))
                    {
                        c.Add(CriteriaType.Equals, "ModelName", ModelName);
                    }
                    int count = ArticleHelper.Assistant.Count<Article>(c);
                    pageSize=pageSize>count?count:pageSize;
                    items=ArticleHelper.Assistant.List<Article>(c, new Order[] {new Order("Updated",OrderMode.Desc)},0,pageSize);
                }
                return items;
            }
        }

        /// <summary>
        /// 取得当前记录的Url
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public string GetUrl(Article a)
        {
            string key=a.ModelName + "$modelchannelurl$const";
            if (!ChannelMap.ContainsKey(key))
            {
                lock (Page)
                {
                    if (!ChannelMap.ContainsKey(key))
                    {
                        List<Channel>  chs=ChannelHelper.GetChannelByModelName(a.ModelName);
                        ChannelMap.Add(key, chs != null && chs.Count > 0 ? chs[0].FullUrl : "");
                    }
                }
            }
            return String.Format("{0}{1}", ChannelMap[key], a.FullUrl);
        }

        private IDictionary<string, string> ChannelMap
        {
            get
            {
                if (HttpContext.Current.Items["______ChannelMap___"] == null)
                {
                    HttpContext.Current.Items["______ChannelMap___"] = new Dictionary<string, string>();
                }
                return HttpContext.Current.Items["______ChannelMap___"] as IDictionary<string, string>;
            }
        }
    }
}
