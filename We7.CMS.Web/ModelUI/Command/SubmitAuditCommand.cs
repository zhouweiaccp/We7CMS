using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.CMS.Common;
using We7.CMS;
using We7.CMS.Common.Enum;
using We7.Model.Core;
using System.Collections.Generic;
using We7.CMS.Accounts;
using We7.Framework.Cache;

namespace We7.Model.UI.Command
{
    public class SubmitAuditCommand : BaseCommand
    {
        public override object Do(PanelContext data)
        {
            List<DataKey> dataKeys = data.State as List<DataKey>;
            if (dataKeys != null)
            {
                foreach (DataKey key in dataKeys)
                {
                    string id = key["ID"] as string;
                    if (ChannelProcess(id))
                    {
                        Article a = ArticleHelper.GetArticle(id);
                        Processing ap = ArticleProcessHelper.GetArticleProcess(a);
                        if (ap.ArticleState != ArticleStates.Checking)
                        {
                            string accName = AccountHelper.GetAccount(AccountID, new string[] { "LastName" }).LastName;
                            ap.ProcessState = ProcessStates.FirstAudit;
                            ap.ProcessDirection = ((int)ProcessAction.Next).ToString();
                            ap.ProcessAccountID = AccountID;
                            ap.ApproveName = accName;
                            ArticleProcessHelper.SaveFlowInfoToDB(a, ap);
                        }
                    }
                }
            }
            CacheRecord.Create(data.ModelName).Release();
            return null;
        }

        public bool ChannelProcess(string id)
        {
            string channelProcessLayerNO = GetProcessLayerNO(id);
            return !String.IsNullOrEmpty(channelProcessLayerNO);
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

        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        ProcessingHelper ArticleProcessHelper
        {
            get { return HelperFactory.GetHelper<ProcessingHelper>(); }
        }
    }
}