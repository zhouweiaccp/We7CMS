<!--#### name="无样式菜单" type="system" version="1.0" created="2009/11/30" 
desc="普通横菜单或竖菜单" author="We7 Group" #####-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.UserMenuProvider" %>
<a href="/">首页</a><a href="/user/index.aspx">会员中心</a>
<% if (Menus.Count > 0)
   {
       for (int i = 0; i < Menus.Count; i++)
       {%>
<%if (Menus[i].ID == ActiveMenuID) //菜单被选中的样式
  { %>
<span class="active">
    <%=Menus[i].Title%></span>
<%}
  else    //菜单未被选中的样式
  { %>
<a href="<% =Menus[i].Href %>" title="<%=Menus[i].Title %>">
    <%=Menus[i].Name%></a>
<%} %>
<%}
           }%>