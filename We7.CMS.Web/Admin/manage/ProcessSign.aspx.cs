using System;
using We7.CMS.Config;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using System.Collections.Generic;
using System.Xml;
using System.Web.UI.WebControls;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class ProcessSign : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.NoMenu;
            }
        }
        /// <summary>
        /// 是否判断用户权限
        /// </summary>
        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        protected ProcessingHelper ProcessingHelper
        {
            get { return HelperFactory.GetHelper<ProcessingHelper>(); }
        }
        protected ProcessHistoryHelper ProcessHistoryHelper
        {
            get { return HelperFactory.GetHelper<ProcessHistoryHelper>(); }
        }
        

        bool IsAdivice 
        {
            get
            {
                if (Request["id"] != null && Request["id"] != "")
                {
                    return false;
                }
                else   if (Request["adviceID"] != null && Request["adviceID"] != "")
                {
                    return true;
                }
                else
                    return false;
            }
        }

       public string ObjectID
        {
            get
            {
                if (Request["id"] != null && Request["id"] != "")
                {
                    return Request["id"].ToString();
                }
                if (Request["adviceID"] != null && Request["adviceID"] != "")
                {
                    return Request["adviceID"].ToString();
                }
                else
                    return "";
            }
        }
        public string HrefAddress
        {
            get
            {
                if (Request["id"] != null && Request["id"] != "")
                {
                    return "/admin/manage/ArticleView.aspx?id=<%="+"ObjectID"+" %>";
                }
                if (Request["adviceID"] != null && Request["adviceID"] != "")
                {
                    return "/admin/Advice/AdviceDetail.aspx?from=advice&ID=<%" + "ObjectID" + " %>";
                }
                else
                    return "";
            }
        }

        public string Titles
        {
            get
            {
                string title = ArticleHelper.GetArticleName(ObjectID);
                return title;
            }
        }

        static string TargetSiteID;
        string TargetSites
        {
            get
            {
                if (SiteConfigs.GetConfig() != null && SiteConfigs.GetConfig().SiteGroupEnabled)
                {
                    if (ViewState["$TargetSites"] == null)
                    {
                        MoreEventArgs evenArgs = new MoreEventArgs();
                        ShareEventFactory.Instance.OnGetShareTargetSites(ObjectID, evenArgs);
                        string sites =evenArgs.ReturnValue;
                        if (!string.IsNullOrEmpty(sites))
                        {
                            TargetSiteID = sites.Split(',')[0];
                        }
                        ViewState["$TargetSites"] = sites;
                    }
                    return ViewState["$TargetSites"] as string;
                }
                else
                    return string.Empty;
            }            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ArticleTitleLabel.Text = Titles;
                BindTemplate();
                if (ObjectID != "")
                {
                    InitControls();
                }
            }
        }

        void InitControls()
        {
            SignPanelDiv.Visible = false;
            string curLayerNOText ="";
            if(!IsAdivice)
                curLayerNOText=ProcessHelper.GetCurLayerNOText(ObjectID);
            else
                curLayerNOText = ProcessHelper.GetCurLayerNOAdviceText(ObjectID);

            if (curLayerNOText != "") //文章当前审批进程：类似 Channel.FirstAudit
            {
                string channelID = "";
                if (!IsAdivice)
                    channelID = ArticleHelper.GetArticle(ObjectID).OwnerID;
                else
                    channelID = AdviceHelper.GetAdvice(ObjectID).TypeID;

                List<string> contents = AccountHelper.GetPermissionContents(AccountID, channelID);
                if (contents.Contains(curLayerNOText))
                {
                    ApproveDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
                    ApproveName.Text = AccountHelper.GetAccount(AccountID, new string[] { "LastName" }).LastName;
                    if (!IsAdivice)
                    {
                        Article article = ArticleHelper.GetArticle(ObjectID);
                        SummaryLabel.Text = "审核路径：" + ProcessingHelper.CreateArticleFlowPathHtml(article, TargetSites);
                    }
                    else
                    {
                        Advice advice = AdviceHelper.GetAdvice(ObjectID);
                        SummaryLabel.Text = "审核路径：" + ProcessingHelper.CreateAdviceFlowPathHtml(advice, TargetSites);
                    }
                    SignPanelDiv.Visible = true;
                }                   
            }
        }

        /// <summary>
        /// 处理流转
        /// </summary>
        void HandleProcessAction()
        {
            ProcessAction action = ProcessAction.Next;
            if (ActionTextBox.Text != "")
                action = (ProcessAction)int.Parse(ActionTextBox.Text);
            Object oldObject = null;
            bool dataSuccess = true;
            if (!IsAdivice)
            {
                oldObject = ArticleHelper.GetArticle(ObjectID);
                Article oa = new Article();
                oa = (Article)oldObject;
                action = UpdateArticleProcessState(action, oa);
                Article a = ArticleHelper.GetArticle(ObjectID);
                dataSuccess = TransferSitesFlow(action, a);
            }
            else
            {
                oldObject = AdviceHelper.GetAdvice(ObjectID);
                Advice a=new Advice();
                a=(Advice)oldObject;
                UpdateAdviceProcessState(action, a);
                dataSuccess = true;
            }

            if (!dataSuccess)
            {
                RollBackProcessState(oldObject);
                Messages.ShowError("无法完成操作！跨站数据传递时出现错误。");
            }
            else
            {
                Messages.ShowMessage("您的操作已成功执行！");
                SignPanelDiv.Visible = false;
                ProcessHistoryList1.Binding();
            }
        }

        /// <summary>
        /// 回滚审核状态
        /// </summary>
        /// <param name="oldObject"></param>
        private void RollBackProcessState(object oldObject)
        {
            if (!IsAdivice)
            {
                Article a = (Article)oldObject;
                ArticleHelper.UpdateArticle(a, new string[] { "State", "ProcessState", "FlowXml", "ProcessDirection" });
            }
        }

        /// <summary>
        /// 站间审批数据传送
        /// </summary>
        /// <param name="action"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        bool TransferSitesFlow(ProcessAction action,Article article)
        {
            bool success=true;
            Processing p = ProcessingHelper.GetArticleProcess(article);
            string oldFlowXml=article.FlowXml;
            MoreEventArgs evenArgs = new MoreEventArgs();
            evenArgs.FlowXml = oldFlowXml;
            switch (action)
            {
                case ProcessAction.Restart:
                case ProcessAction.Next:
                case ProcessAction.Previous:
                    if (p.FromOtherSite)
                        ShareEventFactory.Instance.OnFeedBackShareData(ObjectID, evenArgs);
                    break;
                case ProcessAction.SubmitSite:
                    ShareEventFactory.Instance.OnSubmitSiteShareData(ObjectID, evenArgs);
                    success = true;
                    break;

                default:
                    break;
            }
            return success;
        }

        /// <summary>
        /// 保存文章审核信息
        /// </summary>
        /// <param name="action"></param>
        /// <param name="article"></param>
        ProcessAction UpdateArticleProcessState(ProcessAction action, Article article)
        {
            ProcessAction myAction = action;
            string remark = DescriptionTextBox.Text;
            Processing p = ProcessingHelper.GetArticleProcess(article);
            p.Remark = remark;
            p.ApproveName = AccountHelper.GetAccount(AccountID,new string[]{"LastName"}).LastName;
            p.ApproveTitle = ApproveTitle.Text;
            p.ProcessAccountID = AccountID;

            p.ProcessState = ProcessHelper.GetNextProcessState(myAction, (Article)article);
            if (p.ProcessState == ProcessStates.EndAudit)
            {
                if (p.ProcessEndAction == ProcessEnding.SubmitSite)
                {
                    p.ProcessState = ProcessStates.FirstAudit;
                    myAction = ProcessAction.SubmitSite;
                    p.TargetSites = TargetSites;
                    p.TargetSiteID = TargetSiteID;
                }
                else if (p.ProcessEndAction == ProcessEnding.Start)
                    article.State = (int)ArticleStates.Started;
                else
                    article.State = (int)ArticleStates.Stopped;
            }

            if (p.FromOtherSite)
            {
                if (p.ProcessState == ProcessStates.Unaudit)
                {
                    if (myAction == ProcessAction.Restart) //退回编辑
                    {
                        p.TargetSiteID = p.SourceSiteID;
                        p.TargetSites = p.SourceSiteName;
                    }
                    else  //退回到上一站点的审核状态
                    {
                        ProcessHistory ph = ProcessHistoryHelper.GetLastArticleProcess(article);
                        p.CurLayerNO = ph.FromProcessState;
                        p.TargetSiteID = ph.SiteID;
                        p.TargetSites = ph.SiteName;
                    }
                }
            }

            p.ProcessDirection = ((int)myAction).ToString();
            ProcessingHelper.SaveFlowInfoToDB(article, p);

            return myAction;
        }

        /// <summary>
        /// 保存反馈审批信息
        /// </summary>
        /// <param name="action"></param>
        /// <param name="advice"></param>
        /// <returns></returns>
        ProcessAction UpdateAdviceProcessState(ProcessAction action, Advice advice)
        {
            ProcessAction myAction = action;
            string remark = DescriptionTextBox.Text;
            Processing p = ProcessingHelper.GetAdviceProcess(advice);
            p.Remark = remark;
            p.ApproveName = AccountHelper.GetAccount(AccountID,new string[]{"LastName"}).LastName;
            p.ApproveTitle = ApproveTitle.Text;
            p.ProcessAccountID = AccountID;

            p.ProcessState = ProcessHelper.GetAdviceNextProcess(myAction, advice);
            if (p.ProcessState == ProcessStates.EndAudit)
            {
                advice.State = (int)AdviceState.Finished;
            }

            p.ProcessDirection = ((int)myAction).ToString();
            ProcessingHelper.SaveAdviceFlowInfoToDB(advice, p);

            return myAction;
        }
  
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            HandleProcessAction();
        }

        void BindTemplate()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/Config/Dictionary/SignTemplate.xml"));

            XmlNodeList list=doc.SelectNodes("//item");
            ddlSignTemplate.Items.Add(new ListItem("--请选择--",""));
            foreach (XmlElement xe in list)
            {
                ddlSignTemplate.Items.Add(new ListItem(xe.GetAttribute("text"), xe.GetAttribute("name")));
            }
        }


    }
}
