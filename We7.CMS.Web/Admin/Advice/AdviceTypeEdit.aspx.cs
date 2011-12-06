using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;

using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceTypeEdit : BasePage
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
                tabString += string.Format(strActive, 1, "基本属性", dispay);
                Control ctl = this.LoadControl("../Advice/controls/Advice_Option.ascx");
                ContentHolder.Controls.Add(ctl);
            }
            else
                tabString += string.Format(strLink, 1, "基本属性", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));

            if (AdviceTypeID == null || AdviceTypeID == "")
            {
                return tabString;
            }
            else
            {
                if (tab == 2)
                {
                    tabString += string.Format(strActive, 2, "办理流程", dispay);
                    Control ctl = this.LoadControl("../Advice/controls/Advice_Config.ascx");
                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 2, "办理流程", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "2"));


                if (tab == 3)
                {
                    tabString += string.Format(strActive, 3, "反馈表单", dispay);
                    Control ctl = this.LoadControl("../Advice/controls/Advice_File.ascx");
                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 3, "反馈表单", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "3"));


                if (tab == 4)
                {
                    tabString += string.Format(strActive, 4, "办理权限", dispay);
                    Control ctl = this.LoadControl("../Advice/controls/Advice_Authorize.ascx");
                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 4, "办理权限", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "4"));

                return tabString;
            }
           
        }

        /// <summary>
        /// 构建当前位置导航
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "";

            if (AdviceTypeID != null)
            {
                AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);

                if (adviceType != null)
                {
                    pos = "开始 > <a >设置</a> >  <a href=\"../Advice/AdviceTypes.aspx\" >反馈模型</a> >  <a>编辑模型【"
                        + adviceType.Title + "】</a>";

                    NameLabel.Text = "编辑反馈模型【" + adviceType.Title + "】";
                }              

            }
            else
            {
                pos = "开始 > <a >设置</a> >  <a href=\"../Advice/AdviceTypes.aspx\" >反馈模型</a> >  <a>创建新模型</a>";
            }
            return pos;
        }
    }
}
