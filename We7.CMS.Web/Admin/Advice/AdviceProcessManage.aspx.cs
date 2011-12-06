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

using We7.CMS.Controls;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceProcessManage : BasePage
    {

        public string TabID
        {
            get { return Request["tab"]; }
        }

        public string RoleID
        {
            get { return Request["id"]; }
        }

        public string AdviceTypeID
        {
            get
            {
                if (Request["adviceTypeID"] != null)
                {
                    if (We7Helper.IsGUID(Request["adviceTypeID"]))
                    {
                        return Request["adviceTypeID"];
                    }
                    else
                    {
                        return We7Helper.FormatToGUID(Request["adviceTypeID"]);
                    }
                }
                else
                    return "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MenuTabLabel.Text = BuildNavString();
            PagePathLiteral.Text = BuildPagePath();
            BindAdvice();
            NameLabel.Text = GetAdviceTypeNameByID();
        }

        /// <summary>
        /// 构建标签项，完全可以通过控制每步骤的display属性
        /// 进行功能项的控制，此属性完全可以进行如同模块式的管理
        /// 设计页面、Tab名称、Tab显示属性、Tab所加载控件
        /// 三者以控制其相应的显示
        /// </summary>
        /// <returns></returns>
        string BuildNavString()
        {
            string strActive = @"<LI class=TabIn id=tab{0} style='display:{2}'><A>{1}</A> </LI>";
            string strLink = @"<LI class=TabOut id=tab{0}  style='display:{2}'><A  href={3}>{1}</A> </LI>";
            int tab = 1;
            string tabString = "";
            string dispay = "";
            string rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "tab");
            rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "saved");

            if (TabID != null && We7Helper.IsNumber(TabID))
                tab = int.Parse(TabID);

            Role r = AccountHelper.GetRole(RoleID);

            if (tab == 1)
            {
                tabString += string.Format(strActive, 1, "邮件回复", dispay);
                Control ctl = this.LoadControl("../Advice/controls/AdviceReplyEmailControl.ascx");
                ContentHolder.Controls.Add(ctl);
            }
            else
                tabString += string.Format(strLink, 1, "邮件回复", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));

            if (tab == 2)
            {
                tabString += string.Format(strActive, 2, "邮件发送", dispay);
                Control ctl = this.LoadControl("../Advice/controls/AdviceSendEmailControl.ascx");
                ContentHolder.Controls.Add(ctl);
            }
            else
                tabString += string.Format(strLink, 2, "邮件发送", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "2"));

            return tabString;
        }

        /// <summary>
        /// 构建当前位置导航
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "";

            //if (AdviceTypeID != null)
            //{
            //    AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);

            //    if (adviceType != null)
            //    {
                    pos = "开始 > <a >设置</a> >  <a href=\"../Advice/AdviceProcessManage.aspx\" >反馈监控</a>";
            //    }
            //}
            //else
            //{
            //    pos = "开始 > <a >设置</a> >  <a href=\"../Advice/AdviceProcessManage.aspx\" >反馈监控</a> >";
            //}
            return pos;
        }

        void BindAdvice()
        {
            AdviceTypeDropDownList.Items.Clear();
            List<AdviceType> adviceType = AdviceTypeHelper.GetAdviceTypes();
            AdviceTypeDropDownList.Items.Add("=====切换到其他模型=====");
            if (adviceType != null)
            {
                for (int i = 0; i < adviceType.Count; i++)
                {
                    if (adviceType[i].MailMode != "")
                    {
                        string name = adviceType[i].Title;
                        string value = We7Helper.AddParamToUrl(Request.RawUrl, "adviceTypeID", adviceType[i].ID);
                        ListItem item = new ListItem(name, value);
                        AdviceTypeDropDownList.Items.Add(item);
                    }
                }
            }
            AdviceTypeDropDownList.Visible = true;
        }

        string GetAdviceTypeNameByID()
        {
            if (AdviceTypeID != "")
            {
                string title = AdviceTypeHelper.GetAdviceType(AdviceTypeID).Title.ToString();
                return title + "监控管理";
            }
            else
            {
                return "反馈监控管理";
            }
        }
    }
}
