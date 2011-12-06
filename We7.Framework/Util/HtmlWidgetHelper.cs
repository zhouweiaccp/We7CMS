using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace We7.Framework.Util
{
    /// <summary>
    /// 静态部件生成处理类
    /// </summary>
    public class HtmlWidgetHelper
    {
        private static HtmlWidgetHelper scope = null;
        private static object syncRoot = new object();

        public static HtmlWidgetHelper Scope
        {
            get
            {
                if (scope == null)
                {
                    lock (syncRoot)
                    {
                        if (scope == null)
                        {
                            scope = new HtmlWidgetHelper();
                        }
                    }
                }
                return scope;
            }
        }
        /// <summary>
        /// 生成静态部件
        /// </summary>
        /// <param name="widgetname">部件description</param>
        /// <param name="fileName">部件文件名称</param>
        /// <param name="widgetValue">部件内容</param>
        /// <param name="path">路径</param>
        /// <param name="isNew">是否新增</param>
        /// <returns></returns>
        public EnumCreateHtmlWidget CreateCreateHtmlWidget(string widgetname, string fileName, string widgetValue, string path, bool isNew)
        {
            string filePath = path + "\\" + fileName + "\\" + fileName + ".ascx";
            if (isNew && FileHelper.Exists(filePath))
                return EnumCreateHtmlWidget.repeat;
            else
            {
                string top = string.Empty;
                string resultWidgetValue = System.Text.RegularExpressions.Regex.Match(widgetValue, @"<body>(?<value>[\s|\S]*?)</body>").Groups["value"].Value;
                StringBuilder sbValue = new StringBuilder(); ;
                if (isNew)
                {
                    top = string.Format("<%@ Control Language=\"C#\" AutoEventWireup=\"true\" Inherits=\"We7.CMS.WebControls.BaseControl\" %> \n <script type=\"text/C#\" runat=\"server\"> \n   [ControlDescription(Desc = \"{0}\",Author=\"{1}\")] \n [ControlGroupDescription(Label = \"{2}\", Icon = \"{3}\", Description = \"{4}\", DefaultType = \"{5}\")] \n string MetaData; \n </script> \n <!--htmlWidgetStart--> \n <div>", widgetname.Trim(), "system", widgetname, widgetname, widgetname, fileName);
                    sbValue = new StringBuilder().Append(top).Append(resultWidgetValue).Append("</div> \n <!--HtmlWidgetEnd-->");
                }
                else
                {
                    top = top = string.Format("<%@ Control Language=\"C#\" AutoEventWireup=\"true\" Inherits=\"We7.CMS.WebControls.BaseControl\" %> \n <script type=\"text/C#\" runat=\"server\"> \n   [ControlDescription(Desc = \"{0}\",Author=\"{1}\")] \n [ControlGroupDescription(Label = \"{2}\", Icon = \"{3}\", Description = \"{4}\", DefaultType = \"{5}\")] \n string MetaData; \n </script> \n <!--htmlWidgetStart--> \n ", widgetname.Trim(), "system", widgetname, widgetname, widgetname, fileName);
                    sbValue = new StringBuilder().Append(top).Append(resultWidgetValue).Append(" \n <!--HtmlWidgetEnd-->");
                }
                //if (FileHelper.Exists(filePath)) FileHelper.DeleteFile(filePath);
                //FileHelper.WriteFile(filePath, sbValue.ToString(), System.IO.FileMode.Create);
               WriteFileWithEncoding(filePath, sbValue.ToString(), System.IO.FileMode.Create, Encoding.UTF8);
                return EnumCreateHtmlWidget.success;
            }
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filePath">文件完全限定路径</param>
        /// <param name="content">需要写入的内容</param>
        /// <param name="fileModel">如果文件存在则设置添加模式(默认为追加)</param>
        /// <param name="encoding">编码</param>
        /// <returns>成功：空；失败：错误消息</returns>
        private string WriteFileWithEncoding(string filePath, string content, FileMode fileModel, Encoding encoding)
        {
            try
            {
                //判断文件是否存在
                if (!File.Exists(filePath))
                {
                    FileHelper.WriteFile(filePath, content);
                }
                else
                {
                    FileStream fs = File.Open(filePath, fileModel, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, encoding);
                    sw.Flush();
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
                return "";
            }
            catch (Exception ex)
            {
                //log

                return ex.Message;
            }
        }
    }

    #region 枚举
    public enum EnumCreateHtmlWidget
    {
        /// <summary>
        /// 失败
        /// </summary>
        error,
        /// <summary>
        /// 成功
        /// </summary>
        success,
        /// <summary>
        /// 存在相同文件
        /// </summary>
        repeat
    }
    #endregion
}
