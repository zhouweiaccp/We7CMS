<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeFile="PagedArticleList.Default.cs"
    Inherits="We7.CMS.Web.Widgets.PagedArticleListDefault" %>
<div class="<%=Css %>">
<div class="article_list ">
    <h3 <%=BackgroundIcon() %>>
        
            <%= Channel!=null ? Channel.Name: "" %></h3>
    <ul <%=SetBoxBorderColor() %>>
        <% foreach (Article article in Articles)
           {
         %>
        <li><a target="_self" href="<%=article.Url %>">
            <img alt="<%=article.Title %>" src="<%=article.GetTagThumbnail(ThumbnailTag) %>"></a>
            <br/>
            <a target="_self" href="<%=article.Url %>"><%=ToStr(article.Title, TitleLength)%></a>
            </li>
        <%} %>
        <div class="clear"></div>
    </ul>
    <div class="clear"></div>
    <%= Pager.PagedHtml%>
    <div class="clear"></div>
    <div class="underline_left"></div>
</div>
</div>