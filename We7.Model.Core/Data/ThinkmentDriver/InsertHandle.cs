using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Data;

namespace We7.Model.Core.Data.ThinkmentDriver
{
    class InsertHandle : OperateHandle
    {
        string fieldsValue;
        int returnCode;
        object returnObj;

        public InsertHandle(string modelName) : base(modelName) { }
        public InsertHandle() { }

        public string FieldsValue
        {
            get { return fieldsValue; }
        }

        public object ReturnObj
        {
            get { return returnObj; }
        }

        protected override void Build()
        {
            BuildFields(false);
            BuildFieldsValue();
            SQL.SqlClause = String.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                Connect.Driver.FormatTable(ModelTable.Name), Fields, FieldsValue);
        }

        public void Execute()
        {
            try
            {
                returnObj = null;
                Build();
                returnCode = Connect.Update(SQL);
            }
            catch (Exception ex)
            {
                ThrowException(SQL, ex);
            }

            //SqlStatement _f0 = new SqlStatement(Connect.Driver.GetIdentityExpression(ModelTable.Name));
            //returnObj = Connect.QueryScalar(_f0);
            //EntityObject.SetValue(ExecuteObject, EntityObject.IdentityName, returnObj);
        }

        public int ReturnCode
        {
            get { return returnCode; }
        }

        void BuildParameters(StringBuilder sb, DataField df)
        {
            We7DataColumn dc = df.Column;
            if (dc.Direction==ParameterDirection.ReturnValue)
            {
                return;
            }
            string _f0 = AddParameter(df);
            AddSplitString(sb, _f0);
        }

        void BuildFieldsValue()
        {
            StringBuilder _f1 = new StringBuilder();

            foreach (DataField _f2 in Ctx.Row)
            {
                if (ListFieldDict.Count > 0 &&
                !ListFieldDict.ContainsKey(_f2.Column.Name))
                {
                    continue;
                }
                BuildParameters(_f1, _f2);
            }
            fieldsValue = _f1.ToString();
        }

        protected override void BuildFields(bool allowReadonly)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataField df in Ctx.Row)
            {
                Adorns a = Adorns.None;
                if (ListFieldDict.Count > 0)
                {
                    if (ListFieldDict.ContainsKey(df.Column.Name))
                    {
                        a = ListFieldDict[df.Column.Name].Adorn;
                    }
                    else
                    {
                        continue;
                    }
                }
                AddSplitString(sb, Connect.Driver.FormatField(a, df.Column.Name));
            }
            Fields = sb.ToString();
        }
    }
}
