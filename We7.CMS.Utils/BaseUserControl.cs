using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.Caching;


using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.Framework;
using We7.CMS.Accounts;
using We7.Framework.Config;
using We7.CMS.ShopService;

namespace We7.CMS
{
    /// <summary>
    /// 后台控件基础类
    /// </summary>
    [Serializable]
    public class BaseUserControl : UserControl
    {
        #region 操作助手

        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        /// <summary>
        /// 站点基本信息业务对象
        /// </summary>
        protected SiteSettingHelper CDHelper
        {
            get { return HelperFactory.GetHelper<SiteSettingHelper>(); }
        }

        /// <summary>
        /// 权限业务对象
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// 栏目业务对象
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// 文章业务对象
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        /// <summary>
        /// 模板业务对象
        /// </summary>
        protected TemplateHelper TemplateHelper
        {
            get { return HelperFactory.GetHelper<TemplateHelper>(); }
        }

        /// <summary>
        /// 评论业务对象
        /// </summary>
        protected CommentsHelper CommentsHelper
        {
            get { return HelperFactory.GetHelper<CommentsHelper>(); }
        }

        /// <summary>
        /// 链接业务对象
        /// </summary>
		//protected LinkHelper LinkHelper
		//{
		//    get { return HelperFactory.GetHelper<LinkHelper>(); }
		//}

        /// <summary>
        /// 统计业务对象
        /// </summary>
        protected StatisticsHelper StatisticsHelper
        {
            get { return HelperFactory.GetHelper<StatisticsHelper>(); }
        }

        /// <summary>
        /// 页面访问业务对象
        /// </summary>
        protected PageVisitorHelper PageVisitorHelper
        {
            get { return HelperFactory.GetHelper<PageVisitorHelper>(); }
        }

        /// <summary>
        /// 日志业务对象
        /// </summary>
        protected LogHelper LogHelper
        {
            get { return HelperFactory.GetHelper<LogHelper>(); }
        }

        /// <summary>
        /// 附件业务对象
        /// </summary>
        protected AttachmentHelper AttachmentHelper
        {
            get { return HelperFactory.GetHelper<AttachmentHelper>(); }
        }

        /// <summary>
        /// 别名业务对象
        /// </summary>
        protected TagsHelper TagsHelper
        {
            get { return HelperFactory.GetHelper<TagsHelper>(); }
        }

        /// <summary>
        /// 反馈业务对象
        /// </summary>
        protected AdviceHelper AdviceHelper
        {
            get { return HelperFactory.GetHelper<AdviceHelper>(); }
        }

        /// <summary>
        /// 审核业务对象
        /// </summary>
        protected ProcessingHelper ArticleProcessHelper
        {
            get { return HelperFactory.GetHelper<ProcessingHelper>(); }
        }

        /// <summary>
        /// 审核历史业务对象
        /// </summary>
        protected ProcessHistoryHelper ArticleProcessHistoryHelper
        {
            get { return HelperFactory.GetHelper<ProcessHistoryHelper>(); }
        }

        /// <summary>
        /// 反馈类型业务对象
        /// </summary>
        protected AdviceTypeHelper AdviceTypeHelper
        {
            get { return HelperFactory.GetHelper<AdviceTypeHelper>(); }
        }

        /// <summary>
        /// 文章索引业务对象
        /// </summary>
        protected ArticleIndexHelper ArticleIndexHelper
        {
            get { return HelperFactory.GetHelper<ArticleIndexHelper>(); }
        }

        /// <summary>
        /// 反馈回复业务对象
        /// </summary>
        protected AdviceReplyHelper AdviceReplyHelper
        {
            get { return HelperFactory.GetHelper<AdviceReplyHelper>(); }
        }

        /*
        /// <summary>
        /// 问卷类别业务对象
        /// </summary>
        protected QuestionnaireTypeHelper QuestionnaireTypeHelper
        {
            get { return HelperFactory.GetHelper<QuestionnaireTypeHelper>(); }
        }
        */


        #endregion

        #region We7插件商城相关
        private ShopService.ShopService _ShopService;
        /// <summary>
        /// 商城Service地址 todo
        /// </summary>
        public ShopService.ShopService ShopService
        {
            get
            {
                if (_ShopService == null)
                {
                    _ShopService = new ShopService.ShopService();
                    _ShopService.Timeout = 10000;                    
                    _ShopService.Url = GeneralConfigs.GetConfig().ShopService.TrimEnd('/').TrimEnd('\\') + "/Plugins/ShopPlugin/ShopService.asmx";
                }
                return _ShopService;
            }
        }

        /// <summary>
        /// 通过Web Service Ping接口测试插件商城接口是否可用
        /// </summary>
        /// <returns>true：可用</returns>
        public virtual bool IsShopServicesCanWork()
        {
            try
            {
                string result = ShopService.Ping();
                return result.Equals("Pong");
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage), ex);
                return false;
            }
        }        

        /// <summary>
        /// 获取推荐店铺
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public List<StoreModel> GetRecommendStore(int count)
        {
            try
            {
                StoreModel[] stores = ShopService.GetRecommendStore(count);
                List<StoreModel> listStores = null;
                if (stores.Length > 0)
                {
                    listStores = new List<StoreModel>(stores);                    
                }
                return listStores;
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage), ex);
                return null;
            }
        }

        /// <summary>
        /// 获取推荐商品
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public List<ProductInfo> GetRecommendProduct(int count)
        {
            try
            {
                List<ProductInfo> listProducts = null;
                ProductInfo[] products = ShopService.GetRecommendProduct(count);
                if (products.Length > 0)
                {
                    listProducts = new List<ProductInfo>(products);
                }
                return listProducts;
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage), ex);
                return null;
            }
        }

        /// <summary>
        /// 获取评级对应的星号字串
        /// </summary>
        /// <param name="str">0-6,颗星</param>
        /// <returns>3星,例：★★★☆☆</returns>
        public virtual string GetLevelString(string str)
        {
            int stars = 0;
            int.TryParse(str, out stars);
            
            int max = 5;
            int nostar = max - stars;
            StringBuilder sb = new StringBuilder();
            sb.Append(new string('★', stars));
            sb.Append(new string('☆', nostar));            

            return sb.ToString();
        }

        /// <summary>
        /// 获取没有html符号的字串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual string GetClearHtml(string input)
        {
            return We7Helper.RemoveHtml(input);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="input">输入字串</param>
        /// <param name="count">最大字数</param>
        /// <param name="omit">省略符</param>
        /// <returns></returns>
        public virtual string GetChopString(string input,int count,string omit)
        {
            string result = input;
            if(input.Length>count){
                result = We7.Framework.Util.Utils.CutString(input, 0, count-omit.Length);
                result += omit;
            }
            return result;
        }

        /// <summary>
        /// 站点是否绑定商城
        /// </summary>
        /// <returns></returns>
        public bool IsSiteBindShop()
        {
            string sln = SiteConfigs.GetConfig().ShopLoginName.Trim();
            if (string.IsNullOrEmpty(sln))
                return false;

            try
            {
                //帐号检验
                SiteConfigInfo si = SiteConfigs.GetConfig();
                string[] states = ShopService.CheckSite(si.ShopLoginName, si.ShopPassword, si.SiteUrl);
                if (states != null && states.Length > 0 && states[0] == "1")
                    return true;
                return false;
            }
            catch(Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage),ex);
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 获取用户ID
        /// </summary>
        protected virtual string AccountID
        {
            get
            {
                return Security.CurrentAccountID;
            }
        }

        /// <summary>
        /// 检测用户是否登录
        /// </summary>
        protected bool IsSignin
        {
            get { return CurrentAccount != null; }
        }

        /// <summary>
        /// 退出时清楚Session
        /// </summary>
        protected void SignOut()
        {
            string result = AccountHelper.SignOut();
        }

        /// <summary>
        /// 所处应用的虚拟路径
        /// </summary>
        public string AppPath
        {
            get
            {
                return "/admin";
            }
        }

        /// <summary>
        /// 主题风格路径
        /// </summary>
        public string ThemePath
        {
            get
            {
                string theme = SiteSettingHelper.Instance.Config.CMSTheme;
                if (theme == null || theme == "") theme = "classic";
                return "/admin/" + Constants.ThemePath + "/" + theme;
            }
        }

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        protected string CurrentAccount
        {
            get { return Security.CurrentAccountID; }
        }

        /// <summary>
        /// 通过用户ID获取用户名称
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        protected string GetAccountName(string accountID)
        {
            if (accountID != null && accountID !="")
            {
                if (accountID == We7Helper.EmptyGUID)
                {
                    return "系统管理员";
                }
                else
                {
                    Account act = AccountHelper.GetAccount(accountID, new string[] { "LoginName", "LastName" });
                    if (act != null)
                    {
                        return !string.IsNullOrEmpty(act.LastName) ? act.LastName : act.LoginName;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="pages">日志所在页面</param>
        /// <param name="content">日志内容</param>
        protected void AddLog(string pages, string content)
        {
            if (CDHelper.Config.IsAddLog)
            {
                LogHelper.WriteLog(AccountID, pages, content, CDHelper.Config.DefaultHomePageTitle);
            }
        }

        /// <summary>
        /// 演示站点提示信息
        /// </summary>
        protected bool DemoSiteMessage
        {
            get
            {
                if (GeneralConfigs.GetConfig().IsDemoSite)
                {                    
                    Page.ClientScript.RegisterStartupScript(GetType(), Guid.NewGuid().ToString(), "<script>alert('对不起，演示站点禁止保存！')</script>");
                    return true;
                }
                return false;
            }
        } 
    }
}
