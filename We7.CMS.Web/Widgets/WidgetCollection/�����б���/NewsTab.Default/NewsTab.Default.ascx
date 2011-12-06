<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeFile="NewsTab.Default.cs"
    Inherits="We7.CMS.Web.Widgets.NewsTab_Default" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "新闻Tab选项卡", Author = "西部动力")]
    string MetaData;
</script>
<script type="text/javascript">
    function nTabs(tabObj, obj) {
        var tabList = document.getElementById(tabObj).getElementsByTagName("li");
        for (i = 0; i < tabList.length; i++) {
            if (tabList[i].id == obj.id) {
                document.getElementById(tabObj + "_Title" + i).className = "active";
                document.getElementById(tabObj + "_Content" + i).style.display = "block";
            } else {
                var temp = document.getElementById(tabObj + "_Title" + i)
                if (temp != null) {
                    document.getElementById(tabObj + "_Title" + i).className = "normal";
                    document.getElementById(tabObj + "_Content" + i).style.display = "none";
                }
            }
        }
    }
</script>
<div class="<%=Css %> floatLeft">
    <% string iResult = this.ClientID;%>
    <div class="n4Tab" id="n4Tab<%=iResult%>">
        <div class="TabTitle" <%=BackgroundIcon() %>>
            <%for (int i = 0; i < ChannelList.Count; i++)
              {
                  Channel channel = ChannelList[i]; string sxc = ""; %>
            <% if (i == 0) { sxc = "active"; } else { sxc = "normal"; }%>
            <li id="n4Tab<%=iResult%>_Title<%=i%>" class="<%=sxc%>" onmouseover="nTabs('n4Tab<%=iResult%>',this);">
                <a href="<%=channel.FullUrl %>">
                    <%=channel.Name%></a></li>
            <%} %>
            <div class="clear">
            </div>
        </div>
        <%for (int i = 0; i < ChannelList.Count; i++)
          {
              string style = "";%>
        <% if (i == 0) { style = "display:block"; } else { style = "display:none"; }%>
        <div id="n4Tab<%=iResult%>_Content<%=i%>" style="<%= style%>; <%=SetBoxBorderColor()%>"
            class="TabContent">
            <% foreach (Article article in ChannelList[i].Articles)
               { %>
            <li><a target="_self" href="<%=article.Url %>">
                <%=ToStr(article.Title, TitleLength)%></a><span class="datetime"><%=ToDateStr(article.Updated, DateFormat)%></span></li>
            <%} %>
        </div>
        <%} %>
    </div>
</div>
