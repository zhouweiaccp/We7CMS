using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.ListControl;
using We7.Model.Core.UI;
using We7.CMS.Common;
using We7.Framework;
using We7.CMS;

namespace We7.Model.UI.Converter
{
    /// <summary>
    /// 类别转化
    /// </summary>
    public class ProcessStateConvert : IUxConvert
    {
        #region IUxConvert 成员

        public string GetText(object dataItem, We7.Model.Core.ColumnInfo columnInfo)
        {
            StringBuilder sb = new StringBuilder();
            string id = ModelControlField.GetValue(dataItem, "ID");

            return String.Format(@"<a href=""javascript:ProcessRemark('{0}');"" onclick=""return {1}"">{2}</a>", id, GetProcessEable(id), GetProcessState(id));
        }

        #endregion

        private string GetProcessState(string id)
        {
            if (!ChannelProcess(id))
            {
                return "[流转历程]";
            }
            Article a = ArticleHelper.GetArticle(id);
            if (a != null && a.State != 2)
            {
                return "[流转历程]";
            }
            Processing ap = ArticleProcessHelper.GetArticleProcess(a);
            string processText = "草稿";
            if (ap != null)
                processText = ap.ProcessDirectionText + ap.ProcessText;
            return processText;
        }

        private bool ChannelProcess(string id)
        {
            string channelProcessLayerNO = GetProcessLayerNO(id);
            if (channelProcessLayerNO != null && channelProcessLayerNO != "")
            {
                return true;
            }
            else
                return false;
        }

        string GetProcessLayerNO(string id)
        {
            string channelID = ArticleHelper.GetArticle(id).OwnerID;
            Channel ch = ChannelHelper.GetChannel(channelID, null);
            if (ch != null && ch.Process == "1" && ch.ProcessLayerNO != null)
                return ch.ProcessLayerNO;
            else
                return "";
        }

        private string GetProcessEable(string id)
        {
            return "true";
        }

        ArticleHelper ArticleHelper
        {
            get { return HelperFactory.Instance.GetHelper<ArticleHelper>(); }
        }
        ProcessingHelper ArticleProcessHelper
        {
            get { return HelperFactory.Instance.GetHelper<ProcessingHelper>(); }
        }
        ChannelHelper ChannelHelper
        {
            get { return HelperFactory.Instance.GetHelper<ChannelHelper>(); }
        }

    }
}
