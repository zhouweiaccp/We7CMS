using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using We7.Framework.Util;
using System.Web;
using System.IO;
using We7.Framework;
using Thinkment.Data;
using System.Data;
using System.Collections;
using We7.Model.Core;
using System.Web.Script.Serialization;

namespace We7.CMS.WebControls
{
        public class ShowCategoryModel
        {
            public string Name;
            public string Value;
            public string PicUrl;
            public string LinkUrl;
            public List<ShowCategoryModel> Children = new List<ShowCategoryModel>();
        }
        /// <summary>
        /// 商城类别导航数据接口
        /// </summary>
        interface IShowCategoryProvider
        {
            List<ShowCategoryModel> GetThreeLevelData(string value);                

        }

        /// <summary>
        /// 用于从xml取得数据
        /// </summary>
        internal class XmlShowCategoryProvider : IShowCategoryProvider
        {
            private string rootNode = "root", firstNode = "first", secondNode = "second",thirdNode = "third",dataSourceName, ChannelName = "ChannelName", ChannelID = "ID", FullUrl = "FullUrl", TitleImage = "TitleImage", ParentID = We7.We7Helper.EmptyGUID, ParentIDName = "ParentID", LevelOneMax = "10", LevelTwoMax = "10", LevelThreeMax = "10";         
            public XmlShowCategoryProvider(ParamCollection parameters)
            {
                InitField(parameters);
            }
            public XmlShowCategoryProvider() { }

            /// <summary>
            /// 设置xml文件路径
            /// </summary>
            public string DataSourceName
            {
                set { dataSourceName = value; }
            }

            #region XmlShowCategoryProvider 成员            
            /// <summary>
            /// xml获取三层数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public List<ShowCategoryModel> GetThreeLevelData(string value)
            {
                List<ShowCategoryModel> lsOne = Query(String.Format("{0}/{1}", rootNode, firstNode), LevelOneMax);
                foreach(ShowCategoryModel modelOne in lsOne)
                {
                    if(modelOne.Children.Count == 0)
                    {
                        List<ShowCategoryModel> lsTwo = Query(String.Format("{0}/{1}[@" + ChannelID + "='{3}']/{2}", rootNode, firstNode, secondNode, modelOne.Value), LevelTwoMax);
                        foreach(ShowCategoryModel modelTwo in lsTwo)
                        {
                            if(modelTwo.Children.Count == 0)
                            {
                                List<ShowCategoryModel> lsThird = Query(String.Format("{0}/{1}[@" + ChannelID + "='{4}']/{2}[@" + ChannelID + "='{5}']/{3}", rootNode, firstNode, secondNode, thirdNode, modelOne.Value, modelTwo.Value), LevelThreeMax);
                                modelTwo.Children = lsThird;
                            }
                        }
                        modelOne.Children = lsTwo;
                    }
                }
                return lsOne;
            }

              
            #endregion

            void InitField(ParamCollection parameters)
            {
                dataSourceName = parameters["dataSourceName"];
                if (String.IsNullOrEmpty(dataSourceName))
                    throw new Exception("没有指定类别导航控件的Xml路径");
                dataSourceName = HttpContext.Current.Server.MapPath(dataSourceName);
                if (!File.Exists(dataSourceName))
                {
                    throw new Exception("不存在指定的类别导航文件");
                }
                ChannelName = parameters["ChannelName"];
                ChannelID = parameters["ChannelID"];
                FullUrl = parameters["FullUrl"];
                TitleImage = parameters["TitleImage"];
                ParentID = parameters["ParentID"];
                ParentIDName = parameters["ParentIDName"];
                LevelOneMax = parameters["LevelOneMax"];
                LevelTwoMax = parameters["LevelTwoMax"];
                LevelThreeMax = parameters["LevelThreeMax"];
            }

            List<ShowCategoryModel> Query(string xpath,string count)
            {
                int intCount = 5,myCount = 0;
                bool isRight = false;
                if (int.TryParse(count, out intCount) && intCount > 0)
                {
                    isRight = true;
                }                
                List<ShowCategoryModel> dic = new  List<ShowCategoryModel>();
                XmlNodeList nodes = XmlHelper.GetXmlNodeList(dataSourceName, xpath);                
                foreach (XmlElement xe in nodes)
                {
                    if (isRight)
                    {
                        myCount++;
                        if (myCount >= intCount)
                        {
                            break;
                        }
                    }
                    string name = xe.GetAttribute(ChannelName);
                    string value = xe.GetAttribute(ChannelID);
                    string picUrl = xe.GetAttribute(TitleImage);
                    string linkUrl = xe.GetAttribute(FullUrl);
                    List<ShowCategoryModel> lsChild = new List<ShowCategoryModel>();
                    ShowCategoryModel model = new ShowCategoryModel();
                    if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(value))
                    {
                        model.Name = name;
                        model.Value = value;
                        model.PicUrl = picUrl;
                        model.LinkUrl = linkUrl;                       
                        dic.Add(model);
                    }
                }
                return dic;
            }
        }


        /// <summary>
        /// 用于从db取得数据
        /// </summary>
        internal class DbShowCategoryProvider : IShowCategoryProvider
        {
           private string dataSourceName, ChannelName = "ChannelName", ChannelID = "ID", FullUrl = "FullUrl", TitleImage = "TitleImage", ParentID = We7.We7Helper.EmptyGUID, ParentIDName = "ParentID", LevelOneMax = "10", LevelTwoMax = "10", LevelThreeMax = "10";

            /// <summary>
            /// 业务对象工厂
            /// </summary>
            protected HelperFactory HelperFactory
            {
                get
                {
                    return (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                }
            }

            /// <summary>
            /// 文章业务对象
            /// </summary>
            protected ArticleHelper ArticleHelper
            {
                get { return HelperFactory.GetHelper<ArticleHelper>(); }
            }

            /// <summary>
            /// 数据访问对象
            /// </summary>
            protected ObjectAssistant Assistant
            {
                get { return ArticleHelper.Assistant; }
            }
         
            public DbShowCategoryProvider(ParamCollection parameters)
            {
                InitField(parameters);
            }

            public DbShowCategoryProvider() { }

            /// <summary>
            /// db数据库表名
            /// </summary>
            public string TableName
            {
                set { dataSourceName = value; }
            }

            /// <summary>
            /// DB获取三层数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public List<ShowCategoryModel> GetThreeLevelData(string value)
            {
                List<ShowCategoryModel> lsOne = Query("[" + ParentIDName + "]='" + value + "' ", LevelOneMax);
                foreach (ShowCategoryModel modelOne in lsOne)
                {
                    if (modelOne.Children.Count == 0)
                    {
                        string strWhereTwo = "[" + ParentIDName + "]='" + modelOne.Value + "' ";
                        List<ShowCategoryModel> lsTwo = Query(strWhereTwo, LevelTwoMax);
                        foreach (ShowCategoryModel modelTwo in lsTwo)
                        {
                            if (modelTwo.Children.Count == 0)
                            {
                                string strWhereThird = "[" + ParentIDName + "]='" + modelTwo.Value + "' ";
                                List<ShowCategoryModel> lsThird = Query(strWhereThird, LevelThreeMax);
                                modelTwo.Children = lsThird;
                            }
                        }
                        modelOne.Children = lsTwo;
                    }
                }
                return lsOne;
            }

            void InitField(ParamCollection parameters)
            {
                dataSourceName = parameters["dataSourceName"];
                if (String.IsNullOrEmpty(dataSourceName))
                    throw new Exception("没有指定类别导航控件的数据表名称");
                ChannelName = parameters["ChannelName"];
                ChannelID = parameters["ChannelID"];
                FullUrl = parameters["FullUrl"];
                TitleImage = parameters["TitleImage"];
                ParentID = parameters["ParentID"];
                ParentIDName = parameters["ParentIDName"];
                LevelOneMax = parameters["LevelOneMax"];
                LevelTwoMax = parameters["LevelTwoMax"];
                LevelThreeMax = parameters["LevelThreeMax"];
            }
            List<ShowCategoryModel> Query(string where, string count)
            {
                int intCount = 5, myCount = 0;
                bool isRight = false;
                if (int.TryParse(count, out intCount) && intCount > 0)
                {
                    isRight = true;
                }

                List<ShowCategoryModel> dic = new List<ShowCategoryModel>();

                string sqlField1 = " SELECT DISTINCT [" + ChannelName + "],[" + ChannelID + "],[" + FullUrl + "],[" + TitleImage + "] FROM [" + dataSourceName + "]  ";
                if (!string.IsNullOrEmpty(where))
                {
                    sqlField1 += " where " + where;
                }
                IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
                SqlStatement sqlstatement = new SqlStatement();
                sqlstatement.SqlClause = sqlField1;
                db.DbDriver.FormatSQL(sqlstatement);
                DataTable dt = new DataTable();
                using (IConnection conn = db.CreateConnection())
                {
                    dt = conn.Query(sqlstatement);
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (isRight)
                    {
                        myCount++;
                        if (myCount >= intCount)
                        {
                            break;
                        }
                    }
                    string name = row[ChannelName].ToString();
                    string value = row[ChannelID].ToString();
                    string picUrl = row[TitleImage].ToString();
                    string linkUrl = row[FullUrl].ToString();
                    ShowCategoryModel model = new ShowCategoryModel();
                    if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(value))
                    {
                        model.Name = name;
                        model.Value = value;
                        model.PicUrl = picUrl;
                        model.LinkUrl = linkUrl;
                        dic.Add(model);
                    }
                }
                return dic;
            }


        }
}
