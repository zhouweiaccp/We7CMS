using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using System.Xml;
using System.IO;
using We7.Framework.Util;
using We7.Model.UI.Panel.system;
using We7.Framework;
using We7.Framework.Config;
using We7.Model.Core;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceDetail : BasePage
    {
        #region 属性

        public string AdviceID
        {
            get
            {
                if (Request["ID"] != null)
                    return Request["ID"];
                return string.Empty;
            }
        }
        public bool IsFromDepartment
        {
            get
            {
                return Request["from"] != null && Request["from"].ToString() == "depart";
            }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }


        Advice advice;
        Advice ThisAdvice
        {
            get
            {
                if (advice == null)
                    advice = AdviceHelper.GetAdvice(AdviceID);
                return advice;
            }
        }

        public string AdviceTypeID
        {
            get
            {
                return ThisAdvice.TypeID;
            }
        }
        private string FileName
        {
            get { return Server.MapPath("~/Config/AdviceTag.xml"); }
        }

        private string XPath
        {
            get { return "/AdviceTag"; }
        }

        protected string adviceTag = "";
        #endregion

        #region 页面初始化信息

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
               
        }

        void Initialize()
        {
            BindReplayList();
            Advice adviceModel = ThisAdvice;
            adviceModel.IsRead = 0;
            AdviceHelper.UpdateAdvice(adviceModel, null);
            if (AdviceTypeID != null && AdviceTypeID != "")
            {
                NameLabel.Text = AdviceTypeHelper.GetAdviceType(AdviceTypeID).Title.ToString() + "详细信息"; ;
            }
            PagePathLiteral.Text = BuildPagePath();
            InitializeButtons();            
            DataBindAdviceTag();
            BindReplyUserList();
        }

        /// <summary>
        /// 绑定办理人
        /// </summary>
        private void BindReplyUserList()
        {
            //获取到所拥有这个权限的用户ID
            string content = "Advice.Handle";
            List<string> accountIDs = AdviceHelper.GetAllReceivers(AdviceTypeID, content);
            List<Account> account = new List<Account>();
            this.ddlToOtherHandleUserID.Items.Clear();
            foreach (string aID in accountIDs)
            {
                if (aID != "")
                {
                    Account a = AccountHelper.GetAccount(aID, new string[] { "ID", "DepartmentID", "LoginName", "LastName" });
                    if (a != null)
                    {
                        Department dp = AccountHelper.GetDepartment(a.DepartmentID, new string[] { "Name" });
                        string text = a.LastName;
                        if (string.IsNullOrEmpty(text)) text = a.LoginName;
                        if (dp != null && !string.IsNullOrEmpty(dp.Name))
                        {
                            text = dp.Name + " - " + text;
                        }
                        ListItem lsTemp = new ListItem(text, a.ID);
                        if (!this.ddlToOtherHandleUserID.Items.Contains(lsTemp))
                            this.ddlToOtherHandleUserID.Items.Add(lsTemp);
                    }
                }
            }
        }
        /// <summary>
        ///  操作按钮状态初始化
        /// </summary>
        private void InitializeButtons()
        {
            bool canAccept = false; //反馈受理
            bool canAdmin = false;//反馈管理
            bool canHandle = false;//反馈办理
            bool canRead = false;//查看
            bool canCheck = false; //审核

            AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
            //模型不为空，并且是用户登陆时验证是否具有权限
            if (AdviceTypeID != null && !We7Helper.IsEmptyID(AccountID))
            {
                List<string> contents = AccountHelper.GetPermissionContents(AccountID, AdviceTypeID);
                canAccept = contents.Contains("Advice.Accept");
                canAdmin = contents.Contains("Advice.Admin");
                canHandle = contents.Contains("Advice.Handle");
                canRead = contents.Contains("Advice.Read");

                //canAccept = AccountHelper.HavePermission(AccountID, AdviceTypeID, "Advice.Accept");
                //canAdmin = AccountHelper.HavePermission(AccountID, AdviceTypeID, "Advice.Admin");
                //canHandle = AccountHelper.HavePermission(AccountID, AdviceTypeID, "Advice.Handle");
                //canRead = AccountHelper.HavePermission(AccountID, AdviceTypeID, "Advice.Read");
                if (adviceType.FlowSeries > 0)
                {
                    canCheck = true;
                }
            }
            else
            {
                canAccept = canAdmin = canCheck = canHandle = true;
            }

            canCheck = canCheck && (ThisAdvice.State == (int)AdviceState.Checking);
            canHandle = canHandle && (ThisAdvice.State == (int)AdviceState.WaitHandle || ThisAdvice.State == (int)AdviceState.WaitAccept && adviceType.StateText == "直接办理");
            canAccept = canAccept && (ThisAdvice.State == (int)AdviceState.WaitAccept);

            TransactHyperLink.Visible = canHandle && (adviceType.StateText != "上报办理");//办理
            ToOtherHyperLink.Visible = (canHandle || canAccept) && (adviceType.StateText != "直接办理");//转办 模型类别判断
            trToOtherHandleUser.Visible = ToOtherHyperLink.Visible;//是否转办
            trHandleRemark.Visible = ToOtherHyperLink.Visible;//转办备注
            trPriority.Visible = ToOtherHyperLink.Visible;//邮件优先级

            AuditReportHyperLink.Visible = canHandle && (adviceType.StateText == "上报办理");//上报审核
            ReportHyperLink.Visible = canCheck;
            chbSendEmail.Visible = canCheck;
            fontSendEmail.Visible = canCheck;
            ReturnHyperLink.Visible = (canHandle || canCheck) && (adviceType.StateText != "直接办理");//退回重办 办理类别
            ReplyContentTextBox.Visible = canHandle || ThisAdvice.State == (int)AdviceState.Checking;
            AdminHandHyperLink.Visible = (canHandle || canAccept) && (ThisAdvice.State != (int)AdviceState.Finished);

            switch (adviceType.StateText)
            {
                case "转交办理":

                    if (canHandle)
                    {
                        ToAdviceTextBox.Visible = true;
                        toAdviceLabel.Text = "转交备注：";
                    }
                    break;
                case "上报办理":
                    switch (ThisAdvice.State)
                    {
                        case (int)AdviceState.Checking:

                            toAdviceLabel.Text = "审核意见：";
                            ToAdviceTextBox.Visible = true;
                            break;

                        case (int)AdviceState.WaitHandle:
                            toAdviceLabel.Visible = false;
                            ToAdviceTextBox.Visible = false;

                            break;

                        case (int)AdviceState.WaitAccept:

                            toAdviceLabel.Visible = false;
                            ToAdviceTextBox.Visible = false;
                            break;
                    }

                    break;
                case "直接办理":
                    ToAdviceTextBox.Visible = false;
                    break;
                default:
                    break;
            }

            AdviceReply reply = AdviceReplyHelper.GetAdviceReplyByAdviceID(AdviceID);
            if (reply != null)
            {
                if (reply.Content != null && reply.Content != "")
                {
                    Account accountModel = AccountHelper.GetAccount(ThisAdvice.ToOtherHandleUserID, new string[] { "LastName", "DepartmentID" });
                    string departmentAndUser = "";
                    if (accountModel != null)
                    {
                        Department dp = AccountHelper.GetDepartment(accountModel.DepartmentID, new string[] { "Name" });
                        if (dp != null && !string.IsNullOrEmpty(dp.Name))
                            departmentAndUser = "<p>" + dp.Name + " - " + accountModel.LastName;
                        else
                            departmentAndUser = "<p>" + accountModel.LastName;
                    }

                    if (ReplyContentTextBox.Visible)
                        ReplyContentTextBox.Value = We7Helper.ConvertPageBreakFromCharToVisual(reply.Content);
                    else
                    {
                        replyDiv.InnerHtml = We7Helper.ConvertPageBreakFromCharToVisual(reply.Content) + departmentAndUser;
                    }
                }
            }
        }

        /// <summary>
        /// 构建当前位置导航
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "";

            if (AdviceID != null)
            {
                if (ThisAdvice != null)
                {
                    pos = "开始 > <a >设置</a> >  <a href=\"../Advice/AdviceList.aspx?AdviceTypeID=" + AdviceTypeID + "\" >反馈列表</a> >  <a>【"
                        + advice.Title + "】反馈详细信息</a>";
                }
            }
            else
            {
                pos = "开始 > <a >设置</a> >  <a href=\"../Advice/AdviceList.aspx?AdviceTypeID=" + AdviceTypeID + "\" >反馈列表</a> >  <a>反馈详细信息</a>";
            }
            return pos;
        }

        protected void Pager_Fired(object sender, EventArgs e)
        {
            BindReplayList();
        }

        void BindReplayList()
        {
            if (ThisAdvice != null)
            {
                TitleLabel.Text = "信息" + ThisAdvice.StateText + "：" + "【" + ThisAdvice.Title + "】";
                SimpleEditorPanel uc = this.LoadControl("/ModelUI/Panel/System/SimpleEditorPanel.ascx") as SimpleEditorPanel;
                uc.PanelName = "adminView";
                uc.ModelName = ThisAdvice.ModelName;
                uc.PanelContext.CtrVersion = CtrVersion.V26;
                uc.IsViewer = true;
                ModelDetails.Controls.Add(uc);
            }
        }


        string GetParentNodeAccountName(ProcessHistory[] ph, int j, string curLayerNO)
        {
            string name = "";
            for (int i = j - 1; i >= 0; i--)
            {
                string parentLayerNO = "";
                if (curLayerNO == "1")
                {
                    parentLayerNO = "-3";
                }
                else if (curLayerNO == "-3")
                {
                    parentLayerNO = "-1";
                }
                else
                {
                    parentLayerNO = (Convert.ToInt16(curLayerNO) - 1).ToString();
                }

                if (ph[i].ToProcessState == parentLayerNO)
                {
                    name = GetAccountName(ph[i].ProcessAccountID);
                    break;
                }
            }

            if (name == null || name == "") name = "未知用户";
            return name;
        }

        string GetAccountLoginName(string accountID)
        {
            string name;
            if (We7Helper.IsEmptyID(accountID))
                name = "Administration";
            else
                name = AccountHelper.GetAccount(accountID,new string[]{"LoginName"}).LoginName;
            if (name == null || name == "") name = "user";
            return name;
        }

        string GetAccountName(string accountID)
        {
            string name;
            if (We7Helper.IsEmptyID(accountID))
                name = "管理员";
            else
                name = AccountHelper.GetAccount(accountID, new string[] { "LastName" }).LastName;
            if (name == null || name == "") name = "未知用户";
            return name;
        }

        public string GetRemark(string adviceID, string id)
        {
            string remark = ProcessHistoryHelper.GetAdviceProcessHistory(adviceID, id).Remark;
            if (remark != "") remark = "，并发表意见：" + remark;
            return remark;
        }
        /// <summary>
        /// 绑定反馈类别
        /// </summary>
        private void DataBindAdviceTag()
        {
            AdviceState ast = (AdviceState)System.Enum.Parse(typeof(AdviceState), ThisAdvice.State.ToString());
            if (ast == AdviceState.WaitAccept)
            {
                XmlNode adviceTagNodes = XmlHelper.GetXmlNode(FileName, XPath);
                Dictionary<string, string> tagList = new Dictionary<string, string>();
                foreach (XmlNode node in adviceTagNodes)
                {
                    tagList.Add(node.Attributes["name"].Value, node.Attributes["name"].Value);
                }
                ddlAdviceTag.DataSource = tagList;
                ddlAdviceTag.DataTextField = "key";
                ddlAdviceTag.DataValueField = "value";
                ddlAdviceTag.DataBind();
                ddlAdviceTag.Attributes.Add("onchange", "SelectedAdviceTag('" + ddlAdviceTag.ClientID + "')");
            }
            else
            {
                ddlAdviceTag.Visible = false; 
                //newTag.Visible = false;
                lbAdviceTag.Text = ThisAdvice.AdviceTag;
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 办理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TransactButton_Click(object sender, EventArgs e)
        {
            UserIDTextBox.Text = "";
            if (OperationInfo(AdviceState.Finished, "1", true))
            {
                Messages.ShowMessage(" :) 办理成功！");
                //记录日志
                string content = string.Format("办理了留言反馈:“{0}”的详细信息", AdviceID);
                AddLog("留言反馈详细信息", content);
                BindReplayList();
            }
        }

        /// <summary>
        /// 转办
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToOtherButton_Click(object sender, EventArgs e)
        {
            UpdateAdviceTag();
            if (OperationInfo(AdviceState.WaitHandle))
            {
                AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
                if (adviceType != null)
                {
                    if (adviceType.ParticipateMode == (int)AdviceParticipateMode.Mail || adviceType.ParticipateMode == (int)AdviceParticipateMode.All)
                    {
                        List<string> contents = AccountHelper.GetPermissionContents(AccountID, AdviceTypeID);
                        if (contents.Contains("Advice.Admin") || We7Helper.IsEmptyID(AccountID))
                        {
                            ToOtherReplyUser();
                        }
                    }
                }
                Messages.ShowMessage(" :) 转办成功！");
                //记录日志
                string content = string.Format("转办了留言反馈:“{0}”的详细信息", AdviceID);
                AddLog("留言反馈详细信息", content);
                BindReplayList();
                actionTable.Visible = false;
            }
        }

        /// <summary>
        /// 上报审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AuditReportButton_Click(object sender, EventArgs e)
        {
            UserIDTextBox.Text = "";
            Advice a = AdviceHelper.GetAdvice(AdviceID);
            bool success = false;
            if (a.State == (int)AdviceState.WaitHandle)
            {
                success = OperationInfo(AdviceState.Checking,"1", true);
            }
            else
            {
                success = OperationInfo(AdviceState.Checking);
            }

            if (success)
            {
                Messages.ShowMessage(" :) 上报审核成功！");
                //记录日志
                string content = string.Format("上报审核了留言反馈:“{0}”的详细信息", AdviceID);
                AddLog("留言反馈详细信息", content);
            }

            Initialize();
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReportButton_Click(object sender, EventArgs e)
        {
            if (OperationInfo(AdviceState.Checking))
            {
                Advice a = ThisAdvice;                
                AdviceReply reply = AdviceReplyHelper.GetAdviceReplyByAdviceID(AdviceID);
                if (reply == null)
                {
                    reply = new AdviceReply();
                    reply.AdviceID = AdviceID;                    
                    reply.Suggest = ToAdviceTextBox.Text;
                    reply.UserID = AccountID;
                    reply.CreateDate = DateTime.Now;
                    reply.Updated = DateTime.Now;
                }
                reply.Content = We7Helper.ConvertPageBreakFromVisualToChar(ReplyContentTextBox.Value);
                AdviceReplyHelper.UpdateReplyByAdviceID(reply, null);
                if (a.State == (int)AdviceState.Finished)
                {
                    if (chbSendEmail.Checked)
                    {
                        AdviceEmailConfigs adviceEmailConfigs = new AdviceEmailConfigs();
                        AdviceEmailConfigInfo info = adviceEmailConfigs["ReplyUser"];
                        AdviceHelper.SendResultMailToAdvicer(a, reply, null, info);
                    }
                }
                Messages.ShowMessage(" :) 审核成功！");
                //记录日志
                string content = string.Format("审核通过了留言反馈:“{0}”的详细信息", AdviceID);
                AddLog("留言反馈详细信息", content);
                Response.Write("<script>alert('审核成功！');location.href='AdviceList.aspx?adviceTypeID=" + ThisAdvice.TypeID + "';</script>");                
                //Initialize();
            }
        }

        /// <summary>
        /// 退回重办
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReturnButton_Click(object sender, EventArgs e)
        {
            string action = "";
            bool ret = false;
            if (ThisAdvice.State == (int)AdviceState.WaitHandle)
            {
                action = "重发";
                ret = OperationInfo(AdviceState.WaitAccept, "0", false);
            }
            else if (ThisAdvice.State == (int)AdviceState.Checking)
            {
                action = "重办";
                ret = OperationInfo(AdviceState.WaitHandle, "0", false);
            }
            if (ret)
            {
                Messages.ShowMessage("已成功退回" + action + "！");
                //记录日志
                string content = string.Format("退回" + action + "了留言反馈:“{0}”的详细信息", AdviceID);
                AddLog("留言反馈详细信息", content);
            }
            Initialize();
        }

        /// <summary>
        /// 管理员标记为已处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdminHandButton_Click(object sender, EventArgs e)
        {
            Advice advice = new Advice();
            advice.EnumState = StateMgr.StateProcess(advice.EnumState, EnumLibrary.Business.AdviceEnum, (int)EnumLibrary.AdviceEnum.AdminHandle);
            advice.State = (int)AdviceState.Finished;
            advice.Updated = DateTime.Now;
            advice.ID = AdviceID;
            advice.AdviceTag = ddlAdviceTag.SelectedItem.Value;
            advice.ProcessState = ((int)ProcessStates.Finished).ToString();
            string[] fields = new string[] { "ID", "State", "EnumState", "Updated", "AdviceTag", "ProcessState" };
            AdviceHelper.UpdateAdvice(advice, fields);
            actionTable.Visible = false;
            Messages.ShowMessage("反馈已标记为处理。");
        }

        protected bool OperationInfo(AdviceState state)
        {
            return OperationInfo(state, "1", false);
        }
        /// <summary>
        /// 操作信息
        /// </summary>
        /// <returns></returns>
        protected bool OperationInfo(AdviceState state, string direction, bool saveReply)
        {
            try
            {
                AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
                Advice a = ThisAdvice;

                //处理反馈回复信息
                AdviceReply adviceReply = null;
                if (saveReply)
                {
                    adviceReply = new AdviceReply();
                    adviceReply.AdviceID = AdviceID;
                    adviceReply.Content = We7Helper.ConvertPageBreakFromVisualToChar(ReplyContentTextBox.Value);
                    adviceReply.Suggest = ToAdviceTextBox.Text;
                    adviceReply.UserID = AccountID;
                    adviceReply.CreateDate = DateTime.Now;
                    adviceReply.Updated = DateTime.Now;

                    //增加回复数
                    a.ReplyCount += 1;
                }

                //更新反馈信息
                a.Updated = DateTime.Now;
                a.ToHandleTime = DateTime.Now;
                if (UserIDTextBox.Text.Trim() != "")                    
                    a.ToOtherHandleUserID = UserIDTextBox.Text.ToString();
                else
                    a.ToOtherHandleUserID = AccountID;
                a.State = (int)state;

                //处理反馈进度
                Advice oldAdvice = AdviceHelper.GetAdvice(AdviceID);
                Processing ap = ProcessHelper.GetAdviceProcess(oldAdvice);
                ap.UpdateDate = DateTime.Now;
                ap.ProcessAccountID = AccountID;
                ap.ApproveName = AccountHelper.GetAccount(AccountID,new string[]{"LastName"}).LastName;
                ap.ProcessDirection = direction.ToString();
                ap.Remark = ToAdviceTextBox.Text;
                if (state == AdviceState.WaitHandle)
                {
                    a.ProcessState = ((int)state).ToString();
                    string myText = "请 {0} 办理一下反馈“{1}”。";
                    string userName = AccountHelper.GetAccount(UserIDTextBox.Text, new string[] { "LastName" }).LastName;
                    ap.Remark = string.Format(myText, userName, a.Title) + "<br>" + ap.Remark;
                }
                switch (state)
                {
                    case AdviceState.All:
                        break;
                    case AdviceState.WaitAccept:
                    case AdviceState.WaitHandle:
                    case AdviceState.Finished:
                        break;
                    case AdviceState.Checking:
                        int auditLevel = 0;
                        if (We7Helper.IsNumber(a.ProcessState))
                            auditLevel = int.Parse(a.ProcessState);
                        if (auditLevel < 0)
                        {
                            auditLevel = 0;
                        }
                        auditLevel += 1;
                        if (auditLevel > adviceType.FlowSeries)
                        {
                            a.ProcessState = ((int)AdviceState.Finished).ToString();
                            a.State = (int)AdviceState.Finished;
                            a.MustHandle = 0;
                        }
                        else
                        {
                            a.ProcessState = auditLevel.ToString();
                        }
                        break;
                    default:
                        break;
                }
                ap.CurLayerNO = a.ProcessState;
                ap.AdviceState = (AdviceState)a.State;

                AdviceHelper.OperationAdviceInfo(adviceReply, oldAdvice, ap);
                if (state == AdviceState.WaitHandle)
                    AdviceHelper.UpdateAdvice(a, new string[] { "ToHandleTime", "ToOtherHandleUserID" });

                if (state == AdviceState.Finished)
                {
                    AdviceEmailConfigs adviceEmailConfigs = new AdviceEmailConfigs();
                    AdviceEmailConfigInfo info = adviceEmailConfigs["ReplyUser"];
                    AdviceHelper.SendResultMailToAdvicer(a, adviceReply, adviceType,info);
                }
                return true;
            }
            catch (Exception ex)
            {
                Messages.ShowError(" 信息操作失败！原因：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 邮件转交操作
        /// </summary>
        public void ToOtherReplyUser()
        {
            AdviceReply adviceReply = new AdviceReply();
            //adviceReply.UserID = UserIDTextBox.Text;
            adviceReply.UserID = ddlToOtherHandleUserID.SelectedValue;

            if (AdviceID.Trim() != "")
            {
                adviceReply.AdviceID = AdviceID;
                AdviceReply ar = AdviceReplyHelper.GetAdviceReplyByAdviceID(AdviceID);
                if (ar == null)
                {
                    AdviceReplyHelper.AddAdviceReply(adviceReply);
                }
            }
            List<string> list = new List<string>();
            list.Add(AdviceID);
            //发送邮件给办理人
            AdviceHelper.SendMailToHandler(list, adviceReply.UserID, AdviceTypeID, txtRemark.Text,rblPriority.SelectedValue);
        }

        #endregion



        /// <summary>
        /// 更新反馈标签；
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateAdviceTag()
        {
          
            if(this.ddlAdviceTag.Items.Count <1)
            {
                DataBindAdviceTag();
            }
            string adviceTag = txtAdviceTag.Text.Trim();
            if (string.IsNullOrEmpty(adviceTag) && this.ddlAdviceTag.Items.Count > 0)
            {
                adviceTag = this.ddlAdviceTag.Items[0].Value;                
            }
            else if (adviceTag == "noTag" && this.ddlAdviceTag.Items.Count > 0)
            {
                adviceTag = this.ddlAdviceTag.Items[0].Value;
            }
            Advice advice = new Advice();
            advice.ID = AdviceID;
            advice.AdviceTag = adviceTag;
            string[] fields = new string[] { "ID", "Updated", "AdviceTag" };
            AdviceHelper.UpdateAdvice(advice, fields);
        }


    }
}
