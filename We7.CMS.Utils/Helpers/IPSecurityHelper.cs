using System;
using System.Collections.Generic;
using System.Text;


using Thinkment.Data;
using System.Web;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS
{
    /// <summary>
    /// IP策略安全处理行为类
    /// </summary>
    [Serializable]
    [Helper("We7.IPSecurityHelper")]
    public class IPSecurityHelper:BaseHelper
    {
        /// <summary>
        /// 构造一个ArticleHelper实例
        /// </summary>
        private ArticleHelper ArticleHelper
        {
            get
            {
                HelperFactory hf = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                return hf.GetHelper<ArticleHelper>();
            }
        }

        /// <summary>
        /// 构造一个ChannelHelper实例
        /// </summary>
        private ChannelHelper ChannelHelper
        {
            get
            {
                HelperFactory hf = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                return hf.GetHelper<ChannelHelper>();
            }
        }

        /// <summary>
        /// 检测IP是否在IP策略设置允许范围内
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="ColumnID">栏目ID</param>
        /// <param name="ArticleID">文章ID</param>
        /// <returns></returns>
        public bool CheckIPStrategy(string ip, string ColumnID, string ArticleID)
        {
            return checkArticleStrategy(ip, ArticleID, ColumnID) == StrategyState.DENY ? false : true;
        }

        /// <summary>
        /// 检测常规IP策略设置
        /// </summary>
        /// <param name="ip">IP</param>
        /// <returns></returns>
        private StrategyState checkCommonStrategy(string ip)
        {
            return StrategyConfigs.Instance.IsAllow(ip, GeneralConfigs.GetConfig().IPStrategy);
        }

        /// <summary>
        /// 检测栏目IP策略设置
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="columnId">栏目ID</param>
        /// <returns></returns>
        private StrategyState checkChannelStrategy(string ip,string columnId)
        {
            StrategyState state = StrategyState.DEFAULT;
            if (!String.IsNullOrEmpty(columnId))
            {
                state = StrategyConfigs.Instance.IsAllow(ip, ChannelHelper.QueryStrategy(columnId));

                if (state == StrategyState.DEFAULT)
                {
                    Channel channel=ChannelHelper.GetChannel(columnId, new string[] { "ParentID" });
                    if(channel!=null)
                        state = checkChannelStrategy(ip, channel.ParentID);
                }                   
            }
            return state;
        }

        /// <summary>
        /// 检测栏目的IP策略
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="columnId">栏目ID</param>
        /// <returns></returns>
        private StrategyState checkChannelStrategyWithCommon(string ip, string columnId)
        {
            StrategyState state=checkChannelStrategy(ip, columnId);
            if (state == StrategyState.DEFAULT)
                state = checkCommonStrategy(ip);
            return state;
        }

        /// <summary>
        /// 检测文章IP策略设置
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="articleId">文章ID</param>
        /// <param name="channelID">栏目ID</param>
        /// <returns></returns>
        private StrategyState checkArticleStrategy(string ip, string articleId,string channelID)
        {
            StrategyState state=StrategyState.DEFAULT;

            if(!String.IsNullOrEmpty(articleId))
                state= StrategyConfigs.Instance.IsAllow(ip, ArticleHelper.QueryStrategy(articleId));
            
            if (state == StrategyState.DEFAULT)
                state = checkChannelStrategyWithCommon(ip, channelID);
            return state;
        }
    }

    public enum IPSecurityType
    {
        /// <summary>
        /// 仅允许
        /// </summary>
        Allow = 0,
        /// <summary>
        /// 禁止
        /// </summary>
        Forbidden = 1,
        Other = 2
    }
}
