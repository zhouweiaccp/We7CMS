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
using We7.CMS;
using We7.CMS.Common;
using We7.Framework;


namespace We7.CMS.Web.Admin.controls
{
    public partial class Channel_tag : BaseUserControl
    {

        string ChannelID
        {
            get { return Request["id"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControls();
            }
        }

        void InitControls()
        {
            ChannelIDHidden.Text = string.Format("<input type=hidden id=IDHidden value={0} /> <input type=hidden id=TagTypeHidden value=channel  /> ", ChannelID);
            TagListLitiral.Text = LoadMyTags();
            CommonTagsLiteral.Text = LoadCommonTags();
            TagDictionaryLiteral.Text = LoadTagDictionary();
        }

        string LoadMyTags()
        {
            string tagLi = "<LI><IMG class=Icon height=16 src=\"/admin/images/icon_globe.gif\" width=16 alt=\"{0}\">{1}<A class=Del title=\"{0}\"  href=\"javascript:void(0)\"  > [x]</A> </LI>";

            StringBuilder sb = new StringBuilder();
            if (!We7Helper.IsEmptyID(ChannelID))
            {
                List<string> tagsList   = ChannelHelper.GetTags(ChannelID);
                foreach (string str in tagsList)
                {
                    sb.AppendLine(string.Format(tagLi, str, str.Length > 10 ? We7.Framework.Util.Utils.CutString(str, 0, 10) + ".." : str));
                }
            }

            return sb.ToString();
        }

        string LoadCommonTags()
        {
            string tagA = "<a href=\"javascript:addTag('{0}')\" title=\"为栏目添加标签 {0}？\"  >{0}</a> ";
            StringBuilder sb = new StringBuilder();

            List<Tags> tagsList = TagsHelper.GetTags(1, 10);
            if (tagsList != null)
            {
                List<string> tags = new List<string>();
                foreach (Tags tag in tagsList)
                {
                    sb.AppendLine(string.Format(tagA, tag.Identifier));
                }
            }
            return sb.ToString();
        }

        string LoadTagDictionary()
        {
            string tagA = "<a href=\"javascript:addTag('{0}')\" title=\"为栏目添加标签 {0}？\"  >{0}</a> ";
            int maxCount = 200;
            StringBuilder sb = new StringBuilder();

            TagsGroup ag = TagsHelper.GetTagsGroup();
            int i = 0;
            foreach (TagsGroup.Item tag in ag.Items)
            {
                sb.AppendLine(string.Format(tagA, tag.Words));

                if (i > maxCount) break;
                i++;
            }

            return sb.ToString();
        }

    }
}