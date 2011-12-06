using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Web.Caching;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using Thinkment.Data;
using System.Threading;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// 投票DAL工厂
    /// </summary>
    public class VoteFactory
    {
        public static IVoteHelper Instance
        {
            get
            {
                return new VoteHelper();
            }
        }
    }

    /// <summary>
    /// 投票业务类接口
    /// </summary>
    public interface IVoteHelper
    {
        /// <summary>
        /// 添加一条投票
        /// </summary>
        /// <param name="vote"></param>
        /// <returns></returns>
        bool AddVote(Vote vote);

        /// <summary>
        /// 添加一条投票及选项
        /// </summary>
        /// <param name="vote"></param>
        /// <param name="listEntrys"></param>
        /// <returns></returns>
        bool AddVote(Vote vote, List<VoteEntry> listEntrys);

        /// <summary>
        /// 添加一条投票选项
        /// </summary>
        /// <param name="voteEntry"></param>
        /// <returns></returns>
        bool AddVoteEntry(VoteEntry voteEntry);

        /// <summary>
        /// 删除一条投票（及选项）
        /// </summary>
        /// <param name="vote"></param>
        /// <returns></returns>
        bool DeleteVote(Vote vote);

        /// <summary>
        /// 删除一条投票（及选项）
        /// </summary>
        /// <param name="vote"></param>
        /// <returns></returns>
        bool DeleteVote(string ID);

        /// <summary>
        /// 删除一条投票选项
        /// </summary>
        /// <param name="vote"></param>
        /// <returns></returns>
        bool DeleteVoteEntry(VoteEntry voteEntry);

        /// <summary>
        /// 更新一条投票
        /// </summary>
        /// <param name="vote"></param>
        /// <returns></returns>
        bool UpdateVote(Vote vote);

        /// <summary>
        /// 更新一条投票选项
        /// </summary>
        /// <param name="voteEntry"></param>
        /// <returns></returns>
        bool UpdateVoteEntry(VoteEntry voteEntry);

        /// <summary>
        /// 提交对某一期的投票答案
        /// </summary>
        /// <param name="listAnswers"></param>
        /// <param name="flag">
        ///     -1:投票失败
        ///     -2:投票已过期
        ///     1:成功
        ///     0:您已经投过票了
        /// </param>
        /// <returns></returns>
        bool AddVoteAnswer(List<VoteAnswer> listAnswers, out int flag);

        /// <summary>
        /// 获取所有期投票
        /// </summary>
        /// <returns></returns>
        List<Vote> GetAllVotes();

        /// <summary>
        /// 获取所有未过期投票
        /// </summary>
        /// <returns></returns>
        List<Vote> GetAvailVotes();

        /// <summary>
        /// 获取一期投票通过ID
        /// </summary>
        /// <param name="voteID"></param>
        /// <returns></returns>
        Vote GetVoteByID(string voteID);

        /// <summary>
        /// 获取某一期投票通过标题
        /// </summary>
        /// <param name="voteTitle"></param>
        /// <returns></returns>
        Vote GetVoteByTitle(string voteTitle);

        /// <summary>
        /// 获取某一选项通过ID
        /// </summary>
        /// <param name="voteEntryID"></param>
        /// <returns></returns>
        VoteEntry GetVoteEntryByEntryID(string voteEntryID);

        /// <summary>
        /// 获取某一期的投票选项统计
        /// </summary>
        /// <param name="voteID"></param>
        /// <returns></returns>
        List<VoteAnswerStat> GetVoteAnswerStatByID(string voteID);

        /// <summary>
        /// 获取某一期投票的选项
        /// </summary>
        /// <param name="voteID"></param>
        /// <returns></returns>
        List<VoteEntry> GetVoteEntrysByID(string voteID);

        /// <summary>
        /// 获取某一期投票的选项答案
        /// </summary>
        /// <param name="voteID"></param>
        /// <returns></returns>
        List<VoteAnswer> GetVoteAnswersByID(string voteID);

        /// <summary>
        /// 获取某一期投票的选项统计
        /// </summary>
        /// <param name="voteID"></param>
        /// <returns></returns>
        List<VoteAnswerStat> GetVoteAnswerStatsByID(string voteID);


        /// <summary>
        /// 某用户是否已经投过某一期投票
        /// </summary>
        /// <param name="VoteID"></param>
        /// <param name="AccountID"></param>
        bool IsExistVotePeople(string VoteID, string AccountID);

        /// <summary>
        /// 某IP是否已经投过某一期投票
        /// </summary>
        /// <param name="VoteID"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        bool IsExistVoteIp(string VoteID, string IP);
    }

    /// <summary>
    /// 投票业务
    /// </summary>
    [Serializable]
    [Helper("We7.VoteHelper")]
    public class VoteHelper : BaseHelper, IVoteHelper
    {
        /// <summary>
        /// 添加一条投票
        /// </summary>
        /// <param name="vote"></param>
        #region bool AddVote(Vote vote)
        public bool AddVote(Vote vote)
        {
            try
            {
                vote.ID = We7Helper.CreateNewID();
                vote.AddDate = DateTime.Now;
                Assistant.Insert(vote);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 添加一条投票及选项(进行统计)
        /// </summary>
        /// <param name="vote"></param>
        /// <param name="listEntrys"></param>
        #region bool AddVote(Vote vote, List<VoteEntry> listEntrys)
        public bool AddVote(Vote vote, List<VoteEntry> listEntrys)
        {
            if (vote == null || listEntrys == null)
                return false;
            if (listEntrys.Count == 0)
                return false;

            try
            {
                vote.ID = We7Helper.CreateNewID();
                vote.VotePeoples = 0;
                vote.AddDate = DateTime.Now;
                Assistant.Insert(vote);

                for (int i = 0; i < listEntrys.Count; i++)
                {
                    listEntrys[i].VoteID = vote.ID;
                    Assistant.Insert(listEntrys[i]);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 添加一条投票选项
        /// </summary>
        /// <param name="voteEntry"></param>
        #region bool AddVoteEntry(VoteEntry voteEntry)
        public bool AddVoteEntry(VoteEntry voteEntry)
        {
            try
            {
                List<VoteEntry> listEntrys = GetVoteEntrysByID(voteEntry.VoteID);
                voteEntry.ID = We7Helper.CreateNewID();
                voteEntry.OrderID = (listEntrys == null ? 1 : listEntrys.Count + 1);
                Assistant.Insert(voteEntry);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 删除一条投票（及选项，回答，统计）
        /// </summary>
        /// <param name="vote"></param>
        #region bool DeleteVote(Vote vote)
        public bool DeleteVote(Vote vote)
        {
            try
            {
                string voteID = vote.ID;
                bool flag = Assistant.Delete(vote);
                if (flag)
                {
                    Criteria criteria = new Criteria(CriteriaType.Equals, "VoteID", voteID);
                    Assistant.DeleteList<VoteEntry>(criteria);

                    Assistant.DeleteList<VoteAnswer>(criteria);

                    Assistant.DeleteList<VoteAnswerStat>(criteria);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 删除一条投票（及选项，回答，统计）
        /// </summary>
        /// <param name="vote"></param>
        #region bool DeleteVote(string ID)
        public bool DeleteVote(string ID)
        {
            Criteria criteria = new Criteria(CriteriaType.Equals, "ID", ID);
            List<Vote> listVotes = Assistant.List<Vote>(criteria, null);
            if (listVotes != null && listVotes.Count > 0)
            {
                return DeleteVote(listVotes[0]);
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 删除一条投票选项
        /// </summary>
        /// <param name="voteEntry"></param>
        #region bool DeleteVoteEntry(VoteEntry voteEntry)
        public bool DeleteVoteEntry(VoteEntry voteEntry)
        {
            return Assistant.Delete(voteEntry);
        }
        #endregion

        /// <summary>
        /// 提交对某一期的投票答案
        /// </summary>
        /// <param name="listAnswers"></param>
        /// <param name="flag">
        ///     -1:投票失败
        ///     -2:投票已过期
        ///     1:成功
        ///     0:您已经投过票了
        /// </param>
        #region bool AddVoteAnswer(List<VoteAnswer> listAnswers,out int flag)
        public bool AddVoteAnswer(List<VoteAnswer> listAnswers, out int flag)
        {
            flag = -1;
            try
            {
                if (listAnswers != null && listAnswers.Count > 0)
                {
                    string voteID = listAnswers[0].VoteID;
                    Vote vote = GetVoteByID(voteID);

                    //是否有重复投票限制 && 已经投过票
                    if (!vote.IsCanRepeat && IsVoteAnswerExist(listAnswers[0]))
                    {
                        flag = 0;
                        return false;
                    }

                    //是否截止
                    if (vote != null && vote.EndDate > DateTime.Now)
                    {
                        //登录用AccountID
                        if (!string.IsNullOrEmpty(listAnswers[0].AccountID))
                        {
                            //检查当前用户的记录是否存在
                            if (!IsExistVotePeople(listAnswers[0].VoteID, listAnswers[0].AccountID))
                            {
                                vote.VotePeoples += 1;
                                UpdateVote(vote);
                            }
                        }
                        else
                        {//未登录用IP
                            if (!IsExistVoteIp(listAnswers[0].VoteID, listAnswers[0].AccountID))
                            {
                                vote.VotePeoples += 1;
                                UpdateVote(vote);
                            }
                        }

                        foreach (VoteAnswer voteAnswer in listAnswers)
                        {
                            voteAnswer.ID = We7Helper.CreateNewID();
                            Assistant.Insert(voteAnswer);

                            //获取投票的选项
                            VoteEntry voteEntry = GetVoteEntryByEntryID(voteAnswer.VoteEntryID);
                            //加一票统计
                            UpdateVoteAnswerStat(voteAnswer.VoteID, voteAnswer.VoteEntryID,
                                voteEntry != null ? voteEntry.EntryText : "", 1);
                        }

                        flag = 1;
                        return true;
                    }
                    else
                    {
                        flag = -2;
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                flag = -1;
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 更新一条投票
        /// </summary>
        /// <param name="vote"></param>
        #region bool UpdateVote(Vote vote)
        public bool UpdateVote(Vote vote)
        {
            try
            {
                Assistant.Update(vote);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 更新一条投票选项
        /// </summary>
        /// <param name="voteEntry"></param>
        #region bool UpdateVoteEntry(VoteEntry voteEntry)
        public bool UpdateVoteEntry(VoteEntry voteEntry)
        {
            try
            {
                Assistant.Update(voteEntry);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 某用户是否已经投过某一期投票
        /// </summary>
        /// <param name="VoteID"></param>
        /// <param name="AccountID"></param>
        #region bool IsExistVotePeople(string VoteID, string AccountID)
        public bool IsExistVotePeople(string VoteID, string AccountID)
        {
            Criteria criteria = new Criteria(CriteriaType.Equals, "VoteID", VoteID);
            criteria.Add(CriteriaType.Equals, "AccountID", AccountID);

            List<VoteAnswer> listAnswers = Assistant.List<VoteAnswer>(criteria, null);
            return (listAnswers != null && listAnswers.Count > 0);
        }
        #endregion

        /// <summary>
        /// 某IP是否已经投过某一期投票
        /// </summary>
        /// <param name="VoteID"></param>
        /// <param name="AccountID"></param>
        #region bool IsExistVoteIp(string VoteID, string IP)
        public bool IsExistVoteIp(string VoteID, string IP)
        {
            Criteria criteria = new Criteria(CriteriaType.Equals, "VoteID", VoteID);
            criteria.Add(CriteriaType.Equals, "IP", IP);

            List<VoteAnswer> listAnswers = Assistant.List<VoteAnswer>(criteria, null);
            return (listAnswers != null && listAnswers.Count > 0);
        }
        #endregion

        /// <summary>
        /// 获取所有投票
        /// </summary>
        #region List<Vote> GetAllVotes()
        public List<Vote> GetAllVotes()
        {
            List<Order> listOrder = new List<Order>();
            listOrder.Add(new Order("AddDate", OrderMode.Desc));

            Criteria criteria = new Criteria(CriteriaType.None);
            return Assistant.List<Vote>(criteria, listOrder.ToArray());
        }
        #endregion

        /// <summary>
        /// 获取所有未过期投票
        /// </summary>
        #region List<Vote> GetAvailVotes()
        public List<Vote> GetAvailVotes()
        {
            List<Order> listOrder = new List<Order>();
            listOrder.Add(new Order("AddDate", OrderMode.Desc));

            Criteria criteria = new Criteria(CriteriaType.MoreThan, "EndDate", DateTime.Now);
            return Assistant.List<Vote>(criteria, listOrder.ToArray());
        }
        #endregion

        /// <summary>
        /// 获取一期投票通过ID
        /// </summary>
        /// <param name="voteID"></param>
        /// <returns></returns>
        #region Vote GetVoteByID(string voteID)
        public Vote GetVoteByID(string voteID)
        {
            Criteria criteria = new Criteria(CriteriaType.Equals, "ID", voteID);
            List<Vote> listVotes = Assistant.List<Vote>(criteria, null);
            if (listVotes != null && listVotes.Count > 0)
                return listVotes[0];
            return null;
        }
        #endregion

        /// <summary>
        /// 获取某一期投票通过标题
        /// </summary>
        /// <param name="voteTitle"></param>
        #region Vote GetVoteByTitle(string voteTitle)
        public Vote GetVoteByTitle(string voteTitle)
        {
            Criteria criteria = new Criteria(CriteriaType.Equals, "Title", voteTitle);
            List<Vote> listVotes = Assistant.List<Vote>(criteria, null);
            if (listVotes != null && listVotes.Count > 0)
                return listVotes[0];
            return null;
        }
        #endregion

        /// <summary>
        /// 获取某一期的投票选项统计
        /// </summary>
        /// <param name="voteID"></param>
        #region List<VoteAnswerStat> GetVoteAnswerStatByID(string voteID)
        public List<VoteAnswerStat> GetVoteAnswerStatByID(string voteID)
        {
            Criteria criteria = new Criteria(CriteriaType.Equals, "VoteID", voteID);
            return Assistant.List<VoteAnswerStat>(criteria, null);
        }
        #endregion

        /// <summary>
        /// 获取某一期投票的选项
        /// </summary>
        /// <param name="voteID"></param>
        #region List<VoteEntry> GetVoteEntrysByID(string voteID)
        public List<VoteEntry> GetVoteEntrysByID(string voteID)
        {
            List<Order> listOrder = new List<Order>();
            listOrder.Add(new Order("OrderID", OrderMode.Asc));

            Criteria criteria = new Criteria(CriteriaType.Equals, "VoteID", voteID);
            return Assistant.List<VoteEntry>(criteria, listOrder.ToArray());
        }
        #endregion

        /// <summary>
        /// 获取某一选项
        /// </summary>
        /// <param name="voteEntryID"></param>
        #region VoteEntry GetVoteEntryByEntryID(string voteEntryID)
        public VoteEntry GetVoteEntryByEntryID(string voteEntryID)
        {
            Criteria criteria = new Criteria(CriteriaType.Equals, "ID", voteEntryID);
            List<VoteEntry> listVoteEntrys = Assistant.List<VoteEntry>(criteria, null);
            if (listVoteEntrys != null && listVoteEntrys.Count > 0)
                return listVoteEntrys[0];
            else
                return null;
        }
        #endregion

        /// <summary>
        /// 获取某一期投票的选项答案
        /// </summary>
        /// <param name="voteID"></param>
        #region List<VoteAnswer> GetVoteAnswersByID(string voteID)
        public List<VoteAnswer> GetVoteAnswersByID(string voteID)
        {
            Criteria criteria = new Criteria(CriteriaType.Equals, "VoteID", voteID);
            return Assistant.List<VoteAnswer>(criteria, null);
        }
        #endregion

        /// <summary>
        /// 获取某一期投票的选项统计
        /// </summary>
        /// <param name="voteID"></param>
        #region List<VoteAnswerStat> GetVoteAnswerStatsByID(string voteID)
        public List<VoteAnswerStat> GetVoteAnswerStatsByID(string voteID)
        {
            List<VoteEntry> listEntrys = GetVoteEntrysByID(voteID);
            if (listEntrys == null)
                return null;

            List<Order> listOrder = new List<Order>();
            listOrder.Add(new Order("OrderID", OrderMode.Asc));

            Criteria criteria = new Criteria(CriteriaType.Equals, "VoteID", voteID);
            List<VoteAnswerStat> listAnswerStats = Assistant.List<VoteAnswerStat>(criteria, listOrder.ToArray());
            int total = 0;
            //查漏            
            for (int i = 0; i < listEntrys.Count; i++)
            {
                VoteEntry entry = listEntrys[i];
                VoteAnswerStat stat = listAnswerStats.Find(p => p.VoteEntryID == entry.ID);
                if (stat == null)
                {
                    VoteAnswerStat statNew = new VoteAnswerStat();
                    statNew.VoteEntryID = entry.ID;
                    statNew.VoteID = entry.VoteID;
                    statNew.VoteEntrySum = 0;
                    statNew.VoteEntryText = entry.EntryText;
                    listAnswerStats.Insert(i, statNew);
                }
            }
            //统计总值
            foreach (VoteAnswerStat stat in listAnswerStats)
                total += stat.VoteEntrySum;
            for (int i = 0; i < listAnswerStats.Count; i++)
            {
                if (total > 0)
                    listAnswerStats[i].VoteEntrySumPercent =
                        (int)Math.Round((decimal)listAnswerStats[i].VoteEntrySum / (decimal)total * 170, 0); //百分比                    
                else
                    listAnswerStats[i].VoteEntrySumPercent = 0;
            }
            return listAnswerStats;
        }
        #endregion

        /// <summary>
        /// 是否投票回答已经存在
        /// </summary>
        /// <param name="voteAnswer"></param>
        #region bool IsVoteAnswerExist(VoteAnswer voteAnswer)
        protected bool IsVoteAnswerExist(VoteAnswer voteAnswer)
        {
            Criteria criteria = new Criteria(CriteriaType.Equals, "VoteID", voteAnswer.VoteID);
            if (!string.IsNullOrEmpty(voteAnswer.AccountID))
                criteria.Add(CriteriaType.Equals, "AccountID", voteAnswer.AccountID);
            else
                criteria.Add(CriteriaType.Equals, "IP", voteAnswer.IP);

            List<VoteAnswer> listAnswers = Assistant.List<VoteAnswer>(criteria, null);
            return (listAnswers != null && listAnswers.Count > 0);
        }
        #endregion

        /// <summary>
        /// 更新回答选项统计（）
        /// </summary>
        /// <param name="voteID"></param>
        /// <param name="voteEntryID"></param>
        /// <param name="voteEntryText"></param>
        /// <param name="sum">加几票</param>
        #region bool UpdateVoteAnswerStat(string voteID,string voteEntryID,string voteEntryText,int sum)
        protected bool UpdateVoteAnswerStat(string voteID, string voteEntryID, string voteEntryText, int sum)
        {
            try
            {
                Criteria criteria = new Criteria(CriteriaType.Equals, "VoteID", voteID);
                criteria.Add(CriteriaType.Equals, "VoteEntryID", voteEntryID);
                List<VoteAnswerStat> listAnswerStats = Assistant.List<VoteAnswerStat>(criteria, null);

                VoteEntry entry = GetVoteEntryByEntryID(voteEntryID);

                if (listAnswerStats != null && listAnswerStats.Count > 0)
                {
                    //进行更新
                    listAnswerStats[0].VoteEntrySum += sum;
                    Assistant.Update(listAnswerStats[0]);
                }
                else
                {
                    //新增一个
                    VoteAnswerStat voteAnswerStat = new VoteAnswerStat();
                    voteAnswerStat.ID = We7Helper.CreateNewID();
                    voteAnswerStat.VoteEntryID = voteEntryID;
                    voteAnswerStat.VoteID = voteID;
                    voteAnswerStat.VoteEntrySum = sum;
                    voteAnswerStat.VoteEntryText = voteEntryText;
                    voteAnswerStat.OrderID = entry.OrderID;
                    Assistant.Insert(voteAnswerStat);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
