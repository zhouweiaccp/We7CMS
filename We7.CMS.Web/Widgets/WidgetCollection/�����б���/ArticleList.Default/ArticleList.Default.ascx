<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeFile="ArticleList.Default.cs"
    Inherits="We7.CMS.Web.Widgets.ArticleList_Default" %>
<div class="<%= CssClass %> <%=MarginCss %> ">
    <div class="area">
        <dl>
            <dt><h3 <%=BackgroundIcon() %>><a title="更多" target="_blank" href="<%=Channel.FullUrl %>">
                <%=Channel.Name %></a><a title="更多" target="_blank" href="<%=Channel.FullUrl %>">
                <img align="absmiddle" alt="更多" src="<%=ThemePath %>/images/more.gif" /></a></h3>
            </dt>
            <dd> <ul <%=SetBoxBorderColor() %>>
           <a title="更多" target="_blank" href="<%=Channel.FullUrl %>"><% foreach (Article article in Articles)
               { %></a>
            <li><a target="_self" href="<%=article.Url %>">
                <%=ToStr(article.Title,TitleLength) %></a><span class="datetime"><%=ToDateStr(article.Updated,DateFormat) %></span></li>
            <%} %>
        </ul></dd>
        </dl>    
    </div>
</div>