using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Thinkment.Data;

namespace We7.Model.Core.Data.ThinkmentDriver
{
    class SelectHandle : OperateHandle
    {
        public SelectHandle()
        {
        }

        public SelectHandle(string modelName)
            : base(modelName)
        {
        }

        protected override void Build()
        {
            ConditonCriteria = BuildCriteriaByCtx();
            BuildFields(true);
            BuildCindition();
            SQL.SqlClause = String.Format("SELECT {0} FROM {1} WHERE {2}",
                Fields, Connect.Driver.FormatTable(ModelTable.Name), Condition);
        }

        public void Execute()
        {
            try
            {
                Build();
                DataTable dt = Connect.Query(SQL);
            }
            catch (Exception ex)
            {
                ThrowException(SQL, ex);
            }

        }
    }

}
