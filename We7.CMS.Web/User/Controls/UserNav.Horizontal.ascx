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
                ApplicationHelper.ResetApplication();
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


<!--LOGO及搜索-->
<!--头图菜单-->
<div class="realNav">
    <ul class="clearfix" style="height:33px;">
        <%
            string fileName = Server.MapPath("/user/Resource/menuItems.xml");
            XmlNodeList xmlRoot = XmlHelper.GetXmlNodeList(fileName, "/root/menuTree/menu");
            foreach(XmlNode node in xmlRoot)
            {
                XmlNode itemNode = XmlHelper.GetXmlNodeByAttribute(fileName, "/root/items/item", "id", node.Attributes["id"].Value);
                string type = itemNode.Attributes["id"].Value;
                string url = itemNode.Attributes["url"].Value;
                string label = itemNode.Attributes["label"].Value;
                string realLable = node.Attributes["label"].Value;
              %>  
               <li <%=GetSelectClass(type)%>><a href="<%=url %>"><%=realLable%></a></li>
            <%}                
        %>
        <li style="display:none;" class="navlir"><a href="/">链接</a> </li>        
    </ul>
</div>

<!--头图菜单-->