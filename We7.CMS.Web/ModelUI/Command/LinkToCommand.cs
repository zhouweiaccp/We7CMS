using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.CMS.Common;
using We7.CMS;
using We7.Model.UI.Data;
using System.Collections.Generic;
using We7.Model.Core;
using We7.CMS.Common.Enum;
using We7.Framework.Util;
using We7.Framework.Helper;
using System.Text;
using We7.Framework.Config;


namespace We7.Model.UI.Command
{
    public class LinkToCommand : BaseCommand
    {
        public override object Do(PanelContext data)
        {
            string oid = data.Objects["oid"] as string;
            if (We7Helper.IsEmptyID(oid))
                throw new Exception("不能添加到根栏目");

            if (!string.IsNullOrEmpty(oid))
            {
                Channel targetChannel = HelperFactory.GetHelper<ChannelHelper>().GetChannel(oid,null);
                //if (targetChannel != null && !String.IsNullOrEmpty(targetChannel.ModelName))
                if (targetChannel != null)
                {
                    //ModelInfo modelInfo = ModelHelper.GetModelInfo(targetChannel.ModelName);
                    //We7DataTable dt = modelInfo.DataSet.Tables[0];

                    List<DataKey> dataKeys = data.State as List<DataKey>;
                    foreach (DataKey key in dataKeys)
                    {
                        string id = key["ID"] as string;

                        //SingleTableLinkTo(data, dt, id);

                        Article a = ArticleHelper.GetArticle(id);
                        if (a != null)
                        {
                            DataSet ds = BaseDataProvider.CreateDataSet(data.Model);
                            DataRow row = ds.Tables[data.Table.Name].NewRow();
                            ds.Tables[data.Table.Name].Rows.Add(row);

                            a.OwnerID = oid;
                            if (row.Table.Columns.Contains("OwnerID"))
                            {
                                row["OwnerID"] = oid;
                            }
                            if (row.Table.Columns.Contains("Title"))
                            {
                                row["Title"] = a.Title;
                            }
                            if (row.Table.Columns.Contains("Description"))
                            {
                                row["Description"] = a.Description;
                            }
                            a.ContentType = (int)TypeOfArticle.LinkArticle;
                            if (row.Table.Columns.Contains("ContentType"))
                            {
                                row["ContentType"] = a.ContentType;
                            }
                            a.ContentUrl = GetContentUrl(a);
                            if (row.Table.Columns.Contains("ContentUrl"))
                            {
                                row["ContentUrl"] = a.ContentUrl;
                            }
                            a.Content = "";
                            if (row.Table.Columns.Contains("Content"))
                            {
                                row["Content"] = "";
                            }
                            a.SourceID = a.ID;
                            if (row.Table.Columns.Contains("SourceID"))
                            {
                                row["SourceID"] = a.ID;
                            }
                            a.Updated = DateTime.Now;
                            if (row.Table.Columns.Contains("Updated"))
                            {
                                row["Updated"] = a.Updated;
                            }

                            a.Created = DateTime.Now;
                            if (row.Table.Columns.Contains("Created"))
                            {
                                row["Created"] = a.Created;
                            }

                            a.Overdue = DateTime.Now.AddYears(2);
                            if (row.Table.Columns.Contains("Overdue"))
                            {
                                row["Overdue"] = a.Overdue;
                            }

                            Channel ch = ChannelHelper.GetChannel(oid, null);
                            if (ch != null)
                            {
                                a.ChannelFullUrl = ch.FullUrl;
                                if (row.Table.Columns.Contains("ChannelFullUrl"))
                                {
                                    row["ChannelFullUrl"] = a.ChannelFullUrl;
                                }
                                a.ChannelName = ch.FullPath;
                                if (row.Table.Columns.Contains("ChannelName"))
                                {
                                    row["ChannelName"] = a.ChannelName;
                                }
                                a.FullChannelPath = ch.FullFolderPath;
                                if (row.Table.Columns.Contains("FullChannelPath"))
                                {
                                    row["FullChannelPath"] = a.FullChannelPath;
                                }
                                a.State = ch.Process != null && ch.Process == "1" ? 2 : a.State;
                                if (row.Table.Columns.Contains("State"))
                                {
                                    row["State"] = a.State;
                                }
                            }
                            a.ID = We7Helper.CreateNewID();
                            if (row.Table.Columns.Contains("ID"))
                            {
                                row["ID"] = a.ID;
                            }
                            a.ModelXml = BaseDataProvider.GetXml(ds);
                            ArticleHelper.AddArticle(a);
                            // 往全文检索里更新数据
                            ArticleIndexHelper.InsertData(a.ID, 0);

                        }
                    }
                }
            }
            return null;
        }

        string GetContentUrl(Article a)
        {
            if (!String.IsNullOrEmpty(a.ChannelFullUrl))
            {
                return a.ChannelFullUrl + a.FullUrl;
            }
            else
            {
                List<Channel> chs=HelperFactory.GetHelper<ChannelHelper>().GetChannelByModelName(a.ModelName);
                if (chs != null && chs.Count > 0)
                {
                    return chs[0].RealUrl + We7Helper.GUIDToFormatString(a.ID) + "." + GeneralConfigs.GetConfig().UrlFormat;
                }
            }
            return We7Helper.GUIDToFormatString(a.ID) + "." + GeneralConfigs.GetConfig().UrlFormat;
        }

        ArticleIndexHelper ArticleIndexHelper
        {
            get { return HelperFactory.GetHelper<ArticleIndexHelper>(); }
        }

        void SingleTableLinkTo(PanelContext data, We7DataTable dt, string id)
        {
            if (DbHelper.CheckTableExits(data.Table.Name))
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

                        Channel ch = HelperFactory.GetHelper<ChannelHelper>().GetChannel(row[dc2.Name].ToString(), null);
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
                        DbHelper.ExecuteSql(sql);
                    }
                }
            }
        }
    }
}
