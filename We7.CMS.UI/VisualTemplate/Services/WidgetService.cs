using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.CMS.Module.VisualTemplate.Models;
using We7.Framework.Util;
using System.Web;
using System.IO;
using We7.CMS.WebControls.Core;

namespace We7.CMS.Module.VisualTemplate.Services
{
    /// <summary>
    /// WdigetService
    /// </summary>
    public class WidgetService
    {

        private static string widgetConfigPath = string.Empty;  //配置文件
        /// <summary>
        /// widget配置文件路径
        /// </summary>
        public static string WidgetConfigPath
        {
            get
            {
                if (string.IsNullOrEmpty(widgetConfigPath))
                {
                    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Widgets/WidgetsIndex.xml");
                }
                else
                {
                    return widgetConfigPath;
                }
            }
        }
        /// <summary>
        /// 获取系统widgets
        /// </summary>
        /// <returns></returns>
        public WidgetCollection GetSystemWidgetList()
        {
            var widgets = new WidgetCollection();
            try
            {
                //检查WidgetIndex.xml是否存在
                if (!File.Exists(WidgetConfigPath))
                {
                    BaseControlHelper Helper = new BaseControlHelper();
                    Helper.CreateWidegetsIndex();
                }

                widgets = SerializationHelper.Load<WidgetCollection>(WidgetConfigPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return widgets;
        }
    }
}
