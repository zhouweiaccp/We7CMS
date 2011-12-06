using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Data;

using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using Thinkment.Data;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.CMS.Accounts;

namespace We7.CMS
{
    /// <summary>
    /// 文章（信息）发表报表生成类
    /// </summary>
    [Serializable]
    [Helper("We7.PublishReportHelper")]
    public class PublishReportHelper : BaseHelper
    {
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID]; }
        }
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// 获取某用户的发表数
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public PublishCountView GetPublishCountByAccount(string accountID, DateTime begin, DateTime end)
        {
            List<PublishCountView> list = GePublishCountList("Account", "", begin, end, string.Format(" [AccountID] = '{0}'", accountID));
            if (list.Count > 0)
            {
                List<PublishCountView> list2 = GePublishCountList("Account", "", begin, end, string.Format(" [AccountID] = '{0}' AND [State]=1 ", accountID));
                if (list2.Count > 0)
                    list[0].AcceptCount = list2[1].Count;
                return list[0];
            }
            else
                return null;
        }

        public List<PublishCountView> GePublishCountList(string keyName, string keyValue, DateTime begin, DateTime end)
        {
            return GePublishCountList(keyName, keyValue, begin, end, "");
        }
        /// <summary>
        /// 获取发表数列表
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="keyValue"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        public List<PublishCountView> GePublishCountList(string keyName, string keyValue, DateTime begin, DateTime end, string filterString)
        {
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            SqlStatement sql = new SqlStatement();
            string sqlCommandTxt = "", strWhere = "";
            switch (keyName)
            {
                case "Account":
                    sqlCommandTxt =
                    @"SELECT DISTINCT [AccountID], COUNT([ID]) AS [pubcount], SUM([Clicks]) AS [clickcount]
           FROM [Article]  {0}  
           GROUP BY [AccountID]  ORDER BY COUNT([ID])  DESC";
                    if (keyValue == "all")
                    {
                        strWhere = " WHERE [AccountID] <> '' or [AccountID] is not null";

                    }
                    else
                    {
                        strWhere = " WHERE [AccountID] = '" + keyValue + "'  ";
                    }
                    break;
                case "Depratment":
                    sqlCommandTxt =
              @"SELECT DISTINCT [AccountID], COUNT([ID]) AS [pubcount], SUM([Clicks]) AS [clickcount]
           FROM [Article]  {0}  
           GROUP BY [AccountID]  ORDER BY COUNT([ID])  DESC";
                    strWhere = " WHERE [AccountID] <> '' or [AccountID] is not null";
                    break;

                case "Channel":
                    sqlCommandTxt =
                        @"SELECT DISTINCT [ChannelFullUrl], COUNT(*) as [pubcount], SUM([Clicks])as [clickcount]  
                        FROM [Article]  {0}
                        GROUP BY  [ChannelFullUrl] ORDER BY COUNT(*) DESC";
                    if (keyValue == "{00000000-0000-0000-0000-000000000000}")
                    {
                        strWhere = " WHERE [OwnerID] <> '' or [OwnerID] is not null";

                    }
                    else
                    {
                        string chFullUrl = ChannelHelper.GetFullUrl(keyValue);
                        strWhere = " WHERE [ChannelFullUrl] LIKE '" + chFullUrl+ "%' ";
                        //strWhere = " WHERE [OwnerID] = '" + keyValue + "'  ";
                    }
                    break;
            }

            if (filterString != "") strWhere += " AND " + filterString;
            string dataWhere = "";
            if (begin <= end)
            {
               
                if (begin != DateTime.MinValue)
                {
                    dataWhere += " and [Created] > {0}BEGIN";
                    DataParameter dp = new DataParameter();
                    dp.ParameterName = db.DbDriver.Prefix + "BEGIN";
                    dp.DbType = DbType.DateTime;
                    dp.SourceColumn = "Created";
                    dp.Value = begin;
                    sql.Parameters.Add(dp);
                }
                if (end != DateTime.MaxValue)
                {
                    dataWhere += " and [Created] < {0}END";
                    DataParameter dp2 = new DataParameter();
                    dp2.ParameterName = db.DbDriver.Prefix + "END";
                    dp2.Value = end.AddDays(1);
                    dp2.DbType = DbType.DateTime;
                    dp2.SourceColumn = "Created";
                    sql.Parameters.Add(dp2);
                }
                dataWhere = string.Format(dataWhere, db.DbDriver.Prefix);
            }
            //if (keyValue.Trim() != "")
            //{
            //    strWhere += " and [{1}] like {0}KEY";
            //    DataParameter dp3 = new DataParameter();
            //    dp3.ParameterName = db.DbDriver.Prefix + "KEY";
            //    dp3.Value = "%" + keyValue.Trim() + "%";
            //    dp3.DbType = DbType.String;
            //    sql.Parameters.Add(dp3);
            //}

            strWhere += dataWhere;
            sqlCommandTxt = string.Format(sqlCommandTxt, strWhere);
            sql.SqlClause = sqlCommandTxt;

            db.DbDriver.FormatSQL(sql);
            List<PublishCountView> list = new List<PublishCountView>();
            using (IConnection conn = db.CreateConnection())
            {
                DataTable dt = conn.Query(sql);
                int total = 0, total2 = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    switch (keyName)
                    {
                        case "Account":
                            AccountDataToList(dt, list, out total, out total2);
                            break;
                        case "Depratment":
                            DepartmentDataToList(dt, list, out total, out total2);
                            break;
                        case "Channel":
                            ChannelDataToList(dt, list, out total, out total2, keyValue);
                            break;
                    }

                    foreach (PublishCountView v in list)
                    {
                        if (total > 0)
                            v.Percent = (double)v.Count / (double)total * 100;
                        if (total2 > 0)
                            v.ClickPercent = (double)v.Clicks / (double)total2 * 100;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 用户-数据赋值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="list"></param>
        void AccountDataToList(DataTable dt, List<PublishCountView> list, out int total, out int total2)
        {
            total = 0; total2 = 0;
            foreach (DataRow dr in dt.Rows)
            {
                PublishCountView kv = new PublishCountView();
                kv.KeyValue = dr["AccountID"].ToString();
                string loginName = AccountHelper.GetAccount(kv.KeyValue, new string[] { "LoginName" }).LoginName;
                if(loginName !=null && loginName !="")
                {
                    kv.KeyValue=loginName;
                }
                else
                {
                     kv.KeyValue = "管理员";
                }
               
                kv.Count = Int32.Parse(dr["pubcount"].ToString());
                kv.Clicks = Int32.Parse(dr["clickcount"].ToString());
                total += kv.Count;
                total2 += kv.Clicks;
                list.Add(kv);
            }
        }

        /// <summary>
        /// 部门-数据赋值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="list"></param>
        void DepartmentDataToList(DataTable dt, List<PublishCountView> list, out int total, out int total2)
        {
            total = 0; total2 = 0;
            string siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
            List<Department> departments = AccountHelper.GetDepartments(siteID, We7Helper.EmptyGUID, "",new string[] { "FullName", "Name", "ParentID" });
            foreach (Department dep in departments)
            {
                PublishCountView kv = new PublishCountView();
                kv.KeyValue = dep.Name;
                foreach (DataRow dr in dt.Rows)
                {
                    Account acc = AccountHelper.GetAccount(dr["AccountID"].ToString(), new string[] { "ID", "DepartmentID" });
                    if (acc != null)
                    {
                        Department d = AccountHelper.GetDepartment(acc.DepartmentID, new string[] { "FullName", "ID", });
                        if (d != null && d.FullName.ToLower().StartsWith(dep.FullName.ToLower()))
                        {
                            kv.Count += Int32.Parse(dr["pubcount"].ToString());
                            kv.Clicks += Int32.Parse(dr["clickcount"].ToString());
                        }
                    }
                }

                total += kv.Count;
                total2 += kv.Clicks;
                list.Add(kv);
            }

            list.Sort(new DinoComparer());
        }

        /// <summary>
        /// 栏目-数据赋值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="list"></param>
        void ChannelDataToList(DataTable dt, List<PublishCountView> list, out int total, out int total2,string keyValue)
        {
            total = 0;  total2 = 0;
            string name = "";
            List<Channel> channels=new List<Channel>();
            if (We7Helper.EmptyGUID == keyValue)
            {
                channels = ChannelHelper.GetChannels(We7Helper.EmptyGUID);
            }
            else if (keyValue == "")
            { }
            else
            {
                Channel ch=ChannelHelper.GetChannel(keyValue,null);
                name = ch.Name;
                channels.Add(ch);
                string chFullUrl = ChannelHelper.GetFullUrl(keyValue);
                List<Channel> channelList= ChannelHelper.GetChannels(keyValue);
                if (channelList != null && channelList.Count >0)
                    channels.AddRange(channelList);
            }
            if (channels != null && channels.Count > 0)
            {
                foreach (Channel ch in channels)
                {
                    PublishCountView kv = new PublishCountView();
                    if (name == ch.Name) {
                        continue;
                    }
                    //kv.KeyValue = ch.Name;
                    else
                        kv.KeyValue = ch.Name;

                    kv.ChannelID = ch.ID;
                   
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["ChannelFullUrl"].ToString().ToLower().StartsWith(ch.FullUrl.ToLower()))
                        {
                            kv.Count += Int32.Parse(dr["pubcount"].ToString());
                            kv.Clicks += Int32.Parse(dr["clickcount"].ToString());
                        }
                    }

                    total += kv.Count;
                    total2 += kv.Clicks;
                    list.Add(kv);
                }

                list.Sort(new DinoComparer());
            }
        }

        class DinoComparer : IComparer<PublishCountView>
        {
            public int Compare(PublishCountView x, PublishCountView y)
            {
                if (y == null)
                {
                    if (x == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (x == null)
                    {
                        return 1;
                    }
                    else
                    {
                        int retval = y.Count.CompareTo(x.Count);

                        if (retval != 0)
                        {
                            return retval;
                        }
                        else
                        {
                            return y.Count.CompareTo(x.Count);
                        }
                    }
                }
            }
        }
    }
}
