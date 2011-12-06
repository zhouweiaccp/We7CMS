using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;

using Thinkment.Data;
using We7.CMS.Common;
using System.Xml;
using System.IO;
using We7.CMS.Common.Enum;
using System.Web;
using We7.CMS.Common.PF;
using System.Data;
using We7.Model.Core;
using We7.CMS.Config;

namespace We7.CMS
{
    /// <summary>
    /// 反馈业务类（邮件处理部分）
    /// </summary>
    public partial class AdviceHelper : BaseHelper
    {
        #region 邮件处理流程
        /// <summary>
        /// 发送通知邮件给系统管理员"新反馈邮件通知受理人"
        /// </summary>
        /// <param name="adviceID">咨询投诉ID</param>
        public void SendNotifyMail(string adviceID)
        {
            //得到反馈类型
            Advice advice = GetAdvice(adviceID);
            AdviceType adviceType = AdviceTypeHelper.GetAdviceType(advice.TypeID);
            if (advice != null)
            {
                if (adviceType.MailMode != null && adviceType.MailMode != "")
                {
                    string[] mailMode = adviceType.MailMode.Split(new string[] { "0" }, StringSplitOptions.None);
                    //判断是否需要发送邮件
                    for (int i = 0; i < mailMode.Length; i++)
                    {
                        if (mailMode[i].Trim() != "")
                        {
                            if (Convert.ToInt32(mailMode[i]) == (int)MailMode.MailNotify)
                            {
                                AdviceEmailConfigs adviceEmailConfigs = new AdviceEmailConfigs();
                                AdviceEmailConfigInfo info = adviceEmailConfigs["Accept"];
                                //取得所有受理人
                                string content = "Advice.Accept";
                                List<string> receivers = GetAllReceivers(adviceType.ID, content);
                                //转化为接收人列表
                                string toEmail = GetEmailByAccountID(receivers);
                                MailHelper mailHelper = GetMailHelper(adviceType);
                                string body = BuildSingleMailBody(advice.ID, "", info);
                                string must = "";//必须回复
                                string priority = "Low";//过期提示，优先级别
                                if (advice.MustHandle >= 1)
                                {
                                    must = "<b>此信息为必须回复；</b>";
                                    int days = GetWorkingDays(advice.CreateDate);
                                    if (days >= adviceType.RemindDays)
                                    {
                                        priority = "High";
                                    }
                                }
                                string subject = info.EmailTitle.Replace("{EmailTitle}", advice.Title) + must;
                                //发送邮件
                                mailHelper.Send(toEmail, mailHelper.AdminEmail, subject, body, priority);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 发送通知邮件给办理人（带办理备注）
        /// </summary>
        /// <param name="list">反馈信息ID集合</param>
        /// <param name="replyUserID"></param>
        /// <param name="adviceTypeID"></param>
        /// <param name="remark">办理备注</remark>
        /// <param name="priority">邮件优先级</remark>
        public void SendMailToHandler(List<string> list, string replyUserID, string adviceTypeID, string remark, string priority)
        {
            AdviceType adviceType = AdviceTypeHelper.GetAdviceType(adviceTypeID);
            string tmpMailMode = adviceType.MailMode;
            if (tmpMailMode == null || tmpMailMode == "")
            {
                return;
            }
            string[] mailMode = tmpMailMode.Split(new string[] { "0" }, StringSplitOptions.None);
            for (int i = 0; i < mailMode.Length; i++)
            {
                AdviceEmailConfigs adviceEmailConfigs = new AdviceEmailConfigs();
                AdviceEmailConfigInfo info = adviceEmailConfigs["Handle"];
                //判断是否需要发送邮件
                if (Convert.ToInt32("0" + mailMode[i]) == (int)MailMode.MailHyperLink)
                {
                    //发送链接
                    if (replyUserID.Trim() != "")
                    {
                        string userEmail = AccountHelper.GetAccount(replyUserID, new string[] { "Email", "ID" }).Email;
                        Dictionary<string, string> dictionary = GetAdviceIDAndHtml(list);
                        MailHelper mailHelper = GetMailHelper(adviceType);
                        StringBuilder sbBody = new StringBuilder("");
                        if (!string.IsNullOrEmpty(remark))
                        {
                            sbBody.Append("办理备注：<br/>");
                            sbBody.Append(remark + "<br/>");
                        }
                        sbBody.Append(BuildEmailBody(dictionary, info));
                        string subject = info.EmailTitle.Replace("{EmailTitle}", "数量：" + dictionary.Count.ToString());
                        mailHelper.Send(userEmail, mailHelper.AdminEmail, subject, sbBody.ToString(), priority);
                    }
                }
                else if (Convert.ToInt32("0" + mailMode[i]) == (int)MailMode.HandleByMail)
                {
                    //发送直接办理邮件
                    foreach (string id in list)
                    {
                        SendHandleMail(id, replyUserID, adviceType, remark, priority, info);
                        //等待几秒
                    }
                }
            }
        }

        /// <summary>
        /// 发送通知邮件给办理人（带办理备注和优先级）
        /// </summary>
        /// <param name="id">反馈实体ID</param>
        /// <param name="replyUserID"></param>
        /// <param name="adviceType"></param>
        /// <param name="remark">办理备注</param>
        /// <param name="priority">优先级</param>
        public void SendHandleMail(string id, string replyUserID, AdviceType adviceType, string remark, string priority, AdviceEmailConfigInfo info)
        {
            Advice a = GetAdvice(id); ;
            string userEmail = AccountHelper.GetAccount(replyUserID, new string[] { "Email", "ID" }).Email;
            MailHelper mailHelper = GetMailHelper(adviceType);

            string body = BuildHandleMail(id, remark, info);
            string subject = "";

            string must = "";//必须回复
            //priority = "Low";//过期提示，优先级别
            if (a.MustHandle >= 1)
            {
                must = "此信息为必须回复;";
                int days = GetWorkingDays(a.CreateDate);
                if (days >= adviceType.RemindDays)
                {
                    priority = "High";
                }
                subject = must + " " + info.EmailTitle.Replace("{EmailTitle}", "#" + a.SN.ToString());
            }
            else
                subject = " " + info.EmailTitle.Replace("{EmailTitle}", "#" + a.SN.ToString());
            mailHelper.Send(userEmail, mailHelper.AdminEmail, subject, body, priority);
        }


        /// <summary>
        /// 根据用户ID获取用户邮箱
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <returns></returns>
        string GetEmailByAccountID(List<string> accountID)
        {
            string email = "";
            for (int i = 0; i < accountID.Count; i++)
            {
                Account account = AccountHelper.GetAccount(accountID[i], new string[] { "Email", "ID" });
                if (account != null)
                {
                    email += account.Email + ",";
                }
            }
            return email;
        }

        /// <summary>
        /// 邮件配置信息
        /// </summary>
        /// <param name="adviceType"></param>
        public MailHelper GetMailHelper(AdviceType adviceType)
        {
            MailHelper mailHelper = new MailHelper();

            //取得邮件服务器信息
            if (adviceType.UseSystemMail == 0)
            {
                GeneralConfigInfo ci = GeneralConfigs.GetConfig();
                if (ci != null)
                {
                    mailHelper.SmtpServer = ci.SysMailServer;
                    mailHelper.AdminEmail = ci.SystemMail;
                    mailHelper.UserName = ci.SysMailUser;
                    mailHelper.Password = ci.SysMailPassword;
                    mailHelper.PopServer = ci.SysPopServer;
                }
            }
            else if (adviceType.UseSystemMail == 1)
            {
                mailHelper.SmtpServer = adviceType.MailSMTPServer;
                mailHelper.AdminEmail = adviceType.MailAddress;
                mailHelper.UserName = adviceType.MailUser;
                mailHelper.Password = adviceType.MailPassword;
                mailHelper.PopServer = adviceType.POPServer;
            }
            return mailHelper;
        }

        /// <summary>
        /// 构造带连接邮件信息
        /// </summary>
        /// <param name="couples"></param>
        public string BuildEmailBody(Dictionary<string, string> couples, AdviceEmailConfigInfo info)
        {
            string body = "";
            foreach (KeyValuePair<string, string> c in couples)
            {
                if (c.Key.Trim() != "")
                {
                    body += BuildSingleMailBody(c.Key, c.Value, info);
                }
            }
            return body;
        }

        /// <summary>
        /// 创建一封邮件(带连接进入后台办理)
        /// </summary>
        /// <param name="id">反馈实体ID</param>
        /// <param name="html"></param>
        /// <returns></returns>
        string BuildSingleMailBody(string id, string html, AdviceEmailConfigInfo info)
        {
            Advice a = GetAdvice(id);
            AdviceType adviceType = AdviceTypeHelper.GetAdviceType(a.TypeID);
            string must = "";
            string priority = "";//过期提示，优先级别
            if (a.MustHandle >= 1)
            {
                must = "<b>此信息为必须回复；</b>";
                int days = GetWorkingDays(a.CreateDate);
                if (adviceType != null)
                {
                    if (days >= adviceType.RemindDays)
                    {
                        priority = "<b>此信息已超过" + days + "个工作日还未办理，请即刻办理！</b>";
                    }
                }
            }
            HttpRequest Request = HttpContext.Current.Request;
            string rootUrl = "http://" + Request.Url.Host;
            if (Request.Url.Port != 80)
                rootUrl += ":" + Request.Url.Port.ToString();
            if (string.IsNullOrEmpty(html))
                html = info.EmailContent;
            html = html.Replace("{priority}", priority).Replace("{must}", must).Replace("{SiteUrl}", rootUrl).Replace("{id}", id).Replace("{title}", a.Title).Replace("{DateTime}", DateTime.Now.ToString()).Replace("{SiteFullName}", GeneralConfigs.GetConfig().SiteFullName);
            string handleUrl = rootUrl + "/admin/Advice/AdviceDetail.aspx?from=advice&ID=" + id;
            html = html.Replace("{HandleUrl}", handleUrl);

            return html;
        }

        /// <summary>
        /// 创建一份邮件内容（反馈信息转化为邮件Body，可直接 办理回复邮件）（带办理备注）
        /// </summary>
        /// <param name="id">反馈实体ID</param>
        /// <param name="remark">办理备注</param>       
        /// <returns></returns>
        public string BuildHandleMail(string id, string remark, AdviceEmailConfigInfo info)
        {
            StringBuilder sbHtml = new StringBuilder();

            if (!string.IsNullOrEmpty(remark))
            {
                sbHtml.Append("办理备注：<br/>");
                sbHtml.Append(remark + "<br/>");
            }
            StringBuilder sbContent = new StringBuilder();
            Advice a = GetAdvice(id);
            if (a != null && !String.IsNullOrEmpty(a.ModelName) && !String.IsNullOrEmpty(a.ModelSchema) && !String.IsNullOrEmpty(a.ModelXml))
            {
                DataSet ds = ModelHelper.ReadXml(a.ModelXml, a.ModelSchema);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    row["Object"] = a;

                    We7DataTable we7table = ModelHelper.GetModelInfo(a.ModelName).DataSet.Tables[0];
                    ModelHelper.ExtendDataTable(ds.Tables[0], we7table.Columns);


                    foreach (We7DataColumn col in we7table.Columns)
                    {
                        if (row.Table.Columns.Contains(col.Name) && col.Direction != ParameterDirection.ReturnValue && col.Name != "ID" && col.Name != "IsShow")
                        {
                            sbContent.AppendFormat("{0}：{1}<br />", col.Label, row[col.Name]);
                        }
                    }
                }
            }
            sbHtml.Append(info.EmailContent.Replace("{EmailContent}", sbContent.ToString())).Replace("{SiteFullName}", GeneralConfigs.GetConfig().SiteFullName);
            return sbHtml.ToString();
        }

        /// <summary>
        /// 创建一个留言反馈
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Dictionary<string, string> GetAdviceIDAndHtml(List<string> list)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string id in list)
            {
                AdviceEmailConfigs adviceEmailConfigs = new AdviceEmailConfigs();
                AdviceEmailConfigInfo info = adviceEmailConfigs["HandleNotify"];
                dict.Add(id, BuildHandleMail(id, "", info));
            }
            return dict;
        }

        /// <summary>
        /// 接收一个邮件处理
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="result"></param>
        public void HandleReceiveMail(OpenPOP.MIMEParser.Message msg, MailResult result)
        {
            string subject = msg.Subject.ToString();//回复标题
            string body = (string)msg.MessageBody[msg.MessageBody.Count - 1];
            string replyTime = msg.Date.ToString();
            string rawManage = msg.RawMessage;//完整的邮件信息
            string user = msg.FromEmail;
            int start = subject.IndexOf("#");
            int length = body.IndexOf("=========================================================");
            if (length > -1)
            {
                //body = body.Substring(0, length);//回复内容
            }
            if (!CheckIsGarbageEmail(subject))//如果不是垃圾邮件
            {
                if (start < 0)
                {
                    ErrorEmail(subject, rawManage, user, replyTime, body);
                }
                else
                {
                    string sn = subject.Substring(start);//标题截取关键字
                    bool mailBool = UpdateReplyByEmail(subject, sn, body, replyTime, rawManage, user, result.StateText);
                    if (!mailBool)
                    {
                        ErrorEmail(subject, rawManage, user, replyTime, body);
                    }
                    else
                    {
                        result.Success += 1;
                    }
                }
            }
        }

        /// <summary>
        /// 检测邮件是否是垃圾邮件
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        bool CheckIsGarbageEmail(string subject)
        {
            bool result = false;
            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            if (ci != null && ci.EmailGarbageKey != null)
            {
                string[] keys = ci.EmailGarbageKey.Split(new char[] { ',' });
                foreach (string key in keys)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        if (subject.Contains(key))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据回复邮件更新反馈回复
        /// 此方法仅用于反射环境
        /// </summary>
        /// <param name="subject">标题</param>
        /// <param name="sn">反馈信息流水号</param>
        /// <param name="body">回复内容</param>
        /// <param name="replyTime">回复时间</param>
        /// <returns></returns>
        public bool UpdateReplyByEmail(string subject, string sn, string body, string replyTime, string rawManage, string user)
        {
            int start = sn.IndexOf("#");
            HelperFactory hf = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
            Assistant = hf.Assistant;
            AdviceReplyHelper arh = new AdviceReplyHelper();
            arh.Assistant = hf.Assistant;
            string adviceSN = sn.Remove(start, 1);
            if (adviceSN != "")
            {
                int intAdviceSN = 0;
                if (!int.TryParse(adviceSN, out intAdviceSN))
                {
                    return false;
                }
                Criteria c = new Criteria(CriteriaType.Equals, "SN", adviceSN);
                if (Assistant.Count<Advice>(c) < 0)
                {
                    return false;
                }
                else
                {
                    List<Advice> advice = Assistant.List<Advice>(c, null);
                    for (int i = 0; i < advice.Count; i++)
                    {
                        if (advice != null)
                        {
                            //处理反馈回复信息
                            AdviceReply adviceReply = new AdviceReply();
                            adviceReply.AdviceID = advice[i].ID;
                            adviceReply.Title = subject;
                            adviceReply.Content = body;
                            Account account = AccountHelper.GetAccountByEmail(user);
                            if (account == null)
                            {
                                return true;
                            }
                            adviceReply.UserID = account.ID;
                            adviceReply.MailBody = rawManage;
                            adviceReply.CreateDate = DateTime.Now;
                            adviceReply.Updated = DateTime.Now;

                            //更新反馈信息
                            advice[i].ReplyCount += 1;//增加回复数
                            advice[i].Updated = DateTime.Now;
                            advice[i].ToHandleTime = Convert.ToDateTime(replyTime);
                            advice[i].ToOtherHandleUserID = adviceReply.UserID;
                            advice[i].State = (int)AdviceState.Finished;
                            advice[i].IsRead = 1;

                            //处理反馈进度
                            Processing ap = ProcessingHelper.GetAdviceProcess(advice[i]);
                            if (ap == null)
                            {
                                ap = new Processing();
                                ap.ObjectID = advice[i].ID;
                                ap.CurLayerNO = "1";
                                ap.ProcessAccountID = adviceReply.UserID;
                                ap.ProcessDirection = "1";
                                ap.Remark = "";
                                ap.CreateDate = DateTime.Now;
                            }
                            ap.UpdateDate = DateTime.Now;
                            ap.ProcessAccountID = adviceReply.UserID;
                            //ap.Remark = adviceReply.Content;
                            ap.ProcessDirection = advice[i].ProcessDirection = "1";
                            advice[i].ProcessState = ((int)ProcessStates.Finished).ToString();
                            ap.CurLayerNO = advice[i].ProcessState;
                            OperationAdviceInfo(adviceReply, advice[i], ap);

                            if (advice[i].State == (int)AdviceState.Finished)
                            {
                                AdviceEmailConfigs adviceEmailConfigs = new AdviceEmailConfigs();
                                AdviceEmailConfigInfo info = adviceEmailConfigs["ReplyUser"];
                                SendResultMailToAdvicer(advice[i], adviceReply, null, info);
                            }
                            UpdateAdvice(advice[i], new string[] { "IsRead" });

                        }
                    }
                    return true;
                }
            }
            else
                return false;
        }


        /// <summary>
        /// 根据回复邮件更新反馈回复
        /// 此方法仅用于反射环境
        /// </summary>
        /// <param name="subject">标题</param>
        /// <param name="sn">反馈信息流水号</param>
        /// <param name="body">回复内容</param>
        /// <param name="replyTime">回复时间</param>
        /// <returns></returns>
        public bool UpdateReplyByEmail(string subject, string sn, string body, string replyTime, string rawManage, string user, string stateText)
        {
            int start = sn.IndexOf("#");
            HelperFactory hf = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
            Assistant = hf.Assistant;
            AdviceReplyHelper arh = new AdviceReplyHelper();
            arh.Assistant = hf.Assistant;
            string adviceSN = sn.Remove(start, 1);
            if (adviceSN != "")
            {
                int intAdviceSN = 0;
                if (!int.TryParse(adviceSN, out intAdviceSN))
                {
                    return false;
                }
                Criteria c = new Criteria(CriteriaType.Equals, "SN", adviceSN);
                if (Assistant.Count<Advice>(c) < 0)
                {
                    return false;
                }
                else
                {
                    List<Advice> advice = Assistant.List<Advice>(c, null);
                    for (int i = 0; i < advice.Count; i++)
                    {
                        if (advice != null)
                        {
                            //处理反馈回复信息
                            AdviceReply adviceReply = new AdviceReply();
                            adviceReply.AdviceID = advice[i].ID;
                            adviceReply.Title = subject;
                            adviceReply.Content = body;
                            Account account = AccountHelper.GetAccountByEmail(user);
                            if (account == null)
                            {
                                return true;
                            }
                            adviceReply.UserID = account.ID;
                            adviceReply.MailBody = rawManage;
                            adviceReply.CreateDate = DateTime.Now;
                            adviceReply.Updated = DateTime.Now;

                            //更新反馈信息
                            advice[i].ReplyCount += 1;//增加回复数
                            advice[i].Updated = DateTime.Now;
                            advice[i].ToHandleTime = Convert.ToDateTime(replyTime);
                            advice[i].ToOtherHandleUserID = adviceReply.UserID;
                            if (stateText == "上报办理")
                            {
                                advice[i].State = (int)AdviceState.Checking;
                                advice[i].ProcessState = ((int)AdviceState.Checking).ToString();
                            }
                            else
                            {
                                advice[i].State = (int)AdviceState.Finished;
                                advice[i].ProcessState = ((int)ProcessStates.Finished).ToString();
                            }
                            advice[i].IsRead = 1;

                            //处理反馈进度
                            Processing ap = ProcessingHelper.GetAdviceProcess(advice[i]);
                            if (ap == null)
                            {
                                ap = new Processing();
                                ap.ObjectID = advice[i].ID;
                                ap.CurLayerNO = "1";
                                ap.ProcessAccountID = adviceReply.UserID;
                                ap.ProcessDirection = "1";
                                ap.Remark = "";
                                ap.CreateDate = DateTime.Now;
                            }
                            ap.UpdateDate = DateTime.Now;
                            ap.ProcessAccountID = adviceReply.UserID;
                            ap.ProcessDirection = advice[i].ProcessDirection = "1";

                            #region 孟添加
                            AdviceType adviceType = AdviceTypeHelper.GetAdviceType(advice[i].TypeID);
                            switch (advice[i].State)
                            {
                                case (int)AdviceState.All:
                                    break;
                                case (int)AdviceState.WaitAccept:
                                case (int)AdviceState.WaitHandle:
                                case (int)AdviceState.Finished:
                                    break;
                                case (int)AdviceState.Checking:
                                    int auditLevel = 0;
                                    if (We7Helper.IsNumber(advice[i].ProcessState))
                                        auditLevel = int.Parse(advice[i].ProcessState);
                                    if (auditLevel < 0)
                                    {
                                        auditLevel = 0;
                                    }
                                    auditLevel += 1;
                                    if (auditLevel > adviceType.FlowSeries)
                                    {
                                        advice[i].ProcessState = ((int)AdviceState.Finished).ToString();
                                        advice[i].State = (int)AdviceState.Finished;
                                        advice[i].MustHandle = 0;
                                    }
                                    else
                                    {
                                        advice[i].ProcessState = auditLevel.ToString();
                                    }
                                    break;
                                default:
                                    break;
                            }
                            ap.CurLayerNO = advice[i].ProcessState;
                            #endregion

                            OperationAdviceInfo(adviceReply, advice[i], ap);
                            if (advice[i].State == (int)AdviceState.Finished)
                            {
                                AdviceEmailConfigs adviceEmailConfigs = new AdviceEmailConfigs();
                                AdviceEmailConfigInfo info = adviceEmailConfigs["ReplyUser"];
                                SendResultMailToAdvicer(advice[i], adviceReply, null, info);
                            }
                            if (stateText == "上报办理")
                            {
                                advice[i].State = (int)AdviceState.Checking;
                                AdviceReplyHelper.UpdateReplyByAdviceID(adviceReply, null);
                            }
                            else
                            {
                                advice[i].State = (int)AdviceState.Finished;
                            }
                            UpdateAdvice(advice[i], new string[] { "IsRead", "State", "ProcessState" });

                        }
                    }
                    return true;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// 发送处理结果到提交者（用户）邮箱
        /// </summary>
        /// <param name="advice"></param>
        /// <param name="reply"></param>
        /// <param name="adviceType"></param>
        /// <returns></returns>
        public bool SendResultMailToAdvicer(Advice advice, AdviceReply reply, AdviceType adviceType, AdviceEmailConfigInfo info)
        {
            try
            {
                if (adviceType == null)
                    adviceType = AdviceTypeHelper.GetAdviceType(advice.TypeID);

                string userEmail = advice.Email;
                MailHelper mailHelper = GetMailHelper(adviceType);
                string siteFullName = GeneralConfigs.GetConfig().SiteFullName;
                //string body = "尊敬的{0}先生/女士：您好！<br><br>";
                //body += "我们于{1}接收到您的反馈信息（{2}），有关部门已做出处理，现答复如下：<br><br>";
                //body += "{3}<br><br>";
                //body += "感谢您的参与！<br>";
                //body += "{4}";
                //body = string.Format(body, advice.Name, advice.CreateDate.ToLongDateString(), advice.Title, reply.Content, GeneralConfigs.GetConfig().SiteFullName+ "|" + SiteConfigs.GetConfig().RootUrl);
                info.EmailContent = info.EmailContent.Replace("{UserName}", advice.Name).Replace("{DateTime}", advice.CreateDate.ToLongDateString()).Replace("{Title}", advice.Title).Replace("{EmailContent}", reply.Content).Replace("{SiteFullName}", GeneralConfigs.GetConfig().SiteFullName);
                string subject = info.EmailTitle.Replace("{SiteTitle}", GeneralConfigs.GetConfig().SiteTitle.Trim().Replace("\n", "")).Replace("{AdviceTitle}", advice.Title);
                string priority = "Normal";//过期提示，优先级别

                mailHelper.Send(userEmail, mailHelper.AdminEmail, subject, info.EmailContent, priority);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 错误邮件转存到_data下（*.xml）
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="rawManage"></param>
        void ErrorEmail(string subject, string rawManage, string user, string replyTime, string body)
        {
            try
            {
                if (subject != "")
                {
                    string filePath = HttpContext.Current.Server.MapPath("/_Data/ErrorEmail/");
                    DateTime time = Convert.ToDateTime(replyTime);
                    string fileName = user + time.ToString(".yyyy_MM_dd_HH_mm") + ".xml";
                    string path = Path.Combine(filePath, fileName);

                    //检查是否XML文件存放临时路径存在，如果不存在则进行处理
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    //检查XMLSchema文件是否存在，如果不存在则进行处理
                    if (!File.Exists(subject))
                    {
                        XmlDocument doc = new XmlDocument();
                        //转换字符
                        subject = We7Helper.Base64Encode(subject);
                        rawManage = We7Helper.Base64Encode(rawManage);
                        user = We7Helper.Base64Encode(user);
                        body = We7Helper.Base64Encode(body);

                        string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n"
                            + "<root><infoSubject>" + subject + "</infoSubject><infoBody>" + body + "</infoBody><infoRawManage>"
                            + rawManage + "</infoRawManage><infoUser>" + user + "</infoUser><infoTime>"
                            + replyTime + "</infoTime></root>";
                        doc.LoadXml(xml);
                        doc.Save(path);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
