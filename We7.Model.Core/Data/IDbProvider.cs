using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;

namespace We7.Model.Core.Data
{
    /// <summary>
    /// 数据提供接口
    /// </summary>
    public interface IDbProvider
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="data">模型信息</param>
        /// <returns>是否成功</returns>
        bool Insert(PanelContext data);        

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">模型信息</param>
        /// <returns>是否成功</returns>
        bool Update(PanelContext data);

        /// <summary>
        /// 删除符合条件的数据
        /// </summary>
        /// <param name="data">模型信息</param>
        /// <returns>是否删除成功</returns>
        bool Delete(PanelContext data);

        /// <summary>
        /// 查询信息列表,并返回满足当前条件的记录数
        /// </summary>
        /// <param name="model"></param>
        /// <param name="recordcount"></param>
        /// <returns></returns>
        DataTable Query(PanelContext data, out int recordcount, ref int pageindex);

        /// <summary>
        /// 取得信息实体
        /// </summary>
        /// <param name="data">模型信息</param>
        /// <returns>行数据</returns>
        DataRow Get(PanelContext data);

        /// <summary>
        /// 取得行数
        /// </summary>
        /// <param name="data">模型信息</param>
        /// <returns>行数</returns>
        int GetCount(PanelContext data);

        
    }

}
