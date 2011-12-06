using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Web.UI;
using We7.Framework;
using We7.CMS.Module.VisualTemplate;
using We7.Framework.TemplateEnginer;
using System.IO;
using HtmlAgilityPack;
using We7.Framework.Util;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 用户控件基类
    /// </summary>
    public abstract partial class FrontUserControl : UserControl
    {
        /// <summary>
        /// 将指定值转化为字符串
        /// </summary>
        /// <param name="fieldValue">字段值</param>
        /// <returns></returns>
        protected string ToStr(object fieldValue)
        {
            return fieldValue != null ? fieldValue.ToString() : String.Empty;
        }

        /// <summary>
        /// 将指定值转化为字符串
        /// </summary>
        /// <param name="fieldValue">字段值</param>
        /// <param name="maxlength">值的最大长度</param>
        /// <returns></returns>
        protected string ToStr(object fieldValue, int maxlength)
        {
            return ToStr(fieldValue, maxlength, "...");
        }

        /// <summary>
        /// 将指定值转化为字符串
        /// </summary>
        /// <param name="fieldValue">字段值</param>
        /// <param name="maxlength">值的最大长度</param>
        /// <param name="tail">超过指定长度的结尾字符</param>
        /// <returns></returns>
        protected string ToStr(object fieldValue, int maxlength, string tail)
        {
            string str = ToStr(fieldValue);
            return Utils.GetUnicodeSubString(str, maxlength, tail);
        }

        /// <summary>
        /// 将时间数据按指定格式转化
        /// </summary>
        /// <param name="fieldValue">时间值</param>
        /// <param name="fmt">日期格式</param>
        /// <returns></returns>
        protected string ToDateStr(object fieldValue, string fmt)
        {
            if (fieldValue != null && fieldValue is DateTime)
            {
                return Convert.ToDateTime(fieldValue).ToString(fmt);
            }
            return String.Empty;
        }

        /// <summary>
        /// 将时间数据按指定格式转化
        /// </summary>
        /// <param name="fieldValue">时间值</param>
        /// <returns></returns>
        protected string ToDateStr(object fieldValue)
        {
            return ToDateStr(fieldValue, "yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 将数据转化为字整数
        /// </summary>
        /// <param name="fieldValue">要转化的值</param>
        /// <returns></returns>
        protected int? ToInt(object fieldValue)
        {
            if (fieldValue != null && fieldValue is int)
            {
                return Convert.ToInt32(fieldValue);
            }
            else
            {
                return null;
            }
        }
    }
}
