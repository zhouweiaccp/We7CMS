using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Web;

namespace We7.CMS.Config
{
    public delegate bool CheckDelegate();
    /// <summary>
    /// 生成配置文件的抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractConfig<T> where T:class
    {
        private static readonly object SerializeLock = new object();        

        /// <summary>
        /// 配置文件的名称
        /// </summary>
        /// <returns></returns>
        public abstract string ConfigName();

        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string ConfigFilePath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/Config/" + ConfigName()+".config");
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void Save()
        {
            lock (SerializeLock)
            {
                Serialize();
            }
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <returns></returns>
        public T Load()
        {
            lock (SerializeLock)
            {
               return DeSerialize();
            }
        }

        /// <summary>
        /// 对当前对象进行序列化
        /// </summary>
        protected void Serialize()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (Stream stream = File.Open(ConfigFilePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    serializer.Serialize(stream, this);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("保存配置文件出错:" + ex.Message);
            }
        }

        /// <summary>
        /// 对当前对象行返序列化
        /// </summary>
        protected T DeSerialize()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (Stream stream = File.Open(ConfigFilePath, FileMode.Open, FileAccess.Read))
                {
                    object obj=serializer.Deserialize(stream);
                    return obj as T;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取配置文件出错："+ex.Message);
            }
        }

        /// <summary>
        /// 用于检测是否符合条件
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool Check(CheckDelegate handler)
        {
            if(handler==null)
                throw new Exception("查测的委托不能为空!");
            return handler();
        }
    }
}
