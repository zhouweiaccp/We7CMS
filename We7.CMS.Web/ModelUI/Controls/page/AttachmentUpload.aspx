<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttachmentUpload.aspx.cs" Inherits="We7.Model.UI.Controls.page.AttachmentUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>上传附件</title>
    <base target="_self">
    <link href="/ModelUI/skin/default.css" type="text/css" rel="Stylesheet" />
    <script type="text/javascript" src="/Admin/Ajax/jquery/jquery-1.3.2.min.js"></script>
    <script type="text/javascript">
        function setValue(value)
        {           
           var el= window.dialogArguments;
           el.value=value;
           window.close();
        }        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="2" cellspacing="1" class="table_form">
        <caption>
            附件上传</caption>
        <tr>
            <td>
                <asp:FileUpload ID="fuImage" runat="server" CssClass="file_style" />
                <asp:Button ID="bttnUpload" runat="server" Text="上传" CssClass="button_style" OnClick="bttnUpload_Click" />
                <asp:Literal ID="ltlMsg" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>