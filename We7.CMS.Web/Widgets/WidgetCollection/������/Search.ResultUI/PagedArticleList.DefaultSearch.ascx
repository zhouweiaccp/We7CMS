<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeFile="PagedArticleList.cs"  Inherits="We7.CMS.Web.Widgets.PagedArticleList" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "普通搜索列表", Author = "老孟")]
    string MetaData;
</script>
<div class="<%=CssClass %>">
<div class="article_list ">
    <h3 <%=BackgroundIcon() %>>搜索结果</h3>
    <ul <%=SetBoxBorderColor() %>>
        <% foreach (Article article in Articles)
           { %>
        <li><a target="_self" href="<%=article.Url %>">
            <%
               string title = ToStr(article.Title, TitleLength);
               if (!string.IsNullOrEmpty(KeyWord))
                   title = title.Replace(KeyWord, KeyWord);
            %>
            <%=title%></a><span class="datetime"><%=ToDateStr(article.Updated,DateFormat) %></span></li>
        <%} %>
        <div class="clear"></div>
    </ul>
    <div class="clear"></div>
    <%= Pager.PagedHtml%>
    <div class="clear"></div>
    <div class="underline_left"></div>
</div>
</div>