using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core;
using System.Text;
using We7.CMS.Common;
using We7.Model.Core.Converter;
using System.Reflection;
using We7.Framework;

namespace We7.Model.UI.Converter
{
    public class FormatTags : IOutputConvert
    {
        public object Convert(We7DataColumn column, DataRow row, string[] fields, object extobj)
        {
            Type type=extobj.GetType();
            if (type != null)
            {
                string tagfield = fields != null && fields.Length > 0 ? fields[0] : "Tags";
                PropertyInfo prop = type.GetProperty(tagfield);
                if (prop != null)
                {
                    string tags = prop.GetValue(extobj, null) as string;
                    return Format(tags);
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 格式化标签，使其变为 <a href=''>标签</a> 形式
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        private string Format(string tags)
        {
            if (string.IsNullOrEmpty(tags)) return tags;

            string rawurl =HttpContext.Current.Request.RawUrl.Replace("{", "{{").Replace("}", "}}");
            rawurl = We7Helper.AddParamToUrl(rawurl, "tag", "{0}");
            string url = "<a href='{0}'>{1}</a>";
            tags = tags.Replace("''", ",").Replace("'", "");
            string[] taglist = tags.Split(',');
            StringBuilder sb = new StringBuilder();
            foreach (string tag in taglist)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    string s= HttpUtility.UrlEncode(tag);
                    string myUrl = string.Format(rawurl, s);
                    sb.Append(string.Format(url, myUrl, tag));
                    sb.Append(",");
                }
            }
            string result = sb.ToString();
            if (result.EndsWith(",")) result = result.Remove(result.Length - 1);
            return result;
        }
    }
}
