using System.Data;
using Thinkment.Data;
using We7.Framework;
using System.Text;
using System;
using System.Collections.Generic;
using We7.Model.Core.Data.ThinkmentDriver;
using We7.Framework.Util;
using We7.Model.Core.Config;

namespace We7.Model.Core.Data
{
    public class ModelDBHelper
    {
        private ModelInfo modelInfo;

        private IDatabase db;

        public static ModelDBHelper Create(string modelName)
        {
            return new ModelDBHelper(modelName);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="modelName">模型名称</param>
        private ModelDBHelper(string modelName)
        {
            ModelName=modelName;
        }

        public IDatabase DB
        {
            get 
            {
                if (db == null)
                {
                    db = Assistant.GetDatabases()["We7.CMS.Common"];
                }
                return db;
            }
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public DataTable Query(Criteria ct, List<Order> orders)
        {
            return Query(ct, orders, 0, 0);
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable Query(Criteria ct,List<Order> orders,int from ,int count)
        {
            ClearOrders(orders);
            ListSelectHandle handle = new ListSelectHandle(ModelName);
            return handle.Execute(ct, orders, from, count);
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="ct">查询条件</param>
        /// <param name="orders">排序</param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页记录</param>
        /// <returns></returns>
        public DataTable QueryPagedList(Criteria ct, List<Order> orders, int pageIndex, int pageSize)
        {
            int recordcount;
            return QueryPagedList(ct, orders, pageIndex, pageSize, out recordcount);
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="ct">查询条件</param>
        /// <param name="orders">排序</param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页记录</param>
        /// <param name="recordcount">返回总条数</param>
        /// <returns></returns>
        public DataTable QueryPagedList(Criteria ct, List<Order> orders, int pageIndex, int pageSize, out int recordcount)
        {
            int startIndex,pageItemsCount;
            recordcount = Count(ct);
            Utils.BuidlPagerParam(recordcount, pageSize, ref pageIndex, out startIndex, out pageItemsCount);
            return Query(ct, orders, startIndex, pageItemsCount);
        }

        /// <summary>
        /// 查询Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable Query(string sql)
        {
            ListSelectHandle handle = new ListSelectHandle(ModelName);
            SqlStatement statement=new SqlStatement(sql);
            DB.DbDriver.FormatSQL(statement);
            return handle.Connect.Query(statement);
        }

        /// <summary>
        /// 按条件查询数据记录
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public int Count(Criteria ct)
        {
            CountHandle handle = new CountHandle(ModelName);
            return handle.Execute(ct);
        }


        public void Insert(Dictionary<string,object> dic)
        {
            DataFieldCollection dfc = new DataFieldCollection();
            foreach (string key in dic.Keys)
            {
                if (!Columns.Contains(key))
                    throw new ArgumentException("当前模型不包含:" + key + "字段");
                if (Columns[key].Direction == ParameterDirection.ReturnValue)
                    throw new ArgumentException("字段" + key + "为Return类型，不能添加数据");
                dfc.Add(new DataField(Columns[key], dic[key]));
            }
            Insert(dfc);
        }

        public void Update(Dictionary<string,object> dic,Criteria ct)
        {
            DataFieldCollection dfc = new DataFieldCollection();
            foreach (string key in dic.Keys)
            {
                if (!Columns.Contains(key))
                    throw new ArgumentException("当前模型不包含:" + key + "字段");
                if (Columns[key].Direction == ParameterDirection.ReturnValue)
                    throw new ArgumentException("字段" + key + "为Return类型，不能添加数据");
                dfc.Add(new DataField(Columns[key], dic[key]));
            }
            Update(dfc, ct);
        }

        public void Insert(DataFieldCollection dfc)
        {
            InsertHandle handle = new InsertHandle(ModelName);
            handle.Ctx.Row = dfc;
            handle.Execute();
        }

        public void Update(DataFieldCollection dfc, Criteria ct)
        {
            UpdateHandle handle = new UpdateHandle(ModelName);
            handle.Ctx.Row = dfc;
            handle.ConditonCriteria = ct;
            handle.Execute();
        }

        public void Delete(Criteria ct)
        {
            DeleteHandle handle = new DeleteHandle(ModelName);
            handle.ConditonCriteria = ct;
            handle.Execute();
        }

        public void Execute(string sql)
        {
            ExecuteHandle handle = new ExecuteHandle(ModelName);
            //handle.Execute(sql);
            SqlStatement statement=new SqlStatement(sql);
            Database.DbDriver.FormatSQL(statement);
            handle.Execute(statement);
        }

        /// <summary>
        /// 将TypeCode枚举转化为DbType枚举
        /// </summary>
        /// <param name="typecode">TypeCode枚举</param>
        /// <returns></returns>
        public static DbType ConvertTypeCodeToDbType(TypeCode typecode)
        {
            switch (typecode)
            {
                case TypeCode.Boolean:
                    return DbType.Boolean;
                case TypeCode.Byte:
                    return DbType.Byte;
                case TypeCode.Char:
                    return DbType.String;
                case TypeCode.DBNull:
                    return DbType.Object;
                case TypeCode.DateTime:
                    return DbType.DateTime;
                case TypeCode.Decimal:
                    return DbType.Double;
                case TypeCode.Double:
                    return DbType.String;
                case TypeCode.Empty:
                    return DbType.String;
                case TypeCode.Int16:
                    return DbType.Int16;
                case TypeCode.Int32:
                    return DbType.Int32;
                case TypeCode.Int64:
                    return DbType.Int64;
                case TypeCode.Object:
                    return DbType.Object;
                case TypeCode.SByte:
                    return DbType.SByte;
                case TypeCode.Single:
                    return DbType.Single;
                case TypeCode.String:
                    return DbType.String;
                case TypeCode.UInt16:
                    return DbType.UInt16;
                case TypeCode.UInt32:
                    return DbType.UInt32;
                case TypeCode.UInt64:
                    return DbType.UInt64;
                default:
                    return DbType.String;
            }
        }

        public static CriteriaType ConvertOperationType(OperationType op)
        {
            switch (op)
            {
                case OperationType.EQUER:
                    return CriteriaType.Equals;
                case OperationType.NOTEQUER:
                    return CriteriaType.NotEquals;
                case OperationType.LIKE:
                    return CriteriaType.Like;
                case OperationType.LESSTHAN:
                    return CriteriaType.LessThan;
                case OperationType.MORETHAN:
                    return CriteriaType.MoreThan;
                case OperationType.LESSTHANEQURE:
                    return CriteriaType.LessThanEquals;
                case OperationType.MORETHANEQURE:
                    return CriteriaType.MoreThanEquals;
                default:
                    return CriteriaType.Equals;
            }
        }

        #region 私有方法

        /// <summary>
        /// 模型名称
        /// </summary>
        string ModelName { get; set; }

        /// <summary>
        /// 模型信息
        /// </summary>
        ModelInfo ModelInfo
        {
            get
            {
                if (modelInfo == null)
                {
                    modelInfo = GetModelInfo(ModelName);
                }
                return modelInfo;
            }
        }

        We7DataTable ModelTable
        {
            get { return ModelInfo.DataSet.Tables[0]; }
        }

        We7DataColumnCollection Columns
        {
            get { return ModelTable.Columns; }
        }

        ObjectAssistant Assistant
        {
            get
            {
                return HelperFactory.Instance.Assistant;
            }
        }

        string Prefix
        {
            get { return Connection.Driver.Prefix; }
        }

        IDatabase Database
        {
            get
            {
                return Assistant.GetDatabases()["We7.CMS.Common"];
            }
        }
        IConnection Connection
        {
            get { return Database.CreateConnection(); }
        }

        #endregion

        #region 查询的私有方法

        string BuildListFields()
        {
            return "*";
        }

        string MakeCondition(Criteria ct,SqlStatement sql,ref int paramcount)
        {
            StringBuilder _f0 = new StringBuilder();

            // If the CriteraType is None, we don't put this as a condition
            if (ct.Type != CriteriaType.None)
            {
                if(!Columns.Contains(ct.Name))
                {
                    throw new Exception("不存在"+ct.Name+"字段");
                }
                if(Columns[ct.Name].Direction==ParameterDirection.ReturnValue)
                {
                    throw new Exception(ct.Name+"字段为返回字段,不能作为查询条件");
                }
                string t = Connection.Driver.GetCriteria(ct.Type);
                string pn = AddParameter(Columns[ct.Name], ct.Value, sql, ref paramcount);
                _f0.Append(String.Format(" {0} {1} {2} ", Connection.Driver.FormatField(ct.Adorn, ct.Name, ct.Start, ct.Length), t, pn));
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

                _f0.Append(MakeCondition(ct.Criterias[0],sql,ref paramcount));

                for (int i = 1; i < ct.Criterias.Count; i++)
                {
                    Criteria _f3 = ct.Criterias[i];
                    _f0.Append(_f1);
                    _f0.Append(MakeCondition(_f3,sql,ref paramcount));
                }

                if (_f2)
                {
                    _f0.Append(")");
                }
            }
            return _f0.ToString();
        }

        List<Order> GetOrders(List<Order> orders)
        {
            List<Order> ol = new List<Order>();
            foreach (Order o in orders)
            {
                if (!Columns.Contains(o.Name))
                {
                    throw new Exception("不存在" +o.Name + "字段");
                }
                if (Columns[o.Name].Direction == ParameterDirection.ReturnValue)
                {
                    throw new Exception(o.Name + "字段为返回字段,不能作为查询条件");
                }
                ol.Add(o);
            }
            if (ol.Count == 0)
                ol.Add(new Order("ID", OrderMode.Asc));
            ClearOrders(ol);
            return ol;
        }

        string AddParameter(We7DataColumn dc, object v, SqlStatement sql, ref int parametersCount)
        {
            string _f0 = String.Format("{0}P{1}", Prefix, parametersCount++);
            DataParameter _f1 = new DataParameter();
            _f1.DbType =ConvertTypeCodeToDbType(dc.DataType);
            _f1.ParameterName = _f0;
            _f1.Value = v;
            _f1.SourceColumn = dc.Name;
            _f1.Size = dc.MaxLength;
            _f1.IsNullable =true;
            sql.Parameters.Add(_f1);
            return _f0;
        }

        void ClearOrders(List<Order> orders)
        {
            if (orders == null||orders.Count==0)
            {
                orders = new List<Order>();
                orders.Add(new Order("ID", OrderMode.Asc));
            }
            Dictionary<string,Order> os=new Dictionary<string,Order>();

            foreach (Order o in orders)
            {
                if(!os.ContainsKey(o.Name))
                    os.Add(o.Name, o);
            }

            orders.Clear();
            foreach (Order o in os.Values)
            {
                orders.Add(o);
            }
        }
        #endregion

        #region 为了线程安全临时加的代码

        /// <summary>
        /// 根据模型类型获取模型路径
        /// </summary>
        /// <param name="modeltype">模型类型</param>
        /// <returns>模型路径</returns>
        private string GetModelPath(string modeltype)
        {
            modeltype = modeltype.Contains(".") ? modeltype.Replace(".", "/") : String.Format("system/{0}", modeltype);
            string path = "";

            if (modeltype.StartsWith("Template/", StringComparison.CurrentCultureIgnoreCase))
            {
                path = Utils.GetMapPath(ModelConfig.BaseModelDirectory + "/" + modeltype + ".xml");
            }
            else
            {
                path = Utils.GetMapPath(ModelConfig.ModelsDirectory + "/" + modeltype + ".xml");
            }
            return path;
        }

        /// <summary>
        /// 根据模型类型取得模型数据
        /// </summary>
        /// <param name="modeltype"></param>
        /// <returns></returns>
        private ModelInfo GetModelInfo(string modeltype)
        {
            string modelcacheid = String.Format("model_{0}", modeltype);
            ModelInfo model = AppCtx.Cache.RetrieveObject<ModelInfo>(modelcacheid);
            if (model == null)
            {
                string path = GetModelPath(modeltype);
                model = SerializationHelper.Load(typeof(ModelInfo), path) as ModelInfo;
                model.ModelName = GetModelName(modeltype);
                AppCtx.Cache.AddObjectWithFileChange(modelcacheid, model, path);
            }
            return model;
        }

        private string GetModelName(string name)
        {
            return name.Contains(".") ? name : String.Format("System.{0}", name);
        }

        #endregion

    }


    
}
