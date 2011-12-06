<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Statistic.Default.cs" Inherits="We7.CMS.Web.Widgets.Statistic_Default" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "网站统计")]
    string MetaData;

</script>
<div class="<%=Css %>">
<div class="statistics mtop10">
    <h3 <%=BackgroundIcon() %>>
        网站统计</h3>
    <div <%=SetBoxBorderColor() %> class="statistics_content">
     <%=GetVisitorCount() %>
    </div>
</div>
</div>