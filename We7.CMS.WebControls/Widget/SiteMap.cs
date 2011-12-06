using System;
using System.Collections.Generic;
using System.Text;

using We7.Framework;
using We7.CMS;
using We7.CMS.Common;
using System.Web.UI;

namespace We7.CMS.WebControls
{
    public class SiteMap:BaseWebControl
    {

        #region 属性面板参数
        private string siteMapHtml;

        protected string SiteMapHtml
        {
            get { return siteMapHtml; }
            set { siteMapHtml = value; }
        }
        string tag;

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        #endregion
        public string CssClass { get; set; }

        string BuildHtmlStringOne()
        {
            StringBuilder sb = new StringBuilder();
            List<Channel> channels = ChannelHelper.GetChannels(We7Helper.EmptyGUID, false);
            sb.Append("<div class=\"site_map_one\">");
            for (int i = 0; i < channels.Count; i++)
            {
                Channel ch = channels[i];
                if (Tag != null && Tag.Length > 0)
                {
                    if (ch.Tags==null || !ch.Tags.Contains(Tag))
                    {
                        continue;
                    }

                    //List<ChannelTag> tags = ChannelHelper.GetTags(ch.ID);
                    //List<String> ts = new List<string>();
                    //foreach (ChannelTag t in tags)
                    //{
                    //    ts.Add(t.Identifier);
                    //}
                    //if (!ts.Contains(Tag))
                    //{
                    //    continue;
                    //}
                }
                sb.Append("<table class=\"ch\">");
                sb.Append("<tr>");
                sb.Append("<td class=\"a\">");
                sb.Append(String.Format("<a href=\"{0}\">{1}</a>", ch.FullUrl, ch.Name));
                sb.Append("</td>");
                sb.Append("<td class=\"b\">");
                List<Channel> childChannels = ChannelHelper.GetChannels(ch.ID);
                if (childChannels.Count > 0)
                {
                    for (int j = 0; j < childChannels.Count; j++)
                    {
                        if (Tag != null && Tag.Length > 0)
                        {
                            if (childChannels[j].Tags == null || !childChannels[j].Tags.Contains(Tag))
                            {
                                continue;
                            }

                            //List<ChannelTag> childtags = ChannelHelper.GetTags(ch.ID);
                            //List<String> childts = new List<string>();
                            //foreach (ChannelTag childt in childtags)
                            //{
                            //    childts.Add(childt.Identifier);
                            //}
                            //if (!childts.Contains(Tag))
                            //{
                            //    continue;
                            //}
                        }
                        sb.Append(String.Format("<a href=\"{0}\">{1}</a>", childChannels[j].FullUrl, childChannels[j].Name));
                    }
                }
                else
                {
                    sb.Append("<a></a>");
                }
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }
        string BuildHtmlStringTwo()
        {
            StringBuilder sb = new StringBuilder();
            List<Channel> channels = ChannelHelper.GetChannels(We7Helper.EmptyGUID, false);

            sb.Append("<div class=\"site_map_" + CssClass + "\">");
            for (int i = 0; i < channels.Count; i++)
            {

                Channel ch = channels[i];
                if (Tag != null && Tag.Length > 0)
                {
                    if (ch.Tags == null || !ch.Tags.Contains(Tag))
                    {
                        continue;
                    }
                    //List<ChannelTag> tags = ChannelHelper.GetTags(ch.ID);
                    //List<String> ts = new List<string>();
                    //foreach (ChannelTag t in tags)
                    //{
                    //    ts.Add(t.Identifier);
                    //}
                    //if (!ts.Contains(Tag))
                    //{
                    //    continue;
                    //}
                }
                sb.Append("<div>");
                sb.Append(String.Format("<a href=\"{0}\" class=\"channel\">{1}</a><ul>", ch.FullUrl, ch.Name));
                List<Channel> childChannels = ChannelHelper.GetChannels(ch.ID);
                if (childChannels.Count > 0)
                {
                    for (int j = 0; j < childChannels.Count; j++)
                    {
                        if (Tag != null && Tag.Length > 0)
                        {
                            if (childChannels[j].Tags == null || !childChannels[j].Tags.Contains(Tag))
                            {
                                continue;
                            }
                            //List<ChannelTag> childtags = ChannelHelper.GetTags(ch.ID);
                            //List<String> childts = new List<string>();
                            //foreach (ChannelTag childt in childtags)
                            //{
                            //    childts.Add(childt.Identifier);
                            //}
                            //if (!childts.Contains(Tag))
                            //{
                            //    continue;
                            //}
                        }
                        sb.Append("<li>");
                        sb.Append(String.Format("<a href=\"{0}\">{1}</a>", childChannels[j].FullUrl, childChannels[j].Name));
                        sb.Append("</li>");
                    }
                }
                sb.Append("</ul></div>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            siteMapHtml = this.BuildHtmlStringOne();
            
        }
    }
}
