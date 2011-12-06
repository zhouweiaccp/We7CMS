<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Export.aspx.cs" Inherits="We7.Model.UI.Export" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form runat="server" id="form1">
    <asp:TextBox ID="txtTable" runat="server"></asp:TextBox>
    <asp:Button ID="bttnExport" runat="server" OnClick="bttnExport_Click" Text="导出数据" />
</form>
</body>
</html>
