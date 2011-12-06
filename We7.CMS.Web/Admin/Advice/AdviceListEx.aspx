<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdviceListEx.aspx.cs" Inherits="We7.CMS.Web.Admin.AdviceListEx" %>

<asp:content id="We7Content" contentplaceholderid="MyContentPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />

    <script src="<%=AppPath%>/cgi-bin/article.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/cgi-bin/cookie.js" type="text/javascript"></script>


    <script src="<%=AppPath%>/cgi-bin/tags.js" type="text/javascript"></script>

    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="/Admin/Images/icons_comment.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="反馈列表">
        </asp:Label>
        <span class="summary"><span id="navChannelSpan"></span>
            <asp:Label ID="SummaryLabel" runat="server" Text="反馈列表信息描述">
            </asp:Label>
        </span>
    </h2>
    <div id="mycontent">
        <% if (Permisstions.Count == 0)
           { %>
           
           <img src="/Admin/images/logo_error.gif" />&nbsp;&nbsp;&nbsp;&nbsp;<b>您没有权限查看本页面！</b>
           
        <%}
           else
           { %>
        <div class="Tab menuTab">
            <ul class="Tabs">
                <asp:Label runat="server" ID="MenuTabLabel"></asp:Label>
            </ul>
        </div>
        <br />
        <div class="clear">
        </div>
        <div id="rightWrapper" style="width:100%">
            <div id="container" style="width:100%">
                <asp:PlaceHolder runat="server" ID="ContentHolder"></asp:PlaceHolder>
            </div>
        </div>
        <%} %>
    </div>
</asp:content>
