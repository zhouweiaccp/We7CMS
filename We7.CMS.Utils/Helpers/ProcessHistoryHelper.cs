using System;
using System.Collections.Generic;
using System.Text;

using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using Thinkment.Data;
using We7.CMS.Common;
using System.Xml;
using System.Xml.Serialization;
namespace We7.CMS
{
    /// <summary>
    /// 审核历史业务类
    /// </summary>
    [Serializable]
    [Helper("We7.ArticleProcessHistoryHelper")]
    public class ProcessHistoryHelper:BaseHelper
    {

        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)(System.Web.HttpContext.Current.Application[HelperFactory.ApplicationID]); }
        }

        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        protected AdviceHelper AdviceHelper
        {
            get { return HelperFactory.GetHelper<AdviceHelper>(); }
        }

        public void InsertAdviceProcessHistory(ProcessHistory aph)
        {
            if (aph.ID == null)
            {
                aph.ID = We7Helper.CreateNewID();
            }
            Advice a = AdviceHelper.GetAdvice(aph.ObjectID);
            List<ProcessHistory> list = StrToList(a.FlowXml);
            aph.ItemNum = list.Count;
            list.Add(aph);
            a.FlowXml = ListToStr(list);
            AdviceHelper.UpdateAdvice(a, new string[] { "FlowXml" });
        }

        /// <summary>
        /// 新增文章处理审核历史记录
        /// </summary>
        /// <param name="aph"></param>
        /// <param name="article"></param>
        public void InsertArticleProcessHistory(ProcessHistory aph, Article article)
        {
            if (aph.ID == null)
                aph.ID = We7Helper.CreateNewID();

            List<ProcessHistory> list = StrToList(article.FlowXml);
            aph.ItemNum = list.Count;
            list.Add(aph);
            article.FlowXml = ListToStr(list);
            ArticleHelper.UpdateArticle(article, new string[] { "FlowXml" });
        }

        public void InsertArticleProcessHistory(ProcessHistory aph, string articleID)
        {
            Article a = ArticleHelper.GetArticle(articleID);
            InsertArticleProcessHistory(aph, a);
        }

        /// <summary>
        /// 根据文章ID获取审核历史信息
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public ProcessHistory[] GetArticleProcessHistorys(string articleID)
        {
            Article article = new Article();
            article.ID = articleID;
            Assistant.Select(article);
            List<ProcessHistory> list = StrToList(article.FlowXml);
            return list.ToArray();
        }

        /// <summary>
        /// 取得文章的上一审批节点信息
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public ProcessHistory GetLastArticleProcess(Article article)
        {
            List<ProcessHistory> list = StrToList(article.FlowXml);
            if (list != null && list.Count > 0)
                return list[list.Count - 1];
            else
                return null;
        }

        /// <summary>
        /// 根据反馈ID获取审核历史信息
        /// </summary>
        /// <param name="adviceID"></param>
        /// <returns></returns>
        public ProcessHistory[] GetAdviceProcessHistorys(string adviceID)
        {
            Advice a = AdviceHelper.GetAdvice(adviceID);
            List<ProcessHistory> list = StrToList(a.FlowXml);
            return list.ToArray();
        }

        /// <summary>
        /// 获取文章流转历史用于回执
        /// </summary>
        /// <param name="oldFlowXml"></param>
        /// <returns></returns>
        public ProcessHistory[] GetArticleFeedBackSite(string oldFlowXml)
        {
            List<ProcessHistory> list = StrToList(oldFlowXml);
            return list.ToArray();
        }

        /// <summary>
        /// 根据审核历史信息ID获取审核历史信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProcessHistory GetArticleProcessHistory(string articleID, string historyID)
        {
            Article article = ArticleHelper.GetArticle(articleID);
            List<ProcessHistory> list = StrToList(article.FlowXml);
            MyListHelper listHelper = new MyListHelper();
            listHelper.HistoryID = historyID;
            ProcessHistory aph = list.Find(listHelper.MatchByID);
            return aph;
        }

        /// <summary>
        /// 根据创建时间获取审核历史信息
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ProcessHistory GetProcessHistory(string articleID,DateTime date)
        {
            Article article = ArticleHelper.GetArticle(articleID);
            List<ProcessHistory> list = StrToList(article.FlowXml);
            MyListHelper listHelper = new MyListHelper();
            listHelper.date = date;
            ProcessHistory aph = list.Find(listHelper.MatchByDate);
            return aph;
        }

        /// <summary>
        /// 将字串转换为List对象
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public List<ProcessHistory> StrToList(string xml)
        {
            if (xml == null)
            {
                return new List<ProcessHistory>();
            }
            byte[] buffer = new byte[0];
            buffer = Convert.FromBase64String(xml);
            if (buffer == null || buffer.Length == 0)
            {
                return new List<ProcessHistory>();
            }
            List<ProcessHistory> list = (List<ProcessHistory>)We7Helper.BytesToObject(buffer);
            return list;
        }

        /// <summary>
        /// List对象转换为字串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string ListToStr(List<ProcessHistory> list)
        {
            byte[] buffer = We7Helper.ObjectToBytes(list);
            string xml = Convert.ToBase64String(buffer);
            return xml;
        }

        /// <summary>
        /// 根据审核历史信息ID获取审核历史信息
        /// </summary>
        /// <param name="adviceID"></param>
        /// <param name="historyID"></param>
        /// <returns></returns>
        public ProcessHistory GetAdviceProcessHistory(string adviceID, string historyID)
        {
            Advice a = AdviceHelper.GetAdvice(adviceID);
            List<ProcessHistory> list = StrToList(a.FlowXml);
            MyListHelper listHelper = new MyListHelper();
            listHelper.HistoryID = historyID;
            ProcessHistory aph = list.Find(listHelper.MatchByID);
            return aph;
        }

        class MyListHelper
        {
            public string HistoryID;
            public DateTime date;
            public bool MatchByID(ProcessHistory ph)
            {
                bool result = false;
                if (ph.ID == HistoryID)
                {
                    result = true;
                }

                return result;
            }

            public bool MatchByDate(ProcessHistory ph)
            {
                bool result = false;
                if (ph.CreateDate.Equals(date))
                {
                    result = true;
                }

                return result;
            }
        }

    }
}
