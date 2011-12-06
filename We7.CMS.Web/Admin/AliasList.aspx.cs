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
using System.Xml;
using System.Xml.Serialization;

using Thinkment.Data;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
    public partial class AliasList : BasePage
    {

        protected override void Initialize()
        {
            TagsGroup ag = TagsHelper.GetTagsGroup();

            DetailGridView.DataSource = ag.Items;
            DetailGridView.DataBind();
        }

        protected void QueryButton_Click(object sender, EventArgs e)
        {
            TagsGroup ag = TagsHelper.GetTagsGroup();
            List<TagsGroup.Item> items = new List<TagsGroup.Item>();
            foreach (TagsGroup.Item item in ag.Items)
            {
                if (item.Words.Contains(SearchTextBox.Text.Trim()))
                {
                    items.Add(item);
                }
            }
            DetailGridView.DataSource = items;
            DetailGridView.DataBind();
           // Initialize();
        }

        void ShowMessage(string s)
        {
            MessageLabel.Text = s;
        }

    }
}
