<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ArticleList.SideList.cs"
    Inherits="We7.CMS.Web.Widgets.ArticleList_SideList" %>
<script type="text/C#" runat="server">    
    [RemoveParameter("DateFormat")]
    string MetaData;
</script>
<div class="<%=Css %>">
    <div class="notice ">
        <h3 <%=BackgroundIcon() %>>
            <a target="_blank" title="更多" href="<%=Channel.RealUrl %>">
                <img align="absmiddle" alt="更多" src="<%=ThemePath %>/images/more.gif"></a>
            <%=Channel.Name %></h3>
        <ul <%=SetBoxBorderColor() %>>
            <% foreach (Article article in Articles)
               { %>
            <li><a target="_blank" href="<%=article.Url %>">
                <%=ToStr(article.Title,TitleLength) %></a> </li>
            <%} %>
        </ul>
    </div>
</div>
