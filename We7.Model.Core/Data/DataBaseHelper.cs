using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Util;
using We7.Model.Core.Config;
using We7.Framework;
using System.Data;

namespace We7.Model.Core.Data
{
    /// <summary>
    /// 数据库处理接口
    /// </summary>
    public interface IDataBaseHelper
    {
        /// <summary>
        /// 根据内容模型文件创建表
        /// </summary>
        /// <param name="model"></param>
        void CreateTable(ModelInfo model);

        /// <summary>
        /// 根据内容模型信息更新表结构
        /// </summary>
        /// <param name="model"></param>
        void UpdateTable(ModelInfo model);

        /// <summary>
        /// 根据内容模型信息删除表结构
        /// </summary>
        /// <param name="model"></param>
        void DeleteTable(ModelInfo model);

        /// <summary>
        /// 根据Sql条件查询数据行
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="sqlCondition">查询条件</param>
        /// <returns></returns>
        DataRow GetDataRow(string table,string sqlCondition);

        /// <summary>
        /// 查询数据记录
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="sqlCondition">查询条件</param>
        /// <returns></returns>
        int Count(string table, string sqlCondition);

        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="sqlCondition">查询条件</param>
        /// <param name="start">开始记录</param>
        /// <param name="querycount">查询条数</param>
        /// <returns></returns>
        DataTable Query(string table, string sqlCondition, int start, int querycount,string orders);

        /// <summary>
        /// 创建模型表
        /// </summary>
        void CreateModelTables();
    }

    /// <summary>
    /// 数据库处理工厂
    /// </summary>
    public class DataBaseHelperFactory
    {
        const string DataBaseHelperFactoryID = "We7.Model.DataBaseHelper";

        /// <summary>
        /// 创建数据帮助类
        /// </summary>
        /// <returns></returns>
        public static IDataBaseHelper Create()
        {
            IDataBaseHelper dbhelper = AppCtx.Cache.RetrieveObject(DataBaseHelperFactoryID) as IDataBaseHelper;
            if (dbhelper == null)
            {
                dbhelper = Utils.CreateInstance(ModelConfig.DataBaseHelper) as IDataBaseHelper;
                if (dbhelper == null)
                {
                    throw new Exception("不存在内容模型的数据库帮助类！");
                }
                AppCtx.Cache.AddObjectWithFileChange(DataBaseHelperFactoryID, dbhelper, ModelConfig.ConfigFilePath);
            }
            return dbhelper;
        }
    }
}
