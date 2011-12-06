using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using Thinkment.Data;
using We7.Framework;

namespace We7.CMS
{
    /// <summary>
    /// 反馈流转接口
    /// </summary>
    public interface IAdviceTransferHelper
    {
        /// <summary>
        /// 根据反馈ID获取所有流转记录
        /// </summary>
        /// <param name="adviceId">反馈ID</param>
        /// <returns></returns>
        IList<AdviceTransfer> GetAllTransferByAdviceID(string adviceId);
    }

    /// <summary>
    /// 反馈反馈流转工厂
    /// </summary>
    public class AdviceTransferFactory
    {
        /// <summary>
        /// 创建反馈流转类
        /// </summary>
        /// <returns></returns>
        public static IAdviceTransferHelper Create()
        {
            return HelperFactory.Instance.GetHelper<AdviceTransferHelper>();
        }
    }

    [Helper("We7.AdviceTransferHelper")]
    public class AdviceTransferHelper :BaseHelper, IAdviceTransferHelper
    {
        #region IAdviceTransferHelper 成员

        /// <summary>
        /// 根据反馈ID获取所有流转记录
        /// </summary>
        /// <param name="adviceId">反馈ID</param>
        /// <returns></returns>
        /// 
        public IList<AdviceTransfer> GetAllTransferByAdviceID(string adviceId)
        {
            Order[] os = new Order[] { new Order("Created", OrderMode.Desc) };

            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceId);
            List<AdviceTransfer> trans = Assistant.List<AdviceTransfer>(c, new Order[] { new Order("Created", OrderMode.Desc) });

            return Assistant.List<AdviceTransfer>(c, os);

        }

        #endregion
    }
}
