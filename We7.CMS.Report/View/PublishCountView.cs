using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS
{
    /// <summary>
    /// 发布量统计对象
    /// </summary>
     [Serializable]
    public class PublishCountView : PVKeyCountView
    {
        int clicks;

         /// <summary>
         /// 点击量
         /// </summary>
        public int Clicks
        {
            get { return clicks; }
            set { clicks = value; }
        }

        double clickPercent;
         /// <summary>
         /// 点击量百分比
         /// </summary>
        public double ClickPercent
        {
            get { return clickPercent; }
            set { clickPercent = value; }
        }

        int acceptCount;
         /// <summary>
         /// 稿件采用（正式发布）数
         /// </summary>
        public int AcceptCount
        {
            get { return acceptCount; }
            set { acceptCount = value; }
        }

        string channelID;
         /// <summary>
         /// 栏目ID
         /// </summary>
        public string ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }
    }
}
