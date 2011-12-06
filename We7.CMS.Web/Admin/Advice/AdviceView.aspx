<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdviceView.aspx.cs" Inherits="We7.CMS.Web.Admin.AdviceView" %>

<%@ Register Src="controls/AdviceHistory.ascx" TagName="AdviceHistory" TagPrefix="uc" %>
<asp:content id="We7Content" contentplaceholderid="MyContentPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
    <link rel="stylesheet" type="text/css" media="screen" href="<%=ThemePath%>/css/jquery.easywidgets.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="<%=ThemePath%>/css/mywidgets.css" />
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="/Admin/Images/icons_comment.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="安装插件">
        </asp:Label>
        <span class="summary"><span id="navChannelSpan"></span>
            <asp:Label ID="SummaryLabel" runat="server" Text="通过安装插件，可能轻易扩展网站后台及前台功能。">
            </asp:Label>
        </span>
    </h2>
    <div id="mycontent">
    <div id="conbox">
    <dl>
        <dt>»信息查看<br>
        <div class="widget-place" id="widget-place-1" style="width:60%">
                    <div class="widget movable collapsable removable  closeconfirm" id="widget-guide">
                    <div class="widget-header">
                        <strong>详细信息</strong>
                    </div>
                    <div class="widget-content">
       <asp:PlaceHolder runat="server" ID="ModelDetails"></asp:PlaceHolder>   
       </div>
       </div>
                    <div class="widget movable collapsable removable  closeconfirm" id="Div2">
                    <div class="widget-header">
                        <strong>办理结果</strong>
                    </div>
                    <div class="widget-content">
                    
                    <%
                        if (Replies != null && Replies.Count > 0)
                        {
                            foreach (We7.CMS.Common.AdviceReplyInfo r in Replies)
                            { %>
                   <table cellpadding="0" cellspacing="0" width="100%" border="0">
    <tr>
        <td class="title" colspan="3">
            <b style="float:left">状态：<%=Advice.StateText%></b><b style="float:right">办理人：<%=GetNameByUserID(r.UserID)%>&nbsp;&nbsp;&nbsp;&nbsp;时间：<%=FormatDate(r.Created)%></b>
        </td>
    </tr>
</table>
<hr />
<table cellpadding="0" cellspacing="0" width="100%" border="1">
    <tr>
        <td class="remarkBody" colspan="3">
            <div class="his">
                <%=r.Content%>
            </div>
        </td>
    </tr>
</table>
<%}
                        }
                        else
                        { %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
    <tr>
        <td class="title" colspan="3">
            <b style="float:left">当前状态：<%=Advice.StateText%></b>
        </td>
    </tr>
</table>
                        <%} %>
       </div>
       </div>
               <div class="toolbar">
               <asp:Button id="lnkBack" CssClass="Btn" runat="server" onclick="lnkBack_Click" Text="返回" />
        </div>
       </div>
       <div class="widget-place" id="widget-place-2" style="width:30%">
       <div class="widget movable collapsable removable  closeconfirm" id="widget-RecentComments">
                    <div class="widget-header">
                        <strong>办理踪迹</strong>
                    </div>
                    <div class="widget-content">
                        <uc:AdviceHistory runat="server" id="AdviceHistory1"></uc:AdviceHistory>
                    </div>
                    </div>  
                    </div>
       </dt></dl> 
    </div>
</asp:content>
