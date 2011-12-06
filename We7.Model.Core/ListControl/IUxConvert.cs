using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Util;
using We7.Model.Core.Config;

namespace We7.Model.Core.ListControl
{
    public interface IUxConvert
    {
        string GetText(object dataItem, ColumnInfo columnInfo);
    }

    /// <summary>
    /// 用户自定义转化类型
    /// </summary>
    public class UxConvertFactory
    {
        public static IUxConvert GetConvert(ColumnInfo columnInfo)
        {
            string convertKey=GetConvertCacheKey(columnInfo.ConvertType);
            IUxConvert convert = AppCtx.Cache.RetrieveObject<IUxConvert>(convertKey);
            if (convert == null && ModelConfig.ColumnConvert!=null)
            {
                columnInfo.ConvertType = String.IsNullOrEmpty(columnInfo.ConvertType) ? "" : columnInfo.ConvertType;
                string convertType=ModelConfig.ColumnConvert[columnInfo.ConvertType];
                convert = Utils.CreateInstance<IUxConvert>(convertType);
                AppCtx.Cache.AddObjectWithFileChange(convertKey, convert, ModelConfig.ConfigFilePath);
            }
            return convert;
        }

        static string GetConvertCacheKey(string convert)
        {
            return String.Format("$Model$ConvertType${0}", convert);
        }
    }
}
