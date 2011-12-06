using System;
using System.Collections.Generic;
using System.Text;

using Thinkment.Data;
using System.Web;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS
{
    /// <summary>
    /// 文章索引业务类
    /// </summary>
    [Serializable]
    [Helper("We7.ArticleIndexHelper")]
    public class ArticleIndexHelper : BaseHelper
    {
        /// <summary>
        /// 对ArticleIndex进行各项操作
        /// </summary>
        /// <param name="articleID">文章ID</param>
        /// <param name="operateType">操作类型0 新增索引，1更新索引，2删除索引</param>
        /// <returns></returns>
        public string InsertData(string articleID, int operateType)
        {
            try
            {
                //if (ArticleHelper.ExistDataBystate(articleID, 1))
                //{
                    ArticleIndex articleIndex = new ArticleIndex();
                    articleIndex.ArticleID = articleID;
                    articleIndex.IsLock = 0;
                    articleIndex.Operation = operateType;
                    if (ExistData(articleID))
                    {
                        Assistant.Update(articleIndex, new string[] { "Operation" });
                    }
                    else
                    {
                        Assistant.Insert(articleIndex);
                    }
                //}
                return articleID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询ArticleIndex是否存在此索引
        /// </summary>
        /// <param name="articleID">文章ID</param>
        /// <returns>存在索引返回true，否则为false</returns>
        public bool ExistData(string articleID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", articleID);
            if (Assistant.Count<ArticleIndex>(c) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 给一组文件建立索引
        /// </summary>
        /// <param name="articleIDs">一组文章ID的集合</param>
        /// <returns></returns>
        public int InsertData(List<string> articleIDs)
        {
            int count = 0;
            if (articleIDs != null && articleIDs.Count > 0)
            {
                foreach (string id in articleIDs)
                {
                    ArticleIndex articleIndex = new ArticleIndex();
                    articleIndex.ArticleID = id;
                    articleIndex.IsLock = 0;
                    articleIndex.Operation = 1;
                    Assistant.Insert(articleIndex);
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 获取站点的连接字符串
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            return db.ConnectionString;
        }

    }
}
