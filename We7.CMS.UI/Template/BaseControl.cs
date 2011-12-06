using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using We7.Framework.TemplateEnginer;
using System.Web.UI;
using System.IO;
using We7.CMS.WebControls.Core;
using Thinkment.Data;
using We7.Framework.Algorithm;
using We7.Framework.Util;
using System.Data;
using We7.Framework.Config;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 控件基类
    /// </summary>
    public class BaseControl : UserControl
    {
        private string themePath;
        private HtmlHelper2 htmlHelper;
        private string cssClass = null;
        /// <summary>
        /// 控件的扩展参数
        /// </summary>
        protected Dictionary<string, object> UxParams = new Dictionary<string, object>();

        /// <summary>
        /// 风格样式根目录
        /// </summary>
        protected const string ThemeRoot = "/Widgets/Themes";

        /// <summary>
        /// 当前模板的主题目录
        /// </summary>
        //TODO::这儿要设置成从页面中取得主题页样式
        protected string ThemePath
        {
            get
            {
                if (string.IsNullOrEmpty(themePath))
                {
                    themePath = String.Format("/Widgets/Themes/{0}", GeneralConfigs.GetConfig().Theme);
                }
                return themePath;
            }
        }

        /// <summary>
        /// 是否使用例子数据
        /// </summary>
        protected bool IsDesigning
        {
            get
            {
                return "virtualdata".Equals(Request["virtualdata"], StringComparison.OrdinalIgnoreCase);
            }
        }


        /// <summary>
        /// 部件CssClass属性
        /// </summary>
        [Parameter(Title = "部件的Css样式表", Type = "StyleSelect", DefaultValue = "", Required = true)]
        public virtual string CssClass
        {
            get
            {
                return cssClass;
            }
            set
            {
                cssClass = value;
            }
        }

        /// <summary>
        /// 重写信息重现
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            RenderArguments args = new RenderArguments(this, UxParams);

            StringWriter strWriter = new StringWriter();
            HtmlTextWriter tempWriter = new HtmlTextWriter(strWriter);
            try
            {
                base.Render(tempWriter);
            }
            catch (Exception ex)
            {
                args.Exception = ex;
                args.IsError = true;
            };

            string content = strWriter.ToString();
            new RenderChain().DoRender(ref content, args);
            writer.Write(content);
        }

        protected override sealed void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (IsDesigning)
            {
                OnDesigning();
            }
            else
            {
                OnInitData();
            }
        }

        /// <summary>
        /// 在设计时进行数据初始化
        /// </summary>
        protected virtual void OnDesigning() { }

        protected virtual void OnInitData() { }

        #region 格式化
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
        #endregion

        /// <summary>
        /// 表单提交辅助类
        /// </summary>
        public HtmlHelper2 HtmlHelper
        {
            get
            {
                if (htmlHelper == null)
                {
                    htmlHelper = new HtmlHelper2(this);
                }
                return htmlHelper;
            }
        }

    }
}
