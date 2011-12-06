using System;
using System.Collections.Generic;
using System.Text;

using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using Thinkment.Data;
using System.Web;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// 反馈回复业务处理类
    /// </summary>
    [Serializable]
    [Helper("We7.AdviceReplyHelper")]
    public class AdviceReplyHelper:BaseHelper
    {
        /// <summary>
        /// 业务对象工厂
        /// </summary>
        private HelperFactory HelperFactory
        {
            get
            {
                return (HelperFactory)(HttpContext.Current.Application[HelperFactory.ApplicationID]);
            }
        }

        /// <summary>
        /// 反馈业务对象
        /// </summary>
        private AdviceHelper AdviceHelper
        {
            get
            {
                return HelperFactory.GetHelper<AdviceHelper>();
            }
        }
        /// <summary>
        /// 获取回复对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AdviceReply GetAdviceReply(string id)
        {
            AdviceReply ar = new AdviceReply();
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<AdviceReply> aList = Assistant.List<AdviceReply>(c, null);
            if (aList != null && aList.Count > 0)
            {
                ar = aList[0];
            }
            return ar;
        }

        /// <summary>
        /// 获取回复对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public AdviceReply GetAdviceReply(string id, string[] fields)
        {
            AdviceReply ar = new AdviceReply();
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<AdviceReply> aList = Assistant.List<AdviceReply>(c, null, 0, 0, fields);
            if (aList != null && aList.Count > 0)
            {
                ar = aList[0];
            }
            return ar;
        }

        /// <summary>
        /// 获取回复列表，分页
        /// </summary>
        /// <param name="adviceID"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<AdviceReply> GetAdviceReplys(string adviceID, int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<AdviceReply>(c, o, from, count);
        }

        /// <summary>
        /// 获取回复列表
        /// </summary>
        /// <param name="adviceID"></param>
        /// <returns></returns>
        public List<AdviceReply> GetAdviceReplys(string adviceID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<AdviceReply>(c, o);
        }
        /// <summary>
        /// 获取回复
        /// </summary>
        /// <param name="adviceID"></param>
        /// <returns></returns>
        public AdviceReply GetAdviceReplyByAdviceID(string adviceID)
        {
            //Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            //Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            //if (Assistant.Count<AdviceReply>(c) > 0)
            //{
            //    List<AdviceReply> ar = Assistant.List<AdviceReply>(c, null, 0, 1);
            //    return ar[0];
            //}
            //return null;

            List<AdviceReply> adviceReplys = GetAdviceReplys(adviceID);
            for (int i = 0; i < adviceReplys.Count; i++)
            {
                if (adviceReplys[i].Content != "" && adviceReplys[i].Content != null)
                {
                    return adviceReplys[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 获取回复数量
        /// </summary>
        /// <param name="adviceID"></param>
        /// <returns></returns>
        public int GetAdviceReplyCount(string adviceID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            return Assistant.Count<AdviceReply>(c);
        }

        /// <summary>
        /// 新增一个回复
        /// </summary>
        /// <param name="ar"></param>
        public void AddAdviceReply(AdviceReply ar)
        {
            ar.ID = We7Helper.CreateNewID();
            ar.CreateDate = DateTime.Now;
            Assistant.Insert(ar);
            UpdateAdviceCount(ar.AdviceID);
        }

        /// <summary>
        /// 更新反馈回复数
        /// </summary>
        /// <param name="adviceID"></param>
        public void UpdateAdviceCount(string adviceID)
        {
             Advice advice = AdviceHelper.GetAdvice(adviceID);
             advice.ReplyCount += 1;
             string[] fields = new string[] { "ReplyCount" };
             AdviceHelper.UpdateAdvice(advice,fields);
        }
        /// <summary>
        /// 删除一个回复
        /// </summary>
        /// <param name="id"></param>
        public void DeleteAdviceReply(string id)
        {
            AdviceReply ar = GetAdviceReply(id);
            Assistant.Delete(ar);
        }

        /// <summary>
        /// 删除一组回复
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteAdviceReply(List<string> ids)
        {
            foreach (string id in ids)
            {
                DeleteAdviceReply(id);
            }
        }

        /// <summary>
        /// 获取某个办理人所对应的所有的反馈信息 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<AdviceReply> GetAdviceByUserID(string userID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "UserID", userID);
            if (Assistant.Count<AdviceReply>(c) > 0)
            {
                List<AdviceReply> adviceReply = Assistant.List<AdviceReply>(c, null);
                return adviceReply;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 更新一个回复
        /// </summary>
        /// <param name="adviceReply"></param>
        /// <param name="fields"></param>
        public void UpdateReplyByAdviceID(AdviceReply adviceReply, string[] fields)
        {
            Assistant.Update(adviceReply, fields);
        }

    }
}
