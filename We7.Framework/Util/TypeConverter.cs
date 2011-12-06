using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace We7.Framework.Util
{
    public class TypeConverter
    {
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression, defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                    return true;
                else if (string.Compare(expression, "false", true) == 0)
                    return false;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression)
        {
            return ObjectToInt(expression, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型,转换失败返回0
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str)
        {
            return StrToInt(str, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str, int defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 11 || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(str, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(str, defValue));
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
                return defValue;

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
                return defValue;

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object strValue)
        {
            return ObjectToFloat(strValue.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue)
        {
            if ((strValue == null))
                return 0;

            return StrToFloat(strValue.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
                return defValue;

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(strValue, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// string型转换为
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="dbtype"></param>
        /// <returns></returns>
        public static object StrToObjectByDbType(string value, DbType dbtype)
        {
            object result = null;
            switch (dbtype)
            {
                case DbType.Boolean:
                    bool v;
                    value = value.Trim();
                    Boolean.TryParse(value, out v);
                    result = v;
                    break;
                case DbType.Date:
                case DbType.DateTime:
                case DbType.Time:
                case DbType.DateTime2:
                    value = value.Trim();
                    DateTime dt;
                    DateTime.TryParse(value, out dt);
                    result = dt;
                    break;
                case DbType.DateTimeOffset:
                    break;
                case DbType.Decimal:
                case DbType.Currency:
                case DbType.Double:
                    value = value.Trim();
                    Double dv;
                    Double.TryParse(value, out dv);
                    result = dv;
                    break;
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.Byte:
                case DbType.SByte:
                case DbType.Single:
                case DbType.VarNumeric:
                    value = value.Trim();
                    int i;
                    Int32.TryParse(value, out i);
                    result = i;
                    break;
                case DbType.Object:
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    result = value;
                    break;
                case DbType.Xml:
                    break;
                case DbType.Binary:
                    break;
                case DbType.Guid:
                    break;
                default:
                    result = value;
                    break;
            }
            return result;
        }

        public static object StrToObjectByTypeCode(string value, TypeCode type)
        {
            object result = null;
            switch (type)
            {
                case TypeCode.Boolean:
                    Boolean b;
                    Boolean.TryParse(value, out b);
                    result = b;
                    break;
                case TypeCode.Byte:
                    Byte bt;
                    Byte.TryParse(value, out bt);
                    result = bt;
                    break;
                case TypeCode.Char:
                    Char c;
                    Char.TryParse(value, out c);
                    result = c;
                    break;
                case TypeCode.DBNull:
                    result = DBNull.Value;
                    break;
                case TypeCode.DateTime:
                    DateTime dt;
                    DateTime.TryParse(value, out dt);
                    result = dt;
                    break;
                case TypeCode.Decimal:
                    Decimal dec;
                    Decimal.TryParse(value, out dec);
                    result = dec;
                    break;
                case TypeCode.Double:
                    Double db;
                    Double.TryParse(value, out db);
                    result = db;
                    break;
                case TypeCode.Empty:
                    result = null;
                    break;
                case TypeCode.Int16:
                    Int16 i16;
                    Int16.TryParse(value, out i16);
                    result = i16;
                    break;
                case TypeCode.Int32:
                    Int32 i32;
                    Int32.TryParse(value, out i32);
                    result = i32;
                    break;
                case TypeCode.Int64:
                    Int64 i64;
                    Int64.TryParse(value, out i64);
                    result = i64;
                    break;
                case TypeCode.Object:
                    result = value;
                    break;
                case TypeCode.SByte:
                    SByte sb;
                    SByte.TryParse(value, out sb);
                    result = sb;
                    break;
                case TypeCode.Single:
                    Single sng;
                    Single.TryParse(value, out sng);
                    result = sng;
                    break;
                case TypeCode.String:
                    result = String.IsNullOrEmpty(value)?String.Empty:value;
                    break;
                case TypeCode.UInt16:
                    UInt16 ui16;
                    UInt16.TryParse(value, out ui16);
                    result = ui16;
                    break;
                case TypeCode.UInt32:
                    UInt32 ui32;
                    UInt32.TryParse(value, out ui32);
                    result = ui32;
                    break;
                case TypeCode.UInt64:
                    UInt64 ui64;
                    UInt64.TryParse(value, out ui64);
                    result = ui64;
                    break;
                default:
                    result = value;
                    break;
            }
            return result;
        }
    }
}
