using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Data;

namespace We7.Model.Core.Converter
{
    public class GetEntityValue:IOutputConvert
    {
        public object Convert(We7DataColumn column, DataRow row, string[] fields, object extobj)
        {
            if (fields != null && fields.Length > 0&extobj!=null)
            {
                Type type=extobj.GetType();
                PropertyInfo prop=type.GetProperty(fields[0]);
                if (prop != null)
                {
                    return prop.GetValue(extobj, null);
                }
            }
            return DBNull.Value;
        }
    }
}
