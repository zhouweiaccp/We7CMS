using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace We7
{
    public delegate TResult Func<T1,T2,TResult>(T1 arg1,T2 arg2);

    public delegate void Action<T1, T2>(T1 arg1, T2 arg2);

    public delegate void Action<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);


    /// <summary>
    /// 名称类型
    /// </summary>
    [Serializable]
    public class NameType
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [XmlAttribute("type")]
        public string Type { get; set; }
    }

    /// <summary>
    /// 类型名称集合
    /// </summary>
    public class NameTypeCollection : Collection<NameType>
    {
        /// <summary>
        /// 按名称查询类型字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>类型</returns>
        public string this[string name]
        {
            get
            {
                foreach (NameType item in this)
                {
                    if (item.Name == name)
                        return item.Type;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// 按名称取得对象实例
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>实例</returns>
        public object GetInstance(string name)
        {
            string type = this[name];
            if (!String.IsNullOrEmpty(type))
            {
                return We7.Framework.Util.Utils.CreateInstance(type);
            }
            return null;
        }

        /// <summary>
        /// 按名称取得对象实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <param name="name">名称</param>
        /// <returns>实例</returns>
        public T GetInstance<T>(string name)
        {
            return (T)GetInstance(name);
        }
    }
}
