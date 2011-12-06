using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework.Config
{
    /// <summary>
    /// 网站基本设置描述类, 加[Serializable]标记为可序列化
    /// </summary>
    [Serializable]
    public class  AdviceEmailConfigInfo
    {
        /// <summary>
        /// email类型，对应 item节点value属性值，不可以重复
        /// </summary>
        public string EmailType { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string EmailTitle { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string EmailContent { get; set; }        
    }
}
