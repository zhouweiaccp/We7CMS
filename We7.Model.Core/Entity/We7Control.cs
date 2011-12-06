using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Reflection;

namespace We7.Model.Core
{
    /// <summary>
    /// 控件集合
    /// </summary>
    [Serializable]
    public class We7ControlCollection : Collection<We7Control>
    {
        /// <summary>
        /// 索引，根据控件名查找控件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public We7Control this[string name]
        {
            get
            {
                foreach (We7Control ctr in this)
                {
                    if (ctr.Name == name)
                        return ctr;
                }
                return null;
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Name==name)
                    {
                        this[i] = value;
                    }
                }
            }
        }

        public void AddOrUpdate(We7Control control)
        {
            if (this[control.Name]!=null)
            {
                this[control.Name] = control;
            }
            else
            {
                this.Add(control);
            }
        }
    }

    /// <summary>
    /// 控件信息
    /// </summary>
    [Serializable]
    public class We7Control:ICloneable
    {
        private string id;
        /// <summary>
        /// 控件ID
        /// </summary>
        [XmlAttribute("id")]
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    id = Name;
                }
                return id;
            }
            set { id = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [XmlAttribute("label")]
        public string Label { get; set; }

        /// <summary>
        /// 控件类型
        /// </summary>
        [XmlAttribute("type")]
        public string Type { get; set; }

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
        /// Css样式
        /// </summary>
        [XmlAttribute("css")]
        public string CssClass { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [XmlAttribute("default")]
        public string DefaultValue { get; set; }

        private bool required = false;
        /// <summary>
        /// 是否必须
        /// </summary>
        [XmlAttribute("required")]
        public bool Required
        {
            get { return required; }
            set { required = value; }
        }

        /// <summary>
        /// 控件描述信息
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }

        private bool visible = true;
        /// <summary>
        /// 是否可见
        /// </summary>
        [XmlAttribute("visible")]
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        /// 当为空时是否过滤掉
        /// </summary>
        [XmlAttribute("ignore")]
        public bool IgnoreEmpty { get; set; }

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

        #region ICloneable 成员

        public object Clone()
        {
            We7Control ctr = new We7Control();
            foreach (PropertyInfo p in this.GetType().GetProperties())
            {
                p.SetValue(ctr, p.GetValue(this, null), null);
            }
            return ctr;
        }

        #endregion
    }
}
