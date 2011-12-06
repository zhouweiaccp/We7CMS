/*
 *Author：丁乐 
 * 功能：拿到表信息结构
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Globalization;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Thinkment.Data
{
    /// <summary>
    /// 响应实体
    /// 功能：拿到表信息结构
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public TableInfo()
        {
            //显示实例化
            Fileds = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            FiledsInfo = new Dictionary<string, Property>(StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 实例化此类
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pro">字段字典</param>
        public TableInfo(DataTable dt, Dictionary<string, Property> pro)
        {
            this.Table = dt;
            this.FiledsInfo = pro;
        }
        #region 表信息
        private int _records;
        /// <summary>
        /// 当前有多少条记录
        /// </summary>
        public int records
        {
            get
            {
                if (this.Table != null && this.Table.Rows != null && this.Table.Rows.Count != 0)
                {
                    _records = this.Table.Rows.Count;
                }
                else
                {
                    _records = 0;
                }
                return _records;
            }
        }
        private DataTable _table;

        /// <summary>
        /// 当前Table
        /// </summary>
        public DataTable Table
        {
            get { return _table; }
            set { _table = value; }
        }
        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName = string.Empty;
        /// <summary>
        /// ID
        /// </summary>
        public static string ID = string.Empty;

        /// <summary>
        /// 字段Key Value
        /// </summary>
        public static Dictionary<string, string> Fileds;

        private Dictionary<string, Property> _filedsDescription;
        /// <summary>
        /// 字段描述
        /// </summary>
        public Dictionary<string, Property> FiledsInfo
        {
            get { return _filedsDescription; }
            set { _filedsDescription = value; }
        }

        private Dictionary<string, string> heard;
        /// <summary>
        /// 字段属性Key
        /// </summary>
        private Dictionary<string, string> Heard
        {
            get
            {
                if (heard == null)
                {
                    heard = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    Dictionary<string, Property> _fileds = FiledsInfo;
                    foreach (var item in _fileds)
                    {
                        heard.Add(item.Value.Field, item.Value.Description);
                    }
                }
                return heard;
            }
        }
        #endregion

        /// <summary>
        /// 获取Json
        /// {0}总记录数
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return this.Convert2Json(this.Table);
        }
        /// <summary>
        /// 获取Json
        /// 说明：{"Key":"value"}
        /// 暂时支持类型：DataRow、DataSet、DataTable、IEnumerable、Hashtable
        /// <param name="o">数据类型</param>
        /// </summary>
        /// <returns></returns>
        public string ToJson(object o)
        {
            string json = this.Convert2Json(o);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            MatchCollection mc = Regex.Matches(json, "{\"[\\s|\\S]*?\":\"(?<Key>[\\s|\\S]*?)\",\"[\\s|\\S]*?\":\"(?<Value>[\\s|\\S]*?)\"}");
            for (int i = 0; i < mc.Count; i++)
			{
                sb.Append("\"" + mc[i].Groups["Key"] + "\"").Append(":").Append("\"" + mc[i].Groups["Value"] + "\"");
                if (i!=(mc.Count-1))
                {
                    sb.Append(",");
                }
			}
            return sb.Append("}").ToString();
        }

        /// <summary>
        /// 获取Json
        /// 说明：[{key:xx,value:xx},{..}]
        /// 暂时支持类型：DataRow、DataSet、DataTable、IEnumerable、Hashtable
        /// <param name="o">数据类型</param>
        /// </summary>
        /// <returns></returns>
        public string ToJson(object o, bool isKeyValue)
        {
            if (isKeyValue)
            {
                return this.Convert2Json(o);
            }
            else
            {
                return this.ToJson(o);
            }
        }

        #region dataset,datatable,object等对象转Json
        private void WriteDataRow(StringBuilder sb, DataRow row)
        {
            sb.Append("{");
            foreach (DataColumn column in row.Table.Columns)
            {
                sb.AppendFormat("\"{0}\":", column.ColumnName);
                WriteValue(sb, row[column]);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (row.Table.Columns.Count > 0)
            {
                --sb.Length;
            }
            sb.Append("}");
        }

        private void WriteDataSet(StringBuilder sb, DataSet ds)
        {
            sb.Append("{\"Tables\":{");
            foreach (DataTable table in ds.Tables)
            {
                sb.AppendFormat("\"{0}\":", table.TableName);
                WriteDataTable(sb, table);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (ds.Tables.Count > 0)
            {
                --sb.Length;
            }
            sb.Append("}}");
        }

        private void WriteDataTable(StringBuilder sb, DataTable table)
        {
            sb.Append("{\"rows\":[");
            foreach (DataRow row in table.Rows)
            {
                WriteDataRow(sb, row);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (table.Rows.Count > 0)
            {
                --sb.Length;
            }

            sb.Append("],").Append("\"cols\":["); //字段
            for (int i = 0; i < table.Columns.Count; i++)
            {
                string columnname = table.Columns[i].ColumnName;
                string key = (table.PrimaryKey != null && table.PrimaryKey.Length > 0) ? table.PrimaryKey[0].ColumnName : "ID";
                bool isKey = key.Equals(table.Columns[i].ColumnName);

                sb.Append("{\"name\":\"" + table.Columns[i].ColumnName + "\",");
                sb.Append("\"header\":\"" + (Heard.ContainsKey(columnname) ? Heard[columnname] : " ") + "\",");
                sb.Append("\"editable\":" + false.ToString().ToLower() + ",");
                sb.Append("\"sortable\":" + true.ToString().ToLower() + ",");
                sb.Append("\"hidden\":" + isKey.ToString().ToLower() + ",");
                sb.Append("\"sorttype\":\"" + "date" + "\",");
                sb.Append("\"identify\":" + isKey.ToString().ToLower() + ",");
                sb.Append("\"sortOnLoad\":" + false.ToString().ToLower() + "}");
                if (i != table.Columns.Count - 1)
                {
                    sb.Append(",");
                }
                else
                {
                    sb.Append("]");
                }
            }
            sb.Append(",").Append("\"Records\":\"" + records + "\"");  //当前条数
            sb.Append(",").Append("\"totalRecords\":\"{0}\"");  //总记录数
            sb.Append(",").Append("\"code\":\"{1}\"");  //状态码
            sb.Append(",").Append("\"message\":\"{2}\"");  //信息
            sb.Append(",").Append("\"currPage\":\"{3}\"");  //页码
            sb.Append(",").Append("\"totalPages\":\"{4}\"");  //总页数
            sb.Append("}");

        }

        private void WriteEnumerable(StringBuilder sb, IEnumerable e)
        {
            bool hasItems = false;
            sb.Append("[");
            foreach (object val in e)
            {
                WriteValue(sb, val);
                sb.Append(",");
                hasItems = true;
            }
            // Remove the trailing comma.
            if (hasItems)
            {
                --sb.Length;
            }
            sb.Append("]");
        }

        private void WriteHashtable(StringBuilder sb, Hashtable e)
        {
            bool hasItems = false;
            sb.Append("{");
            foreach (string key in e.Keys)
            {
                sb.AppendFormat("\"{0}\":", key.ToLower());
                WriteValue(sb, e[key]);
                sb.Append(",");
                hasItems = true;
            }
            // Remove the trailing comma.
            if (hasItems)
            {
                --sb.Length;
            }
            sb.Append("}");
        }

        private void WriteObject(StringBuilder sb, object o)
        {
            MemberInfo[] members = o.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public);
            sb.Append("{");
            bool hasMembers = false;
            foreach (MemberInfo member in members)
            {
                bool hasValue = false;
                object val = null;
                if ((member.MemberType & MemberTypes.Field) == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    val = field.GetValue(o);
                    hasValue = true;
                }
                else if ((member.MemberType & MemberTypes.Property) == MemberTypes.Property)
                {
                    PropertyInfo property = (PropertyInfo)member;
                    if (property.CanRead && property.GetIndexParameters().Length == 0)
                    {
                        val = property.GetValue(o, null);
                        hasValue = true;
                    }
                }
                if (hasValue)
                {
                    sb.Append("\"");
                    sb.Append(member.Name);
                    sb.Append("\":");
                    WriteValue(sb, val);
                    sb.Append(",");
                    hasMembers = true;
                }
            }
            if (hasMembers)
            {
                --sb.Length;
            }
            sb.Append("}");
        }

        private void WriteString(StringBuilder sb, string s)
        {
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");
        }
        private void WriteValue(StringBuilder sb, object val)
        {
            if (val == null || val == System.DBNull.Value)
            {
                sb.Append("null");
            }
            else if (val is string || val is Guid)
            {
                WriteString(sb, val.ToString());
            }
            else if (val is bool)
            {
                sb.Append(val.ToString().ToLower());
            }
            else if (val is double ||
                val is float ||
                val is long ||
                val is int ||
                val is short ||
                val is byte ||
                val is decimal)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture.NumberFormat, "{0}", val);
            }
            else if (val.GetType().IsEnum)
            {
                sb.Append((int)val);
            }
            else if (val is DateTime)
            {
                //sb.Append("new Date(\"");
                //sb.Append(((DateTime)val).ToString("MMMM, d yyyy HH:mm:ss", new CultureInfo("en-US", false).DateTimeFormat));
                //sb.Append("\")");
                DateTime time = Convert.ToDateTime(val);
                sb.Append("\"").Append(time.ToString("yyyy-MM-dd HH:mm")).Append("\"");
            }
            else if (val is DataSet)
            {
                WriteDataSet(sb, val as DataSet);
            }
            else if (val is DataTable)
            {
                WriteDataTable(sb, val as DataTable);
            }
            else if (val is DataRow)
            {
                WriteDataRow(sb, val as DataRow);
            }
            else if (val is Hashtable)
            {
                WriteHashtable(sb, val as Hashtable);
            }
            else if (val is IEnumerable)
            {
                WriteEnumerable(sb, val as IEnumerable);
            }
            else
            {
                WriteObject(sb, val);
            }
        }
        /// <summary>
        /// 转换Json
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private string Convert2Json(object o)
        {
            StringBuilder sb = new StringBuilder();
            WriteValue(sb, o);
            return sb.ToString();
        }
        #endregion
    }

}