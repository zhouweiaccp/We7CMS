using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;


using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS
{
    /// <summary>
    /// 关键字业务类
    /// </summary>
    [Serializable]
    [Helper("We7.KeywordHelper")]
    public class KeywordHelper : BaseHelper
    {        
        /// <summary>
        /// 关键字配置路径
        /// </summary>
        public string KeywordPath
        {
            get 
            {
                string root = System.Web.HttpContext.Current.Server.MapPath("~/Config/Dictionary");
                return Path.Combine(root, Constants.KeywordsPath); 
            }
        }
        /// <summary>
        /// 取得关键字信息
        /// </summary>
        /// <returns></returns>
        public KeyWordGroup GetKeyWordGroup()
        {
            string root = System.Web.HttpContext.Current.Server.MapPath("~/Config/Dictionary");
            KeyWordGroup keyWordGroup = new KeyWordGroup();
            keyWordGroup.FromFile(root, "keywords.xml");
            return keyWordGroup;
        }
    }
}
