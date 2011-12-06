using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.Framework;
using We7.CMS.Accounts;

namespace We7.CMS.Web.User.Action
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AddMessage : BaseAction
    {
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
        /// 保存到发件箱
        /// </summary>
        public string SavetoOutbox { get; set; }

        /// <summary>
        /// 当前动作
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 权限业务对象
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }
        protected MessageHelper MessageHelper
        {
            get { return HelperFactory.GetHelper<MessageHelper>(); }
        }

        public override void Execute()
        {
            if (Action.ToLower() == "savedraft")
                SaveToDraft();
            else
                SendMessage();
        }

        void SaveToDraft()
        {
            ShortMessage sm = new ShortMessage();
            sm.AccountID = Security.CurrentAccountID;
            sm.Receivers = Receivers;
            sm.Subject = Subject;
            sm.Content = Content;
            sm.SendTime = DateTime.Now;
            sm.State =(int)MessageState.Draft;
            MessageHelper.AddMessage(sm);
        }

        void SendMessage()
        {
            string[] receivers = Receivers.Split(',');
            List<Account> accounts = new List<Account>();
            foreach (string user in receivers)
            {
                Account acc = AccountHelper.GetAccountByLoginName(user);
                if (acc != null)
                {
                    accounts.Add(acc);
                }
                else
                {
                    Message = "收件人“" + user + "”不是合法用户，请检查再试！";
                    return;
                }
            }
            foreach (Account acc in accounts)
            {
                ShortMessage sm = new ShortMessage();
                sm.AccountID = Security.CurrentAccountID;
                sm.ReceiverID = acc.ID;
                sm.Receivers = acc.LoginName;
                sm.Subject = Subject;
                sm.Content = Content;
                sm.SendTime = DateTime.Now;
                sm.State = (int)MessageState.NewMessage;
                MessageHelper.AddMessage(sm);
                if (SavetoOutbox == "1")
                {
                    sm.State = (int)MessageState.Outbox;
                    MessageHelper.AddMessage(sm);
                }
            }

            Message = "短消息已成功发送！";
        }

        //public void ProcessRequest(HttpContext context)
        //{
        //    context.Response.ContentType = "text/plain";
        //    context.Response.Write("Hello World");
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
