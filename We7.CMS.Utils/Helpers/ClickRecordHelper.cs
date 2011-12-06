using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Web.Caching;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using Thinkment.Data;
using System.Threading;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// 对象点击量更新接口
    /// </summary>
    public interface IObjectClickHelper
    {
        /// <summary>
        /// 更新指定对象的点击量报表
        /// </summary>
        /// <param name="modelName">模块名称</param>
        /// <param name="id">文章ID</param>
        /// <param name="dictClickReport">点击量报表</param>
        void UpdateClicks(string modelName, string id, Dictionary<string, int> dictClickReport);
    }
    
    /// <summary>
    /// 点击量对象更新器工厂
    /// </summary>
    public class ClickHelperFactory
    {
        public static IObjectClickHelper Create(string modelName)
        {
            if (string.IsNullOrEmpty(modelName))
            {
                return HelperFactory.Instance.GetHelper<ArticleHelper>();
            }
            else if (String.Compare(modelName, "Advice", true) == 0)
            {

                return AdviceFactory.Create() as IObjectClickHelper;
            }
            else
            {
                return HelperFactory.Instance.GetHelper<ArticleHelper>();
            }            
        }
    }

    /// <summary>
    /// 点击量业务类接口
    /// </summary>
    public interface IClickRecordHelepr
    {
        /// <summary>
        /// 获取指定类型对象某天的点击量数据
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectID">对象ID</param>
        /// <param name="dateInteger">日期的整型表示,如20110215</param>
        /// <returns>如果没有找到返回null</returns>
        ClickRecords GetClickRecordByDate(string objectType, string objectID, int dateInteger);
        /// <summary>
        /// 保存一条点击量数据记录，有则更新，无则插入
        /// </summary>
        /// <param name="cr">点击量数据</param>
        void SaveClickRecord(ClickRecords cr);
        /// <summary>
        /// 将日期类型转换为整型表示
        /// </summary>
        /// <param name="date">日期类型</param>
        /// <returns>日期的整型格式为8位，如 20110215</returns>
        int ConvertIntegerFromDate(DateTime date);
        /// <summary>
        /// 获取某对象的点击量记录
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectID">对象ID</param>
        /// <returns>点击量记录</returns>
        List<ClickRecords> GetObjectClickRecords(string objectType, string objectID);
        /// <summary>
        /// 获取某对象的点击量键值对报表
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectID">对象ID</param>
        /// <returns>日点击量;昨日点击量;周点击量;月点击量;季点击量;年点击量;总点击量</returns>
        Dictionary<string, int> GetObjectClickReport(string objectType, string objectID);
    }

    /// <summary>
    /// 点击量业务类
    /// </summary>
    [Serializable]
    [Helper("We7.ClickRecordHelper")]
    public class ClickRecordHelper : BaseHelper,IClickRecordHelepr
    {
        private static object syncRoot = new object();        

        /// <summary>
        /// 获取指定类型对象某天的点击量数据
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectID">对象ID</param>
        /// <param name="dateInteger">日期的整型表示,如20110215</param>
        /// <returns>如果没有找到返回null</returns>
        #region ClickRecords GetClickRecordByDate(string objectType, string objectID, int dateInteger)
        public ClickRecords GetClickRecordByDate(string objectType, string objectID, int dateInteger)
        {
            Criteria cTmp = new Criteria(CriteriaType.Equals, "ObjectType", objectType);
            cTmp.Add(CriteriaType.Equals, "ObjectID", objectID);
            cTmp.Add(CriteriaType.Equals, "VisitDate", dateInteger);
            List<ClickRecords> cList = Assistant.List<ClickRecords>(cTmp, null);
            
            if (cList != null && cList.Count > 0)
                return cList[0];
            return null;
        }
        #endregion

        /// <summary>
        /// 保存一条点击量数据记录，有则更新，无则插入
        /// </summary>
        /// <param name="cr">点击量数据</param>
        #region void SaveClickRecord(ClickRecords cr)
        public void SaveClickRecord(ClickRecords cr)
        {
            ClickRecords clickRecord = GetClickRecordByDate(cr.ObjectType, cr.ObjectID, cr.VisitDate);
            if (clickRecord == null)
            {
                lock (syncRoot)
                {
                    clickRecord = GetClickRecordByDate(cr.ObjectType, cr.ObjectID, cr.VisitDate);
                    if (clickRecord == null)
                    {
                        //插入逻辑
                        cr.ID = We7Helper.CreateNewID();
                        cr.Clicks = 1;                        
                        Assistant.Insert(cr);
                    }
                    else
                    {
                        //更新逻辑
                        clickRecord.Clicks += 1;
                        Assistant.Update(clickRecord, new string[] { "Clicks" });
                    }
                }
            }
            else
            {
                //更新逻辑
                clickRecord.Clicks += 1;
                Assistant.Update(clickRecord, new string[] { "Clicks" });
            }
        }
        #endregion

        /// <summary>
        /// 将日期类型转换为整型表示
        /// </summary>
        /// <param name="date">日期类型</param>
        /// <returns>日期的整型格式为8位，如 20110215</returns>
        #region int ConvertIntegerFromDate(DateTime date)
        public int ConvertIntegerFromDate(DateTime date)
        {
            int dateInteger = Convert.ToInt32(date.ToString("yyyyMMdd"));
            return dateInteger;
        }
        #endregion

        /// <summary>
        /// 获取某对象的点击量记录
        /// </summary> 
        /// <param name="objectType">对象类型</param>
        /// <param name="objectID">对象ID</param>
        /// <returns>点击量记录</returns>
        #region List<ClickRecords> GetObjectClickRecords(string objectType, string objectID)
        public List<ClickRecords> GetObjectClickRecords(string objectType, string objectID)
        {
            Criteria cTmp = new Criteria(CriteriaType.Equals, "ObjectType", objectType);
            cTmp.Add(CriteriaType.Equals, "ObjectID", objectID);
            List<ClickRecords> cList = Assistant.List<ClickRecords>(cTmp, null);
            
            return cList;
        }
        #endregion

        /// <summary>
        /// 获取某对象的点击量键值对报表
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectID">对象ID</param>
        /// <returns>日点击量;昨日点击量;周点击量;月点击量;季点击量;年点击量;总点击量</returns>
        #region Dictionary<string, int> GetObjectClickReport(string objectType, string objectID)
        public Dictionary<string, int> GetObjectClickReport(string objectType, string objectID)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            List<ClickRecords> listAll = GetObjectClickRecords(objectType, objectID);
            if (listAll != null && listAll.Count > 0)
            {
                int todayInteger = ConvertIntegerFromDate(DateTime.Now);
                int yesterdayInteger =ConvertIntegerFromDate(DateTime.Now.AddDays(-1));
                int lastweekInterger = ConvertIntegerFromDate(DateTime.Now.AddDays(-7));
                int lastmonthInteger = ConvertIntegerFromDate(DateTime.Now.AddMonths(-1));
                int lastseasonInteger = ConvertIntegerFromDate(DateTime.Now.AddMonths(-3));
                int lastyearInteger = ConvertIntegerFromDate(DateTime.Now.AddYears(-1));
                int sum = 0;
                //日点击量
                ClickRecords cr = listAll.Find(p => p.VisitDate == todayInteger);
                dict.Add("日点击量", cr!=null ? cr.Clicks : 0);
                //昨日点击量
                cr = listAll.Find(p => p.VisitDate == yesterdayInteger);
                dict.Add("昨日点击量", cr != null ? cr.Clicks : 0);
                //周点击量
                List<ClickRecords> listTmp = listAll.FindAll(p => p.VisitDate > lastweekInterger && p.VisitDate <= todayInteger);
                if (listTmp != null && listTmp.Count > 0)
                {
                    sum=0;
                    foreach (ClickRecords c in listTmp)
                        sum += c.Clicks;
                    dict.Add("周点击量", sum);
                }
                else
                    dict.Add("周点击量", 0);
                //月点击量
                listTmp = listAll.FindAll(p => p.VisitDate > lastmonthInteger && p.VisitDate <= todayInteger);
                if (listTmp != null && listTmp.Count > 0)
                {
                    sum = 0;
                    foreach (ClickRecords c in listTmp)
                        sum += c.Clicks;
                    dict.Add("月点击量", sum);
                }
                else
                    dict.Add("月点击量", 0);
                //季点击量
                listTmp = listAll.FindAll(p => p.VisitDate > lastseasonInteger && p.VisitDate <= todayInteger);
                if (listTmp != null && listTmp.Count > 0)
                {
                    sum = 0;
                    foreach (ClickRecords c in listTmp)
                        sum += c.Clicks;
                    dict.Add("季点击量", sum);
                }
                else
                    dict.Add("季点击量", 0);
                //年点击量
                listTmp = listAll.FindAll(p => p.VisitDate > lastyearInteger && p.VisitDate <= todayInteger);
                if (listTmp != null && listTmp.Count > 0)
                {
                    sum = 0;
                    foreach (ClickRecords c in listTmp)
                        sum += c.Clicks;
                    dict.Add("年点击量", sum);
                }
                else
                    dict.Add("年点击量", 0);
                //总点击量
                sum = 0;
                foreach (ClickRecords c in listAll)
                    sum += c.Clicks;
                dict.Add("总点击量", sum);
            }

            return dict;
        }
        #endregion
    }
}
