<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Recommand.Newest.cs"
 Inherits="We7.CMS.Web.Widgets.Recommand_Newest" %>

<script type="text/C#" runat="server">
    [Description("最新推荐")]
    [We7.CMS.WebControls.Core.ControlDescription(Name = "RecommandNewest控件", Author = "老莫"
        , Created = "2011/03/22", Desc = "最新推荐控件", Version = "v1.0")]
    string MetaData;
</script>
<div class="<%=Css %>">
<div class="month_tip ">
    <h3 <%=BackgroundIcon() %>>
        最新推荐</h3>
    <ul <%=SetBoxBorderColor() %>>
        <% foreach (Article article in Articles)
                   {%>
                <li><a target="_self" href="<%=article.Url %>">
                        <%=ToStr(article.Title,TitleLength) %></a></li>
                <%} %>
    </ul>
</div>
</div>