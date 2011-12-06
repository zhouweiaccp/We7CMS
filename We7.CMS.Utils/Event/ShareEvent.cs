using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS
{
    /// <summary>
    /// 共享事件委托实例
    /// </summary>
    public class ShareEventFactory
    {
        private static object lockObject = new object();
        private static ShareEvent instance;
        /// <summary>
        /// 共享事件委托实例
        /// </summary>
        public static ShareEvent Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ShareEvent();
                        }
                    }
                }
                return instance;
            }
        }
    }

    public class ShareEvent
    {
        #region 共享事件接口
        public static event EventHandler<MoreEventArgs> ShareActive;
        /// <summary>
        /// 触发共享数据发送事件
        /// </summary>
        /// <param name="account"></param>
        public virtual void OnShareActive(List<string> list,MoreEventArgs e)
        {

            if (ShareActive != null)
            {
                ShareActive(list, e);
            }
        }

        public static event EventHandler<EventArgs> AutoShareArticles;
        /// <summary>
        /// 触发共享数据发送事件
        /// </summary>
        /// <param name="account"></param>
        public virtual void OnAutoShareArticles(string id)
        {

            if (AutoShareArticles != null)
            {
                AutoShareArticles(id, new EventArgs());
            }
        }


        public static event EventHandler<EventArgs> FetchShareDataCommand;
        /// <summary>
        /// 触发获取共享数据事件
        /// </summary>
        /// <param name="account"></param>
        public virtual void OnFetchShareDataCommand(string channelID)
        {

            if (FetchShareDataCommand != null)
            {
                FetchShareDataCommand(channelID, new EventArgs());
            }
        }

        public static event EventHandler<MoreEventArgs> LoadChannelShareConfig;
        /// <summary>
        /// 加载栏目共享通道配置页面ascx
        /// </summary>
        /// <param name="account"></param>
        public virtual void OnLoadChannelShareConfig(int tab, MoreEventArgs e)
        {

            if (LoadChannelShareConfig != null)
            {
                LoadChannelShareConfig(tab, e);
            }
        }

        public static event EventHandler<MoreEventArgs> GetShareTargetSites;
        /// <summary>
        ///  
        /// </summary>
        /// <param name="account"></param>
        public virtual void OnGetShareTargetSites(string objectID,MoreEventArgs e)
        {

            if (GetShareTargetSites != null)
            {
                GetShareTargetSites(objectID, e);
            }
        }

        public static event EventHandler<MoreEventArgs> FeedBackShareData;
        /// <summary>
        ///  
        /// </summary>
        /// <param name="account"></param>
        public virtual void OnFeedBackShareData(string objectID,MoreEventArgs e)
        {

            if (FeedBackShareData != null)
            {
                FeedBackShareData(objectID, e);
            }
        }

        public static event EventHandler<MoreEventArgs> SubmitSiteShareData;
        /// <summary>
        ///  
        /// </summary>
        /// <param name="account"></param>
        public virtual void OnSubmitSiteShareData(string objectID, MoreEventArgs e)
        {

            if (SubmitSiteShareData != null)
            {
                SubmitSiteShareData(objectID, e);
            }
        }

        public static event EventHandler<MoreEventArgs> GetSharedCommand;
        public virtual void OnGetSharedCommand(string objectID)
        {

            if (GetSharedCommand != null)
            {
                GetSharedCommand(objectID, new MoreEventArgs() );
            }
        }
        public static event EventHandler<MoreEventArgs> PublishSharedCommand;
        public virtual void OnPublishSharedCommand(Object obj)
        {

            if (PublishSharedCommand != null)
            {
                PublishSharedCommand(obj, new MoreEventArgs());
            }
        }

        #endregion



    }

    /// <summary>
    /// 事件参数
    /// </summary>
    public class MoreEventArgs : EventArgs
    {
        private string _message;
        public string FlowXml
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }
        /// <summary>
        /// 返回值
        /// </summary>
        public string ReturnValue { get; set; }

        /// <summary>
        /// 返回对象
        /// </summary>
        public object ReturnObject { get; set; }
    }
}
