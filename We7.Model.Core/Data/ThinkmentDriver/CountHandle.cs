using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;

namespace We7.Model.Core.Data.ThinkmentDriver
{
    class CountHandle : OperateHandle
    {
        int returnCode;

        public CountHandle(string modelName)
            : base(modelName)
        {

        }

        public CountHandle() { }

        public int ReturnCode
        {
            get { return returnCode; }
            private set { returnCode = value; }
        }

        public void Execute()
        {
            try
            {
                Build();
                //mc = (int)mc3.QueryScalar(ms1);/*thehim-5-21:增加SQLite驱动时,出现类型转换错误*/
                ReturnCode = Convert.ToInt32(Connect.QueryScalar(SQL));
            }
            catch (Exception ex)
            {
                ThrowException(SQL, ex);
            }
        }

        public int Execute(Criteria c)
        {
            ConditonCriteria = c;
            Execute();
            return ReturnCode;
        }

        protected override void Build()
        {
            SQL.SqlClause = String.Format("SELECT COUNT(*) FROM {0}", Connect.Driver.FormatTable(ModelTable.Name));
            if (ConditonCriteria != null)
            {
                BuildCindition();
                if(!string.IsNullOrEmpty(Condition))
                    SQL.SqlClause += " WHERE " + Condition;
            }
        }
    }
}
