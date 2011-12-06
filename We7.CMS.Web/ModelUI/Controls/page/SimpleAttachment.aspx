<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleAttachment.aspx.cs"
    Inherits="We7.Model.UI.Controls.page.SimpleAttachment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>上传附件</title>
    <base target="_self">
    <link href="/ModelUI/skin/default.css" type="text/css" rel="Stylesheet" />
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
    <table border="0" align="left">
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <%# GetFileName(Container.DataItem) %>
                    </td>
                    <td>
                        <asp:ImageButton ImageUrl="~/Admin/images/icon_cancel.gif" CommandArgument='<%# Container.DataItem %>'
                            ID="btnDel" runat="server" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <asp:HiddenField ID="AttachmentValue" runat="server" />
    </form>
</body>
</html>
