using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web;
using System.Collections.Specialized;
using We7.CMS.Config;
using We7.CMS.Common.Enum;
using System.Data;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Common
{
    /// <summary>
    /// 点击量的日记录汇总类
    /// </summary>
    [Serializable]
    public class ClickRecords
    {
        string id;
        string objectType;
        string objectID;
        int visitDate;
        int clicks;

        /// <summary>
        /// Constructor
        /// </summary>
        public ClickRecords()
        {
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 对象类型
        /// </summary>
        public string ObjectType
        {
            get { return objectType; }
            set { objectType = value; }
        }

        /// <summary>
        /// 对象ID
        /// </summary>
        public string ObjectID
        {
            get { return objectID; }
            set { objectID = value; }
        }

        /// <summary>
        /// 访问日期
        /// </summary>
        public int VisitDate
        {
            get { return visitDate; }
            set { visitDate = value; }
        }

        /// <summary>
        /// 某一日期的流量统计
        /// </summary>
        public int Clicks
        {
            get { return clicks; }
            set { clicks = value; }
        }
    }
}
