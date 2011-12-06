<!--### name="店铺导航" type="system" version="1.0" created="2010/06/09" 
desc="店铺导航" author="We7 Group" ###-->
<%@ Control Language="C#" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="We7.Framework" %>
<%@ Import Namespace="We7.CMS" %>
<%@ Import Namespace="We7.Framework.Util" %>
<%@ Import Namespace="We7.CMS.Accounts" %>
<script runat="server" type="text/C#">
    /// <summary>
    /// 业务助手工厂
    /// </summary>
    protected HelperFactory HelperFactory
    {
        get
        {
            HelperFactory factory = HttpContext.Current.Application[HelperFactory.ApplicationID] as HelperFactory;
            if (factory == null)
            {
                We7.CMS.ApplicationHelper.ResetApplication();
                factory = HttpContext.Current.Application[HelperFactory.ApplicationID] as HelperFactory;
            }
            return factory;
        }
    }
    /// <summary>
    /// 栏目类业务助手
    /// </summary>
    protected ChannelHelper ChannelHelper
    {
        get { return HelperFactory.GetHelper<ChannelHelper>(); }
    }
    protected static We7.CMS.Accounts.IAccountHelper AccountHelper
    {
        get { return AccountFactory.CreateInstance(); }
    }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    protected string GetSelectClass(string type)
    {
        string fileName = Server.MapPath("/user/Resource/menuItems.xml");
        string currentPath = Request.Url.PathAndQuery;
        XmlNode currentNode = XmlHelper.GetXmlNodeByAttribute(fileName, "/root/items/item", "url", currentPath);
        if (currentNode == null)
        {
            return "";
        }
        else
        {
            if (currentNode.Attributes["parent"] != null && currentNode.Attributes["parent"].Value == type || currentNode.Attributes["id"].Value == type)
            {
                return " class='select' ";
            }
        }
        return "";
    }


    protected string UserName
    {
        get { return AccountHelper.GetAccount(Security.CurrentAccountID, null).LoginName; }
    }
</script>
<div id="meunLeft" class="realLeft">
    <%       
        string fileName = Server.MapPath("/user/Resource/menuItems.xml");
        string currentPath = Request.Url.PathAndQuery;
        currentPath = We7.We7Helper.RemoveParamFromUrl(currentPath, "id");
        currentPath = Server.UrlDecode(currentPath);
        XmlNode currentNode = XmlHelper.GetXmlNodeByAttribute(fileName, "/root/items/item", "url", currentPath);
        if (currentNode != null)
        {
           
            string attrId = "";
            if (currentNode.Attributes["parent"] != null)
            {
                attrId = currentNode.Attributes["parent"].Value;
            }
            else
            {
                attrId = currentNode.Attributes["id"].Value;
            }
            string xPath = "/root/menuTree/menu[@id='" + attrId + "']/menu";
            XmlNodeList xmlTreeRoot = XmlHelper.GetXmlNodeList(fileName, xPath);
            //xml树中的父节点
            //XmlNode treeNode = XmlHelper.GetTreeXmlNodeByAttribute(xmlTreeRoot, "id", attrId);
            foreach (XmlNode temp in xmlTreeRoot)
            {
                //判断二级菜单权限
                XmlNode itemNodeLevelTwo = XmlHelper.GetXmlNodeByAttribute(fileName, "/root/items/item", "id", temp.Attributes["id"].Value);
                string oldidLevelTwo = "";
                if (itemNodeLevelTwo.Attributes["oldid"] != null && !string.IsNullOrEmpty(itemNodeLevelTwo.Attributes["oldid"].Value))
                {
                    oldidLevelTwo = itemNodeLevelTwo.Attributes["oldid"].Value;
                }
                List<We7.CMS.Common.Permission> lsPermissionLevelTwo = AccountHelper.GetPermissions("System.User", oldidLevelTwo);
                if (lsPermissionLevelTwo.Count > 0)
                {         
    %>
    <div class="box mb_1">
        <div class="tit">
            <a class="setKG" href="####"></a><strong>
                <%=temp.Attributes["label"].Value%></strong></div>
        <div class="con">
            <ul>
                <%
                    if (temp.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode tempNode in temp.ChildNodes)
                        {
                            XmlNode itemNode = XmlHelper.GetXmlNodeByAttribute(fileName, "/root/items/item", "id", tempNode.Attributes["id"].Value);
                            string type = itemNode.Attributes["id"].Value;
                            string url = itemNode.Attributes["url"].Value;
                            string label = itemNode.Attributes["label"].Value;
                            string realLable = tempNode.Attributes["label"].Value;
                            string oldid = "";
                            if (itemNode.Attributes["oldid"] != null && !string.IsNullOrEmpty(itemNode.Attributes["oldid"].Value))
                            {
                                oldid = itemNode.Attributes["oldid"].Value;
                            }
                            //判断三级菜单权限
                            List<We7.CMS.Common.Permission> lsPermission = AccountHelper.GetPermissions("System.User", oldid);
                            if (lsPermission.Count > 0)
                            {
                %>
                <li <%=GetSelectClass(type)%>><a href="<%=url %>">
                    <%=realLable%></a></li>
                <%}
                        }
                    }
                %>
            </ul>
        </div>
    </div>
    <%}

            }
        }       
    %>
      <div class="box mb_1">
        <div class="tit">
            <strong>
                <a href="/">返回网站首页</a></strong></div>
    </div>
    <div class="box mb_1">
        <div class="tit">
            <strong>
                <a href="/user/logout.aspx?returnurl=/user/login.aspx">退出登录</a></strong></div>
         </div>
</div>
<!--头图菜单-->
<script>
    $(function () {
        $(".realLeft .box .tit").click(function () {
            $(this).toggleClass("select");

            $(this).siblings(".con").toggle();
        });
    })
</script>
