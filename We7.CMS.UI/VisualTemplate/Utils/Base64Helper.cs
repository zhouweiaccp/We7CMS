using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Module.VisualTemplate.Utils
{
    /// <summary>
    /// Base64 (UTF-8) 编码/解码。
    /// </summary>
    public class Base64Helper
    {
        /// <summary>
        /// 解码。
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static string Decode(string base64Str)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64Str));
        }
        /// <summary>
        /// 编码。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encode(string utf8Str)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(utf8Str));
        }
    }
}
