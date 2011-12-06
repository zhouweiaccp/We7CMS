using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace We7.Model.Core.Data
{
    /// <summary>
    /// SQL构造器
    /// </summary>
    public interface ISQLBuilder
    {

        /// <summary>
        /// 构造添加SQL
        /// </summary>
        /// <param name="data">模型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>SQL</returns>
        string BuildInsertSQL(PanelContext data, IDataParameterCollection parameters);

        /// <summary>
        /// 构造添加SQL
        /// </summary>
        /// <param name="data">模型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>SQL</returns>
        string BuildDeleteSQL(PanelContext data, IDataParameterCollection parameters);

        /// <summary>
        /// 构造添加SQL
        /// </summary>
        /// <param name="data">模型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>SQL</returns>
        string BuildUpdateSQL(PanelContext data, IDataParameterCollection parameters);

        /// <summary>
        /// 构造添加SQL
        /// </summary>
        /// <param name="data">模型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>SQL</returns>
        string BuildCountSQL(PanelContext data, IDataParameterCollection parameters);


        /// <summary>
        /// 构造添加SQL
        /// </summary>
        /// <param name="data">模型</param>
        /// <param name="parameters">参数集合</param>
        /// <param name="startindex">Start Index</param>
        /// <param name="itemcount">Item Count</param>
        /// <returns></returns>
        string BuildListSQL(PanelContext data, IDataParameterCollection parameters, int startindex, int itemcount);

        /// <summary>
        /// 构造获取指定SQL记录
        /// </summary>
        /// <param name="data">模型数据</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>SQL</returns>
        string BuildGetSQL(PanelContext data, IDataParameterCollection parameters);

        /// <summary>
        /// 取得Connection
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();

        /// <summary>
        /// 取得Command
        /// </summary>
        /// <returns></returns>
        IDbCommand GetCommand();

        /// <summary>
        /// 取得Adapter
        /// </summary>
        /// <returns></returns>
        IDbDataAdapter GetDataAdapter();

        /// <summary>
        /// 取得参数
        /// </summary>
        /// <returns></returns>
        IDbDataParameter GetDataparameter();
    }
}
