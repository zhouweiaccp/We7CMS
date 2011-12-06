using System;
using System.IO;
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
    public partial class DataControlList : BasePage
    {

        protected override void Initialize()
        {
            string selectQuery = this.FieldDropDownList.SelectedItem.ToString();
            string query = SearchTextBox.Text;
            DataControl[] ds = TemplateHelper.GetDataControls(query,selectQuery);

                DetailGridView.DataSource = ds;
                DetailGridView.DataBind();

                ModeDataList.DataSource = ds;
                ModeDataList.DataBind();
            
            if (ds.Length == 0)
            {
                ShowMessage("没有符合条件的控件。");
            }
            else
            {
                ShowMessage(String.Format("总共 {0} 个控件。", ds.Length));
            }
        }

        protected override We7.CMS.Common.Enum.MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        protected void QueryButton_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        protected void Sort(string sortName)
        {
            string sortText = sortName;
            DataControl[] ds = TemplateHelper.SortDataControls(sortText);

                DetailGridView.DataSource = ds;
                DetailGridView.DataBind();

                ModeDataList.DataSource = ds;
                ModeDataList.DataBind();

            if (ds.Length == 0)
            {
                ShowMessage("没有符合条件的控件。");
            }
            else
            {
                ShowMessage(String.Format("总共 {0} 个控件。", ds.Length));
            }
        }
        protected void ArticleButton_Click(object sender, EventArgs e)
        {
            string sort ="文章";
            Sort(sort);
        }
        protected void ChannelButton_Click(object sender, EventArgs e)
        {
            string sort ="栏目";
            Sort(sort);
        }
        protected void ImgButton_Click(object sender, EventArgs e)
        {
            string sort = "图片";
            Sort(sort);
        }
        protected void ListButton_Click(object sender, EventArgs e)
        {
            string sort = "列表";
            Sort(sort);
        }
        protected void MenuButton_Click(object sender, EventArgs e)
        {
            string sort = "菜单";
            Sort(sort);
        }
        protected void AdButton_Click(object sender, EventArgs e)
        {
            string sort = "广告";
            Sort(sort);
        }
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string sort = "登录";
            Sort(sort);
        }
        protected void StoreButton_Click(object sender, EventArgs e)
        {
            string sort = "商铺";
            Sort(sort);
        }
        protected void OtherButton_Click(object sender, EventArgs e)
        {
            string sort = "其他";
            Sort(sort);
        }
        void ShowMessage(string s)
        {
            MessageLabel.Text = s;
        }
    }
}
