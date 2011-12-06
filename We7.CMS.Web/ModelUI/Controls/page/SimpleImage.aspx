<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleImage.aspx.cs" Inherits="We7.CMS.Web.Admin.ContentModel.Controls.Page.SimpleImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>上传图片</title>
    <style>
        *{
            font-szie:12px;
            padding:0;
            margin:0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                <asp:FileUpload ID="fuImage" runat="server" Width="300px" onchange="imgCurrentImage.src=this.value" /><asp:Button ID="btnUpload"
                    runat="server" OnClick="btnUpload_Click" Text="上传" /><asp:Literal ID="ltlMsg" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="imgCurrentImage" runat="server" ImageUrl="~/ModelUI/skin/images/flower.jpg" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="ImageValue" runat="server" />
    </form>
</body>
</html>
