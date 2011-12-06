using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework.Util
{
   /// <summary>
    /// 获取简体中文拼音首字母类
    /// </summary>
    public class CNspellTranslator
    {
        public CNspellTranslator()
        {
        }

        /// <summary>
        /// 获取简体中文字符串拼音首字母
        /// </summary>
        /// <param name="input">简体中文字符串</param>
        /// <returns>拼音首字母</returns>
        public string GetSpells(string input)
        {
            int len = input.Length;
            string reVal = "";
            for (int i = 0; i < len; i++)
            {
                reVal += GetSpell(input.Substring(i, 1));
            }
            return reVal;
        }

        /// <summary>
        /// 获取一个简体中文字的拼音首字母
        /// </summary>
        /// <param name="cn">一个简体中文字</param>
        /// <returns>拼音首字母</returns>
        public string GetSpell(string cn)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cn);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "?";
            }
            else return cn;
        }
    }
}
