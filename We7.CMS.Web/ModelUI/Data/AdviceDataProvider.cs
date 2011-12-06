using System;
using System.Collections.Generic;
using System.Web;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.CMS;
using System.IO;
using Thinkment.Data;
using We7.Model.Core;
using System.Data;
using We7.Framework;
using We7.CMS.Accounts;

namespace We7.Model.UI.Data
{
    public class AdviceDataProvider : BaseDataProvider
    {
        public override bool Insert(We7.Model.Core.PanelContext data)
        {
            Advice advice = new Advice();
            advice.OwnID = Security.CurrentAccountID;
            //advice.TypeID = GetAdviceTypeID(data.ModelName);
            //advice.ID = GetValue<string>(data, "ID");
            advice.Title = GetValue<string>(data, "Title");
            advice.UserID = GetValue<string>(data, "UserID");
            advice.Content = GetValue<string>(data, "Content");
            advice.CreateDate = DateTime.Now;
            advice.Updated = DateTime.Now;

            advice.Name = GetValue<string>(data, "Name");
            advice.Email = GetValue<string>(data, "Email");
            advice.Address = GetValue<string>(data, "Address");
            advice.Phone = GetValue<string>(data, "Phone");
            advice.Fax = GetValue<string>(data, "Fax");

            advice.State = (int)AdviceState.WaitAccept;
            advice.SN = AdviceHelper.CreateArticleSN();
            int isshow;
            Int32.TryParse(GetValue<string>(data, "IsShow"), out isshow);
            string stateStr = StateMgr.StateProcess(advice.EnumState, EnumLibrary.Business.AdviceDisplay, isshow);
            advice.IsShow = isshow;
            advice.EnumState = stateStr;
            advice.Display1 = GetValue<string>(data, "Display1");
            advice.Display2 = GetValue<string>(data, "Display2");
            advice.Display3 = GetValue<string>(data, "Display3");
            if (advice.SN < 100000)
            {
                advice.SN = advice.SN + 100000;
            }
            advice.MyQueryPwd = We7Helper.CreateNewID().Substring(1, 8);

            //下面是添加模型信息
            string config, schema;
            advice.ModelXml = GetModelDataXml(data, advice.ModelXml, out schema, out config);//获取模型数据
            advice.ModelConfig = config;
            advice.ModelName = data.ModelName;
            advice.ModelSchema = schema;
            advice.TypeID = data.Objects["AdviceTypeID"] as string;
            if (String.IsNullOrEmpty(advice.TypeID))
            {
                AdviceType type = AdviceTypeHelper.GetAdviceTypeByModelName(data.ModelName);
                if(type!=null)
                advice.TypeID = type.ID;
            }

            AdviceHelper.AddAdvice(advice);

            SetValue(data, "ID", advice.ID);

            try
            {
                AdviceHelper.SendNotifyMail(advice.ID);
            }
            catch { }
            finally { }
            return true;
        }

        public  bool InsertEmailAdvice(We7.Model.Core.PanelContext data)
        {
            Advice advice = new Advice();
            advice.OwnID = Security.CurrentAccountID;
            //advice.TypeID = GetAdviceTypeID(data.ModelName);
            //advice.ID = GetValue<string>(data, "ID");
            advice.Title = GetValue<string>(data, "Title");
            advice.UserID = GetValue<string>(data, "UserID");
            advice.Content = GetValue<string>(data, "Content");
            advice.CreateDate = DateTime.Now;
            advice.Updated = DateTime.Now;

            advice.Name = GetValue<string>(data, "Name");
            advice.Email = GetValue<string>(data, "Email");
            advice.Address = GetValue<string>(data, "Address");
            advice.Phone = GetValue<string>(data, "Phone");
            advice.Fax = GetValue<string>(data, "Fax");

            advice.State = (int)AdviceState.WaitAccept;
            advice.SN = AdviceHelper.CreateArticleSN();
            int isshow;
            Int32.TryParse(GetValue<string>(data, "IsShow"), out isshow);
            string stateStr = StateMgr.StateProcess(advice.EnumState, EnumLibrary.Business.AdviceDisplay, isshow);
            advice.IsShow = isshow;
            advice.EnumState = stateStr;
            advice.Display1 = GetValue<string>(data, "Display1");
            advice.Display2 = GetValue<string>(data, "Display2");
            advice.Display3 = GetValue<string>(data, "Display3");
            if (advice.SN < 100000)
            {
                advice.SN = advice.SN + 100000;
            }
            advice.MyQueryPwd = We7Helper.CreateNewID().Substring(1, 8);

            //下面是添加模型信息
            string config, schema;
            advice.ModelXml = GetModelDataXml(data, advice.ModelXml, out schema, out config);//获取模型数据
            advice.ModelConfig = config;
            advice.ModelName = data.ModelName;
            advice.ModelSchema = schema;
            advice.TypeID = GetValue<string>(data, "TypeID");
            if (String.IsNullOrEmpty(advice.TypeID))
            {
                AdviceType type = AdviceTypeHelper.GetAdviceTypeByModelName(data.ModelName);
                if (type != null)
                advice.TypeID = type.ID;
            }

            AdviceHelper.AddAdvice(advice);

            SetValue(data, "ID", advice.ID);
            try
            {
                AdviceHelper.SendNotifyMail(advice.ID);
            }
            catch { }
            finally { }
            return true;
        }

        public override bool Update(We7.Model.Core.PanelContext data)
        {
            string id = GetValue<string>(data, "ID");
            Advice advice = AdviceHelper.GetAdvice(id);
            if (advice == null)
            {
                Insert(data);
            }
            else
            {
                advice.Title = GetValue<string>(data, "Title");
                advice.UserID = GetValue<string>(data, "UserID");
                advice.Content = GetValue<string>(data, "Content");
                advice.Updated = DateTime.Now;
                advice.Name = GetValue<string>(data, "Name");
                advice.Email = GetValue<string>(data, "Email");
                advice.Address = GetValue<string>(data, "Address");
                advice.Phone = GetValue<string>(data, "Phone");
                advice.Fax = GetValue<string>(data, "Fax");

                int isshow;
                Int32.TryParse(GetValue<string>(data, "IsShow"), out isshow);
                string stateStr = StateMgr.StateProcess(advice.EnumState, EnumLibrary.Business.AdviceDisplay, isshow);
                advice.IsShow = isshow;
                advice.EnumState = stateStr;

                advice.Display1 = GetValue<string>(data, "Display1");
                advice.Display2 = GetValue<string>(data, "Display2");
                advice.Display3 = GetValue<string>(data, "Display3");

                //下面是添加模型信息
                string config, schema;
                advice.ModelXml = GetModelDataXml(data, advice.ModelXml, out schema, out config);//获取模型数据
                advice.ModelConfig = config;
                advice.ModelName = data.ModelName;
                advice.ModelSchema = schema;
                advice.TypeID = data.Objects["AdviceTypeID"] as string;

                List<string> udpatedFields = new List<string>() { "ModelXml", "ModelName", "ModelSchema", "TypeID", "Title", "UserID", "Content", "Updated", "Name", "Email", "Phone", "Address", "Fax", "Display1", "Display2", "Display3"};
                if (!String.IsNullOrEmpty(GetValue<string>(data, "IsShow")))
                {
                    udpatedFields.Add("IsShow");
                }
                
                AdviceHelper.UpdateAdvice(advice, udpatedFields.ToArray());
                SetValue(data, "ID", advice.ID);
            }
            return true;
        }

        public override bool Delete(We7.Model.Core.PanelContext data)
        {
            try
            {
                string adviceid = data.DataKey["ID"] as string;
                AdviceHelper.DeleteAdvice(adviceid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public override System.Data.DataTable Query(We7.Model.Core.PanelContext data, out int recordcount, ref int pageindex)
        {
            DataSet ds = CreateDataSet(data.Model);

            recordcount = GetCount(data);
            int startindex, itemscount;
            We7.Framework.Util.Utils.BuidlPagerParam(recordcount, data.PageSize, ref pageindex, out startindex, out itemscount);
            List<Advice> list = AdviceHelper.Assistant.List<Advice>(CreateCriteria(data), new Order[] { new Order("Updated", OrderMode.Desc) }, startindex, itemscount);
            foreach (Advice a in list)
            {
                if (String.IsNullOrEmpty(a.ModelXml))
                    continue;
                ReadXml(ds, a.ModelXml);
                DataRowCollection rows = ds.Tables[data.Table.Name].Rows;
                if (rows.Count > 0)
                {
                    rows[rows.Count - 1][OBJECTCOLUMN] = a;
                }
            }
            return ds.Tables[data.Table.Name];
        }

        public override System.Data.DataRow Get(We7.Model.Core.PanelContext data)
        {
            DataRow row = null;
            string id = data.DataKey["ID"] as string;
            if (!String.IsNullOrEmpty(id))
            {
                Advice advice =AdviceHelper.GetAdvice(id);
                if (advice != null && !String.IsNullOrEmpty(advice.ModelXml))
                {
                    DataSet ds = CreateDataSet(data.Model);
                    ReadXml(ds, advice.ModelXml);
                    row = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
                    if (row != null)
                    {
                        row[OBJECTCOLUMN] = advice;
                    }
                }
            }
            return row;
        }

        public override int GetCount(We7.Model.Core.PanelContext data)
        {
            return AdviceHelper.Assistant.Count<Advice>(CreateCriteria(data));
        }

        string GetAdviceTypeID(string ModelName)
        {
            List<AdviceType> list = AdviceTypeHelper.GetAdviceTypes();
            foreach (AdviceType advicetype in list)
            {
                if (advicetype.ModelName== ModelName)
                    return advicetype.ID;
            }
            return String.Empty;
        }
    }
}
