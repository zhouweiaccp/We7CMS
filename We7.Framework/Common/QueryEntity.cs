using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Web.UI;

namespace We7
{
   /// <summary>
   /// 摘要：
   /// 查询参数模型类。主要提供有条件查询的参数值传递的模型
   /// </summary>
    [Serializable]
    public class QueryEntity
    {
        #region 构造函数
        public QueryEntity() {
        }
        #endregion
      
        #region 字段
       

        /// <summary>
        /// 查询的数据模型名称
        /// </summary>
        private string m_modelName;
        /// <summary>
        /// 查询参数集合
        /// </summary>
        private List<QueryParam> queryParams=new List<QueryParam>();
        /// <summary>
        /// 查询排序字段实体类数组
        /// </summary>
        private Order[] m_orders;
       
        
        #endregion

        #region 属性
        /// <summary>
        /// 获取或者设置查询的数据模型名称
        /// </summary>
        public string ModelName
        {
            get { return m_modelName; }
            set { m_modelName = value; }
        }
        /// <summary>
        /// 查询参数模型实例。提供查询字段名称。具体值。查询的条件。值类型等集合
        /// </summary>
        public List<QueryParam> QueryParams
        {
            get { return queryParams; }
            set { queryParams = value; }
        }
        /// <summary>
        /// 获取或设置排序字段实体数组
        /// </summary>
        public Order[] Orders
        {
            get { return m_orders; }
            set { m_orders = value; }
        }
        #endregion      
    }

    /// <summary>
    /// 摘要：
    /// 参数模型类。提供查询参数的模型。
    /// </summary>
    [Serializable]
    public class QueryParam
    {

        #region 构造函数
        
        #endregion

        #region 字段
        /// <summary>
        /// 查询运算符 枚举类型
        /// </summary>
        private CriteriaType m_criteriaType;
       /// <summary>
       /// 查询数据列名称
       /// </summary>
        private string m_columnKey;

      /// <summary>
      /// 查询数据列值
      /// </summary>
        private object m_columnValue;        
      
        #endregion
    
        #region 属性
        /// <summary>
        /// 获取或设置查询运算符
        /// 枚举类型
        /// </summary>
        public CriteriaType CriteriaType
        {
            get { return m_criteriaType; }
            set { m_criteriaType = value; }
        }
        /// <summary>
        /// 获取或设置查询数据列键名称
        /// </summary>
        public string ColumnKey
        {
            get { return m_columnKey; }
            set { m_columnKey = value; }
        }
        /// <summary>
        /// 获取或设置查询数据列值
        /// </summary>
        public object ColumnValue
        {
            get { return m_columnValue; }
            set { m_columnValue = value; }
        }
        #endregion     
    }

}
