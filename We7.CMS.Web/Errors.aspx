<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Errors.aspx.cs" Inherits="We7.CMS.Web.Errors" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>We7温馨提示</title>
    <style type="text/css">
        BODY
        {
            color: #333;
            background-color: #F9FEE8;
        }
        body, html
        {
            height: 98%;
        }
    #main{
            position:absolute;
            top:50%;
            left:50%;
            width:620px;
            height:500px;
            margin-top:-200px;
            margin-left:-300px;
            }
        div.error p
        {
            font-size: 14px;
            color: #BFC79E;
            line-height: 140%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="main">
        <table style="display: block; width: 620px; text-align: left; ">
            <tr>
                <td style="text-align: right">
                    <h2>
                        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/install/Images/error_img.png" />
                    </h2>
                </td>
                <td style="width: 10px">
                </td>
                <td style="vertical-align: top; padding: 5px; text-align: left;">
                    <br />
                    <h3 class="error">
                        <asp:Label ID="TitleLabel" runat="server" Text=""></asp:Label></h2>
                        <p style="color: #c00">
                            报告：</p>
                        <div class="error">
                            <asp:Literal ID="MessageLabel" runat="server"></asp:Literal>
                            <asp:HyperLink ID="HelpHyperLink" Target="_blank" runat="server" Visible="false">HyperLink</asp:HyperLink>
                        </div>
                        <p runat="server" id="DetailInfo">
                            <asp:HyperLink ID="ViewLogHyperlink" runat="server"></asp:HyperLink>
                        </p>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="border-top: dotted 1px #ccc; font-size: 14px; text-align: center;
                    padding: 15px">
                    <a href="javascript:history.back();">返回上一页</a> | <a href="/">返回首页</a> | <a href="/admin/">
                        返回后台</a> | <a href="http://we7.cn:8443/trac/newticket" target="_blank">报告Bug</a>
                    | <a href="http://help.we7.cn" target="_blank">帮助中心</a>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
