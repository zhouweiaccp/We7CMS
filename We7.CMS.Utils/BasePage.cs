using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;

using We7.CMS.Config;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using System.Xml;
using We7.Framework;
using We7.Framework.Config;
using We7.CMS.Accounts;
using Thinkment.Data;
using We7.CMS.ShopService;
using System.Text;

namespace We7.CMS
{
    /// <summary>
    /// 后台页面基础类
    /// </summary>
    public partial class BasePage : Page
    {
        #region helper reference
        /// <summary>
        /// 权限业务对象
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return HelperFactory.Instance; }
        }

        /// <summary>
        /// 模板业务对象
        /// </summary>
        protected TemplateHelper TemplateHelper
        {
            get { return HelperFactory.GetHelper<TemplateHelper>(); }
        }

        /// <summary>
        /// 用户控件业务对象
        /// </summary>
        protected DataControlHelper DataControlHelper
        {
            get { return HelperFactory.GetHelper<DataControlHelper>(); }
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
        /// 通用信息业务对象
        /// </summary>
        protected SiteSettingHelper CDHelper
        {
            get { return HelperFactory.GetHelper<SiteSettingHelper>(); }
        }

        /// <summary>
        /// 菜单业务对象
        /// </summary>
        protected MenuHelper MenuHelper
        {
            get { return HelperFactory.GetHelper<MenuHelper>(); }
        }


        /// <summary>
        /// 审核信息业务对象
        /// </summary>
        protected ProcessingHelper ProcessHelper
        {
            get { return HelperFactory.GetHelper<ProcessingHelper>(); }
        }

        /// <summary>
        /// 审核历史业务对象
        /// </summary>
        protected ProcessHistoryHelper ProcessHistoryHelper
        {
            get { return HelperFactory.GetHelper<ProcessHistoryHelper>(); }
        }

        /// <summary>
        /// 评论业务对象
        /// </summary>
        protected CommentsHelper CommentsHelper
        {
            get { return HelperFactory.GetHelper<CommentsHelper>(); }
        }

        /// <summary>
        /// 访问统计业务对象
        /// </summary>
        protected StatisticsHelper StatisticsHelper
        {
            get { return HelperFactory.GetHelper<StatisticsHelper>(); }
        }

        /// <summary>
        /// 页面访问统计对象
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
        /// 链接业务对象
        /// </summary>
		//protected LinkHelper LinkHelper
		//{
		//    get { return HelperFactory.GetHelper<LinkHelper>(); }
		//}

        /// <summary>
        /// 信息业务对象
        /// </summary>
        protected MessageHelper MessageHelper
        {
            get { return HelperFactory.GetHelper<MessageHelper>(); }
        }

        /// <summary>
        /// 附件业务对象
        /// </summary>
        protected AttachmentHelper AttachmentHelper
        {
            get { return HelperFactory.GetHelper<AttachmentHelper>(); }
        }

        /// <summary>
        /// 标签业务对象
        /// </summary>
        protected TagsHelper TagsHelper
        {
            get { return HelperFactory.GetHelper<TagsHelper>(); }
        }

        /// <summary>
        /// 版本信息业务对象
        /// </summary>
        protected TemplateVersionHelper TemplateVersionHelper
        {
            get { return HelperFactory.GetHelper<TemplateVersionHelper>(); }
        }

        /// <summary>
        /// 反馈类型业务对象
        /// </summary>
        protected AdviceTypeHelper AdviceTypeHelper
        {
            get { return HelperFactory.GetHelper<AdviceTypeHelper>(); }
        }

        /// <summary>
        /// 反馈业务对象
        /// </summary>
        protected AdviceHelper AdviceHelper
        {
            get { return HelperFactory.GetHelper<AdviceHelper>(); }
        }

        /// <summary>
        /// 反馈回复业务对象
        /// </summary>
        protected AdviceReplyHelper AdviceReplyHelper
        {
            get { return HelperFactory.GetHelper<AdviceReplyHelper>(); }
        }

        /// <summary>
        /// 点击量业务对象
        /// </summary>
        protected ClickRecordHelper ClickRecordHelper
        {
            get { return HelperFactory.GetHelper<ClickRecordHelper>(); }
        }

        /*
        /// <summary>
        /// 问卷调查类别业务对象
        /// </summary>
        protected QuestionnaireTypeHelper QuestionnaireTypeHelper
        {
            get { return HelperFactory.GetHelper<QuestionnaireTypeHelper>(); }
        }

        /// <summary>
        /// 问卷业务对象
        /// </summary>
        protected QuestionnaireHelper QuestionnaireHelper
        {
            get { return HelperFactory.GetHelper<QuestionnaireHelper>(); }
        }

        /// <summary>
        /// 问题业务对象
        /// </summary>
        protected QuestionHelper QuestionHelper
        {
            get { return HelperFactory.GetHelper<QuestionHelper>(); }
        }

        /// <summary>
        /// 问题业务对象
        /// </summary>
        protected OptionHelper OptionHelper
        {
            get { return HelperFactory.GetHelper<OptionHelper>(); }
        }

        /// <summary>
        /// 答卷业务对象
        /// </summary>
        protected AnswerSheetHelper AnswerSheetHelper
        {
            get { return HelperFactory.GetHelper<AnswerSheetHelper>(); }
        }
        */
        #endregion

        #region 基本属性
        private string loginMeth = "";

        public string LoginMeth
        {
            get { return loginMeth; }
            set { loginMeth = value; }
        }

        /// <summary>
        /// 所处应用的虚拟路径
        /// </summary>
        public string AppPath
        {
            get
            {
                if (MasterPageIs == MasterPageMode.User)
                    return "";
                else
                    return "/admin";
            }
        }

        private string humanLoginType;
        /// <summary>
        /// 登陆方式属性
        /// </summary>
        public string HumanLoginType
        {
            get { return humanLoginType; }
            set { humanLoginType = value; }
        }

        /// <summary>
        /// 取得当前Cookie
        /// </summary>
        /// <returns></returns>
        protected HttpCookie GetCookie()
        {
            return Request.Cookies["wethepowerseven"];
        }

        OwnerRank MenuOwner
        {
            get
            {
                if (MasterPageIs == MasterPageMode.User)
                    return OwnerRank.Normal;
                else
                    return OwnerRank.Admin;
            }
        }

        #endregion

        #region 主题风格设置
        /// <summary>
        /// 在页面初始化之前设置masterpage值，改变主题风格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (MasterPageIs != MasterPageMode.None)
            {
                string theme = GeneralConfigs.GetConfig().CMSTheme;
                if (theme == null || theme == "") theme = "classic";

                if (MasterPageIs == MasterPageMode.FullMenu)
                {
                    string url = "~/admin/" + Constants.ThemePath + "/" + theme + "/content.Master";
                    Page.MasterPageFile = url;
                }
                else if (MasterPageIs == MasterPageMode.NoMenu)
                {
                    string url = "~/admin/" + Constants.ThemePath + "/" + theme + "/ContentNoMenu.Master";
                    Page.MasterPageFile = url;
                }
                else if (MasterPageIs == MasterPageMode.User)
                {
                    string url = "~/User/DefaultMaster/content.Master";
                    string masterFile = Path.Combine(TemplateHelper.DefaultTemplateGroupPath, "content.Master");
                    if (File.Exists(masterFile))
                    {
                        url = string.Format("{0}/{1}", Constants.TemplateUrlPath, "content.Master");
                    }
                    Page.MasterPageFile = url;
                }
            }
        }

        /// <summary>
        /// 获取主题风格路径
        /// </summary>
        public string ThemePath
        {
            get
            {
                string theme = GeneralConfigs.GetConfig().CMSTheme;
                if (theme == null || theme == "") theme = "classic";
                return "/admin/" + Constants.ThemePath + "/" + theme;
            }
        }

        /// <summary>
        /// 获取站点名称
        /// </summary>
        public string ThisSiteName
        {
            get
            {
                return SiteConfigs.GetConfig().SiteName;
            }
        }

        /// <summary>
        /// 当前用户ID
        /// </summary>
        protected virtual string AccountID
        {
            get
            {
                return Security.CurrentAccountID;
            }
        }

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
        /// 截取字符串
        /// </summary>
        /// <param name="input">输入字串</param>
        /// <param name="count">最大字数</param>
        /// <param name="omit">省略符</param>
        /// <returns></returns>
        public virtual string GetChopString(string input, int count, string omit)
        {
            string result = input;
            if (input.Length > count)
            {
                result = We7.Framework.Util.Utils.CutString(input, 0, count - omit.Length);
                result += omit;
            }
            return result;
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
        /// 获取免费的模板
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public List<ProductInfo> GetFreeTemplates(int count)
        {
            try
            {
                List<ProductInfo> listProducts = null;
                ProductInfo[] products = ShopService.GetRecommendProductByType(count,"mb",-1);
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
        /// 获取文件尺寸对应的M数
        /// </summary>
        /// <param name="productSize">字节数</param>
        /// <returns></returns>
        public string GetProductFileSize(string productSize)
        {
            string result=string.Empty;
            int sizes;
            int.TryParse(productSize, out sizes);
            if (sizes == 0)
                result = "0";
            if (sizes > 0)
                result = ((double)sizes / (double)1048576).ToString("f2");
            return result + "M";
        }

        /// <summary>
        /// 根据价格字段查询商品是否免费
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsFree(object input)
        {
            int price = 0;
            int.TryParse(input.ToString(), out price);

            return price.Equals(0);
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
        /// 页面加载
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Response.Expires = -1;

                if (IsCheckInstallation)
                {
                    CheckInstallation();
                }
                if (!IsPostBack)
                {
                    if (NeedAnAccount)
                    {
                        CheckSignin();
                    }
                    if (NeedAnPermission)
                    {
                        CheckPermission();
                    }
                    Initialize();
                }
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage), ex);
            }
        }
         
        /// <summary>
        /// 是否检查已安装
        /// </summary>
        protected virtual bool IsCheckInstallation
        {
            get { return true; }
        }

        /// <summary>
        /// 是否需要登录
        /// </summary>
        protected virtual bool NeedAnAccount
        {
            get { return true; }
        }

        /// <summary>
        /// 是否判断用户权限
        /// </summary>
        protected virtual bool NeedAnPermission
        {
            get { return true; }
        }

        /// <summary>
        /// 采用哪一种母版-masterpage
        /// </summary>
        protected virtual MasterPageMode MasterPageIs
        {
            get { return MasterPageMode.FullMenu; }
        }

        /// <summary>
        /// 检测用户是否已经登录
        /// </summary>
        protected virtual void CheckSignin()
        {
            if (!Security.IsAuthenticated())
            {
                Account a = null;
                if (SiteConfigs.GetConfig().SiteGroupEnabled)
                    a = AccountHelper.GetAuthenticatedAccount();
                if (a == null)
                {
                    Response.Redirect(AppPath + "/Signin.aspx?returnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl), false);
                }
            }
        }


        /// <summary>
        /// 检测用户的数据库配置文件是否已经创建
        /// </summary>
        protected virtual void CheckInstallation()
        {
            if (!BaseConfigs.ConfigFileExist())
            {
                Response.Write("您的数据库配置文件尚未生成，看起来数据库尚未建立，您需要建立数据库配置文件或生成数据库。现在开始吗？<a href='/install/index.aspx'><u>现在配置数据库</u></a>");
                Response.End();
            }
        }


        /// <summary>
        /// 检查用户是否有权限访问本页
        /// </summary>
        protected virtual void CheckPermission()
        {
            if (!NeedAnPermission || AccountID == We7Helper.EmptyGUID)
            {
                return;
            }
            string errorPage = Request.Url.Host + ":" + Request.Url.Port.ToString() + AppPath + "/Errors.aspx";
            if (HttpContext.Current.Session["ALLMENUURL"]!=null && HttpContext.Current.Session["ALLMENUURL"].ToString() == errorPage)
                return;

            // 检查权限
           if(!MenuHelper.URLHavePermission(HttpContext.Current,MenuOwner))
           {
                HanldeNoPermission();
            }
        }

        /// <summary>
        /// 如果一个用户没任何权限就跳转到错误页
        /// </summary>
        protected virtual void HanldeNoPermission()
        {
            HttpContext context = HttpContext.Current;
            if (context.Request["iframe"] != null)
                Response.Redirect("/Errors.aspx?t=permission&iframe=1",false);
               // Server.Transfer("/Errors.aspx?t=permission&iframe=1");
            else
                Response.Redirect("/Errors.aspx?t=permission&iframe=1", false);
                //Server.Transfer("/Errors.aspx?t=permission");
        }

        /// <summary>
        /// 处理错误信息
        /// </summary>
        /// <param name="e"></param>
        protected virtual void HandleException(Exception e)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// 登出
        /// </summary>
        protected string SignOut()
        {
            return AccountHelper.SignOut();
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="pages">页面</param>
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
                    ClientScript.RegisterStartupScript(GetType(), Guid.NewGuid().ToString(), "<script>alert('对不起，此演示站点您没有该操作权限！')</script>");
                    return true;
                }
                return false;
            }
        } 

    }
}
