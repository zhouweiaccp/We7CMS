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
using Thinkment.Data;
using We7.CMS;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceTracking : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Pager_Fired(object sender, EventArgs e)
        {
            DataGridView.DataBind();
        }

        List<string> GetIDs()
        {
            List<string> list = new List<string>();
            for (int i = 0; i <DataGridView.Rows.Count; i++)
            {
                if (((CheckBox)DataGridView.Rows[i].FindControl("chkItem")).Checked)
                {
                    list.Add(((Label)(DataGridView.Rows[i].FindControl("lblID"))).Text);
                }
            }
            return list;
        }
        /// <summary>
        /// 获取操作人名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetProcessNameByAccountID(string accountID)
        {
            return "立杰";
        }

        /// <summary>
        /// 获取反馈类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTypeByID(string id)
        {
            return "领导信箱";
        }

        /// <summary>
        /// 根据用户ID查询办理用户IP
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public string GetUserIPByAccountID(string accountID)
        {
            return "192.168.0.01";
            
        }
        protected void SeleteButton_Click(object sender, EventArgs e)
        {
        }
        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
        }
    }
}
