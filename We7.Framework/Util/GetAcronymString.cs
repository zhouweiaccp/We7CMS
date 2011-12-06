using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework.Util
{
    /// <summary>
    /// 字符串拼音生成类
    /// </summary>
    public class GetAcronymString
    {
        private GetAcronymString() { }

        ///   <summary>   
        ///   获取一串汉字的拼音声母   
        ///   </summary>   
        ///   <param name="chinese">Unicode格式的汉字字符串</param>   
        ///   <returns>拼音声母字符串</returns>   
        ///   <example>   
        ///   “西部动力”转换为“xbdl”   
        ///   </example>   
        public static String Convert(String chinese)
        {
            char[] buffer = new char[chinese.Length];
            for (int i = 0; i < chinese.Length; i++)
            {
                buffer[i] = Convert(chinese[i]);
            }
            return new String(buffer);
        }

        ///   <summary>   
        ///   获取一个汉字的拼音声母   
        ///   </summary>   
        ///   <param   name="chinese">Unicode格式的一个汉字</param>   
        ///   <returns>汉字的声母</returns>   
        public static char Convert(Char chinese)
        {
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            Encoding unicode = Encoding.Unicode;

            //   Convert   the   string   into   a   byte[].   
            byte[] unicodeBytes = unicode.GetBytes(new Char[] { chinese });
            //   Perform   the   conversion   from   one   encoding   to   the   other.   
            byte[] asciiBytes = Encoding.Convert(unicode, gb2312, unicodeBytes);

            //   计算该汉字的GB-2312编码   
            if (asciiBytes.Length > 1)
            {
                int n = (int)asciiBytes[0] << 8;
                n += (int)asciiBytes[1];
                //   根据汉字区域码获取拼音声母   
                if (In(0xB0A1, 0xB0C4, n)) return 'a';
                if (In(0XB0C5, 0XB2C0, n)) return 'b';
                if (In(0xB2C1, 0xB4ED, n)) return 'c';
                if (In(0xB4EE, 0xB6E9, n)) return 'd';
                if (In(0xB6EA, 0xB7A1, n)) return 'e';
                if (In(0xB7A2, 0xB8c0, n)) return 'f';
                if (In(0xB8C1, 0xB9FD, n)) return 'g';
                if (In(0xB9FE, 0xBBF6, n)) return 'h';
                if (In(0xBBF7, 0xBFA5, n)) return 'j';
                if (In(0xBFA6, 0xC0AB, n)) return 'k';
                if (In(0xC0AC, 0xC2E7, n)) return 'l';
                if (In(0xC2E8, 0xC4C2, n)) return 'm';
                if (In(0xC4C3, 0xC5B5, n)) return 'n';
                if (In(0xC5B6, 0xC5BD, n)) return 'o';
                if (In(0xC5BE, 0xC6D9, n)) return 'p';
                if (In(0xC6DA, 0xC8BA, n)) return 'q';
                if (In(0xC8BB, 0xC8F5, n)) return 'r';
                if (In(0xC8F6, 0xCBF0, n)) return 's';
                if (In(0xCBFA, 0xCDD9, n)) return 't';
                if (In(0xCDDA, 0xCEF3, n)) return 'w';
                if (In(0xCEF4, 0xD188, n)) return 'x';
                if (In(0xD1B9, 0xD4D0, n)) return 'y';
                if (In(0xD4D1, 0xD7F9, n)) return 'z';
                return 'd';
            }
            else
            {
                return chinese;
            }

        }

        private static bool In(int Lp, int Hp, int Value)
        {
            return ((Value <= Hp) && (Value >= Lp));
        }
    }
}
