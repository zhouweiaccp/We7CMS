using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace We7.CMS
{
    /// <summary>
    /// 识别文本编码
    /// </summary>
    public class EncodingReading
    {
        public static Encoding EncodingInfo(string path)
        {
            //对编码格式处理
            FileInfo fileInfo = new FileInfo(path);
            IdentifyEncoding identitfy = new IdentifyEncoding();
            Encoding encoding = Encoding.GetEncoding(identitfy.GetEncodingName(fileInfo));
            return encoding;
        }
    }
}
