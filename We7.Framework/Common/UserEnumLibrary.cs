using System;
using System.Collections.Generic;
using System.Text;

namespace We7
{
    /// <summary>
    /// 用于状态管理的枚举类库
    /// 请将处理的业务，及相关
    /// 的状态放置于此。
    /// </summary>
    [Serializable]
    public static class EnumLibrary
    {
        /// <summary>
        /// 状态字段总长
        /// </summary>
        public const int StateLength = 20;

        /// <summary>
        /// 状态位长度，即每个状态所占字串长度
        /// </summary>
        public const int PlaceLenth = 2;

        /// <summary>
        /// 请依次对应于业务枚举进行填写
        /// 状态在State字段所处位置，从左至右依次为0,2,4,6,8,…
        /// </summary>
        public static int[] Position ={ 2, 0, 0, 0, 0, 0, 2, 0, 4, 2,2,0,2,4,6,8,0,2,4,0,4,6,2,0,2};

        /// <summary>
        /// 所要处理的业务枚举
        /// </summary>
        public enum Business : int
        {
            //CD部分
            ChannelContentType = 0,     //Channel表 栏目内容信息类型
            ProductInfoType = 1,        //Article表 产品信息发布类型
            ProductProviderType = 2,    //Product表 产品供应商类型
            IndustryType = 3,           //Industry表 行业类型
            CompanyBaseInfoType = 4,    //CompanyBaseInfo表 公司推荐类型
            PermissionType = 5,         //EntityPermission表 权限应用类型
            ArticleType = 6,            //Article表 展会信息等发布类型
            ChannelNodeLevel = 8,         //Channel表 当前栏目节点层数
            RecruitingType = 10,        //CompanyBaseInfo表 热门招聘企业推荐
            HomeRecommend = 18,         //Article表 对商铺首页进行推荐
            //ID部分
            SitePartnership = 7,    //SitePartnership表 站点关系类型
            SiteValidateStyle = 9,      //SitePartnership表 站点生效方式
            SiteSyncType = 20,          //SitePartnership表 信息共享方式
            SiteAutoUsering = 21,       //SitePartnership表 自动匹配用户
            //广告位管理
            //AdZoneState=11,           //广告位的状态，0表示禁用，1表示站点申请，2站群表示通过审核
            AdZoneTemplate = 12,        //AdZone表  广告位的模板
            AdZoneType = 13,            //AdZone表  广告位版位类型
            AdZoneShowType = 14,        //AdZone表  广告位显示方式
            AdZoneDefaultSetting = 15,  //AdZone表  广告位设置
            AdvertisementType = 16,     //Advertisement表   广告类型
            //AdvertisementEnumState = 17,//广告状态
            AdPublishState = 19,       //AdPublish表 广告发布状态，0表示禁用，1表示站点申请，2站群表示通过审核
            //反馈部分
            AdviceMode = 22,            //AdviceType表 反馈模式
            AdviceDisplay = 23,         //Advice表 反馈前台显示模式
            AdviceEnum = 24,            //反馈状态

            Others = 99
        }
        #region 广告版位设置参数设置
        public enum AdvertisementType : int
        { 
            AdImage = 1,//图片
            AdFlash,//动画
            AdText, //文本
            AdCode,//代码
            AdPage //页面
        }

        /// <summary>
        /// 广告关联状态
        /// 0表示禁用，1表示站点申请，2站群表示通过审核
        /// 同时费除广告位与广告的状态审核
        /// </summary>
        public enum AdPublishState
        {
            NoUsing = 0, //禁用
            Applying,  //申请
            ApplyPassed //通过审核
        }

        //public enum AdvertisementEnumState : int
        //{
        //    UnUsed = 0, //禁用
        //    ApplyAdvertisement,  //申请
        //    ThroughAdvertisement, //通过审核
        //}

        //public enum AdZoneState : int
        //{
        //    UnUsed = 0, //禁用
        //    ApplyZone,  //申请
        //    ThroughAdZone, //通过审核
        //}

        public enum AdZoneTemplate : int
        {
            ChannelTemplate = 1, //栏目模板
            HomepageTemplate,  //首页模板
            ContentpageTemplate, //文章和展会详细信息模板
            ProductcontentpageTemplate, //产品详细信息模板
            DefaultChannelTemplate,    //栏目页模板
            LoginTemplate,   //登陆页模板
            ErrorTemplate,   //错误页模板
            Others
        }

        public enum AdZoneType : int
        {
            RectangleBanner = 1, //矩形横幅
            ShowWindow,  //弹出窗口
            MoveAdZone, //随屏移动
            FixAdZone, //固定位置
            FloatMoveAdZone, //漂浮移动
            CharacterAdZone, //文字代码
            CoupletAdZone, //对联广告
            Others
        }
        public enum AdZoneShowType : int
        {
            ChanceShow = 1, //按权重随机显示，权重越大显示机会越大
            FirstShow, //显示权重最大的广告
            CircleShow,  //循环显示该广告位的广告
            Others
        }
        public enum AdZoneDefaultSetting : int
        {
            defaultSetting = 1, //默认设置
            CustomerSetting,  //用户设置
            Others
        }
        #endregion


        ///// <summary>
        //// 各业务所对应的状态枚举，
        ///// 其命名为相应的业务枚举名
        ///// </summary>
        #region CD部分
        /// <summary>
        /// 权限应用类型 EntityPermission表
        /// </summary>
        public enum PermissionType : int
        {
            Manager = 0,     //管理权限
            Member,          //会员权限
            Public           //公用权限
        }
        public enum HomeRecommend : int
        {
            DefaultArticle=0,    //默认
            RecommendArticle,    //推荐
            Others
        }

        public enum RecruitingType : int
        {
            UnUsed = 0, //禁用
            Using,  //未推荐
            UsingToRecruiting //应用于推荐
        }
        /// <summary>
        /// 文章内容类型 Channel表
        /// </summary>
        public enum ChannelContentType : int
        {
            Article = 0,  //文章
            Product,   //产品
            Recruitment,   //招聘
            SeekJob,  //求职
            Others
        }
        /// <summary>
        /// 文章内容类型 Article表
        /// </summary>
        public enum ArticleType : int
        {
            Article = 0,  //文章信息
            Product,    //产品信息
            Recruitment,   //招聘
            SeekJob,  //求职
            Others
        }
        /// <summary>
        /// 产品信息发布类型 Article表
        /// </summary>
        public enum ProductInfoType : int
        {
            Defaults= 0,//默认
            Provide ,//供应
            Buy,         //求购
            UrgentBuy,   //工程案
            Product,     //产品
            Others
        }

        /// <summary>
        /// 产品供应商类型 Product表
        /// </summary>
        public enum ProductProviderType : int
        {
            Developer = 0,
            Producer,
            Agent,
            Dealer,
            Others
        }
        /// <summary>
        /// 行业类型 Industry表
        /// </summary>
        public enum IndustryType : int
        {
            UnUsed = 0, //禁用
            Using,  //启用
            UsingToHomepage, //应用于首页
            Others
        }
        /// <summary>
        ///公司推荐类型 CompanyBaseInfo表
        /// </summary>
        public enum CompanyBaseInfoType : int
        {
            UnUsed = 0, //禁用
            Using,  //启用
            UsingToHomepage, //应用于推荐
            UsingToRecruiting, //应用于推荐
            Others
        }

        #endregion

        #region 反馈部分
        /// <summary>
        /// 反馈模式 AdviceType表
        /// </summary>
        public enum AdviceMode : int
        {
            Immediate = 0, //直接办理
            DeliverTo,  //转交办理
            Flow, //上报办理
            Others
        }

        public enum AdviceDisplay : int
        {
            DefaultDisplay = 0,//默认显示方式
            DisplayFront = 1,//前台显示
            UnDisplayFront = 2//前台不显示
        }

        public enum AdviceEnum : int
        {
            OtherHandle =0,//非管理员办理。暂时没有用到。
            AdminHandle = 1//管理员办理
        }

        #endregion

        #region ID部分
        /// <summary>
        /// 站点关系类型：共享或接收
        /// </summary>
        public enum SitePartnership : int
        {
            Sharing = 0,
            Receiving
        }

        /// <summary>
        /// 站点关系生效方式：是否必须接收后
        /// </summary>
        public enum SiteValidateStyle : int
        {
            MustReceived = 0,
            NoMustReceived
        }

        /// <summary>
        /// 站点同步方式：自动/手动
        /// </summary>
        public enum SiteSyncType : int
        {
            ManualSync = 0,
            AutoSync
        }

        /// <summary>
        /// 是否自动匹配用户
        /// 指同步后以何身份发布，是否自动匹配联盟用户身份
        /// </summary>
        public enum SiteAutoUsering : int
        {
            UnMatchingUser = 0,
            MatchingUser
        }
        #endregion

        #region 未涉及数据的结构
        /// <summary>
        ///
        /// </summary>
        public enum UserSearchType : int
        {
            供应信息 = 0,
            求购信息 = 1,
            施工案例 = 2,
            产品信息 = 3,
            公司名录 = 4,
            招聘信息 = 5,
            展会信息 = 6,
            新闻信息 = 7
            
        }
        #endregion
    }
}
