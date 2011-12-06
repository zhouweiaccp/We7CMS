using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using Thinkment.Data;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin.controls
{
    public partial class Advice_Config : BaseUserControl
    {
        /// <summary>
        /// 获取反馈模型ID
        /// </summary>
        public string AdviceTypeID
        {
            get
            {
                if (Request["adviceTypeID"] != null)
                {
                    return We7Helper.FormatToGUID((string)Request["adviceTypeID"]);
                }
                return Request["adviceTypeID"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializePage();
            }

            InitControls();
        }

        /// <summary>
        /// 更新时初始化页面数据
        /// </summary>
        void InitializePage()
        {
            AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
            if (AdviceTypeID != null)
            { 
                //页面赋值
                RemindDaysTxtBox.Text = adviceType.RemindDays.ToString();

                if (adviceType.StateText != "")
                    RadlAdviceType.Items.FindByText(adviceType.StateText.ToString()).Selected = true;
                if (adviceType.FlowSeries > 0)
                    FlowSeriesDropDownList.Items.FindByValue(adviceType.FlowSeries.ToString()).Selected = true;
                if (adviceType.FlowInnerDepart.ToString() == "1")
                    FlowInnerDepartCheckBox.Checked = true;
                switch ((AdviceParticipateMode)adviceType.ParticipateMode)
                {
                    case AdviceParticipateMode.Mail:
                        EmailCheckBox.Checked = true;
                        NoteCheckBox.Checked = false;
                        break;
                    case AdviceParticipateMode.SMS:
                        EmailCheckBox.Checked = false;
                        NoteCheckBox.Checked = true;
                        break;
                    case AdviceParticipateMode.All:
                        EmailCheckBox.Checked = true;
                        NoteCheckBox.Checked = true;
                        break;
                    default :
                        EmailCheckBox.Checked = false;
                        NoteCheckBox.Checked = false;
                        break;
                }

                if (adviceType.UseSystemMail == 1 || adviceType.UseSystemMail ==0)  //是否使用默认邮件地址 0 默认； 1 专用
                    UseSystemMailRadioButtonList.Items.FindByValue(adviceType.UseSystemMail.ToString()).Selected = true;
                if (adviceType.MailSMTPServer != "" && adviceType.MailSMTPServer != null)
                    SMTPServerTextBox.Text = adviceType.MailSMTPServer.ToString();
                if (adviceType.POPServer != "" && adviceType.POPServer != null)
                    SysPopServerTextBox.Text = adviceType.POPServer.ToString();
                if (adviceType.MailAddress != null && adviceType.MailAddress != "")
                    EmailAddressTextBox.Text = adviceType.MailAddress.ToString();
                if (adviceType.MailUser != null && adviceType.MailUser != "")
                    MailUserTextBox.Text = adviceType.MailUser;
                if (adviceType.MailPassword != null && adviceType.MailPassword != "")
                    MailPasswordTextBox.Text = adviceType.MailPassword;
                if (adviceType.SMSUser != null && adviceType.SMSUser != "")
                {
                    string[] noteUser = adviceType.SMSUser.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < noteUser.Length; i++)
                    {
                        NoteCheckBoxList.Items.FindByValue(noteUser[i].ToString()).Selected = true;
                    }
                }
                if (adviceType.MailMode != null && adviceType.MailMode != "")
                {
                    string[] mailMode = adviceType.MailMode.Split(new string[] { "0" }, StringSplitOptions.None);
                    for (int i = 0; i < mailMode.Length; i++)
                    {
                        if (mailMode[i].Trim() != "")
                        {
                            EmailCheckBoxList.Items.FindByValue("0" + mailMode[i].ToString()).Selected = true;
                        }
                    }
                }
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            GetConfigInfo();
        }

        protected void InitControls()
        {
            EmailCheckBox.Attributes["onclick"] = string.Format("onCheckViewByID('{0}','{1}');", EmailCheckBox.ClientID,EmailTable.ClientID);
            NoteCheckBox.Attributes["onclick"] = string.Format("onCheckViewByID('{0}','{1}');", NoteCheckBox.ClientID,NoteTable.ClientID);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "keys", "<script>ViewAdviceType();</script>");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mailInit", string.Format("<script>onCheckViewByID('{0}','{1}');</script>", EmailCheckBox.ClientID, EmailTable.ClientID));
            Page.ClientScript.RegisterStartupScript(this.GetType(), "smsInit", string.Format("<script>onCheckViewByID('{0}','{1}');</script>", NoteCheckBox.ClientID, NoteTable.ClientID));

        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        void GetConfigInfo()
        {
            AdviceParticipateMode type = AdviceParticipateMode.None;
            AdviceType adviceType = new AdviceType();
            adviceType.EnumState = StateMgr.StateProcess(adviceType.EnumState, EnumLibrary.Business.AdviceMode, Convert.ToInt32(RadlAdviceType.SelectedItem.Value.ToString()));
            //adviceType.ToWhichDepartment = Convert.ToInt32((AdviceToWhichDepartment)int.Parse(ToWhichDepartmentRadioButtonList.SelectedItem.Value.ToString()));
            adviceType.FlowSeries = Convert.ToInt32(FlowSeriesDropDownList.SelectedItem.Value);
            if (EmailCheckBox.Checked && NoteCheckBox.Checked == false)
            {
                type =AdviceParticipateMode.Mail;
            }
            else if (NoteCheckBox.Checked && EmailCheckBox.Checked == false)
            {
                type =AdviceParticipateMode.SMS;
            }
            else if (NoteCheckBox.Checked && EmailCheckBox.Checked)
            {
                type = AdviceParticipateMode.All;
            }

            adviceType.ParticipateMode = Convert.ToInt32(type);

            if (EmailCheckBox.Checked) 
            //thehim:仅在邮件参与选中后保存下面数据
            {
                if (FlowInnerDepartCheckBox.Checked)
                {
                    adviceType.FlowInnerDepart = 1;
                }
                else
                {
                    adviceType.FlowInnerDepart = 0;
                }
                string mailMode = "";
                for (int j = 0; j < EmailCheckBoxList.Items.Count; j++)
                {
                    if (EmailCheckBoxList.Items[j].Selected)
                    {
                        mailMode += EmailCheckBoxList.Items[j].Value;
                    }
                }
                adviceType.MailMode = mailMode;
                adviceType.UseSystemMail = Convert.ToInt32(UseSystemMailRadioButtonList.SelectedItem.Value.ToString());
                adviceType.MailSMTPServer = SMTPServerTextBox.Text.ToString();
                adviceType.POPServer = SysPopServerTextBox.Text.ToString();
                adviceType.MailAddress = EmailAddressTextBox.Text.ToString();
                adviceType.MailUser = MailUserTextBox.Text.ToString();
                adviceType.MailPassword = MailPasswordTextBox.Text.ToString();
            }

            if (NoteCheckBox.Checked)
            {
                string noteUser = "";
                for (int i = 0; i < NoteCheckBoxList.Items.Count; i++)
                {
                    if (NoteCheckBoxList.Items[i].Selected)
                    {
                        noteUser += NoteCheckBoxList.Items[i].Value + ",";
                    }
                }
                adviceType.SMSUser = noteUser;
            }
            if (RemindDaysTxtBox.Text.Trim() == "")
            {
                 RemindDaysTxtBox.Text = "0";
            }
            adviceType.RemindDays = Convert.ToInt32(RemindDaysTxtBox.Text.Trim());
            adviceType.ID = AdviceTypeID;
            string[] fields = new string[] { "EnumState", "ToWhichDepartment", "FlowSeries", "ParticipateMode", "FlowInnerDepart", "MailMode", "UseSystemMail", "MailSMTPServer", "POPServer", "MailAddress", "MailUser", "MailPassword", "SMSUser", "RemindDays" };

            AdviceTypeHelper.UpdateAdviceType(adviceType, fields);
            Messages.ShowMessage("模型修改成功！");
        }
    }
}