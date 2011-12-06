using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 系统菜单项，同时用于标识可访问功能项
    /// </summary>
    [Serializable]
    public class MenuItem
    {
        string id;
        string parentID;
        string name;
        string title;
        string href;
        int type; 
        int index;
        DateTime created=DateTime.Now;
        List<MenuItem> items;
        DateTime updated=DateTime.Now;

        string icon;
        string iconHover;
        int group;
        int menuType;
        string referenceID;

        /// <summary>
        /// 编组组号，如0-3
        /// </summary>
        public int Group
        {
            get { return group; }
            set { group = value; }
        }

        /// <summary>
        /// 菜单图标：鼠标滑过图片
        /// </summary>
        public string IconHover
        {
            get { return iconHover; }
            set { iconHover = value; }
        }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        public MenuItem()
        {
            items = new List<MenuItem>();
        }

        /// <summary>
        /// 主键
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 父菜单ID
        /// </summary>
        public string ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 0 - 系统菜单，1 - 用户自定义菜单，2 - 系统菜单隐藏
        /// </summary>
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 序列
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// 子菜单列表
        /// </summary>
        public List<MenuItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        /// <summary>
        /// 可访问URL
        /// </summary>
        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// 权限类别标示符
        /// </summary>
        public string EntityID { get; set; }

        /// <summary>
        /// 不含参数的URL路径
        /// </summary>
        public string PageLocation
        {
            get
            {
                if (string.IsNullOrEmpty(href))
                    return null;
                else
                {
                    if (Href.IndexOf("?") > -1)
                        return Href.Substring(0, Href.IndexOf("?"));
                    else
                        return Href;
                }
            }
        }

        /// <summary>
        /// URL参数部分
        /// </summary>
        public string QueryKey
        {
            get
            {
                if (string.IsNullOrEmpty(href))
                    return null;
                else
                {
                    if (Href.IndexOf("?") > -1)
                        return Href.Substring(Href.IndexOf("?")+1);
                    else
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// 用于权限树
        /// checked-子节点拥有全部权限；
        /// undetermined-子节点拥有部分权限
        /// unchecked-没有权限
        /// </summary>
        public string PermissionState { get; set; }

        /// <summary>
        /// 菜单类型（0：普通菜单,1:顶部一级菜单,2:分组类型,3:引用类型）
        /// </summary>
        public int MenuType
        {
            get
            {
                return menuType;
            }
            set
            {
                menuType = value;
            }
        }

        /// <summary>
        ///引用的菜单ID 
        /// </summary>
        public string ReferenceID
        {
            get
            {
                return referenceID;
            }
            set
            {
                referenceID = value;
            }
        }




    }

}
