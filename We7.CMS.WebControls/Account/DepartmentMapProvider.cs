using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using System.Web.UI.WebControls;
using System.Data;
using We7.CMS.Common.PF;
using We7.Framework.Config;
using We7.Framework.Util;

namespace We7.CMS.WebControls
{
    public class DepartmentMapProvider : BaseWebControl
    {
        private string queryKey = "DepartID";
        private string appendQueryKey;

        #region 属性面板参数
        /// <summary>
        /// 样式
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryKey
        {
            get { return queryKey; }
            set { queryKey = value; }
        }

        /// <summary>
        /// 附加查询信息
        /// </summary>
        public string AppendQueryKey
        {
            get { return appendQueryKey; }
            set { appendQueryKey = value; }
        }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        private int _maxLength = 5;
        /// <summary>
        /// 标题最大字数
        /// </summary>
        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                _maxLength = value;
            }
        }
        private int _columnCount = 4;
        /// <summary>
        /// 显示列数
        /// </summary>
        public int ColumnCount
        {
            get
            {
                return _columnCount;
            }
            set
            {
                _columnCount = value;
            }
        }
        /// <summary>
        /// 二级最多显示个数
        /// </summary>
        public int LevelTwoMax { get; set; }

        /// <summary>
        /// 三级最多显示个数
        /// </summary>
        public int LevelThreeMax { get; set; }

        /// <summary>
        /// 四级最多显示个数
        /// </summary>
        public int LevelFourMax { get; set; }

        /// <summary>
        /// 父栏目编号        
        /// </summary>
        public string ParentID { get; set; }

        public bool IncludeParent { get; set; }


        #endregion

        private List<Department> _items = null;
        /// <summary>
        /// 取得带所有层级关系数据的Department
        /// </summary>
        public List<Department> Items
        {
            get
            {
                if (_items == null)
                {
                    string siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
                    if (IncludeParent && !String.IsNullOrEmpty(ParentID))
                    {
                        _items = new List<Department>();
                        string[] ids = ParentID.Split(',');
                        foreach (string id in ids)
                        {
                            Department depart = AccountHelper.GetDepartment(id, null);
                            if (depart != null)
                            {
                                _items.Add(depart);
                            }
                        }                        
                    }
                    else
                    {
                        _items = AccountHelper.GetOrderDepartments(siteID, ParentID);
                    }                   
                    CreateDepartmentList(_items);
                }
                return _items;
            }
        }

        int currentLevel = 1;
        /// <summary>
        /// 得到所有层级的Department
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private void CreateDepartmentList(List<Department> list)
        {
            foreach (Department tempDepartment in list)
            {
                List<Department> tempChildren = AccountHelper.GetOrderDepartments(null, tempDepartment.ID);
                if (tempChildren.Count > 0)
                {
                    //计算递归中当前层级
                    currentLevel++;
                    tempDepartment.Children = tempChildren;
                    CreateDepartmentList(tempDepartment.Children);
                    currentLevel--;
                }
            }
        }
        /// <summary>
        /// 得到当前层级显示个数(如果为0则显示全部)
        /// </summary>
        /// <returns></returns>
        private int GetLevelCount()
        {
            switch (currentLevel)
            {
                case 2:
                    return LevelTwoMax;
                case 3:
                    return LevelThreeMax;
                case 4:
                    return LevelFourMax;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 格式化url
        /// </summary>
        /// <param name="departID"></param>
        /// <returns></returns>
        public string FormartUrl(string departID)
        {
            if (!String.IsNullOrEmpty(AppendQueryKey)&& !String.IsNullOrEmpty(AppendQueryKey.Trim()))
            {
                string[] ss=AppendQueryKey.Split('&');
                StringBuilder sb = new StringBuilder();
                foreach (string s in ss)
                {
                    sb.AppendFormat("{0}={1}&", s, Request[s]);
                }
                Utils.TrimEndStringBuilder(sb, "&");
                return String.Format("{0}?{1}={2}&{3}",Url,QueryKey,departID,sb.ToString());
            }
            return string.Format(Url + "?" + QueryKey + "={0}", departID);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

    }
}
