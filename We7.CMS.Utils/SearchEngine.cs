using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;

namespace We7.CMS
{
    /// <summary>
    /// 搜索引擎处理
    /// </summary>
    public class SearchEngine
    {
        /// <summary>
        /// 搜索引擎处理
        /// </summary>
        public SearchEngine() { }

        #region 初始化变量
        /// <summary>
        /// 搜索引擎特征数组
        /// </summary>
        private string[][] _Enginers = new string[][] {
　　　　new string[]{"google","utf8","q","Google"},
　　　　new string[]{"baidu","gb2312","wd","百度"},
　　　　new string[]{"yahoo","utf8","p","雅虎"},
　　　　new string[]{"yisou","utf8","search","一搜"},
　　　　new string[]{"live","utf8","q","MSN&Live"},
　　　　new string[]{"tom","gb2312","word","TOM"},
　　　　new string[]{"163","gb2312","q","网易有道"},
　　　　new string[]{"iask","gb2312","k","新浪爱问"},
　　　　new string[]{"soso","gb2312","w","腾讯SoSo"},
　　　　new string[]{"sogou","gb2312","query","搜狐搜狗"},
　　　　new string[]{"zhongsou","gb2312","w","中国搜索"},
　　　　new string[]{"3721","gb2312","p","3721"},
　　　　new string[]{"openfind","utf8","q","openfind"},
　　　　new string[]{"alltheweb","utf8","q","alltheweb"},
　　　　new string[]{"lycos","utf8","query","lycos"},
　　　　new string[]{"onseek","utf8","q","onseek"}
　　};

        private string _EngineName = "";
        /// <summary>
        /// 搜索引擎名称
        /// </summary>
        public string EngineName
        {
            get
            {
                return _EngineName;
            }
        }

        private string _Coding = "utf8";
        /// <summary>
        /// 搜索引擎编码
        /// </summary>
        public string Coding
        {
            get
            {
                return _Coding;
            }
        }

        private string _RegexWord = "";
        /// <summary>
        /// 搜索引擎关键字查询参数名称
        /// </summary>
        public string RegexWord
        {
            get
            {
                return _RegexWord;
            }
        }
        private string _Regex = @"(";
        #endregion

        #region 搜索引擎关键字
        /// <summary>
        /// 建立搜索关键字正则表达式
        /// </summary>
        /// <param name="myString"></param>
        public void EngineRegEx(string myString)
        {
            for (int i = 0, j = _Enginers.Length; i < j; i++)
            {
                if (myString.Contains(_Enginers[i][0]))
                {
                    _EngineName = _Enginers[i][3];
                    _Coding = _Enginers[i][1];
                    _RegexWord = _Enginers[i][2];
                    _Regex += _Enginers[i][0] + @".+.*[?/&]" + _RegexWord + @"[=:])(?<key>[^&]*)";
                    break;
                }
            }
        }
        /// <summary>
        /// 得到搜索引擎关键字
        /// </summary>
        /// <param name="myString">url</param>
        /// <returns></returns>
        public string SearchKey(string myString)
        {
            if (myString != null && myString != "")
            {
                EngineRegEx(myString.ToLower());
                if (_EngineName != "")
                {
                    Regex myReg = new Regex(_Regex, RegexOptions.IgnoreCase);
                    Match matche = myReg.Match(myString);
                    myString = matche.Groups["key"].Value;
                    //去处表示为空格的+
                    myString = myString.Replace("+", " ");
                    if (_Coding == "gb2312")
                    {
                        myString = GetUTF8String(myString);
                    }
                    else
                    {
                        myString = Uri.UnescapeDataString(myString);
                    }
                }
                else
                    myString = "";
            }
            return myString;
        }
        /// <summary>
        /// 整句转码
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
        public string GetUTF8String(string myString)
        {
            Regex myReg = new Regex("(?<key>%..%..)", RegexOptions.IgnoreCase);
            MatchCollection matches = myReg.Matches(myString);
            string myWord;
            for (int i = 0, j = matches.Count; i < j; i++)
            {
                myWord = matches[i].Groups["key"].Value.ToString();
                myString = myString.Replace(myWord, GB2312ToUTF8(myWord));
            }
            return myString;
        }

        /// <summary>
        /// 单字GB2312转UTF8 URL编码
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
        public string GB2312ToUTF8(string myString)
        {
            string[] myWord = myString.Split('%');
            byte[] myByte = new byte[] { Convert.ToByte(myWord[1], 16), Convert.ToByte(myWord[2], 16) };
            Encoding GB = Encoding.GetEncoding("GB2312");
            Encoding U8 = Encoding.UTF8;
            myByte = Encoding.Convert(GB, U8, myByte);
            char[] Chars = new char[U8.GetCharCount(myByte, 0, myByte.Length)];
            U8.GetChars(myByte, 0, myByte.Length, Chars, 0);
            return new string(Chars);
        }
        #endregion
        //判断否为搜索引擎爬虫,并返回其类型
        public string isCrawler(string SystemInfo)
        {
            string[] BotList = new string[] { "Google", "Baidu", "MSN", "Yahoo", "TMCrawler", "iask", "Sogou" };
            foreach (string Bot in BotList)
            {
                if (SystemInfo.ToLower().Contains(Bot.ToLower()))
                {
                    return Bot;
                }
            }
            return "null";
        }
    }
}
