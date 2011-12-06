<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ArticleList.2ColumnWidthImage.cs"
    Inherits="We7.CMS.Web.Widgets.ArticleList_2ColumnWidthImage" %>
<script type="text/C#" runat="server">
    [ControlDescription("图文混排+文字列表式显示")]
    [RemoveParameter("DateFormat")]
    string MetaData;   
</script>
<div class="<%=Css %>">
    <div class="area">
        <h3 <%=BackgroundIcon() %>>
            
                <%=Channel.Name %><a title="更多" target="_blank" href="<%=Channel.FullUrl %>"><img
                    align="absmiddle" alt="更多" src="<%=ThemePath %>/images/more.gif"></a></h3>
        <div class="tv_center">
            <% if (PicArticle != null)
               { %>
            <ul class="tv_pictrue" <%=SetBoxBorderColor() %>>
                <li class="tv_u_thumb"><a target="_self"  href="<%=PicArticle.Url %>">
                    <img height="120" width="160" alt="<%=PicArticle.Title %>" src="<%=PicArticle.GetTagThumbnail(ThumbnailTag) %>"></a>
                    </li>
                        <li>
                        <a target="_self" class="tv_u_thumb_title" href="<%=PicArticle.Url %>"><%=ToStr(PicArticle.Title, PicDescLength)%></a>
                        <br/><span class="description"><%=ToStr(PicArticle.Description, DescriptionLength)%></span>
                        </li>
                        <div class="clear"></div>
            </ul>
            <div class="clear">
            </div>
            <%} %>
            <ul class="tv_text" <%=SetBoxBorderColor() %>>
                <% foreach (Article article in Articles)
                   { %>
                <li><a target="_blank" href="<%=article.Url %>"><span style="font-weight: normal;
                    font-style: normal;">
                    <%=ToStr(article.Title,TitleLength) %></span></a></li>
                <%} %>
                <div class="clear">
                </div>
            </ul>
        </div>
    </div>
</div>
