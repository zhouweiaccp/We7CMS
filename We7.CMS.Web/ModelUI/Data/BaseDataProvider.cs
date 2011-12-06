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
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS;
using We7.Model.Core;
using System.IO;
using System.Text;
using We7.CMS.Common;
using We7.Framework.Util;
using Thinkment.Data;
using We7.CMS.Accounts;

namespace We7.Model.UI.Data
{
    public abstract class BaseDataProvider : IDbProvider
    {
        public const string OBJECTCOLUMN = "Object";

        protected HelperFactory HelperFactory
        {
            get { return HelperFactory.Instance; }
        }

        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        protected ArticleIndexHelper ArticleIndexHelper
        {
            get { return HelperFactory.GetHelper<ArticleIndexHelper>(); }
        }

        protected AdviceTypeHelper AdviceTypeHelper
        {
            get
            {
                return HelperFactory.GetHelper<AdviceTypeHelper>();
            }
        }

        protected AdviceHelper AdviceHelper
        {
            get
            {
                return HelperFactory.GetHelper<AdviceHelper>();
            }
        }

        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// 根据模型信息创建数据集
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataSet CreateDataSet(ModelInfo model)
        {
            DataSet ds = new DataSet("We7Data");
            DataTable dt = new DataTable(model.DataSet.Tables[0].Name);
            ds.Tables.Add(dt);

            foreach (We7DataColumn field in model.DataSet.Tables[0].Columns)
            {
                dt.Columns.Add(field.Name, Type.GetType("System." + field.DataType.ToString()));
            }
            dt.Columns.Add(OBJECTCOLUMN, typeof(object));
            return ds;
        }

        public static void UpdateRow(DataRow row, DataFieldCollection fields)
        {
            foreach (DataColumn dc in row.Table.Columns)
            {
                DataField field = fields.IndexOf(dc.ColumnName);
                row[dc] = field != null && field.Value != null ? field.Value : DBNull.Value;
            }
        }

        public static string GetXml(DataSet ds)
        {
            if (ds != null)
            {
                StringBuilder sb = new StringBuilder();
                StringWriter writer = new StringWriter(sb);
                ds.WriteXml(writer);
                return sb.ToString();
            }
            return "";
        }

        public static void ReadXml(DataSet ds, string xml)
        {
            if (ds == null)
                throw new SystemException("DbProvider::ReadXml(DataSet ds,string xml)中ds为空");
            if (String.IsNullOrEmpty(xml))
                throw new SystemException("DbProvider::ReadXml(DataSet ds,string xml)中xml为空");
            StringReader reader = new StringReader(xml);
            ds.ReadXml(reader);
        }

        protected T GetValue<T>(PanelContext data, string field)
        {
            T o = default(T);
            DataField fd = data.Row.IndexByMapping(field);
            if (fd != null)
            {
                if (fd.Value != null)
                {
                    try
                    {
                        o = (T)fd.Value;
                    }
                    catch { o = default(T); }
                }
                else if (!String.IsNullOrEmpty(fd.Column.DefaultValue))
                {
                    o = (T)TypeConverter.StrToObjectByTypeCode(fd.Column.DefaultValue, fd.Column.DataType);
                }
            }
            else
            {
                We7DataColumn column = data.Table.Columns[field];
                if (column != null && !String.IsNullOrEmpty(column.DefaultValue) && (column.Direction == ParameterDirection.Output || column.Direction == ParameterDirection.InputOutput))
                {
                    o = (T)(T)TypeConverter.StrToObjectByTypeCode(column.DefaultValue, column.DataType);
                }
            }
            return o;
        }

        protected void SetValue(PanelContext data, string field, object value)
        {
            DataField fd = data.Row.IndexByMapping(field);
            if (fd != null)
            {
                fd.Value = value;
            }
        }

        /// <summary>
        /// 取得模型的XML
        /// </summary>
        /// <param name="data">模型数据</param>
        /// <returns>Xml字符串</returns>
        protected string GetModelDataXml(PanelContext data, string xml, out string schema, out string modelconfig)
        {
            DataSet ds = CreateDataSet(data.Model);

            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            ds.WriteXmlSchema(writer);
            schema = sb.ToString();

            modelconfig = File.ReadAllText(ModelHelper.GetModelPath(data.ModelName));


            if (!String.IsNullOrEmpty(xml))
            {
                ReadXml(ds, xml);
            }

            DataRow row = null;
            if (ds.Tables[data.Table.Name].Rows.Count > 0)
            {
                row = ds.Tables[data.Table.Name].Rows[0];
            }
            else
            {
                row = ds.Tables[data.Table.Name].NewRow();
                ds.Tables[data.Table.Name].Rows.Add(row);
            }
            UpdateRow(row, data.Row);
            return GetXml(ds);
        }

        /// <summary>
        /// 检测字段数据
        /// </summary>
        /// <param name="qf"></param>
        /// <returns></returns>
        bool CheckFieldData(QueryField qf)
        {
            return !(qf.WhenNull == WhenNull.IGNORE && qf.Column.DataType == TypeCode.String && (qf.Value == null || String.IsNullOrEmpty(qf.Value.ToString())));
        }

        /// <summary>
        /// 处理分支
        /// </summary>
        /// <param name="qf">字段数据</param>
        /// <param name="c">查询条件</param>
        /// <returns>是否继续执行</returns>
        protected virtual bool DoBranches(QueryField qf, Criteria c) { return true; }

        protected Criteria CreateCriteria(PanelContext data)
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Add(CriteriaType.Equals, "ModelName", data.Model.ModelName);

            if (data.Model.Type==ModelType.ARTICLE && !data.Model.AuthorityType &&
                Security.CurrentAccountID != We7Helper.EmptyGUID)
            {
                c.Add(CriteriaType.Equals, "AccountID", Security.CurrentAccountID);
            }

            if (data.QueryFields.Count > 0)
            {
                foreach (DataField fd in data.QueryFields)
                {
                    QueryField qf = fd as QueryField;
                    if (!CheckFieldData(qf) || !DoBranches(qf, c))
                        continue;
                    if (qf.Column.Direction == ParameterDirection.Output || qf.Column.Direction == ParameterDirection.InputOutput)
                    {
                        string column = String.IsNullOrEmpty(qf.Column.Mapping) ? qf.Column.Name : qf.Column.Mapping;

                        if (qf.Operator == OperationType.LIKE)
                        {
                            c.Add(CriteriaType.Like, column, "%" + fd.Value + "%");
                        }
                        else if (qf.Operator == OperationType.NOTEQUER)
                        {
                            c.Add(CriteriaType.NotEquals, column, fd.Value);
                        }
                        else if(qf.Operator==OperationType.EQUER)
                        {
                            c.Add(CriteriaType.Equals, column, fd.Value);
                        }
                    }
                }
            }
            return c;
        }

        #region IDbProvider 成员

        public abstract bool Insert(We7.Model.Core.PanelContext data);

        public abstract bool Update(We7.Model.Core.PanelContext data);

        public abstract bool Delete(We7.Model.Core.PanelContext data);

        public abstract DataTable Query(We7.Model.Core.PanelContext data, out int recordcount, ref int pageindex);

        public abstract DataRow Get(We7.Model.Core.PanelContext data);

        public abstract int GetCount(We7.Model.Core.PanelContext data);

        #endregion
    }
}
