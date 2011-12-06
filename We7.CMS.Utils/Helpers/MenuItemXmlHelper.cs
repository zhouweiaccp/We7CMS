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
    /// 问题处理助手类
    /// </summary>
    [Serializable]
    [Helper("We7.MenuItemXmlHelper")]
    public class MenuItemXmlHelper : BaseHelper
    {
        /// <summary>
        /// 构造
        /// </summary>
        public MenuItemXmlHelper()
        { }
        /// <summary>
        ///xml文件路径 
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        public MenuItemXmlHelper(string filePath)
        {
            FilePath = filePath;
        }

        #region 基本操作：插入、删除、更新、获取

        /// <summary>
        /// 根据ID删除一条记录
        /// </summary>
        /// <param name="xmlPath"></param>
        public void DeleteMenuItemWidthChilds(string oldId)
        {
            //删除当前item节点
            string xPath = "/root/items/item[@oldid='" + oldId + "']";
            string id = GetMenuItemXml(xPath).ID;
            XmlHelper.DeleteXmlNode(FilePath, xPath);
            xPath = "/root/menuTree/menu/menu[@id='" + id + "']";
            XmlHelper.DeleteXmlNode(FilePath, xPath);
            XmlNodeList tempNode = XmlHelper.GetXmlNodeList(FilePath, "/root/items/item[@oldparent='" + oldId + "']");
            foreach (XmlNode node in tempNode)
            {
                XmlHelper.DeleteXmlNode(FilePath, "/root/items/item[@id='" + node.Attributes["id"].Value + "']");
            }
        }

        /// <summary>
        /// 或许特定ITEM个数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int GetMenuCountByAttribut(string key, string value)
        {
            string xPath = "/root/items";
            List<MenuItemXml> list = GetMenuItemXmls(xPath);
            if (key == "type")
            {
                if (value == "100")
                    return list.Count;
                else
                    return list.FindAll(delegate(MenuItemXml entity) { return entity.Type == int.Parse(value); }).Count;
            }
            if (key == "group")
            {

                return list.FindAll(delegate(MenuItemXml entity) { return entity.Group == value; }).Count;
            }
            return 0;
        }

        /// <summary>
        /// 根据ID删除一条记录
        /// </summary>
        /// <param name="xmlPath"></param>
        public void DeleteMenuItemXml(string xmlPath)
        {
            DeleteMenuItemXml(xmlPath);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public MenuItemXml GetMenuItemXml(string xPath)
        {
            XmlNode tempNode = XmlHelper.GetXmlNode(FilePath, xPath);

            return ConvertToEntityByNode(tempNode);
        }

        /// <summary>
        /// 更新子节点的某一属性
        /// </summary>
        /// <param name="oldParentId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateChildsAttribute(string oldParentId, string key, string value)
        {
            XmlNodeList tempNode = XmlHelper.GetXmlNodeList(FilePath, "/root/items/item[@oldparent='" + oldParentId + "']");
            if (tempNode != null)
            {
                foreach (XmlNode node in tempNode)
                {
                    if (node.Attributes[key] == null || node.Attributes[key].Value != value)
                    {
                        Dictionary<string, string> dicAttr = new Dictionary<string, string>();
                        dicAttr.Add(key, value);
                        XmlHelper.UpdateXmlNode(FilePath, "/root/items/item[@id='" + node.Attributes["id"].Value + "']", dicAttr);
                    }
                }
            }
        }



        /// <summary>
        /// 根据节点转换成实体
        /// </summary>
        /// <returns></returns>
        private MenuItemXml ConvertToEntityByNode(XmlNode tempNode)
        {
            if (tempNode != null)
            {
                MenuItemXml a = new MenuItemXml();
                a.ID = tempNode.Attributes["id"] == null ? "" : tempNode.Attributes["id"].Value;
                a.Lable = tempNode.Attributes["label"] == null ? "" : tempNode.Attributes["label"].Value;
                a.MatchParameter = tempNode.Attributes["matchParameter"] == null ? "" : tempNode.Attributes["matchParameter"].Value;
                a.Name = tempNode.Attributes["name"] == null ? "" : tempNode.Attributes["name"].Value;
                a.Oldid = tempNode.Attributes["oldid"] == null ? "" : tempNode.Attributes["oldid"].Value;
                a.Oldparent = tempNode.Attributes["oldparent"] == null ? "" : tempNode.Attributes["oldparent"].Value;
                a.Parent = tempNode.Attributes["parent"] == null ? "" : tempNode.Attributes["parent"].Value;
                a.Url = tempNode.Attributes["url"] == null ? "" : tempNode.Attributes["url"].Value;
                a.Link = tempNode.Attributes["Link"] == null ? "" : tempNode.Attributes["Link"].Value;
                a.Type = tempNode.Attributes["type"] == null ? 0 : int.Parse(tempNode.Attributes["type"].Value);
                a.Group = tempNode.Attributes["group"] == null ? "0" : tempNode.Attributes["group"].Value;
                a.NodeName = tempNode.Name;
                return a;
            }
            return null;
        }
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="a">一条记录对象</param>
        public void AddMenuItemXmls(MenuItemXml a, string xPath)
        {
            Dictionary<string, string> dicAttr = new Dictionary<string, string>();
            dicAttr.Add("id", a.ID);
            dicAttr.Add("label", a.Lable);
            dicAttr.Add("url", a.Url);
            dicAttr.Add("matchParameter", a.MatchParameter);
            dicAttr.Add("name", a.Name);
            dicAttr.Add("oldid", a.Oldid);
            dicAttr.Add("oldparent", a.Oldparent);
            dicAttr.Add("parent", a.Parent);
            dicAttr.Add("link", a.Link);
            dicAttr.Add("type", a.Type.ToString());
            dicAttr.Add("group", a.Group);

            XmlHelper.CreateXmlNode(FilePath, xPath, a.NodeName, "", dicAttr);
        }

        /// <summary>
        /// 更新一条记录记录
        /// </summary>
        /// <param name="a">一条记录记录</param>
        /// <param name="xPath">xPath</param>
        public void UpdateMenuItemXml(MenuItemXml a, string xPath)
        {
            Dictionary<string, string> dicAttr = new Dictionary<string, string>();
            dicAttr.Add("id", a.ID);
            dicAttr.Add("label", a.Lable);
            dicAttr.Add("url", a.Url);
            dicAttr.Add("matchParameter", a.MatchParameter);
            dicAttr.Add("name", a.Name);
            dicAttr.Add("oldid", a.Oldid);
            dicAttr.Add("oldparent", a.Oldparent);
            dicAttr.Add("parent", a.Parent);
            dicAttr.Add("link", a.Link);
            dicAttr.Add("type", a.Type.ToString());
            dicAttr.Add("group", a.Group);
            XmlHelper.UpdateXmlNode(FilePath, xPath, dicAttr);
        }

        /// <summary>
        /// 通过xPath获取菜单列表
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public List<MenuItemXml> GetMenuItemXmls(string xPath)
        {
            List<MenuItemXml> lsResult = new List<MenuItemXml>();

            XmlNode parentNode = XmlHelper.GetXmlNode(FilePath, xPath);
            if (parentNode != null)
            {
                XmlNodeList lsNodes = parentNode.ChildNodes;
                foreach (XmlNode tempNode in lsNodes)
                {
                    lsResult.Add(ConvertToEntityByNode(tempNode));
                }
            }

            return lsResult;
        }




        /// <summary>
        /// 添加一条显示记录
        /// </summary>
        /// <param name="a"></param>
        /// <param name="xPath"></param>
        public void AddMenuItemDisplay(MenuItemXml a, string xPath)
        {
            Dictionary<string, string> dicAttr = new Dictionary<string, string>();
            dicAttr.Add("id", a.ID);
            dicAttr.Add("label", a.Lable);
            dicAttr.Add("link", a.Link);

            XmlHelper.CreateXmlNode(FilePath, xPath, a.NodeName, "", dicAttr);
        }





        #endregion
    }
}
