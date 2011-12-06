using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;

using Thinkment.Data;
using System.Xml;
using System.IO;
using System.Web;
using System.Xml.Schema;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using We7.CMS.Config;
using OpenPOP;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using We7.Model.Core;
using System.Data;
using We7.CMS.Accounts;

namespace We7.CMS
{
    public interface IAdviceHelper
    {
        /// <summary>
        /// 添加反馈
        /// </summary>
        /// <param name="advice">反馈内容</param>
        void AddAdvice(AdviceInfo advice);

        /// <summary>
        /// 删除反馈
        /// </summary>
        /// <param name="id">反馈ID</param>
        void DeleteAdvice(string id);

        /// <summary>
        /// 删除转发信息
        /// </summary>
        /// <param name="id">转发信息ID号</param>
        void DeleteTransfer(string id);

        /// <summary>
        /// 根据ID号取得反馈实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AdviceInfo GetAdvice(string id);

        /// <summary>
        /// 取得转发记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns></returns>
        AdviceTransfer GetTransfer(string id);

        /// <summary>
        /// 更新反馈信息
        /// </summary>
        /// <param name="advice">反馈实体</param>
        void UpdateAdvice(AdviceInfo advice);

        /// <summary>
        /// 更新反馈状态
        /// </summary>
        /// <param name="id">反馈信息ID</param>
        /// <param name="state">反馈状态</param>
        void UpdateAdviceState(string id, int state);

        /// <summary>
        /// 转发反馈
        /// </summary>
        /// <param name="id">转发反馈ID</param>
        /// <param name="typeID">反馈类型ID</param>
        /// <param name="suggest">转发意见</param>
        void TransferAdvice(string id, string typeID, string suggest);

        /// <summary>
        /// 不受理反馈
        /// </summary>
        /// <param name="id">反馈ID</param>
        /// <param name="content">不受理原因</param>
        void RefuseAdvice(string id, string content);

        /// <summary>
        /// 受理反馈
        /// </summary>
        /// <param name="id">反馈ID</param>
        void AcceptAdvice(string id);

        /// <summary>
        /// 受理反馈
        /// </summary>
        /// <param name="id">反馈ID号</param>
        /// <param name="priority">反馈优先级</param>
        /// <param name="isShow">是否前台显示</param>
        void AcceptAdvice(string id, int priority, bool isShow);

        /// <summary>
        /// 设置反馈优先级
        /// </summary>
        /// <param name="id">反馈ID号</param>
        /// <param name="priority">反馈优先级(0:普通,1:必办,2:催办)</param>
        void SetAdvicePriority(string id, int priority);

        /// <summary>
        /// 前台显示反馈
        /// </summary>
        /// <param name="id">反馈ID</param>
        void ShowAdvice(string id);

        /// <summary>
        /// 前台隐藏反馈
        /// </summary>
        /// <param name="id">反馈ID</param>
        void HideAdvice(string id);

        /// <summary>
        /// 反馈置顶
        /// </summary>
        /// <param name="id"></param>
        void SetTop(string id);

        /// <summary>
        /// 取消置顶
        /// </summary>
        /// <param name="id"></param>
        void CancelTop(string id);

        /// <summary>
        /// 回复反馈信息
        /// </summary>
        /// <param name="id">反馈ID</param>
        /// <param name="content">反馈内容</param>
        void ReplyAdvice(string id, string content);

        /// <summary>
        /// 根据用户查询提定类型的反馈内容
        /// </summary>
        /// <param name="typeID">反馈类型ID</param>
        /// <param name="state">数据类型(0：未受理，1：不受理，2，受理中，3，转办中，9：已办结,10：全部)</param>
        /// <returns></returns>
        List<AdviceInfo> QueryAdvice(string typeID, int states);

        /// <summary>
        /// 查询指定反馈类型，指定用户下的，指定状态的记录条数
        /// </summary>
        /// <param name="typeID">反馈类型</param>
        /// <param name="state">数据类型(0：未受理，1：不受理，2，受理中，3，转办中，9：已办结,10：全部)</param>
        /// <returns></returns>
        int QueryAdviceCount(string typeID, int state);

        /// <summary>
        /// 查询指定反馈类型，指定用户下的，指定状态的记录条数
        /// </summary>
        /// <param name="typeID">反馈类型</param>
        /// <param name="state">数据类型(0：未受理，1：不受理，2，受理中，3，转办中，9：已办结,10：全部)</param>
        /// <param name="queryInfo">附加查询信息</param>
        /// <returns></returns>
        int QueryAdviceCount(string typeID, int state, Dictionary<string, object> queryInfo);

        /// <summary>
        /// 分页查询定义反馈型下，指定用户的指定状态的信息
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="state">数据类型(0：未受理，1：不受理，2，受理中，3，转办中，9：已办结,10：全部)</param>
        /// <param name="from">起始记录</param>
        /// <param name="count">查询的条数</param>
        /// <returns></returns>
        List<AdviceInfo> QueryAdvice(string typeID, int state, int from, int count);

        /// <summary>
        /// 分页查询定义反馈型下，指定用户的指定状态的信息
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="state">数据类型(0：未受理，1：不受理，2，受理中，3，转办中，9：已办结,10：全部)</param>
        /// <param name="queryInfo">查询信息</param>
        /// <param name="from">起始记录</param>
        /// <param name="count">查询的条数</param>
        /// <returns></returns>
        List<AdviceInfo> QueryAdvice(string typeID, int state, Dictionary<string, object> queryInfo, int from, int count);

        /// <summary>
        /// 查找指定反馈类型下的转发信息
        /// </summary>
        /// <param name="typeID">反馈类型</param>
        /// <returns></returns>
        List<AdviceTransfer> QueryTransfers(string typeID, int from, int count);

        /// <summary>
        /// 查询指定反反馈下的反馈内容
        /// </summary>
        /// <param name="adviceID">反馈ID</param>
        /// <returns></returns>
        List<AdviceTransfer> QueryTransferHistories(string adviceID);

        /// <summary>
        /// 根据把馈ID查询反馈回信息信息
        /// </summary>
        /// <param name="adviceID"></param>
        /// <returns></returns>
        List<AdviceReplyInfo> QueryReplies(string adviceID);

        /// <summary>
        /// 根据把馈ID查询反馈回信息记录类
        /// </summary>
        /// <param name="adviceID"></param>
        /// <returns></returns>
        int QueryRepliesCount(string adviceID);

        /// <summary>
        /// 查询指定反馈类型下的回复信息
        /// </summary>
        /// <param name="adviceID">反馈ID</param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<AdviceReplyInfo> QueryReplies(string adviceID, int from, int count);

        /// <summary>
        /// 取得指用户拥有的指定反馈类型的权限
        /// </summary>
        /// <param name="typeID">反馈类型ID</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        List<string> GetPermissions(string typeID, string userID);

        /// <summary>
        /// 根据反馈类型ID，取得授权信息
        /// </summary>
        /// <param name="typeID">反馈类型ID</param>
        /// <returns></returns>
        List<AdviceAuth> GetAdviceAuth(string typeID);

        /// <summary>
        /// 根据反馈类型ID，以及授权类型取得授权信息
        /// </summary>
        /// <param name="typeID">反馈类型ID</param>
        /// <param name="authType">授权类型(0：用户，1：角色,2:部门)</param>
        /// <returns></returns>
        List<AdviceAuth> GetAdviceAuth(string typeID, string authType);

        /// <summary>
        /// 根据反馈类型ID取得反馈类型信息
        /// </summary>
        /// <param name="typeID">反馈类型ID</param>
        /// <returns></returns>
        AdviceType GetAdviceType(string typeID);

        /// <summary>
        /// 返回当前反馈记录下的相关反馈类型
        /// </summary>
        /// <param name="adviceID">反馈ID</param>
        /// <returns></returns>
        List<AdviceType> GetRelatedAdviceTypes(string adviceID);


    }

    /// <summary>
    /// 反馈工厂
    /// </summary>
    public class AdviceFactory
    {
        /// <summary>
        /// 创建反馈业务类
        /// </summary>
        /// <returns></returns>
        public static IAdviceHelper Create()
        {
            return HelperFactory.Instance.GetHelper<AdviceHelper2>();
        }
    }

    [Helper("We7.AdviceHelper2")]
    public class AdviceHelper2 : BaseHelper, IAdviceHelper, IObjectClickHelper
    {
        /// <summary>
        /// 添加反馈
        /// </summary>
        /// <param name="advice">反馈内容</param>
        public void AddAdvice(AdviceInfo advice)
        {
            if (String.IsNullOrEmpty(advice.ID))
            {
                advice.ID = We7Helper.CreateNewID();
            }

            if (advice.Created == default(DateTime))
            {
                advice.Created = DateTime.Now;
            }

            advice.SN = CreateSN();

            Assistant.Insert(advice);
        }

        /// <summary>
        /// 删除反馈
        /// </summary>
        /// <param name="id">反馈ID</param>
        public void DeleteAdvice(string id)
        {
            AdviceInfo adivce = GetAdvice(id);
            if (adivce != null)
            {
                Assistant.Delete(adivce);
            }
        }

        /// <summary>
        /// 删除转发信息
        /// </summary>
        /// <param name="id">转发信息ID号</param>
        public void DeleteTransfer(string id)
        {
            AdviceTransfer tran = GetTransfer(id);
            if (tran != null)
            {
                Assistant.Delete(tran);
            }
        }

        /// <summary>
        /// 根据ID号取得反馈实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AdviceInfo GetAdvice(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<AdviceInfo> list = Assistant.List<AdviceInfo>(c, null);
            return list != null && list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 取得转发记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns></returns>
        public AdviceTransfer GetTransfer(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<AdviceTransfer> list = Assistant.List<AdviceTransfer>(c, null);
            return list != null && list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 更新反馈信息
        /// </summary>
        /// <param name="advice">反馈实体</param>
        public void UpdateAdvice(AdviceInfo advice)
        {
            Assistant.Update(advice);
        }

        /// <summary>
        /// 更新反馈状态
        /// </summary>
        /// <param name="id">反馈信息ID</param>
        /// <param name="state">反馈状态</param>
        public void UpdateAdviceState(string id, int state)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.State = state;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// 不受理反馈
        /// </summary>
        /// <param name="id">反馈ID</param>
        /// <param name="content">不受理原因</param>
        public void RefuseAdvice(string id, string content)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
                IConnection ic = Assistant.GetConnections()[db];
                ic.IsTransaction = true;

                try
                {
                    AdviceReplyInfo reply = new AdviceReplyInfo();
                    reply.ID = We7Helper.CreateNewID();
                    reply.AdviceID = id;
                    reply.Content = content;
                    reply.Created = DateTime.Now;
                    reply.Title = "不受理当前信息";
                    reply.UserID = Security.CurrentAccountID;

                    Assistant.Insert(reply);

                    advice.State = 1;
                    Assistant.Update(advice);
                    ic.Commit();
                }
                catch (Exception ex)
                {
                    ic.Rollback();
                    throw ex;
                }
                finally
                {
                    ic.Dispose();
                }


            }
        }

        /// <summary>
        /// 转发反馈
        /// </summary>
        /// <param name="id">转发反馈ID</param>
        /// <param name="typeID">反馈类型ID</param>
        /// <param name="suggest">转发意见</param>
        public void TransferAdvice(string id, string typeID, string suggest)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
                IConnection ic = Assistant.GetConnections()[db];
                ic.IsTransaction = true;

                try
                {

                    AdviceTransfer tran = new AdviceTransfer();
                    tran.ID = We7Helper.CreateNewID();
                    tran.State = 0;
                    tran.Suggest = suggest;
                    tran.FromTypeID = advice.TypeID;
                    tran.ToTypeID = typeID;
                    tran.Created = DateTime.Now;
                    tran.AdviceID = id;
                    tran.UserID = Security.CurrentAccountID;
                    Assistant.Insert(tran); //插件转发信息

                    advice.State = 3;
                    advice.TypeID = typeID;
                    Assistant.Update(advice);//更新反馈状态

                    ic.Commit();
                }
                catch (Exception ex)
                {
                    ic.Rollback();
                    throw ex;
                }
                finally
                {
                    ic.Dispose();
                }
            }
        }

        /// <summary>
        /// 受理反馈
        /// </summary>
        /// <param name="id">反馈ID</param>
        public void AcceptAdvice(string id)
        {
            AcceptAdvice(id, 0, true);
        }

        /// <summary>
        /// 受理反馈
        /// </summary>
        /// <param name="id">反馈ID号</param>
        /// <param name="priority">反馈优先级</param>
        /// <param name="isShow">是否前台显示</param>
        public void AcceptAdvice(string id, int priority, bool isShow)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.Priority = priority;
                advice.IsShow = isShow ? 1 : 0;
                advice.State = 2;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// 设置反馈优先级
        /// </summary>
        /// <param name="id">反馈ID号</param>
        /// <param name="priority">反馈优先级(0:普通,1:必办,2:催办)</param>
        public void SetAdvicePriority(string id, int priority)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.Priority = priority;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// 前台显示反馈
        /// </summary>
        /// <param name="id">反馈ID</param>
        public void ShowAdvice(string id)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.IsShow = 1;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// 前台隐藏反馈
        /// </summary>
        /// <param name="id">反馈ID</param>
        public void HideAdvice(string id)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.IsShow = 0;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// 反馈置顶
        /// </summary>
        /// <param name="id"></param>
        public void SetTop(string id)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.IsTop = 1;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// 取消置顶
        /// </summary>
        /// <param name="id"></param>
        public void CancelTop(string id)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.IsTop = 0;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// 回复反馈信息
        /// </summary>
        /// <param name="id">反馈ID</param>
        /// <param name="content">反馈内容</param>
        public void ReplyAdvice(string id, string content)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
                IConnection ic = Assistant.GetConnections()[db];
                ic.IsTransaction = true;

                try
                {
                    AdviceReplyInfo reply = new AdviceReplyInfo();
                    reply.ID = We7Helper.CreateNewID();
                    reply.AdviceID = id;
                    reply.Content = content;
                    reply.Created = DateTime.Now;
                    reply.Title = "受理信息";
                    reply.UserID = Security.CurrentAccountID;

                    Assistant.Insert(reply);

                    advice.State = 9;
                    Assistant.Update(advice);
                    ic.Commit();
                }
                catch (Exception ex)
                {
                    ic.Rollback();
                    throw ex;
                }
                finally
                {
                    ic.Dispose();
                }
            }
        }

        /// <summary>
        /// 根据用户查询提定类型的反馈内容
        /// </summary>
        /// <param name="typeID">反馈类型ID</param>
        /// <param name="state">数据类型(0：未受理，1：不受理，2，受理中，3，转办中，9：已办结)</param>
        /// <returns></returns>
        public List<AdviceInfo> QueryAdvice(string typeID, int state)
        {
            Order[] os = new Order[] { new Order("Priority", OrderMode.Desc), new Order("Created", OrderMode.Desc) };

            if (state == 3) //查找转办的数据
            {
                Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
                List<AdviceTransfer> trans = Assistant.List<AdviceTransfer>(c, new Order[] { new Order("Created", OrderMode.Desc) });
                c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (AdviceTransfer tran in trans)
                {
                    c.AddOr(CriteriaType.Equals, "ID", tran.AdviceID);
                }
                return c.Criterias.Count > 0 ? Assistant.List<AdviceInfo>(c, os) : new List<AdviceInfo>();
            }
            else
            {
                return Assistant.List<AdviceInfo>(CreateAdviceQueryCriteria(typeID, state), os);
            }
        }

        /// <summary>
        /// 查询指定反馈类型，指定用户下的，指定状态的记录条数
        /// </summary>
        /// <param name="typeID">反馈类型</param>
        /// <param name="state">数据类型(0：未受理，1：不受理，2，受理中，3，转办中，9：已办结,10：全部)</param>
        /// <returns></returns>
        public int QueryAdviceCount(string typeID, int state)
        {
            if (state == 3) //查找转办的数据
            {
                Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
                return Assistant.Count<AdviceTransfer>(c);
            }
            else
            {
                return Assistant.Count<AdviceInfo>(CreateAdviceQueryCriteria(typeID, state));
            }
        }

        /// <summary>
        /// 查询指定反馈类型，指定用户下的，指定状态的记录条数
        /// </summary>
        /// <param name="typeID">反馈类型</param>
        /// <param name="state">数据类型(0：未受理，1：不受理，2，受理中，3，转办中，9：已办结,10：全部)</param>
        /// <param name="queryInfo">附加查询信息</param>
        /// <returns></returns>
        public int QueryAdviceCount(string typeID, int state, Dictionary<string, object> queryInfo)
        {
            if (state == 3) //查找转办的数据
            {
                Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
                AppendQueryInfo(c, queryInfo);
                return Assistant.Count<AdviceTransfer>(c);
            }
            else
            {
                Criteria c = CreateAdviceQueryCriteria(typeID, state);
                AppendQueryInfo(c, queryInfo);
                return Assistant.Count<AdviceInfo>(c);
            }
        }

        /// <summary>
        /// 分页查询定义反馈型下，指定用户的指定状态的信息
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="state">数据类型(0：未受理，1：不受理，2，受理中，3，转办中，9：已办结)</param>
        /// <param name="from">起始记录</param>
        /// <param name="count">查询的条数</param>
        /// <returns></returns>
        public List<AdviceInfo> QueryAdvice(string typeID, int state, int from, int count)
        {
            Order[] os = new Order[] { new Order("Priority", OrderMode.Desc), new Order("Created", OrderMode.Desc) };

            if (state == 3) //查找转办的数据
            {
                Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
                List<AdviceTransfer> trans = Assistant.List<AdviceTransfer>(c, new Order[] { new Order("Created", OrderMode.Desc) }, from, count);
                c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (AdviceTransfer tran in trans)
                {
                    c.AddOr(CriteriaType.Equals, "ID", tran.AdviceID);
                }
                return c.Criterias.Count > 0 ? Assistant.List<AdviceInfo>(c, os) : new List<AdviceInfo>();
            }
            else
            {
                return Assistant.List<AdviceInfo>(CreateAdviceQueryCriteria(typeID, state), os, from, count);
            }
        }

        /// <summary>
        /// 分页查询定义反馈型下，指定用户的指定状态的信息
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="state">数据类型(0：未受理，1：不受理，2，受理中，3，转办中，9：已办结,10：全部)</param>
        /// <param name="queryInfo">查询信息</param>
        /// <param name="from">起始记录</param>
        /// <param name="count">查询的条数</param>
        /// <returns></returns>
        public List<AdviceInfo> QueryAdvice(string typeID, int state, Dictionary<string, object> queryInfo, int from, int count)
        {
            Order[] os = new Order[] { new Order("Priority", OrderMode.Desc), new Order("Created", OrderMode.Desc) };

            if (state == 3) //查找转办的数据
            {
                Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
                List<AdviceTransfer> trans = Assistant.List<AdviceTransfer>(c, new Order[] { new Order("Created", OrderMode.Desc) }, from, count);
                c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (AdviceTransfer tran in trans)
                {
                    c.AddOr(CriteriaType.Equals, "ID", tran.AdviceID);
                }
                if (queryInfo != null)
                {
                    AppendQueryInfo(c, queryInfo);
                }
                return c.Criterias.Count > 0 ? Assistant.List<AdviceInfo>(c, os) : new List<AdviceInfo>();
            }
            else
            {
                Criteria c = CreateAdviceQueryCriteria(typeID, state);
                if (queryInfo != null)
                {
                    AppendQueryInfo(c, queryInfo);
                }
                return Assistant.List<AdviceInfo>(c, os, from, count);
            }
        }

        /// <summary>
        /// 查找指定反馈类型下的转发信息
        /// </summary>
        /// <param name="typeID">反馈类型</param>
        /// <returns></returns>
        public List<AdviceTransfer> QueryTransfers(string typeID, int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
            List<AdviceTransfer> trans = Assistant.List<AdviceTransfer>(c, new Order[] { new Order("Created", OrderMode.Desc) }, from, count);
            if (trans != null && trans.Count > 0)
            {
                c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (AdviceTransfer tran in trans)
                {
                    c.AddOr(CriteriaType.Equals, "ID", tran.AdviceID);
                }
                List<AdviceInfo> advices = Assistant.List<AdviceInfo>(c, null);
                foreach (AdviceTransfer tran in trans)
                {
                    foreach (AdviceInfo advice in advices)
                    {
                        if (tran.AdviceID == advice.ID)
                            tran.Advice = advice;
                    }
                }
            }
            return trans;
        }

        /// <summary>
        /// 查询指定反反馈下的反馈内容
        /// </summary>
        /// <param name="adviceID">反馈ID</param>
        /// <returns></returns>
        public List<AdviceTransfer> QueryTransferHistories(string adviceID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            return Assistant.List<AdviceTransfer>(c, new Order[] { new Order("Created") }) ?? new List<AdviceTransfer>();
        }

        /// <summary>
        /// 根据把馈ID查询反馈回信息信息
        /// </summary>
        /// <param name="adviceID">反馈ID</param>
        /// <returns></returns>
        public List<AdviceReplyInfo> QueryReplies(string adviceID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            return Assistant.List<AdviceReplyInfo>(c, new Order[] { new Order("Created", OrderMode.Asc) });
        }

        /// <summary>
        /// 查询指定反馈下的记录数
        /// </summary>
        /// <param name="adviceID">反馈ID</param>
        /// <returns></returns>
        public int QueryRepliesCount(string adviceID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            return Assistant.Count<AdviceReplyInfo>(c);
        }

        /// <summary>
        /// 查询指定反馈类型下的回复信息
        /// </summary>
        /// <param name="adviceID">反馈ID</param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<AdviceReplyInfo> QueryReplies(string adviceID, int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            return Assistant.List<AdviceReplyInfo>(c, new Order[] { new Order("Created", OrderMode.Asc) }, from, count);
        }


        /// <summary>
        /// 取得指用户拥有的指定反馈类型的权限
        /// </summary>
        /// <param name="typeID">反馈类型ID</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public List<string> GetPermissions(string typeID, string userID)
        {
            IAccountHelper accountHelper = AccountFactory.CreateInstance();
            if (userID == We7Helper.EmptyGUID)
            {
                return new List<string>() { "Advice.Accept", "Advice.Read", "Advice.Admin", "Advice.Handle" };
            }
            else
            {
                return accountHelper.GetPermissionContents(userID, typeID);
            }
        }

        /// <summary>
        /// 根据反馈类型ID，取得授权信息
        /// </summary>
        /// <param name="typeID">反馈类型ID</param>
        /// <returns></returns>
        public List<AdviceAuth> GetAdviceAuth(string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            return Assistant.List<AdviceAuth>(c, null);
        }

        /// <summary>
        /// 根据反馈类型ID，以及授权类型取得授权信息
        /// </summary>
        /// <param name="typeID">反馈类型ID</param>
        /// <param name="authType">授权类型(0：用户，1：角色,2:部门)</param>
        /// <returns></returns>
        public List<AdviceAuth> GetAdviceAuth(string typeID, string authType)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            c.Add(CriteriaType.Equals, "AuthType", authType);
            return Assistant.List<AdviceAuth>(c, null);
        }

        /// <summary>
        /// 根据反馈类型ID取得反馈类型信息
        /// </summary>
        /// <param name="typeID">反馈类型ID</param>
        /// <returns></returns>
        public AdviceType GetAdviceType(string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", typeID);
            List<AdviceType> list = Assistant.List<AdviceType>(c, null);
            return list != null && list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 返回当前反馈记录下的相关反馈类型
        /// </summary>
        /// <param name="adviceID">反馈ID</param>
        /// <returns></returns>
        public List<AdviceType> GetRelatedAdviceTypes(string adviceID)
        {
            AdviceInfo advice = GetAdvice(adviceID);
            if (advice != null)
            {
                Criteria c = new Criteria(CriteriaType.Equals, "ModelName", advice.ModelName);
                c.Add(CriteriaType.NotEquals, "ID", advice.TypeID);
                return Assistant.List<AdviceType>(c, new Order[] { new Order("CreateDate", OrderMode.Desc) });
            }
            return new List<AdviceType>();
        }


        /// <summary>
        /// 取得转化信息
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        private List<AdviceTransfer> GetTransfers(string typeID)
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Mode = CriteriaMode.Or;
            c.Add(CriteriaType.Equals, "FromTypeID", typeID);

            Criteria subC = new Criteria(CriteriaType.None);
            subC.Add(CriteriaType.Equals, "ToTypeID", typeID);
            subC.Add(CriteriaType.Equals, "State", 0);
            c.Criterias.Add(subC);

            return Assistant.List<AdviceTransfer>(c, null);
        }

        /// <summary>
        /// 创建反馈查询条件
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="state">数据类型(0：未受理，1：不受理，2，受理中，3，转办中，9：已办结,10：全部)</param>
        /// <returns></returns>
        private Criteria CreateAdviceQueryCriteria(string typeID, int state)
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Add(CriteriaType.Equals, "TypeID", typeID);

            if (state == 0) //未处理反馈
            {
                c.Add(CriteriaType.Equals, "State", 0);
            }
            else if (state == 1) //不处理
            {
                c.Add(CriteriaType.Equals, "State", 1);
            }
            else if (state == 2) //受理中的类型
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;
                subC.AddOr(CriteriaType.Equals, "State", 2);
                subC.AddOr(CriteriaType.Equals, "State", 3);
                c.Criterias.Add(subC);
            }
            else if (state == 9) //已办结
            {
                c.Add(CriteriaType.Equals, "State", 9);
            }
            else if (state == 10) //全部
            {
            }

            return c;
        }

        private void AppendQueryInfo(Criteria c, Dictionary<string, object> queryInfo)
        {
            foreach (string key in queryInfo.Keys)
            {
                if (key.ToLower() == "title")
                {
                    c.Add(CriteriaType.Like, "Title", "%" + queryInfo[key] + "%");
                }

                if (key.ToLower() == "user")
                {
                    c.Add(CriteriaType.Like, "Name", "%" + queryInfo[key] + "%");
                }

                if (key.ToLower() == "relationid")
                {
                    c.Add(CriteriaType.Equals, "RelationID", "" + queryInfo[key] + "");
                }
            }
        }

        /// <summary>
        /// 创建反馈SN
        /// </summary>
        private string CreateSN()
        {
            Criteria c = new Criteria(CriteriaType.MoreThanEquals, "Created", new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            int count = Assistant.Count<AdviceInfo>(c);
            string sn = String.Empty;
            if (count > 0)
            {
                List<AdviceInfo> list = Assistant.List<AdviceInfo>(c, new Order[] { new Order("SN", OrderMode.Desc) }, 0, 1);
                if (list[0].SN.Length == 12)
                {
                    string s = list[0].SN.Substring(list[0].SN.Length - 4, 4);
                    long l;
                    if (long.TryParse(s, out l))
                    {

                        sn = DateTime.Now.ToString("yyyyMMdd") + (++l).ToString().PadLeft(4, '0');
                    }
                }
            }
            if (String.IsNullOrEmpty(sn))
            {
                sn = DateTime.Now.ToString("yyyyMMdd") + "0001";
            }
            return sn;
        }

        #region IObjectClickHelper 成员

        public void UpdateClicks(string modelName, string id, Dictionary<string, int> dictClickReport)
        {
            AdviceInfo targetObject = GetAdvice(id);
            if (targetObject != null)
            {
                targetObject.DayClicks = dictClickReport["日点击量"];
                targetObject.YesterdayClicks = dictClickReport["昨日点击量"];
                targetObject.WeekClicks = dictClickReport["周点击量"];
                targetObject.MonthClicks = dictClickReport["月点击量"];
                targetObject.QuarterClicks = dictClickReport["季点击量"];
                targetObject.YearClicks = dictClickReport["年点击量"];
                targetObject.Clicks = dictClickReport["总点击量"];
                UpdateAdvice(targetObject);
            }
        }

        #endregion
    }




    #region 过去的

    /// <summary>
    /// 反馈业务类
    /// </summary>
    [Serializable]
    [Helper("We7.AdviceHelper")]
    public partial class AdviceHelper : BaseHelper
    {
        #region HelperFactory
        /// <summary>
        /// 业务助手工厂
        /// </summary>
        private HelperFactory HelperFactory
        {
            get
            {
                return (HelperFactory)(HttpContext.Current.Application[HelperFactory.ApplicationID]);
            }
        }
        /// <summary>
        /// 权限业务助手
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }
        /// <summary>
        /// 咨询投诉类型业务助手
        /// </summary>
        private AdviceTypeHelper AdviceTypeHelper
        {
            get
            {
                return HelperFactory.GetHelper<AdviceTypeHelper>();
            }
        }
        /// <summary>
        /// 反馈回复业务助手
        /// </summary>
        private AdviceReplyHelper AdviceReplyHelper
        {
            get
            {
                return HelperFactory.GetHelper<AdviceReplyHelper>();
            }
        }

        /// <summary>
        /// 审核信息业务助手
        /// </summary>
        private ProcessingHelper ProcessingHelper
        {
            get
            {
                return HelperFactory.GetHelper<ProcessingHelper>();
            }
        }

        private ProcessHistoryHelper ProcessHistoryHelper
        {
            get
            {
                return HelperFactory.GetHelper<ProcessHistoryHelper>();
            }
        }

        #endregion

        #region Should Deleted
        /// <summary>
        /// XmlSwitcherMode 的摘要说明。
        /// </summary>
        public enum AdviceMode
        {
            /// <summary>
            /// 添加模式。
            /// </summary>
            Add,
            /// <summary>
            /// 查看模式。
            /// </summary>
            Browse,
            /// <summary>
            /// 修改模式。
            /// </summary>
            Modify
        }

        private AdviceMode appMode;
        /// <summary>
        /// 当前的应用模式：添加、浏览、修改
        /// </summary>
        public AdviceMode AppMode
        {
            get { return appMode; }
            set { appMode = value; }
        }

        private string dataXml;
        /// <summary>
        /// 如果是浏览与修改模式，此内容应该是带有值的XML字串；
        /// 如果是添加模式，此内容应该是模板XML字串。
        /// </summary>
        public string DataXml
        {
            get
            {
                return dataXml;
            }
            set
            {
                if (value == null || value == "")
                    throw new Exception("要修改或查看的 XML 为空，请检查！");
                dataXml = value;
            }
        }

        private Hashtable ctrls;
        /// <summary>
        /// 生成控件的集合
        /// </summary>
        public Hashtable Ctrls
        {
            get { return ctrls; }
            set { ctrls = value; }
        }

        private bool isXmlValid = true;


        public AdviceHelper()
        {
            ctrls = new Hashtable();
        }

        /// <summary>
        /// 依据所赋初始化界面控件
        /// </summary>
        public void InitControl(ref Control ctrl)
        {
            ctrl.Controls.Clear();
            //try
            //{
            //    ValidateXmlData(dataXml, false);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            // 创建容器表格
            Table tblContainer = new Table();
            tblContainer.Attributes["id"] = "personalForm";
            tblContainer.EnableViewState = true;
            ctrl.Controls.Add(tblContainer);
            //tblContainer.GridLines = GridLines.Both;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dataXml);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("fw", doc.DocumentElement.NamespaceURI);

            // 生成控件
            XmlNodeList itemNodes = doc.SelectNodes("/fw:Document/fw:Items/fw:Item", nsmgr);
            TableRow itemRow = null;
            TableCell itemCell = null;
            string itemLabel;
            ctrls.Clear();
            foreach (XmlNode itemNode in itemNodes)
            {
                itemRow = new TableRow();
                itemRow.EnableViewState = true;
                tblContainer.Rows.Add(itemRow);

                //创建Label控件
                itemCell = new TableCell();
                itemCell.EnableViewState = true;
                itemCell.CssClass = "formTitle";
                //itemCell.Wrap = false;
                itemLabel = itemNode.SelectSingleNode("fw:Label", nsmgr).InnerText;
                itemCell.Text = itemLabel;
                itemRow.Cells.Add(itemCell);

                WebControl itemCtrl = null;
                XmlNode ctrlNode = itemNode.SelectSingleNode("fw:Control", nsmgr);
                string ctrlName = null;
                XmlNodeList valueNodes = itemNode.SelectNodes("fw:Content/fw:Value", nsmgr);

                //生成单项控件
                ctrlName = ctrlNode.Attributes["Type"].Value;
                Assembly asm = Assembly.GetAssembly(typeof(WebControl));
                string nameSpace = typeof(WebControl).Namespace;
                string controlFullName = nameSpace + "." + ctrlName;
                itemCtrl = (WebControl)asm.CreateInstance(controlFullName);
                itemCtrl.EnableViewState = true;
                itemCtrl.ID = "MyCtrl" + ctrls.Count;
                //itemCtrl.Width = Unit.Pixel(250);

                //生成 ListControl 的项
                if (itemCtrl is ListControl)
                {
                    XmlNodeList listItemNodes = itemNode.SelectNodes("fw:Content/fw:ListItem", nsmgr);
                    ListControl listCtrl = (ListControl)itemCtrl;
                    listCtrl.EnableViewState = true;
                    foreach (XmlNode listItemNode in listItemNodes)
                    {
                        listCtrl.Items.Add(listItemNode.InnerText);
                    }
                }

                //如果是修改或查看模式则需要给ListControl赋值
                if (appMode == AdviceMode.Modify || appMode == AdviceMode.Browse)
                {
                    if (itemCtrl is ListControl)
                    {
                        ListControl listCtrl = (ListControl)itemCtrl;
                        foreach (XmlNode valueNode in valueNodes)
                            listCtrl.Items.FindByValue(valueNode.InnerText).Selected = true;
                    }

                    else if (ctrlName == "TextBox")
                        ((TextBox)itemCtrl).Text = valueNodes[0].InnerText;
                }

                //设置每个控件的属性
                PropertyInfo ctrlProp;
                foreach (XmlAttribute ctrlAttr in ctrlNode.Attributes)
                {
                    if ((ctrlProp = itemCtrl.GetType().GetProperty(ctrlAttr.Name)) == null)
                    {
                        if (ctrlAttr.Name != "Type")
                        {
                            itemCtrl.Attributes.Add(ctrlAttr.Name, ctrlAttr.Value);
                        }
                        continue;
                    }

                    // 对枚举类型赋值
                    if (ctrlProp.PropertyType.IsEnum)
                    {
                        System.Enum enumPropValue = (System.Enum)System.Enum.Parse(ctrlProp.PropertyType, ctrlAttr.Value);
                        ctrlProp.SetValue(itemCtrl, enumPropValue, null);
                        continue;
                    }

                     //如果该属性的类型具有 “Parse(string) ”方法，例如 Width，Height，则对其赋值
                    else if (ctrlProp.PropertyType.GetMethod("Parse", new Type[] { typeof(string) }) != null)
                    {
                        MethodInfo parseMethod = ctrlProp.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });
                        object classValue = parseMethod.Invoke(null, new object[] { ctrlAttr.Value });
                        ctrlProp.SetValue(itemCtrl, classValue, null);
                        continue;
                    }

                    //对值类型赋值
                    else if (ctrlProp.PropertyType.IsValueType)
                    {
                        ctrlProp.SetValue(itemCtrl, Convert.ChangeType(ctrlAttr.Value, ctrlProp.PropertyType), null);
                        continue;
                    }
                }

                //如果是查看模式将控件赋只读
                if (appMode == AdviceMode.Browse)
                { itemCtrl.Enabled = false; }

                itemCell = new TableCell();
                itemCell.CssClass = "formValue";
                //itemCell.Wrap = false;
                itemRow.Cells.Add(itemCell);
                itemCell.Controls.Add(itemCtrl);

                //创建Additional控件
                itemCell = new TableCell();
                //itemCell.Wrap = false;
                string itemLabel1 = itemNode.SelectSingleNode("fw:Additional", nsmgr).InnerText;
                itemCell.Text = itemLabel1;
                itemCell.CssClass = "formExtend";
                itemRow.Cells.Add(itemCell);

                // 将刚才创建的项目的控件存储下来，以便提交时获取控件的值
                //if (AppMode == AdviceMode.Add || appMode == AdviceMode.Modify)
                ctrls.Add(itemLabel, itemCtrl);
            }
        }

        /// <summary>
        /// 读取生成控件的赋值
        /// 此操作注意错误捕捉！
        /// </summary>
        /// <param name="xmlTemplate">空XML模板字串</param>
        /// <returns>返回带值的XML模板</returns>
        public string GetControlsValue(string xmlTemplate)
        {
            if (xmlTemplate.Trim() == string.Empty)
            {
                throw new Exception("模板XML字串为空！");
            }

            //if (!ValidateXmlData(xmlTemplate,false))
            //{
            //    throw new Exception("模板XML字串不是XML或格式错误！");
            //}

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlTemplate);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("fw", doc.DocumentElement.NamespaceURI);

            foreach (string itemLabel in ctrls.Keys)
            {
                //得到与 itemLabel 对应的 Content 节点
                XmlNode labelNode = doc.SelectSingleNode("/fw:Document/fw:Items/fw:Item/fw:Label[.='" + itemLabel + "'] ", nsmgr);
                XmlNode contentNode = labelNode.SelectSingleNode("../fw:Content", nsmgr);
                WebControl itemCtrl = (WebControl)ctrls[itemLabel];

                //删除 Content/Value 节点
                XmlNodeList selectedItemNodes = contentNode.SelectNodes("fw:Value", nsmgr);
                foreach (XmlNode oldNode in selectedItemNodes)
                    contentNode.RemoveChild(oldNode);

                // 读取控件的值，存到 XmlDocument 中
                if (itemCtrl is ListControl)
                { // 列表控件
                    ListControl listCtrl = (ListControl)itemCtrl;
                    foreach (ListItem item in listCtrl.Items)
                    {
                        if (item.Selected)
                        {
                            XmlNode valueNode = doc.CreateNode(XmlNodeType.Element, "Value", doc.DocumentElement.NamespaceURI);
                            valueNode.InnerText = item.Value;
                            contentNode.PrependChild(valueNode);
                        }
                    }
                }
                else if (itemCtrl is TextBox)
                { // TextBox
                    TextBox tbCtrl = (TextBox)itemCtrl;
                    XmlNode valueNode = doc.CreateNode(XmlNodeType.Element, "Value", nsmgr.LookupNamespace("fw"));
                    valueNode.InnerText = tbCtrl.Text;
                    contentNode.PrependChild(valueNode);
                }

            }

            return doc.OuterXml;
        }

        #endregion

        /// <summary>
        /// 根据ID获取一个反馈实体对象
        /// </summary>
        /// <param name="id">反馈实体ID</param>
        /// <returns></returns>
        public Advice GetAdvice(string id)
        {
            Advice a = new Advice();
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Advice> aList = Assistant.List<Advice>(c, null);
            if (aList != null && aList.Count > 0)
            {
                a = aList[0];
            }
            return a;
        }

        /// <summary>
        /// 查找此反馈类型存在
        /// </summary>
        /// <param name="id">反馈实体ID</param>
        /// <returns></returns>
        public bool Exist(string id)
        {
            bool exist = false;
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            if (Assistant.Count<Advice>(c) > 0)
            {
                exist = true;
            }
            return exist;
        }

        /// <summary>
        /// 根据特定条件获取一个反馈实体对象
        /// </summary>
        /// <param name="id">反馈实体ID</param>
        /// <param name="fields">对象字段集合</param>
        /// <returns></returns>
        public Advice GetAdvice(string id, string[] fields)
        {
            Advice a = new Advice();
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Advice> aList = Assistant.List<Advice>(c, null, 0, 0, fields);
            if (aList != null && aList.Count > 0)
            {
                a = aList[0];
            }
            return a;
        }

        /// <summary>
        /// 获取咨询投诉列表，分页
        /// </summary>
        /// <param name="from">从第几条记录开始</param>
        /// <param name="count">返回的记录条数</param>
        /// <param name="deptid">回复ID</param>
        /// <returns></returns>
        public List<Advice> GetAdvices(int from, int count, string deptid)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ReplyDepID", deptid);
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<Advice>(c, o, from, count);
        }

        /// <summary>
        /// 获取咨询投诉列表
        /// </summary>
        /// <param name="c">查询条件</param>
        /// <param name="from">从第几条记录开始</param>
        /// <param name="count">返回的记录条数</param>
        /// <returns></returns>
        public List<Advice> GetAdvices(Criteria c, int from, int count)
        {
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<Advice>(c, o, from, count);
        }

        /// <summary>
        /// 根据条件获取咨询投诉列表
        /// </summary>
        /// <param name="c">查询条件</param>
        /// <param name="from">从第几条记录开始</param>
        /// <param name="count">返回的记录条数</param>
        /// <param name="typeID">类型ID</param>
        /// <returns></returns>
        public List<Advice> GetAdvices(Criteria c, int from, int count, string typeID)
        {
            if (c == null)
                c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            else
                c.Criterias.Add(new Criteria(CriteriaType.Equals, "TypeID", typeID));
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<Advice>(c, o, from, count);
        }

        /// <summary>
        /// 前端控件调用：IsShow >0 
        /// </summary>
        /// <param name="from">从第几条记录开始</param>
        /// <param name="count">返回的记录条数</param>
        /// <param name="typeID">类型ID</param>
        /// <returns></returns>
        public List<Advice> GetAdvicesByType(int from, int count, string typeID)
        {
            Criteria c = new Criteria(CriteriaType.MoreThan, "IsShow", 0);
            if (typeID != null && typeID.Length > 0)
            {
                c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            }

            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<Advice>(c, o, from, count);

        }

        /// <summary>
        /// 获取咨询投诉数量
        /// </summary>
        /// <param name="c">查询条件</param>
        /// <returns></returns>
        public int GetAdviceCount(Criteria c)
        {
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        ///  根据类型获取咨询投诉数量
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <returns></returns>
        public int GetAdviceCounts(string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        /// 根据条件获取咨询投诉数量
        /// </summary>
        /// <param name="c">查询条件</param>
        /// <param name="deptid">回复ID</param>
        /// <returns></returns>
        public int GetAdviceCount(Criteria c, string deptid)
        {
            if (c == null)
                c = new Criteria(CriteriaType.None);
            c.Criterias.Add(new Criteria(CriteriaType.Equals, "ReplyDepID", deptid));
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        /// 通过类型ID获取咨询投诉数量
        /// </summary>
        /// <param name="typeid">类型ID</param>
        /// <returns></returns>
        public int GetAdviceCountByType(string typeid)
        {
            Criteria c = new Criteria(CriteriaType.MoreThan, "IsShow", 0);
            if (typeid == null || typeid.Length == 0)
            {
                return Assistant.Count<Advice>(c);
            }
            else
            {
                c.Criterias.Add(new Criteria(CriteriaType.Equals, "TypeID", typeid));
                return Assistant.Count<Advice>(c);
            }
        }

        /// <summary>
        /// 获取咨询投诉列表
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <returns></returns>
        public List<Advice> GetAdvices(string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            return Assistant.List<Advice>(c, null);
        }

        /// <summary>
        /// 获取咨询投诉列表
        /// </summary>
        /// <returns></returns>
        public List<Advice> GetAdvices()
        {
            return Assistant.List<Advice>(null, null);
        }

        /// <summary>
        /// 新增一个咨询投诉
        /// </summary>
        /// <param name="a"></param>
        /// 
        public Advice AddAdvice(Advice a)
        {
            a.ID = String.IsNullOrEmpty(a.ID) ? We7Helper.CreateNewID() : a.ID;
            a.CreateDate = DateTime.Now;
            Assistant.Insert(a);
            return a;
        }

        /// <summary>
        /// 删除一个咨询投诉
        /// </summary>
        /// <param name="id">反馈实体ID</param>
        public void DeleteAdvice(string id)
        {
            Advice a = GetAdvice(id);
            Assistant.Delete(a);
        }

        /// <summary>
        /// 删除一组咨询投诉
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteAdvice(List<string> ids)
        {
            foreach (string id in ids)
            {
                DeleteAdvice(id);
            }
        }

        /// <summary>
        /// 更新一个咨询投诉
        /// </summary>
        /// <param name="advice"></param>
        /// <param name="fields">对象字段集合</param>
        public void UpdateAdvice(Advice advice, string[] fields)
        {
            if (advice != null)
                Assistant.Update(advice, fields);
        }

        /// <summary>
        /// 通过用户ID获取咨询投诉数
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <param name="typeID">类型ID</param>
        /// <returns></returns>
        public int GetAdviceCountByAccountID(string accountID, string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "UserID", accountID);
            c.Criterias.Add(new Criteria(CriteriaType.Equals, "TypeID", typeID));
            return Assistant.Count<Advice>(c);

        }

        /// <summary>
        ///  通过用户ID获取咨询投诉列表
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <param name="from">从第几条记录开始</param>
        /// <param name="count">返回的记录条数</param>
        /// <param name="typeID">类型ID</param>
        /// <returns></returns>
        public List<Advice> GetAdvicesByAccountID(string accountID, int from, int count, string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "UserID", accountID);
            c.Criterias.Add(new Criteria(CriteriaType.Equals, "TypeID", typeID));
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            if (Assistant.Count<Advice>(c) > 0)
            {
                return Assistant.List<Advice>(c, o, from, count);
            }
            else
            {
                return null;
            }
        }

        ////<summary>
        ////依据ID获取当前属性内容
        ////</summary>
        ////<param name="ID"></param>
        ////<returns>返回属性XML字串，未找到则返回string.Empty</returns>
        public string GetAdviceModel(string Advice)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", Advice);
            if (Assistant.Count<Advice>(c) <= 0)
            {
                return string.Empty;
            }
            string xml = Assistant.List<Advice>(c, null)[0].ModelXml;
            if (xml != null)
            {
                return xml;
            }
            return string.Empty;
        }

        /// <summary>
        /// 保存属性XML
        /// </summary>
        /// <param name="xmlData">带有数据的XML字串</param>
        /// <param name="id">反馈实体ID</param>
        public void SaveArticleModel(string xmlData, string adviceTypeID, string title)
        {
            //Criteria c = new Criteria(CriteriaType.Equals, "ID", adviceTypeID);
            try
            {
                Criteria c = new Criteria(CriteriaType.Equals, "ID", adviceTypeID);
                if (Assistant.Count<AdviceType>(c) <= 0)
                {
                    throw new Exception("留言信息类型已经不存在或数据已经被破坏！");
                }
                Advice advice = new Advice();
                advice.ID = We7Helper.CreateNewID();
                advice.Title = title;
                advice.TypeID = adviceTypeID;
                advice.Updated = DateTime.Now;
                advice.ModelXml = xmlData;
                Assistant.Insert(advice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  创建咨询投诉的sn
        /// </summary>
        /// <returns></returns>
        public long CreateArticleSN()
        {
            long maxSn = 0;
            Criteria c = new Criteria(CriteriaType.MoreThan, "SN", 0);
            long totalHave = Assistant.Count<Article>(c);
            long totalAll = Assistant.Count<Article>(null);
            if (totalAll > totalHave)
            {
                Order[] orders = new Order[] { new Order("Updated", OrderMode.Asc) };
                List<Advice> articles = Assistant.List<Advice>(null, orders);
                foreach (Advice a in articles)
                {
                    if (a.SN > maxSn) maxSn = a.SN;
                }

                foreach (Advice a in articles)
                {
                    if (a.SN <= 0)
                    {
                        a.SN = maxSn++;
                        UpdateAdvice(a, new string[] { "SN" });
                    }
                }
            }
            else
            {
                Order[] orders = new Order[] { new Order("SN", OrderMode.Desc) };
                if (Assistant.Count<Advice>(null) > 0)
                {
                    List<Advice> articles = Assistant.List<Advice>(null, orders, 0, 1);
                    if (articles.Count > 0)
                        maxSn = articles[0].SN;
                }
                else
                    maxSn = 0;
            }
            return maxSn + 1;
        }

        /// <summary>
        /// 根据查询类获得反馈数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int QueryAdviceCountByAll(AdviceQuery query)
        {
            Criteria c = CreateCriteriaByAll(query);
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        /// 根据查询类生成Criteria
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Criteria CreateCriteriaByAll(AdviceQuery query)
        {
            Criteria c = new Criteria(CriteriaType.None);

            if (query.State != 0)
            {
                c.Add(CriteriaType.Equals, "State", query.State);
            }
            else
            {
                c.Add(CriteriaType.NotEquals, "State", 999);//用999获取所有状态值数据
            }

            if (query.State == (int)AdviceState.WaitHandle && !We7Helper.IsEmptyID(query.AccountID))
            {
                c.Add(CriteriaType.Equals, "ToOtherHandleUserID", query.AccountID);
                //Criteria cri = GetAdviceIDByReplyUserID(query.AccountID);
                //if (cri != null)
                //{
                //    c.Criterias.Add(cri);
                //}
            }
            if (query.NotState > 0)
            {
                c.Add(CriteriaType.NotEquals, "State", query.NotState);
            }
            if (query.NotEnumState > 0)
            {
                c.Add(CriteriaType.NotEquals, "EnumState", query.NotEnumState);
            }

            if (query.AdviceTypeID != null && query.AdviceTypeID != "")
            {
                c.Add(CriteriaType.Equals, "TypeID", query.AdviceTypeID);
            }

            if (query.AccountID != null && query.AccountID != "")
            {
                Criteria cr = CreateSubCriteriaByAccount(query.AccountID, query.AdviceTypeID);
                if (cr != null)
                    c.Criterias.Add(cr);
            }

            if (query.Title != null && query.Title != "")
            {
                c.Add(CriteriaType.Like, "Title", "%" + query.Title.Trim() + "%");
            }


            if (query.IsShow != 9999)
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;

                //强制显示至前台
                string display = StateMgr.ConvertEnumToStr(EnumLibrary.AdviceDisplay.DisplayFront);
                Criteria displayC = new Criteria(CriteriaType.Equals, "EnumState", display);
                displayC.Adorn = Adorns.Substring;
                displayC.Start = EnumLibrary.Position[(int)EnumLibrary.Business.AdviceDisplay];
                displayC.Length = EnumLibrary.PlaceLenth;
                subC.Criterias.Add(displayC);

                Criteria sub2c = new Criteria(CriteriaType.None);
                display = StateMgr.ConvertEnumToStr(EnumLibrary.AdviceDisplay.UnDisplayFront);
                displayC = new Criteria(CriteriaType.NotEquals, "EnumState", display);
                displayC.Adorn = Adorns.Substring;
                displayC.Start = EnumLibrary.Position[(int)EnumLibrary.Business.AdviceDisplay];
                displayC.Length = EnumLibrary.PlaceLenth;
                sub2c.Mode = CriteriaMode.And;
                sub2c.Add(CriteriaType.Equals, "IsShow", query.IsShow);
                sub2c.Criterias.Add(displayC);

                subC.Criterias.Add(sub2c);

                c.Criterias.Add(subC);
            }

            if (query.SN != 0 && query.SN > 0)
            {
                c.Add(CriteriaType.Equals, "SN", query.SN);
            }

            if (query.MyQueryPwd != null && query.MyQueryPwd != "")
            {
                c.Add(CriteriaType.Equals, "MyQueryPwd", query.MyQueryPwd);
            }

            if (query.StartCreated != null && query.StartCreated != DateTime.MinValue)
            {
                c.Add(CriteriaType.MoreThanEquals, "CreateDate", query.StartCreated);
                c.Add(CriteriaType.LessThan, "CreateDate", query.EndCreated);
            }
            if (query.AdviceInfoType != null && query.AdviceInfoType != "")
            {
                c.Add(CriteriaType.Equals, "AdviceInfoType", query.AdviceInfoType);
            }
            if (query.MustHandle > 0)
            {
                c.Add(CriteriaType.Equals, "MustHandle", query.MustHandle);
            }

            if (query.Name != null && query.Name != "")
            {
                c.Add(CriteriaType.Like, "Name", "%" + query.Name.Trim() + "%");
            }

            if (query.Phone != null && query.Phone != "")
            {
                c.Add(CriteriaType.Like, "Phone", "%" + query.Phone.Trim() + "%");
            }
            if (query.Email != null && query.Email != "")
            {
                c.Add(CriteriaType.Like, "Email", "%" + query.Email.Trim() + "%");
            }
            if (query.Fax != null && query.Fax != "")
            {
                c.Add(CriteriaType.Like, "Fax", "%" + query.Fax.Trim() + "%");
            }
            if (query.Address != null && query.Address != "")
            {
                c.Add(CriteriaType.Like, "Address", "%" + query.Address.Trim() + "%");
            }
            if (query.Content != null && query.Content != "")
            {
                c.Add(CriteriaType.Like, "Content", "%" + query.Content.Trim() + "%");
            }
            if (query.AdviceTag != null && query.AdviceTag != "")
            {
                c.Add(CriteriaType.Equals, "AdviceTag", query.AdviceTag);
            }

            return c;
        }

        /// <summary>
        /// 办理人不同权限时所拥有的不同状态的反馈信息
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <param name="adviceTypeID"></param>
        /// <returns></returns>
        Criteria CreateSubCriteriaByAccount(string accountID, string adviceTypeID)
        {
            if (accountID != We7Helper.EmptyGUID)
            {
                List<string> allOwners = AccountHelper.GetRolesOfAccount(accountID);
                allOwners.Add(accountID);
                List<string> objList = new List<string>();
                objList.Add(adviceTypeID);
                List<Permission> permission = AccountHelper.GetPermissions(allOwners, objList);
                if (permission != null && permission.Count > 0)
                {
                    Criteria states = new Criteria(CriteriaType.None);
                    states.Mode = CriteriaMode.Or;

                    int stateInt = 0;
                    for (int i = 0; i < permission.Count; i++)
                    {
                        switch (permission[i].Content)
                        {
                            case "Advice.FirstAudit":
                                stateInt = (int)ProcessStates.FirstAudit;
                                states.Add(CriteriaType.Equals, "ProcessState", stateInt);
                                break;
                            case "Advice.SecondAudit":
                                stateInt = (int)ProcessStates.SecondAudit;
                                states.Add(CriteriaType.Equals, "ProcessState", stateInt);
                                break;
                            case "Advice.ThirdAudit":
                                stateInt = (int)ProcessStates.ThirdAudit;
                                states.Add(CriteriaType.Equals, "ProcessState", stateInt);
                                break;
                            case "Advice.Admin":
                                stateInt = (int)AdviceState.All;
                                states.Add(CriteriaType.Equals, "State", stateInt);
                                break;
                            case "Advice.Handle":
                                stateInt = (int)AdviceState.WaitHandle;
                                states.Add(CriteriaType.Equals, "State", stateInt);
                                break;
                            case "Advice.Accept":
                                stateInt = (int)AdviceState.WaitAccept;
                                states.Add(CriteriaType.Equals, "State", stateInt);
                                break;
                            default:
                                break;
                        }
                    }
                    return states;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加咨询投诉
        /// </summary>
        /// <param name="oldList"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        ArrayList StatesAdd(ArrayList oldList, object value)
        {
            if (!oldList.Contains(value))
            {
                oldList.Add(value);
            }
            return oldList;
        }

        /// <summary>
        /// 通过用户ID获取咨询投诉ID
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <returns></returns>
        public Criteria GetAdviceIDByReplyUserID(string accountID)
        {
            List<AdviceReply> adviceReplyList = AdviceReplyHelper.GetAdviceByUserID(accountID);
            Criteria c = new Criteria(CriteriaType.None);
            c.Mode = CriteriaMode.Or;
            if (adviceReplyList != null)
            {
                for (int i = 0; i < adviceReplyList.Count; i++)
                {
                    c.Add(CriteriaType.Equals, "ID", adviceReplyList[i].AdviceID);
                }
                return c;
            }
            else
                return null;
        }

        /// <summary>
        /// 根据查询类查询反馈数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="from">从第几条记录开始</param>
        /// <param name="count">返回的记录条数</param>
        /// <returns></returns>
        public List<Advice> GetAdviceByQuery(AdviceQuery query, int from, int count)
        {
            Criteria c = CreateCriteriaByAll(query);
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<Advice>(c, o, from, count);
        }

        /// <summary>
        /// 根据查询信息获取反馈数
        /// </summary>
        /// <param name="query">查询信息对象</param>
        /// <returns>查询到的反馈信息数目</returns>
        public int GetAdviceCount(AdviceQuery query)
        {
            Criteria c = CreateCriteriaByAll(query);
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        ///根据ID更改状态 
        /// </summary>
        /// <param name="id">反馈实体ID</param>
        /// <param name="enumState"></param>
        public void UpdateAdviceType(string id, int state)
        {
            Advice a = GetAdvice(id);
            a.State = state;
            a.Updated = DateTime.Now;
            Assistant.Update(a, new string[] { "State", "Updated" });
        }

        /// <summary>
        /// 根据ID更改流程状态
        /// </summary>
        /// <param name="id">反馈实体ID</param>
        /// <param name="processState"></param>
        public void UpdateAdviceProcessState(string id, string processState)
        {
            Advice a = GetAdvice(id);
            a.ProcessState = processState;
            a.Updated = DateTime.Now;
            Assistant.Update(a, new string[] { "ProcessState", "Updated" });
        }

        /// <summary>
        /// 更新一个咨询投诉记录
        /// </summary>
        /// <param name="id">反馈实体ID</param>
        /// <param name="ProcessState">流转状态</param>
        /// <param name="state">反馈状态</param>
        public void UpdateAdviceProcess(string id, string ProcessState, AdviceState state)
        {
            Advice a = GetAdvice(id);
            a.ProcessState = ProcessState;
            a.State = (int)state;
            a.Updated = DateTime.Now;
            Assistant.Update(a, new string[] { "Updated", "ProcessState", "State" });
        }

        /// <summary>
        /// 取得模型下符合状态的反馈
        /// </summary>
        /// <param name="adviceIDlist"></param>
        /// <param name="state"></param>
        /// <param name="from">从第几条记录开始</param>
        /// <param name="count">返回的记录条数</param>
        /// <returns></returns>
        public List<Advice> GetArticlesByAdviceTypeID(List<string> adviceIDlist, AdviceState state, int from, int count)
        {
            List<Advice> list = new List<Advice>();

            Criteria c = new Criteria(CriteriaType.Equals, "State", (int)state);
            if (adviceIDlist != null && adviceIDlist.Count > 0)
            {
                Criteria subc = new Criteria(CriteriaType.None);
                subc.Mode = CriteriaMode.Or;
                foreach (string adID in adviceIDlist)
                {
                    subc.AddOr(CriteriaType.Equals, "TypeID", adID);
                }
                c.Criterias.Add(subc);
            }
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            list = Assistant.List<Advice>(c, orders, from, count);
            return list;
        }

        /// <summary>
        /// 按栏目ID与所馈状态查询列表
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="state"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Advice> GetList(string ownerId, AdviceState state, bool IsShow, int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (state != AdviceState.All)
            {
                c.Add(CriteriaType.Equals, "State", (int)state);
            }
            c.Add(CriteriaType.Equals, "OwnID", ownerId);
            if (IsShow)
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Advice>(c, orders, from, count);
        }

        /// <summary>
        /// 按栏目ID与所馈状态查询列表
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="state"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Advice> GetListByType(string typeID, AdviceState state, bool IsShow, int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (state != AdviceState.All)
            {
                c.Add(CriteriaType.Equals, "State", (int)state);
            }
            c.Add(CriteriaType.Equals, "TypeID", typeID);
            if (IsShow)
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Advice>(c, orders, from, count);
        }

        /// <summary>
        /// 按栏目ID与所馈状态统计记录
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public int GetCount(string ownerId, AdviceState state, bool IsShow)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (state != AdviceState.All)
            {
                c.Add(CriteriaType.Equals, "State", (int)state);
            }
            c.Add(CriteriaType.Equals, "OwnID", ownerId);
            if (IsShow)
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        /// 根据反馈类型取得反馈信息
        /// </summary>
        /// <param name="typeId">反馈类型ID</param>
        /// <param name="state">反馈状态</param>
        /// <param name="IsShow">是否显示</param>
        /// <returns></returns>
        public int GetCountByType(string typeId, AdviceState state, bool IsShow)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (state != AdviceState.All)
            {
                c.Add(CriteriaType.Equals, "State", (int)state);
            }
            c.Add(CriteriaType.Equals, "TypeID", typeId);
            if (IsShow)
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        /// 取得所有具有指定权限的用户
        /// </summary>
        /// <param name="adviceTypeID">反馈模型</param>
        /// <param name="content">指定权限</param>
        /// <returns></returns>
        public List<string> GetAllReceivers(string adviceTypeID, string content)
        {
            List<string> accountIDs = new List<string>();
            List<Permission> ps = AccountHelper.GetPermissions(adviceTypeID, content);
            if (ps != null && ps.Count > 0)
            {
                foreach (Permission p in ps)
                {
                    if (p.OwnerType == 0 && !accountIDs.Contains(p.OwnerID))
                        accountIDs.Add(p.OwnerID);
                    else if (p.OwnerType == 1)
                    {
                        accountIDs.AddRange(AccountHelper.GetAccountsOfRole(p.OwnerID));
                    }
                }
            }
            return accountIDs;
        }


        /// <summary>
        /// 反馈流程综合处理
        /// </summary>
        /// <param name="ar"></param>
        /// <param name="a"></param>
        /// <param name="ap"></param>
        public void OperationAdviceInfo(AdviceReply ar, Advice a, Processing ap)
        {
            if (ar != null)
            {
                AdviceReplyHelper.AddAdviceReply(ar);
            }
            if (a != null)
            {
                ProcessingHelper.SaveAdviceFlowInfoToDB(a, ap);
            }
        }

        /// <summary>
        /// 获取所有的反馈信息类型
        /// </summary>
        /// <returns></returns>
        public List<Advice> CreatedAdviceRate(string adTypeId)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", adTypeId);

            ListField[] fields = new ListField[1];
            fields[0] = new ListField();
            fields[0].FieldName = "AdviceInfoType";
            fields[0].Adorn = Adorns.Distinct;

            Order[] orders = new Order[1];
            orders[0] = new Order();
            orders[0].Name = "AdviceInfoType";
            orders[0].Mode = OrderMode.Asc;

            List<Advice> list = Assistant.List<Advice>(c, orders, 0, 0, fields);
            return list;
        }

        /// <summary>
        /// 工作日统计
        /// </summary>
        /// <param name="create">起始时间</param>
        /// <returns></returns>
        public int GetWorkingDays(DateTime create)
        {
            int days = 0;
            string fileName = HttpContext.Current.Server.MapPath("/Config/WorkingSet.config");
            NonWorkingDays workdays = NonWorkingDays.LoadNonWorkingDays(fileName);
            for (DateTime tmpTime = create; tmpTime < DateTime.Now; tmpTime = tmpTime.AddDays(1))
            {
                bool add = true;
                if (workdays.Weekends != null)
                {
                    foreach (DayOfWeek item in workdays.Weekends)
                    {
                        if (tmpTime.DayOfWeek == item)
                        {
                            add = false;
                            break;
                        }
                    }
                }

                if (workdays.NonworkingDays != null)
                {
                    foreach (ExceptionDays item in workdays.NonworkingDays)
                    {
                        if (tmpTime > item.StartTime && tmpTime <= item.EndTime)
                        {
                            add = false;
                            break;
                        }
                    }
                }

                if (workdays.WorkingDays != null)
                {
                    foreach (ExceptionDays item in workdays.WorkingDays)
                    {
                        if (tmpTime > item.StartTime && tmpTime <= item.EndTime)
                        {
                            add = true;
                            break;
                        }
                    }
                }

                if (add)
                {
                    days++;
                }
            }
            return days;
        }

        /// <summary>
        /// 重定Page，取消控件是否在Form中的验证
        /// </summary>
        class MyPage : Page
        {
            public override void VerifyRenderingInServerForm(Control control) { }
        }

        /// <summary>
        /// 添加或修改反馈标签
        /// </summary>
        /// <param name="editTagName"></param>
        /// <param name="newTagName"></param>
        /// <param name="fileName"></param>
        /// <param name="xPath"></param>
        public void InsertAdviceTagToAdviceTagXml(string editTagName, string newTagName, string fileName, String xPath)
        {

            Dictionary<string, string> tag = new Dictionary<string, string>();
            tag.Add("name", newTagName);
            if (string.IsNullOrEmpty(editTagName))
            {
                We7.Framework.Util.XmlHelper.CreateXmlNode(fileName, xPath, "tag", "", tag);
            }
            else
            {
                We7.Framework.Util.XmlHelper.CreateOrUpdateAttribute(fileName, xPath + "/tag[@name='" + editTagName + "']", tag);
            }


        }

    }

    #endregion
}

