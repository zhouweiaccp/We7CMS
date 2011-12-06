using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using System.Xml.Serialization;

namespace We7.Model.Core
{
    /// <summary>
    /// 查询模式
    /// </summary>
    public enum QueryMode { AND, OR }

    /// <summary>
    /// 查询类型
    /// </summary>
    public enum OperationType 
    { 
        /// <summary>
        /// 等于
        /// </summary>
        [XmlEnum("=")]
        EQUER=0, 
        /// <summary>
        /// 不等
        /// </summary>
        [XmlEnum("<>")]
        NOTEQUER,
        /// <summary>
        /// 模糊查询
        /// </summary>
        [XmlEnum("like")]
        LIKE, 
        /// <summary>
        /// 小于
        /// </summary>
        [XmlEnum("<")]
        LESSTHAN,
        /// <summary>
        /// 大于
        /// </summary>
        [XmlEnum(">")]
        MORETHAN,
        /// <summary>
        /// 小于等于
        /// </summary>
        [XmlEnum("<=")]
        LESSTHANEQURE,
        /// <summary>
        /// 大于等于
        /// </summary>
        [XmlEnum(">=")]
        MORETHANEQURE
    }

    /// <summary>
    /// 空处理
    /// </summary>
    public enum WhenNull { NONE, IGNORE }

    /// <summary>
    /// 数据字段值
    /// </summary>
    [Serializable]
    public class DataField
    {
        public DataField(We7DataColumn column, object value)
        {
            Column = column;
            Value = value;
        }

        /// <summary>
        /// 列信息
        /// </summary>
        public We7DataColumn Column { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public object Value { get; set; }
    }

    /// <summary>
    /// 数据字段值集合
    /// </summary>
    [Serializable]
    public class DataFieldCollection : Collection<DataField>
    {
        /// <summary>
        /// 字段值的表信息
        /// </summary>
        public We7DataTable Table { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="column">列信息</param>
        /// <returns>字段值</returns>
        public object this[We7DataColumn column]
        {
            get
            {
                foreach (DataField field in this)
                {
                    if (field.Column == column)
                        return field.Value;
                }
                return null;
            }
            set
            {
                foreach (DataField field in this)
                {
                    if (field.Column == column)
                    {
                        field.Value = value;
                        return;
                    }
                }
                Add(new DataField(column, value));
            }
        }

        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="name">列名</param>
        /// <returns>字段值</returns>
        public object this[string name]
        {
            get
            {
                foreach (DataField field in this)
                {
                    if (field.Column.Name == name)
                        return field.Value;
                }
                return null;
            }
            set
            {
                We7DataColumn column = null;
                foreach (DataField field in this)
                {
                    if (field.Column.Name == name)
                        column = field.Column;
                }
                if (column == null)
                {
                    column = Table.Columns[name];
                }
                if (column == null)
                {
                    throw new Exception("不存在列名为" + name + "的数据列");
                }
                this[column] = value;
            }
        }

        /// <summary>
        /// 根据列名索引数据字段
        /// </summary>
        /// <param name="name">列名</param>
        /// <returns>数据字段</returns>
        public DataField IndexOf(string name)
        {
            foreach (DataField field in this)
            {
                if (field.Column.Name == name)
                    return field;
            }
            return null;
        }

        /// <summary>
        /// 按映射字段索引字段
        /// </summary>
        /// <param name="mappingField">映射字段</param>
        /// <returns>返回数据字段</returns>
        public DataField IndexByMapping(string mappingField)
        {
            foreach (DataField field in this)
            {
                if (field.Column.Direction == ParameterDirection.Output || field.Column.Direction == ParameterDirection.InputOutput)
                {
                    string mapping = String.IsNullOrEmpty(field.Column.Mapping) ? field.Column.Name : field.Column.Mapping;
                    if (mapping == mappingField)
                        return field;
                }
            }
            return null;
        }

    }

    /// <summary>
    /// 查询字段
    /// </summary>
    [Serializable]
    public class QueryField : DataField
    {
        public QueryField(We7DataColumn column, object value)
            : base(column, value)
        {
        }

        /// <summary>
        /// 操作符
        /// </summary>
        public OperationType Operator { get; set; }

        /// <summary>
        /// 空处理
        /// </summary>
        public WhenNull WhenNull { get; set; }
    }

    /// <summary>
    /// 查询字段信息集合
    /// </summary>
    [Serializable]
    public class QueryFieldCollection : Collection<QueryField>
    {
        /// <summary>
        /// 数据表
        /// </summary>
        public We7DataTable Table { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns>字段值</returns>
        public object this[We7DataColumn column]
        {
            get
            {
                foreach (QueryField field in this)
                {
                    if (field.Column == column)
                        return field.Value;
                }
                return null;
            }
            set
            {
                foreach (QueryField field in this)
                {
                    if (field.Column == column)
                    {
                        field.Value = value;
                        return;
                    }
                }
                Add(new QueryField(column, value));
            }
        }

        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns>字段值</returns>
        public object this[string name]
        {
            get
            {
                foreach (QueryField field in this)
                {
                    if (field.Column.Name == name)
                        return field.Value;
                }
                return null;
            }
            set
            {
                We7DataColumn column = null;
                foreach (QueryField field in this)
                {
                    if (field.Column.Name == name)
                        column = field.Column;
                }
                if (column == null)
                {
                    column = Table.Columns[name];
                }
                if (column == null)
                {
                    throw new Exception("不存在列名为" + name + "的数据列");
                }
                this[column] = value;
            }
        }

        /// <summary>
        /// 根据列名索引数据字段
        /// </summary>
        /// <param name="name">列名</param>
        /// <returns>数据字段</returns>
        public QueryField IndexOf(string name)
        {
            foreach (QueryField field in this)
            {
                if (field.Column.Name == name)
                    return field;
            }
            return null;
        }
    }
}
