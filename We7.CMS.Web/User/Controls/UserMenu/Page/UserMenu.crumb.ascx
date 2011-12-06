<!--#### name="当前位置" type="system" version="1.0" created="2009/11/30" 
desc="会员菜单的当前位置显示" author="We7 Group" #####-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.UserMenuProvider" %>
<script runat="server">
    public string Separator;
</script>
<span class="UserMenu_crumb_<%= CssClass %>"><strong>当前位置：</strong>
    <%
        if (ActiveMenu != null && !string.IsNullOrEmpty(ActiveMenu.Title))
        {
    %>
    <%=ActiveMenu.Title%><%=Separator%><%=ActiveSubMenu.Title %>
    <%}
    %>
</span>