using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using We7.Framework;
using We7.Framework.Util;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// Html呈现数据链
    /// </summary>
    public class RenderChain
    {
        private const string RenderChainCacheKey = "$RenderChain$CacheKey$";

        private List<IRender> renders;
        private int index;
        private static string configPath;

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string ConfigPath
        {
            get
            {
                if (String.IsNullOrEmpty(configPath))
                {
                    configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config/Render.config");
                }
                return configPath;
            }
        }

        /// <summary>
        /// 取得所有的Html格式化器
        /// </summary>
        private List<IRender> Renders
        {
            get
            {
                if (renders == null)
                {
                    renders = AppCtx.Cache.RetrieveObject<List<IRender>>(RenderChainCacheKey);
                    if (renders == null)
                    {
                        renders = GetRenders();
                        AppCtx.Cache.AddObjectWithFileChange(RenderChainCacheKey, renders, ConfigPath);
                    }
                }
                return renders;
            }
        }

        public void DoRender(ref string content, RenderArguments args)
        {
            if (!args.IsFinished && index < Renders.Count)
            {
                Renders[index++].Render(this, ref content, args);
            }
        }

        /// <summary>
        /// 取得所有的数据呈现者
        /// </summary>
        /// <returns></returns>
        private List<IRender> GetRenders()
        {
            List<IRender> list = new List<IRender>();
            XmlNodeList nodes = XmlHelper.GetXmlNodeList(ConfigPath, "//item");
            if (nodes != null)
            {
                foreach (XmlElement xe in nodes)
                {
                    string type = xe.GetAttribute("type");
                    if (!String.IsNullOrEmpty(type) && !String.IsNullOrEmpty(type.Trim()))
                    {
                        IRender render = Utils.CreateInstance<IRender>(type);
                        if (render != null)
                            list.Add(render);
                    }
                }
            }
            return list;
        }
    }
}
