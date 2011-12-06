using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Data;
using System.Globalization;
using System.Collections;
using System.Reflection;
using Thinkment.Data;
using We7.Framework;
using We7.CMS.Web.Admin.Ajax.BusinessSubmit.Entity;
using We7.CMS.Web.Admin.Ajax.BusinessSubmit.Data;

namespace We7.CMS.Web.Admin.Ajax.BusinessSubmit
{
    public class JsonResult : IJsonResult
    {
        private We7.CMS.Web.Admin.Ajax.BusinessSubmit.Data.IDataBase iDatabase;
        /// <summary>
        /// 数据业务接口
        /// </summary>
        public We7.CMS.Web.Admin.Ajax.BusinessSubmit.Data.IDataBase IDatabase
        {
            get
            {
                if (iDatabase == null)
                {
                    iDatabase = new DataBaseAssistant() as We7.CMS.Web.Admin.Ajax.BusinessSubmit.Data.IDataBase;
                }
                return iDatabase;
            }
        }
        public string code()
        {
            return "\"code\":\"{0}\"";
        }

        public string Message()
        {
            return "\"message\":\"{1}\"";
        }
        public string info()
        {
            return "{" + code() + "," + Message() + "}";
        }
        public string ToJson(IQueryCondition condition)
        {
            QueryCondition queryEntity = condition as QueryCondition;
            TableInfo ti = new TableInfo();
            TableInfo.TableName = queryEntity.Tb;  //设置表名
            TableInfo.ID = queryEntity.ID;  //设置ID
            TableInfo.Fileds = queryEntity.ConditionDic;
            string returnJson = string.Empty;
         
            switch (queryEntity.Oper)
            {
                case Enum_operType.Add:

                    break;
                case Enum_operType.Del:
                    try
                    {
                        IDatabase.Delete(ti);
                        returnJson =info().Replace("{0}","200").Replace("{1}","数据已删除");
                    }
                    catch (Exception ex)
                    {
                        returnJson = info().Replace("{0}", "300").Replace("{1}", ex.Message);
                    }
                    break;
                case Enum_operType.Seach:
                    try
                    {
                        Order[] o;
                        if (string.IsNullOrEmpty(queryEntity.Sort))
                        {
                            o = new Order[] {new Order("ID",OrderMode.Asc) };
                        }
                        else
                        {
                            o = new Order[] { new Order("ID", OrderMode.Asc), new Order(queryEntity.Sort,(OrderMode)queryEntity.Sord) };
                        }
                        
                        if (queryEntity.Page != -1)
                        {
                            queryEntity.total = IDatabase.Total(queryEntity.ConditionForCriteria);  //总记录数
                        }
                        List<TableInfo> aList = IDatabase.GetDtByCondition<TableInfo>(queryEntity.ConditionForCriteria, o, queryEntity.Begin - 1, queryEntity.Count, queryEntity.F);
                        returnJson = aList[0].ToJson().Replace("{0}", queryEntity.total.ToString()).Replace("{1}", "200").Replace("{2}", "数据成功返回").Replace("{3}", queryEntity.Page.ToString()).Replace("{4}",queryEntity.totalPage.ToString());
                    }
                    catch (Exception ex)
                    {
                        returnJson = info().Replace("{0}", "300").Replace("{1}", ex.Message);
                    }
                    break;
                case Enum_operType.Update:
                    try
                    {
                        IDatabase.Update(ti, queryEntity.F);
                        returnJson  = info().Replace("{0}", "200").Replace("{1}", "数据已修改");
                    }
                    catch (Exception ex)
                    {
                        returnJson  = info().Replace("{0}", "300").Replace("{1}", ex.Message);
                    }
                    break;
                default:
                    break;
            }
            return returnJson;
        }
    }
}