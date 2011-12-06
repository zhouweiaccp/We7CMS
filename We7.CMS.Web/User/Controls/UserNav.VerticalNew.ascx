<!--### name="菜单导航(纵向)" type="system" version="1.0" created="2010/06/09" 
desc="菜单导航（纵向）" author="We7 Group" ###-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.UserMenuProvider" %>
<%@ Import Namespace="We7.CMS.Common.Enum" %>
<%@ Import Namespace="We7.CMS.Common" %>
<div id="meunLeft" class="realLeft">
    <%       
        List<We7.CMS.Common.MenuItem> lsTopMenu = GetMenuTreeByType(((int)TypeOfMenu.TopMenu));
        foreach (We7.CMS.Common.MenuItem topItem in lsTopMenu)
        {
            if (RootID == topItem.ID)
            {
                List<We7.CMS.Common.MenuItem> lsGroupMenu = GetSubMenuList(lsTopMenu, topItem.ID);
                foreach (We7.CMS.Common.MenuItem groupMenu in lsGroupMenu)
                {
                    if (groupMenu.MenuType == (int)TypeOfMenu.GroupMenu)
                    {                                                       
    %>
    <div class="box mb_1">
        <div class="tit">
            <a class="setKG" href="####"></a><strong>
                <%=groupMenu.Name%></strong></div>
        <div class="con">
            <ul>
                <%
                        if (groupMenu != null && groupMenu.Items.Count > 0)
                        {
                            List<We7.CMS.Common.MenuItem> lsItems = GetSubMenuList(lsGroupMenu, groupMenu.ID);
                            if (lsItems != null && lsItems.Count > 0)
                            {
                                for (int j = 0; j < lsItems.Count; j++)
                                {
                                    We7.CMS.Common.MenuItem item = lsItems[j];
                                    if (item.MenuType == (int)TypeOfMenu.ReferenceMenu)
                                    {
                                        item = MenuHelper.GetMenuItem(item.ReferenceID);
                                    }
                                    string classStr = "";
                                    if (item.ID == ActiveMenuID)
                                    {
                                        classStr = " class='select' ";
                                    }%>
                                <li <%=classStr%>><a href="<%=item.Href %>">
                                    <%=item.Name%></a></li>                                                
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
        } 
    %>
    <div class="box mb_1">
        <div class="tit">
            <strong><a href="/">返回网站首页</a></strong></div>
    </div>
    <div class="box mb_1">
        <div class="tit">
            <strong><a href="/user/logout.aspx?returnurl=/user/login.aspx">退出登录</a></strong></div>
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
