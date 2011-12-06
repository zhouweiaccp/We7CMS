using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Web;
using System.Xml;
using System.Data;
using System.Text.RegularExpressions;

using Thinkment.Data;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Framework.Config;
using We7.Framework;
using We7.Framework.Util;
using System.Reflection;
using We7.CMS.Accounts;

namespace We7.CMS
{
    /// <summary>
    /// 文章处理方法类
    /// </summary>
    [Serializable]
    [Helper("We7.ArticleHelper")]
    public partial class ArticleHelper : BaseHelper, IObjectClickHelper
    {


        #region 基本操作：插入、删除、更新、获取

        /// <summary>
        /// 根据条件获取文章数
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int QueryArtilceCount(Criteria c)
        {
            return Assistant.Count<Article>(c);
        }

        /// <summary>
        /// 根据ID删除一篇文章
        /// </summary>
        /// <param name="id">文章ID</param>
        public void DeleteArticle(string id)
        {
            //删除文章
            Article a = new Article();
            a.ID = id;
            Assistant.Delete(a);
            //删除相关文章
            DeleteRelatedArticles(id);
            Criteria ch = new Criteria(CriteriaType.Equals, "ArticleID", id);
            //删除引用

            //删除附件
            List<Attachment> atts = Assistant.List<Attachment>(ch, null);
            foreach (Attachment att in atts)
            {
                string file = HttpContext.Current.Server.MapPath(att.FilePath + "/" + att.FileName);
                if (File.Exists(file))
                    File.Delete(file);
                Assistant.Delete(att);
            }
            //清除wap相关

            //删除文章标签

            //删除评论
            List<Comments> coms = Assistant.List<Comments>(ch, null);
            foreach (Comments c in coms)
            {
                Assistant.Delete(c);
            }
        }

        /// <summary>
        /// 获取一篇文章
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <param name="fields">返回的字段集合</param>
        /// <returns></returns>
        public Article GetArticle(string id, string[] fields)
        {
            Article a = new Article();
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Article> aList = Assistant.List<Article>(c, null, 0, 0, fields);
            if (aList != null && aList.Count > 0)
            {
                a = aList[0];
            }
            return a;
        }

        private static readonly string ArticleKeyID = "$ArticleID{0}";
        /// <summary>
        /// 获取一篇文章（使用了缓存）
        /// </summary>
        /// <param name="ArticleID">文章ID</param>
        /// <returns></returns>
        public Article GetArticle(string ArticleID)
        {
            if (ArticleID != null && ArticleID != string.Empty)
            {
                HttpContext Context = HttpContext.Current;
                string key = string.Format(ArticleKeyID, ArticleID);
                Article article = (Article)Context.Items[key];//内存
                if (article == null)
                {
                    article = (Article)Context.Cache[key];//缓存
                    if (article == null)
                    {
                        if (ArticleID != null && ArticleID != string.Empty)
                        {
                            //读取数据库
                            Order[] o = new Order[] { new Order("ID") };
                            Criteria c = new Criteria(CriteriaType.Equals, "ID", ArticleID);
                            List<Article> articles = Assistant.List<Article>(c, o);
                            if (articles.Count > 0)
                            {
                                article = articles[0];
                            }
                        }

                        if (article != null)
                        {
                            CacherCache(key, Context, article, CacheTime.Short);
                        }
                    }

                    if (Context.Items[key] == null)
                    {
                        Context.Items.Remove(key);
                        Context.Items.Add(key, article);
                    }
                }
                return article;
            }
            else
                return null;

        }

        /// <summary>
        /// 通过ID获取文章标题
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns></returns>
        public string GetArticleName(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Article> articles = Assistant.List<Article>(c, null);
            if (articles.Count > 0)
                return articles[0].Title;
            else
                return "";
        }

        /// <summary>
        /// 通过SN获取栏目ID
        /// </summary>
        /// <param name="sn">栏目SN</param>
        /// <returns></returns>
        public string GetArticleIDBySN(string sn)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "SN", sn);
            List<Article> articles = Assistant.List<Article>(c, null);
            if (articles.Count > 0)
                return articles[0].ID;
            else
                return "";
        }

        /// <summary>
        /// 添加一篇文章
        /// </summary>
        /// <param name="a">一篇文章对象</param>
        /// <returns></returns>
        public Article AddArticles(Article a)
        {
            if (a.Created == DateTime.MinValue)
                a.Created = DateTime.Now;
            if (a.Updated == DateTime.MinValue)
                a.Updated = a.Created;
            if (string.IsNullOrEmpty(a.ID))
                a.ID = We7Helper.CreateNewID();
            a.SN = CreateArticleSN();
            a.Clicks = 0;
            a.CommentCount = 0;
            if (String.IsNullOrEmpty(a.ModelName))
            {
                a.ModelName = Constants.ArticleModelName;
            }
            Assistant.Insert(a);
            return a;
        }

        /// <summary>
        /// 添加一篇文章
        /// </summary>
        /// <param name="a">一篇文章对象</param>
        public void AddArticle(Article a)
        {
            if (a.Created == DateTime.MinValue)
                a.Created = DateTime.Now;
            if (a.Updated == DateTime.MinValue)
                a.Updated = a.Created;
            if (string.IsNullOrEmpty(a.ID))
                a.ID = We7Helper.CreateNewID();
            a.SN = CreateArticleSN();
            if (String.IsNullOrEmpty(a.ModelName))
            {
                a.ModelName = Constants.ArticleModelName;
            }
            a.Clicks = 0;
            a.CommentCount = 0;
            Assistant.Insert(a);
        }

        /// <summary>
        /// 创建文章的sn
        /// </summary>
        /// <returns></returns>
        public long CreateArticleSN()
        {
            CreateSNHelper helper = new CreateSNHelper();
            helper.Assistant = Assistant;
            return helper.SnBase;
        }

        /// <summary>
        /// 更新一篇文章记录
        /// </summary>
        /// <param name="a">一篇文章记录</param>
        /// <param name="fields">需要更新的字段</param>
        public void UpdateArticle(Article a, string[] fields)
        {
            //清除缓存
            HttpContext Context = HttpContext.Current;
            string key = string.Format(ArticleKeyID, a.ID);
            Context.Cache.Remove(key);
            Context.Items.Remove(key);

            //a.Updated = DateTime.Now;
            Assistant.Update(a, fields);
        }

        /// <summary>
        /// 查找某个站点共享过来的文章
        /// </summary>
        /// <param name="ownerID">栏目ID</param>
        /// <param name="sourceID">站点ID</param>
        /// <returns></returns>
        public Article GetArticleBySource(string ownerID, string sourceID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "SourceID", sourceID);
            if (ownerID != null)
                c.Add(CriteriaType.Equals, "OwnerID", ownerID);
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            List<Article> articles = Assistant.List<Article>(c, orders);
            if (articles.Count > 0)
                return articles[0];
            else
                return null;
        }

        /// <summary>
        /// 拷贝一篇文章到Wap
        /// </summary>
        /// <param name="sourceArticle">文章来源</param>
        /// <returns></returns>
        public Article CopyToWapArticle(Article sourceArticle)
        {
            Article wap = new Article();
            wap.Content = We7Helper.RemoveHtml(sourceArticle.Content);
            wap.Description = sourceArticle.Description;
            wap.Title = sourceArticle.Title;
            wap.SourceID = sourceArticle.ID;
            wap.Index = sourceArticle.Index;
            wap.Source = sourceArticle.Source;
            wap.AllowComments = sourceArticle.AllowComments;
            wap.IsImage = sourceArticle.IsImage;
            wap.Author = sourceArticle.Author;
            wap.State = 0;
            wap.ContentType = Convert.ToInt32(TypeOfArticle.WapArticle);
            wap.Overdue = sourceArticle.Overdue;
            wap.Thumbnail = sourceArticle.Thumbnail;
            wap.Updated = sourceArticle.Updated;
            return wap;
        }

        /// <summary>
        /// 更新一篇文章的流转状态
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <param name="ProcessState">流转状态</param>
        /// <param name="state">文章状态</param>
        public void UpdateArticleProcess(string id, string ProcessState, ArticleStates state)
        {
            Article a = GetArticle(id);
            a.ProcessState = ProcessState;
            a.State = (int)state;
            a.Updated = DateTime.Now;
            Assistant.Update(a, new string[] { "Updated", "ProcessState", "State" });
        }

        /// <summary>
        /// 通过Url取得文章列表
        /// </summary>
        /// <param name="url">用来查询的Url</param>
        /// <param name="from">起始记录</param>
        /// <param name="count">查询条数</param>
        /// <param name="fields">查询的字段</param>
        /// <param name="OnlyInUser">是否是只查询当前用户的文章</param>
        /// <returns>文章列表</returns>
        public List<Article> GetArticlesByUrl(string url, int from, int count, string[] fields, bool OnlyInUser)
        {
            Criteria c = new Criteria(CriteriaType.Like, "ChannelFullUrl", url);
            if (OnlyInUser)
                c.Add(CriteriaType.Equals, "State", 1);

            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Article>(c, orders, from, count, fields);
        }

        #endregion

        #region tag

        /// <summary>
        /// 通过标签集合获取带有这些标签的文章
        /// </summary>
        /// <param name="tags">标签集合</param>
        /// <returns>文章数</returns>
        public int QueryArticlesCountByTags(List<string> tags)
        {
            if (tags.Count > 0)
            {
                Criteria c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (string tag in tags)
                {
                    c.Add(CriteriaType.Like, "Tags", "'" + tag + "'");
                }

                return Assistant.Count<Article>(c);
            }
            else
                return 0;
        }


        /// <summary>
        /// 获取一个文章的所有标签集合
        /// </summary>
        /// <param name="articleID">文章ID</param>
        /// <returns></returns>
        public List<string> GetTags(string articleID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", articleID);

            List<Article> articles = Assistant.List<Article>(c, null);
            if (articles.Count > 0 && articles[0] != null)
            {
                string tags = articles[0].Tags.Replace("'", ",");
                List<string> list = new List<string>();
                if (tags.Length > 0)
                {
                    string[] temp = tags.Split(',');
                    if (temp.Length > 0)
                    {
                        foreach (string str in temp)
                        {
                            if (str.Trim().Length > 0 && !str.Equals(","))
                            {
                                list.Add(str);
                            }
                        }
                    }
                }
                return list;
            }
            else
                return null;
        }






        /// <summary>
        /// 获取一组文章标签记录
        /// </summary>
        /// <param name="tag">文章标签</param>
        /// <param name="articleID">文章ID</param>
        /// <returns></returns>
        //public int GetTagCount(string tag, string articleID)
        //{
        //    Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", articleID);
        //    c.Criterias.Add(new Criteria(CriteriaType.Equals, "Identifier", tag));
        //    return Assistant.Count<ArticleTag>(c);
        //}
        


        #endregion

        #region 相关文章

        /// <summary>
        /// 添加一条相关文章
        /// </summary>
        /// <param name="at">相关文章记录</param>
        public void AddRelatedArticle(RelatedArticle at)
        {
            at.ID = Guid.NewGuid().ToString();
            Assistant.Insert(at);
        }

        /// <summary>
        /// 获取一个篇文章的相关文章数
        /// </summary>
        /// <param name="tag">此参数无用</param>
        /// <param name="articleID">文章ID</param>
        /// <returns></returns>
        public int GetRelatedArticleCount(string tag, string articleID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", articleID);
            return Assistant.Count<RelatedArticle>(c);
        }

        /// <summary>
        ///  删除一篇文章且与articleID有关的相关文章第一条记录
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <param name="relateID">相关文章ID</param>
        public void DeleteRelatedArticle(string id, string relateID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", id);
            c.Add(CriteriaType.Equals, "RelatedID", relateID);
            List<RelatedArticle> ras = Assistant.List<RelatedArticle>(c, null);
            if (ras.Count > 0)
            {
                Assistant.Delete(ras[0]);
            }
        }

        /// <summary>
        /// 删除所有与articleID有关的所有相关文章记录
        /// </summary>
        /// <param name="articleID">文章ID</param>
        public void DeleteRelatedArticles(string articleID)
        {
            //先删除ArticleID=articleID
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", articleID);
            List<RelatedArticle> ras = Assistant.List<RelatedArticle>(c, null);
            foreach (RelatedArticle ra in ras)
            {
                Assistant.Delete(ra);
            }
            //再删除RelatedID=articleID
            c = new Criteria(CriteriaType.Equals, "RelatedID", articleID);
            ras = Assistant.List<RelatedArticle>(c, null);
            foreach (RelatedArticle ra in ras)
            {
                Assistant.Delete(ra);
            }
        }

        /// <summary>
        /// 获取一篇文章的相关文章记录
        /// </summary>
        /// <param name="articleID">文章ID</param>
        /// <returns></returns>
        public List<Article> GetRelatedArticles(string articleID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", articleID);
            List<RelatedArticle> ts = Assistant.List<RelatedArticle>(c, null);
            List<Article> articles = new List<Article>();
            List<string> ids = new List<string>();
            foreach (RelatedArticle ra in ts)
            {
                if (!ids.Contains(ra.RelatedID))
                {
                    Article a = GetArticle(ra.RelatedID, null);
                    if (a != null)
                    {
                        articles.Add(a);
                        ids.Add(ra.RelatedID);
                    }
                }
            }
            return articles;
        }

        # endregion

        #region IP策略
        /// <summary>
        /// 更新文章下的策略
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <param name="stragegy">策略信息</param>
        /// <returns></returns>
        public void UpdateStrategy(string id, string strategy)
        {
            Article article = GetArticle(id);

            if (article == null)
                return;

            article.IPStrategy = strategy;
            Assistant.Update(article, new string[] { "IPStrategy" });
        }

        /// <summary>
        /// 查询文章的安全策略
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns></returns>
        public string QueryStrategy(string id)
        {
            Article article = GetArticle(id);
            return article != null ? article.IPStrategy : String.Empty;
        }

        #endregion

        #region 文章查询：基于ArticleQuery的复杂查询

        /// <summary>
        /// 根据查询类生成Criteria
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>查询对象</returns>
        Criteria CreateCriteriaByAll(ArticleQuery query)
        {
            string parameters;
            string modelname = ChannelHelper.GetModelName(query.ChannelID, out parameters);

            Criteria c = new Criteria(CriteriaType.None);
            //if (String.IsNullOrEmpty(modelname) && query.EnumState != null && query.EnumState != "")
            //{
            //    Criteria csubC = new Criteria();
            //    csubC.Name = "EnumState";
            //    csubC.Value = query.EnumState;
            //    if (query.ExcludeThisChannel)
            //        csubC.Type = CriteriaType.NotEquals;
            //    else
            //        csubC.Type = CriteriaType.Equals;

            //    csubC.Adorn = Adorns.Substring;
            //    csubC.Start = EnumLibrary.Position[(int)EnumLibrary.Business.ArticleType];
            //    csubC.Length = EnumLibrary.PlaceLenth;
            //    c.Criterias.Add(csubC);
            //}
            if (String.IsNullOrEmpty(query.ModelName) || Constants.ArticleModelName.Equals(query.ModelName, StringComparison.OrdinalIgnoreCase))
            {
                AppendModelCondition(c);
            }

            if (query.State != ArticleStates.All)
                c.Add(CriteriaType.Equals, "State", (int)query.State);

            if (query.ArticleType > 0)
                c.Add(CriteriaType.Equals, "ContentType", query.ArticleType);
            else
                c.Add(CriteriaType.NotEquals, "ContentType", 16);//除去wap文章

            if (query.KeyWord != null && query.KeyWord.Length > 0)
            {
                Criteria keyCriteria = new Criteria(CriteriaType.None);
                keyCriteria.Mode = CriteriaMode.Or;
                keyCriteria.AddOr(CriteriaType.Like, "Title", "%" + query.KeyWord + "%");
                keyCriteria.AddOr(CriteriaType.Like, "Description", "%" + query.KeyWord + "%");
                c.Criterias.Add(keyCriteria);
            }

            if (query.Author != null && query.Author.Length > 0)
            {
                c.Add(CriteriaType.Like, "Author", "%" + query.Author + "%");
            }

            if (query.BeginDate <= query.EndDate)
            {
                if (query.BeginDate != DateTime.MinValue)
                    c.Add(CriteriaType.MoreThanEquals, "Updated", query.BeginDate);
                if (query.EndDate != DateTime.MinValue && query.EndDate != DateTime.MaxValue)
                    c.Add(CriteriaType.LessThanEquals, "Updated", query.EndDate.AddDays(1));
            }
            else
            {
                if (query.EndDate != DateTime.MinValue)
                    c.Add(CriteriaType.MoreThanEquals, "Updated", query.EndDate);
                if (query.BeginDate != DateTime.MaxValue)
                    c.Add(CriteriaType.LessThanEquals, "Updated", query.BeginDate.AddDays(1));
            }

            if (!We7Helper.IsEmptyID(query.ChannelID))
            {
                if (CheckModel(modelname))
                {
                    c.Add(CriteriaType.Equals, "ModelName", modelname);
                    if (!String.IsNullOrEmpty(parameters))
                    {
                        CriteriaExpressionHelper.Execute(c, parameters);
                    }
                }
                else
                {
                    if (query.ExcludeThisChannel)
                    {
                        if (query.IncludeAllSons)
                        {
                            c.Add(CriteriaType.NotLike, "ChannelFullUrl", query.ChannelFullUrl + "%");
                        }
                        else
                            c.Add(CriteriaType.NotEquals, "OwnerID", query.ChannelID);
                    }
                    else
                    {
                        if (query.IncludeAllSons)
                        {
                            c.Add(CriteriaType.Like, "ChannelFullUrl", query.ChannelFullUrl + "%");
                        }
                        else
                        {
                            if (query.ChannelID.Contains(","))
                            {
                                string[] oids = query.ChannelID.Split(',');
                                Criteria subC = new Criteria(CriteriaType.None);
                                subC.Mode = CriteriaMode.Or;
                                foreach (string s in oids)
                                {
                                    subC.Add(CriteriaType.Equals, "OwnerID", s);
                                }
                                c.Criterias.Add(subC);
                            }
                            else
                            {
                                c.Add(CriteriaType.Equals, "OwnerID", query.ChannelID);
                            }
                        }
                    }
                }
            }

            if (!We7Helper.IsEmptyID(query.AccountID))
            {
                Channel channel = ChannelHelper.GetChannel(query.ChannelID, null);

                if (query.IncludeAdministrable)
                {
                    List<string> channels = AccountHelper.GetObjectsByPermission(query.AccountID, "Channel.Article");

                    Criteria keyCriteria = new Criteria(CriteriaType.None);
                    if (channels != null && channels.Count > 0)
                    {
                        keyCriteria.Mode = CriteriaMode.Or;
                        foreach (string ownerID in channels)
                        {
                            keyCriteria.AddOr(CriteriaType.Equals, "OwnerID", ownerID);
                        }
                    }

                    keyCriteria.AddOr(CriteriaType.Equals, "AccountID", query.AccountID);

                    if (keyCriteria.Criterias.Count > 0)
                    {
                        c.Criterias.Add(keyCriteria);
                    }
                }
                else
                    c.Add(CriteriaType.Equals, "AccountID", query.AccountID);
            }

            if (query.IsShowHome != null && query.IsShowHome == "1")
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            if (query.Tag != null && query.Tag != "")
            {

                c.Add(CriteriaType.Like, "Tags", "%" + query.Tag + "%");
            }
            if (query.IsImage != null && query.IsImage == "1")
            {
                c.Add(CriteriaType.Equals, "IsImage", 1);
            }
            if (query.Overdue)
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;
                Criteria subChildC1 = new Criteria(CriteriaType.MoreThanEquals, "Overdue", DateTime.Now);
                subC.Criterias.Add(subChildC1);
                //Criteria subChildC2 = new Criteria(CriteriaType.Equals, "Overdue", DateTime.MinValue);
                //subC.Criterias.Add(subChildC2);
                Criteria subChildC3 = new Criteria(CriteriaType.LessThanEquals, "Overdue", DateTime.Today.AddYears(-30));
                subC.Criterias.Add(subChildC3);
                c.Criterias.Add(subC);
            }

            if (!string.IsNullOrEmpty(query.ArticleParentID))
            {
                c.Add(CriteriaType.Equals, "ParentID", query.ArticleParentID);
            }

            //if (query.SearcherKey != null && query.SearcherKey != "")
            //{
            //    c.Add(CriteriaType.Like, "Title", "%" + query.SearcherKey + "%");
            //}

            if (query.ArticleID != null && query.ArticleID != "")
            {
                c.Add(CriteriaType.Like, "ListKeys", "%" + query.ArticleID + "%");
            }
            return c;
        }

        /// <summary>
        /// 按是否有内容模型的方式查询信息
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <returns>查询对象</returns>
        Criteria CreateCriteriaByModel(ArticleQuery query)
        {
            Criteria c = new Criteria(CriteriaType.None);

            if (query.State != ArticleStates.All)
                c.Add(CriteriaType.Equals, "State", (int)query.State);

            if (query.ArticleType > 0)
                c.Add(CriteriaType.Equals, "ContentType", query.ArticleType);
            else
                c.Add(CriteriaType.NotEquals, "ContentType", 16);//除去wap文章

            if (query.KeyWord != null && query.KeyWord.Length > 0)
            {
                Criteria keyCriteria = new Criteria(CriteriaType.None);
                keyCriteria.Mode = CriteriaMode.Or;
                keyCriteria.AddOr(CriteriaType.Like, "Title", "%" + query.KeyWord + "%");
                keyCriteria.AddOr(CriteriaType.Like, "Description", "%" + query.KeyWord + "%");
                c.Criterias.Add(keyCriteria);
            }

            if (query.Author != null && query.Author.Length > 0)
            {
                c.Add(CriteriaType.Like, "Author", "%" + query.Author + "%");
            }

            if (query.BeginDate <= query.EndDate)
            {
                if (query.BeginDate != DateTime.MinValue)
                    c.Add(CriteriaType.MoreThanEquals, "Updated", query.BeginDate);
                if (query.EndDate != DateTime.MinValue && query.EndDate != DateTime.MaxValue)
                    c.Add(CriteriaType.LessThanEquals, "Updated", query.EndDate.AddDays(1));
            }
            else
            {
                if (query.EndDate != DateTime.MinValue)
                    c.Add(CriteriaType.MoreThanEquals, "Updated", query.EndDate);
                if (query.BeginDate != DateTime.MaxValue)
                    c.Add(CriteriaType.LessThanEquals, "Updated", query.BeginDate.AddDays(1));
            }



            if (query.UseModel)
            {
                if (!We7Helper.IsEmptyID(query.ChannelID) && (String.IsNullOrEmpty(query.ModelName) || String.IsNullOrEmpty(query.ModelName.Trim())))
                {
                    string parameters;
                    string modelname = ChannelHelper.GetModelName(query.ChannelID, out parameters);
                    c.Add(CriteriaType.Equals, "ModelName", modelname);
                    if (!String.IsNullOrEmpty(parameters))
                    {
                        CriteriaExpressionHelper.Execute(c, parameters);
                    }
                }
                else
                {
                    c.Add(CriteriaType.Equals, "ModelName", query.ModelName);
                }
            }
            else
            {
                if (!We7Helper.IsEmptyID(query.ChannelID))
                {
                    string parameters;
                    string modelname = ChannelHelper.GetModelName(query.ChannelID, out parameters);
                    c.Add(CriteriaType.Equals, "ModelName", modelname);
                    if (query.ExcludeThisChannel)
                    {
                        if (query.IncludeAllSons)
                        {
                            c.Add(CriteriaType.NotLike, "ChannelFullUrl", query.ChannelFullUrl + "%");
                        }
                        else
                            c.Add(CriteriaType.NotEquals, "OwnerID", query.ChannelID);
                    }
                    else
                    {
                        if (query.IncludeAllSons)
                        {
                            c.Add(CriteriaType.Like, "ChannelFullUrl", query.ChannelFullUrl + "%");
                        }
                        else
                        {
                            if (query.ChannelID.Contains(","))
                            {
                                string[] oids = query.ChannelID.Split(',');
                                Criteria subC = new Criteria(CriteriaType.None);
                                subC.Mode = CriteriaMode.Or;
                                foreach (string s in oids)
                                {
                                    subC.Add(CriteriaType.Equals, "OwnerID", s);
                                }
                                c.Criterias.Add(subC);
                            }
                            else
                            {
                                c.Add(CriteriaType.Equals, "OwnerID", query.ChannelID);
                            }
                        }
                    }
                }
            }

            if (!We7Helper.IsEmptyID(query.AccountID))
            {
                Channel channel = ChannelHelper.GetChannel(query.ChannelID, null);

                if (query.IncludeAdministrable)
                {
                    List<string> channels = AccountHelper.GetObjectsByPermission(query.AccountID, "Channel.Article");

                    Criteria keyCriteria = new Criteria(CriteriaType.None);
                    if (channels != null && channels.Count > 0)
                    {
                        keyCriteria.Mode = CriteriaMode.Or;
                        foreach (string ownerID in channels)
                        {
                            keyCriteria.AddOr(CriteriaType.Equals, "OwnerID", ownerID);
                        }
                    }

                    keyCriteria.AddOr(CriteriaType.Equals, "AccountID", query.AccountID);

                    if (keyCriteria.Criterias.Count > 0)
                    {
                        c.Criterias.Add(keyCriteria);
                    }
                }
                else
                    c.Add(CriteriaType.Equals, "AccountID", query.AccountID);
            }

            if (query.IsShowHome != null && query.IsShowHome == "1")
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            if (query.Tag != null && query.Tag != "")
            {

                c.Add(CriteriaType.Like, "Tags", "%" + query.Tag + "%");
            }
            if (query.IsImage != null && query.IsImage == "1")
            {
                c.Add(CriteriaType.Equals, "IsImage", 1);
            }
            if (query.Overdue)
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;
                Criteria subChildC1 = new Criteria(CriteriaType.MoreThanEquals, "Overdue", DateTime.Now);
                subC.Criterias.Add(subChildC1);
                //Criteria subChildC2 = new Criteria(CriteriaType.Equals, "Overdue", DateTime.MinValue);
                //subC.Criterias.Add(subChildC2);
                Criteria subChildC3 = new Criteria(CriteriaType.LessThanEquals, "Overdue", DateTime.Today.AddYears(-30));
                subC.Criterias.Add(subChildC3);
                c.Criterias.Add(subC);
            }

            //if (query.SearcherKey != null && query.SearcherKey != "")
            //{
            //    c.Add(CriteriaType.Like, "Title", "%" + query.SearcherKey + "%");
            //}

            if (query.ArticleID != null && query.ArticleID != "")
            {
                c.Add(CriteriaType.Like, "ListKeys", "%" + query.ArticleID + "%");
            }
            return c;
        }


        /// <summary>
        /// 根据查询类生成Criteria
        /// </summary>
        /// <param name="queryParam">存入对象的Hashtable键值对</param>
        /// <returns></returns>
        Criteria CreateCriteriaByParameter(Hashtable queryParam)
        {
            Criteria c = new Criteria(CriteriaType.None);
            foreach (DictionaryEntry de in queryParam)
            {
                c.Add(CriteriaType.Equals, de.Key.ToString(), de.Value);
            }
            return c;
        }

        /// <summary>
        /// 根据查询类获得文章数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int QueryArtilceCountByAll(ArticleQuery query)
        {
            Criteria c = CreateCriteriaByAll(query);
            return Assistant.Count<Article>(c);
        }

        /// <summary>
        /// 根据查询类获得文章数量(含有内容模型信息)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int QueryArtilceModelCountByAll(ArticleQuery query)
        {
            Criteria c = CreateCriteriaByModel(query);
            return Assistant.Count<Article>(c);
        }


        /// <summary>
        /// 根据查询类获取文章列表（分页）
        /// </summary>
        /// <param name="query">查询类</param>
        /// <param name="from">第几条开始</param>
        /// <param name="count">获取条数</param>
        /// <param name="fields">string[]字段列表，null为全部</param>
        /// <returns></returns>
        public List<Article> QueryArtilcesByAll(ArticleQuery query, int from, int count, string[] fields)
        {
            try
            {
                Criteria c = CreateCriteriaByAll(query);
                List<Order> orders = CreateOrdersByAll(query.OrderKeys);
                Order[] o = null;
                if (orders != null) o = orders.ToArray();

                return Assistant.List<Article>(c, o, from, count, fields);
            }
            catch (Exception ex)
            {
            }
            return new List<Article>();
        }

        /// <summary>
        /// 根据查询类获取文章列表（分页）
        /// </summary>
        /// <param name="query">查询类</param>
        /// <param name="from">第几条开始</param>
        /// <param name="count">获取条数</param>
        /// <param name="fields">string[]字段列表，null为全部</param>
        /// <returns></returns>
        public List<Article> QueryArtilceModelByAll(ArticleQuery query, int from, int count, string[] fields)
        {
            Criteria c = CreateCriteriaByModel(query);
            List<Order> orders = CreateOrdersByAll(query.OrderKeys);
            Order[] o = null;
            if (orders != null) o = orders.ToArray();

            return Assistant.List<Article>(c, o, from, count, fields);
        }



        /// <summary>
        /// 根据查询实体得到查询参数对象
        /// </summary>
        /// <param name="queryEntity">查询参数实体</param>
        /// <returns>查询参数对象 Criteria</returns>
        private Criteria GetCriteriaByQueryEntity(QueryEntity queryEntity)
        {
            Criteria c = null;
            if (queryEntity != null)
            {
                c = new Criteria(CriteriaType.Equals, "ModelName", queryEntity.ModelName);
                List<QueryParam> queryPanamList = queryEntity.QueryParams;
                for (int i = 0; i < queryPanamList.Count; i++)
                {
                    QueryParam qp = queryPanamList[i];
                    if (qp.CriteriaType == CriteriaType.Like)
                        qp.ColumnValue = String.Format("%{0}%", qp.ColumnValue);
                    c.Add(qp.CriteriaType, qp.ColumnKey, qp.ColumnValue);
                }
            }

            return c;
        }

        /// <summary>
        /// 创建模型查询参数
        /// </summary>
        /// <returns></returns>
        public static Criteria CreateModelCondition()
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Mode = CriteriaMode.Or;
            c.AddOr(CriteriaType.Equals, "ModelName", Constants.ArticleModelName);
            c.AddOr(CriteriaType.Equals, "ModelName", String.Empty);
            c.AddOr(CriteriaType.IsNull, "ModelName", null);
            return c;
        }

        /// <summary>
        /// 为查询条件追加模型参数
        /// </summary>
        /// <param name="c"></param>
        public static void AppendModelCondition(Criteria c)
        {
            if (c == null)
            {
                throw new ArgumentNullException("参数不能为代");
            }
            c.Criterias.Add(CreateModelCondition());
        }

        #region 文章查询记录
        /*
        /// <summary>
        /// 根据查询类获得文章数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int QueryArtilceCountByAll(ArticleQuery query)
        {
            Criteria c = CreateCriteriaByAll(query);
            return Assistant.Count<Article>(c);
        }

        /// <summary>
        /// 根据查询类获取文章列表（分页）
        /// </summary>
        /// <param name="query">查询类</param>
        /// <param name="from">第几条开始</param>
        /// <param name="count">获取条数</param>
        /// <param name="fields">string[]字段列表，null为全部</param>
        /// <returns></returns>
        public List<Article> QueryArtilcesByAll(ArticleQuery query, int from, int count, string[] fields)
        {
            Criteria c = CreateCriteriaByAll(query);
            List<Order> orders = CreateOrdersByAll(query.OrderKeys);
            Order[] o = null;
            if (orders != null) o = orders.ToArray();

            return Assistant.List<Article>(c, o, from, count, fields);
        }
        */

        #endregion

        /// <summary>
        /// 按栏目查找文章
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="includechildren"></param>
        /// <returns></returns>
        public List<Article> QueryArticlesByChannel(string cid, bool includechildren)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (includechildren)
            {
                Channel ch = ChannelHelper.GetChannel(cid, null);
                c.Add(CriteriaType.Like, "ChannelFullUrl", ch.FullUrl + "%");
            }
            else
            {
                c.Add(CriteriaType.Equals, "OwnerID", cid);
            }

            return Assistant.List<Article>(c, new Order[] { new Order("Updated", OrderMode.Desc) });
        }

        /// <summary>
        /// 按栏目查找文章(分页)
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="includechildren"></param>
        /// <param name="from"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<Article> QueryArticlesByChannel(string cid, bool includechildren, int from, int PageSize)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (includechildren)
            {
                Channel ch = ChannelHelper.GetChannel(cid, null);
                c.Add(CriteriaType.Like, "ChannelFullUrl", ch.FullUrl + "%");
                c.Add(CriteriaType.Equals, "State", 1);
            }
            else
            {
                c.Add(CriteriaType.Equals, "OwnerID", cid);
                c.Add(CriteriaType.Equals, "State", 1);
            }

            return Assistant.List<Article>(c, new Order[] { new Order("Updated", OrderMode.Desc) }, from, PageSize);
        }

        public List<Article> QueryModelRecordByChannelID(string cid, int start, int count)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", cid);
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };

            return Assistant.List<Article>(c, orders, start, count);
        }

        public int QueryModelRecordCountByChannelID(string cid)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", cid);
            return Assistant.Count<Article>(c);
        }

        public List<Article> QueryModelRecordByChannelID2(string cid, int start, int count)
        {

            //Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", cid);
            Criteria c = new Criteria(CriteriaType.None);
            ExtendCriteria(c, cid);
            Order[] orders = new Order[] { new Order("IsShow", OrderMode.Desc), new Order("Updated", OrderMode.Desc) };

            return Assistant.List<Article>(c, orders, start, count);
        }

        public int QueryModelRecordCountByChannelID2(string cid)
        {
            //Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", cid);
            Criteria c = new Criteria(CriteriaType.None);
            ExtendCriteria(c, cid);
            return Assistant.Count<Article>(c);
        }

        /// <summary>
        ///  根据查询类获取文章列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Article> QueryArtilcesByAll(ArticleQuery query)
        {
            try
            {
                Criteria c = CreateCriteriaByAll(query);

                List<Order> orders = CreateOrdersByAll(query.OrderKeys);
                Order[] o = null;
                if (orders != null) o = orders.ToArray();

                return Assistant.List<Article>(c, o);
            }
            catch (Exception ex)
            {
            }
            return new List<Article>();
        }

        /// <summary>
        /// 获取某段时间内发表的文章数
        /// </summary>
        /// <param name="begin">从第几条记录开始</param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int GetArticleCountByTime(DateTime begin, DateTime end)
        {
            Criteria c = new Criteria(CriteriaType.MoreThanEquals, "Created", begin);
            c.Add(CriteriaType.LessThanEquals, "Created", end);
            if (Assistant.Count<Article>(c) > 0)
            {
                return Assistant.Count<Article>(c);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 按模型查询数据
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public List<Article> QueryArticleByModel(string modelName)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ModelName", modelName);
            return Assistant.List<Article>(c, new Order[] { new Order("Updated", OrderMode.Desc) });
        }

        #endregion

        #region 特殊方法
        /// <summary>
        /// 按照配置文件加水印到图片
        /// </summary>
        /// <param name="ImageConfig">图片配置类</param>
        /// <param name="thumbnailFile">加水印后文件</param>
        /// <param name="originalFilePath">原文件</param>
        public static void AddWatermarkToImage(GeneralConfigInfo ImageConfig, string thumbnailFile, string originalFilePath)
        {
            if (ImageConfig.WaterMarkStatus != 0)
            {
                string waterparkedFile = ImageUtils.GenerateWatermarkFile(originalFilePath);
                System.Drawing.Image img = System.Drawing.Image.FromFile(thumbnailFile);
                System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                img.Dispose();
                img = null;

                if (ImageConfig.WaterMarkType == 1 && File.Exists(We7Utils.GetMapPath(ImageConfig.WaterMarkPicfile)))
                {
                    ImageUtils.AddImageSignPic(bmp, waterparkedFile, We7Utils.GetMapPath(ImageConfig.WaterMarkPicfile), ImageConfig.WaterMarkStatus, ImageConfig.AttachImageQuality, ImageConfig.WaterMarkTransparency);
                }
                else
                {
                    string watermarkText = ImageConfig.WaterMarkText;
                    //watermarkText = ImageConfig.Watermarktext.Replace("{1}", ImageConfig.Forumtitle);
                    //watermarkText = watermarkText.Replace("{2}", "http://" + DNTRequest.GetCurrentFullHost() + "/");
                    //watermarkText = watermarkText.Replace("{3}", Utils.GetDate());
                    //watermarkText = watermarkText.Replace("{4}", Utils.GetTime());
                    ImageUtils.AddImageSignText(bmp, waterparkedFile, watermarkText, ImageConfig.WaterMarkStatus, ImageConfig.AttachImageQuality, ImageConfig.WaterMarkFontName, ImageConfig.WaterMarkFontSize);
                }

                bmp.Dispose();
                bmp = null;

                if (File.Exists(waterparkedFile))
                {
                    System.IO.File.Delete(thumbnailFile);
                    System.IO.File.Copy(waterparkedFile, thumbnailFile);
                    System.IO.File.Delete(waterparkedFile);
                }
            }
        }
        /// <summary>
        /// 创建并写入Config文件
        /// </summary>
        /// <param name="path"></param>
        public void Write(string path)
        {
            string str = "<?xml version=\"" + "1.0\"?>" +
"<GeneralConfigInfo xmlns:xsi=\"" + "http://www.w3.org/2001/XMLSchema-instance\"" + " xmlns:xsd=\"" + "http://www.w3.org/2001/XMLSchema\"" + ">" +
"<SiteTitle>We7</SiteTitle>" +
"<IcpInfo />" +
"<RewriteUrl />" +
"<UrlExtName>.aspx</UrlExtName>" +
"<PostInterval>0</PostInterval>" +
"<WaterMarkStatus>3</WaterMarkStatus>" +
"<WaterMarkType>0</WaterMarkType>" +
"<WaterMarkTransparency>5</WaterMarkTransparency>" +
"<WaterMarkText>We7.cn</WaterMarkText>" +
"<WaterMarkPic>watermark.gif</WaterMarkPic>" +
"<WaterMarkFontName>Tahoma</WaterMarkFontName>" +
"<WaterMarkFontSize>12</WaterMarkFontSize>" +
"<AttachImageQuality>80</AttachImageQuality>" +
"<OverdueDateTime>365</OverdueDateTime>" +
"<ADVisbleToSite>0</ADVisbleToSite>" +
"</GeneralConfigInfo>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str);
            doc.Save(path);
        }

        #endregion

        #region 静态URL
        private static readonly string ArticleKeyName = "$ArticleID{0}";

        /// <summary>
        /// 通过URL取得文章ID号
        /// </summary>
        /// <returns></returns>
        public string GetArticleIDFromURL()
        {
            HttpContext Context = HttpContext.Current;
            if (Context.Request["aid"] != null)
                return Context.Request["aid"];
            else
            {
                return GetArticleIDFromURL(Context.Request.RawUrl);
            }
        }

        /// <summary>
        /// 通过URL取得文章ID号
        /// </summary>
        /// <returns></returns>
        public string GetArticleIDFromURL(string url)
        {
            string id = GetArticleIDOrSNFromUrl(url);
            if (We7Helper.IsNumber(id))
                return GetArticleIDBySN(id);
            return id;
        }

        /// <summary>
        /// 从url获取文章id或者SN
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetArticleIDOrSNFromUrl(string path)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si == null) return "";
            string ext = si.UrlFormat;
            if (ext == null || ext.Length == 0) ext = "html";

            if (path.LastIndexOf("?") > -1)
            {
                if (path.ToLower().IndexOf("article=") > -1)
                    path = path.Substring(path.ToLower().IndexOf("article=") + 8);
                else
                    path = path.Remove(path.LastIndexOf("?"));
            }

            string mathstr = @"/(\w|\s|(-)|(_))+\." + ext + "$";
            if (path.ToLower().EndsWith("default." + ext))
                path = path.Remove(path.Length - 12);
            if (path.ToLower().EndsWith("index." + ext))
                path = path.Remove(path.Length - 10);

            if (Regex.IsMatch(path, mathstr))
            {
                int lastSlash = path.LastIndexOf("/");
                if (lastSlash > -1)
                {
                    path = path.Remove(0, lastSlash + 1);
                }

                int lastDot = path.LastIndexOf(".");
                if (lastDot > -1)
                {
                    path = path.Remove(lastDot, path.Length - lastDot);
                }

                if (We7Helper.IsGUID(We7Helper.FormatToGUID(path)))
                    path = We7Helper.FormatToGUID(path);
                else
                {
                    int lastSub = path.LastIndexOf("-");
                    if (lastSub > -1)
                    {
                        path = path.Remove(0, lastSub + 1);
                    }

                    if (!We7Helper.IsNumber(path))
                        path = "";
                }

                return path;
            }
            else
                return string.Empty;

        }


        #endregion

        #region 更新冗余字段，定义Helper对象
        /// <summary>
        /// Http上下文
        /// </summary>
        HttpContext context
        { get { return HttpContext.Current; } }
        /// <summary>
        /// 业对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)context.Application[HelperFactory.ApplicationID]; }
        }

        /// <summary>
        /// 统计业务对象
        /// </summary>
        protected StatisticsHelper StatisticsHelper
        {
            get { return HelperFactory.GetHelper<StatisticsHelper>(); }
        }

        /// <summary>
        /// 评论业务对象
        /// </summary>
        protected CommentsHelper CommentsHelper
        {
            get { return HelperFactory.GetHelper<CommentsHelper>(); }
        }
        /// <summary>
        /// 栏目业务对象
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// 权限业务对象
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// 按更新时间获取所有信息记录
        /// </summary>
        /// <returns></returns>
        public List<Article> GetAllArticle()
        {
            Order[] o = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Article>(null, o);
        }

        /// <summary>
        /// 更新点击数
        /// </summary>
        /// <returns></returns>
        public int UpdateClicks()
        {
            int count = 0;
            List<Article> allList = GetAllArticle();
            if (allList != null)
            {
                foreach (Article article in allList)
                {
                    article.Clicks = StatisticsHelper.GetArticleStatisticsCount(article.ID);
                    Assistant.Update(article, new string[] { "Clicks" });
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 更新评论统计数
        /// </summary>
        /// <returns></returns>
        public int UpdateCommentCount()
        {
            int count = 0;
            List<Article> allList = GetAllArticle();
            if (allList != null)
            {
                foreach (Article article in allList)
                {
                    article.CommentCount = CommentsHelper.ArticleIDQueryCommentsCount(article.ID, true);
                    Assistant.Update(article, new string[] { "CommentCount" });
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 查找包含子栏目文章
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        /// <param name="IncludeAllSons"></param>
        /// <returns></returns>
        Criteria CreateCriteriaBySubChannelList(string ChannelID, bool IncludeAllSons)
        {
            Criteria subCriteria = new Criteria(CriteriaType.None);
            //包含所有子栏目的文章
            if (IncludeAllSons)
            {
                Channel channel = ChannelHelper.GetChannel(ChannelID, new string[] { "FullUrl" });
                if (channel != null)
                    subCriteria.AddOr(CriteriaType.Like, "ChannelFullUrl", "%" + channel.FullUrl + "%");
                else
                    subCriteria.AddOr(CriteriaType.Equals, "OwnerID", "-1");
            }
            else
                subCriteria.Add(CriteriaType.Equals, "OwnerID", ChannelID);

            return subCriteria;
        }

        /// <summary>
        /// 更新文章标签
        /// 2011-11-9 标签已经整合，未发现此方法的引用
        /// 
        /// </summary>
        /// <returns></returns>
        //public int UpdateArticleTags()
        //{
        //    int count = 0;
        //    List<Article> allList = GetAllArticle();
        //    if (allList != null)
        //    {
        //        foreach (Article article in allList)
        //        {
        //            StringBuilder sb = new StringBuilder();
        //           
        //            //List<ArticleTag> allTagsByArticleID = GetTags(article.ID);
        //            //if (allTagsByArticleID != null)
        //            //{
        //            //    int i = 0;
        //            //    foreach (ArticleTag articleTag in allTagsByArticleID)
        //            //    {
        //            //        sb.Append(articleTag.Identifier);
        //            //        if (allTagsByArticleID.Count > i + 1)
        //            //            sb.Append(",");
        //            //        i++;
        //            //    }
        //            //}
        //            //if (sb.ToString() != "")
        //            //{
        //            //    article.Tags = sb.ToString();
        //            //    Assistant.Update(article, new string[] { "Tags" });
        //            //    count++;
        //            //}
        //        }
        //    }
        //    return count;
        //}

        /// <summary>
        /// 更新文章栏目ChannelFullUrl，文章中EnumState为空就初始化，文章，产品等过期已过把文章状态设成过期状态，默认的过期时间为100天
        /// </summary>
        /// <returns></returns>
        public int UpdateOtherFieldArticle()
        {
            int count = 0;
            List<Article> allList = GetAllArticle();
            if (allList != null)
            {
                foreach (Article article in allList)
                {
                    Channel ch = null;
                    if (String.IsNullOrEmpty(article.OwnerID) && !String.IsNullOrEmpty(article.ModelName))
                    {
                        ch = ChannelHelper.FirestByModelName(article.ModelName);
                    }
                    else
                    {
                        ch = ChannelHelper.GetChannel(article.OwnerID, new string[] { "FullUrl", "FullPath", "ChannelName" });
                    }
                    if (ch != null)
                    {
                        article.ChannelFullUrl = ch.FullUrl;
                        article.FullChannelPath = ch.FullPath;
                        article.ChannelName = ch.Name;
                        List<string> listString = new List<string>();
                        listString.Add("ChannelFullUrl");
                        listString.Add("FullChannelPath");
                        listString.Add("ChannelName");
                        string[] updateString = listString.ToArray();
                        Assistant.Update(article, updateString);
                        count++;
                    }
                }
            }
            return count;
        }
        /// <summary>
        ///标签查询
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="from">从第几条记录开始</param>
        /// <param name="count">返回的记录数</param>
        /// <param name="fields">返回的字段集合</param>
        /// <returns></returns>
        public List<Article> QueryArticlesByTags(List<string> tags, int from, int count, string[] fields)
        {
            if (tags.Count > 0)
            {
                Criteria c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (string tag in tags)
                {
                    c.Add(CriteriaType.Like, "Tags", "%'" + tag + "'%");
                }

                Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
                return Assistant.List<Article>(c, orders, from, count, fields);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 取得我的审核栏目
        /// </summary>
        /// <param name="state"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public List<Article> GetArticlesByState(ArticleStates state, string accountID, int from, int count)
        {
            List<Article> list = new List<Article>();
            Criteria c = new Criteria(CriteriaType.Equals, "State", (int)state);
            c.Criterias.Add(new Criteria(CriteriaType.Equals, "AccountID", accountID));
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            list = Assistant.List<Article>(c, orders, from, count);
            return list;
        }
        #endregion


        #region 数据移植或数据采集后的数据批量更新

        /// <summary>
        /// 数据移植之后把原有数据建立索引
        /// 禁用（停用）；审核中；过期；回收站的数据部不应该写到索引表里面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state">信息状态，1—启用；0—禁用（停用）；2—审核中；3—过期；4—回收站</param>
        /// <returns></returns>
        public List<string> GetDataBystate(int state)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "State", state);
            List<Article> articleList = Assistant.List<Article>(c, null);
            List<string> stringList = new List<string>();
            foreach (Article a in articleList)
            {
                stringList.Add(a.ID);
            }
            return stringList;
        }

        /// <summary>
        /// 用于数据采集来的数据进行批量更新SN
        /// </summary>
        /// <returns></returns>
        public int UpdateSN()
        {
            int count = 0;
            long sn = 0;
            Criteria c = new Criteria(CriteriaType.Equals, "SN", 0);
            List<Article> articleList = Assistant.List<Article>(c, null);
            if (articleList != null && articleList.Count > 0)
            {
                foreach (Article a in articleList)
                {
                    if (count == 0)
                    {
                        sn = CreateArticleSN();
                    }
                    a.SN = sn;
                    UpdateArticle(a, new string[] { "SN" });
                    sn++;
                    count++;
                }
            }
            return count;
        }

        #endregion

        #region

        bool CheckModel(string modelname)
        {
            if (!String.IsNullOrEmpty(modelname))
            {
                modelname = modelname.ToLower();
                List<string> list = new List<string>() { "article", "system.article" };
                return !list.Contains(modelname.ToLower());
            }
            return false;
        }

        void ExtendCriteria(Criteria c, string oid)
        {
            string parameters, modelname;
            modelname = ChannelHelper.GetModelName(oid, out parameters);
            if (CheckModel(modelname))
            {
                c.Add(CriteriaType.Equals, "ModelName", modelname);
                if (!String.IsNullOrEmpty(parameters))
                {
                    CriteriaExpressionHelper.Execute(c, parameters);
                }
            }
        }
        #endregion

        #region 根据查询实体得到Artilce
        /// <summary>
        /// 参数查询得到数据总条数
        /// </summary>
        /// <param name="queryEntity">查询实体</param>
        /// <returns>数据总条数</returns>
        public int QueryArtilceModelCountByParameter(QueryEntity queryEntity)
        {
            if (queryEntity != null)
            {
                Criteria c = new Criteria(CriteriaType.Equals, "ModelName", queryEntity.ModelName);
                List<QueryParam> queryPanamList = queryEntity.QueryParams;
                for (int i = 0; i < queryPanamList.Count; i++)
                {
                    QueryParam qp = queryPanamList[i];
                    if (qp.CriteriaType == CriteriaType.Like)
                    {
                        qp.ColumnValue = string.Format("%{0}%", qp.ColumnValue);
                    }
                    c.Add(qp.CriteriaType, qp.ColumnKey, qp.ColumnValue);
                }
                return Assistant.Count<Article>(c);
            }
            return 0;
        }

        /// <summary>
        /// 查询文章集合
        /// </summary>
        /// <param name="queryEntity">查询参数实体</param>
        /// <param name="orders">排序规则实体数组</param>
        /// <param name="from">开始条数</param>
        /// <param name="count">每页条数</param>
        /// <param name="fields">节点数组</param>
        /// <returns>文章实体泛型集合</returns>
        public List<Article> QueryArticles(QueryEntity queryEntity, int from, int count, string[] fields)
        {

            if (queryEntity != null)
            {
                Criteria c = GetCriteriaByQueryEntity(queryEntity);
                return Assistant.List<Article>(c, queryEntity.Orders, from, count, fields);


            }
            else
            {
                return new List<Article>();
            }
        }

        #endregion

        #region IObjectClickHelper实现
        /// <summary>
        /// 更新指定对象的点击量报表
        /// </summary>
        /// <param name="modelName">模块名称</param>
        /// <param name="id">文章ID</param>
        /// <param name="dictClickReport">点击量报表</param>
        public void UpdateClicks(string modelName, string id, Dictionary<string, int> dictClickReport)
        {
            ArticleHelper helper = HelperFactory.GetHelper<ArticleHelper>();
            Article targetObject = helper.GetArticle(id);
            if (targetObject != null)
            {
                targetObject.DayClicks = dictClickReport["日点击量"];
                targetObject.YesterdayClicks = dictClickReport["昨日点击量"];
                targetObject.WeekClicks = dictClickReport["周点击量"];
                targetObject.MonthClicks = dictClickReport["月点击量"];
                targetObject.QuarterClicks = dictClickReport["季点击量"];
                targetObject.YearClicks = dictClickReport["年点击量"];
                targetObject.Clicks = dictClickReport["总点击量"];

                Assistant.Update(targetObject, new string[] { "DayClicks", "YesterdayClicks", "WeekClicks", "MonthClicks", "QuarterClicks", "YearClicks", "Clicks" });
            }
        }
        #endregion
    }
    /// <summary>
    /// 创建序列号SN处理类    /// </summary>
    [Serializable]
    public class CreateSNHelper
    {
        static object lockHelper = new object();//互斥锁

        private static long snBase = 0;
        private static long AppSN = 0;

        //public long SnBase
        //{
        //    get
        //    {
        //        lock (lockHelper)
        //        {

        //            long result = 0;
        //            //if (snBase != 0)
        //            //{
        //            long maxSn = 0;

        //            ListField[] fields = new ListField[1];
        //            ListField field = new ListField("SN");
        //            field.Adorn = Adorns.Distinct;
        //            fields[0] = field;
        //            Order[] tmpOrders = new Order[] { new Order("SN", OrderMode.Desc) };

        //            List<Article> tempList = assistant.List<Article>(null, tmpOrders, 0, 0, fields);
        //            long totalHave = (long)tempList.Count;

        //            long totalAll = Assistant.Count<Article>(null);
        //            if (totalAll > totalHave)
        //            {
        //                Order[] orders = new Order[] { new Order("SN", OrderMode.Asc) };
        //                List<Article> articles = Assistant.List<Article>(null, orders);
        //                foreach (Article a in articles)
        //                {
        //                    if (a.SN > maxSn) maxSn = a.SN;
        //                }

        //                //处理没有SN与SN重复的文章
        //                long lastSn = 0;//记载上一个SN
        //                long curSn = 0;//记载本次修改前的SN
        //                foreach (Article a in articles)
        //                {
        //                    curSn = a.SN;
        //                    if (a.SN > 0 && a.SN == lastSn)
        //                    {
        //                        a.SN = ++maxSn;
        //                        UpdateArticle(a, new string[] { "SN" });
        //                    }

        //                    if (a.SN <= 0)
        //                    {
        //                        a.SN = ++maxSn;
        //                        UpdateArticle(a, new string[] { "SN" });
        //                    }
        //                    lastSn = curSn;
        //                }
        //            }
        //            else
        //            {
        //                Order[] orders = new Order[] { new Order("SN", OrderMode.Desc) };
        //                List<Article> articles = Assistant.List<Article>(null, orders, 0,0);
        //                if (articles != null && articles.Count > 0)
        //                    maxSn = articles[0].SN;
        //                else
        //                    maxSn = 0;
        //            }
        //            result = maxSn + 1;
        //            //}
        //            //else
        //            //{
        //            //    result = snBase + 1;
        //            //}
        //            snBase = result;
        //            return result;
        //        }
        //    }
        //}

        public long SnBase
        {
            get
            {
                lock (lockHelper)
                {
                    Criteria c = new Criteria(CriteriaType.Equals, "SN", ++AppSN);
                    long totalAll = Assistant.Count<Article>(c);
                    if (totalAll > 0)
                    {
                        List<Article> articles = Assistant.List<Article>(null, new Order[] { new Order("SN", OrderMode.Desc) }, 0, 1);
                        AppSN = articles[0].SN + 1;
                    }
                    return AppSN;
                }
            }
        }

        private ObjectAssistant assistant;
        /// <summary>
        /// 当前Helper的数据访问对象
        /// </summary>
        public ObjectAssistant Assistant
        {
            get { return assistant; }
            set { assistant = value; }
        }

        private static readonly string ArticleKeyID = "ArticleID{0}";

        /// <summary>
        /// 更新一篇文章记录
        /// </summary>
        /// <param name="a">一篇文章记录</param>
        /// <param name="fields">需要更新的字段</param>
        void UpdateArticle(Article a, string[] fields)
        {
            try
            {
                //清除缓存
                HttpContext Context = HttpContext.Current;
                string key = string.Format(ArticleKeyID, a.ID);
                Context.Cache.Remove(key);
                Context.Items.Remove(key);
            }
            catch (Exception)
            {
            }

            Assistant.Update(a, fields);
        }

    }

    public class CriteriaExpressionHelper
    {
        static List<CriteriaExpression> expList = new List<CriteriaExpression>();
        static CriteriaExpressionHelper()
        {
            expList.Add(new LikeExpression());
        }

        public static void Execute(Criteria c, string expr)
        {
            foreach (CriteriaExpression exp in expList)
            {
                exp.Execute(c, expr);
            }
        }
    }

    public interface CriteriaExpression
    {
        void Execute(Criteria c, string expr);
    }

    public class LikeExpression : CriteriaExpression
    {
        Regex regex = new Regex(@"like\((?<field>\S*),(?<value>\S*)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public void Execute(Criteria c, string expr)
        {
            StringReader reader = new StringReader(expr);
            string s = null;
            while (!String.IsNullOrEmpty(s = reader.ReadLine()))
            {
                s = s.Trim();
                Match m = regex.Match(s);
                if (m != null && m.Success)
                {
                    c.Add(CriteriaType.Like, m.Groups["field"].Value, m.Groups["value"].Value);
                }
            }
        }
    }
}