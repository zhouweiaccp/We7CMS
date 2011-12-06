using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 执行Plugin命令过程中返回的结果
    /// </summary>
    public class PluginJsonResult
    {
        public PluginJsonResult()
        {
        }

        public PluginJsonResult(bool success, string description)
        {
            Success = success;
            Description = description;
        }

        public PluginJsonResult(bool success)
            : this(success, "")
        {
        }

        public bool Success { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("{");
            sb.AppendFormat("success:{0},desc:'{1}'", Success.ToString().ToLower(), Description);
            sb.Append("}");


            return sb.ToString();
        }
    }

    /// <summary>
    /// 插件类型
    /// </summary>
    public enum PluginType:byte{
        /// <summary>
        /// 插件
        /// </summary>
        PLUGIN,
        /// <summary>
        /// 控件
        /// </summary>
        RESOURCE
    }
}
