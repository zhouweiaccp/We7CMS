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
using We7.Model.UI.Data;
using We7.Framework.Helper;
using We7.Model.Core.Data;
using We7.Framework.Cache;

namespace We7.Model.UI.Command
{
    public class AddTagCommand : BaseCommand
    {
        public override object Do(PanelContext data)
        {
            string tag = data.Objects["tag"] as string;
            List<DataKey> dataKeys = data.State as List<DataKey>;
            if(!String.IsNullOrEmpty(tag)&&dataKeys!=null)
            {
                foreach (DataKey key in dataKeys)
                {
                    string id = key["ID"] as string;
                    Article a = ArticleHelper.GetArticle(id, null);
                    if (a != null)
                    {
                        a.Tags +=String.Format("'{0}'",tag);
                        if (!String.IsNullOrEmpty(a.ModelXml))
                        {
                            DataSet ds = BaseDataProvider.CreateDataSet(data.Model);
                            BaseDataProvider.ReadXml(ds, a.ModelXml);

                            if (ds.Tables[data.Table.Name].Rows.Count > 0 && ds.Tables[data.Table.Name].Columns.Contains("Tags"))
                            {
                                ds.Tables[data.Table.Name].Rows[0]["Tags"] = a.Tags;
                                a.ModelXml = BaseDataProvider.GetXml(ds);
                            }
                        }

                        AddTagToSingleTable(data, id, a.Tags);
                        ArticleHelper.UpdateArticle(a, new string[] { "Tags", "ModelXml" });
                    }
                }
            }
            UIHelper.SendMessage("添加标签成功");
            CacheRecord.Create(data.ModelName).Release();
            return false;
        }

        void AddTagToSingleTable(PanelContext data, string id, string tag)
        {
            if (DbHelper.CheckColumnsExits(data.Table.Name, "Tags"))
            {
                DbHelper.ExecuteSql(String.Format("UPDATE [{0}] SET [Tags]='{1}' WHERE [ID]='{2}'", data.Table.Name, tag.Replace("'","''"), id));
            }
        }
    }
}
