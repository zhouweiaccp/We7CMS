<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LatestNews.LatestTopPicture.cs"
    Inherits="We7.CMS.Web.Widgets.LatestNews_LatestTopPictureEx" %>
<div class="<%=Css %>">
    <div class="content">
        <div class="news">
            <ul>
                <%for (int i = 0; i < ChannelList.Count; i++)
                  {
                      Channel channel = ChannelList[i]; string sxc = "";%>
                <% 
                  string tmp = string.Empty;
                  if (i == 0) tmp = "on";
                %>
                <li class=" <%=tmp %>"><a href="<%=channel.FullUrl %>">
                    <%=channel.Name%></a></li>
                <%} %>
            </ul>
        </div>
        <div class="news_list">
            <!--区块一开始-->
            <div class="news_list_01">
                <%for (int i = 0; i < ChannelList.Count; i++)
                  {%>
                <ul class="list_01" style='display: <%=i==0?"block":"none"%>;'>
                    <% foreach (Article article in ChannelList[i].Articles)
                       { %>
                    <li><span class="datetime">
                        <%=ToDateStr(article.Updated, DateFormat)%></span> <a target="_self " href="<%=article.Url %>">
                            <font style="">
                                <%=ToStr(article.Title, TitleLength)%></font></a> </li>
                    <%} %>
                </ul>
                <%} %>
            </div>
            <!--区块一结束-->
            <div class="news_flash">
                <embed height="165" width="226" src="/Widgets/WidgetCollection/文章列表类/LatestNews.LatestTopPictureEx/swf/usualfocus.swf"
                    wmode="opaque" flashvars="borderwidth=226&amp;borderheight=145&amp;textheight=20&amp;interval_time=6&amp;<%=FlashSlideData %>"
                    menu="false" quality="high" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer">
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="news_list_bottom">
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $("div.news ul").tabs("div.news_list_01 ul", { tabs: 'li', current: 'on', event: 'mouseover' });
    })
</script>
