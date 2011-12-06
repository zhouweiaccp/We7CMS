using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Module.VisualTemplate.Models.Temp
{
    /// <summary>
    /// 部件类型
    /// </summary>
    public enum WidgetType
    {
        /// <summary>
        /// 部件
        /// </summary>
        widget,
        /// <summary>
        /// 布局
        /// </summary>
        layout,
        /// <summary>
        /// 布局列
        /// </summary>
        layoutColumn
    }
    public class WidgetDesign
    {
        public WidgetDesign()
        {
            this.Params = new Dictionary<string, object>();
            this.Items = new List<WidgetDesign>();
            Columns = new List<Dictionary<string, object>>();
            ID = "";
            TargetId = "";
            WidgetType = WidgetType.widget;
        }
        /// <summary>
        /// Id
        /// </summary>
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 所在列
        /// </summary>
        public string TargetId
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        public string TagPrefix
        {
            get;
            set;
        }
        /// <summary>
        /// 注册的头
        /// </summary>
        public string Tag
        {
            get;
            set;
        }
        /// <summary>
        /// 类型:是widget还是layout，layoutColumn
        /// </summary>
        public WidgetType WidgetType
        {
            get;
            set;
        }
        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, object> Params { get; set; }

        /// <summary>
        /// 子控件
        /// </summary>
        public List<WidgetDesign> Items
        {
            get;
            set;
        }

        public List<Dictionary<string, object>> Columns { get; set; }
        /// <summary>
        /// 返回控件字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.Compare("body", Name, true) == 0)
            {
                return "<body></body>";
            }
            if (string.IsNullOrEmpty(Tag) || string.IsNullOrEmpty(TagPrefix) || string.IsNullOrEmpty(ID))

                throw new ArgumentNullException("Tag,TagPrefix,ID属性不可为空");
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<{0}:{1} ID=\"{2}\"", TagPrefix, Tag, ID);


            foreach (var attr in Params)
            {
                if (string.Compare("runat", attr.Key, true) == 0)
                    continue;
                sb.AppendFormat(" {0}=\"{1}\"", attr.Key, attr.Value);
            }
            sb.Append(" runat=\"server\"");

            sb.AppendFormat("</{0}:{1}", TagPrefix, Tag);
            return sb.ToString();
        }
    }
}
