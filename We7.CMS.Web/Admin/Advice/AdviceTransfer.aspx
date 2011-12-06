<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdviceTransfer.aspx.cs"
    Inherits="We7.CMS.Web.Admin.AdviceTransfer" %>

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
        <dt>»反馈转发办理<br></dt>            
     <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
    <b>转办单位：</b>
    <asp:RadioButtonList ID="rdAdviceTypeList" runat="server" RepeatColumns="9" RepeatDirection="Horizontal">
        </asp:RadioButtonList>
    <b>转办意见：<br /></b>
           <FCKeditorV2:FCKeditor ID="fckContent" ToolbarSet="Basic" runat="server" Height="250px" Width="60%" BasePath="/admin/fckeditor/">
           </FCKeditorV2:FCKeditor>
    <div>
            <asp:Button id="lnkTransfer" CssClass="Btn" runat="server" Text="提交" onclick="lnkTransfer_Click"></asp:Button>
            <asp:Button id="lnkBack"  CssClass="Btn" runat="server" Text="返回" onclick="lnkBack_Click"></asp:Button>
    </div>
       </dl> 
    </div>
</asp:content>
