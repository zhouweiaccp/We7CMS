<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/ContentNoMenu.Master" AutoEventWireup="true" CodeBehind="TemplateBindTo.aspx.cs" Inherits="We7.CMS.Web.Admin.TemplateBindTo" Title="指定模板的默认匹配" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
<style>
h3{margin-top:15px!important;}
#bindList
{
	font-size:18px;  color:Blue; 
	text-align:center;	margin-top:20px;
}
 LABEL.block_info {
	MARGIN-LEFT: 5px; BACKGROUND-COLOR: #fffad6;
	display:inline;
}
#bindList li { list-style-type:none;}
#bindList a:link, #bindList a:visited 
{
	color:Red;
	font-size:12px;
}

#bindList a:hover, #bindList a:active {
background:none repeat scroll 0 0 #333366;
color:#FFFFFF;
}
</style>
<script type="text/javascript">
function closeMe()
{
    window.opener.location.href = window.opener.location.href; 
    if (window.opener.progressWindow) 
    { 
        window.opener.progressWindow.close(); 
    } 
    window.close(); 
}
</script>
<script src="/admin/template/js/bind.js" type="text/javascript"></script>
<h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="">
        </asp:Label>
</h2>
<div id="bindList" style="margin-bottom:30px;">
     <asp:Literal ID="bindListLitiral" runat="server"></asp:Literal>
</div>
  <img src="/admin/images/bulb.gif" align="absmiddle"/> <LABEL 
  class=block_info>下面为所有可用的模板默认指定页，点击即可指定。指定后在浏览前台对应页面时就会以此模板的样式进行展示。</LABEL> 

<div style="margin:5px 10px;">
    <asp:Literal ID="TemplateTypeLiteral" runat="server"></asp:Literal>
</div>
<br />
<%--<div class="toolbar" style="text-align:center; width:100%;" >
    <li class="smallButton8" style="display:block; float:none">
        <a href="javascript:closeMe()"> 关闭</a>
     </li>
</div>--%>
                        <br />
</asp:Content>
