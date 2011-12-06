using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 插件安装管理界面类
    /// </summary>
    public class PluginMessage
    {
        private PluginType pluginType;
        public PluginMessage(PluginType plugintype)
        {
            pluginType = plugintype;
        }

        /// <summary>
        /// 安装界面的标题
        /// </summary>
        public string InstallTitle
        {
            get
            {
                string result = "";
                switch (pluginType)
                {
                    case PluginType.PLUGIN:
                        result = "安装插件";
                        break;
                    case PluginType.RESOURCE:
                        result = "安装控件";
                        break;
                    default:
                        result = "安装插件";
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// 安装界面的简介
        /// </summary>
        public string InstallSummary
        {
            get
            {
                string result = "";
                switch (pluginType)
                {
                    case PluginType.PLUGIN:
                        result = "通过安装插件，可能轻易扩展系统功能。";
                        break;
                    case PluginType.RESOURCE:
                        result = "通过安装控件，可能轻易扩展系统功能。";
                        break;
                    default:
                        result = "通过安装插件，可能轻易扩展系统功能。";
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// 安装界面的简介
        /// </summary>
        public string InstallInfo
        {
            get
            {
                string result = "";
                switch (pluginType)
                {
                    case PluginType.PLUGIN:
                        result = "插件可以无限扩展We7的功能。您可以从We7 插件网站自动安装插件或者在这个页面上传 .zip 格式的插件包。";
                        break;
                    case PluginType.RESOURCE:
                        result = "控件可以无限扩展网站的前台表现。您可以从We7 插件网站自动安装控件或者在这个页面上传 .zip 格式的控件包。";
                        break;
                    default:
                        result = "插件可以无限扩展We7的功能。您可以从We7 插件网站自动安装插件或者在这个页面上传 .zip 格式的插件包。。";
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// 上传成功
        /// </summary>
        public string UploadSuccess
        {
            get
            {
                switch (pluginType)
                {
                    case PluginType.PLUGIN:
                        return "插件上传成功,是否现在进行安装?";
                    case PluginType.RESOURCE:
                        return "控件上传成功!";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 上传错误
        /// </summary>
        public string UploadError
        {
            get
            {
                switch (pluginType)
                {
                    case PluginType.PLUGIN:
                        return "插件上传失败!";
                    case PluginType.RESOURCE:
                        return "控件上传失败!";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 安装的类型
        /// </summary>
        public string PluginLabel
        {
            get
            {
                switch (pluginType)
                {
                    case PluginType.PLUGIN:
                        return "插件";
                    case PluginType.RESOURCE:
                        return "控件";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 插件路径
        /// </summary>
        public string PluginPath
        {
            get
            {
                switch (pluginType)
                {
                    case PluginType.PLUGIN:
                        return "Plugins";
                    case PluginType.RESOURCE:
                        return "We7Controls";
                    default:
                        return "Plugins";
                }
            }
        }
    }
}
