using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Web;
using System.Text.RegularExpressions;

namespace We7.CMS.Config
{
    /// <summary>
    /// 策略的集合
    /// </summary>
    [Serializable]
    [XmlRoot("configuration")]
    public class StrategyConfigs:AbstractConfig<StrategyConfigs>
    {
        private static StrategyConfigs instance;
        private List<StrategyInfo> items = new List<StrategyInfo>();

        static StrategyConfigs()
        {
            instance = new StrategyConfigs();
            try
            {
                instance =instance.Load();
            }
            catch
            {
            }
        }

        private StrategyConfigs()
        {
        }

        /// <summary>
        /// 索引：根据策略的关键字查询策略
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public StrategyInfo this[string key]
        {
            get
            {
                StrategyInfo result= default(StrategyInfo);
                foreach (StrategyInfo info in Items)
                {
                    if (info.Key == key)
                    {
                        result = info; 
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 配置文件的名称
        /// </summary>
        /// <returns></returns>
        public override string ConfigName()
        {
            return "Strategy";
        }

        /// <summary>
        /// 取得策略配置文件的实例
        /// </summary>
        [XmlIgnore]
        public static StrategyConfigs Instance
        {
            get
            { 
                return instance;
            }
        }

        /// <summary>
        /// 重新加载数据
        /// </summary>
        public void ReLoad()
        {
            instance = instance.Load();
        }

        /// <summary>
        /// 策略配置文件的集合
        /// </summary>
        [XmlElement("item")]
        public List<StrategyInfo> Items 
        {
            get
            {

                return items;
            }
            set
            {
                items = value;
            }
        }

        /// <summary>
        /// 是否包含同名的策略
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsName(string name)
        {
            StrategyInfo result=Items.Find(delegate(StrategyInfo info)
            {
                return info.Name == name ? true : false;
            });
            
            return result==null?false:true;
        }

        /// <summary>
        /// 将IP格式化为15字符长，不足的加0
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string Format15L(string ip)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(ip))
            {
                Regex reg=new Regex("\\d{1,3}",RegexOptions.Compiled);
                MatchCollection mc=reg.Matches(ip);
                if (mc.Count == 4)
                {
                    foreach (Match mt in mc)
                    {
                        string subip="000" + mt.Value;
                        subip=subip.Substring(subip.Length - 3, 3);
                        sb.Append(subip).Append(".");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将IP还原，去掉0
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string FormatOL(string ip)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(ip))
            {
                Regex reg = new Regex("\\d{1,3}", RegexOptions.Compiled);
                MatchCollection mc = reg.Matches(ip);
                if (mc.Count == 4)
                {
                    foreach (Match mt in mc)
                    {
                        string subip = mt.Value.TrimStart("0".ToCharArray());
                        subip = String.IsNullOrEmpty(subip) ? "0" : subip;
                        sb.Append(subip).Append(".");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 添加0,格式化对15字符的长度
        /// </summary>
        /// <param name="rang"></param>
        /// <returns></returns>
        public string Format15LRang(string rang)
        {
            StringBuilder sb = new StringBuilder();
            String[] ips=rang.Split("-".ToCharArray());
            string ipstart,ipend;
            if (ips.Length == 2)
            {
                ipstart = Format15L(ips[0]);
                ipend = Format15L(ips[1]);

                if (!String.IsNullOrEmpty(ipstart) && !String.IsNullOrEmpty(ipend))
                {
                    sb.Append(ipstart).Append("-").Append(ipend);
                }
            }
            else if (!String.IsNullOrEmpty(ips[0]))
            {
                ipstart = Format15L(ips[0]);
                if (!String.IsNullOrEmpty(ipstart))
                {
                    sb.Append(ipstart).Append("-").Append(ipstart);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 去掉多余的0,格式化对原字符的长度
        /// </summary>
        /// <param name="rang"></param>
        /// <returns></returns>
        public string FormatOLRang(string rang)
        {
            StringBuilder sb = new StringBuilder();
            String[] ips = rang.Split("-".ToCharArray());

            if (ips.Length == 2)
            {
                string ipstart = FormatOL(ips[0]);
                string ipend = FormatOL(ips[1]);

                if (!String.IsNullOrEmpty(ipstart) && !String.IsNullOrEmpty(ipend))
                {
                    sb.Append(ipstart);
                    if(ipstart!=ipend)
                        sb.Append("-").Append(ipend);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 检测IP是否在某一个IP段内
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="rang"></param>
        /// <returns></returns>
        public bool CheckIPInRang(string ip, string rang)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(ip) && !String.IsNullOrEmpty(rang))
            {
                ip = Format15L(ip);
                string[] ips = rang.Split("-".ToCharArray());
                if (ips.Length == 2)
                {
                    Regex reg = new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$", RegexOptions.Compiled | RegexOptions.Multiline);
                    if (reg.IsMatch(ips[0]) && reg.IsMatch(ips[1]) && reg.IsMatch(ip))
                    {
                        if (ip.CompareTo(ips[0]) >= 0 && ip.CompareTo(ips[1]) <= 0)
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 检测IP是否在某一个策略内
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="strStrategy"></param>
        /// <returns></returns>
        public StrategyState CheckIPInStragegy(string ip, string strStrategy)
        {
            Regex reg = new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$", RegexOptions.Compiled | RegexOptions.Multiline);

            if (!reg.IsMatch(ip))
                throw new Exception("IP格式不正确");

            StrategyState result = StrategyState.DEFAULT;

            if (!String.IsNullOrEmpty(strStrategy))
            {
                StrategyInfo info=this[strStrategy];
                if (info != null && info.Enable)
                {
                    foreach (string rang in info.DenyIPRang)
                    {
                        if (CheckIPInRang(ip, rang))
                        {
                            return StrategyState.DENY;
                        }
                    }

                    if (info.AllowIPRang.Count == 0)
                    {
                        result = StrategyState.ALLOW;
                    }
                    else
                    {
                        result = Check(delegate(){
                                    foreach (string rang in info.AllowIPRang)
                                    {
                                        if (CheckIPInRang(ip, rang))
                                            return true;
                                    }
                                    return false;
                                }) ? StrategyState.ALLOW : StrategyState.DENY;
                    }                        
                }
            }
            return result;
        }

        /// <summary>
        /// 检测所给IP在指定策略内是否可用
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="strStrategy"></param>
        /// <returns></returns>
        public StrategyState IsAllow(string ip, string strStrategy)
        {
            StrategyState state=StrategyState.DEFAULT;

            if (!String.IsNullOrEmpty(strStrategy))
            {
                string[] strs = strStrategy.Split("|".ToCharArray());
                int flag = 0;
                for (int i=0;i < strs.Length; i++)
                {
                    string str = strs[i];
                    StrategyState s = CheckIPInStragegy(ip, str);
                    switch (s)
                    {
                        case StrategyState.DEFAULT:
                            break;
                        case StrategyState.ALLOW:
                            return StrategyState.ALLOW;
                        case StrategyState.DENY:
                            state = StrategyState.DENY;
                            break;
                    }
                }
            }
            return state;
        }
    }
}
/// <summary>
/// 策略的类型
/// </summary>
public enum StrategyMode
{
    /// <summary>
    /// 文章
    /// </summary>
    ARTICLE,
    /// <summary>
    /// 栏目
    /// </summary>
    CHANNEL,
    /// <summary>
    /// 常规
    /// </summary>
    CONVENTION
}

public enum StrategyState
{
    /// <summary>
    /// 默认
    /// </summary>
    DEFAULT,
    /// <summary>
    /// 允许
    /// </summary>
    ALLOW,
    /// <summary>
    /// 禁止
    /// </summary>
    DENY
}
