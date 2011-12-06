<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateGroupEdit.aspx.cs"
    MasterPageFile="~/admin/theme/classic/ContentNoMenu.Master" Inherits="We7.CMS.Web.Admin.TemplateGroupEdit" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
<link media="screen" rel="stylesheet" href="<%=AppPath%>/ajax/jquery/colorbox/colorbox.css" />
 <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
<script src="<%=AppPath%>/ajax/jquery/colorbox/jquery.colorbox-min.js"></script>
<script type="text/javascript" src="/Admin/cgi-bin/CheckBrowser.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#button").colorbox({ width: "50%", inline: true, href: "#templateConfigContainer", onClosed: function () { RefreshRpt(); } });
        $(".editAction").colorbox({ width: "70%", height: "80%", iframe: true });
        //$("#createVisualTemplate").colorbox({width:"70%", height:"80%", iframe:true});
        $("#createVisualTemplate").colorbox({ width: "50%", inline: true, href: "#VisualTemplateContainer", onClosed: function () { RefreshRpt(); } });
    });
    function importTemplate() {
   
        weShowModelDialog("/admin/Folder.aspx?folder=<%=path %>" ,null);
    }
</script>
<script type="text/javascript" src="/scripts/we7/we7.loader.js">
	$(document).ready(function(){
		we7('.popup').popup();
        we7('.hint').hint();
		we7('span[rel=xml-hint]').help();
	});
    </script>
<style>
#templateConfigContainer h3 { margin-top:15px;}
</style>
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="编辑模板组">
        </asp:Label>
        <span class="summary">
        </span>
    </h2>
    <div>
    <table><tr>
    <td><asp:Image runat="server" ID="GroupImage" Height="180" Width="220" /></td>
    <td valign="top"> <asp:Label ID="SummaryLabel" runat="server" Text="">
            </asp:Label>
            <br /><br />
            创建于<asp:Label ID="CreatedLabel" runat="server" Text="">
            </asp:Label>
             <div class="toolbar"
            <li class="smallButton4">
            <asp:HyperLink ID="EditHyperLink" class='editAction'  NavigateUrl="#"
                runat="server">
                 修改</asp:HyperLink></li></div>
            </td>
    </tr></table>
    </div>
    <div class="toolbar" style="display: <%=ToolbarVisible%>">
        <li class="smallButton6"  id="NewSpan">
        <div>
            <a href="#"  id="button" class="hint" title="新建普通模板">新建模板▼</a></div>
        </li>
         <li class="smallButton8" >
            <a href="TemplateFileDetail.aspx"  id="createVisualTemplate" class="hint" title="新建可视化模板，可视可拖拽">新建可视化模板▼</a>
        </li>
        <li class="smallButton4">
            <asp:HyperLink ID="NewSubHyperLink"   runat="server">
                 新建子模板</asp:HyperLink></li>
        <li class="smallButton4">
            <asp:HyperLink ID="NewMasterPageHyperLink"    runat="server">
                 新建母版</asp:HyperLink></li>
        <li class="smallButton6">
           <a href="Javascript:void(0);" onclick="importTemplate();">模板文件目录</a>
                 </li>
        <li class="smallButton8">
            <asp:LinkButton runat="server" ID="CreateMapLink" OnClick="CreateMapLink_Click" class="hint" title="新建模板后，请重建模板索引，以便列表调用">重建模板索引图</asp:LinkButton></li>
       <%-- <li class="smallButton8">
            <asp:LinkButton runat="server" ID="CreateControlIndex" OnClick="CreateControlIndex_Click">重建控件索引</asp:LinkButton></li>--%>
        <li class="smallButton8">
            <asp:LinkButton runat="server" ID="CreateWidgetIndex" OnClick="CreateWidgetIndex_Click" class="hint" title="新建或下载部件后，请重建部件索引，以便列表调用">重建部件索引</asp:LinkButton></li>
            <li class="smallButton8">
            <asp:LinkButton runat="server" ID="CreateThemeIndex" OnClick="CreateThemeIndex_Click" class="hint" title="新建主题后，请重建主题索引，以便列表调用">重建主题索引</asp:LinkButton></li>
        <li class="smallButton4">
            <asp:HyperLink ID="RefreshHyperLink"    runat="server">
                 刷新</asp:HyperLink></li>
    </div>
    <br />
    <div style="clear:both"></div>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
    <div>
        <asp:PlaceHolder runat="server" ID="ContentHolder"></asp:PlaceHolder>
    </div>
    <div style='display:none'>
    <div id="templateConfigContainer" >
    <asp:Literal runat="server" ID="TemplateConfigLiteral" ></asp:Literal>
    </div>
    <div id="VisualTemplateContainer" >
    <asp:Literal runat="server" ID="VisualTemplateConfigLiteral" ></asp:Literal>
    </div>
    </div>
</asp:Content>
