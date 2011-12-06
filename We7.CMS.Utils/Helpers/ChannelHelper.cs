using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml;
using System.Text.RegularExpressions;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;

using Thinkment.Data;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using System.Data;

namespace We7.CMS
{
    /// <summary>
    /// 栏目处理助手类
    /// </summary>
    [Serializable]
    [Helper("We7.ChannelHelper")]
    public class ChannelHelper : BaseHelper
    {
        /// <summary>
        /// 获取首页栏目信息
        /// </summary>
        /// <returns>栏目信息</returns>
        public Channel GetFirstChannel()
        {
            return GetFirstChannel(We7Helper.EmptyGUID);
        }

        /// <summary>
        /// 获取某个父栏目下的第一个子栏目（索引最大的一个子栏目）
        /// </summary>
        /// <param name="ParentID">父栏目ID</param>
        /// <returns>栏目信息</returns>
        public Channel GetFirstChannel(string ParentID)
        {
            Order[] ods = new Order[] { new Order("Index") };
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", ParentID);
            List<Channel> channels = Assistant.List<Channel>(c, ods, 0, 1);
            if (channels.Count > 0)
            {
                return channels[0];
            }
            return null;
        }

        /// <summary>
        /// 获取所有栏目（按栏目Index升序排序）
        /// </summary>
        /// <returns>一组栏目信息</returns>
        public Channel[] GetAllChannels()
        {
            Order[] o = new Order[] { new Order("Index") };
            return Assistant.List<Channel>(null, o).ToArray();
        }

        public List<Channel> GetAllLinkChannels()
        {
            Order[] o = new Order[] { new Order("Index") };
            Criteria c = new Criteria(CriteriaType.Equals, "Type", (int)TypeOfChannel.QuoteChannel);
            return Assistant.List<Channel>(c, o);
        }

        /// <summary>
        /// 通过别名来获取一个栏目
        /// </summary>
        /// <param name="alias">栏目别名</param>
        /// <returns>栏目信息</returns>
        public Channel GetChannelByAlias(string alias)
        {
            Order[] o = new Order[] { new Order("ID") };
            Criteria c = new Criteria(CriteriaType.Equals, "Alias", alias);
            List<Channel> channels = Assistant.List<Channel>(c, o);
            if (channels.Count > 0)
            {
                return channels[0];
            }
            else
                return null;
        }

        /// <summary>
        /// 通过别名来获取一个栏目
        /// </summary>
        /// <param name="alias">栏目别名</param>
        /// <returns>栏目信息</returns>
        public Channel GetChannelById(string id)
        {
            Order[] o = new Order[] { new Order("ID") };
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Channel> channels = Assistant.List<Channel>(c, o);
            if (channels.Count > 0)
            {
                return channels[0];
            }
            else
                return null;
        }


        /// <summary>
        /// 获取WAP根栏目列表
        /// </summary>
        /// <returns>栏目列表</returns>
        public List<Channel> GetWapRootChannels()
        {
            Order[] o = new Order[] { new Order("ID") };
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", We7Helper.EmptyWapGUID);
            return Assistant.List<Channel>(c, o);
        }

        private static readonly string ChannelKeyID = "ChannelID{0}";

        /// <summary>
        /// 通过栏目ID获取此栏目信息（存到缓存里面去了）
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        /// <param name="fields">返回字段组合</param>
        /// <returns>栏目记录</returns>
        public Channel GetChannel(string channelID, string[] fields)
        {
            if (channelID != null && channelID != string.Empty)
            {
                Channel channel=null;
                HttpContext Context = HttpContext.Current;
                if (Context != null)
                {
                    string key = string.Format(ChannelKeyID, channelID);
                    channel = (Channel)Context.Items[key];//内存
                    if (channel == null)
                    {
                        channel = (Channel)Context.Cache[key];//缓存
                        if (channel == null)
                        {
                            if (channelID != null && channelID != string.Empty)
                            {
                                //读取数据库
                                Order[] o = new Order[] { new Order("ID") };
                                Criteria c = new Criteria(CriteriaType.Equals, "ID", channelID);
                                List<Channel> channels = Assistant.List<Channel>(c, o);
                                if (channels.Count > 0)
                                {
                                    channel = channels[0];
                                }
                            }

                            if (channel != null)
                            {
                                CacherCache(key, Context, channel, CacheTime.Short);
                            }
                        }

                        if (Context.Items[key] == null)
                        {
                            Context.Items.Remove(key);
                            Context.Items.Add(key, channel);
                        }
                    }
                }
                else
                {
                    if (channelID != null && channelID != string.Empty)
                    {
                        //读取数据库
                        Order[] o = new Order[] { new Order("ID") };
                        Criteria c = new Criteria(CriteriaType.Equals, "ID", channelID);
                        List<Channel> channels = Assistant.List<Channel>(c, o);
                        if (channels.Count > 0)
                        {
                            channel = channels[0];
                        }
                    }

                }
                return channel;
            }
            else
                return null;


        }

        /// <summary>
        /// 通过Url获取channel
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Channel GetChannel(string url)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "FullUrl", url);
            List<Channel> channels = Assistant.List<Channel>(c, null);
            if (channels.Count > 0)
            {
                return channels[0];
            }
            else
                return null;
        }

        /// <summary>
        /// 报得指定栏目下的子栏目
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public List<Channel> GetChildren(string oid)
        {
            Channel ch = GetChannel(oid, null);
            if (ch != null)
            {
                Criteria c = new Criteria(CriteriaType.Like,"FullUrl",ch.FullUrl+"%");
                c.Add(CriteriaType.Equals, "State", 1);
                return Assistant.List<Channel>(c, new Order[]{new Order("ID") });                
            }
            return null;
        }

        /// <summary>
        /// 取得指定栏目下的所有子栏目
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="includeDisableChannel"></param>
        /// <returns></returns>
        public List<Channel> GetChildren(string oid, bool includeDisableChannel)
        {
            Channel ch = GetChannel(oid, null);
            if (ch != null)
            {
                Criteria c = new Criteria(CriteriaType.Like, "FullUrl", ch.FullUrl + "%");
                if (!includeDisableChannel)
                {
                    c.Add(CriteriaType.Equals, "State", 1);
                }
                return Assistant.List<Channel>(c, new Order[] { new Order("ID") });
            }
            return null;
        }

        /// <summary>
        /// 更新一个栏目
        /// </summary>
        /// <param name="ch">栏目记录</param>
        public void UpdateChannel(Channel ch)
        {
            ///更新缓存
            HttpContext Context = HttpContext.Current;
            string key = string.Format(ChannelKeyID, ch.ID);
            Context.Cache.Remove(key);

            string[] fields = new string[] {
                "Name", "Description", "State", "TemplateName",
                "ReferenceID", "Index", "Alias", "Parameter","ProcessEnd",
                "FullPath", "DetailTemplate","ChannelFolder","FullUrl","SecurityLevel",
                "TitleImage","Process","ProcessLayerNO","Type","ChannelName","RefAreaID","ParentID","IsComment","ReturnUrl", "EnumState","KeyWord","DescriptionKey","ModelName"};
            ch.FullPath = GetChannelFullPath(ch.ParentID) + "/" + ch.Name;
            ch.FullUrl = GetChannelFullUrl(ch.ParentID) + "/" + ch.ChannelName;

            //设置当前节点层数
            string tmpStr1 = ch.FullUrl.Substring(0, ch.FullUrl.Length - 1);
            int chNum = 1;
            if (tmpStr1.Length != 0)
            {
                string tmpStr2 = tmpStr1.Replace("/", "");
                chNum = tmpStr1.Length - tmpStr2.Length;
            }
            ch.EnumState = StateMgr.StateProcess(ch.EnumState, EnumLibrary.Business.ChannelNodeLevel, chNum);
            Assistant.Update(ch, fields);
        }

        /// <summary>
        /// 更新栏目路径字段：FullPath，同时更新Article表中对应的冗余字段FullChannelPath
        /// </summary>
        /// <param name="ch">更新后的channel对象</param>
        /// <param name="oldChannelPath">旧的FullPath</param>
        public void UpdateChannelPathBatch(Channel ch, string oldChannelPath)
        {
            //批量更新下属子栏目
            Criteria c = new Criteria(CriteriaType.Like, "FullUrl", oldChannelPath+"%");
             List<Channel> channels = Assistant.List<Channel>(c, new Order[] { new Order("Updated", OrderMode.Desc) });
            foreach (Channel mychannel in channels)
            {
                try
                {
                    Regex reg = new Regex("^" + oldChannelPath, RegexOptions.IgnoreCase);
                    mychannel.FullPath = reg.Replace(mychannel.FullPath, ch.FullPath);
                    HelperFactory.Instance.GetHelper<ChannelHelper>().UpdateChannel(mychannel, new string[] { "FullPath" });
                }
                catch { }
            }

             //批量更新下属文章信息
            c = new Criteria(CriteriaType.Like, "ChannelFullUrl", ch.FullUrl+"%");
            List<Article> articles = Assistant.List<Article>(c, new Order[] { new Order("Updated", OrderMode.Desc) });
            foreach (Article a in articles)
            {
                try
                {
                    Regex reg = new Regex("^" + oldChannelPath, RegexOptions.IgnoreCase);
                    a.FullChannelPath = reg.Replace(a.FullChannelPath, ch.FullPath);
                    a.ChannelName=ch.Name;
                    HelperFactory.Instance.GetHelper<ArticleHelper>().UpdateArticle(a, new string[] { "FullChannelPath","ChannelName" });
                }
                catch { }
            }
         }

        /// <summary>
        /// 批量更新栏目Url地址
        /// </summary>
        /// <param name="oldUrl"></param>
        /// <param name="newUrl"></param>
        public void UpdateChannelUrlBatch2(string oldUrl, string newUrl)
        {
            Criteria c=new Criteria(CriteriaType.None);
            c.Add(CriteriaType.Like, "ChannelFullUrl",  oldUrl+"%" );
            List<Article> articles=Assistant.List<Article>(c,new Order[]{new Order("Updated",OrderMode.Desc)});
            foreach (Article a in articles)
            {
                try
                {
                    Regex reg = new Regex("^" + oldUrl, RegexOptions.IgnoreCase);
                    a.ChannelFullUrl = reg.Replace(a.ChannelFullUrl, newUrl);
                    HelperFactory.Instance.GetHelper<ArticleHelper>().UpdateArticle(a, new string[] { "ChannelFullUrl" });
                }
                catch { }
            }

            c = new Criteria(CriteriaType.Like, "FullUrl",  oldUrl+"%" );

            List<Channel> channels = Assistant.List<Channel>(c, new Order[] { new Order("Updated", OrderMode.Desc) });
            foreach (Channel ch in channels)
            {
                try
                {
                    Regex reg = new Regex("^" + oldUrl, RegexOptions.IgnoreCase);
                    ch.FullUrl = reg.Replace(ch.FullUrl, newUrl);
                    HelperFactory.Instance.GetHelper<ChannelHelper>().UpdateChannel(ch, new string[] { "FullUrl" });
                }
                catch { }
            }
        }

        /// <summary>
        /// 批量更新栏目Url数据：Channel、Article表
        /// </summary>
        /// <param name="oldUrl"></param>
        /// <param name="newUrl"></param>
        public void UpdateChannelUrlBatch(string oldUrl, string newUrl)
        {
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            SqlStatement sql = new SqlStatement();

            string sqlCommandTxt =
       @"update {1} set {2}=replace({2},{0}OLDURL,{0}NEWURL) 
            where {2} like {0}KEY";

            DataParameter dp = new DataParameter();
            dp.ParameterName = db.DbDriver.Prefix + "OLDURL";
            dp.DbType = DbType.String;
            dp.Value = oldUrl;
            dp.Size = 255;
            sql.Parameters.Add(dp);

            DataParameter dp2 = new DataParameter();
            dp2.ParameterName = db.DbDriver.Prefix + "NEWURL";
            dp2.Value = newUrl;
            dp2.DbType = DbType.String;
            dp2.Size = 255;
            sql.Parameters.Add(dp2);

            DataParameter dp3 = new DataParameter();
            dp3.ParameterName = db.DbDriver.Prefix + "KEY";
            dp3.Value = oldUrl + "%";
            dp3.DbType = DbType.String;
            dp3.Size = 255;
            sql.Parameters.Add(dp3);


            using (IConnection conn = db.CreateConnection())
            {
                sql.SqlClause = string.Format(sqlCommandTxt, db.DbDriver.Prefix, "[Article]", "[ChannelFullUrl]");
                db.DbDriver.FormatSQL(sql);
                conn.Update(sql);

                sql.SqlClause = string.Format(sqlCommandTxt, db.DbDriver.Prefix, "[Channel]", "[FullUrl]");
                db.DbDriver.FormatSQL(sql);
                conn.Update(sql);
            }

        }


        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="typeID">权限对象的类型</param>
        /// <param name="ownerID">权限对象的ID号</param>
        /// <param name="parentID">上级栏目</param>
        public void DeleteChildrenPermission(int typeID, string ownerID, string parentID)
        {
            List<Channel> chs = GetChildren(parentID,true);
            if (chs != null)
            {
                foreach (Channel ch in chs)
                {
                    List<Permission> permissions = AccountFactory.CreateInstance().GetPermissions(typeID, ownerID, ch.ID);
                    if (permissions != null && permissions.Count > 0)
                    {
                        AccountFactory.CreateInstance().DeletePermission(typeID, ownerID, ch.ID);
                    }
                }
            }
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="typeID">权限对象的类型</param>
        /// <param name="ownerID">权限对象的ID号</param>
        /// <param name="parentID">上级栏目</param>
        /// <param name="contents">栏目内容</param>
        public void DeleteChildrenPermission(int typeID, string ownerID, string parentID,string[] contents)
        {
            List<Channel> chs = GetChildren(parentID,true);
            if (chs != null)
            {
                foreach (Channel ch in chs)
                {
                    List<Permission> permissions = AccountFactory.CreateInstance().GetPermissions(typeID, ownerID, ch.ID);
                    if (permissions != null && permissions.Count > 0)
                    {
                        AccountFactory.CreateInstance().DeletePermission(typeID, ownerID, ch.ID, contents);
                    }
                }
            }
        }

        /// <summary>
        /// 为子栏目添加权限
        /// </summary>
        /// <param name="typeID">权限对象类型</param>
        /// <param name="ownerID">权限拥有者的ID</param>
        /// <param name="parentID">上级栏目</param>
        /// <param name="contents">权限内容</param>
        public void AddChildrenPermission(int typeID, string ownerID, string parentID, string[] contents)
        {
            List<Channel> chs = GetChildren(parentID,true);
            if (chs != null)
            {
                foreach (Channel ch in chs)
                {
                    AccountFactory.CreateInstance().DeletePermission(typeID, ownerID, ch.ID);
                    AccountFactory.CreateInstance().AddPermission(typeID, ownerID, ch.ID, contents);
                }
            }
        }

        /// <summary>
        /// 取得模型名称
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public string GetModelName(string oid, out string Parameter)
        {
            Channel ch = GetChannel(oid, null);
            if (ch != null)
            {
                //if (String.IsNullOrEmpty(ch.ModelName))
                //{
                //    UpdateModelType(ch);
                //}
                Parameter = ch.Parameter;
                return ch.ModelName;
            }
            else
            {
                Parameter = String.Empty;
                return String.Empty;
            }
        }


        /// <summary>
        /// 更新一个栏目记录
        /// </summary>
        /// <param name="ch">栏目记录</param>
        /// <param name="fields">要更新的字段</param>
        public void UpdateChannel(Channel ch, string[] fields)
        {
            ///更新缓存
            HttpContext Context = HttpContext.Current;
            string key = string.Format(ChannelKeyID, ch.ID);
            Context.Cache.Remove(key);

            ch.FullPath = GetChannelFullPath(ch.ParentID) + "/" + ch.Name;
            ch.FullUrl = GetChannelFullUrl(ch.ParentID) + "/" + ch.ChannelName;

            //设置当前节点层数
            string tmpStr1 = ch.FullUrl.Substring(0, ch.FullUrl.Length - 1);
            int chNum = 1;
            if (tmpStr1.Length != 0)
            {
                string tmpStr2 = tmpStr1.Replace("/", "");
                chNum = tmpStr1.Length - tmpStr2.Length;
            }
            ch.EnumState = StateMgr.StateProcess(ch.EnumState, EnumLibrary.Business.ChannelNodeLevel, chNum);

            Assistant.Update(ch, fields);
        }

        /// <summary>
        /// 更新一个栏目的父栏目ID
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <param name="parent">父栏目ID</param>
        public void UpdateChannelParent(string id, string parent)
        {
            Channel ch = new Channel();
            ch.ID = id;
            ch.ParentID = parent;
            Assistant.Update(ch, new string[] { "ParentID" });
        }

        /// <summary>
        /// 判断一个栏目是否是一个栏目的子栏目
        /// </summary>
        /// <param name="parent">父栏目ID</param>
        /// <param name="child">子栏目ID</param>
        /// <returns></returns>
        public bool IsChild(string parent, string child)
        {
            Channel ch = new Channel();
            ch.ID = child;
            Criteria c = new Criteria(CriteriaType.Equals, "ID", child);
            string[] fields = new string[] { "ParentID", "ID" };
            while (Assistant.Count<Channel>(c) > 0)
            {
                Assistant.Select(ch, fields);
                if (ch.ParentID == parent)
                {
                    return true;
                }
                c.Value = ch.ParentID;
                ch.ID = ch.ParentID;
            }
            return false;
        }

        /// <summary>
        /// 判断一个栏目是否有子栏目
        /// </summary>
        /// <param name="id">父栏目ID</param>
        /// <returns></returns>
        public bool HasChild(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", id);
            return (Assistant.Count<Channel>(c) > 0) ? true : false;
        }

        /// <summary>
        /// 添加一个栏目
        /// </summary>
        /// <param name="ch">栏目信息</param>
        /// <returns></returns>
        public Channel AddChanel(Channel ch)
        {
            string emptyID = "{00000000-";

            if (ch.ID == null || ch.ID.Length < 1)
            {
                ch.ID = We7Helper.CreateNewID();
            }
            ch.Created = DateTime.Now;
            ch.FullPath = GetChannelFullPath(ch.ParentID) + "/" + ch.Name;
            ch.FullUrl = GetChannelFullUrl(ch.ParentID) + "/" + ch.ChannelName;

            //设置当前节点层数
            string tmpStr1 = ch.FullUrl.Substring(0, ch.FullUrl.Length - 1);
            int chNum = 1;
            if (tmpStr1.Length != 0)
            {
                string tmpStr2 = tmpStr1.Replace("/", "");
                chNum = tmpStr1.Length - tmpStr2.Length;
            }
            ch.EnumState = StateMgr.StateProcess(ch.EnumState, EnumLibrary.Business.ChannelNodeLevel, chNum);

            Assistant.Insert(ch);
            return ch;
        }


        /// <summary>
        /// wjz 添加一个栏目
        /// </summary>
        /// <param name="ch">栏目信息 ADD TIME 2010-08-03</param>
        /// <returns></returns>
        public Channel AddChanelByDBImport(Channel ch)
        {
            string emptyID = "{00000000-";

            if (ch.ID == null || ch.ID.Length < 1)
            {
                ch.ID = We7Helper.CreateNewID();
            }
            ch.Created = DateTime.Now;
            ch.FullPath = GetChannelFullPath(ch.ParentID) + "/" + ch.Name;
            //ch.FullUrl = GetChannelFullUrl(ch.ParentID) + "/" + ch.ChannelName;
            //设置当前节点层数
            string tmpStr1 = ch.FullUrl.Substring(0, ch.FullUrl.Length - 1);
            int chNum = 1;
            if (tmpStr1.Length != 0)
            {
                string tmpStr2 = tmpStr1.Replace("/", "");
                chNum = tmpStr1.Length - tmpStr2.Length;
            }
            ch.EnumState = StateMgr.StateProcess(ch.EnumState, EnumLibrary.Business.ChannelNodeLevel, chNum);

            Assistant.Insert(ch);
            return ch;
        }

        /// <summary>
        /// 通过栏目ID删除栏目，同时删除子栏目和相关的文章
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        public void DeleteChannel(string channelID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", channelID);
            List<Channel> channels = Assistant.List<Channel>(c, null, 0, 0, new string[] { "ID", "ChannelName" });
            foreach (Channel channel in channels)
            {
                DeleteChannel(channel.ID);
            }

            Channel ch = new Channel();
            ch.ID = channelID;
            Assistant.Delete(ch);

            //同时删除文章
            Criteria ca = new Criteria(CriteriaType.Equals, "OwnerID", channelID);
            List<Article> artcles = Assistant.List<Article>(ca, null);
            foreach (Article a in artcles)
            {
                Assistant.Delete(a);
                //删除文章评论
                Criteria articleCommentsCriteria = new Criteria(CriteriaType.Equals, "ArticleID", a.ID);
                List<Comments> articleComments = Assistant.List<Comments>(articleCommentsCriteria, null);
                foreach (Comments articleComment in articleComments)
                {
                    Assistant.Delete(articleComment);
                }
            }
            //同时删除栏目评论
            Criteria channelCommentsCriteria = new Criteria(CriteriaType.Equals, "ArticleID", ch.ChannelName);
            List<Comments> channelComments = Assistant.List<Comments>(channelCommentsCriteria, null);
            foreach (Comments channelComment in channelComments)
            {
                Assistant.Delete(channelComment);
            }
        }

        /// <summary>
        ///查找一个栏目的所有子栏目
        /// </summary>
        /// <param name="parentID">父栏目ID</param>
        /// <returns></returns>
        public List<Channel> GetChannels(string parentID)
        {
            return GetChannels(parentID, false);
        }

        /// <summary>
        ///  查找一个栏目的下的启用的子栏目集合
        /// </summary>
        /// <param name="parentID">一个栏目ID</param>
        /// <param name="OnlyInUser">是否只查找启用的栏目</param>
        /// <returns></returns>
        public List<Channel> GetChannels(string parentID, bool OnlyInUser)
        {
            Criteria condition = new Criteria(CriteriaType.Equals, "ParentID", parentID);
            if (OnlyInUser)
                condition.Add(CriteriaType.Equals, "State", 1);

            Order[] ods = new Order[] { new Order("Index") };

            List<Channel> list = null;
            if (Assistant.Count<Channel>(condition) > 0)
            {
                list = Assistant.List<Channel>(condition, ods);
            }
            else
            {
                list = null;
            }
            return list;
        }

        /// <summary>
        /// 取得url类似 /news/*的栏目列表的前几位
        /// </summary>
        /// <param name="urlPatern"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Channel> GetChannels(string urlPatern, int top)
        {
            //此处缺少缓存处理
            Criteria condition = new Criteria(CriteriaType.Like, "FullUrl", urlPatern);
            Order[] ods = new Order[] { new Order("Index") };
            return Assistant.List<Channel>(condition, ods, 0, top);
        }

        /// <summary>
        /// 根据标签列表获取栏目列表
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public List<Channel> GetChannelsByTags(string[] tags)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (tags.Length > 0)
            {
                c.Mode = CriteriaMode.Or;
                foreach (string tag in tags)
                {
                    c.AddOr(CriteriaType.Like, "Tags", "%'" + tag + "'%");
                }
                Order[] o = new Order[] { new Order("Index") };
                return Assistant.List<Channel>(c, o);
            }
            else
                return null;
        }

        /// <summary>
        /// 通过栏目ID获取这个栏目的标签
        ///
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        /// <returns></returns>
        public List<string> GetTags(string channelID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", channelID);

            List<Channel> articles = Assistant.List<Channel>(c, null);
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
        /// 获取所有栏目标签记录
        ///  2011-11-9 标签已经整合，未发现此方法的引用
        /// </summary>
        /// <returns></returns>
        //public List<ChannelTag> GetAllTags()
        //{
        //    return Assistant.List<ChannelTag>(null, null);
        //}

      
 
 
        /// <summary>
        /// 获取一个栏目的FullPath
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns>栏目的FullPath</returns>
        string GetChannelFullPath(string id)
        {
            if (We7Helper.IsEmptyID(id))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            string pid = id;
            do
            {
                Channel c = new Channel();
                Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", pid);
                List<Channel> cList = Assistant.List<Channel>(cTmp, null, 0, 0, new string[] { "ID", "Name", "ParentID" });
                if (cList != null && cList.Count > 0)
                {
                    c = cList[0];
                    sb.Insert(0, c.Name);
                    sb.Insert(0, "/");
                    pid = c.ParentID;
                }
                else
                {
                    pid = string.Empty;
                }
            }
            while (!We7Helper.IsEmptyID(pid));
            return sb.ToString();
        }

        /// <summary>
        /// 获取一个栏目的FullUrl
        /// </summary>
        /// <param name="id">栏目ID</param>
        /// <returns></returns>
        string GetChannelFullUrl(string id)
        {
            if (We7Helper.IsEmptyID(id))
            {
                return "/";
            }
            StringBuilder sb = new StringBuilder();
            string pid = id;
            do
            {
                Channel c = new Channel();
                Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", pid);
                List<Channel> cList = Assistant.List<Channel>(cTmp, null, 0, 0, new string[] { "ID", "ChannelName", "ParentID" });
                if (cList != null && cList.Count > 0)
                {
                    c = cList[0];
                    sb.Insert(0, c.ChannelName);
                    sb.Insert(0, "/");
                    pid = c.ParentID;
                }
                else
                {
                    pid = string.Empty;
                }
            }
            while (!We7Helper.IsEmptyID(pid));
            return sb.ToString();
        }

        /// <summary>
        /// 获取一个栏目的名称
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        /// <returns>栏目名称</returns>
        public string GetChannelName(string channelID)
        {
            Channel ch = GetChannel(channelID, new string[] { "ID", "FullPath", "Name" });
            if (ch != null)
                return ch.Name;
            else
                return "";
        }

        /// <summary>
        /// 获取一个栏目的存放的物理路径
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        /// <returns></returns>
        public string GetChannelFilePath(string channelID)
        {
            Channel ch = GetChannel(channelID, new string[] { "ID", "ChannelFolder", "Name" });
            if (ch != null && ch.ChannelFolder != null && ch.ChannelFolder.Length > 0)
            {
                if (ch.ChannelFolder.IndexOf("/") > -1 || ch.ChannelFolder.IndexOf("\\") > -1)
                //兼容旧版本中存放 /_data/channels/sasa/这样路径的情况
                {
                    ch.ChannelFolder = ch.ChannelFolder.Replace("\\", "/");
                    ch.ChannelFolder = ch.ChannelFolder.Replace("//", "/");
                    if (ch.ChannelFolder.EndsWith("/")) ch.ChannelFolder = ch.ChannelFolder.Remove(ch.ChannelFolder.Length - 1);
                    ch.ChannelFolder = ch.ChannelFolder.Substring(ch.ChannelFolder.LastIndexOf("/") + 1);
                }
                string path = "/" + Constants.ChannelPath.Replace("\\", "/") + "/" + ch.ChannelFolder;
                HttpContext Context = HttpContext.Current;
                if (!Directory.Exists(Context.Server.MapPath(path)))
                    Directory.CreateDirectory(Context.Server.MapPath(path));
                return path;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 不同账号建立不同文件夹
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GetChannelFilePath(string channelID, string username)
        {
            string chpath = GetChannelFilePath(channelID);
            if (We7Helper.IsEmptyID(username))
                return chpath;
            else
            {
                if (!chpath.EndsWith("/")) chpath = chpath + "/";
                chpath = chpath + username;
                return chpath;
            }
        }

        /// <summary>
        /// 直接获取数据库相应字段FullUrl
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        /// <returns></returns>
        public string GetFullUrl(string channelID)
        {
            Channel ch = GetChannel(channelID, new string[] { "ID", "FullUrl", "Name" });
            if (ch != null)
            {
                return ch.FullUrl;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 直接获取数据库相应字段FullPath
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        /// <returns></returns>
        public string GetFullPath(string channelID)
        {
            Channel ch = GetChannel(channelID, new string[] { "ID", "FullPath", "Name" });
            if (ch != null)
            {
                return ch.FullPath;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取所有栏目的名称集合
        /// </summary>
        /// <returns></returns>
        public ArrayList GetAllChannelNames()
        {
            Order[] o = new Order[] { new Order("Index") };
            Channel[] cs = Assistant.List<Channel>(null, o).ToArray();
            ArrayList channelNames = new ArrayList();

            foreach (Channel c in cs)
            {
                channelNames.Add(c.Name);
            }
            return channelNames;
        }

        /// <summary>
        /// 通过栏目名称获取栏目ID
        /// </summary>
        /// <param name="OnlyName">栏目名称</param>
        /// <returns></returns>
        public string GetChannelIDByOnlyName(string OnlyName)
        {
            Order[] ods = new Order[] { new Order("Index") };
            Criteria c = new Criteria(CriteriaType.Equals, "ChannelName", OnlyName);
            List<Channel> channels = Assistant.List<Channel>(c, ods, 0, 1);
            if (channels.Count > 0)
            {
                return channels[0].ID;
            }
            else
                return We7Helper.NotFoundID;
        }

        /// <summary>
        /// 通过url获取栏目ID
        /// </summary>
        /// <param name="fullurl"></param>
        /// <returns></returns>
        public string GetChannelIDByFullUrl(string fullurl)
        {
            Order[] ods = new Order[] { new Order("Index") };
            Criteria c = new Criteria(CriteriaType.Equals, "FullUrl", fullurl);
            List<Channel> channels = Assistant.List<Channel>(c, ods, 0, 1);
            if (channels.Count > 0)
            {
                return channels[0].ID;
            }
            else
                return We7Helper.NotFoundID;
        }

        /// <summary>
        /// 获取一个栏目的子栏目
        /// </summary>
        /// <param name="parentID">栏目ID</param>
        /// <param name="recusive">true代表所有子栏目，false代表只为这个栏目的子栏目</param>
        /// <returns></returns>
        public List<Channel> GetSubChannelList(string parentID, bool recusive)
        {
            return GetSubChannelList(parentID, recusive, false);
        }
        #region 获取专题图片
        /// <summary>
        /// 通过标签和栏目类型取得专题栏目
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        //public List<Channel> QueryTopPhotos(string tag, int count)
        //{
        //    Criteria c = new Criteria(CriteriaType.Equals, "Type", 3);
        //    Order[] orders = new Order[] { new Order("Created", OrderMode.Desc) };
        //    List<Channel> channels = Assistant.List<Channel>(c, orders);
        //    if (tag != null && tag.Trim().Length > 0)
        //    {
        //        List<Channel> list = new List<Channel>();
        //        foreach (Channel a in channels)
        //        {
        //            List<ChannelTag> tags = GetTags(a.ID);
        //            List<String> ts = new List<string>();
        //            foreach (ChannelTag t in tags)
        //            {
        //                ts.Add(t.Identifier);
        //            }
        //            if (!ts.Contains(tag))
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                if (list.Count < count)
        //                    list.Add(a);
        //                else
        //                    break;
        //            }
        //        }
        //        channels = list;
        //    }
        //    return channels;

        //}

        #endregion

        /// <summary>
        /// 获取一个栏目的子栏目
        /// </summary>
        /// <param name="parentID">栏目ID</param>
        /// <param name="recusive">是否只包括这个栏目的第一级子栏目</param>
        /// <param name="OnlyInUser">是否只包含启用的栏目</param>
        /// <returns></returns>
        public List<Channel> GetSubChannelList(string parentID, bool recusive, bool OnlyInUser)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", parentID);
            if (OnlyInUser)
                c.Add(CriteriaType.Equals, "State", 1);

            Order od = new Order("Index", OrderMode.Asc);
            List<Channel> chs = Assistant.List<Channel>(c, new Order[] { od });
            List<Channel> allchs = Assistant.List<Channel>(c, new Order[] { od });
            if (recusive)
            {
                foreach (Channel ch in chs)
                {
                    List<Channel> subs = GetSubChannelList(ch.ID, recusive);
                    foreach (Channel sub in subs)
                    {
                        ch.Channels.Add(sub);
                        allchs.Add(sub);
                    }
                }
            }
            return allchs;
        }

        /// <summary>
        /// 获取所有引用型栏目
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Channel[] QueryAllChannel(int begin, int end)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Type", "1");
            Order[] ods = new Order[] { new Order("ID") };
            List<Channel> items = Assistant.List<Channel>(c, ods, begin, end);
            if (Assistant.List<Channel>(c, ods, begin, end).Count > 0)
            {
                return items.ToArray();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取引用型栏目数
        /// </summary>
        /// <returns></returns>
        public int QueryAllCounts()
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Type", "1");
            List<Channel> items = Assistant.List<Channel>(c, null);
            if (items.Count > 0)
            {
                return items.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 判断是否为联播型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ChannelType(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            c.Add(CriteriaType.Equals, "Type", "2");
            if (Assistant.List<Channel>(c, null).Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 通过栏目ID查找栏目的别名
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public string GetChannelAlias(string channelID)
        {
            Channel ch = GetChannel(channelID, new string[] { "ID", "Alias" });
            if (ch != null)
                return ch.Alias;
            else
                return "";
        }

        /// <summary>
        /// 格式化栏目的FullPath
        /// </summary>
        /// <param name="channelFullPath">栏目的FullPath</param>
        /// <param name="dotChar"></param>
        /// <returns></returns>
        public string FullPathFormat(string channelFullPath, string dotChar)
        {
            channelFullPath = channelFullPath.Replace("//", "/");
            if (channelFullPath.StartsWith("/"))
                channelFullPath = channelFullPath.Remove(0, 1);
            channelFullPath = channelFullPath.Replace("/", dotChar);
            return channelFullPath;
        }

        /// <summary>
        /// 更新一个栏目
        /// </summary>
        /// <param name="ch">栏目对象</param>
        /// <param name="fields">需要更新的字段</param>
        public void UpdateChannelTitle(Channel ch, string[] fields)
        {
            Assistant.Update(ch, fields);
        }

        /// <summary>
        /// 根据EnumState获取栏目类型
        /// </summary>
        /// <param name="channelEnumState">栏目类型</param>
        /// <returns></returns>
        public List<Channel> GetChannelsByType(string channelEnumState)
        {
            HttpContext Context = HttpContext.Current;
            string key = string.Format(ChannelKeyID, channelEnumState);
            List<Channel> ChannelList = (List<Channel>)Context.Items[key];//内存
            if (ChannelList == null)
            {
                ChannelList = (List<Channel>)Context.Cache[key];//缓存
                if (ChannelList == null)
                {
                    Criteria c = null;
                    if (!string.IsNullOrEmpty(channelEnumState))
                    {
                        c = new Criteria(CriteriaType.Equals, "EnumState", channelEnumState);
                        c.Adorn = Adorns.Substring;
                        c.Start = EnumLibrary.Position[(int)EnumLibrary.Business.ChannelContentType];
                        c.Length = EnumLibrary.PlaceLenth;
                    }
                    Order[] ods = new Order[] { new Order("Index") };
                    ChannelList = Assistant.List<Channel>(c, ods);
                }

                if (Context.Items[key] == null)
                {
                    Context.Items.Remove(key);
                    Context.Items.Add(key, ChannelList);
                }
            }
            return ChannelList;
        }

        /// <summary>
        /// 按模型名称取得栏目列表
        /// </summary>
        /// <param name="modelname">模型名称</param>
        /// <returns></returns>
        public List<Channel> GetChannelByModelName(string modelname)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ModelName", modelname);
            //c.Add(CriteriaType.Equals,"State",1);

            return Assistant.List<Channel>(c, new Order[] { new Order("Created", OrderMode.Desc) });
        }

        /// <summary>
        /// 按模型名称取得的栏目的第一个值
        /// </summary>
        /// <param name="modelname"></param>
        /// <returns></returns>
        public Channel FirestByModelName(string modelname)
        {
            List<Channel> chs = GetChannelByModelName(modelname);
            return chs.Count > 0 ? chs[0] : null;
        }

        /// <summary>
        /// 根据Title查询ID
        /// </summary>
        /// <param name="channelTitle">栏目名称</param>
        /// <returns></returns>
        public string GetChannelIDByTitle(string channelTitle)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Name", channelTitle);
            List<Channel> ch = Assistant.List<Channel>(c, null);
            if (ch.Count > 0)
            {
                return ch[0].ID;
            }
            return "";
        }

        /// <summary>
        /// 获取一个标签的栏目集合
        ///  2011-11-9 标签已经整合，未发现此方法的引用
        /// </summary>
        /// <param name="Tag">标签</param>
        /// <returns></returns>
        //public List<Channel> QueryChannelByTag(string Tag)
        //{
        //    List<Channel> list = Assistant.List<Channel>(null, null, 0, 0, new string[] { "ID", "Name" });
        //    if (Tag == null)
        //    {
        //        return list;
        //    }
        //    else
        //    {
        //        List<Channel> returnlist = new List<Channel>();
        //        foreach (Channel c in list)
        //        {
        //            List<ChannelTag> tags = GetChannelTags(c.ID);
        //            List<String> ts = new List<string>();
        //            foreach (ChannelTag ct in tags)
        //            {
        //                //ts.Add(ct.Identifier);
        //            }
        //            if (!ts.Contains(Tag))
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                returnlist.Add(c);
        //            }
        //        }
        //        return returnlist;
        //    }
        //}

        /// <summary>
        /// 获取一个栏目的标签集合
        ///  2011-11-9 标签已经整合，未发现此方法的引用
        /// </summary>
        /// <param name="channelID">栏目ID</param>
        /// <returns></returns>
        //public List<ChannelTag> GetChannelTags(string channelID)
        //{
        //    Criteria c = new Criteria(CriteriaType.Equals, "ColumnID", channelID);
        //    List<ChannelTag> ts = Assistant.List<ChannelTag>(c, null);
        //    return ts;
        //}

        /// <summary>
        /// 获取栏目的IP策略
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public string QueryStrategy(string channelID)
        {
            Channel channel = GetChannel(channelID, new string[] { "IPStrategy" });
            return channel != null ? channel.IPStrategy : String.Empty;
        }

        /// <summary>
        /// 更新IP安全策略
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="ipStrategy"></param>
        /// <returns></returns>
        public void UpdateStrategy(string channelID, string ipStrategy)
        {
            Channel channel = GetChannel(channelID, new string[] { "IPStrategy" });
            channel.IPStrategy = ipStrategy;
            Assistant.Update(channel, new string[] { "IPStrategy" });
        }


        #region 栏目优化
        /// <summary>
        /// 按照更新时间获取所有的栏目集合
        /// </summary>
        /// <returns></returns>
        public List<Channel> GetAllChannel()
        {
            Order[] o = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Channel>(null, o);
        }

        /// <summary>
        /// 更新栏目标签
        ///  2011-11-9 标签已经整合，未发现此方法的引用
        /// </summary>
        /// <returns></returns>
        //public int UpdateArticleTags()
        //{
        //    int count = 0;
        //    List<Channel> allList = GetAllChannel();
        //    if (allList != null)
        //    {
        //        foreach (Channel channel in allList)
        //        {
        //            StringBuilder sb = new StringBuilder();
                    
        //            //List<ChannelTag> allTagsByChannelID = GetTags(channel.ID);
        //            //if (allTagsByChannelID != null)
        //            //{
        //            //    int i = 0;
        //            //    foreach (ChannelTag channelTag in allTagsByChannelID)
        //            //    {
        //            //        sb.Append("'" + channelTag.Identifier + "'");
                             
        //            //        i++;
        //            //    }
        //            //}
        //            //if (sb.ToString() != "")
        //            //{
        //            //    channel.Tags = sb.ToString();
        //            //    Assistant.Update(channel, new string[] { "Tags" });
        //            //    count++;
        //            //}
        //        }
        //    }
        //    return count;
        //}

        /// <summary>
        /// 优化栏目标签查询
        /// </summary>
        /// <param name="channelEnumState"></param>
        /// <param name="stringList"></param>
        /// <returns></returns>
        public List<Channel> GetChannelType(string channelEnumState, List<string> stringList)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "EnumState", channelEnumState);
            c.Adorn = Adorns.Substring;
            c.Start = EnumLibrary.Position[(int)EnumLibrary.Business.ChannelContentType];
            c.Length = EnumLibrary.PlaceLenth;
            c.Mode = CriteriaMode.Or;
            if (stringList != null)
            {
                foreach (string tag in stringList)
                {
                    c.AddOr(CriteriaType.Like, "Tags", "%" + tag + "%");
                }
            }
            List<Channel> channels = Assistant.List<Channel>(c, null);
            return channels;
        }



        /// <summary>
        ///  通过栏目权限和栏目状态获取相应的栏目集合
        /// </summary>
        /// <param name="channelEnumState">栏目状态</param>
        /// <param name="iDList">栏目ID集合</param>
        /// <returns></returns>
        public List<Channel> GetChannelByIDList(string channelEnumState, List<string> iDList)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "EnumState", channelEnumState);
            c.Adorn = Adorns.Substring;
            c.Start = EnumLibrary.Position[(int)EnumLibrary.Business.ChannelContentType];
            c.Length = EnumLibrary.PlaceLenth;

            if (iDList != null)
            {
                Criteria keyCriteria = new Criteria(CriteriaType.None);
                keyCriteria.Mode = CriteriaMode.Or;
                foreach (string id in iDList)
                {
                    keyCriteria.Add(CriteriaType.Equals, "ID", id);
                }
                c.Criterias.Add(keyCriteria);
            }
            List<Channel> channels = Assistant.List<Channel>(c, null);
            return channels;
        }


        /// <summary>
        /// 通过权限和栏目名获取栏目集合
        /// </summary>
        /// <param name="stringList">栏目ID集合</param>
        /// <param name="name">按栏目名称排序</param>
        /// <param name="filedName">栏目名称</param>
        /// <returns></returns>
        public List<Channel> GetChannelByParentID(List<string> stringList, string name, string filedName)
        {
            Criteria c = new Criteria(CriteriaType.None);

            if (stringList != null)
            {
                Criteria keyCriteria = new Criteria(CriteriaType.None);
                keyCriteria.Mode = CriteriaMode.Or;
                foreach (string id in stringList)
                {
                    keyCriteria.Add(CriteriaType.Equals, "ID", id);
                }
                c.Criterias.Add(keyCriteria);
            }
            if (name != null && name.Length > 0 && filedName != null && filedName.Length > 0)
            {
                c.Criterias.Add(new Criteria(CriteriaType.Like, filedName, name + "%"));

            }
            Order[] o = new Order[] { new Order("Name", OrderMode.Desc) };
            List<Channel> channels = Assistant.List<Channel>(c, o);
            return channels;
        }
        #endregion


        /// <summary>
        /// 格式化栏目ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string FormatChannelGUID(string id)
        {
            if (We7Helper.IsEmptyID(id) || We7Helper.IsGUID(id))
                return id;
            else
            {
                Channel ch = GetChannelByAlias(id);
                if (ch != null)
                    return ch.ID;
                else
                    return null;
            }
        }

        /// <summary>
        /// 栏目查询：通过查询类进行复杂查询
        /// </summary>
        /// <param name="query">查询类</param>
        /// <returns></returns>
        public List<Channel> QueryChannels(ChannelQuery query)
        {
            Criteria c = CreateCriteriaByQuery(query);
            List<Order> orders = CreateOrdersByAll(query.OrderKeys);
            Order[] o = null;
            if (orders != null) o = orders.ToArray();
            return Assistant.List<Channel>(c, o);
        }

        /// <summary>
        /// 创建查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private Criteria CreateCriteriaByQuery(ChannelQuery query)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (query.EnumState != null && query.EnumState != "")
            {
                Criteria csubC = new Criteria();
                csubC.Name = "EnumState";
                csubC.Value = query.EnumState;
                csubC.Type = CriteriaType.Equals;
                csubC.Adorn = Adorns.Substring;
                csubC.Start = EnumLibrary.Position[(int)EnumLibrary.Business.ArticleType];
                csubC.Length = EnumLibrary.PlaceLenth;
                c.Criterias.Add(csubC);
            }

            if (query.State != ArticleStates.All)
                c.Add(CriteriaType.Equals, "State", (int)query.State);

            if (query.ParentID != null && query.ParentID != "")
            {
                if (query.IncludeAllSons)
                {
                    Channel ch = GetChannel(query.ParentID, null);
                    if (ch != null)
                    {
                        c.Add(CriteriaType.Like, "FullUrl", ch.FullUrl + "%");
                    }
                }
                else
                    c.Add(CriteriaType.Equals, "ParentID", query.ParentID);
            }
            if (!string.IsNullOrEmpty(query.Tag))
                c.Add(CriteriaType.Like, "Tags", "%'" + query.Tag + "'%");

            return c;
        }

        #region 静态URL

        private static readonly string ChannelKeyName = "Channel:{0}";

        /// <summary>
        /// 通过URL取得栏目ID号
        /// </summary>
        /// <returns></returns>
        public string GetChannelIDFromURL()
        {
            HttpContext Context = HttpContext.Current;
            if (Context.Request["id"] != null)
                return Context.Request["id"];
            else
            {
                string chID = string.Empty;
                string channelUrl = GetChannelUrlFromUrl(Context.Request.RawUrl, Context.Request.ApplicationPath);
                string key = string.Format(ChannelKeyName, channelUrl);
                string channelID = (string)Context.Items[key];
                if (string.IsNullOrEmpty(channelUrl)) channelID = We7Helper.EmptyGUID;
                if (channelID == null || channelID.Length == 0)
                {
                    channelID = (string)Context.Cache[key];
                    if (channelID == null || channelID.Length == 0)
                    {
                        if (channelUrl != string.Empty)
                            channelID = GetChannelIDByFullUrl(channelUrl);
                        if (channelID != null && channelID.Length > 0)
                        {
                            CacherCache(key, Context, channelID, CacheTime.Short);
                        }
                    }
                    if (Context.Items[key] == null)
                    {
                        Context.Items.Remove(key);
                        Context.Items.Add(key, channelID);
                    }
                }
                return channelID;
            }
        }


        /// <summary>
        /// 通过URL取得栏目唯一名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public string GetChannelNameFromUrl(string path, string app)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si == null) return "";
            string ext = si.UrlFormat;
            if (ext == null || ext.Length == 0) ext = "html";

            if (path.LastIndexOf("?") > -1)
            {
                if (path.ToLower().IndexOf("channel=") > -1)
                {
                    path = path.Substring(path.ToLower().IndexOf("channel=") + 8);
                    if (path.IndexOf("&") > -1)
                        path = path.Remove(path.IndexOf("&"));
                }
                else
                    path = path.Remove(path.LastIndexOf("?"));
            }

            if (path.ToLower().EndsWith(".aspx") || path.ToLower().EndsWith("." + ext))
                path = path.Remove(path.LastIndexOf("/") + 1);

            if (!path.StartsWith("/")) path = "/" + path;
            string mathstr = @"(?:\/(\w|\s|(-)|(_))+((\/?))?)$";
            if (Regex.IsMatch(path, mathstr))
            {
                if (!app.StartsWith("/"))
                {
                    app = "/" + app;
                }
                if (!app.EndsWith("/"))
                {
                    app += "/";
                }
                path = path.Replace("//", "/");
                if (path.ToLower().StartsWith(app.ToLower()))
                {
                    path = path.Remove(0, app.Length);
                }
                if (path.EndsWith("/"))
                {
                    path = path.Remove(path.Length - 1);
                }

                int lastSlash = path.LastIndexOf("/");
                if (lastSlash > -1)
                {
                    path = path.Remove(0, lastSlash + 1);
                }

                if (path.ToLower() == "go") path = string.Empty;
                return path;
            }
            else
                return string.Empty;
        }

        ///// <summary>
        ///// 通过url获取当前栏目名称
        ///// </summary>
        ///// <returns></returns>
        //public string GetChannelNameFromURL()
        //{
        //    HttpContext Context = HttpContext.Current;
        //    if (Context.Request["id"] != null)
        //        return Context.Request["id"];
        //    else
        //    {
        //        string chID = string.Empty;
        //        string channelName = GetChannelUrlFromUrl(Context.Request.RawUrl, Context.Request.ApplicationPath);
        //        return channelName;
        //    }
        //}

        /// <summary>
        /// 通过URL取得栏目唯一名称
        /// 2.6版修改：栏目唯一名称变更为 FullUrl
        /// </summary>
        /// <param name="path"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public string GetChannelUrlFromUrl(string path, string app)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si == null) return "";
            string ext = si.UrlFormat;
            if (ext == null || ext.Length == 0) ext = "html";

            if (path.LastIndexOf("?") > -1)
            {
                if (path.ToLower().IndexOf("channel=") > -1)
                {
                    path = path.Substring(path.ToLower().IndexOf("channel=") + 8);
                    if (path.IndexOf("&") > -1)
                        path = path.Remove(path.IndexOf("&"));
                }
                else
                    path = path.Remove(path.LastIndexOf("?"));
            }

            if (path.ToLower().EndsWith(".aspx") || path.ToLower().EndsWith("." + ext))
                path = path.Remove(path.LastIndexOf("/") + 1);

            if (!path.StartsWith("/")) path = "/" + path;
            string mathstr = @"(?:\/(\w|\s|(-)|(_))+((\/?))?)$";
            if (Regex.IsMatch(path, mathstr))
            {
                if (!app.StartsWith("/"))
                {
                    app = "/" + app;
                }
                if (!app.EndsWith("/"))
                {
                    app += "/";
                }
                path = path.Replace("//", "/");
                if (path.ToLower().StartsWith(app.ToLower()))
                {
                    path = path.Remove(0, app.Length);
                }
                //if (path.EndsWith("/"))
                //{
                //    path = path.Remove(path.Length - 1);
                //}

                //int lastSlash = path.LastIndexOf("/");
                //if (lastSlash > -1)
                //{
                //    path = path.Remove(0, lastSlash + 1);
                //}

                if (path.ToLower() == "go") path = string.Empty;
                if (!path.EndsWith("/")) path += "/";
                if (!path.StartsWith("/")) path = "/" + path;
                return path;
            }
            else
                return string.Empty;
        }


        #endregion
    }



    /// <summary>
    /// 栏目查询类
    /// </summary>
    public class ChannelQuery
    {
        public string ParentID { set; get; }
        public string Tag { set; get; }
        public string EnumState { set; get; }
        public ArticleStates State { set; get; }
        public bool IncludeAllSons { get; set; }
        public string ChannelFullUrl { get; set; }
        /// <summary>
        ///  排序字段请按“Created|Asc,Clicks|Desc”模式传入
        /// </summary>
        public string OrderKeys { set; get; }

    }

}
