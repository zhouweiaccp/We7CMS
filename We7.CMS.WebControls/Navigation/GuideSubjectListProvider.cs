using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.WebControls
{
    public class GuideSubjectListProvider : BaseWebControl
    {
        /// <summary>
        /// 父类别编号
        /// </summary>
        public string KeyWord { get; set; }

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
        string CacheKey { get { return "$We7$CMS$WebControls$GuideSubjectListProvider$" + KeyWord ?? ""; } }

        private List<Category> _items = null;
        public List<Category> Items
        {
            get
            {
                if (_items == null)
                {
                    if (String.IsNullOrEmpty(KeyWord))
                    {
                        KeyWord = Request["KeyWord"];
                    }
                    _items = HelperFactory.GetHelper<CategoryHelper>().GetChildrenListByKeyword(KeyWord);
                }
                return _items;
            }
        }

        /// <summary>
        /// 格式化url
        /// </summary>
        /// <param name="departID"></param>
        /// <returns></returns>
        public string FormartUrl(string keyword)
        {
            return string.Format(Url + "?cat={0}", keyword);
        }
    }
}
