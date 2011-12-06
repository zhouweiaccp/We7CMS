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
using We7.Model.UI.Data;
using We7.Framework.Helper;
using We7.Framework.Cache;

namespace We7.Model.UI.Command
{
    public class MoveToCommand : BaseCommand
    {
        public override object Do(PanelContext data)
        {
            string oid = data.Objects["oid"] as string;
            if (We7Helper.IsEmptyID(oid))
                throw new Exception("不能移动到根栏目");

            ChannelHelper chHelper = HelperFactory.GetHelper<ChannelHelper>();
            Channel channel = chHelper.GetChannel(oid, null);
            if (channel == null)
                throw new Exception("当前栏目不存在");
            if (channel.ModelName != data.ModelName)
                throw new Exception("移动到的栏目类型与当前栏目类型不一致");

            if (!string.IsNullOrEmpty(oid))
            {
                List<DataKey> dataKeys = data.State as List<DataKey>;
                foreach (DataKey key in dataKeys)
                {
                    string id = key["ID"] as string;
                    Article a = ArticleHelper.GetArticle(id);
                    if (a != null)
                    {
                        DataSet ds = BaseDataProvider.CreateDataSet(data.Model);
                        BaseDataProvider.ReadXml(ds, a.ModelXml);
                        if (ds.Tables[data.Table.Name].Rows.Count > 0)
                        {
                            DataRow row=ds.Tables[data.Table.Name].Rows[0];                            
                            a.OwnerID = oid;
                            if(row.Table.Columns.Contains("OwnerID"))
                            {
                               row["OwnerID"]=oid;
                            }
                            Channel ch = ChannelHelper.GetChannel(oid, null);
                            if (ch != null)
                            {
                                a.ChannelFullUrl = ch.FullUrl;
                                if(row.Table.Columns.Contains("ChannelFullUrl"))
                                {
                                   row["ChannelFullUrl"]=a.ChannelFullUrl;
                                }
                                a.ChannelName = ch.FullPath;
                                 if(row.Table.Columns.Contains("ChannelName"))
                                {
                                   row["ChannelName"]=a.ChannelName;
                                }
                                a.FullChannelPath = ch.FullFolderPath;
                                 if(row.Table.Columns.Contains("FullChannelPath"))
                                {
                                   row["FullChannelPath"]=a.FullChannelPath;
                                }
                            }
                            a.ModelXml = BaseDataProvider.GetXml(ds);
                        }
                    }
                    if (DbHelper.CheckTableExits(data.Table.Name))
                    {
                        DbHelper.ExecuteSql(String.Format("UPDATE [{0}] SET [OwnerID]='{2}' WHERE [ID]='{1}'", data.Table.Name, id, oid));
                    }
                    ArticleHelper.UpdateArticle(a, new string[] { "ID", "OwnerID", "ChannelFullUrl", "ChannelName", "FullChannelPath","ModelXml" });
                    // 往全文检索里更新数据
                    ArticleIndexHelper.InsertData(id, 1);
                }
            }
            UIHelper.SendMessage("移动成功");
            CacheRecord.Create(data.ModelName).Release();
            return null;
        }

        ArticleIndexHelper ArticleIndexHelper
        {
            get { return HelperFactory.GetHelper<ArticleIndexHelper>(); }
        }
    }
}
