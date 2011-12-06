using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.Framework.Util;
using System.Web.UI.HtmlControls;
using We7.CMS.Common.Enum;

namespace We7.CMS.WebControls
{
    public class TreeMenuTwoProvider : BaseWebControl
    {
        #region 属性面板参数
        public string CssClass { get; set; }

        /// <summary>
        /// 获取当前栏目对象
        /// </summary>
        public Channel CurrentChannel
        {
            get
            {
                string id = ChannelHelper.GetChannelIDFromURL();
                Channel ch = ChannelHelper.GetChannel(id, null);
                return ch;
            }
        }
        List<Channel> lsChannels = new List<Channel>();
        /// <summary>
        /// 当前栏目层级关系
        /// </summary>
        /// <param name="currentChannel"></param>
        public void InitChannels(Channel currentChannel)
        {
            if (currentChannel != null)
            {
                lsChannels.Add(currentChannel);
                Channel tempChanel = ChannelHelper.GetChannel(currentChannel.ParentID, null);
                InitChannels(tempChanel);
            }
        }

        /// <summary>
        /// 当前的栏目ID
        /// </summary>
        public string ChannelID
        {
            get
            {
                return ChannelHelper.GetChannelIDFromURL();
            }
        }
        string parentID;
        /// <summary>
        /// 上级栏目ID
        /// </summary>
        public string ParentID
        {
            get { return ChannelHelper.FormatChannelGUID(parentID); }
            set { parentID = value; }
        }

        bool showParentName;
        /// <summary>
        /// 显示父栏目
        /// </summary>
        public bool ShowParentName
        {
            get { return showParentName; }
            set { showParentName = value; }
        }

        string level;
        /// <summary>
        /// 分级
        /// </summary>
        public string Level
        {
            get { return level; }
            set { level = value; }
        }


        int titleMaxLength = 0;
        /// <summary>
        /// 标题最大字数
        /// </summary>
        public int TitleMaxLength
        {
            get { return titleMaxLength; }
            set { titleMaxLength = value; }
        }
        string tag;
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        string showToolTip;
        /// <summary>
        /// 鼠标提示
        /// </summary>
        public string ShowToolTip
        {
            get { return showToolTip; }
            set { showToolTip = value; }
        }


        public string HtmlData;
        #endregion
        protected void GetAllChildChannel(StringBuilder sb, string parentID)
        {
            ParentID = parentID;
            List<Channel> childChannels = GetChannels();
            //ChannelHelper.GetChannels(parentID,true);
            if (childChannels != null)
            {
                sb.Append("<ul>");
                for (int j = 0; j < childChannels.Count; j++)
                {
                    //if (type == "One")
                    //{
                    List<Channel> channelList = ChannelHelper.GetChannels(childChannels[j].ID, true);

                    if (channelList != null && channelList.Count > 0)
                    {
                        bool isFind = false;
                        for (int k = 0; k < lsChannels.Count; k++)
                        {
                            if (lsChannels[k].ID == childChannels[j].ID)
                            {
                                isFind = true;
                                break;
                            }
                        }
                        if (isFind)
                        {
                            if (CurrentChannel != null && childChannels[j].ID == CurrentChannel.ID)
                            {
                                sb.Append("<li><span class=\"folder TreeMenuTwoProviderCurrent\"><a href='" + childChannels[j].RealUrl + "'>" + childChannels[j].Name + "</a></span>");
                            }
                            else
                            {
                                sb.Append("<li><span class=\"folder\"><a href='" + childChannels[j].RealUrl + "'>" + childChannels[j].Name + "</a></span>");
                            }                            
                        }
                        else
                        {
                            sb.Append("<li class=\"closed\" ><span class=\"folder\"><a href='" + childChannels[j].RealUrl + "'>" + childChannels[j].Name + "</a></span>");
                        }
                        GetAllChildChannel(sb, childChannels[j].ID);
                    }
                    else
                    {
                        if (CurrentChannel != null && childChannels[j].ID == CurrentChannel.ID)
                        {
                            sb.Append("<li><span class=\"file TreeMenuTwoProviderCurrent\"><a href='" + childChannels[j].RealUrl + "'>" + childChannels[j].Name + "</a></span>");
                        }
                        else
                        {
                            sb.Append("<li><span class=\"file\"><a href='" + childChannels[j].RealUrl + "'>" + childChannels[j].Name + "</a></span>");
                        }                        
                    }
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }
        }



        /// <summary>
        /// 获取子栏目列表
        /// </summary>
        /// <returns>栏目列表</returns>
        protected List<Channel> GetChannels()
        {
            ChannelQuery query = new ChannelQuery();
            if (OrderFields == null || OrderFields == string.Empty)
                OrderFields = "Index|Asc";
            query.OrderKeys = OrderFields;
            query.State = ArticleStates.Started;

            if (ParentID != null && ParentID.Length > 0)
            {
                query.ParentID = ParentID;
            }
            else if (Level != null && Level.Length > 0 & We7Helper.IsNumber(Level))
            {
                query.ParentID = GetParentIDByLevel((int.Parse(Level) - 1).ToString());
            }
            else
            {
                if (CurrentChannel == null)
                {
                    query.ParentID = We7.We7Helper.EmptyGUID;
                }
                else
                {
                    query.ParentID = CurrentChannel.ID;
                }                
            }
            if (Tag != null && Tag.Length > 0)
            {
                query.Tag = Tag;
            }
            List<Channel> list = ChannelHelper.QueryChannels(query);
            if (list == null) list = new List<Channel>();
            list = FormatChannelsData(list);
            return list;
        }

        /// <summary>
        /// 根据栏目级别获取父栏目ID
        /// </summary>
        /// <param name="level">栏目级别</param>
        /// <returns>上级栏目ID</returns>
        string GetParentIDByLevel(string level)
        {
            string channelName = GetChannelName(level);
            if (channelName == "")
                return We7Helper.EmptyGUID;
            else
                return ChannelHelper.GetChannelIDByFullUrl(channelName);
        }

        /// <summary>
        /// 格式化栏目数据
        /// </summary>
        /// <param name="list">栏目列表</param>
        /// <returns>栏目列表</returns>
        protected virtual List<Channel> FormatChannelsData(List<Channel> list)
        {
            DateTime now = DateTime.Now;

            foreach (Channel ch in list)
            {
                if (ShowToolTip != null && ShowToolTip.Length > 0)
                    ch.Title = ch.Name;

                if (TitleMaxLength > 0 && ch.Name.Length > TitleMaxLength)
                {
                    ch.Name = ch.Name.Substring(0, TitleMaxLength) + "...";
                }
                if (We7Helper.GetChannelUrlFromUrl(Request.RawUrl).ToLower().StartsWith(ch.RealUrl.ToLower()))
                    ch.MenuIsSelected = true;
            }

            return list;
        }


        /// <summary>
        /// 根据栏目层数取得栏目Url名称
        /// </summary>
        /// <param name="level">栏目等级</param>
        /// <returns>栏目名称</returns>
        public string GetChannelName(string level)
        {
            string channelName = "default";
            if (We7Helper.IsNumber(level))
            {
                int index = int.Parse(level);
                if (index > 0)
                {
                    string url = We7Helper.GetChannelUrlFromUrl(Request.RawUrl);
                    int pos = 0;
                    for (int i = 0, temp = 0; i <= index; i++)
                    {
                        temp = url.IndexOf("/", pos) + 1;
                        if (temp == 0)
                            break;
                        else
                            pos = temp;
                    }
                    url = url.Substring(0, pos);
                    channelName = ChannelHelper.GetChannelUrlFromUrl(url, Context.Request.ApplicationPath);
                }
            }
            return channelName;
        }

        /// <summary>
        /// 绑定树
        /// </summary>
        private void BindTree()
        { 
            StringBuilder sb = new StringBuilder("");
            List<Channel> channels = GetChannels();
            bool displayParent = false;
            if (ShowParentName)
            {
                string parentID = "";
                if (ParentID != null && ParentID.Length > 0)
                {
                    parentID = ParentID;
                }
                else
                {
                    parentID = We7.We7Helper.EmptyGUID;
                }
                Channel parentChannel = ChannelHelper.GetChannel(parentID, null);
                if(parentChannel != null)
                {
                    displayParent = true;
                    sb.Append("<li><span class=\"folder\"><a href='" + parentChannel.RealUrl + "'>" + parentChannel.Name + "</a></span><ul>");
                }
            }
            //ChannelHelper.GetChannels(We7.We7Helper.EmptyGUID, true);
           
            for (int i = 0; i < channels.Count; i++)
            {
                Channel ch = channels[i];
                List<Channel> channelList = ChannelHelper.GetChannels(ch.ID, true);

                if (channelList != null && channelList.Count > 0)
                {
                    bool isFind = false;
                    for (int j = 0; j < lsChannels.Count; j++)
                    {
                        if (lsChannels[j].ID == ch.ID)
                        {
                            isFind = true;
                            break;
                        }
                    }
                    if (isFind)
                    {
                        if (CurrentChannel != null && ch.ID == CurrentChannel.ID)
                        {
                            sb.Append("<li><span class=\"folder TreeMenuTwoProviderCurrent\"><a href='" + ch.RealUrl + "'>" + ch.Name + "</a></span>");
                        }
                        else
                        {
                            sb.Append("<li><span class=\"folder\"><a href='" + ch.RealUrl + "'>" + ch.Name + "</a></span>");
                        }
                    }
                    else
                    {
                        sb.Append("<li class=\"closed\" ><span class=\"folder\"><a href='" + ch.RealUrl + "'>" + ch.Name + "</a></span>");
                    }                    
                    GetAllChildChannel(sb, ch.ID);
                }
                else
                {
                    if (CurrentChannel != null && ch.ID == CurrentChannel.ID)
                    {
                        sb.Append("<li><span class=\"file TreeMenuTwoProviderCurrent\"><a href='" + ch.RealUrl + "'>" + ch.Name + "</a></span>");
                    }
                    else
                    {
                        sb.Append("<li><span class=\"file\"><a href='" + ch.RealUrl + "'>" + ch.Name + "</a></span>");
                    }
                }
                sb.Append("</li>");
            }
            if(displayParent)
            {
                sb.Append("</ul></li>");
            }
            sb.Append("<script>$(function(){$('#" + this.ClientID + "').treeview({unique:true});});</script>").ToString();
            HtmlData = sb.ToString();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitChannels(CurrentChannel);
            IncludeJavaScript();
            JavaScriptManager.Include("/Admin/Ajax/jquery/jquery.cookie.js",
                                      "/Admin/Ajax/jquery/jquery.treeview.min.js");
            //HtmlGenericControl ctr = new HtmlGenericControl("link");
            //ctr.Attributes["href"] = "/Admin/Ajax/jquery/css/jquery.treeview.css";
            //ctr.Attributes["type"] = "text/css";
            //ctr.Attributes["rel"] = "Stylesheet";
            //Page.Header.Controls.Add(ctr);
            BindTree();
        }
    }
}
