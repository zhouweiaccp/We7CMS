using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core;
using System.Collections.Generic;
using We7.CMS.Common;
using We7.CMS;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.Model.UI.Data;
using We7.Framework.Helper;
using We7.Framework.Cache;

namespace We7.Model.UI.Command
{
    public class SetPublisCommand : BaseCommand
    {
        public override object Do(PanelContext data)
        {
            try
            {
                List<DataKey> dataKeys = data.State as List<DataKey>;
                if (dataKeys != null)
                {
                    foreach (DataKey key in dataKeys)
                    {
                        string id = key["ID"] as string;
                        Article a = ArticleHelper.GetArticle(id, null);
                        if (a != null && a.State != 2)
                        {
                            a.State = 1;

                            if (!String.IsNullOrEmpty(a.ModelXml))
                            {
                                DataSet ds = BaseDataProvider.CreateDataSet(data.Model);
                                BaseDataProvider.ReadXml(ds, a.ModelXml);

                                if (ds.Tables[data.Table.Name].Rows.Count > 0 && ds.Tables[data.Table.Name].Columns.Contains("State"))
                                {
                                    ds.Tables[data.Table.Name].Rows[0]["State"] = 1;
                                    a.ModelXml = BaseDataProvider.GetXml(ds);
                                }
                            }
                            if (DbHelper.CheckTableExits(data.Table.Name))
                            {
                                DbHelper.ExecuteSql(String.Format("UPDATE [{0}] SET [State]=1 WHERE [ID]='{1}'", data.Table.Name, id));
                            }
                            ArticleHelper.UpdateArticle(a, new string[] { "State", "ModelXml" });                            
                        }
                    }
                }
                UIHelper.SendMessage("发布成功");
                CacheRecord.Create(data.ModelName).Release();
            }
            catch (Exception ex)
            {
                UIHelper.SendError("发布失败:" + ex.Message);
            }
            return null;
        }
    }
}
