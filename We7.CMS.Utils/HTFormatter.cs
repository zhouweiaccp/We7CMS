using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace We7.CMS
{
    /// <summary>
    /// Html格式化类，用于修改文件的根路径
    /// </summary>
    public class HTFormatter
    {
        string fileName;
        string output;
        string root;

        public HTFormatter()
        {
        }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 格式化数据
        /// </summary>
        public string Output
        {
            get { return output; }
            private set { output = value; }
        }

        /// <summary>
        /// 根目录
        /// </summary>
        public string Root
        {
            get { return root; }
            set { root = value; }
        }

        /// <summary>
        /// 格式化
        /// </summary>
        public void Process()
        {
            string input;
            Encoding thisEncode = EncodingReading.EncodingInfo(FileName);
            using (FileStream fs = new FileStream(FileName, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, thisEncode))
                {
                    input = sr.ReadToEnd();
                }
            }
            output = ProcessImage(input);
            output = ProcessLink(output);
            output = ProcessBackground(output);
        }

        /// <summary>
        /// 处理背景
        /// </summary>
        /// <param name="input">要修改的值</param>
        /// <returns></returns>
        string ProcessBackground(string input)
        {
            string rs = @"\<(.*|.*(\r\n).*|.*)(background|BACKGROUND)\s*=\s*(?:""(?<url>[^""]*)""|'(?<url>[^']*)'|(?<url>[^\>^\s]+))(.*|.*(\r\n).*|.*)\>";
            return Process(input, rs);
        }

        /// <summary>
        /// 处理图片
        /// </summary>
        /// <param name="input">要修改的值</param>
        /// <returns></returns>
        string ProcessImage(string input)
        {
            string rs = @"\<(img|IMG)(.*|.*(\r\n).*|.*)(src|SRC)\s*=\s*(?:""(?<url>[^""]*)""|'(?<url>[^']*)'|(?<url>[^\>^\s]+))(.*|.*(\r\n).*|.*)\>";
            return Process(input, rs);
        }

        /// <summary>
        /// 修改链接
        /// </summary>
        /// <param name="input">要值改的值</param>
        /// <returns></returns>
        string ProcessLink(string input)
        {
            string rs = @"\<(link|LINK)(.*|.*(\r\n).*|.*)(href|HREF)\s*=\s*(?:""(?<url>[^""]*)""|'(?<url>[^']*)'|(?<url>[^\>^\s]+))(.*|.*(\r\n).*|.*)\>";
            return Process(input, rs);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input">修改的值</param>
        /// <param name="rs">匹配的正则表达示</param>
        /// <returns></returns>
        string Process(string input, string rs)
        {
            Regex reg = new Regex(rs, RegexOptions.IgnoreCase);
            MatchEvaluator me = new MatchEvaluator(Replace);
            return reg.Replace(input, me);
        }

        /// <summary>
        /// 数据替换
        /// </summary>
        /// <param name="m">匹配</param>
        /// <returns></returns>
        string Replace(Match m)
        {
            string img = m.Groups["url"].Value;
            string full = m.Value;
            if (string.IsNullOrEmpty(img))
                return full;
            return full.Replace(img, String.Format("{0}{1}", root, img));
        }
    }
}
