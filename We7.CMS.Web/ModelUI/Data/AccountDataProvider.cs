using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.CMS.Common.PF;
using System.Collections.Generic;
using We7.CMS.Common;
using Thinkment.Data;

namespace We7.Model.UI.Data
{
    public class AccountDataProvider : BaseDataProvider
    {
        public override bool Insert(We7.Model.Core.PanelContext data)
        {
            Account account = new Account();
            account.ID = GetValue<string>(data, "ID");
            account.LoginName = GetValue<string>(data, "LoginName");
            account.Password = GetValue<string>(data, "Password");

            account.FirstName = GetValue<string>(data, "FirstName");
            account.LastName = GetValue<string>(data, "LastName");
            account.MiddleName = GetValue<string>(data, "MiddleName");

            account.Description = GetValue<string>(data, "Description");
            account.Email = GetValue<string>(data, "Email");
            account.QQ = GetValue<string>(data, "QQ");
            account.Home = GetValue<string>(data, "Home");
            account.Mobile = GetValue<string>(data, "Mobile");

            //wangjz 添加目的是为了倒入数据是同步通过邮件验证
            account.EmailValidate = GetValue<int>(data, "EmailValidate");
            account.PasswordHashed = GetValue<int>(data, "PasswordHashed");
            account.UserType = GetValue<int>(data, "UserType");
            //end wangjz

            account.Updated = DateTime.Now;
            account.Created = DateTime.Now;
            

            //下面是添加模型信息
            string config, schema;
            account.ModelXml = GetModelDataXml(data, account.ModelXml, out schema, out config);//获取模型数据
            account.ModelConfig = config;
            account.ModelName = data.ModelName;
            account.ModelSchema = schema;

            AccountHelper.AddAccount(account);
            return true;
        }


        public override bool Update(We7.Model.Core.PanelContext data)
        {
            string id = GetValue<string>(data, "ID");
            Account account = AccountHelper.GetAccount(id, null);
            if (account != null)
            {
                account.Updated = DateTime.Now;
                string s = GetValue<string>(data, "FirstName");
                account.FirstName = !String.IsNullOrEmpty(s) ? s : account.FirstName;

                s = GetValue<string>(data, "LastName");
                account.LastName = !String.IsNullOrEmpty(s) ? s : account.LastName;

                s = GetValue<string>(data, "MiddleName");
                account.MiddleName = !String.IsNullOrEmpty(s) ? s : account.MiddleName;

                s = GetValue<string>(data, "Description");
                account.Description = !String.IsNullOrEmpty(s) ? s : account.Description;

                s = GetValue<string>(data, "Email");
                account.Email = !String.IsNullOrEmpty(s) ? s : account.Email;

                s = GetValue<string>(data, "QQ");
                account.QQ = !String.IsNullOrEmpty(s) ? s : account.QQ;

                s = GetValue<string>(data, "Home");
                account.Home = !String.IsNullOrEmpty(s) ? s : account.Home;

                s = GetValue<string>(data, "Mobile");
                account.Mobile = !String.IsNullOrEmpty(s) ? s : account.Mobile;

                //下面是添加模型信息
                string config, schema;
                account.ModelXml = GetModelDataXml(data, account.ModelXml, out schema, out config);//获取模型数据
                account.ModelConfig = config;
                account.ModelName = data.ModelName;
                account.ModelSchema = schema;

                AccountHelper.UpdateAccount(account, new string[] { "Updated", "FirstName", "MiddleName", "Description", "Email", "QQ", "Home", "Mobile", "ModelXml", "ModelConfig", "ModelName", "ModelSchema" });
            }
            return true;
        }

        public override bool Delete(We7.Model.Core.PanelContext data)
        {
            try
            {
                string accountId = data.DataKey["ID"] as string;
                AccountHelper.DeleteAccont(accountId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public override DataTable Query(We7.Model.Core.PanelContext data, out int recordcount, ref int pageindex)
        {
            DataSet ds = CreateDataSet(data.Model);

            recordcount = GetCount(data);
            int startindex, itemscount;
            We7.Framework.Util.Utils.BuidlPagerParam(recordcount, data.PageSize, ref pageindex, out startindex, out itemscount);
            List<Account> list = AccountHelper.GetAccountList(CreateCriteria(data), new Order[] { new Order("Updated", OrderMode.Desc) }, startindex, itemscount);
            foreach (Account a in list)
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

        public override DataRow Get(We7.Model.Core.PanelContext data)
        {
            DataRow row = null;
            string id = data.DataKey["ID"] as string;
            if (!String.IsNullOrEmpty(id))
            {
                Account account = AccountHelper.GetAccount(id, null);
                if (account != null && !String.IsNullOrEmpty(account.ModelXml))
                {
                    DataSet ds = CreateDataSet(data.Model);
                    ReadXml(ds, account.ModelXml);
                    row = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
                    if (row != null)
                    {
                        row[OBJECTCOLUMN] = account;
                    }
                }
            }
            return row;
        }

        public override int GetCount(We7.Model.Core.PanelContext data)
        {
            return AccountHelper.GetAccountCountByCriteria(CreateCriteria(data));
        }
    }
}
