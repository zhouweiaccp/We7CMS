// Author:
//   Marek Sieradzki (marek.sieradzki@gmail.com)
//
// (C) 2005 Marek Sieradzki
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Thinkment.Data
{
    /// <summary>
    /// SQL语句构造及执行管理
    /// </summary>
    public class ObjectManager
    {
        EntityObject curObject;
        Type objType;
        DataBase curDatabase;

        public ObjectManager()
        {
        }
        /// <summary>
        /// 当前表映射信息
        /// </summary>
        public EntityObject CurObject
        {
            get { return curObject; }
            set { curObject = value; }
        }
        /// <summary>
        /// 对象类型
        /// </summary>
        public Type ObjType
        {
            get { return objType; }
            set { objType = value; }
        }
        /// <summary>
        /// 当前数据库
        /// </summary>
        public DataBase CurDatabase
        {
            get { return curDatabase; }
            set { curDatabase = value; }
        }
        /// <summary>
        /// 执行插入操作
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="o"></param>
        /// <param name="fields"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public int MyInsert(IConnection conn, object o, string[] fields, out object identity)
        {
            InsertHandle ins = new InsertHandle();
            ins.Connect = conn;
            ins.EntityObject = curObject;
            ins.ExecuteObject = o;
            if (fields != null)
            {
                foreach (string f in fields)
                {
                    ins.AddFields(f);
                }
            }
            ins.Execute();
            identity = ins.ReturnObj;


            return ins.ReturnCode;
        }
        /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="o"></param>
        /// <param name="fields"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int MyUpdate(IConnection conn, object o, string[] fields, Criteria condition)
        {
            Criteria ct = condition;
            if (ct == null)
            {
                if (CurObject.PrimaryKeyName == null || CurObject.PrimaryKeyName.Length == 0)
                {
                    throw new DataException(ErrorCodes.ConditionRequired);
                }
                ct = CurObject.BuildCriteria(o);
            }
            UpdateHandle upt = new UpdateHandle();
            upt.Connect = conn;
            upt.EntityObject = curObject;
            upt.ExecuteObject = o;
            upt.ConditonCriteria = ct;
            if (fields != null)
            {
                foreach (string f in fields)
                {
                    upt.AddFields(f);
                }
            }
            upt.Execute();
            return upt.ReturnCode;
        }
        /// <summary>
        /// 执行选取操作
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="o"></param>
        /// <param name="fields"></param>
        public void MySelect(IConnection conn, object o, string[] fields)
        {
            SelectHandle rs = new SelectHandle();
            rs.EntityObject = curObject;
            rs.Connect = conn;
            rs.ExecuteObject = o;

            if (fields != null && fields.Length > 0)
            {
                foreach (string f in fields)
                {
                    rs.AddFields(f);
                }
            }
            rs.Execute();
        }
        /// <summary>
        /// 执行删除一条记录
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public int MyDelete(IConnection conn, object o)
        {
            Criteria condition = curObject.BuildCriteria(o);
            return MyDeleteList(conn, condition);
        }
        /// <summary>
        /// 执行一组记录
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int MyDeleteList(IConnection conn, Criteria condition)
        {
            DeleteHandle ds = new DeleteHandle();
            ds.Connect = conn;
            ds.ConditonCriteria = condition;
            ds.EntityObject = curObject;
            ds.Execute();
            return ds.ReturnCode;
        }
        /// <summary>
        /// 执行统计记录数
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int MyCount(IConnection conn, Criteria condition)
        {
            CountHandle cs = new CountHandle();
            cs.Connect = conn;
            cs.ConditonCriteria = condition;
            cs.EntityObject = CurObject;
            cs.Execute();
            return cs.ReturnCode;
        }
        /// <summary>
        /// 执行列出记录列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<T> MyList<T>(IConnection conn, Criteria condition, Order[] orders,
            int from, int count, string[] fields)
        {
            ListField[] fs = null;
            if (fields != null && fields.Length > 0)
            {
                fs = new ListField[fields.Length];
                for (int i = 0; i < fields.Length; i++)
                {
                    fs[i] = new ListField(fields[i]);
                }
            }
            return MyList<T>(conn, condition, orders, from, count, fs);
        }
        /// <summary>
        /// 执行列出记录列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<T> MyList<T>(IConnection conn, Criteria condition, Order[] orders,
            int from, int count, ListField[] fields)
        {
            if (typeof(T) != ObjType)
            {
                throw new DataException(ErrorCodes.UnmatchType);
            }
            ListSelectHandle<T> ls = new ListSelectHandle<T>();
            ls.Connect = conn;
            ls.EntityObject = curObject;
            ls.From = from;
            ls.Count = count;
            ls.ConditonCriteria = condition;
            if (orders != null)
            {
                foreach (Order od in orders)
                {
                    ls.OrderList.Add(od);
                }
            }
            if (fields != null)
            {
                foreach (ListField f in fields)
                {
                    ls.ListFieldDict.Add(f.FieldName, f);
                }
            }
            ls.Execute();
            return ls.Data;
        }
        /// <summary>
        /// 执行列出记录列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public DataTable MyList<T>(IConnection conn, Criteria condition, Order[] orders,
            int from, int count, ConListField[] fields)
        {
            if (typeof(T) != ObjType)
            {
                throw new DataException(ErrorCodes.UnmatchType);
            }
            ListSelectHandle<T> ls = new ListSelectHandle<T>();
            ls.Connect = conn;
            ls.EntityObject = curObject;
            ls.From = from;
            ls.Count = count;
            ls.ConditonCriteria = condition;
            if (orders != null)
            {
                foreach (Order od in orders)
                {
                    ls.OrderList.Add(od);
                }
            }
            if (fields == null)
            {
                throw new Exception("ConListField shouldn't be Null!");
            }

            foreach (ConListField f in fields)
            {
                ls.ConListFieldDict.Add(f.AliasName, f);
            }

            ls.Execute(true);
            return ls.Table;
        }
    }

    class SelectHandle : OperateHandle
    {
        public SelectHandle()
        {
        }

        protected override void Build()
        {
            ConditonCriteria = EntityObject.BuildCriteria(ExecuteObject);
            BuildFields(true);

            SQL.SqlClause = String.Format("SELECT {0} FROM {1} ",
                Fields, Connect.Driver.FormatTable(EntityObject.TableName));

            if (ConditonCriteria != null && !(ConditonCriteria.Type == CriteriaType.None && ConditonCriteria.Criterias.Count == 0))
            {
                BuildCindition();
                SQL.SqlClause += string.Format(" WHERE {0}", Condition);
            }
        }

        public void Execute()
        {
            try
            {
                Build();
                DataTable dt = Connect.Query(SQL);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BindObject(ExecuteObject, dr);
                        return;
                    }
                }
                else
                    throw new DataException(ErrorCodes.NoData);
            }
            catch (Exception ex)
            {
                throw new DataException(ex.Message);
            }

        }
    }

    class UpdateHandle : OperateHandle
    {
        int returnCode;
        string fieldsSet;

        public string FieldsSet
        {
            get { return fieldsSet; }
            protected set { fieldsSet = value; }
        }

        public void Execute()
        {
            Build();
            returnCode = Connect.Update(this.SQL);
        }

        public int ReturnCode
        {
            get { return returnCode; }
        }

        protected override void Build()
        {
            BuildFieldsSet();
            SQL.SqlClause = String.Format("UPDATE {0} SET {1}", Connect.Driver.FormatTable(EntityObject.TableName), FieldsSet);
            if (ConditonCriteria != null && !(ConditonCriteria.Type == CriteriaType.None && ConditonCriteria.Criterias.Count == 0))
            {
                BuildCindition();
                SQL.SqlClause += String.Format(" WHERE {0}", Condition);
            }
        }

        void AddParameters(StringBuilder sb, Property p)
        {
            string pn = AddParameter(p);
            string s = String.Format("{0}={1}", Connect.Driver.FormatField(Adorns.None, p.Field), pn);
            AddSplitString(sb, s);
        }

        void BuildFieldsSet()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Property p in EntityObject.PropertyDict.Values)
            {
                if (p.Readonly ||
                    (ListFieldDict.Count > 0 &&
                        !ListFieldDict.ContainsKey(p.Name)))
                {
                    continue;
                }
                AddParameters(sb, p);
            }
            fieldsSet = sb.ToString();
        }
    }

    class ListSelectHandle<T> : OperateHandle
    {
        int from;
        int count;
        List<T> data;
        DataTable table;

        public ListSelectHandle()
        {
            data = new List<T>();
            table = new DataTable();
        }

        public List<T> Data
        {
            get { return data; }
        }

        public int From
        {
            get { return from; }
            set { from = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public DataTable Table
        {
            get { return table; }
        }

        protected override void Build()
        {
            BuildFields(true);
            if (ConditonCriteria != null && !(ConditonCriteria.Type == CriteriaType.None && ConditonCriteria.Criterias.Count == 0))
            {
                BuildCindition();
            }
            else
            {
                Condition = String.Empty;
            }
            List<Order> os = new List<Order>();
            foreach (Order o in this.OrderList)
            {
                if (!EntityObject.PropertyDict.ContainsKey(o.Name))
                {
                    string msg = String.Format("Property '{0}' doesn't not belong to '{1}'.", o.Name, EntityObject.TypeName);
                    throw new DataException(msg, ErrorCodes.UnknownProperty);
                }
                Property p = EntityObject.PropertyDict[o.Name];
                os.Add(new Order(p.Field, o.Mode));
            }
            if (os.Count == 0 && From <= 0)
            {
                os = EntityObject.BuildOrderList();
            }
            SQL.SqlClause = Connect.Driver.BuildPaging(
                Connect.Driver.FormatTable(EntityObject.TableName), Fields, Condition, os, From, count);
        }

        public void Execute()
        {
            data.Clear();
            Build();
            DataTable dt = Connect.Query(SQL);
            if (EntityObject.IsDataTable)  //如果是输出TableInfo
            {
                table = dt;
                //object o = Activator.CreateInstance(EntityObject.TypeForDT, table);
                object o = new TableInfo(table, EntityObject.PropertyDict);
                data.Add((T)o);
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    object o = Activator.CreateInstance(EntityObject.ObjType);
                    BindObject(o, dr);
                    data.Add((T)o);
                }
            }
        }

        public void Execute(bool forContent)
        {
            table.Clear();
            Build(forContent);
            table = Connect.Query(SQL);
        }

        protected override void Build(bool forContent)
        {
            BuildFields(true, true);
            if (ConditonCriteria != null)
            {
                BuildCindition(ConListFieldDict);
            }
            else
            {
                Condition = String.Empty;
            }
            List<Order> os = new List<Order>();
            foreach (Order o in this.OrderList)
            {
                if (!ConListFieldDict.ContainsKey(o.Name))
                {
                    string msg = String.Format("Property '{0}' doesn't not belong to '{1}'.", o.Name, EntityObject.TypeName);
                    throw new DataException(msg, ErrorCodes.UnknownProperty);
                }

                ConListField f = ConListFieldDict[o.Name];
                o.AliasName = o.Name;
                o.Name = f.FieldName;
                os.Add(o);
            }

            string table = Connect.Driver.FormatTable(EntityObject.TableName);
            SQL.SqlClause = Connect.Driver.BuildPaging(table, Fields, Condition, os, From, count);
        }

    }

    static class UpdateXmlElement
    {
        public static string GetXEAttribute(XmlElement xe, string name, string value)
        {
            if (xe.HasAttribute(name))
            {
                return xe.GetAttribute(name);
            }
            return value;
        }

        public static int GetXEAttribute(XmlElement xe, string name, int value)
        {
            return Convert.ToInt32(GetXEAttribute(xe, name, value.ToString()));
        }

        public static bool GetXEAttribute(XmlElement xe, string name, bool value)
        {
            return GetXEAttribute(xe, name, value ? Boolean.TrueString : Boolean.FalseString) == Boolean.TrueString;
        }

        public static void SetXEAttribute(XmlElement xe, string name, int value)
        {
            if (value != 0)
            {
                xe.SetAttribute(name, value.ToString());
            }
        }

        public static void SetXEAttribute(XmlElement xe, string name, string value)
        {
            if (value != null && value.Length > 0)
            {
                xe.SetAttribute(name, value.ToString());
            }
        }

        public static void SetXEAttribute(XmlElement xe, string name, bool value)
        {
            if (value)
            {
                xe.SetAttribute(name, Boolean.TrueString);
            }
        }
    }

    /// <summary>
    /// 表级别的映射对象信息
    /// </summary>
    public class EntityObject : ICloneable
    {
        string tableName;
        string primaryKeyName;
        string typeName;
        string identityName;
        Type objType;
        Dictionary<string, Property> propertyDict;
        bool isDataTable = false;
        Type typeForDT;

        public EntityObject()
        {
            propertyDict = new Dictionary<string, Property>(StringComparer.OrdinalIgnoreCase);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// 是否输出DaTable
        /// author:丁乐 2011/11/8
        /// </summary>
        public bool IsDataTable
        {
            get { return isDataTable; }
            set { isDataTable = value; }
        }
        /// <summary>
        /// 输出Datatable时的Type类型
        /// </summary>
        public Type TypeForDT
        {
            get { return typeForDT; }
            set { typeForDT = value; }
        }

        public Dictionary<string, Property> PropertyDict
        {
            get { return propertyDict; }
            set { propertyDict = value; }
        }

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        /// <summary>
        /// 键值名称
        /// </summary>
        public string PrimaryKeyName
        {
            get { return primaryKeyName; }
            set { primaryKeyName = value; }
        }

        public Type ObjType
        {
            get { return objType; }
            set { objType = value; }
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        public void Build()
        {
            objType = System.Type.GetType(typeName);
            if (objType == null)
            {
                throw new DataException(ErrorCodes.UnkownObject);
            }
            foreach (PropertyInfo pi in objType.GetProperties())
            {
                if (propertyDict.ContainsKey(pi.Name))
                {
                    Property p = propertyDict[pi.Name];
                    p.Info = pi;
                }
            }
            foreach (Property p in propertyDict.Values)
            {
                if (p.Info == null)
                {
                    string s = String.Format("{0}.{1}", ObjType.ToString(), p.Name);
                    //throw new DataException(s, ErrorCodes.UnknownProperty);
                }
            }
        }
        /// <summary>
        /// 将属性输出到XML
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("Object");
            UpdateXmlElement.SetXEAttribute(xe, "table", tableName);
            UpdateXmlElement.SetXEAttribute(xe, "type", typeName);
            UpdateXmlElement.SetXEAttribute(xe, "identity", primaryKeyName);
            foreach (Property p in propertyDict.Values)
            {
                xe.AppendChild(p.ToXml(doc));
            }
            return xe;
        }
        /// <summary>
        /// 从XML加载
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public EntityObject FromXml(XmlElement xe)
        {
            propertyDict.Clear();
            typeName = UpdateXmlElement.GetXEAttribute(xe, "type", "");
            tableName = UpdateXmlElement.GetXEAttribute(xe, "table", "");
            primaryKeyName = UpdateXmlElement.GetXEAttribute(xe, "primaryKey", "");
            identityName = UpdateXmlElement.GetXEAttribute(xe, "identity", "");
            foreach (XmlElement el in xe.SelectNodes("Property"))
            {
                Property p = new Property().FromXml(el);
                propertyDict.Add(p.Name, p);
            }
            return this;
        }
        /// <summary>
        /// 从属性字典中获取
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetValue(object obj, string name)
        {
            /*
           * modify:如果是表信息类型，则走TableInfo
           * author:丁乐
           */
            if (obj.GetType() == typeof(TableInfo))
            {
                if (name.ToUpper()=="ID")
                {
                    return TableInfo.ID;
                }
                else if (!TableInfo.Fileds.ContainsKey(name))
                {
                    throw new DataException(name, ErrorCodes.UnknownProperty);
                }
                return TableInfo.Fileds[name];
            }

            else
            {
                if (!propertyDict.ContainsKey(name))
                {
                    throw new DataException(name, ErrorCodes.UnknownProperty);
                }
                Property p = propertyDict[name];
                return p.Info.GetValue(obj, null);
            }
        }

        public void SetValue(object obj, string name, object value)
        {
            if (!propertyDict.ContainsKey(name))
            {
                throw new DataException(name, ErrorCodes.UnknownProperty);
            }
            Property p = propertyDict[name];

            //thehim 2009-6-16:处理oracle number类型转换为decimal的情况
            if (p != null && value != null)
            {
                if (p.Info.PropertyType.Name.ToLower() == "int32" &&
                value.GetType().Name.ToLower() == "decimal")
                    value = Convert.ToInt32(value);
                else if (p.Info.PropertyType.Name.ToLower() == "int64" &&
                value.GetType().Name.ToLower() == "decimal")
                    value = Convert.ToInt64(value);
            }
            if (value == null && p != null)
            {
                if (p.Info.PropertyType.Name.ToLower() == "string")
                {
                    value = string.Empty;
                }
                else if (p.Info.PropertyType.Name.ToLower() == "int32" || p.Info.PropertyType.Name.ToLower() == "int64")
                {
                    value = -1;
                }
                else
                {
                    //throw new ArgumentNullException("Cant't set the null to the objectvalue!");
                }
            }
            p.Info.SetValue(obj, value, null);
        }
        /// <summary>
        /// 构建条件
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Criteria BuildCriteria(object o)
        {
            string[] _f0 = PrimaryKeyName.Split(new char[] { ';' },
               StringSplitOptions.RemoveEmptyEntries);
            if (_f0.Length == 0)
            {
                throw new DataException(ErrorCodes.NoPrimaryKey);
            }
            Criteria _f1 = null;
            foreach (string _f2 in _f0)
            {
                object _f3 = GetValue(o, _f2);
                Criteria _f4 = new Criteria(CriteriaType.Equals, _f2, _f3);
                if (_f1 == null)
                {
                    _f1 = _f4;
                }
                else
                {
                    _f4.Criterias.Add(_f4);
                }
            }
            return _f1;
        }
        /// <summary>
        /// 构建排序对象列表
        /// </summary>
        /// <returns></returns>
        public List<Order> BuildOrderList()
        {
            string[] _f0 = PrimaryKeyName.Split(new char[] { ';' },
              StringSplitOptions.RemoveEmptyEntries);
            if (_f0.Length == 0)
            {
                throw new DataException(ErrorCodes.NoPrimaryKey);
            }
            List<Order> _f1 = new List<Order>();
            foreach (string _f2 in _f0)
            {
                if (!PropertyDict.ContainsKey(_f2))
                {
                    throw new DataException(ErrorCodes.UnknownProperty);
                }
                _f1.Add(new Order(PropertyDict[_f2].Field));
            }
            return _f1;
        }
        /// <summary>
        /// 标识字段名
        /// </summary>
        public string IdentityName
        {
            get { return identityName; }
            set { identityName = value; }
        }
        /// <summary>
        /// 是否有主键
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                return identityName != null && identityName != String.Empty;
            }
        }
    }

    class OperateHandle : BaseHandle
    {
        List<Order> orderList;
        Dictionary<string, ListField> listFieldDict;
        Dictionary<string, ConListField> conListFieldDict;
        string fields;
        string orders;

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

        protected void BuildFields(bool allowReadonly)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Property p in EntityObject.PropertyDict.Values)
            {
                Adorns a = Adorns.None;
                if ((!allowReadonly && p.Readonly))
                {
                    continue;
                }
                if (ListFieldDict.Count > 0)
                {
                    if (ListFieldDict.ContainsKey(p.Name))
                    {
                        a = listFieldDict[p.Name].Adorn;
                    }
                    else if (ListFieldDict.ContainsKey(p.Field.ToUpper()))
                    {
                        a = listFieldDict[p.Field.ToUpper()].Adorn;
                    }
                    else if (ListFieldDict.ContainsKey(p.Field.ToLower()))
                    {
                        a = listFieldDict[p.Field.ToLower()].Adorn;
                    }
                    else
                    {
                        continue;
                    }
                }
                AddSplitString(sb, Connect.Driver.FormatField(a, p.Field));
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

        protected void BindObject(object obj, DataRow dr)
        {
            foreach (Property p in EntityObject.PropertyDict.Values)
            {
                if (ListFieldDict.Count > 0 &&
                    !ListFieldDict.ContainsKey(p.Name))
                {
                    continue;
                }
                object v = dr[p.Field];
                if (v == DBNull.Value)
                {
                    v = null;
                }
                EntityObject.SetValue(obj, p.Name, v);
            }
        }
    }

    class InsertHandle : OperateHandle
    {
        string fieldsValue;
        int returnCode;
        object returnObj;

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
                Connect.Driver.FormatTable(EntityObject.TableName), Fields, FieldsValue);
        }

        public void Execute()
        {
            returnObj = null;
            Build();
            returnCode = Connect.Update(SQL);

            if (EntityObject.IsIdentity)
            {
                SqlStatement _f0 = new SqlStatement(Connect.Driver.GetIdentityExpression(EntityObject.TableName));
                returnObj = Connect.QueryScalar(_f0);
                EntityObject.SetValue(ExecuteObject, EntityObject.IdentityName, returnObj);
            }
        }

        public int ReturnCode
        {
            get { return returnCode; }
        }

        void BuildParameters(StringBuilder sb, Property p)
        {
            if (p.Readonly)
            {
                return;
            }
            string _f0 = AddParameter(p);
            AddSplitString(sb, _f0);
        }

        void BuildFieldsValue()
        {
            StringBuilder _f1 = new StringBuilder();

            foreach (Property _f2 in EntityObject.PropertyDict.Values)
            {
                if (ListFieldDict.Count > 0 &&
                !ListFieldDict.ContainsKey(_f2.Name))
                {
                    continue;
                }
                BuildParameters(_f1, _f2);
            }
            fieldsValue = _f1.ToString();
        }
    }
    abstract class BaseHandle
    {
        object executeObject;
        string condition;
        EntityObject entityObject;
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
        }

        public EntityObject EntityObject
        {
            get { return entityObject; }
            set { entityObject = value; }
        }

        public object ExecuteObject
        {
            get { return executeObject; }
            set { executeObject = value; }
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

        protected string AddParameter(Property p, object v)
        {
            string _f0 = String.Format("{0}P{1}", Prefix, parametersCount++);
            DataParameter _f1 = new DataParameter();
            _f1.DbType = p.Type;
            _f1.ParameterName = _f0;
            _f1.Value = v;
            _f1.SourceColumn = p.Field; //add by thehim 2009-9-25
            _f1.Size = p.Size;
            _f1.IsNullable = p.Nullable;
            SQL.Parameters.Add(_f1);
            return _f0;
        }

        protected string AddParameter(Property p)
        {
            string _f0 = String.Format("{0}P{1}", Prefix, parametersCount++);
            DataParameter _f1 = new DataParameter();
            _f1.DbType = p.Type;
            _f1.ParameterName = _f0;
            _f1.Value = EntityObject.GetValue(this.ExecuteObject, p.Name);
            _f1.Size = p.Size;
            _f1.IsNullable = p.Nullable;
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
                if (!EntityObject.PropertyDict.ContainsKey(ct.Name))
                {
                    throw new DataException("No such field in object. " + ct.Name);
                }

                //添加了对空的判定
                if (ct.Type == CriteriaType.IsNull)
                {
                    Property p = EntityObject.PropertyDict[ct.Name];
                    _f0.Append(String.Format(" {0} Is NULL ", connect.Driver.FormatField(ct.Adorn, p.Field, ct.Start, ct.Length)));
                }
                else if (ct.Type == CriteriaType.IsNotNull)
                {
                    Property p = EntityObject.PropertyDict[ct.Name];
                    _f0.Append(String.Format(" {0} Is Not NULL ", connect.Driver.FormatField(ct.Adorn, p.Field, ct.Start, ct.Length)));
                }
                else //这儿是没添加空值判断之前的代码
                {
                    string t = Connect.Driver.GetCriteria(ct.Type);
                    Property p = EntityObject.PropertyDict[ct.Name];
                    string pn = AddParameter(p, ct.Value);
                    _f0.Append(String.Format(" {0} {1} {2} ", connect.Driver.FormatField(ct.Adorn, p.Field, ct.Start, ct.Length), t, pn));
                }
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
                if (!fields.ContainsKey(ct.Name))
                {
                    throw new DataException("No such field in object. " + ct.Name);
                }

                //添加了对空的判定
                if (ct.Type == CriteriaType.IsNull)
                {
                    Property p = EntityObject.PropertyDict[ct.Name];
                    _f0.Append(String.Format(" {0} Is NULL ", connect.Driver.FormatField(ct.Adorn, p.Field, ct.Start, ct.Length)));
                }
                else if (ct.Type == CriteriaType.IsNotNull)
                {
                    Property p = EntityObject.PropertyDict[ct.Name];
                    _f0.Append(String.Format(" {0} Is Not NULL ", connect.Driver.FormatField(ct.Adorn, p.Field, ct.Start, ct.Length)));
                }
                else  //这儿是没添加空值判断之前的代码
                {
                    string t = Connect.Driver.GetCriteria(ct.Type);

                    string pn = "";
                    ConListField f = fields[ct.Name];
                    if (f.FieldName != f.AliasName)
                    {
                        pn = AddParameter(f, ct.Value);
                    }
                    else
                    {
                        Property p = EntityObject.PropertyDict[ct.Name];
                        pn = AddParameter(p, ct.Value);
                    }
                    Adorns adorn = ct.Adorn;
                    _f0.Append(String.Format(" {0} {1} {2} ", connect.Driver.FormatField(adorn, f.FieldName, ct.Start, ct.Length), t, pn));
                    //_f0.Append(String.Format(" {0} {1} {2} ", f.AliasName, t, ct.Value));
                }
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

    }

    class CountHandle : OperateHandle
    {
        int returnCode;

        public int ReturnCode
        {
            get { return returnCode; }
            private set { returnCode = value; }
        }

        public void Execute()
        {
            Build();
            //mc = (int)mc3.QueryScalar(ms1);/*thehim-5-21:增加SQLite驱动时,出现类型转换错误*/
            ReturnCode = Convert.ToInt32(Connect.QueryScalar(SQL));
        }

        protected override void Build()
        {
            SQL.SqlClause = String.Format("SELECT COUNT(*) FROM {0}", Connect.Driver.FormatTable(EntityObject.TableName));
            if (ConditonCriteria != null && !(ConditonCriteria.Type == CriteriaType.None && ConditonCriteria.Criterias.Count == 0))
            {
                BuildCindition();
                SQL.SqlClause += " WHERE " + Condition;
            }
        }
    }

    [Serializable]
    public class DataBase : IDatabase
    {
        string name;
        string driver;
        string connectString;
        Dictionary<string, EntityObject> objects;
        IDbDriver dbDriver;

        public DataBase()
        {
            objects = new Dictionary<string, EntityObject>();
        }

        public IDbDriver DbDriver
        {
            get
            {
                if (dbDriver == null)
                {
                    if (Driver == null || Driver.Length == 0)
                    {
                        throw new DataException(ErrorCodes.DriverRequired);
                    }
                    try
                    {
                        Type tp = Type.GetType(Driver);
                        dbDriver = (IDbDriver)Activator.CreateInstance(tp);
                    }
                    catch (Exception)
                    {
                        throw new DataException(ErrorCodes.DriverFailed);
                    }
                }
                return dbDriver;
            }
            //新添，可以赋值
            set { dbDriver = value; }
        }

        public Dictionary<string, EntityObject> Objects
        {
            get { return objects; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Driver
        {
            get { return driver; }
            set { driver = value; }
        }

        public string ConnectionString
        {
            get { return connectString; }
            set { connectString = value; }
        }

        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("Database");
            xe.SetAttribute("name", name);
            xe.SetAttribute("driver", driver);
            xe.SetAttribute("connectionString", connectString);
            foreach (EntityObject obj in Objects.Values)
            {
                xe.AppendChild(obj.ToXml(doc));
            }
            return xe;
        }

        public DataBase FromXml(XmlElement xe)
        {
            Objects.Clear();

            name = xe.GetAttribute("name");
            driver = xe.GetAttribute("driver");
            connectString = xe.GetAttribute("connectionString");
            foreach (XmlElement el in xe.SelectNodes("Object"))
            {
                EntityObject item = new EntityObject().FromXml(el);
                Objects.Add(item.TypeName, item);
            }
            return this;
        }

        public IConnection CreateConnection()
        {
            //return DbDriver.CreateConnection(ConnectionString);
            return DbDriver.CreateConnection(ConnectionString, false);
        }
    }

    class Dictionaries
    {
        Dictionary<Type, ObjectManager> objectManagerDict;
        Dictionary<string, IDatabase> databaseDict;
        Dictionary<IDatabase, IConnection> connectionDict;
        /// <summary>
        /// 列字段信息
        /// 丁乐 2011/11/7
        /// </summary>
        Dictionary<string, ObjectManager> objColumnDic;
        /// <summary>
        /// 全局数据库连接串；thehim-05-21
        /// </summary>
        string GlobalDBString = string.Empty;
        string GlobalDBDriver = string.Empty;

        public Dictionaries()
        {
            databaseDict = new Dictionary<string, IDatabase>();
            objectManagerDict = new Dictionary<Type, ObjectManager>();
            connectionDict = new Dictionary<IDatabase, IConnection>();
            objColumnDic = new Dictionary<string, ObjectManager>(StringComparer.InvariantCultureIgnoreCase);
            //type = Assembly.Load("We7.CMS.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").CreateInstance("We7.CMS.Common.ResponseJson").GetType();
        }

        /// <summary>
        /// 数据库表字段字典
        /// </summary>
        public Dictionary<string, ObjectManager> ObjColumnDic
        {
            get { return objColumnDic; }
            set { objColumnDic = value; }
        }

        public Dictionary<Type, ObjectManager> ObjectManagerDict
        {
            get { return objectManagerDict; }
        }

        public Dictionary<string, IDatabase> DatabaseDict
        {
            get { return databaseDict; }
        }

        public Dictionary<IDatabase, IConnection> ConnectionDict
        {
            get { return connectionDict; }
        }

        /// <summary>
        /// 允许外面的连接串传入进来；thehim:2009-05-21
        /// </summary>
        /// <param name="dbString"></param>
        public void SetGlobalDBString(string dbString, string dbDriver)
        {
            GlobalDBString = dbString;
            GlobalDBDriver = dbDriver;
        }

        bool InArray(string[] ar, string s)
        {
            foreach (string a in ar)
            {
                if (String.Compare(a, s, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void LoadDatabases(string fileName)
        {
            XmlDocument _f0 = new XmlDocument();
            _f0.Load(fileName);
            foreach (XmlElement _f1 in _f0.SelectNodes("Objects/Database"))
            {
                FileInfo _f2 = new FileInfo(fileName);
                DataBase _f3 = new DataBase();
                _f3.FromXml(_f1);

                //如果全局连接串不为空，那么就使用全局连接串;thehim-05-21
                if (GlobalDBString != string.Empty)
                {
                    _f3.ConnectionString = GlobalDBString;
                    _f3.Driver = GlobalDBDriver;
                }

                _f3.ConnectionString = _f3.ConnectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
                _f3.ConnectionString = _f3.ConnectionString.Replace("{$Current}", Path.GetDirectoryName(_f2.FullName));
                databaseDict.Add(_f3.Name, _f3);

                connectionDict.Add(_f3, _f3.CreateConnection());
            }
        }

        public void LoadDatabases(string fileName, string[] dbs)
        {
            XmlDocument _f0 = new XmlDocument();
            _f0.Load(fileName);
            if (_f0.SelectNodes("//DbConnectionString").Count > 0)
            {
                foreach (XmlNode node in _f0.SelectNodes("//DbConnectionString"))
                {
                    DataBase _f3 = new DataBase();
                    _f3.ConnectionString = node.Attributes["value"].Value;
                    _f3.Driver = node.Attributes["driver"].Value;

                    databaseDict.Add(node.Attributes["key"].Value, _f3);
                }
            }
            else
            {
                foreach (XmlElement _f1 in _f0.SelectNodes("Objects/Database"))
                {
                    FileInfo _f2 = new FileInfo(fileName);
                    DataBase _f3 = new DataBase();
                    _f3.FromXml(_f1);

                    if (dbs != null && !InArray(dbs, _f3.Name))
                    {
                        continue;
                    }

                    //如果全局连接串不为空，那么就使用全局连接串;thehim-05-21
                    if (GlobalDBString != string.Empty)
                    {
                        _f3.ConnectionString = GlobalDBString;
                        _f3.Driver = GlobalDBDriver;
                    }

                    _f3.ConnectionString = _f3.ConnectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
                    _f3.ConnectionString = _f3.ConnectionString.Replace("{$Current}", Path.GetDirectoryName(_f2.FullName));
                    databaseDict.Add(_f3.Name, _f3);

                    connectionDict.Add(_f3, _f3.CreateConnection());
                    foreach (EntityObject _f4 in _f3.Objects.Values)
                    {
                        _f4.Build();
                        ObjectManager oa = new ObjectManager();
                        oa.CurObject = _f4;
                        oa.CurObject.IsDataTable = false;
                        oa.CurObject.TypeForDT = null;
                        oa.CurDatabase = _f3;
                        oa.ObjType = _f4.ObjType;
                        objectManagerDict.Add(oa.ObjType, oa);

                    }
                    //添加以表名为KEY的字典 /*丁乐 2011/11/7*/
                    foreach (EntityObject _f4 in _f3.Objects.Values)
                    {
                        EntityObject _f5 = new EntityObject();
                        _f5 = _f4.Clone() as EntityObject;
                        _f5.Build();
                        ObjectManager oa = new ObjectManager();
                        _f5.TypeForDT = typeof(TableInfo);
                        _f5.IsDataTable = true;
                        oa.ObjType = typeof(TableInfo);
                        oa.CurObject = _f5;
                        oa.CurDatabase = _f3;
                        objColumnDic.Add(_f5.TableName.ToUpper(), oa);
                    }
                }
            }
        }

        public void LoadDataSource(string root, string[] dbs)
        {
            if (Directory.Exists(root))
            {
                DirectoryInfo dir = new DirectoryInfo(root);
                FileInfo[] files = dir.GetFiles("*.xml");
                foreach (FileInfo file in files)
                {
                    LoadDatabases(file.FullName, dbs);
                }
            }
        }

        public IConnection CreateConnection(string database)
        {
            return GetDatabase(database).CreateConnection();
        }

        public IConnection GetDBConnection(string database)
        {
            return ConnectionDict[GetDatabase(database)];
        }

        public IConnection GetDBConnection(Type type)
        {
            return GetObjectManager(type).CurDatabase.CreateConnection();
            //return pa3[mgo(type).md1];
        }

        public IDatabase GetDatabase(string database)
        {
            if (!DatabaseDict.ContainsKey(database))
            {
                throw new DataException(ErrorCodes.UnkownDatabase);
            }
            return DatabaseDict[database];
        }

        /// <summary>
        /// 获取SQL构造类
        /// author:丁乐 2011/11/9
        /// </summary>
        /// <param name="type">Key(如类为TableInfo则返回表信息)</param>
        /// <returns></returns>
        public ObjectManager GetObjectManager(Type type)
        {
            if (type == typeof(TableInfo))  //如果是需要返回表信息类型(DataTable)
            {
                // string key = type.GetField("TableName").GetValue(Activator.CreateInstance(type)).ToString().ToUpper();  //远程实例化TableInfo获取静态字段的值（表名）Key
                string key = TableInfo.TableName.ToUpper();
                if (!objColumnDic.ContainsKey(key))  //如果找不到当前表
                {
                    throw new DataException(ErrorCodes.UnkownObject);
                }
                else
                {
                    return objColumnDic[key];
                }
            }
            else
            {
                if (!ObjectManagerDict.ContainsKey(type))  //系统内置：以类型为KEY
                {
                    throw new DataException(ErrorCodes.UnkownObject);
                }
                return ObjectManagerDict[type];
            }
        }
    }

    class DeleteHandle : BaseHandle
    {
        int returnCode;

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

            if (ConditonCriteria == null || ConditonCriteria.Type == CriteriaType.None && ConditonCriteria.Criterias.Count == 0)
            {
                SQL.SqlClause = String.Format("DELETE FROM {0}", Connect.Driver.FormatTable(EntityObject.TableName));
            }
            else
            {
                this.BuildCindition();
                SQL.SqlClause = String.Format("DELETE FROM {0} WHERE {1}",
                    Connect.Driver.FormatTable(EntityObject.TableName), this.Condition);
            }
        }

        protected override void Build(bool forContent)
        {
        }

    }
}
