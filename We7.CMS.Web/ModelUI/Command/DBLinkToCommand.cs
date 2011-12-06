using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core;
using System.Web.UI.WebControls;
using We7.Framework.Helper;
using System.Data;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.CMS;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.Framework;
using We7.Framework.Cache;

namespace We7.Model.UI.Command
{
    public class DBLinkToCommand : ICommand
    {
        #region ICommand 成员

        object ICommand.Do(PanelContext data)
        {
            string oid = data.Objects["oid"] as string;
            if (We7Helper.IsEmptyID(oid))
                throw new Exception("不能添加到根栏目");
            Channel targetChannel = HelperFactory.Instance.GetHelper<ChannelHelper>().GetChannel(oid);
            if (targetChannel != null && !String.IsNullOrEmpty(targetChannel.ModelName))
            {
                ModelInfo modelInfo = ModelHelper.GetModelInfo(targetChannel.ModelName);
                We7DataTable dt = modelInfo.DataSet.Tables[0];

                List<DataKey> dataKeys = data.State as List<DataKey>;
                foreach (DataKey dk in dataKeys)
                {
                    string id = dk["ID"].ToString();
                    SingleTableLinkTo(data, dt, id);
                }
            }
            UIHelper.SendMessage("引用成功");
            CacheRecord.Create(data.ModelName).Release();
            return null;
        }

        void SingleTableLinkTo(PanelContext data, We7DataTable dt, string id)
        {
            DataTable datatables = DbHelper.Query(String.Format("SELECT * FROM [{0}] WHERE [ID]='{1}'", data.Table.Name, id));
            if (datatables.Rows.Count > 0)
            {
                DataRow row = datatables.Rows[0];
                We7DataColumn dc1 = dt.Columns.IndexOfMappingField("ContentUrl");
                We7DataColumn dc2 = data.Table.Columns.IndexOfMappingField("OwnerID");
                if (dc1 != null && dc2 != null)
                {
                    StringBuilder sbFields = new StringBuilder();
                    StringBuilder sbValues = new StringBuilder();
                    sbFields.Append("[ID],");
                    sbValues.Append("'" + We7Helper.CreateNewID() + "',");

                    Channel ch = HelperFactory.Instance.GetHelper<ChannelHelper>().GetChannel(row[dc2.Name].ToString(), null);
                    sbFields.AppendFormat("[{0}],", dc1.Name);
                    sbValues.AppendFormat("{0},", String.Format("{0}{1}.{2}", ch.FullUrl, We7Helper.GUIDToFormatString(row["ID"].ToString()), GeneralConfigs.GetConfig().UrlFormat));

                    dc1 = dt.Columns.IndexOfMappingField("Title");
                    dc2 = data.Table.Columns.IndexOfMappingField("Title");
                    if (dc1 != null && dc2 != null)
                    {
                        sbFields.AppendFormat("[{0}],", dc1.Name);
                        sbValues.AppendFormat("'{0}',", row[dc2.Name]);
                    }

                    dc1 = dt.Columns.IndexOfMappingField("ContentType");
                    dc2 = data.Table.Columns.IndexOfMappingField("ContentType");
                    if (dc1 != null && dc2 != null)
                    {
                        sbFields.AppendFormat("[{0}],", dc1.Name);
                        sbValues.AppendFormat("{0},", (int)TypeOfArticle.LinkArticle);
                    }

                    Utils.TrimEndStringBuilder(sbFields, ",");
                    Utils.TrimEndStringBuilder(sbValues, ",");
                    string sql = String.Format("INSERT INTO [{0}]({1}) VALUES({2})", dt.Name, sbFields, sbValues);
                    if (DbHelper.CheckTableExits(data.Table.Name))
                        DbHelper.ExecuteSql(sql);
                }
            }
        }

        #endregion
    }
}
