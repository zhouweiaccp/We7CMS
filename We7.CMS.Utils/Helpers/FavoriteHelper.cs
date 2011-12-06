using System;

using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;
using System.Collections.Generic;
using Thinkment.Data;
using System.Net;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Xml;

namespace We7.CMS.Helpers
{
    /// <summary>
    /// 反馈业务类
    /// </summary>
    [Serializable]
    [Helper("We7.AdviceHelper")]
    public class FavoriteHelper : BaseHelper
    {
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="favorite"></param>
        public void AddFavorite(Favorite favorite)
        {
            if (String.IsNullOrEmpty(favorite.FavoriteID))
                favorite.FavoriteID = We7Helper.CreateNewID();
            Assistant.Insert(favorite);
        }
        /// <summary>
        /// 根据用户AccountID查询收藏夹信息
        /// </summary>
        /// <param name="accountID">用户AccountID</param>
        /// <param name="from">开始记录</param>
        /// <param name="count">记录条数</param>
        /// <returns>记录列表</returns>
        public List<Favorite> ListFavoriteByAccount(string accountID, int from, int count)
        {
            Criteria query = new Criteria(CriteriaType.Equals, "AccountID", accountID);
            Order[] order = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Favorite>(query, order, from, count);
        }

        /// <summary>
        /// 根据用户查询用户拥有的收藏信息条数
        /// </summary>
        /// <param name="accountID">用户的AccountID</param>
        /// <returns>记录条数</returns>
        public int GetCountByAccount(string accountID)
        {
            Criteria query = new Criteria(CriteriaType.Equals, "AccountID", accountID);
            return Assistant.Count<Favorite>(query);
        }

        public Favorite GetFavorite(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "FavoriteID", id);
            Order[] order = new Order[] { new Order("Updated", OrderMode.Desc) };
            List<Favorite> list = Assistant.List<Favorite>(c, order);
            return list.Count > 0 ? list[0] : null;
        }

       /// <summary>
        /// 判断该物品是否已被添加到当前用户的收藏夹
       /// </summary>
       /// <param name="accountID"></param>
       /// <param name="articleID"></param>
       /// <returns></returns>
        public bool IsAddFavorite(string accountID, string articleID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AccountID", accountID);
            c.Add(CriteriaType.Equals, "ArticleID", articleID);
            int count = Assistant.Count<Favorite>(c);
            return count != 0 ? true : false;
        }

        /// <summary>
        /// 取得所有的记录
        /// </summary>
        /// <param name="accountId">用户ID</param>
        /// <returns></returns>
        public List<Favorite> ListAllFavoriteByAccount(string accountID)
        {
            Criteria query = new Criteria(CriteriaType.Equals, "AccountID", accountID);
            Order[] order = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Favorite>(query, order);
        }

        public void DelFavorite(string id)
        {
            Favorite f = new Favorite();
            f.FavoriteID = id;
            Assistant.Delete(f);
        }

        /// <summary>
        /// 取得页面的标题信息
        /// </summary>
        /// <param name="url">页面地址</param>
        /// <returns>标题</returns>
        public string GetTitle(string url)
        {
            try
            {
                WebClient wc = new WebClient();
                string txt = wc.DownloadString(url);
                if (!String.IsNullOrEmpty(txt))
                {
                    Regex regex = new Regex(@"(?<=<title\s*.*>).*?(?=</title>)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
                    Match mc = regex.Match(txt);
                    return mc.Success ? mc.Value : "";
                }
            }
            catch
            {
            }
            return "";
        }

        /// <summary>
        /// 获取绝对路径;
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetAbsoluteURl(string url)
        {
            Regex regex = new Regex(@"\.(com|net|org|gov|cc|biz|info|cn)(.(cn|hk|tw))?(?<url>/.*)", RegexOptions.IgnoreCase);
            Match mc = regex.Match(url);
            return mc.Groups["url"].Value;
        }


        public void UpdateFavorite(Favorite fav)
        {
            fav.Updated = DateTime.Now;
            Assistant.Update(fav, new string[] { "Title", "Description", "Url", "Updated", "Tag", "Thumbnail" });
        }

        /// <summary>
        /// 获取该用户收藏夹的标签列表
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public DataTable ListAllTagByAccount(string accountID)
        {
            DataTable dt = new DataTable();
            string strTagList = "SELECT Distinct [tag] FROM [Favorite] WHERE [AccountID]='{0}' ";
            string sql = string.Format(strTagList, accountID);
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            SqlStatement sqlstatement = new SqlStatement();
            sqlstatement.SqlClause = sql;
            db.DbDriver.FormatSQL(sqlstatement);
            using (IConnection conn = db.CreateConnection())
            {
                dt = conn.Query(sqlstatement);
            }
            return dt;
        }

        /// <summary>
        /// 根据标签获取当前用户的收藏列表
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public List<Favorite> ListCurrentAccountFavoriteByTag(string accountID, string tag)
        {
            Criteria query = new Criteria(CriteriaType.Equals, "AccountID", accountID);
            query.Add(CriteriaType.Like, "Tag", tag);
            Order[] order = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Favorite>(query, order);
        }

        /// <summary>
        /// 根据条件进行收藏的查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Favorite> ListFavoritesByCriteria(Criteria query)
        {
            return Assistant.List<Favorite>(query,null);
        }


        /// <summary>
        /// 获取默认收藏夹标签
        /// </summary>
        /// <returns></returns>
        public DataTable ListDefaultTag(ref bool definedTag)
        {
            string columnName = "tag";
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn(columnName, typeof(string)));
            string defautlTagPath = HttpContext.Current.Server.MapPath("~/config/urlTags.xml");
            XmlDocument xml = new XmlDocument();
            xml.Load(defautlTagPath);
            XmlNode root = xml.SelectSingleNode("urlTags/defaultTagList");
            if (root.ChildNodes.Count > 0)
            {
                foreach (XmlNode n in root.ChildNodes)
                {
                    DataRow dr = dt.NewRow();
                    if (n != null && n.Attributes!=null && n.Attributes["name"] != null && !string.IsNullOrEmpty(n.Attributes["name"].Value))
                    {
                        dr[columnName] = n.Attributes["name"].Value;
                    }
                    else
                    {
                        dr[columnName] = string.Empty;
                    }
                    dt.Rows.Add(dr);
                }
            }
            definedTag = Convert.ToBoolean(xml.SelectSingleNode("urlTags/TagDefined").Attributes["define"].Value);
            return dt;
        }

        /// <summary>
        /// 简单Table(一列)去除重复行
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable SelectDistinct(DataTable dt)
        {
            DataTable dtTag = dt.Copy();
            string tag = "";
            int j = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i] != null)
                {
                    if (tag.Contains(";" + dt.Rows[i]["tag"].ToString() + ";"))
                    {
                        dtTag.Rows.RemoveAt(i - j);
                        j = j + 1;
                    }
                    else
                    {
                        tag += ";" + dt.Rows[i]["tag"].ToString() + ";";
                    }
                }
            }
            return dtTag;
        }


    }
}
