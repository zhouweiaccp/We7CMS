<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProccessMsg.aspx.cs" Inherits="We7.CMS.Web.Admin.ProccessMsg" %>

<%@ Register Src="controls/AdviceHistory.ascx" TagName="AdviceHistory" TagPrefix="uc" %>
<%@ Register Assembly="FCKeditor.net" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:content id="We7Content" contentplaceholderid="MyContentPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
    <link rel="stylesheet" type="text/css" media="screen" href="<%=ThemePath%>/css/jquery.easywidgets.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="<%=ThemePath%>/css/mywidgets.css" />
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="/Admin/Images/icons_comment.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="安装插件">
        </asp:Label>
        <span class="summary"><span id="Span1"></span>
            <asp:Label ID="SummaryLabel" runat="server" Text="通过安装插件，可能轻易扩展网站后台及前台功能。">
            </asp:Label>
        </span>
    </h2>
    <div id="Div1">
    <div id="conbox">
    <dl>
        <dt>»信息提示<br></dt>
        <table>
            <tr>
                <td>
                    <% if (Request["type"] == "1")
                       { %>
                       <img src="/Admin/images/logo_check.gif" />
                    <%}
                       else
                       { %>
                        <img src="/Admin/images/logo_error.gif" />
                    <%} %>
                </td>
                <td>
                    <%=Msg %><a href="<%=BackUrl %>">返回</a>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    
                </td>
            </tr>
        </table>
       </dl> 
    </div>
</asp:content>
