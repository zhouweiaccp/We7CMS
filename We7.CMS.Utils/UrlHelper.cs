using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using System.Web;
using We7.Framework;
using We7.Framework.Config;
using System.Text.RegularExpressions;

namespace We7.CMS
{
    public class UrlHelper
    {
        /// <summary>
        /// 栏目属性集合
        /// </summary>
        private static Dictionary<string, Channel> Channels
        {
            get
            {
                Dictionary<string, Channel> channels = HttpContext.Current.Items["$UrlHelper$Channels"] as Dictionary<string, Channel>;
                if (channels == null)
                {
                    channels = new Dictionary<string, Channel>();
                    HttpContext.Current.Items["$UrlHelper$Channels"] = channels;
                }
                return channels;
            }
        }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        private static string Ext
        {
            get
            {
                string ext = HttpContext.Current.Items["$UrlHelper$Ext"] as string;
                if (String.IsNullOrEmpty(ext))
                {
                    GeneralConfigInfo si = GeneralConfigs.GetConfig();
                    ext = si.UrlFormat;
                }
                return ext;
            }
        }

        /// <summary>
        ///根据栏目ID取得栏目信息
        /// </summary>
        /// <param name="ownerID"></param>
        /// <returns></returns>
        public static Channel GetChannel(string ownerID)
        {
            Channel ch=null;

            if (!String.IsNullOrEmpty(ownerID))
            {
                if (Channels.ContainsKey(ownerID))
                {
                    ch = Channels[ownerID];
                }
                else
                {
                    ch = HelperFactory.Instance.GetHelper<ChannelHelper>().GetChannel(ownerID,null);
                    Channels.Add(ownerID, ch);
                }
            }
            return ch;
        }

        public static Channel GetModelChannel(string modelName)
        {
            Channel ch = null;

            if (!String.IsNullOrEmpty(modelName))
            {
                if (Channels.ContainsKey(modelName))
                {
                    ch = Channels[modelName];
                }
                else
                {
                    List<Channel> chs = HelperFactory.Instance.GetHelper<ChannelHelper>().GetChannelByModelName(modelName);
                    if (chs != null && chs.Count > 0)
                    {
                        ch = chs[0];
                        Channels.Add(modelName, chs[0]);
                    }
                }
            }
            return ch;
        }

        /// <summary>
        /// 取得当前栏目Url
        /// </summary>
        /// <param name="ownerID"></param>
        /// <returns></returns>
        public static string GetChannelUrl(string ownerID)
        {
            Channel ch = GetChannel(ownerID);
            return ch != null ? ch.FullUrl : String.Empty;
        }

        public static string GetModelChannelUrl(string modelName)
        {
            Channel ch = GetModelChannel(modelName);
            return ch != null ? ch.FullUrl : String.Empty;
        }
        
        /// <summary>
        /// 根据栏目ID与信息ID取得详细页Url
        /// </summary>
        /// <param name="ownerID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetUrl(string ownerID, string id)
        {
            return String.Format("{0}/{1}.{2}", GetChannelUrl(ownerID).TrimEnd('/', '\\'), We7Helper.GUIDToFormatString(id), Ext);
        }

        public static string GetModelUrl(string modelName, string id)
        {
            return String.Format("{0}/{1}.{2}", GetModelChannelUrl(modelName).TrimEnd('/', '\\'), We7Helper.GUIDToFormatString(id), Ext);
        }

        /// <summary>
        /// 从Url中取得当前信息的ID号
        /// </summary>
        /// <returns></returns>
        public static string GetID()
        {
            string result = String.Empty;

            HttpContext Context = HttpContext.Current;
            if (Context!=null)
            {
                HttpRequest request=Context.Request;
                if(!String.IsNullOrEmpty(request["id"]))
                {
                    result = request["id"];
                }
                else if(!String.IsNullOrEmpty(request["aid"]))
                {
                    result=request["aid"];
                }
                else
                {
                    result=GetIDFromUrl();
                }
            }
            return result;
        }

        /// <summary>
        /// 从Url中取得ID号
        /// </summary>
        /// <returns></returns>
        public static string GetIDFromUrl()
        {
            string path = HttpContext.Current.Request.RawUrl; //取得Url的原始地址

            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si == null) return "";
            string ext = si.UrlFormat;
            if (ext == null || ext.Length == 0) ext = "html";

            if (path.LastIndexOf("?") > -1)
            {
                if (path.ToLower().IndexOf("article=") > -1)
                    path = path.Substring(path.ToLower().IndexOf("article=") + 8);
                else
                    path = path.Remove(path.LastIndexOf("?"));
            }

            string mathstr = @"/(\w|\s|(-)|(_))+\." + ext + "$";
            if (path.ToLower().EndsWith("default." + ext))
                path = path.Remove(path.Length - 12);
            if (path.ToLower().EndsWith("index." + ext))
                path = path.Remove(path.Length - 10);

            if (Regex.IsMatch(path, mathstr))
            {
                int lastSlash = path.LastIndexOf("/");
                if (lastSlash > -1)
                {
                    path = path.Remove(0, lastSlash + 1);
                }

                int lastDot = path.LastIndexOf(".");
                if (lastDot > -1)
                {
                    path = path.Remove(lastDot, path.Length - lastDot);
                }

                if (We7Helper.IsGUID(We7Helper.FormatToGUID(path)))
                    path = We7Helper.FormatToGUID(path);
                else
                {
                    int lastSub = path.LastIndexOf("-");
                    if (lastSub > -1)
                    {
                        path = path.Remove(0, lastSub + 1);
                    }

                    if (!We7Helper.IsNumber(path))
                        path = "";
                    else
                        path = HelperFactory.Instance.GetHelper<ArticleHelper>().GetArticleIDBySN(path);
                }
                
                return path;
            }
            else
                return string.Empty;
        }
    }
}
