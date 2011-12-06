using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core;
using We7.CMS;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Accounts;

namespace We7.Model.UI.Command
{
    public abstract class BaseCommand:ICommand
    {   
        /// <summary>
        /// 业务工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID]; }
        }
        /// <summary>
        /// 栏目业务工厂
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        /// <summary>
        /// 栏目业务工厂
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// 获取用户ID
        /// </summary>
        protected virtual string AccountID
        {
            get{ return Security.CurrentAccountID; }
        }

        /// <summary>
        /// 执行的命令
        /// </summary>
        /// <param name="data">模型参数</param>
        /// <returns>执行命令得到的结果</returns>
        public abstract object Do(PanelContext data);
    }
}
