using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Xml;
using System.IO;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Framework.Util;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceListControl : BaseUserControl
    {

        #region 属性


        /// <summary>
        /// 获取AdviceTypeID
        /// </summary>
        string AdviceTypeID
        {
            get
            {
                return Request["adviceTypeID"];
            }
        }

        /// <summary>
        /// 转交办理用户ID
        /// </summary>
        string AdviceReplyUserID
        {
            get
            {
                return Request["adviceReplyUserID"];
            }
        }

        string states = "0";
        string State
        {
            get
            {
                if (Request["state"] != null)
                {
                    return Request["state"];
                }
                else
                    return states;
            }
            set { states = value; }
        }

        AdviceQuery query = null;
        AdviceQuery CurrentQuery
        {
            get
            {
                if (query == null)
                {
                    query = CreateQuery();
                }
                return query;
            }
            set
            {
                query = value;
            }
        }

        /// <summary>
        /// 当前过滤条件，反馈内容信息状态
        /// </summary>
        protected AdviceState CurrentState
        {
            get
            {
                AdviceState s = AdviceState.All;
                if (State != null)
                {
                    if (We7Helper.IsNumber(State.ToString()))
                        s = (AdviceState)int.Parse(State.ToString());
                }
                return s;
            }
        }

        private int _resultsPageNumber = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        protected int PageNumber
        {
            get
            {
                if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
                    _resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
                return _resultsPageNumber;
            }
        }



        /// <summary>
        /// 反馈模型名称
        /// </summary>
        void SetTitleName()
        {
            if (AdviceTypeID != null && AdviceTypeID != "")
            {
                AdviceType a = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
                if (a != null) NameLabel.Text = a.Title + "列表";
            }
        }

        #endregion

        #region 初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Expires = -1;

            if (!IsPostBack)
            {
                SetTitleName();
                InitializeButtons();
                AdviceTypeIDS.Text = AdviceTypeID;

                LoadAdvices();
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

                AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
                string stateText = adviceType.StateText;
                if (adviceType.FlowSeries > 0)
                {
                    canCheck = true;
                }
                if (adviceType.ParticipateMode == (int)AdviceParticipateMode.Mail)
                {
                    ReceiveHyperLink.Visible = false;
                }
            }
            else
            {
                canAccept = canAdmin = canCheck = canHandle = true;
            }

            DeleteHyperLink.Visible = canAdmin; //删除
            HastenHyperLink.Visible = canAdmin;//催办
            MustHandleHyperLink.Visible = canAccept || canAdmin; //必须办理
            ReceiveHyperLink.Visible = canAdmin;//接收反馈回复邮件
            DisplayHyperLink.Visible = canAdmin;//前台显示
            UndisplayHyperLink.Visible = canAdmin;//前台不显示
            UnControlDisplayHyperLink.Visible = canAdmin;//前台不控制
        }

        string GetTimeNote(DateTime date)
        {
            if (date > DateTime.Today)
                return date.ToString("今天 HH:mm");
            else if (date > DateTime.Today.AddDays(-1))
                return date.ToString("昨天 HH:mm");
            else
                return date.ToString("yyyy-MM-dd");
        }

        #endregion

        #region 获取列表数据
        public bool HasMustHandle = false;

        /// <summary>
        /// 初始化页面信息
        /// </summary>
        void LoadAdvices()
        {
            AdviceUPager.PageIndex = PageNumber;
            AdviceUPager.ItemCount = AdviceHelper.QueryAdviceCountByAll(CurrentQuery);
            AdviceUPager.UrlFormat = We7Helper.AddParamToUrl(Request.RawUrl.Replace("{", "{{").Replace("}", "}}"), Keys.QRYSTR_PAGEINDEX, "{0}");
            AdviceUPager.PrefixText = "共 " + AdviceUPager.MaxPages + "  页 ·   第 " + AdviceUPager.PageIndex + "  页 · ";

            List<Advice> list = new List<Advice>();
            list = AdviceHelper.GetAdviceByQuery(CurrentQuery, AdviceUPager.Begin - 1, AdviceUPager.Count);
            AdviceType adviceType = new AdviceType();
            foreach (Advice a in list)
            {
                if (a.MustHandle > 1)
                {
                    HasMustHandle = true;
                }
                if (a.TypeID != null && a.TypeID != "")
                {
                    adviceType = AdviceTypeHelper.GetAdviceType(a.TypeID);
                    if (adviceType != null)
                    {
                        a.TypeTitle = adviceType.Title;
                    }
                }
                if (a.UserID != null && a.UserID.Length > 0)
                {
                    a.Name = AccountHelper.GetAccount(a.UserID, new string[] { "LastName" }).LastName;
                }
                if (a.Name == null || a.Name == "")
                {
                    a.Name = "匿名用户";
                }
                a.TimeNote = GetTimeNote(a.CreateDate);
                a.AlertNote = GetAlertNote(a.ToHandleTime, adviceType.RemindDays, a.MustHandle);
            }

            AdviceGridView.DataSource = list;
            AdviceGridView.DataBind();

            BuildStateLinks();//刷新状态统计栏
        }

        private int GetAlertNote(DateTime dateTime, int remindDays, int mustHandle)
        {
            if (dateTime != DateTime.MinValue && dateTime != DateTime.MaxValue)
            {
                TimeSpan c = DateTime.Now.Subtract(dateTime);
                if (c.Days > remindDays && mustHandle == 1)
                    return 1;
                else
                    return 0;
            }
            return 0;
        }

        /// <summary>
        /// 构建按类型/状态过滤的超级链接字符串
        /// </summary>
        /// <returns></returns>
        void BuildStateLinks()
        {
            string rawUrl = We7Helper.RemoveParamFromUrl(Request.RawUrl, Keys.QRYSTR_PAGEINDEX);
            rawUrl = rawUrl.Replace("{", "{{").Replace("}", "}}");
            string links = "<li> <a href='" + We7Helper.RemoveParamFromUrl(rawUrl, "state") + "'   {0} >全部<span class=\"count\">({1})</span></a> |</li>" +
            "<li><a href='" + We7Helper.AddParamToUrl(rawUrl, "state", Convert.ToString((int)AdviceState.WaitAccept)) + "'{2}>待受理<span class=\"count\">({3})</span></a>|</li>" +
            "<li><a href='" + We7Helper.AddParamToUrl(rawUrl, "state", Convert.ToString((int)AdviceState.WaitHandle)) + "'{4}>待办理<span class=\"count\">({5})</span></a>|</li>" +
            "<li><a href='" + We7Helper.AddParamToUrl(rawUrl, "state", Convert.ToString((int)AdviceState.Checking)) + "'{6}>审核中<span class=\"count\">({7})</span></a>|</li>" +
            "<li><a href='" + We7Helper.AddParamToUrl(rawUrl, "state", Convert.ToString((int)AdviceState.Finished)) + "'{8}> 已办结<span class=\"count\">({9})</span></a></li>";

            string css99, css0, css1, css2, css3;
            css99 = css0 = css1 = css2 = css3 = "";
            if (CurrentState == AdviceState.All) css99 = " class=\"current\"";
            if (CurrentState == AdviceState.WaitAccept) css0 = " class=\"current\"";
            if (CurrentState == AdviceState.WaitHandle) css1 = " class=\"current\"";
            if (CurrentState == AdviceState.Checking) css2 = " class=\"current\"";
            if (CurrentState == AdviceState.Finished) css3 = " class=\"current\"";
            links = string.Format(links, css99, _GetAdviceCountByState(AdviceState.All),
                css0, _GetAdviceCountByState(AdviceState.WaitAccept), css1, _GetAdviceCountByState(AdviceState.WaitHandle),
                css2, _GetAdviceCountByState(AdviceState.Checking), css3, _GetAdviceCountByState(AdviceState.Finished));

            StateLiteral.Text = links;
        }

        string _GetAdviceCountByState(AdviceState adviceState)
        {
            CurrentQuery.State = Convert.ToInt32(adviceState);
            int n = AdviceHelper.QueryAdviceCountByAll(CurrentQuery);
            return n.ToString();
        }

        /// <summary>
        /// 获取所选择的所有数据的ID
        /// </summary>
        /// <returns></returns>
        private List<string> GetIDs()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < AdviceGridView.Rows.Count; i++)
            {
                if (((CheckBox)AdviceGridView.Rows[i].FindControl("chkItem")).Checked)
                {
                    list.Add(((Label)(AdviceGridView.Rows[i].FindControl("lblID"))).Text);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取办理人名称
        /// </summary>
        /// <param name="adviceID"></param>
        /// <returns></returns>
        public string GetUserName(string adviceID, string toOtherHandleUserID)
        {
            string userID = "";
            string userName = "";
            string action = " <b>转</b> ";
            if (toOtherHandleUserID.Trim() != "")
                userID = toOtherHandleUserID;
            if (adviceID != "")
            {
                AdviceReply ar = AdviceReplyHelper.GetAdviceReplyByAdviceID(adviceID);
                if (ar != null)
                    userID = ar.UserID;
            }
            if (userID != "" && userID != null)
                userName = AccountHelper.GetAccount(userID, new string[] { "LastName" }).LastName;
            if (!string.IsNullOrEmpty(userName))
                userName = action + userName;
            return userName;
        }

        #endregion

        #region 按钮事件
        /// <summary>
        /// 删除反馈信息事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            List<string> list = GetIDs();
            if (list.Count < 1)
            {
                Messages.ShowMessage("您没有选择任何一条记录！");
                return;
            }
            AdviceHelper.DeleteAdvice(list);
            Messages.ShowMessage(string.Format("您已经成功删除了{0}条反馈信息", list.Count.ToString()));
            //记录日志
            string atContent = string.Format("删除了{0}个留言反馈", list.Count.ToString());
            AddLog("留言反馈管理", atContent);
            LoadAdvices();
        }

        /// <summary>
        /// 催办
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HastenButton_Click(object sender, EventArgs e)
        {
            List<string> adviceIDs = GetIDs();
            int count = 0;
            List<string> adviceIDList = new List<string>();
            foreach (string adviceID in adviceIDs)
            {
                Advice advice = AdviceHelper.GetAdvice(adviceID);
                if (advice.MustHandle == 1)
                {
                    advice.MustHandle = 2;
                    string[] fields = new string[] { "MustHandle" };
                    adviceIDList.Add(adviceID);
                    AdviceHelper.UpdateAdvice(advice, fields);
                    count++;
                }
            }
            CurrentQuery = CreateQuery();
            LoadAdvices();
            Messages.ShowMessage(string.Format("您已经成功将{0}条反馈信息进行了 催促办理。", count.ToString()));
        }

        /// <summary>
        /// 获取邮件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReceiveBtn_Click(object sender, EventArgs e)
        {
            if (AdviceTypeID != null && AdviceTypeID != "")
            { 
                AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
                //try
                //{
                    MailHelper mailHelper = AdviceHelper.GetMailHelper(adviceType);
                    string typeName = typeof(AdviceHelper).ToString();
                    bool delete = false;
                    if (DeleteEmailTextBox.Text == "1")
                    {
                        delete = true;
                    }
                    string stateText = adviceType.StateText;
                    MailResult result = mailHelper.ReceiveMail("We7.CMS.Utils.dll", typeName, "HandleReceiveMail", delete, stateText);
                    LoadAdvices();
                    string errorRoot = "<a href=\"/admin/Advice/AdviceProcessManage.aspx\" >反馈监控管理</a>";
                    string message = "";
                    if (result.Count > 0)
                    {
                        message = "您共获取到了" + result.Count + "封邮件";
                    }
                    else
                    {
                        message = "Sorry,没有邮件可以获取...";
                    }
                    if (result.Success > 0)
                    {
                        message += ",并成功受理了" + result.Success + "条反馈信息。";
                    }
                    else if (result.Count > 0 && result.Success == 0)
                    {
                        message += ",有" + result.Count + "封回复邮件存在错误信息，不可直接对应回复到反馈信息，请到" + errorRoot + " <邮件回复>下进行处理。";
                    }
                    Messages.ShowMessage(message);
                //}
                //catch (Exception ex)
                //{
                //    Messages.ShowMessage("系统繁忙，请稍后在试！错误信息："+ex.Message);
                //}
            }
            else
            {
                Messages.ShowMessage("请先选择反馈模型后再获取回复邮件！");
            }
        }

        /// <summary>
        /// 按反馈标题查询信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void QueryButton_ServerClick(object sender, EventArgs e)
        {
            string searchTitle = SearchTextBox.Text.ToString();
            if (searchTitle != null || searchTitle != "")
            {
                LoadAdvices();
            }
            else
            {
                Messages.ShowMessage("请输入查询标题!");
            }
        }

        /// <summary>
        /// 必须办理回复事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MustHandleButton_Click(object sender, EventArgs e)
        {
            List<string> adviceIDs = GetIDs();
            int count = 0;
            foreach (string adviceID in adviceIDs)
            {
                Advice advice = AdviceHelper.GetAdvice(adviceID);
                advice.MustHandle = 1;
                string[] fields = new string[] { "MustHandle" };
                AdviceHelper.UpdateAdvice(advice, fields);
                count++;
            }
            CurrentQuery = CreateQuery();
            LoadAdvices();
            Messages.ShowMessage(string.Format("您已经成功将{0}条反馈信息追加为 必须回复。", count.ToString()));
        }

        protected void DisplayButton_Click(object sender, EventArgs e)
        {
            List<string> adviceIDs = GetIDs();
            int count = 0;
            foreach (string adviceID in adviceIDs)
            {
                Advice advice = AdviceHelper.GetAdvice(adviceID);
                int state = (int)EnumLibrary.AdviceDisplay.DisplayFront;
                string stateStr = StateMgr.StateProcess(advice.EnumState, EnumLibrary.Business.AdviceDisplay, state);
                advice.EnumState = stateStr;
                advice.IsShow = 1;
                string[] fields = new string[] { "EnumState", "IsShow" };
                AdviceHelper.UpdateAdvice(advice, fields);
                count++;
            }
            CurrentQuery = CreateQuery();
            LoadAdvices();
            Messages.ShowMessage(string.Format("您已经成功将{0}条反馈信息追加为 前台显示。", count.ToString()));
        }

        protected void UndisplayButton_Click(object sender, EventArgs e)
        {
            List<string> adviceIDs = GetIDs();
            int count = 0;
            foreach (string adviceID in adviceIDs)
            {
                Advice advice = AdviceHelper.GetAdvice(adviceID);
                int state = (int)EnumLibrary.AdviceDisplay.UnDisplayFront;
                string stateStr = StateMgr.StateProcess(advice.EnumState, EnumLibrary.Business.AdviceDisplay, state);
                advice.EnumState = stateStr;
                advice.IsShow = 0;
                string[] fields = new string[] { "EnumState", "IsShow" };
                AdviceHelper.UpdateAdvice(advice, fields);
                count++;
            }
            CurrentQuery = CreateQuery();
            LoadAdvices();
            Messages.ShowMessage(string.Format("您已经成功将{0}条反馈信息追加为 前台不显示。", count.ToString()));
        }

        protected void UnControlDisplayButton_Click(object sender, EventArgs e)
        {
            List<string> adviceIDs = GetIDs();
            int count = 0;
            foreach (string adviceID in adviceIDs)
            {
                Advice advice = AdviceHelper.GetAdvice(adviceID);
                int state = (int)EnumLibrary.AdviceDisplay.DefaultDisplay;
                string stateStr = StateMgr.StateProcess(advice.EnumState, EnumLibrary.Business.AdviceDisplay, state);
                advice.EnumState = stateStr;
                string[] fields = new string[] { "EnumState" };
                AdviceHelper.UpdateAdvice(advice, fields);
                count++;
            }
            CurrentQuery = CreateQuery();
            LoadAdvices();
            Messages.ShowMessage(string.Format("您已经成功将{0}条反馈信息追加为 前台显示不控制。", count.ToString()));
        }

        protected void AdviceGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label AlertLabel = (Label)e.Row.FindControl("AlertLabel");
                if (AlertLabel != null)
                {
                    Label NoteLabel = (Label)e.Row.FindControl("NoteLabel");

                    switch (AlertLabel.Text)
                    {
                        case "1":
                            e.Row.CssClass = "alertRow";
                            NoteLabel.Text = "必须回复";
                            break;
                        case "2":
                            Label CreateLabel = (Label)e.Row.FindControl("CreateLabel");
                            
                            
                            if (CreateLabel != null)
                            {
                                DateTime create = Convert.ToDateTime(CreateLabel.Text);
                                int days = AdviceHelper.GetWorkingDays(create);
                                if (days >= 3)
                                {
                                    e.Row.CssClass = "overdueRow";
                                    if (NoteLabel != null)
                                        NoteLabel.Text = "延期";
                                }
                                else
                                {
                                    e.Row.CssClass = "hastenRow";
                                    if (NoteLabel != null)
                                        NoteLabel.Text = "催办";
                                }

                            }
                            break;

                        default:
                            break;
                    }
                }

            }
        }

        public string GetProcessState(string id)
        {
            Advice a = AdviceHelper.GetAdvice(id);
            if (a == null)
            {
                return "";
            }
            Processing ap = ArticleProcessHelper.GetAdviceProcess(a);
            string processText = "";
            if (ap != null && ap.ProcessState != ProcessStates.Finished)
                processText = ap.ProcessDirectionText + ap.ProcessText;
            else
                processText = ap.ProcessText;
            return processText;
        }

        public string GetProcessEable(string id)
        {
            return "true";
        }
        #endregion

        #region 私有方法
        public string GetDisplayStyle(string enumStateString,string mustHandle )
        {
            string display = "";
            try
            {
                EnumLibrary.AdviceDisplay adDisplay = EnumLibrary.AdviceDisplay.DefaultDisplay;
                adDisplay = (EnumLibrary.AdviceDisplay)StateMgr.GetStateValueEnum(enumStateString, EnumLibrary.Business.AdviceDisplay);
                switch (adDisplay)
                {
                    case EnumLibrary.AdviceDisplay.DisplayFront:
                        display = "公开";
                        break;
                    case EnumLibrary.AdviceDisplay.UnDisplayFront:
                        display = "";
                        break;
                    default:
                    case EnumLibrary.AdviceDisplay.DefaultDisplay:
                        display = "";
                        break;
                }

                if (string.IsNullOrEmpty(mustHandle)) mustHandle = "0";
                int i =int.Parse(mustHandle);
                if (i == 1)
                    display += ",必办";
                else if (i == 2)
                    display += ",催办";

                if (display.StartsWith(","))
                    display = display.Remove(0, 1);
            }
            catch
            {
            }
            return display;
        }

        public string GetIcons(object mustHandle)
        {
            int i = (int)mustHandle;
            string img = "";
            if (i > 1)
                img = string.Format("<img src={0} border=0 alt='催办信息' /> ", "/admin/images/warning.gif");
            return img;
        }

        AdviceQuery CreateQuery()
        {
            AdviceQuery tempAQ = new AdviceQuery();
            tempAQ.AdviceTypeID = AdviceTypeID;
            tempAQ.State = (int)CurrentState;
            tempAQ.AccountID = AccountID;
            tempAQ.IsShow = 9999;

            string keyWord = SearchTextBox.Text.Trim();
            string selectedValue = ddlSearchKey.SelectedValue;
            switch (selectedValue)
            {
                case "name":
                    tempAQ.Name = keyWord; break;
                case "phone":
                    tempAQ.Phone = keyWord; break;
                case "fax":
                    tempAQ.Fax = keyWord; break;
                case "address":
                    tempAQ.Address = keyWord; break;
                case "email":
                    tempAQ.Email = keyWord; break;
                case "content":
                    tempAQ.Content = keyWord; break;
                default:
                    tempAQ.Title = keyWord; break;
            }
            
            return tempAQ;
        }

        #endregion

    }
}