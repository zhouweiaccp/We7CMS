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
    public class AdviceDataProviderEx : BaseDataProvider
    {
        public override bool Insert(We7.Model.Core.PanelContext data)
        {
            AdviceInfo advice = new AdviceInfo();
            advice.Title = GetValue<string>(data, "Title");
            advice.UserID = GetValue<string>(data, "UserID");
            advice.Content = GetValue<string>(data, "Content");
            advice.Created = DateTime.Now;
            advice.Name = GetValue<string>(data, "Name");
            advice.Email = GetValue<string>(data, "Email");
            advice.Address = GetValue<string>(data, "Address");
            advice.Phone = GetValue<string>(data, "Phone");
            advice.Fax = GetValue<string>(data, "Fax");
            advice.RelationID = GetValue<string>(data, "RelationID");
            advice.State = 0; //待受理
            int isshow;
            Int32.TryParse(GetValue<string>(data, "IsShow"), out isshow);
            advice.IsShow = isshow;
            advice.Display1 = GetValue<string>(data, "Display1");
            advice.Display2 = GetValue<string>(data, "Display2");
            advice.Display3 = GetValue<string>(data, "Display3");
            advice.MyQueryPwd = We7Helper.CreateNewID().Substring(1, 8);

            //下面是添加模型信息
            string config, schema;
            advice.ModelXml = GetModelDataXml(data, advice.ModelXml, out schema, out config);//获取模型数据
            advice.ModelConfig = config;
            advice.ModelName = data.ModelName;
            advice.ModelSchema = schema;
            advice.TypeID = data.Objects["AdviceTypeID"] as string;
            advice.Public = GetValue<int>(data, "Public");
            AdviceFactory.Create().AddAdvice(advice);
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
            IAdviceHelper helper = AdviceFactory.Create();
            string id = GetValue<string>(data, "ID");
            AdviceInfo advice = helper.GetAdvice(id);
            if (advice == null)
            {
                Insert(data);
            }
            else
            {
                advice.Title = GetValue<string>(data, "Title");
                advice.UserID = GetValue<string>(data, "UserID");
                advice.Content = GetValue<string>(data, "Content");
                advice.Name = GetValue<string>(data, "Name");
                advice.Email = GetValue<string>(data, "Email");
                advice.Address = GetValue<string>(data, "Address");
                advice.Phone = GetValue<string>(data, "Phone");
                advice.Fax = GetValue<string>(data, "Fax");
                advice.Public = GetValue<int>(data, "Public");

                int isshow;
                Int32.TryParse(GetValue<string>(data, "IsShow"), out isshow);
                advice.IsShow = isshow;


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
                helper.UpdateAdvice(advice);
                SetValue(data, "ID", advice.ID);
            }
            return true;
        }

        public override bool Delete(We7.Model.Core.PanelContext data)
        {
            try
            {
                string adviceid = data.DataKey["ID"] as string;
                AdviceFactory.Create().DeleteAdvice(adviceid);
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
            List<AdviceInfo> list = HelperFactory.Instance.Assistant.List<AdviceInfo>(CreateCriteria(data), new Order[] { new Order("Created", OrderMode.Desc) }, startindex, itemscount);
            foreach (AdviceInfo a in list)
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
                AdviceInfo advice = AdviceFactory.Create().GetAdvice(id);
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
            return HelperFactory.Instance.Assistant.Count<AdviceInfo>(CreateCriteria(data));
        }
    }

    public class AdviceDataProviderProxy : BaseDataProvider
    {
        BaseDataProvider v26Provider = new AdviceDataProvider();
        BaseDataProvider v27Provider = new AdviceDataProviderEx();

        BaseDataProvider GetProvider(PanelContext ctx)
        {
            if (ctx.CtrVersion == CtrVersion.V26)
            {
                return v26Provider;
            }
            else
            {
                return v27Provider;
            }
        }

        public override bool Insert(PanelContext data)
        {
            return GetProvider(data).Insert(data);
        }

        public override bool Update(PanelContext data)
        {
            return GetProvider(data).Update(data);
        }

        public override bool Delete(PanelContext data)
        {
            return GetProvider(data).Delete(data);
        }

        public override DataTable Query(PanelContext data, out int recordcount, ref int pageindex)
        {
            return GetProvider(data).Query(data, out recordcount, ref pageindex);
        }

        public override DataRow Get(PanelContext data)
        {
            return GetProvider(data).Get(data);
        }

        public override int GetCount(PanelContext data)
        {
            return GetProvider(data).GetCount(data);
        }
    }
}
