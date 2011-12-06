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
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;
using System.Xml;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceReplyUserList : BasePage
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
        string AdviceTypeID
        {
            get
            {
                return Request["adviceTypeID"];
            }
        }

        /// <summary>
        /// 反馈信息ID
        /// </summary>
        string AdviceID
        {
            get
            {
                return Request["adviceID"];
            }
        }
        /// <summary>
        /// 反馈标签
        /// </summary>
        string AdviceTag
        {
            get { return HttpUtility.UrlDecode(Request["adviceTag"]); }
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


        protected void SearchButton_Click(object sender, EventArgs e)
        {
            Initialize();

        }

        protected override void Initialize()
        {
            //获取到所拥有这个权限的用户ID
            string content = "Advice.Handle";
            List<string> accountIDs = AdviceHelper.GetAllReceivers(AdviceTypeID, content);
            List<Account> account = new List<Account>();
            foreach (string aID in accountIDs)
            {
                if (aID != "")
                {
                    account.Add(AccountHelper.GetAccount(aID, new string[] { "LoginName", "DepartmentID" }));
                }
            }
            if (SearchTextBox.Text.Trim() != "" && SearchTextBox.Text != null)
            {
                List<Account> at = new List<Account>();
                for (int i = 0; i < account.Count; i++)
                {
                    if (account[i].LoginName == SearchTextBox.Text)
                    {
                        at.Add(account[i]);
                    }
                }
                DetailGridView.DataSource = at;
            }
            else
            {
                DetailGridView.DataSource = account;
            }
            DetailGridView.DataBind();
            UpdateAdviceTag();

        }


        /// <summary>
        /// 更新反馈标签；
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateAdviceTag()
        {

            if (!string.IsNullOrEmpty(AdviceTag))
            {
                Advice advice = new Advice();
                advice.ID = AdviceID;
                advice.AdviceTag = AdviceTag;
                string[] fields = new string[] { "ID", "Updated", "AdviceTag" };
                AdviceHelper.UpdateAdvice(advice, fields);
            }
        }
        /// <summary>
        ///  判断出所有用户归属的部门
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetDeptNameByID(string userDepartmentID)
        {
            string userDepartment = "";
            if (userDepartmentID != "" && userDepartmentID != We7Helper.EmptyGUID)
            {
                Department department = AccountHelper.GetDepartment(userDepartmentID, null);
                if (department != null)
                {
                    userDepartment = department.Name;
                }
            }

            return userDepartment;
        }
    }
}
