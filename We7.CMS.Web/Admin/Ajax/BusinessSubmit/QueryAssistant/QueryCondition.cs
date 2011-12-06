using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using Thinkment.Data;

namespace We7.CMS.Web.Admin.Ajax.BusinessSubmit.Entity
{
    /// <summary>
    /// Ajax 请求实体
    /// </summary>
    public class QueryCondition : IQueryCondition
    {
        public event ResponseDelegate ResponseJsonEvent;

        public QueryCondition(System.Collections.Specialized.NameValueCollection querystring)
        {
            int i = -1;
            #region 页码
            if (int.TryParse(querystring["_page"], out i))
            {
                this.Page = i;
            }
            else
            {
                this.Page = 1;  //默认第一页
            }
            #endregion
            #region 操作类型
            if (int.TryParse(querystring["_oper"], out i))
            {
                this.Oper = (Enum_operType)i;
            }
            else
            {
                this.Oper = Enum_operType.Seach;
            }
            #endregion
            #region 查询条数
            if (int.TryParse(querystring["_rows"], out i))
            {
                this.Rows = i;
            }
            else
            {
                this.Rows = 10;
            }
            #endregion
            #region 排序字段
            this.Sort = querystring["_sort"];
            #endregion
            #region 排序顺序
            if (int.TryParse(querystring["_sord"], out i))
            {
                this.Sord = i;
            }
            else  //默认为0
            {
                this.Sord = 0;
            }
            #endregion
            bool flag;
            if (bool.TryParse(querystring["_search"], out flag))
            {
                this.Search = flag;
            }
            #region 时间梭
            this.T = querystring["_t"];
            #endregion
            #region 条件
            this.C = querystring["_c"];
            #endregion
            #region 表名
            this.Tb = querystring["_tb"];
            #endregion
            #region 字段
            string fields = querystring["_f"];
            if (!string.IsNullOrEmpty(fields) && !fields.ToUpper().Contains(",ID,") && !fields.ToUpper().StartsWith("ID,") && !fields.ToUpper().EndsWith(",ID"))
            {
                fields += ",ID";
            }
            this.F = string.IsNullOrEmpty(fields) ? null : fields.Split(',');
            #endregion
            #region ID
            this.ID = querystring["_id"];
            #endregion
        }

        /// <summary>
        /// 输出Json
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string ToJson(IQueryCondition condition)
        {
            if (ResponseJsonEvent!=null)
            {
                return ResponseJsonEvent(condition);
            }
            else
            {
                return string.Empty;
            }
        }

        private Enum_operType _oper;
        /// <summary>
        /// 操作类型
        /// </summary>
        public Enum_operType Oper
        {
            get { return _oper; }
            set { _oper = value; }
        }
        private string _id;
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get
            {
                return _id;
            }
            set { _id = value; }
        }
        private int _total;
        /// <summary>
        /// 总数据数
        /// </summary>
        public int total
        {
            get { return _total; }
            set { _total = value; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPage
        {
            get {
                return ((total % Rows) == 0) ? total / Rows : (total / Rows) + 1;
            }
        }

        private int _page;
        /// <summary>
        /// 页码
        /// </summary>
        public int Page
        {
            get { return _page; }
            set
            {
                if (value >= 1)
                    _page = value;
                else
                    _page = 1;
            }
        }
        private int _rows;
        /// <summary>
        /// 行数
        /// </summary>
        public int Rows
        {
            get { return _rows; }
            set
            {
                if (value >= 1)
                    _rows = value;
                else
                    _rows = 1;
            }
        }
        private string _sort;
        /// <summary>
        /// 用于排序的列名索引（一个名称）
        /// </summary>
        public string Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }
        private int _sord;
        /// <summary>
        /// 排序的顺序（值为 asc(0) 或 desc(1)）
        /// 默认为asc
        /// </summary>
        public int Sord
        {
            get { return _sord; }
            set { _sord = value; }
        }
        private bool _search;
        /// <summary>
        /// 是否搜索（如果不搜索，其值为 false，）
        /// </summary>
        public bool Search
        {
            get { return _search; }
            set { _search = value; }
        }
        private string _t;
        /// <summary>
        /// 一个用于禁止缓存的时间戳
        /// </summary>
        public string T
        {
            get { return _t; }
            set { _t = value; }
        }
        private string _c;
        /// <summary>
        /// 条件
        /// </summary>
        public string C
        {
            get { return _c; }
            set { _c = value; }
        }
        private string _tb;
        /// <summary>
        /// 表名
        /// </summary>
        public string Tb
        {
            get { return _tb; }
            set { _tb = value; }
        }
        private string[] _f;
        /// <summary>
        /// 字段
        /// </summary>
        public string[] F
        {
            get
            {
                if (Enum_operType.Update==Oper)
                {
                    _f = new List<string>(conditionDic.Keys as IEnumerable<string>).ToArray();
                }
                return _f;
            }
            set { _f = value; }
        }
        private Criteria conditionForCriteria;
        /// <summary>
        /// 条件
        /// </summary>
        public Criteria ConditionForCriteria
        {
            get
            {
                if (null == conditionForCriteria && !string.IsNullOrEmpty(C))
                {
                    conditionForCriteria = MakeCondition(C);
                }
                return conditionForCriteria;
            }
            set
            {
                ConditionForCriteria = value;
            }
        }
        private Criteria MakeCondition(string C)
        {
            string c = C;
            string[] cs = c.Split('|');//条件数组
            Criteria criteria = new Criteria(CriteriaType.None);
            for (int i = 0; i < cs.Length; i++)
            {
                string[] value = cs[i].Split('@');

                int type;
                if (int.TryParse(value[1], out type))
                {
                    if (value.Length == (int)Enum_IsKeyValue.yes&&!string.IsNullOrEmpty(value[2])) //且值不为空
                    {
                        criteria.Add((CriteriaType)type, value[0], value[2]);
                    }
                    else if ((value.Length == (int)Enum_IsKeyValue.no))
                    {
                        criteria.Add((CriteriaType)type, value[0], null);
                    }
                }
                else //如果表达式不是数字。则直接false
                {
                    criteria.Add(CriteriaType.Equals, "1", "2");
                }
            }
            return criteria;
        }

        private Dictionary<string, string> conditionDic;
        /// <summary>
        /// 条件字段
        /// </summary>
        public Dictionary<string, string> ConditionDic
        {
            get
            {
                if (conditionDic == null)
                {
                    conditionDic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    if (Oper == Enum_operType.Update)
                    {
                        string[] cs = C.Split('|');//条件数组
                        for (int i = 0; i < cs.Length; i++)
                        {
                            string[] value = cs[i].Split('@');
                            int type;
                            if (int.TryParse(value[1], out type))
                            {
                                if (value.Length == (int)Enum_IsKeyValue.yes)
                                {
                                    if (!conditionDic.ContainsKey(value[0]))
                                    {
                                        conditionDic.Add(value[0], value[2]);
                                    }

                                }
                            }
                        }
                    }
                }
                return conditionDic;
            }
        }

        /// <summary>
        /// 本页显示：从第几条开始
        /// </summary>
        public int Begin
        {
            get { return Rows * (Page - 1) + 1; }
        }

        /// <summary>
        /// 本页显示：到第几条结束
        /// </summary>
        public int End
        {
            get
            {
                int i = Rows * Page;
                return i < total ? i : total;
            }
        }
        /// <summary>
        /// 本页显示记录数
        /// </summary>
        public int Count
        {
            get { return End - Begin + 1; }
        }

    }
    /// <summary>
    /// 是否是KeyValue
    /// </summary>
    public enum Enum_IsKeyValue
    {
        /// <summary>
        /// 是
        /// </summary>
        yes = 3,
        /// <summary>
        /// 否
        /// </summary>
        no = 2
    }
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum Enum_operType
    {
        Add = 0,
        Del = 1,
        Seach = 2,
        Update = 3
    }
}