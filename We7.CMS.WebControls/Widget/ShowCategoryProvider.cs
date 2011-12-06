using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using System.Web.UI;
using We7.Model.Core;
using System.Web.Script.Serialization;

namespace We7.CMS.WebControls
{
    public class ShowCategoryProvider : BaseWebControl
    {

        /// <summary>
        /// css样式
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// 一级最多显示个数
        /// </summary>
        public int LevelOneMax { get; set; }

        /// <summary>
        /// 二级最多显示个数
        /// </summary>
        public int LevelTwoMax { get; set; }

        /// <summary>
        /// 三级最多显示个数
        /// </summary>
        public int LevelThreeMax { get; set; }

        /// <summary>
        /// 一级类别默认是否显示
        /// </summary>
        public bool Display{get; set;}
        
        /// <summary>
        /// 类别最大显示字数
        /// </summary>
        public int TitleMaxLength { get; set; }

        /// <summary>
        /// 父栏目编号        
        /// </summary>
        public string ParentID { get; set; }

        private string _dataSourceType = "db";
        /// <summary>
        /// 数据源类型        
        /// </summary>
        public string DataSourceType 
        {
            get { return _dataSourceType; }
            set { this._dataSourceType = value; }
        }


        private string _dataSourceName = "Channel";
        /// <summary>
        /// 数据源名称        
        /// </summary>
        public string DataSourceName
        {
            get { return _dataSourceName; }
            set { _dataSourceName = value; }
        }

        private string _channelName = "Title";
        /// <summary>
        /// 显示名称(字段名或者xml属性名)        
        /// </summary>
        public string ChannelName { get { return _channelName; } set { this._channelName = value; } }

        private string _channelID = "ID";
        /// <summary>
        /// 显示值(字段名或者xml属性名)        
        /// </summary>
        public string ChannelID { get { return _channelID; } set { this._channelID = value; } }

        private string _parentIDName = "ParentID";
        /// <summary>
        /// 父类别名(字段名)        
        /// </summary>
        public string ParentIDName { get { return _parentIDName; } set { this._parentIDName = value; } }
                
        private string  _fullUrl ="FullUrl";
        /// <summary>
        /// 链接地址(字段名或者xml属性名)    
        /// </summary>
        public string FullUrl { get { return _fullUrl; } set { this._fullUrl = value; } }

        private string _titleImage = "TitleImage";
        /// <summary>
        /// 图片地址(字段名或者xml属性名)    
        /// </summary>
        public string TitleImage { get { return _titleImage; } set { this._titleImage = value; } }

        /// <summary>
        /// json数据
        /// </summary>
        public string JsonData
        {
            get;
            set;
        }
        /// <summary>
        /// 集合数据
        /// </summary>
        public List<ShowCategoryModel> lsResult
        {
            get;
            set;
        }


        /// <summary>
        /// 格式化栏目数据
        /// </summary>
        /// <param name="list">栏目列表</param>
        /// <returns>栏目列表</returns>
        protected virtual List<ShowCategoryModel> FormatChannelsData(List<ShowCategoryModel> list)
        {
            DateTime now = DateTime.Now;
            foreach (ShowCategoryModel ch in list)
            {
                if (TitleMaxLength > 0 && ch.Name.Length > TitleMaxLength)
                {
                    ch.Name =  We7.Framework.Util.Utils.CutString(ch.Name,0, TitleMaxLength);
                }
                if (ch.Children.Count > 0)
                {
                    foreach (ShowCategoryModel levelTwoModel in ch.Children)
                    {
                        if (TitleMaxLength > 0 && levelTwoModel.Name.Length > TitleMaxLength)
                        {
                            levelTwoModel.Name =  We7.Framework.Util.Utils.CutString(levelTwoModel.Name,0, TitleMaxLength);
                        }
                        if(levelTwoModel.Children.Count > 0)
                        {
                            foreach (ShowCategoryModel levelThirdModel in levelTwoModel.Children)
                            {
                                if (TitleMaxLength > 0 && levelThirdModel.Name.Length > TitleMaxLength)
                                {
                                    levelThirdModel.Name =  We7.Framework.Util.Utils.CutString(levelThirdModel.Name,0, TitleMaxLength);
                                }
                            }
                        }
                    }                    
                }
            }
            return list;
        }


        /// <summary>
        /// 绑定数据
        /// </summary>
        void BindData()
        {
            IShowCategoryProvider provider = GetProvider();
            List<ShowCategoryModel> ls = provider.GetThreeLevelData(ParentID);
            ls = FormatChannelsData(ls);
            lsResult = ls;
            JavaScriptSerializer js = new JavaScriptSerializer();
            JsonData = js.Serialize(lsResult);             
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (ParentID != null && ParentID.Length > 0)
            {}
            else
            {
                ParentID = We7.We7Helper.EmptyGUID;
            }
            BindData();
        }


        /// <summary>
        /// 取得查询实体
        /// </summary>
        /// <returns></returns>
        IShowCategoryProvider GetProvider()
        {
            ParamCollection col = new ParamCollection(); 

            col.Add(new Param("dataSourceName",DataSourceName ));
            col.Add(new Param("ChannelName", ChannelName));
            col.Add(new Param("ChannelID", ChannelID));
            col.Add(new Param("FullUrl", FullUrl));
            col.Add(new Param("TitleImage", TitleImage));
            col.Add(new Param("ParentID", ParentID));
            col.Add(new Param("ParentIDName", ParentIDName));
            col.Add(new Param("LevelOneMax", LevelOneMax.ToString()));
            col.Add(new Param("ParentID", LevelTwoMax.ToString()));
            col.Add(new Param("ParentID", LevelThreeMax.ToString()));

            if (!String.IsNullOrEmpty(DataSourceType) && DataSourceType == "xml")
            {
                return new XmlShowCategoryProvider(col);
            }
            else
            {
                return new DbShowCategoryProvider(col);
            }
        }


    }
}
