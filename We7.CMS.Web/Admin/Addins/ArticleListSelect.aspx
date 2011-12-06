<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ArticleListSelect.aspx.cs"
    Inherits="We7.CMS.Web.Admin.Addins.ArticleListSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
   <base target="_self" /> 
    <style type="text/css"> 
.toolbar {background: #f2f2f2;color:#666;padding: 4px 2px 4px 7px;border: 1px solid #ddd;margin: 0 0 1em}
	.toolbar p {position:relative;text-align:right}
	.toolbar p a:link, .toolbar p a:visited, .toolbar p a:hover, .toolbar p a:active {text-decoration:none;background:#fff;padding:2px 5px;border: 1px solid #ccc;font-size= 80%}
	.toolbar p a:hover {background:#c00;color:#fff}
	.toolbar p span {text-decoration:none;background:#fff;padding:2px 5px;border: 1px solid #ccc;color:#ccc}
	.toolbar * {margin:0}
	.toolbar h4 {margin-top:-1.45em;padding:0;border:none}
	</style>
       <link rel="stylesheet" type="text/css" href=<%=ThemePath%>/css/main.css media="screen" />
<link rel="stylesheet" type="text/css" href=<%=ThemePath%>/css/print.css media="print"  />
    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>

    <script type="text/javascript">
    function add()
    {
        window.quoteArticles.addArticles();
    } 

    </script>

</head>
<body id="classic">
    <form id="mainForm" runat="server">
        <div class="toolbar">
            <asp:HyperLink ID="AddHyperLink" runat="server" NavigateUrl="javascript:add();" Font-Size="Small">
                <asp:Image ID="CancelImage" runat="server" ImageUrl="~/admin/Images/additem.png" />
                Ìí¼Ó</asp:HyperLink>
            <span>| </span>
            <asp:HyperLink ID="CancelHyperLink" runat="server" NavigateUrl="javascript:window.close();">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/admin/Images/icon_cancel.gif" />
                È¡Ïû</asp:HyperLink>
        </div>
        <table class="tree">
            <tr>
                <td>
                    <iframe id="ifChannels" src="/admin/cgi-bin/controls/iChannels.aspx?oid=<%=QuoteOwnerID %>"
                        width="250" height="300" frameborder="0" scrolling="yes"></iframe>
                    <iframe id="quoteArticles" src="/admin/cgi-bin/controls/iArticles.aspx" width="350" height="300"
                        frameborder="0" scrolling="yes"></iframe>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
