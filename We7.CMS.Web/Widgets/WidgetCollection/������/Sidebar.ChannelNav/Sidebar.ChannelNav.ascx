<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Sidebar.ChannelNav.cs" Inherits="We7.CMS.Web.Widgets.Sidebar_ChannelNav" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "侧栏导航控件", Author = "系统")]
    string MetaData;
    
</script>
<div class="<%= Css %>">
<div class="subnav">
    <h3 <%=BackgroundIcon() %>>
        栏目导航</h3>
    <ul <%=SetBoxBorderColor() %>>
        <% if (ChannelChildren != null && ChannelChildren.Count > 0){ %>
            <% foreach (Channel ch in ChannelChildren){ %>
                <li><a target="_self" href="<%=ch.RealUrl %>">
                    <%= ch.Name %>
                    </a></li>           
            <% } %>
        <% } %>
    </ul>
</div>
</div>