<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeFile="ChannelMenu.TopMenu.cs" Inherits="We7.CMS.Web.Widgets.ChannelMenu_TopMenu" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "网站顶部菜单显示两级", Author = "系统")]
    [RemoveParameter("Tag")]
    string MetaData;
</script>
<div class="<%=Css %>">
<div class="menu">
    <div class="main_menu" id="menubox">
        <ul>
            <% if (IsSelected(null))
               { %>
            <li class="current">
                <%}
               else
               { %>
                <li>
                    <%} %>
                    <a target="_self" href="/"><span><em>网站首页</em></span></a></li>
                <% foreach (Channel ch in FirstLevelChannels)
                   {
                       if (IsSelected(ch))
                       { %>
                <li class="current">
                    <%}
                       else
                       { %>
                    <li>
                        <%} %>
                        <a target="_self" href="<%=ch.RealUrl %>"><span><em>
                            <%=ch.Name %></em></span></a></li>
                    <%} %>
        </ul>
    </div>
    <div class="sub_menu">
        <div class="sub_center">
            <ul>
                <% foreach (Channel ch in SecondLevelChannels)
                   { %>
                <li><a target="_self" href="<%=ch.RealUrl %>">
                    <%=ch.Name %></a></li>
                <%} %>
            </ul>
        </div>
    </div>
</div>
</div>