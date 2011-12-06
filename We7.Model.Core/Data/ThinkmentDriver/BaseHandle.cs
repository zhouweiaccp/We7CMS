using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Data;
using We7.Framework;

namespace We7.Model.Core.Data.ThinkmentDriver
{
    abstract class BaseHandle
    {
        string condition;
        Criteria criteria;
        SqlStatement sql;
        int parametersCount;
        IConnection connect;

        public string Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        public BaseHandle()
        {
            sql = new SqlStatement();
            Connect = HelperFactory.Instance.Assistant.GetDatabases()["We7.CMS.Common"].CreateConnection();
        }

        public BaseHandle(string modelName):this()
        {
            Ctx = new QueryContext(modelName);
        }

        public QueryContext Ctx { get; set; }

        protected ModelInfo ModelInfo
        {
            get { return Ctx.Model; }
        }

        protected We7DataTable ModelTable
        {
            get { return ModelInfo.DataSet.Tables[0]; }
        }

        protected We7DataColumnCollection Columns
        {
            get { return ModelTable.Columns; }
        }

        protected SqlStatement SQL
        {
            get { return sql; }
        }

        public Criteria ConditonCriteria
        {
            get { return criteria; }
            set { criteria = value; }
        }

        public IConnection Connect
        {
            get { return connect; }
            set { connect = value; }
        }

        protected string Prefix
        {
            get { return connect.Driver.Prefix; }
        }

        protected abstract void Build();

        protected abstract void Build(bool forContent);

        protected void AddSplitString(StringBuilder sb, string s)
        {
            if (sb.Length > 0)
            {
                sb.Append(",");
            }
            sb.Append(s);
        }

        private string AddParameter(ConListField f, object v)
        {
            string _f0 = String.Format("{0}P{1}", Prefix, parametersCount++);
            DataParameter _f1 = new DataParameter();
            _f1.DbType = f.Type;
            _f1.ParameterName = _f0;
            _f1.Value = v;
            _f1.Size = f.Size;
            //_f1.IsNullable = p.Nullable;
            SQL.Parameters.Add(_f1);
            return _f0;
        }

        protected string AddParameter(We7DataColumn dc, object v)
        {
            string _f0 = String.Format("{0}P{1}", Prefix, parametersCount++);
            DataParameter _f1 = new DataParameter();
            _f1.DbType = ModelDBHelper.ConvertTypeCodeToDbType(dc.DataType);
            _f1.ParameterName = _f0;
            _f1.Value = v;
            _f1.SourceColumn = dc.Name;
            _f1.Size = dc.MaxLength;
            _f1.IsNullable = dc.Nullable;
            SQL.Parameters.Add(_f1);
            return _f0;
        }

        protected string AddParameter(DataField df)
        {
            We7DataColumn dc = df.Column;
            string _f0 = String.Format("{0}P{1}", Prefix, parametersCount++);
            DataParameter _f1 = new DataParameter();
            _f1.DbType = ModelDBHelper.ConvertTypeCodeToDbType(dc.DataType);
            _f1.ParameterName = _f0;
            _f1.Value = df.Value;            
            _f1.SourceColumn = dc.Name;
            _f1.Size = dc.MaxLength;
            _f1.IsNullable = dc.Nullable;
            if (_f1.Value == null)
            {
                if (_f1.DbType == DbType.String)
                {
                    _f1.Value = "";
                }
                else if (_f1.DbType == DbType.DateTime)
                {
                    _f1.Value = DateTime.Now;
                }
            }          
            SQL.Parameters.Add(_f1);
            return _f0;
        }

        protected void BuildCindition()
        {
            if (criteria != null)
            {
                condition = MakeCondition(criteria);
            }
        }

        protected void BuildCindition(Dictionary<string, ConListField> fields)
        {
            if (criteria != null)
            {
                condition = MakeCondition(criteria, fields);
            }
        }

        string MakeCondition(Criteria ct)
        {
            StringBuilder _f0 = new StringBuilder();

            // If the CriteraType is None, we don't put this as a condition
            if (ct.Type != CriteriaType.None)
            {
                if (!Columns.Contains(ct.Name))
                {
                    throw new ArgumentException("在数据集中不存在当前字段:" + ct.Name);
                }
                We7DataColumn dc = Columns[ct.Name];
                if (dc.Direction == ParameterDirection.ReturnValue)
                    throw new ArgumentException("不能使用ReturnValue字段作为查询字段:" + ct.Name);

                string t = Connect.Driver.GetCriteria(ct.Type);
                string pn = AddParameter(dc, ct.Value);
                _f0.Append(String.Format(" {0} {1} {2} ", connect.Driver.FormatField(ct.Adorn, dc.Name, ct.Start, ct.Length), t, pn));
            }

            if (ct.Criterias.Count > 0)
            {
                string _f1 = ct.Mode == CriteriaMode.And ? " AND " : " OR ";
                if (ct.Type != CriteriaType.None)
                {
                    _f0.Append(_f1);
                }

                bool _f2 = ct.Criterias.Count > 1;
                if (_f2)
                {
                    _f0.Append("(");
                }

                _f0.Append(MakeCondition(ct.Criterias[0]));

                for (int i = 1; i < ct.Criterias.Count; i++)
                {
                    Criteria _f3 = ct.Criterias[i];
                    _f0.Append(_f1);
                    _f0.Append(MakeCondition(_f3));
                }

                if (_f2)
                {
                    _f0.Append(")");
                }
            }
            return _f0.ToString();
        }

        string MakeCondition(Criteria ct, Dictionary<string, ConListField> fields)
        {
            StringBuilder _f0 = new StringBuilder();

            // If the CriteraType is None, we don't put this as a condition
            if (ct.Type != CriteriaType.None)
            {
                if (!Columns.Contains(ct.Name))
                {
                    throw new ArgumentException("在数据集中不存在当前字段:" + ct.Name);
                }
                We7DataColumn dc = Columns[ct.Name];
                if (dc.Direction == ParameterDirection.ReturnValue)
                    throw new ArgumentException("不能使用ReturnValue字段作为查询字段:" + ct.Name);

                string t = Connect.Driver.GetCriteria(ct.Type);

                string pn = "";
                ConListField f = fields[ct.Name];
                if (f.FieldName != f.AliasName)
                {
                    pn = AddParameter(f, ct.Value);
                }
                else
                {
                    pn = AddParameter(dc, ct.Value);
                }
                Adorns adorn = ct.Adorn;
                _f0.Append(String.Format(" {0} {1} {2} ", connect.Driver.FormatField(adorn, f.FieldName, ct.Start, ct.Length), t, pn));
                //_f0.Append(String.Format(" {0} {1} {2} ", f.AliasName, t, ct.Value));
            }

            if (ct.Criterias.Count > 0)
            {
                string _f1 = ct.Mode == CriteriaMode.And ? " AND " : " OR ";
                if (ct.Type != CriteriaType.None)
                {
                    _f0.Append(_f1);
                }

                bool _f2 = ct.Criterias.Count > 1;
                if (_f2)
                {
                    _f0.Append("(");
                }

                _f0.Append(MakeCondition(ct.Criterias[0], fields));

                for (int i = 1; i < ct.Criterias.Count; i++)
                {
                    Criteria _f3 = ct.Criterias[i];
                    _f0.Append(_f1);
                    _f0.Append(MakeCondition(_f3, fields));
                }

                if (_f2)
                {
                    _f0.Append(")");
                }
            }
            return _f0.ToString();
        }


        protected Criteria BuildCriteriaByCtx()
        {
            Criteria ct = new Criteria(CriteriaType.None);
            foreach (QueryField qf in Ctx.QueryFields)
            {
                ct.Add(ModelDBHelper.ConvertOperationType(qf.Operator), qf.Column.Name, qf.Value);
            }
            return ct;
        }

        protected List<Order> BuildOrderList()
        {
            throw new NotImplementedException("根据PanelConext取值的方法还没有实现");
        }
    }
}
