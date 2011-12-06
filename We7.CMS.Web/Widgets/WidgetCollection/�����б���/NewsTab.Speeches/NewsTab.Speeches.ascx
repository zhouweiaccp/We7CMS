<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeFile="NewsTab.Speeches.cs"
    Inherits="We7.CMS.Web.Widgets.NewsTab_Speeches" %>
<script type="text/C#" runat="server">
[ControlDescription(Desc = "显示指定栏目信息（纵向", Author = "西部动力")]
    string MetaData;    
</script>
<div class="<%=Css %>">
<div class="ttnew_right_ldjh">
    <h3 <%=BackgroundIcon() %>> 指定栏目</h3>
    <div class="ttnew_right_ldjh_nr">
        <div class="ttnew_right_ldjh_nr_1">
            <% foreach (Channel channel in ChannelList)
               { %>
            <div class="ttnew_right_ldjh_nr_11 " onmousemove="ChekTab('<%=channel.ID %>')">
                ·&nbsp;<a href=""><%=channel.Name%></a>&nbsp;·</div>
            <div class="ttnew_right_ldjh_nr_12">
                <ul>
                    <% foreach (Article article in channel.Articles)
                       { %>
                    <li><a target="_self" href="<%=article.Url %>">
                        <%=ToStr(article.Title, TitleLength)%></a><span class="datetime"><%=ToDateStr(article.Updated, DateFormat)%></span></li>
                    <%} %>
                </ul>
            </div>
            <%} %>
            <div class="clear"></div>
        </div>
    </div>
</div>
</div>