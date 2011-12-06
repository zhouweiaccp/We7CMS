<!--### name="菜单导航（横向）" type="system" version="1.0" created="2010/06/09" 
desc="菜单导航(横向)" author="We7 Group" ###-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.UserMenuProvider" %>
<%@ Import Namespace="We7.CMS.Common.Enum" %>
<%@ Import Namespace="We7.CMS.Common" %>
<!--LOGO及搜索-->
<!--头图菜单-->
<div class="realNav">
    <ul class="clearfix" style="height: 33px;">
        <%
            //We7.CMS.Common.MenuItem
            List<We7.CMS.Common.MenuItem> lsTopMenu = GetMenuTreeByType(((int)TypeOfMenu.TopMenu));
            foreach (We7.CMS.Common.MenuItem item in lsTopMenu)
            {
                string classStr = "";
                if (item.ID == RootID)
                {
                    classStr = " class='select' ";
                }
        %>
        <li <%=classStr%>><a href="<%=item.Href %>"><%=item.Name%></a></li>
           <%}%>
        <li style="display: none;" class="navlir"><a href="/">链接</a> </li>
    </ul>
</div>
<!--头图菜单-->
