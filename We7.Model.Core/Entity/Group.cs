using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace We7.Model.Core
{
    /// <summary>
    /// 分组集合
    /// </summary>
    [Serializable]
    public class GroupCollection : Collection<Group>
    {
        public Group this[string name]
        {
            get
            {
                foreach (Group group in this)
                {
                    if (group.Name == name)
                        return group;
                }
                return null;
            }
        }
    }

    /// <summary>
    /// 分组
    /// </summary>
    [Serializable]
    public class Group
    {
        /// <summary>
        /// 分组名称
        /// </summary>
	    [XmlAttribute("name")]
        public string Name { get; set; }
		/// <summary>
		/// 索引值
		/// </summary>
		[XmlAttribute("index")]
		public int Index { get; set; }
		/// <summary>
		/// 后继页索引值
		/// </summary>
		[XmlAttribute("next")]
		public int Next { get; set; }

		/// <summary>
		/// 是否启用
		/// </summary>
		[XmlAttribute("enable")]
		public bool Enable { get; set; }

        private We7ControlCollection controls = new We7ControlCollection();
        /// <summary>
        /// 控件集
        /// </summary>
        [XmlElement("control")]
        public We7ControlCollection Controls
        {
            get { return controls; }
            set { controls = value; }
        }
		private ColumnInfoCollection columns = new ColumnInfoCollection();
		/// <summary>
		/// 列信息定义
		/// </summary>
		[XmlElement("column")]
		public ColumnInfoCollection Columns
		{
			get { return columns; }
			set { columns = value; }
		}

    }
}
