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
using System.Xml;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

//using We7.CMS;

namespace Thinkment.Data
{
    /// <summary>
    /// 映射对象执行SQL
    /// </summary>
    public class ObjectAssistant
    {
        /// <summary>
        /// 数据库字段字典
        /// </summary>
        public Dictionary<string, ObjectManager> DicForTable
        {
            get { return _d1.ObjColumnDic; }
            set
            {
                _d1.ObjColumnDic = value;
            }
        }

        Dictionaries _d1;

        public ObjectAssistant()
        {
            _d1 = new Dictionaries();
        }
        /// <summary>
        /// 将目录下的映射文件全部加载
        /// </summary>
        /// <param name="dir"></param>
        public void LoadDataSource(string dir)
        {
            _d1.LoadDataSource(dir, null);
        }
        /// <summary>
        /// 将目录下的映射文件加载到数据列表中
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="dbs"></param>
        public void LoadDataSource(string dir, string[] dbs)
        {
            _d1.LoadDataSource(dir, dbs);
        }

        /// <summary>
        /// 加载指定的映射文件
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadFromFile(string fileName)
        {
            _d1.LoadDatabases(fileName, null);
        }
        /// <summary>
        /// 加载旧数据库的配置信息（扩展）
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadOldDBConfig(string fileName)
        {
            _d1.LoadDatabases(fileName);
        }

        /// <summary>
        /// 载入数据库连接串信息
        /// </summary>
        /// <param name="connectStr"></param>
        public void LoadDBConnectionString(string connectStr, string dbDriver)
        {
            _d1.SetGlobalDBString(connectStr, dbDriver);
        }
        /// <summary>
        /// 取得数据库列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, IDatabase> GetDatabases()
        {
            return _d1.DatabaseDict;
        }

        #region 2008-11-28日逐项删除数据库内容时增加
        public Dictionary<Type, ObjectManager> GetObjects()
        {
            return _d1.ObjectManagerDict;
        }

        public Dictionary<IDatabase, IConnection> GetConnections()
        {
            return _d1.ConnectionDict;
        }
        #endregion

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object Insert(object obj)
        {
            return Insert(obj, null);
        }
        /// <summary>
        /// 插入一条记录（仅列出字段有效）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public object Insert(object obj, string[] fields)
        {
            using (IConnection conn = _d1.GetDBConnection(obj.GetType()))
            {
                return Insert(conn, obj, fields);
            }
        }
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(object obj)
        {
            return Update(obj, null, null);
        }
        /// <summary>
        /// 更新一条记录（仅列出字段有效）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public int Update(object obj, string[] fields)
        {
            return Update(obj, fields, null);
        }
        /// <summary>
        /// 更新满足条件的一组记录（仅列出字段有效）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fields"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int Update(object obj, string[] fields, Criteria condition)
        {
            using (IConnection conn = _d1.GetDBConnection(obj.GetType()))
            {
                return Update(conn, obj, fields, condition);
            }
        }

        /// <summary>
        /// 选取一条记录
        /// </summary>
        /// <param name="obj"></param>
        public void Select(object obj)
        {
            Select(obj, null);
        }
        /// <summary>
        /// 选取一条记录（仅列出字段有效）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fields"></param>
        public void Select(object obj, string[] fields)
        {
            using (IConnection conn = _d1.GetDBConnection(obj.GetType()))
            {
                Select(conn, obj, fields);
            }
        }

        /// <summary>
        /// 计算满足条件的记录数
        /// </summary>
        /// <typeparam name="T">数据库对象</typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int Count<T>(Criteria condition)
        {
            using (IConnection conn = _d1.GetDBConnection(typeof(T)))
            {
                return Count<T>(conn, condition);
            }
        }
        /// <summary>
        /// 按条件取得数据库记录列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public List<T> List<T>(Criteria condition, Order[] orders)
        {
            return List<T>(condition, orders, 0, 0, (ListField[])null);
        }
        /// <summary>
        /// 按条件取得数据库记录列表（其中部分）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> List<T>(Criteria condition, Order[] orders, int from, int count)
        {
            return List<T>(condition, orders, from, count, (string[])null);
        }
        /// <summary>
        /// 按条件取得数据库记录列表（其中部分，仅选取列出部分字段）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<T> List<T>(Criteria condition, Order[] orders, int from, int count, string[] fields)
        {
            using (IConnection conn = _d1.GetDBConnection(typeof(T)))
            {
                return List<T>(conn, condition, orders, from, count, fields);
            }
        }
        /// <summary>
        ///  按条件取得数据库记录列表（其中部分）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<T> List<T>(Criteria condition, Order[] orders, int from, int count, ListField[] fields)
        {
            using (IConnection conn = _d1.GetDBConnection(typeof(T)))
            {
                return List<T>(conn, condition, orders, from, count, fields);
            }
        }
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="obj">就删这条</param>
        /// <returns></returns>
        public bool Delete(object obj)
        {
            using (IConnection conn = _d1.GetDBConnection(obj.GetType()))
            {
                return Delete(conn, obj);
            }
        }
        /// <summary>
        /// 删除满足条件的一组数据库记录
        /// </summary>
        /// <typeparam name="T">哪个表</typeparam>
        /// <param name="condition">条件</param>
        /// <returns>成功删除条数</returns>
        public int DeleteList<T>(Criteria condition)
        {
            using (IConnection conn = _d1.GetDBConnection(typeof(T)))
            {
                return DeleteList<T>(conn, condition);
            }
        }
        /// <summary>
        /// 插入一条记录（指定数据库）
        /// </summary>
        /// <param name="conn">连接串信息</param>
        /// <param name="obj"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public object Insert(IConnection conn, object obj, string[] fields)
        {
            //idSyc.Obj = obj;
            //asyncTriggerBroadcast asyCall = new asyncTriggerBroadcast(idSyc.StartBroadcast);
            //IAsyncResult result = asyCall.BeginInvoke(null, null);
            //asyCall.EndInvoke(result);

            ObjectManager oa = _d1.GetObjectManager(obj.GetType());
            object identity;
            return oa.MyInsert(conn, obj, fields, out identity);
        }
        /// <summary>
        /// 更新一组记录（指定数据库）
        /// </summary>
        /// <param name="conn">连接串信息</param>
        /// <param name="obj"></param>
        /// <param name="fields"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int Update(IConnection conn, object obj, string[] fields, Criteria condition)
        {
            //idSyc.Obj = obj;
            //asyncTriggerBroadcast asyCall = new asyncTriggerBroadcast(idSyc.StartBroadcast);
            //IAsyncResult result = asyCall.BeginInvoke(null, null);
            //asyCall.EndInvoke(result);

            ObjectManager oa = _d1.GetObjectManager(obj.GetType());
            return oa.MyUpdate(conn, obj, fields, condition);
        }
        /// <summary>
        ///选取一条记录（仅列出字段有效）（指定数据库）
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="obj"></param>
        /// <param name="fields"></param>
        public void Select(IConnection conn, object obj, string[] fields)
        {
            ObjectManager oa = _d1.GetObjectManager(obj.GetType());
            oa.MySelect(conn, obj, fields);
        }
        /// <summary>
        /// 计算满足条件的记录条数（指定数据库）
        /// </summary>
        /// <typeparam name="T">哪个表</typeparam>
        /// <param name="conn">连接串信息</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int Count<T>(IConnection conn, Criteria condition)
        {
            ObjectManager oa = _d1.GetObjectManager(typeof(T));
            return oa.MyCount(conn, condition);
        }
        /// <summary>
        /// 选取满足条件的记录（部分）（指定数据库）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<T> List<T>(IConnection conn, Criteria condition,
            Order[] orders, int from, int count, string[] fields)
        {
            ObjectManager oa = _d1.GetObjectManager(typeof(T));
            return oa.MyList<T>(conn, condition, orders, from, count, fields);
        }
        /// <summary>
        ///  选取满足条件的记录（部分）（指定数据库）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<T> List<T>(IConnection conn, Criteria condition,
            Order[] orders, int from, int count, ListField[] fields)
        {
            ObjectManager oa = _d1.GetObjectManager(typeof(T));
            return oa.MyList<T>(conn, condition, orders, from, count, fields);
        }
        /// <summary>
        ///  删除一条记录（指定数据库）
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Delete(IConnection conn, object obj)
        {
            ObjectManager oa = _d1.GetObjectManager(obj.GetType());
            return oa.MyDelete(conn, obj) == 1;
        }
        /// <summary>
        ///  删除满足条件的记录（指定数据库）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int DeleteList<T>(IConnection conn, Criteria condition)
        {
            ObjectManager oa = _d1.GetObjectManager(typeof(T));
            return oa.MyDeleteList(conn, condition);
        }

        #region 内容管理数据访问专用

        /// <summary>
        /// 使用内容模型取出满足条件记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns>DataTable数据</returns>
        public DataTable ConList<T>(Criteria condition, Order[] orders, int from, int count, ConListField[] fields)
        {
            using (IConnection conn = _d1.GetDBConnection(typeof(T)))
            {
                return ConList<T>(conn, condition, orders, from, count, fields);
            }
        }
        /// <summary>
        /// 使用内容模型取出满足条件记录（指定数据库）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="condition"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public DataTable ConList<T>(IConnection conn, Criteria condition,
            Order[] orders, int from, int count, ConListField[] fields)
        {
            ObjectManager oa = _d1.GetObjectManager(typeof(T));
            return oa.MyList<T>(conn, condition, orders, from, count, fields);
        }
        #endregion
    }
}