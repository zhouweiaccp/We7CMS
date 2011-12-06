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
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class ChannelNodesGet : BasePage
    {
        static string TopTitle = "根栏目";
        static string TopSummary = "此栏目下的所有栏目，将作为第一级栏目。";

        /// <summary>
        /// 这里不判断权限（Menu中不包含此页面地址），否则ajax发送过来的请求不能处理
        /// </summary>
        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        string ChannelID
        {
            get
            {
                if (Request["node"] != null && Request["node"].ToString()!="root")
                    return We7Helper.FormatToGUID(Request["node"]);
                else
                    return string.Empty;
            }
        }

        string DataType
        {
            get
            {
                return Request["type"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (DataType == null)
                Response.Write(getChannelTree());
            else if (DataType == "detail")
                Response.Write(GetNodeContent());
            //Response.Flush();
            //Response.End();
        }

        string GetNodeContent()
        {
            if (!We7Helper.IsEmptyID(ChannelID))
            {
                string id = ChannelID;
                Channel ch = ChannelHelper.GetChannel(id, null);
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class=itemDetail style=line-height:220%><ul>");
                sb.Append("<li>栏目名称：" + ch.Name + "</li>");
                //sb.Append("<li>唯一名称：" + ch.ChannelName + "</li>");
                sb.Append("<li>Url地址：<a href=\"" + ch.FullUrl + "\" target=\"_blank\"><u>" + ch.FullUrl + "</u></a> <a href='/go/rss.aspx?ChannelUrl=" + ch.FullUrl + "' title='RSS地址' target='_blank'><img src='/admin/images/icon_share.gif' /></a></li>");
                //sb.Append("<li>别名：" + ch.Alias + "</li>");
                //sb.Append(string.Format("<li>文件存放目录：<a href=Folder.aspx?folder={0}&filter=*&create=auto target=ifRightDetail><u>{0}</u></a></li>", string.Format("\\{0}\\{1}", Constants.ChannelPath, ch.ChannelFolder)));
                //if(TemplateHelper.GetTemplateName(ch.TemplateName)==string.Empty)
                //    sb.Append(string.Format("<li>索引模板：<u>{0}(模板文件不存在)</u></li>", ch.TemplateName));
                //else
                //    sb.Append(string.Format("<li>索引模板：<a href=Compose.aspx?file={0} target=_blank><u>{1}</u></a></li>", ch.TemplateName, TemplateHelper.GetTemplateName(ch.TemplateName)));

                //if (TemplateHelper.GetTemplateName(ch.DetailTemplate) == string.Empty)
                //    sb.Append(string.Format("<li>详细页模板：<u>{0}(模板文件不存在)</u></li>", ch.DetailTemplate));
                //else
                //    sb.Append(string.Format("<li>详细页模板：<a href=Compose.aspx?file={0} target=_blank><u>{1}</u></a></li>", ch.DetailTemplate, TemplateHelper.GetTemplateName(ch.DetailTemplate)));

                sb.Append("<li>安全级别：" + ch.SecurityLevelText + "</li>");
                sb.Append("<li>类型：" + ch.TypeText + "</li>");
                sb.Append("<li>状态：" + ch.StateText + "</li>");
                sb.Append("</ul><div>");
                return sb.ToString();
            }
            else
                return string.Empty;
        }

        string getChannelTree()
        {
            string id = ChannelID;

            Channel c = new Channel();
            if (We7Helper.IsEmptyID(id))
            {
                if (id == We7Helper.EmptyWapGUID)
                {
                    c.ID = We7Helper.EmptyWapGUID;
                    c.ParentID = We7Helper.EmptyWapGUID;
                }
                else
                {
                    c.ID = We7Helper.EmptyGUID;
                    c.ParentID = We7Helper.EmptyGUID;
                }
                c.Name = TopTitle;
                c.Description = TopSummary;
            }
            else
            {
                c.ID = id;
                c = ChannelHelper.GetChannel(id, null);
            }

            List<Channel> channelList = ChannelHelper.GetChannels(c.ID);
            List<Channel> list = new List<Channel>();

            if (channelList != null)
            {
                foreach (Channel ch in channelList)
                {
                    if (We7Helper.IsEmptyID(AccountID) || HavePermission(ch.ID))
                    {
                        list.Add(ch);
                    }
                }
            }

            string TopStr = @"[";
            string BottomStr = "]";
            string MiddleStr = "";

            foreach (Channel ch in list)
            {
                List<Channel> listSon = ChannelHelper.GetChannels(ch.ID);

                string name=ch.Name;
                if(ch.Type==((int)TypeOfChannel.QuoteChannel).ToString())
                    name="[专题]"+name;
                if (listSon!=null && listSon.Count > 0)   //有子菜单
                {
                    string strHaveSubMenu = @"text:'{0}',id:'{1}'";
                    MiddleStr = MiddleStr + "{" + string.Format(strHaveSubMenu, name, We7Helper.RemoveBrarket(ch.ID)) + "},";
                }
                else
                {
                    string strNotHaveSubMenu = @"text:'{0}',id:'{1}',leaf:true";
                    MiddleStr = MiddleStr + "{" + string.Format(strNotHaveSubMenu, name, We7Helper.RemoveBrarket(ch.ID)) + "},";
                }
            }

            if (MiddleStr.EndsWith(",")) MiddleStr = MiddleStr.Remove(MiddleStr.Length - 1);

            return TopStr + MiddleStr + BottomStr;

        }

        bool HavePermission(string chID)
        {
            List<string> channels = AccountHelper.GetObjectsByPermission(AccountID, "Channel.Input");
            return channels.Exists(delegate(string s) { return (s == chID) ? true : false; });            
        }

    }
}