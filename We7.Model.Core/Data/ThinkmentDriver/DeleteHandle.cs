using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;

namespace We7.Model.Core.Data.ThinkmentDriver
{
    class ExecuteHandle : OperateHandle
    {
        int returnCode;

        public ExecuteHandle(string modelName)
            : base(modelName)
        {
        }

        public ExecuteHandle() { }

        public void Execute(string sql)
        {
            try
            {
                returnCode = 0;
                SQL.Parameters.Clear();
                SQL.SqlClause = sql;
                returnCode = Connect.Update(SQL);
            }
            catch (Exception ex)
            {
                ThrowException(SQL, ex);
            }
        }

        public void Execute(SqlStatement statement)
        {
            try
            {
                returnCode = Connect.Update(statement);
            }
            catch (Exception ex)
            {
                ThrowException(statement, ex);
            }
        }

        protected override void Build()
        {
        }

        protected override void Build(bool forContent)
        {
        }
    }

    class DeleteHandle:OperateHandle
    {
        int returnCode;

        public DeleteHandle(string modelName)
            : base(modelName)
        {
        }

        public DeleteHandle() { }

        public int ReturnCode
        {
            get { return returnCode; }
        }

        public void Execute()
        {
            returnCode = 0;
            Build();
            returnCode = Connect.Update(SQL);
        }

        protected override void Build()
        {
            SQL.Parameters.Clear();
            if (ConditonCriteria == null)
            {
                SQL.SqlClause = String.Format("DELETE FROM {0}", Connect.Driver.FormatTable(ModelTable.Name));
            }
            else
            {
                this.BuildCindition();
                SQL.SqlClause = String.Format("DELETE FROM {0} WHERE {1}",
                    Connect.Driver.FormatTable(ModelTable.Name), this.Condition);
            }
        }

        protected override void Build(bool forContent)
        {
        }
    }
}
