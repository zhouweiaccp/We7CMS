using System;
using System.Text;

namespace We7.CMS.Config
{
    /// <summary>
    /// 整合程序配置信息
    /// </summary>
    [Serializable]
    public class ApplicationInfo
    {
        #region Private fields
        private string _appName;
        private string _appUrl;
        private string _apiKey;
        private string _secret;
        private string _callbackUrl;
        private string _ipAddresses;
        #endregion

        #region Properties
        /// <summary>
        /// 整合程序名称 50字节限制
        /// </summary>
        public string AppName
        {
            get { return _appName; }
            set { _appName = value; }
        }

        /// <summary>
        /// 整合程序Url
        /// </summary>
        public string AppUrl
        {
            get { return _appUrl; }
            set { _appUrl = value; }
        }

        /// <summary>
        /// 整合程序API代码 32位
        /// </summary>
        public string APIKey
        {
            get { return _apiKey; }
            set { _apiKey = value; }
        }

        /// <summary>
        /// 整合程序密钥 32位
        /// </summary>
        public string Secret
        {
            get { return _secret; }
            set { _secret = value; }
        }

        /// <summary>
        /// 登录完成后返回地址 100字节限制
        /// </summary>
        public string CallbackUrl
        {
            get { return _callbackUrl; }
            set { _callbackUrl = value; }
        }

        ///// <summary>
        ///// 同步数据的地址 100字节
        ///// </summary>
        //private string _asyncUrl;
        //public string AsyncUrl
        //{
        //    get { return _asyncUrl; }
        //    set { _asyncUrl = value; }
        //}

        ///// <summary>
        ///// 应用程序类型
        ///// </summary>
        //private int _applicationType;
        //public int ApplicationType
        //{
        //    get { return _applicationType; }
        //    set { _applicationType = value; }
        //}

        /// <summary>
        /// 允许的服务器IP地址 逗号分隔
        /// </summary>
        public string IPAddresses
        {
            get { return _ipAddresses; }
            set { _ipAddresses = value; }
        }
        #endregion
    }
}
