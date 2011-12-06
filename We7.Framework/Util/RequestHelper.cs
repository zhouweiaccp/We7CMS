using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace We7.Framework.Util
{
    /// <summary>
    /// Request帮助类
    /// </summary>
    public sealed class RequestHelper
    {
        /// <summary>
        /// 获取参数值 不存在返回默认值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        public static T Get<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name is null!");
            }

            object value = HttpContext.Current.Request[name];

            if (value != null)
            {
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T Get<T>(string name, T defaultValue)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name is null!");
            }

            object value = HttpContext.Current.Request[name];

            if (value != null)
            {
                return (T)value;
            }
            else
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// 添加或修改参数值
        /// </summary>
        /// <param name="url">Url链接字符串</param>
        /// <param name="key">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public static string AddOrUpdateParam( string url, string key, string value)
        {
            int fragPos = url.LastIndexOf("#");
            string fragment = string.Empty;
            if (fragPos > -1)
            {
                fragment = url.Substring(fragPos);
                url = url.Substring(0, fragPos);
            }
            int querystart = url.IndexOf("?");
            if (querystart < 0)
            {
                url += "?" + key + "=" + value;
            }
            else
            {
                Regex reg = new Regex(@"(?<=[&\?])" + key + @"=[^\s&#]*", RegexOptions.Compiled);
                if (reg.IsMatch(url))
                    url = reg.Replace(url, key + "=" + value);
                else
                    url += "&" + key + "=" + value;
            }
            url= url + fragment;
            return url;

        }

        /// <summary>
        /// 删除参数
        /// </summary>
        /// <param name="url">URL链接</param>
        /// <param name="key">参数名称</param>
        /// <returns></returns>
        public static string RemoveParam(string url,string key)
        {
            Regex reg = new Regex(@"[&\?]" + key + @"=[^\s&#]*&?", RegexOptions.Compiled);
            return reg.Replace(url, new MatchEvaluator(PutAwayGarbageFromURL));
        }

        //根据正则表达式删除
        private static string PutAwayGarbageFromURL(Match match)
        {
            string value = match.Value;
            if (value.EndsWith("&"))
                return value.Substring(0, 1);
            else
                return string.Empty;
        }


    }
}