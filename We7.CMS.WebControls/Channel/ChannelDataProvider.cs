using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using We7.CMS;
using We7;
using We7.CMS.Controls;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Framework;
using We7.Framework.Cache;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 栏目类控件数据提供者
    /// </summary>
    public partial class ChannelDataProvider : BaseWebControl
    {
        /// <summary>
        /// 区别多级栏目和普通栏目(前者false;后者true)
        /// </summary>
        private bool channelFlag = true;

        /// <summary>
        /// 是否包含所有子栏目
        /// </summary>
        public bool IncludeAllSons { get; set; }

        /// <summary>
        /// 栏目类业务助手
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        #region 获取数据

        /// <summary>
        /// 获取当前栏目对象
        /// </summary>
        /// <returns>当前栏目</returns>
        protected Channel GetThisChannel()
        {
            if (DesignHelper.IsDesigning && DesignRecords.Count > 0)
                return DesignRecords[0];

            string id = ChannelHelper.GetChannelIDFromURL();
            Channel ch = ChannelHelper.GetChannel(id, null);
            return ch;
        }

        /// <summary>
        /// 获取子栏目列表
        /// </summary>
        /// <returns>栏目列表</returns>
        protected List<Channel> GetChannels()
        {
            List<Channel> list = new List<Channel>();
            if (DesignHelper.IsDesigning)
            {
                list = DesignRecords;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("ChannelDataProvider$GetChannels$");
                sb.AppendFormat("ID:{0}$Url:{1}$", ID, We7Helper.GetChannelUrlFromUrl(Request.RawUrl));


                list = CacheRecord.Create("channel").GetInstance<List<Channel>>(sb.ToString(), delegate()
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
                        query.ParentID = ChannelID;
                    }
                    if (Tag != null && Tag.Length > 0)
                    {
                        query.Tag = Tag;
                    }
                    if (IncludeAllSons)
                        query.IncludeAllSons = true;

                    if (channelFlag && string.IsNullOrEmpty(level))
                    {
                        if (ThisChannel == null)
                        {
                            ThisChannel = new Channel();
                        }
                        ThisChannel.HaveSon = ChannelHelper.HasChild(ChannelID);
                        if (!ThisChannel.HaveSon && noChildsDisplay)
                        {
                            query.ParentID = ThisChannel.ParentID;
                        }
                    }
                    return ChannelHelper.QueryChannels(query);
                });
            }
            return list != null ? FormatChannelsData(list) : new List<Channel>();
        }

        /// <summary>
        /// 根据栏目级别获取父栏目ID
        /// </summary>
        /// <param name="level">栏目级别</param>
        /// <returns>上级栏目ID</returns>
        string GetParentIDByLevel(string level)
        {
            string channelUrl = GetChannelFullUrl(level);
            if (channelUrl == "")
                return We7Helper.EmptyGUID;
            else
                return ChannelHelper.GetChannelIDByFullUrl(channelUrl);
        }

        private string GetChannelFullUrl(string level)
        {
            string channelUrl = "";
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
                    channelUrl = url;
                }
            }
            return channelUrl;
        }

        /// <summary>
        /// 取得栏目树列表对象
        /// </summary>
        /// <returns>栏目列表</returns>
        protected List<Channel> GetChannelTree()
        {
            if (DesignHelper.IsDesigning)
                return DesignRecords;
            StringBuilder sb = new StringBuilder();
            sb.Append("ChannelDataProvider$GetChannelTree$");
            sb.AppendFormat("ID:{0}$Url:{1}$", ID,We7Helper.GetChannelUrlFromUrl(Request.RawUrl));

            return CacheRecord.Create("channel").GetInstance<List<Channel>>(sb.ToString(), delegate()
            {
                channelFlag = false;
                List<Channel> mainMenu = GetChannels();
                if (MaxTreeClass > 1)
                {
                    foreach (Channel ch in mainMenu)
                    {
                        ch.SubChannels = GetSubChannels(ch.ID, 2);
                        ch.HaveSon = ch.SubChannels != null && ch.SubChannels.Count > 0;
                        ch.MenuIsSelected = We7Helper.GetChannelUrlFromUrl(Request.RawUrl).ToLower().StartsWith(ch.FullUrl.ToLower());
                    }
                }
                channelFlag = true;
                return mainMenu;
            });
        }


        /// <summary>
        /// 取得子栏目列表
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        /// <param name="maxClass">最大级别数</param>
        /// <returns>栏目列表</returns>
        List<Channel> GetSubChannels(string channelID, int maxClass)
        {
            List<Channel> subList = ChannelHelper.GetChannels(channelID, true);

            if (maxClass <= MaxTreeClass && subList != null)
            {
                foreach (Channel ch in subList)
                {
                    ch.SubChannels = GetSubChannels(ch.ID, maxClass + 1);
                    ch.HaveSon = ch.SubChannels != null && ch.SubChannels.Count > 0;
                    ch.MenuIsSelected =We7Helper.GetChannelUrlFromUrl(Request.RawUrl).ToLower().StartsWith(ch.FullUrl.ToLower());
                }
            }
            return subList;
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
            string url = GetChannelFullUrl(level);
            if (!string.IsNullOrEmpty(url))
            {
                if (url.EndsWith("/")) url = url.Remove(url.Length - 1);
                channelName = url.Substring(url.LastIndexOf("/") + 1);
            }
            return channelName;
        }

        /// <summary>
        /// 根据栏目名称取Banner
        /// </summary>
        /// <returns></returns>
        protected string GetBannerByName()
        {
            if (DesignHelper.IsDesigning)
            {
                return DesignHelper.GetTagThumbnail("class");
            }
            else
            {
                return "/_data/Channels/banners/banner_" + GetChannelName(Level) + ".jpg";
            }
        }

        private List<Channel> designRecords;
        protected List<Channel> DesignRecords
        {
            get
            {
                if (designRecords == null)
                {
                    List<Channel> chs;
                    DesignHelper.FillItems<Channel>(out chs, Int32.MaxValue);
                    designRecords = GetSubChannel(We7Helper.EmptyGUID, chs);
                    if (designRecords == null)
                        designRecords = new List<Channel>();
                }
                return designRecords;
            }
        }

        public string GetBanner()
        {
            if (DesignHelper.IsDesigning)
            {
                return DesignHelper.GetTagThumbnail("banner");
            }
            else
            {
                return "/_data/Channels/banners/banner_" + GetChannelName(Level) + ".jpg";
            }
        }


        #endregion

        /// <summary>
        /// 取得指定栏目ID下的子栏目
        /// </summary>
        /// <param name="id">指定栏目ID</param>
        /// <param name="list">栏目列表</param>
        /// <returns>指定ID下的子栏目</returns>
        protected List<Channel> GetSubChannel(string id, List<Channel> list)
        {
            if (list == null)
                return null;
            List<Channel> result = new List<Channel>();
            foreach (Channel c in list)
            {
                if (c.ParentID == id || id == We7Helper.EmptyGUID && String.IsNullOrEmpty(c.ParentID))
                {
                    result.Add(c);
                    List<Channel> children = GetSubChannel(c.ID, list);
                    if (children != null)
                    {
                        c.SubChannels = children;
                    }
                }
            }
            return result;
        }
    }
}
