<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PictureArticleList.Default.cs" Inherits="We7.CMS.Web.Widgets.PictureArticleList_Default" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "推荐图片文章列表", Author = "老莫")]
    string MetaData;
</script>
<div class="<%=Css %>">
<div class="pic_article mtop10">
    <h3 <%=BackgroundIcon() %>>
        <%=Channel.Name??"图文推荐" %></h3>
    <ul <%=SetBoxBorderColor() %>>
        <% foreach (Article article in Articles)
           {
         %>
        <li><a target="_self" href="<%=article.Url %>">
            <img height="120" width="160" alt="<%=article.Title %>" src="<%=article.GetTagThumbnail(ThumbnailTag) %>"></a><span><a
                target="_self" href="<%=article.Url %>"><%=ToStr(article.Title, TitleLength)%></a></span></li>
        <%} %>
        <div class="clear">
        </div>
    </ul>

</div>
</div>