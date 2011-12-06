<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signin.aspx.cs" Inherits="We7.CMS.Web.Admin.Signin" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>登录<%=ProductBrand%>网站管理中心</title>
    <script src="/admin/cgi-bin/DialogHelper.js" type="text/javascript"></script>
    <script src="/admin/ajax/jquery/jquery.latest.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/admin/theme/classic/js/common.js"></script>

    <style type="text/css">
        *
        {
            padding-right: 0px;
            padding-left: 0px;
            padding-bottom: 0px;
            margin: 0px;
            padding-top: 0px;
        }
        BODY
        {
            color: #333;
            text-align: center;
            background-color: #FBFBFB;
        }
        BODY
        {
            font: 12px/1.5em Arial;
        }
        TD
        {
            font: 12px/1.5em Arial;
        }
        TH
        {
            font: 12px/1.5em Arial;
        }
        .loginbox
        {
            margin: 180px auto 60px;
            text-align: left;
        }
        TD
        {
        }
        .logo
        {
            padding-right: 70px;
            padding-left: 0px;
            background: url(/admin/images/login_logo.gif) no-repeat 100% 50%;
            padding-bottom: 30px;
            width: 226px;
            padding-top: 90px;
            text-align: right;
        }
        .logo P
        {
            margin: -50px 0px 0px;
        }
        table.loginform
        {
            margin-top: 10px;
        }
        .loginform TH
        {
            padding-right: 3px;
            padding-left: 3px;
            font-size: 12px;
            padding-bottom: 3px;
            padding-top: 3px;
            color: #666;
        }
        .loginform TD
        {
            padding-right: 3px;
            padding-left: 3px;
            font-size: 12px;
            padding-bottom: 3px;
            padding-top: 3px;
            color: #666;
        }
        .t_input
        {
            border-right: #eee 1px solid;
            padding-right: 2px;
            border-top: #666 1px solid;
            padding-left: 2px;
            padding-bottom: 3px;
            border-left: #666 1px solid;
            padding-top: 3px;
            border-bottom: #eee 1px solid;
        }
        .submit
        {
            background-image: url(/admin/images/login_button.jpg);
            background-repeat: no-repeat;
            background-color: Transparent;
            border-bottom-style: none;
            width: 73px;
            height: 26px;
            cursor: pointer;
            border-width: 0px;
        }
        .submit1
        {
            border-right: #666 1px solid;
            padding-right: 5px;
            border-top: #eee 1px solid;
            padding-left: 5px;
            font-size: 14px;
            background: #ddd;
            padding-bottom: 0px;
            border-left: #eee 1px solid;
            cursor: pointer;
            padding-top: 0px;
            border-bottom: #666 1px solid;
            height: 22px;
        }
        .footer
        {
            left: 50%;
            margin-left: -250px;
            width: 500px;
            color: #999;
            bottom: 10px;
            position: absolute;
        }
        A
        {
            color: #2366a8;
            text-decoration: none;
        }
        A:hover
        {
            text-decoration: underline;
        }
    </style>

    <script type="text/javascript">
        function checkTopWindow() {
            if (window != top) {
                top.location = window.location;
            }
        }
    </script>
</head>
<body id="classic">
    <form id="mainForm" runat="server">
    <%--    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>--%>
    <table class="loginbox" cellpadding="0" callspacing="0">
        <tbody>
            <tr>
                <td class="logo">
                    <p>
                        请输入你的用户名、密码，登录到本站，维护本站信息</p>
                </td>
                <td>
                    <table class="loginform" cellpadding="0" callspacing="0" style="height: 120px">
                        <tbody>
                            <tr>
                                <th>
                                    登录名：
                                </th>
                                <td>
                                    <asp:TextBox ID="LoginNameTextBox" runat="server" Columns="30"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    密 码：
                                </th>
                                <td>
                                    <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" Columns="30"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="tbAuthenCode2" runat="server" visible="false">
                                <th>
                                    验证码：
                                </th>
                                <td>
                                    <asp:TextBox ID="CodeNumberTextBox" runat="server" Columns="30" />
                                </td>
                                <td>
                                    <img alt="x" src="/Admin/cgi-bin/controls/CaptchaImage/SmallJpegImage.aspx" runat="server"
                                        id="Img2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="SubmitButton" runat="server" Text="   " OnClick="SubmitButton_Click"
                                        CssClass="submit" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="MessageLabel" runat="server" Text="" ForeColor="red"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    <p>
    </p>
    <p class="footer">
        <asp:Literal ID="CopyrightLiteral" runat="server"></asp:Literal>
    </p>
    </form>
</body>
</html>
