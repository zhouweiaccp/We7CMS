using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core.Data;
using We7.Model.Core;
using We7.CMS.Common;
using We7.CMS;
using We7.CMS.Config;
using Thinkment.Data;
using System.Collections.Generic;
using System.IO;
using System.Text;
using We7.Model.Core.Config;
using We7.Framework;
using We7.Framework.Config;
using We7.CMS.Accounts;

namespace We7.Model.UI.Data
{
    public class ArticleDataProvider : BaseDataProvider
    {
        const int MaxCount = 160000;

        public override bool Insert(PanelContext data)
        {
            CheckModelData(data);

            Article article = new Article();
            article.ID = GetValue<string>(data, "ID");
            article.Title = GetValue<string>(data, "Title");
            article.Description = GetValue<string>(data, "Description");
            article.SubTitle = GetValue<string>(data, "SubTitle");
            article.Index = GetValue<int>(data, "Index");
            article.AllowComments = GetValue<int>(data, "AllowComments");
            article.Author = GetValue<string>(data, "Author");
            article.ContentType = GetValue<int>(data, "ContentType");
            article.ContentUrl = GetValue<string>(data, "ContentUrl");
            article.Content = GetValue<string>(data, "Content");
            article.KeyWord = GetValue<string>(data, "KeyWord");
            article.DescriptionKey = GetValue<string>(data, "DescriptionKey");
            article.Overdue = GetValue<DateTime>(data, "Overdue");
            article.OwnerID = GetValue<string>(data, "OwnerID");
            article.Tags=GetValue<string>(data,"Tags");
            article.PrivacyLevel = GetValue<int>(data, "PrivacyLevel");
            article.Source = SiteConfigs.GetConfig().SiteName;
            article.Created = DateTime.Now;
            article.Updated = DateTime.Now;
            article.Thumbnail = GetValue<string>(data, "Thumbnail");
            article.ListKeys = GetValue<string>(data, "ListKeys");
            article.ListKeys2 = GetValue<string>(data, "ListKeys2");
            article.ListKeys3 = GetValue<string>(data, "ListKeys3");
            article.ListKeys4 = GetValue<string>(data, "ListKeys4");
            article.ListKeys5 = GetValue<string>(data, "ListKeys5");

            string config, schema;
            article.ModelXml = GetModelDataXml(data, article.ModelXml, out schema, out config);//获取模型数据
            article.ModelConfig = config;
            article.ModelSchema = schema;

            if (article.ModelXml.Length >= MaxCount)
            {
                UIHelper.Message.AppendInfo(MessageType.ERROR, "输入内容过长，请重新输入");
                Logger.WriteLine();
                Logger.WriteDate();
                Logger.Write("输入内容过长");
                Logger.Write(article.ModelXml);
                return false;
            }

            article.ModelName = data.Model.ModelName;
            article.TableName = data.Table.Name;
            article.State = GetValue<int>(data, "State");

            if (!String.IsNullOrEmpty(article.OwnerID))
            {
                Channel ch = ChannelHelper.GetChannel(article.OwnerID, null);
                if (ch != null)
                {
                    article.ChannelFullUrl = ch.FullUrl;
                    article.ChannelName = ch.FullPath;
                    article.FullChannelPath = ch.FullPath;
                    article.State = ch.Process != null && ch.Process == "1" ? 2 : article.State;
                }
            }
            int type = 0;
            article.EnumState = StateMgr.StateProcess(article.EnumState, EnumLibrary.Business.ArticleType, type);

            article.AccountID = GetValue<string>(data, "AccountID");
            if (String.IsNullOrEmpty(article.AccountID))
            {
                try
                {
                    article.AccountID = Security.CurrentAccountID;
                }
                catch { }
            }

            ArticleHelper.AddArticle(article);
            ArticleIndexHelper.InsertData(article.ID, 0);
            return true;
        }

        public override bool Update(PanelContext data)
        {
            try
            {

                CheckModelData(data);

                Article article = ArticleHelper.GetArticle(GetValue<string>(data, "ID"));
                if (article == null)
                {
                    Insert(data);
                }
                else
                {

                    article.Title = GetValue<string>(data, "Title");
                    article.Description = GetValue<string>(data, "Description");
                    article.SubTitle = GetValue<string>(data, "SubTitle");
                    article.Index = GetValue<int>(data, "Index");
                    article.AllowComments = GetValue<int>(data, "AllowComments");
                    article.Author = GetValue<string>(data, "Author");
                    article.ContentType = GetValue<int>(data, "ContentType");
                    article.ContentUrl = GetValue<string>(data, "ContentUrl");
                    article.Content = GetValue<string>(data, "Content");
                    article.KeyWord = GetValue<string>(data, "KeyWord");
                    article.DescriptionKey = GetValue<string>(data, "DescriptionKey");
                    article.Overdue = GetValue<DateTime>(data, "Overdue");
                    article.IsShow = GetValue<int>(data, "IsShow");
                    article.Tags = GetValue<string>(data, "Tags");
                    article.OwnerID = GetValue<string>(data, "OwnerID");
                    article.PrivacyLevel = GetValue<int>(data, "PrivacyLevel");
                    article.Source = SiteConfigs.GetConfig().SiteName;
                    article.ListKeys = GetValue<string>(data, "ListKeys");
                    article.ListKeys2 = GetValue<string>(data, "ListKeys2");
                    article.ListKeys3 = GetValue<string>(data, "ListKeys3");
                    article.ListKeys4 = GetValue<string>(data, "ListKeys4");
                    article.ListKeys5 = GetValue<string>(data, "ListKeys5");

                    article.State = GetValue<int>(data, "State");
                    if (!String.IsNullOrEmpty(article.OwnerID))
                    {
                        Channel ch = ChannelHelper.GetChannel(article.OwnerID, null);
                        if (ch != null)
                        {
                            article.ChannelFullUrl = ch.FullUrl;
                            article.ChannelName = ch.FullPath;
                            article.FullChannelPath = ch.FullPath;
                        }
                    }
                    int type = 0;
                    article.EnumState = StateMgr.StateProcess(article.EnumState, EnumLibrary.Business.ArticleType, type);

                    article.Updated = DateTime.Now;

                    string config, schema;
                    article.ModelXml = GetModelDataXml(data, article.ModelXml, out schema, out config);//获取模型数据

                    if (article.ModelXml.Length >= MaxCount)
                    {
                        UIHelper.Message.AppendInfo(MessageType.ERROR, "输入内容过长，请重新输入");
                        Logger.WriteLine();
                        Logger.WriteDate();
                        Logger.Write("输入内容过长");
                        Logger.Write(article.ModelXml);
                        return false;
                    }
                    article.ModelConfig = config;
                    article.ModelSchema = schema;

                    article.ModelName = data.Model.ModelName;
                    article.TableName = data.Table.Name;
                    string[] updatefields = new string[] { "OwnerID", "ChannelFullUrl", "ChannelName", "FullChannelPath", "Description", "Title", "Content", "ListKeys", "ListKeys2", "ListKeys3", "ListKeys4", "ListKeys5", "Updated", "Index", "EnumState", "Source", "AllowComments", "Author", "State", "IsShow", "SubTitle", "ContentUrl", "ContentType", "Overdue", "Tags", "KeyWord", "DescriptionKey", "ModelXml", "ModelName", "TableName", "ModelConfig", "ModelSchema" };
                    ArticleHelper.UpdateArticle(article, updatefields);

                    // 往全文检索里更新数据
                    ArticleIndexHelper.InsertData(article.ID, 1);
                }
            }
            catch { }

            return true;
        }

        public override bool Delete(PanelContext data)
        {
            if (data.DataKey["ID"] != null)
            {
                ArticleHelper.DeleteArticle(data.DataKey["ID"].ToString());
            }
            return true;
        }

        public override DataTable Query(PanelContext data, out int recordcount, ref int pageindex)
        {
            DataSet ds = CreateDataSet(data.Model);

            recordcount = GetCount(data);
            int startindex, itemscount;
            We7.Framework.Util.Utils.BuidlPagerParam(recordcount, data.PageSize, ref pageindex, out startindex, out itemscount);
            List<Article> list = ArticleHelper.Assistant.List<Article>(CreateCriteria(data), new Order[] { new Order("Updated", OrderMode.Desc) }, startindex, itemscount);
            foreach (Article a in list)
            {
                try
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
                catch (Exception ex)
                {
                    ArticleHelper.DeleteArticle(a.ID);
                    Logger.WriteLine();
                    Logger.WriteDate();
                    Logger.Write("模型内容错误"+ex.Message);
                    Logger.Write(a.ModelXml);
                }
            }
            return ds.Tables[data.Table.Name];
        }

        public override DataRow Get(PanelContext data)
        {
            DataRow row = null;
            if (data.DataKey["ID"] != null)
            {
                Article article = ArticleHelper.GetArticle(data.DataKey["ID"].ToString());
                if (article != null && !String.IsNullOrEmpty(article.ModelXml))
                {
                    DataSet ds = CreateDataSet(data.Model);
                    ReadXml(ds, article.ModelXml);
                    row = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
                    if (row != null)
                    {
                        row[OBJECTCOLUMN] = article;
                    }
                }
            }
            return row;
        }

        public override int GetCount(PanelContext data)
        {
            return ArticleHelper.Assistant.Count<Article>(CreateCriteria(data));
        }

        /// <summary>
        /// 处理分支
        /// </summary>
        /// <param name="qf">字段数据</param>
        /// <param name="c">Criteria</param>
        /// <returns>是否继续执行</returns>
        protected override bool DoBranches(QueryField qf, Criteria c)
        {
            bool nextstep = true;

            if (qf.Column.Direction == ParameterDirection.Output || qf.Column.Direction == ParameterDirection.InputOutput)
            {
                string column = String.IsNullOrEmpty(qf.Column.Mapping) ? qf.Column.Name : qf.Column.Mapping;
                if (String.Compare(column,"State",true)==0)
                {
                    string state = qf.Value as string;
                    if (!String.IsNullOrEmpty(state))
                    {
                        int enumstate;
                        Int32.TryParse(state, out enumstate);
                        c.Add(CriteriaType.Equals, "State", enumstate);
                    }
                    nextstep = false;
                }
                else if (String.Compare(column, "OwnerID", true) == 0)
                {
                    string oid = qf.Value as string;
                    if (!String.IsNullOrEmpty(oid))
                    {
                        Channel ch = ChannelHelper.GetChannel(oid, null);
                        if (String.IsNullOrEmpty(ch.Parameter))
                        {
                            c.Add(CriteriaType.Equals, "OwnerID", oid);
                        }
                        else
                        {
                            CriteriaExpressionHelper.Execute(c, ch.Parameter);
                        }
                    }
                    nextstep = false;
                }
            }

            return nextstep;
        }

        /// <summary>
        /// 检测模型数据是否正确
        /// </summary>
        /// <param name="data"></param>
        void CheckModelData(PanelContext data)
        {
            if (data == null)
                throw new SystemException("参数data::PanelContext不能为空");

            DataField fid = data.Row.IndexOf("ID");
            string id = fid != null && fid.Value != null ? fid.Value.ToString() : "";
            if (String.IsNullOrEmpty(id))
                throw new Exception("ID不能为空");

            //DataField ftitle =
            //string title = GetValue<string>(data, "Title");// ftitle != null && ftitle.Value != null ? ftitle.Value.ToString() : "";
            //if (String.IsNullOrEmpty(title))
            //    throw new Exception("Title不能为空");

            //DataField foid = data.Row.IndexOf("OwnerID");
            //string oid = foid != null && foid.Value != null ? foid.Value.ToString() : "";
            //if (String.IsNullOrEmpty(oid))
            //    throw new Exception("栏目ID不能为空");
        }        
    }
}
