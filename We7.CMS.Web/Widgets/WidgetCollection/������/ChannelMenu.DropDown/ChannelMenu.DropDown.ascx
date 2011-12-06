<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeFile="ChannelMenu.DropDown.cs"
    Inherits="We7.CMS.Web.Widgets.ChannelMenu_DropDown" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "网站浮动菜单显示两级", Author = "系统")]
    string MetaData;
</script>
<script type="text/javascript" src="Js/jquery.downhx.js"></script>
<div class="<%= Css %>">
    <div id="nav">
        <ul>
        <% if (IsSelected(null))
               { %>
            <li><span class="nav_on"><a target="_self" href="/" id="product" class="active">
                <%}
               else
               { %>
                <li><span class="nav"><a target="_self" href="/" id="product">
                    <%} %>
                    <span><em>网站首页</em></span></a></span></li>
            <%for (int i = 0; i < TopChannels.Count; i++)
              { %>
              <% if(TopChannels[i].HaveSon){ %>
            <li onmouseover="document.getElementById('son<%=i%>').style.display='block'" onmouseout="document.getElementById('son<%=i%>').style.display='none'">
                <%}
               else
               {%>
                <li>
                    <%}%>
                    <% if (IsSelected(TopChannels[i]))
                       {%>
                    <span class="nav_on"><a id="product" class="active"><%=TopChannels[i].Name%></a></span>
                    <%}
                       else
                       {%>
                    <span class="nav"><a id="product" href="<%=TopChannels[i].FullUrl %>"><%=TopChannels[i].Name%></a></span>
                    <%}%>
                    <%if (TopChannels[i].HaveSon)
                      {
                          if (TopChannels[i].MenuIsSelected)
                          {%>
                    <div class="sonall son<%=i%>" id="son<%=i%>">
                        <%}
                else
                {%>
                        <div class="sonall son<%=i%>" id="son<%=i%>" style="display: none;">
                            <%}%>
                            
                            <%for (int j = 0; j < TopChannels[i].SubChannels.Count; j++)
                              {%>
                              <span>
                                <img src="/Widgets/WidgetCollection/导航类/ChannelMenu.DropDown/images/menu_l.gif" /></span>
                            <span><a href="<%=TopChannels[i].SubChannels[j].FullUrl %>">
                                <%=TopChannels[i].SubChannels[j].Name%></a></span>
                            <%}%>
                        </div>
                        <%}%>
                </li>
                <%}%>
        </ul>
    </div>
</div>
