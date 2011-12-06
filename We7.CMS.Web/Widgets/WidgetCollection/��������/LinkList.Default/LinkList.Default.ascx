<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LinkList.Default.cs" Inherits="We7.CMS.Web.Widgets.LinkList_Default" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "友情链接", Author = "系统")]
    string MetaData;
</script>
<div class="<%=Css %>">
<div class="friend_link">
    <h3 <%=BackgroundIcon() %>>
        友情链接<a title="更多" target="_blank" href="/links/"><img align="absmiddle" alt="更多" src="<%=ThemePath %>/images/more.gif"></a></h3>
    <div <%=SetBoxBorderColor() %> class="friend_content">
        <ul class="friend_pic clear">
            <% foreach (Link link in Items)
               {
            %>
            <li><a target="_blank" href="<%=link.Url %>">
                <img border="0" title="<%=link.Title %>" alt="<%=link.Title %>" src="<%=link.Thumbnail %>"></a></li>
            <%} %>
        </ul>
        <div class="clear">
        </div>
        <ul class="friend_text">
            <% foreach (Link link in Items)
               {
            %>
            <li><a target="_blank" href="<%=link.Url %>">
                <%=link.Title %></a></li>
            <%} %>
            <div class="clear">
            </div>
        </ul>
    </div>
    <div class="clear">
    </div>
</div>
</div>