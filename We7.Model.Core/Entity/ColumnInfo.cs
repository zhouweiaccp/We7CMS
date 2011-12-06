using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace We7.Model.Core
{
    /// <summary>
    /// 表格列信息集合
    /// </summary>
    [Serializable]
    public class ColumnInfoCollection : Collection<ColumnInfo>
    {
        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ColumnInfo this[string name]
        {
            get
            {
                foreach (ColumnInfo p in this)
                {
                    if (p.Name == name)
                        return p;
                }
                return null;
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Name == name)
                    {
                        this[i] = value;
                    }
                }
            }
        }

        public void AddOrUpdate(ColumnInfo panel)
        {
            if (this[panel.Name] != null)
            {
                this[panel.Name] = panel;
            }
            else
            {
                this.Add(panel);
            }
        }
    }

    /// <summary>
    /// 表格列信息
    /// </summary>
    [Serializable]
    public class ColumnInfo
    {
        /// <summary>
        /// 列对应的数据字段
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 列标签
        /// </summary>
        [XmlAttribute("label")]
        public string Label { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        [XmlAttribute("width")]
        public string Width { get; set; }

		/// <summary>
		/// 高
		/// </summary>
		[XmlAttribute("height")]
		public string Height { get; set; }

        /// <summary>
        /// 列类型
        /// </summary>
        [XmlAttribute("type")]
        public string Type { get; set; }

        /// <summary>
        /// 对应方式
        /// </summary>
        [XmlAttribute("align")]
        public HorizontalAlign Align { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [XmlAttribute("index")]
        public int Index { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [XmlAttribute("visible")]
        public bool Visible { get; set; }

		/// <summary>
		/// 是否缩略图
		/// </summary>
		[XmlAttribute("isThumb")]
		public bool IsThumb { get; set; }

		/// <summary>
		/// 是否有链接
		/// </summary>
		[XmlAttribute("isLink")]
		public bool IsLink { get; set; }

        /// <summary>
        /// 映射字段
        /// </summary>
        [XmlAttribute("mapping")]
        public string Mapping { get; set; }

        /// <summary>
        /// 表达式
        /// </summary>
        [XmlAttribute("expr")]
        public string Expression { get; set; }

        /// <summary>
        /// 转化类型
        /// </summary>
        [XmlAttribute("convert")]
        public string ConvertType { get; set; }

        private ParamCollection _Params = new ParamCollection();
        /// <summary>
        /// 参数集合
        /// </summary>
        [XmlElement("param")]
        public ParamCollection Params
        {
            get { return _Params; }
            set { _Params = value; }
        }
    }
}
