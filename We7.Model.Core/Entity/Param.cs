using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace We7.Model.Core
{
    /// <summary>
    /// 参数集合
    /// </summary>
    [Serializable]
    public class ParamCollection : Collection<Param>
    {
        /// <summary>
        /// 索引，根据参数名取得参数值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name]
        {
            get
            {
                foreach (Param param in this)
                {
                    if (param.Name == name)
                        return param.Value;
                }
                return "";
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Name==name)
                    {
                        this[i].Value = value;
                    }
                }
            }
        }

        public void AddOrUpdate(Param pa)
        {
            bool has = false;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Name == pa.Name)
                {
                    this[i] = pa;
                    has = true;
                }
            }

            if (!has)
            {
                this.Add(pa);
            }
            
        }

        /// <summary>
        /// 包含指定属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            foreach (Param param in this)
            {
                if (param.Name == name)
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 参数信息
    /// </summary>
    [Serializable]
    public class Param
    {
        #region Cotr
        public Param() { }

        public Param(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
        public Param(string name, string value,string text)
        {
            this.Name = name;
            this.Value = value;
            this.Text = text;
        }

        #endregion
        /// <summary>
        /// 参数名
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        private string _value;
        /// <summary>
        /// 值
        /// </summary>
        [XmlAttribute("value")]
        public string Value
        {
            get
            {
                if (String.IsNullOrEmpty(_value))
                {
                    _value = Text;
                }
                return _value;
            }
            set { _value = value; }
        }

        /// <summary>
        /// 文本
        /// </summary>
        [XmlText]
        public string Text { get; set; }
    }
}
