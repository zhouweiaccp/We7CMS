<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdviceProcessEx.aspx.cs"
    Inherits="We7.CMS.Web.Admin.AdviceProcessEx" %>

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
    <div id="mycontent">
         <% if (Permissions.Count == 0)
            { %>
           
           <img src="/Admin/images/logo_error.gif" />&nbsp;&nbsp;&nbsp;&nbsp;<b>您没有权限查看本页面！</b>
           
        <%}
            else
            { %>
        <div id="conbox">
            <dl>
                <dt>»反馈信息办理<br>
                 <WEC:MessagePanel ID="MessagePanel" runat="server">
                </WEC:MessagePanel>
                <div class="widget-place" id="widget-place-1" style="width:60%">
                            <div class="widget movable collapsable removable  closeconfirm" id="widget-guide">
                            <div class="widget-header">
                                <strong>详细信息</strong>
                            </div>
                            <div class="widget-content">
               <asp:PlaceHolder runat="server" ID="ModelDetails"></asp:PlaceHolder>   
               </div>
               </div>
        <div style="padding:0 5px;">
                           
               <FCKeditorV2:FCKeditor ID="fckContent" ToolbarSet="Basic" runat="server"
                                            Height="250px" Width="100%" BasePath="/admin/fckeditor/">
                                        </FCKeditorV2:FCKeditor> 
                       <div class="toolbar">
                        <asp:Button id="lnkProccess" CssClass="Btn" runat="server" onclick="lnkProccess_Click" Text="提交"></asp:Button>
                        <asp:Button id="lnkTransfer" CssClass="Btn" runat="server" onclick="lnkTransfer_Click" Text="转办"></asp:Button>
                        <asp:Button id="lnkRefuse" CssClass="Btn" runat="server" onclick="lnkRefuse_Click" Text="不受理"></asp:Button>
                        <asp:Button id="lnkBack" CssClass="Btn" runat="server" onclick="lnkBack_Click" Text="返回"></asp:Button>
                </div>
               </div>
         </div>
               <div class="widget-place" id="widget-place-2" style="width:40%">
               <div class="widget movable collapsable removable  closeconfirm" id="widget-RecentComments">
                            <div class="widget-header">
                                <strong>办理踪迹</strong>
                            </div>
                            <div class="widget-content">
                                <uc:AdviceHistory runat="server" id="AdviceHistory1"></uc:AdviceHistory>
                            </div>
                            </div>  
                            </div>
               </dt>
           </dl> 
        </div>
        <% } %>
    </div>
</asp:content>
