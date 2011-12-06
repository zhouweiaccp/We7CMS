using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Data;

namespace We7.Model.Core.Data.ThinkmentDriver
{
    class UpdateHandle : OperateHandle
    {
        int returnCode;
        string fieldsSet;

        public UpdateHandle() { }

        public UpdateHandle(string modelName) : base(modelName) { }

        public string FieldsSet
        {
            get { return fieldsSet; }
            protected set { fieldsSet = value; }
        }

        public void Execute()
        {
            try
            {
                Build();
                returnCode = Connect.Update(this.SQL);
            }
            catch (Exception ex)
            {
                ThrowException(SQL, ex);
            }
        }

        public int ReturnCode
        {
            get { return returnCode; }
        }

        protected override void Build()
        {
            BuildFieldsSet();
            SQL.SqlClause = String.Format("UPDATE {0} SET {1}", Connect.Driver.FormatTable(ModelTable.Name), FieldsSet);
            if (ConditonCriteria != null)
            {
                BuildCindition();
                SQL.SqlClause += String.Format(" WHERE {0}", Condition);
            }
        }

        void AddParameters(StringBuilder sb,DataField df)
        {
            string pn = AddParameter(df);
            string s = String.Format("{0}={1}", Connect.Driver.FormatField(Adorns.None, df.Column.Name), pn);
            AddSplitString(sb, s);
        }

        void BuildFieldsSet()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataField p in Ctx.Row)
            {
                if (p.Column.Direction==ParameterDirection.ReturnValue ||
                    (ListFieldDict.Count > 0 &&
                        !ListFieldDict.ContainsKey(p.Column.Name)))
                {
                    continue;
                }
                AddParameters(sb, p);
            }
            fieldsSet = sb.ToString();
        }
    }
}
