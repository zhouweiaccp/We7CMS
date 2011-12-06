using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Data;

namespace We7.Model.Core.Data.ThinkmentDriver
{
    class OperateHandle : BaseHandle
    {
        List<Order> orderList;
        Dictionary<string, ListField> listFieldDict;
        Dictionary<string, ConListField> conListFieldDict;
        string fields;
        string orders;

        public OperateHandle(string modelName)
            : base(modelName)
        {
            orderList = new List<Order>();
            listFieldDict = new Dictionary<string, ListField>();
            conListFieldDict = new Dictionary<string, ConListField>();
        }

        public OperateHandle()
        {
            orderList = new List<Order>();
            listFieldDict = new Dictionary<string, ListField>();
            conListFieldDict = new Dictionary<string, ConListField>();
        }

        public string Fields
        {
            get { return fields; }
            set { fields = value; }
        }

        public Dictionary<string, ListField> ListFieldDict
        {
            get { return listFieldDict; }
        }

        public Dictionary<string, ConListField> ConListFieldDict
        {
            get { return conListFieldDict; }
        }

        public List<Order> OrderList
        {
            get { return orderList; }
            set { orderList = value; }
        }

        public void AddFields(string f)
        {
            ListField _f0 = new ListField(f);
            ListFieldDict.Add(_f0.FieldName, _f0);
        }

        protected void BuildOrders()
        {
            StringBuilder _f0 = new StringBuilder();
            foreach (Order _f1 in orderList)
            {
                CriteriaType ct = _f1.Mode == OrderMode.Desc ? CriteriaType.Desc : CriteriaType.Asc;
                string ms = " " + Connect.Driver.GetCriteria(ct) + " ";
                AddSplitString(_f0, _f1.Name + ms);
            }
            orders = _f0.ToString();
        }

        protected virtual void BuildFields(bool allowReadonly)
        {
            StringBuilder sb = new StringBuilder();

            foreach (We7DataColumn dc in Columns)
            {
                if (dc.Direction == ParameterDirection.ReturnValue)
                    continue;
                Adorns a = Adorns.None;
                if (ListFieldDict.Count > 0)
                {
                    if (ListFieldDict.ContainsKey(dc.Name))
                    {
                        a = listFieldDict[dc.Name].Adorn;
                    }
                    else
                    {
                        continue;
                    }
                }
                AddSplitString(sb, Connect.Driver.FormatField(a, dc.Name));
            }
            fields = sb.ToString();
        }

        protected void BuildFields(bool allowReadonly, bool forContent)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, ConListField> item in ConListFieldDict)
            {
                AddSplitString(sb, Connect.Driver.FormatField(item.Value));
            }
            fields = sb.ToString();
        }

        public string Orders
        {
            get { return orders; }
        }

        protected override void Build()
        {
        }

        protected override void Build(bool forContent)
        {
        }

        protected void ThrowException(SqlStatement statement, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Sql：：{0}　　　", statement.SqlClause);
            if (statement.Parameters != null)
            {
                for (int i = 0; i < statement.Parameters.Count; i++)
                {
                    sb.AppendFormat("param{0}：：name={1},value={2}　　　", i, statement.Parameters[i].ParameterName, statement.Parameters[i].Value ?? "null");
                }
            }
            sb.AppendFormat("Message：：{0}　　　", ex.Message);
            throw new Exception(sb.ToString());
        }
    }
}
