using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web.Caching;
using System.Web;
using System.Collections.ObjectModel;

namespace We7.CMS.Config
{
    /// <summary>
    /// 注册模块信息
    /// </summary>
    public class AccountModelInfo:Collection<AccountModelItem>
    {
        static string ACCOUNTMODELINFO="____ACCOUNTMODELINFO";
        public static AccountModelInfo Instance
        {
            get
            {
                AccountModelInfo info=Cache[ACCOUNTMODELINFO] as AccountModelInfo;
                if (info == null)
                {
                    info = new AccountModelInfo();
                    info.Load();
                    Cache.Insert(ACCOUNTMODELINFO,info,new CacheDependency(FilePath));
                }
                return info;
            }
        }
        private AccountModelInfo() { }

        public AccountModelItem this[string key]
        {
            get
            {
                foreach (AccountModelItem item in this)
                {
                    if (item.Value == key)
                        return item;
                }
                return null;
            }
        }

        public AccountModelInfo Load()
        {
            Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load(FilePath);
            XmlNodeList list=doc.DocumentElement.SelectNodes("item");
            foreach (XmlElement node in list)
            {
                Add(new AccountModelItem().Load(node));
            }
            return this;
        }

        static string FilePath
        {
            get { return HttpContext.Current.Server.MapPath("~/Config/AccountModel.xml"); }
        }

        static Cache Cache
        {
            get
            {
                return HttpContext.Current.Cache ?? HttpRuntime.Cache;
            }
        }
    }

    /// <summary>
    /// 注册模块信息条目项
    /// </summary>
    public class AccountModelItem
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string ModelPath { get; set; }

        public bool AllowRegister { get; set; }

        public AccountModelItem Load(XmlElement xe)
        {
            Name = xe.GetAttribute("name");
            Value = xe.GetAttribute("value");
            ModelPath = xe.GetAttribute("modelpath");
            AllowRegister = !String.IsNullOrEmpty(xe.GetAttribute("allowregister")) || xe.GetAttribute("allowregister").ToLower() == "true";
            return this;
        }
    }
}
