using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace We7.Framework
{
    public abstract class SNGenerator
    {
        /// <summary>
        /// 根据数据对象创建SN
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToSN(object o)
        {
            string sn = String.Empty;
            if (o != null)
            {
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, o);
                byte[] buffer = ms.ToArray();
                sn = Convert.ToBase64String(buffer);
            }
            return sn;
        }

        /// <summary>
        /// 从SN中转化出SN对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sn"></param>
        /// <returns></returns>
        public static TResult FromSN<TResult>(string sn)
        {
            TResult result = default(TResult);
            if (!String.IsNullOrEmpty(sn))
            {
                byte[] buffer = Convert.FromBase64String(sn);
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    result = (TResult)bf.Deserialize(ms);
                }
            }
            return result;
        }

        public string ToSN()
        {
            StringBuilder sb = new StringBuilder();
            if (Fields != null)
            {
                foreach (string field in Fields)
                {
                    PropertyInfo prop = GetType().GetProperty(field);
                    if (prop != null)
                    {
                        string v = prop.GetValue(this, null) as string;
                        sb.Append(v);
                    }
                    sb.Append("$");
                }
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(sb.ToString()));
        }
        public void FromSN(string str)
        {
            if(!String.IsNullOrEmpty(str))
            {
                string s=Encoding.UTF8.GetString(Convert.FromBase64String(str));
                string[] ss=s.Split('$');
                for (int i = 0; i < Fields.Length; i++)
                {
                    PropertyInfo prop = GetType().GetProperty(Fields[i]);
                    prop.SetValue(this, ss[i], null);
                }
            }
        }

        protected abstract string[] Fields{get;}
    }

    /// <summary>
    /// 插件序列号
    /// </summary>
    [Serializable]
    public class PluginSN : SNGenerator
    {

        public string Url { get; set; }

        public string UserName { get; set; }

        protected override string[] Fields
        {
            get { return new string[] { "Url", "UserName" }; }
        }
    }
}
