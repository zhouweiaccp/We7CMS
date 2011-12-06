using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.CMS.Accounts;
using System.Collections.Generic;
using We7.CMS.Web.Admin;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceListEx :BasePage
    {
        #region 属性字段
        private string adviceTypeID;
        private IAdviceHelper adviceHelper = AdviceFactory.Create();
        private AdviceType adviceType;

        public string TabID
        {
            get { return Request["tab"]; }
        }

        public string TypeID
        {
            get { return Request["typeID"]; }
        }

        List<string> permissions;
        protected List<string> Permisstions
        {
            get
            {
                if (permissions == null)
                {
                    permissions = adviceHelper.GetPermissions(TypeID, Security.CurrentAccountID);
                }
                return permissions;
            }
        }

        public AdviceType AdviceType
        {
            get
            {
                if (adviceType == null)
                {
                    adviceType=adviceHelper.GetAdviceType(TypeID);
                }
                return adviceType;
            }
        }
        #endregion

        /// <summary>
        /// 加载时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Page_Load(object sender, EventArgs e)
        {
            MenuTabLabel.Text = BuildNavString();
            NameLabel.Text = AdviceType.Title;
            SummaryLabel.Text=AdviceType.Description;
        }

        private string BuildNavString()
        {
            string strActive = @"<LI class=TabIn id=tab{0} style='display:{2}'><A>{1}</A> </LI>";
            string strLink = @"<LI class=TabOut id=tab{0}  style='display:{2}'><A  href={3}>{1}</A> </LI>";
            int tab = 0;
            bool hasFirst = false;
            string tabString = "";
            string dispay = "";
            string rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "tab");
            rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "saved");

            if (TabID != null && We7Helper.IsNumber(TabID))
            {
                tab = int.Parse(TabID);
                hasFirst = true;
            }

            if (Permisstions.Contains("Advice.Read") || Permisstions.Contains("Advice.Admin"))
            {
                if (tab == 1 || !hasFirst)
                {
                    tabString += string.Format(strActive, 1, "全部", dispay);
                    AddControl(10);
                    hasFirst = true;
                }
                else
                    tabString += string.Format(strLink, 1, "全部", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));
            }

            if (Permisstions.Contains("Advice.Read") || Permisstions.Contains("Advice.Admin") || Permisstions.Contains("Advice.Accept"))
            {
                if (tab == 2 || !hasFirst)
                {
                    tabString += string.Format(strActive, 2, "待受理", dispay);
                    AddControl(0);
                    hasFirst = true;
                }
                else
                    tabString += string.Format(strLink, 2, "待受理", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "2"));
            }

            if (Permisstions.Contains("Advice.Read") || Permisstions.Contains("Advice.Admin") || Permisstions.Contains("Advice.Handle"))
            {
                if (tab == 3 || !hasFirst)
                {
                    tabString += string.Format(strActive, 3, "待办中", dispay);
                    AddControl(2);
                    hasFirst = true;
                }
                else
                    tabString += string.Format(strLink, 3, "待办中", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "3"));
            }

            if (Permisstions.Contains("Advice.Read") || Permisstions.Contains("Advice.Admin") || Permisstions.Contains("Advice.Accept") || Permisstions.Contains("Advice.Handle"))
            {
                if (tab == 6 || !hasFirst)
                {
                    tabString += string.Format(strActive, 6, "不受理", dispay);
                    AddControl(1);
                    hasFirst = true;
                }
                else
                    tabString += string.Format(strLink, 6, "不受理", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "6"));
            }            

            if (Permisstions.Contains("Advice.Read") || Permisstions.Contains("Advice.Admin") || Permisstions.Contains("Advice.Accept") || Permisstions.Contains("Advice.Handle"))
            {
                if (tab == 5 || !hasFirst)
                {
                    tabString += string.Format(strActive, 5, "已办结", dispay);
                    AddControl(9);
                    hasFirst = true;
                }
                else
                    tabString += string.Format(strLink, 5, "已办结", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "5"));
            }

            if (Permisstions.Contains("Advice.Read") || Permisstions.Contains("Advice.Admin") || Permisstions.Contains("Advice.Accept") || Permisstions.Contains("Advice.Handle"))
            {
                if (tab == 4 || !hasFirst)
                {
                    tabString += string.Format(strActive, 4, "转办记录", dispay);
                    AddControl(3);
                    hasFirst = true;
                }
                else
                    tabString += string.Format(strLink, 4, "转办记录", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "4"));
            }

            return tabString;
        }

        void AddControl(int type)
        {
            AdviceListControlEx ctr = LoadControl("~/Admin/Advice/controls/AdviceListControlEx.ascx") as AdviceListControlEx;
            ctr.ShowType = type;
            ctr.TypeID = TypeID;
            ContentHolder.Controls.Add(ctr);
        }

      



        //private void DataBind()
        //{
        //    IAdviceHelper helper = AdviceFactory.Create();

        //    helper.QueryAdviceCount(adviceTypeID, GetStates());
        //}

        //private int[] GetPermissions()
        //{
        //    if (String.Compare(Security.CurrentAccountID, We7Helper.EmptyGUID, true) == 0)
        //    {
        //        return new int[] {1, 2, 3, 4 };
        //    }
        //    else
        //    {
        //        return adviceHelper.GetPermissions(adviceTypeID, Security.CurrentAccountID);
        //    }
        //}

        //private int[] GetStates()
        //{
        //    int[] permissions = GetStates();
        //    List<int> list = new List<int>(permissions);
        //    if (list.Contains(1) || list.Contains(2))
        //    {
        //        return new int[] {0,1,2,3,9 };
        //    }
        //    else if (list.Contains(4) || list.Contains(3))
        //    {
        //        return new int[] { 0, 2,3};
        //    }
        //    else if (list.Contains(4) && !list.Contains(3))
        //    {
        //        return new int[] { 2,3};
        //    }
        //    else if (list.Contains(3) && !list.Contains(4))
        //    {
        //        return new int[] { 0 };
        //    }
        //    return new int[] { };
        //}
    }
}
