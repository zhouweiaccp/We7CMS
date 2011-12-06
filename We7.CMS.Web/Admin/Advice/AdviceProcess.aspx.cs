using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using We7.CMS;
using We7.CMS.Controls;
using We7.CMS.Config;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
namespace We7.CMS.Web.Admin
{
    public partial class AdviceProcess : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadArticles();
            }
        }
 
          #region 获取数据

        void LoadArticles()
        {
            //取出所有待审批文章，逐一判断是否具有权限
            List<Advice> GetAllArticles = AdviceHelper.GetArticlesByAdviceTypeID(null, AdviceState.Checking, Pager.Begin, Pager.Count);
            if (GetAllArticles.Count == 0)
            {
                Messages.ShowMessage("您好，暂无待审核文章记录！");
                return;
            }
            List<Advice> adviceList = new List<Advice>();
            foreach (Advice advice in GetAllArticles)
            {
                try
                {
                    string curLayerNOText = ProcessHelper.GetCurLayerNOAdviceText(advice.ID);
                    if (curLayerNOText != "") //反馈当前审批进程：类似 Advice.FirstAudit
                    {
                        List<string> contents = AccountHelper.GetPermissionContents(AccountID, advice.TypeID);
                        if (contents.Contains(curLayerNOText))
                        {
                            adviceList.Add(advice);
                        }
                    }
                }
                catch
                { }
            }

            if (adviceList != null)
            { Pager.RecorderCount = adviceList.Count; }
            else
            {
                Pager.RecorderCount = 0;
            }
            if (Pager.Count < 0)
                Pager.PageIndex = 0;
            Pager.FreshMyself();
            if (Pager.Count <= 0)
            {
                DataGridView.DataSource = null;
                DataGridView.DataBind();
                return;
            }

            DataGridView.DataSource = adviceList.GetRange(Pager.Begin, Pager.Count);
            DataGridView.DataBind();
            
        }

          #endregion
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void SubmitBtn_Click(object sender, EventArgs e)
        //{
        //    List<string> list = new List<string>();
        //    list = GetIDs();
        //    int count = 0;
        //    if (list.Count > 0)
        //    {
        //        foreach (string id in list)
        //        {
        //            try
        //            {
        //                //ArticleProcessHelper.UpdateArticleProcess(id, AccountID);
        //                InsertArticleProcessHistory(id, "");
        //                Advice a = AdviceHelper.GetAdvice(id);
        //                string oldProcessState = a.ProcessState;
        //                int processState = Int32.Parse(a.ProcessState) + 1;
        //                string processStates = processState.ToString();
        //                AdviceHelper.UpdateAdviceProcess(id, processStates, AdviceState.Checking);

        //                Processing ap = ArticleProcessHelper.GetArticleProcess(id);
        //                GeneralConfigInfo si = GeneralConfigs.GetConfig();

        //               if (oldProcessState == GetChannelProcessLayerNO(id))
        //                {
        //                    //ArticleProcessHelper.DelArticleProcess(id);

        //                    if (si.ArticleAutoPublish == "true")
        //                    {
        //                        Advice advice = new Advice();
        //                        advice.ID = id;
        //                        advice.State = 1;
        //                        string[] fields = new string[] { "ID", "State" };
        //                        AdviceHelper.UpdateAdvice(advice,fields);
        //                    }
        //                }
        //                count++;
        //            }
        //            catch
        //            { }
        //        }
        //    }
        //    string message = string.Format("您已经成功审核{0}条记录", count);
        //    Messages.ShowMessage(message);
        //    LoadArticles();
        //}

        //void InsertArticleProcessHistory(string id ,string state)
        //{
        //    Processing ap = ArticleProcessHelper.GetArticleProcess(id);
        //    ProcessHistory aph = new ProcessHistory();
        //    aph.ObjectID = ap.ObjectID;
        //    if (state == "")
        //    {
        //        aph.ToProcessState = ap.CurLayerNO;
        //    }
        //    else
        //    {
        //        aph.ToProcessState = "-1";
        //    }
        //    aph.ProcessDirection = ap.ProcessDirection;
        //    aph.ProcessAccountID = ap.ProcessAccountID;
        //    aph.Remark = ap.Remark;
        //    aph.CreateDate = DateTime.Now;
        //    aph.UpdateDate = DateTime.Now;
        //    ArticleProcessHistoryHelper.InsertAdviceProcessHistory(aph);
        //}
       /// <summary>
       /// 退回
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        //protected void UntreadBtn_Click(object sender, EventArgs e)
        //{
        //    List<string> list = new List<string>();
        //    list = GetIDs();
        //    int count = 0;
        //    if (list.Count > 0)
        //    {
        //        foreach (string id in list)
        //        {
        //            try
        //            {
        //                Advice a = AdviceHelper.GetAdvice(id);
        //                int processState = Int32.Parse(a.ProcessState)-1;
        //                string processStates = processState.ToString();
        //                AdviceState state = AdviceState.Checking;
        //                if (processState == 0) state = AdviceState.All;
        //                AdviceHelper.UpdateAdviceProcess(id, processStates, state);

        //                Processing ap = new Processing();
        //                ap = ArticleProcessHelper.GetArticleProcess(id);
        //                if (ap.CurLayerNO == "0")
        //                {
        //                    InsertArticleProcessHistory(id,"-1");
        //                    //ArticleProcessHelper.DelArticleProcess(id);
        //                }
        //                else
        //                {
        //                    ArticleProcessHelper.UntreadArticleProcess(id, AccountID);
        //                    InsertArticleProcessHistory(id,"");
        //                }
        //                count++;
        //            }
        //            catch
        //            { }
        //        }
        //    }
        //    string message = string.Format("您已经成功退回{0}条记录", count);
        //    Messages.ShowMessage(message);
        //    LoadArticles();
        //}

        /// <summary>
        /// 审核状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetProcessState(string id)
        {
            Advice a = AdviceHelper.GetAdvice(id);
            Processing ap = ProcessHelper.GetAdviceProcess(a);
            string processText = "";
            if (ap != null)
                processText = ap.ProcessDirectionText + ap.ProcessText;
            return processText;
        }
        /// <summary>
        /// 绑定前台模型名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetAdviceText(string id)
        {
            if (id != null && id != "")
            {
                string typeID = AdviceHelper.GetAdvice(id).TypeID;
                if (typeID != null && typeID != "")
                {
                    return AdviceTypeHelper.GetAdviceType(typeID).Title.ToString();
                }
                else
                    return "";
            }
            else
                    return "";
        }
        List<string> GetIDs()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < DataGridView.Rows.Count; i++)
            {
                if (((CheckBox)DataGridView.Rows[i].FindControl("chkItem")).Checked)
                {
                    list.Add(((Label)(DataGridView.Rows[i].FindControl("lblID"))).Text);
                }
            }
            return list;
        }
        protected void Pager_Fired(object sender, EventArgs e)
        {
            LoadArticles();
        }

        string GetChannelProcessLayerNO(string id)
        {
            string adviceTypeID = AdviceHelper.GetAdvice(id).TypeID;
            AdviceType adviceType = AdviceTypeHelper.GetAdviceType(adviceTypeID);
            if (adviceType.FlowSeries != null)
                return adviceType.FlowSeries.ToString();
            else
                return "";
        }

        
    }
}
