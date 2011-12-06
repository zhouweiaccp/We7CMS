using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.Xml.Serialization;
using We7.Framework;
using We7.CMS.WebControls.Core;

namespace We7.CMS.WebControls
{
    public class ThinkmentDataControl:BaseControl
    {
        protected HelperFactory HelperFactory
        {
            get 
            {
                return We7.Framework.HelperFactory.Instance;
            }
        }

        ObjectAssistant assistant;
        /// <summary>
        /// 当前Helper的数据访问对象
        /// </summary>
        protected ObjectAssistant Assistant
        {
            get
            {
                if (assistant == null)
                {
                    assistant = HelperFactory.Assistant;
                }
                return assistant;
            }
            set { assistant = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected MultiDataSourceHelper MultiDataSourceHelper
        {
            get { return HelperFactory.GetHelper<MultiDataSourceHelper>(); }
        }

        ///// <summary>
        ///// 数据源
        ///// </summary>
        //[Parameter(Title = "数据源", Type = "String", DefaultValue = "", Required = false)]
        //public string DbSource;

        ///// <summary>
        ///// 显示宽度
        ///// </summary>
        //[Parameter(Title = "显示宽度", Type = "String", DefaultValue = "100%")]
        //public string Width;

        ///// <summary>
        ///// 显示高度
        ///// </summary>
        //[Parameter(Title = "显示高度", Type = "String", DefaultValue = "")]
        //public string Height;

        ///// <summary>
        ///// 
        ///// </summary>
        //protected string DocSize()
        //{

        //    StringBuilder s = new StringBuilder("style='padding: 10px;");
        //    if (!string.IsNullOrEmpty(Width))
        //    {
        //        s.Append(String.Format("width:{0};", Width));
        //    }
        //    if (!string.IsNullOrEmpty(Height))
        //    {
        //        s.Append(String.Format("height:{0};", Height));
        //    }
        //    return s.Append("'").ToString();
        //}
    }
}
