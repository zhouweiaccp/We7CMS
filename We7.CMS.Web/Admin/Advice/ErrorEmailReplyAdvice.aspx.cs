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
using Thinkment.Data;
using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin
{
    public partial class ErrorEmailReplyAdvice : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 模型ID
        /// </summary>
        //string Title
        //{
        //    get
        //    {
        //        return Request["title"];
        //    }
        //}
        AdviceQuery query = null;
        AdviceQuery CurrentQuery
        {
            get
            {
                if (query == null)
                {
                    query = new AdviceQuery();
                    //query.AccountID = AccountID;
                    query.Title = SearchTextBox.Text.Trim();
                    query.AdviceTypeID = AdviceTypeDropDownList.SelectedItem.Value;
                    query.IsShow = 9999;
                }
                return query;
            }
        }

                   /// <summary>
        /// 按反馈标题查询信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       protected void SearchButton_Click(object sender, EventArgs e)
        {

            string searchTitle = SearchTextBox.Text.ToString();
            if (searchTitle != null || searchTitle != "")
            {
                LoadAdvices();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAdvice();
                LoadAdvices();
            }
       }
       /// <summary>
       /// 初始化页面信息
       /// </summary>
       void LoadAdvices()
       {
           query = null;
           //CurrentQuery.State = (int)CurrentState;

           AdviceUPager.PageIndex = PageNumber;
           AdviceUPager.ItemCount = AdviceHelper.QueryAdviceCountByAll(CurrentQuery);
           AdviceUPager.UrlFormat = We7Helper.AddParamToUrl(Request.RawUrl.Replace("{", "{{").Replace("}", "}}"), Keys.QRYSTR_PAGEINDEX, "{0}");
           AdviceUPager.PrefixText = "共 " + AdviceUPager.MaxPages + "  页 ·   第 " + AdviceUPager.PageIndex + "  页 · ";

           List<Advice> list = new List<Advice>();
           list = AdviceHelper.GetAdviceByQuery(CurrentQuery, AdviceUPager.Begin - 1, AdviceUPager.Count);
           AdviceType adviceType = new AdviceType();
           foreach (Advice a in list)
           {
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
                   a.Name = AccountHelper.GetAccount(a.UserID, new string[] { "LoginName" }).LoginName;
               }
               if (a.Name == null || a.Name == "")
               {
                   a.Name = "匿名用户";
               }
           }
           AdviceGridView.DataSource = list;
           AdviceGridView.DataBind();
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
       public string GetUserName(string adviceID)
       {
           string userID = "";
           string userName = "";
           if (adviceID != "")
           {
               AdviceReply ar = AdviceReplyHelper.GetAdviceReplyByAdviceID(adviceID);
               if (ar != null)
                   userID = ar.UserID;
           }
           if (userID != "" && userID != null)
           {
               userName = AccountHelper.GetAccount(userID, new string[] { "LoginName" }).LoginName;
           }
           return userName;
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
                       //string value = Helper.AddParamToUrl(Request.RawUrl, "adviceTypeID", adviceType[i].ID);
                       string value = adviceType[i].ID;
                       ListItem item = new ListItem(name, value);
                       AdviceTypeDropDownList.Items.Add(item);
                   }
               }
           }
           AdviceTypeDropDownList.Visible = true;
       }

       protected void AdviceTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
       {
           LoadAdvices();
       }
    }
}
