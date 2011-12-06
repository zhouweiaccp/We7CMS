using System;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;
using System.Collections.Generic;
using Thinkment.Data;
using System.Net;
using System.Text.RegularExpressions;

namespace We7.CMS.Helpers
{
    /// <summary>
    /// 积分业务类
    /// </summary>
    [Serializable]
    [Helper("We7.CMS.Helpers.PointHelper")]
    public class PointHelper : BaseHelper
    {
        /// <summary>
        /// 添加积分
        /// </summary>
        /// <param name="point"></param>
        public void AddPoint(Point point)
        {
            if (String.IsNullOrEmpty(point.ID))
                point.ID = We7Helper.CreateNewID();
            Assistant.Insert(point);
        }

        /// <summary>
        /// 根据用户AccountID查询积分信息
        /// </summary>
        /// <param name="accountID">用户AccountID</param>
        /// <param name="from">开始记录</param>
        /// <param name="count">记录条数</param>
        /// <returns>记录列表</returns>
        public List<Point> ListPointByAccount(string accountID, int from, int count)
        {
            Criteria query = new Criteria(CriteriaType.Equals, "AccountID", accountID);
            Order[] order = new Order[] { new Order("Created", OrderMode.Desc) };
            return Assistant.List<Point>(query, order, from, count);
        }

        /// <summary>
        /// 根据用户查询用户拥有的积分信息条数
        /// </summary>
        /// <param name="accountID">用户的AccountID</param>
        /// <returns>记录条数</returns>
        public int GetCountByAccount(string accountID)
        {
            Criteria query = new Criteria(CriteriaType.Equals, "AccountID", accountID);
            return Assistant.Count<Point>(query);
        }

        public Point GetPoint(string id)
        {
            Criteria c=new Criteria(CriteriaType.Equals,"ID",id);
            Order[] order = new Order[] { new Order("Created", OrderMode.Desc) };
            List<Point> list=Assistant.List<Point>(c, order);
            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 取得所有的记录
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <returns></returns>
        public List<Point> ListAllPointByAccount(string accountID)
        {
            Criteria query = new Criteria(CriteriaType.Equals, "AccountID", accountID);
            Order[] order = new Order[] { new Order("Created", OrderMode.Desc) };
            return Assistant.List<Point>(query, order);
        }

        /// <summary>
        /// 删除积分记录
        /// </summary>
        /// <param name="id"></param>
        public void DelPoint(string id)
        {
            Point f=new Point();
            f.ID=id;
            Assistant.Delete(f);
        }


        public void UpdatePoint(Point fav)
        {
            fav.Created = DateTime.Now;
            Assistant.Update(fav);
        }
    }
}
