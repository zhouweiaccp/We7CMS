using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.Framework;
using Thinkment.Data;

namespace We7.CMS.WebControls
{
    public class CategoryMapProvider : BaseWebControl
    {
        private string queryKey = "cat";
        /// <summary>
        /// 类别关键字
        /// </summary>
        public string Keyword { get; set; }

        private int _maxLength = 5;
        /// <summary>
        /// 标题最大字数
        /// </summary>
        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                _maxLength = value;
            }
        }
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
        /// 链接地址
        /// </summary>
        public string Url { get; set; }

        private int _columnCount = 4;
        /// <summary>
        /// 显示列数
        /// </summary>
        public int ColumnCount
        {
            get { return _columnCount; }
            set { _columnCount = value; }
        }

        /// <summary>
        /// 缓存
        /// </summary>
        string CacheKey { get { return "$We7$CMS$WebControls$CategoryMapProvider$" + Keyword ?? ""; } }

        public CategoryCollection Items
        {
            get
            {
                return HelperFactory.GetHelper<CategoryHelper>().GetAllChildrenByKeyword(Keyword);
            }
        }


        public List<Category> GetAllFirstChildrenByKeyword(string keyword)
        {
            return CategoryHelper.CacheRecord.GetInstance<List<Category>>("GetAllFirstChildrenByKeyword$" + keyword, () =>
            {
                List<Category> result = null;
                List<Category> list = HelperFactory.Instance.Assistant.List<Category>(new Criteria(CriteriaType.Equals, "KeyWord", keyword), new Order[] { new Order("Index"), new Order("ID", OrderMode.Asc) });
                if (list != null && list.Count > 0)
                {
                    result = HelperFactory.Instance.Assistant.List<Category>(new Criteria(CriteriaType.Equals, "ParentID", list[0].ID), new Order[] { new Order("Index"), new Order("ID") });
                }
                return result ?? new List<Category>();
            });
        }

        /// <summary>
        /// 格式化url
        /// </summary>
        /// <param name="departID"></param>
        /// <returns></returns>
        public string FormartUrl(string keyword)
        {
            return string.Format(Url + "?" + QueryKey + "={0}", keyword);
        }
    }
}
