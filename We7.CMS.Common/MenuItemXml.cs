using System;
using System.Collections.Generic;
using System.Text;


namespace We7.CMS.Common
{
    /// <summary>
    /// 文章信息类
    /// </summary>
    [Serializable]
    public class MenuItemXml 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 显示文字
        /// </summary>
        public string Lable { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 父类别编号
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// 旧编号
        /// </summary>
        public string Oldid { get; set; }

        /// <summary>
        /// 旧父类别编号
        /// </summary>
        public string Oldparent { get; set; }

        /// <summary>
        /// 是否匹配参数
        /// </summary>
        public string MatchParameter { get; set; }

        /// <summary>
        /// 是否带链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 分组 0：系统菜单 1：自定义
        /// </summary>
        public string Group { get; set; }
    }
}
