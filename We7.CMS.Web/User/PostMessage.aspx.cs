using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.User
{
    public partial class PostMessage : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.User;
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public string Receivers { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 回复标题
        /// </summary>
        public string ReSubject { get; set; }
        /// <summary>
        /// 回复信息接受人
        /// </summary>
        public string ReReceiver { get; set; }
        public string ReContent { get; set; }

        /// <summary>
        /// 是否显示已有短消息(已经加了Display)
        /// </summary>
        public string SMDisplay
        {
            get
            {
                return CurrentMessage==null ? "display:none" : "";
            }
        }

        public ShortMessage CurrentMessage
        {
            get
            {
                ShortMessage sm = null;
                if (Request["id"] != null && Request["id"].ToString() != "")
                    sm = MessageHelper.GetMessage(Request["id"].ToString(), null);
                return sm;
            }
        }

       
        #region form Action 提交处理
        /// <summary>
        /// 取得Action传回来的值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>字段的数据值</returns>
        public object Get(string key)
        {
            return BaseAction.Get(key, ActionID);
        }

        /// <summary>
        /// 取得Action传回来的值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">字段名</param>
        /// <returns>字段的数据值</returns>
        public T Get<T>(string key)
        {
            try
            {
                return (T)BaseAction.Get(key, ActionID);
            }
            catch
            {
            }
            return default(T);
        }

        private string actionID;
        /// <summary>
        /// 当前的ActionID
        /// </summary>
        public string ActionID
        {
            set { actionID = value; }
            get
            {
                if (String.IsNullOrEmpty(actionID))
                {
                    actionID = this.ClientID;
                }
                return actionID;
            }
        }

        private int minActionID = 0;
        /// <summary>
        /// 创建新的ActionID
        /// </summary>
        public string CreateActionID()
        {
            ActionID = this.ClientID + minActionID;
            minActionID++;
            return ActionID;
        }

        /// <summary>
        /// 取得当前的Action
        /// </summary>
        /// <typeparam name="T">Action类型</typeparam>
        /// <returns>Action对象</returns>
        public T GetAction<T>()
        {
            return BaseAction.GetAction<T>(ActionID);
        }

        /// <summary>
        /// 是否显示消息(已经加了Display)
        /// </summary>
        public string MessageDisplay
        {
            get
            {
                string message = (Get("Message") ?? "").ToString();
                return String.IsNullOrEmpty(message) ? "display:none" : "";
            }
        }

        public void LoadMessage()
        {
            if (CurrentMessage != null)
            {
                ShortMessage msg=CurrentMessage;
                Subject = msg.Subject;
                Content = msg.Content;
                Receivers = msg.Receivers;
                SendTime = msg.SendTime;
                if (msg.State == (int)MessageState.Draft)
                {
                    ReSubject = Subject;
                    ReReceiver = Receivers;
                    ReContent = Content;
                }
                else
                {
                    ReSubject = "回复：" + Subject;
                    ReReceiver = msg.AccountName;
                }
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadMessage();
            
        }

    }
}
