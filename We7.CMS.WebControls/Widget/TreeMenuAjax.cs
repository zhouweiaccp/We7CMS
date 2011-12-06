using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Framework;
using We7.CMS.Accounts;

namespace We7.CMS.WebControls
{
    public class TreeMenuAjax :IHttpHandler
    {

        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID]; }
        }

        /// <summary>
        /// 权限业务对象
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// 栏目业务对象
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }


        /// <summary>
        /// 当前用户ID
        /// </summary>
        protected virtual string AccountID
        {
            get
            {              
                return Security.CurrentAccountID;
            }
        }

        static string TopTitle = "根栏目";
        static string TopSummary = "此栏目下的所有栏目，将作为第一级栏目。";



        /// <summary>
        /// 获取当前栏目对象
        /// </summary>
        /// <returns>当前栏目</returns>
        protected Channel GetThisChannel()
        {
            string id = ChannelHelper.GetChannelIDFromURL();
            Channel ch = ChannelHelper.GetChannel(id, null);
            return ch;
        }

        
        List<Channel> lsChannels = new List<Channel>();
        public void InitChannels(Channel currentChannel)
        {
            if(currentChannel != null)
            {
                lsChannels.Add(currentChannel);
                Channel tempChanel = ChannelHelper.GetChannel(currentChannel.ID, null);
                InitChannels(tempChanel);                   
            }
        }



        string GetNodeContent(string ChannelID)
        {
            if (!We7Helper.IsEmptyID(ChannelID))
            {
                string id = ChannelID;
                Channel ch = ChannelHelper.GetChannel(id, null);
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class=itemDetail style=line-height:220%><ul>");
                sb.Append("<li>栏目名称：" + ch.Name + "</li>");
                //sb.Append("<li>唯一名称：" + ch.ChannelName + "</li>");
                sb.Append("<li>Url地址：<a href=\"" + ch.FullUrl + "\" target=\"_blank\"><u>" + ch.FullUrl + "</u></a></li>");
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

        string getChannelTree(string ChannelID)
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

            List<Channel> channelList = ChannelHelper.GetChannels(c.ID,true);
            List<Channel> list = new List<Channel>();
            foreach (Channel ch in channelList)
            {
                if (We7Helper.IsEmptyID(AccountID) || HavePermission(ch.ID))
                {
                    list.Add(ch);                 
                }
            }

            string TopStr = @"[";
            string BottomStr = "]";
            string MiddleStr = "";

            foreach (Channel ch in list)
            {
                List<Channel> listSon = ChannelHelper.GetChannels(ch.ID,true);

                string name = ch.Name;
                if (ch.Type == ((int)TypeOfChannel.QuoteChannel).ToString())
                    name = "[专题]" + name;
                //name = "<a href='" + ch.FullUrl + "' target='_blank'>" + name + "<a>";
                if (listSon != null && listSon.Count > 0)   //有子菜单
                {
                    string strHaveSubMenu = @"text:'{0}',id:'{1}',href:'{2}',hrefTarget:'_blank'";
                    MiddleStr = MiddleStr + "{" + string.Format(strHaveSubMenu, name, We7Helper.RemoveBrarket(ch.ID),ch.FullUrl) + "},";
                }
                else
                {
                    string strNotHaveSubMenu = @"text:'{0}',id:'{1}',leaf:true,href:'{2}',hrefTarget:'_blank'";
                    MiddleStr = MiddleStr + "{" + string.Format(strNotHaveSubMenu, name, We7Helper.RemoveBrarket(ch.ID),ch.FullUrl) + "},";
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


        #region IHttpHandler 成员

        bool IHttpHandler.IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            //throw new NotImplementedException();

            if (context.Request["type"] == null)
            {
                context.Response.Write(getChannelTree(GetChannelID(context)));
            }
            else if (context.Request["type"] == "detail")
            {
                context.Response.Write(GetNodeContent(GetChannelID(context)));
            }
            else if ("All" == context.Request["type"])
            {
                context.Response.Write(GetAll());
            }
                context.Response.End();
        }


        private string GetAll()
        {
            string result = "";
            string id = ChannelHelper.GetChannelIDFromURL();
            Channel ch = ChannelHelper.GetChannel(id, null);
            InitChannels(ch);
            for (int i = lsChannels.Count - 1; i >= 0;i-- )
            {
                result += getChannelTree(lsChannels[i].ID);
            }
            return result;
        }

        private string GetChannelID(HttpContext context)
        {
            if (context.Request["node"] != null && context.Request["node"].ToString() != "root")
                return We7Helper.FormatToGUID(context.Request["node"]);
            else
                return string.Empty;
        }

        #endregion
    }
}
