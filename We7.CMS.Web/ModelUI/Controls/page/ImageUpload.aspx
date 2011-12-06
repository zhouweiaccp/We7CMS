<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageUpload.aspx.cs" Inherits="We7.CMS.Web.Admin.ContentModel.Controls.Page.ImageUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>上传图片</title>
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
            图片上传</caption>
        <tr>
            <td>
                <asp:FileUpload ID="fuImage" runat="server" CssClass="file_style" />
                <asp:Button ID="bttnUpload" runat="server" Text="上传" CssClass="button_style" OnClick="bttnUpload_Click" />
                <asp:Button ID="bttnOrigin" runat="server" Text="使用原图" CssClass="button_style" 
                    onclick="bttnOrigin_Click" />
                <asp:Button ID="bttnThumbnail" runat="server" Text="使用缩略图" 
                    CssClass="button_style" onclick="bttnThumbnail_Click" />
                <asp:Literal ID="ltlMsg" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                允许上传类型：jpg|jpep|gif|png|bmp<br />
                允许上传大小：1000 KB<br />
                缩略图生成方式：<asp:DropDownList ID="ddlThumbType" runat="server">
                    <asp:ListItem Text="指定高宽缩放,可能变形" Value="HW"></asp:ListItem>
                    <asp:ListItem Text="指定宽，高按比例" Value="W"></asp:ListItem>
                    <asp:ListItem Text="指定高，宽按比例" Value="H"></asp:ListItem>
                    <asp:ListItem Text="指定高宽裁减,不变形" Value="Cut"></asp:ListItem>
                </asp:DropDownList>
                <br />
                缩略图大小：宽
                <asp:TextBox ID="txtWidth" runat="server" Columns="3">142</asp:TextBox>&nbsp;px
                ，高
                <asp:TextBox ID="txtHeight" runat="server" Columns="3">112</asp:TextBox>
                &nbsp;px
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="imgThumb" runat="server" ImageUrl="~/ModelUI/skin/images/nopic.gif" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="ImageValue" runat="server" />
    </form>
</body>
</html>
