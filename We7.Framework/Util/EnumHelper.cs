using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework.Util
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 将枚举转换成Dictionary类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<int, string> EnumParseDictionary(Type type)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string[] names = Enum.GetNames(type);
            int[] values = (int[])Enum.GetValues(type);
            for (int i = 0; i < names.Length; i++)
            {
                dic.Add(values[i], names[i]);            
            }

            return dic;
        }
    }
}
