using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Config;
using We7.Framework.Config;

namespace We7.CMS.Common
{
    /// <summary>
    /// 流转对象基类
    /// </summary>
    [Serializable]
    public class ProcessObject
    {
        /// <summary>
        /// 审批进程，0：草稿，1-3：一审~三审，8：站间审核
        /// </summary>
        public string ProcessState { get; set; }

        /// <summary>
        /// 流转来源方向，退回？交？
        /// </summary>
        public string ProcessDirection { get; set; }

        /// <summary>
        /// 拥有处置权限的站点ID
        /// </summary>
        public string ProcessSiteID { get; set; }

        /// <summary>
        /// 存放文章流转历史数据
        /// </summary>
        public string FlowXml { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 共享来源，本站的的不显示
        /// </summary>
        public string ShareSource
        {
            get
            {
                if (Source == SiteConfigs.GetConfig().SiteName)
                    return string.Empty;
                else
                    return Source;
            }
        }
    }
}
