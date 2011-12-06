using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Model.Core;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.ContentModel
{
    public partial class Field : System.Web.UI.Page
    {
       

        /// <summary>
        /// Model Name
        /// </summary>
        public string ModelName
        {
            get
            {
                return RequestHelper.Get<string>("modelname","System.Article");
            }
        }

        /// <summary>
        /// ContentModel Type
        /// </summary>
        public string modelType
        {
            get
            {
                string t = We7Request.GetQueryString("modelType");
                if (t.Length == 0)
                    return ModelType.ARTICLE.ToString();
                else
                    return t;
            }
        }

        /// <summary>
        /// 内容模型实体
        /// </summary>
        public ModelInfo modelInfo;

        /// <summary>
        /// 获取该内容模型的tablecollection
        /// </summary>
        /// <returns></returns>
        public We7DataTableCollection GetDataTabels()
        {
           
            We7DataTableCollection tables = new We7DataTableCollection();
            modelInfo = ModelHelper.GetModelInfo(ModelName);

            if (modelInfo != null && modelInfo.DataSet != null)
            {
                tables = modelInfo.DataSet.Tables;
            }
            return tables;
        }

        /// <summary>
        /// 将类型编码转换为汉字
        /// </summary>
        /// <param name="typecode"></param>
        /// <returns></returns>
        public string ConvertDatTypeToString(TypeCode typecode)
        {
            string ConvertCode = string.Empty;

            switch (typecode)
            {
                case TypeCode.Boolean:
                    ConvertCode = "是否类型";
                    break;
                case TypeCode.Byte:
                    ConvertCode = "字节类型";
                    break;
                case TypeCode.Char:
                    ConvertCode = "单字符类型";
                    break;
                case TypeCode.DBNull:
                    ConvertCode = "可空类型";
                    break;
                case TypeCode.DateTime:
                    ConvertCode = "日期类型";
                    break;
                case TypeCode.Decimal:
                    ConvertCode = "小数类型";
                    break;
                case TypeCode.Double:
                    ConvertCode = "小数类型";
                    break;
                case TypeCode.Empty:
                    ConvertCode = "空类型";
                    break;
                case TypeCode.Int16:
                    ConvertCode = "整数类型";
                    break;
                case TypeCode.Int32:
                    ConvertCode = "整数类型";
                    break;
                case TypeCode.Int64:
                    ConvertCode = "整数类型";
                    break;
                case TypeCode.Object:
                    ConvertCode = "对象类型";
                    break;
                case TypeCode.SByte:
                    ConvertCode = "字符类型";
                    break;
                case TypeCode.Single:
                    ConvertCode = "小数类型";
                    break;
                case TypeCode.String:
                    ConvertCode = "文本类型";
                    break;
                case TypeCode.UInt16:
                    ConvertCode = "整数类型";
                    break;
                case TypeCode.UInt32:
                    ConvertCode = "整数类型";
                    break;
                case TypeCode.UInt64:
                    ConvertCode = "整数类型";
                    break;
                default:
                    ConvertCode = "未知类型";
                    break;
            }
            return ConvertCode;
        }
    }
}
