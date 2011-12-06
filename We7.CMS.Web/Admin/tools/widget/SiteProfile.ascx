<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteProfile.ascx.cs"
    Inherits="We7.CMS.Web.Admin.tools.widget.SiteProfile" %>
<div class="widget movable collapsable removable  closeconfirm" id="widget-siteprofiles">
    <div class="widget-header">
        <strong>概览</strong>
    </div>
    <div class="widget-content">
        <div class="inside">
            <table width="90%" style="text-align: center">
                <tr>
                    <td>
                        累计浏览量：<asp:Label ID="LabelTotalVisitors" runat="server" Text="LabelTotalVisitors"></asp:Label>
                    </td>
                    <td>
                        本月发表文章数：<asp:Label ID="LabelMonthArticles" runat="server" Text="LabelMonthArticles"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        今日浏览量：<asp:Label ID="TodayPVLabel" runat="server" Text="LabelTotalVisitors"></asp:Label>
                    </td>
                    <td>
                        本周发表文章数：<asp:Label ID="LabelWeekArticles" runat="server" Text="LabelWeekArticles"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        文章总数：<asp:Label ID="LabelTotalArticles" runat="server" Text="LabelTotalArticles"></asp:Label>
                    </td>
                    <td>
                        本月评论数：<asp:Label ID="LabelMonthComments" runat="server" Text="LabelMonthComments"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        评论总数：<asp:Label ID="LabelTotalComments" runat="server" Text="LabelTotalComments"></asp:Label>
                    </td>
                    <td>
                        本周评论数：<asp:Label ID="LabelWeekComments" runat="server" Text="LabelWeekComments"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
