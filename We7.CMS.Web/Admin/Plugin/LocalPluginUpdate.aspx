<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LocalPluginUpdate.aspx.cs"
    Inherits="We7.CMS.Web.Admin.Plugin.LocalPluginUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <style>
        body
        {
        	padding:0;
        	margin:0;
        	font-size:12px;
        	font-family:宋体 Arial;
        }
        input
        {
        	font-size:12px;
        	font-family:宋体 Arial;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        <tr>
            <td>
                <asp:FileUpload ID="ZipFileUpload" runat="server" />    
            </td>
            <td>
                <asp:Button ID="UploadButton" runat="server" Text="更新插件" 
            onclick="UploadButton_Click" Height="22px" />
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
