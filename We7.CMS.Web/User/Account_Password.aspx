<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Account_Password.aspx.cs"
    MasterPageFile="~/User/DefaultMaster/content.Master" Inherits="We7.CMS.Web.User.Account_Password" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">

    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
    <script src="/Admin/Ajax/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/article.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/cookie.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/tags.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <form id="Form2" method="post" runat="server">
    <div class="realRight ml10">
        <div class="mybox">
            <div class="mytit">
                修改登录密码</div>
            <div class="con">
                <div class="pCenter clearfix" id="pCenter">
                    <div class="at_tab_l clearfix">
                        <table class="basicInfor">
                            <tbody id="tbDetails" runat="server">
                                <tr>
                                    <th>
                                        原始密码：
                                    </th>
                                    <td>
                                        <span class="at_text t_1_d">
                                            <asp:TextBox ID="txtPritivePassword" runat="server" TextMode="Password"></asp:TextBox></span>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPritivePassword"
                                            ErrorMessage="请先输入原始密码！"></asp:RequiredFieldValidator>
                                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Text="原始密码错误！" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        新密码：
                                    </th>
                                    <td>
                                        <span class="at_text t_1_d">
                                            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox></span>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassword"
                                            ErrorMessage="密码不能为空！"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        确认新密码：
                                    </th>
                                    <td>
                                        <span class="at_text t_1_d">
                                            <asp:TextBox ID="txtNewPasswordConfirm" runat="server" TextMode="Password"></asp:TextBox></span>
                                    </td>
                                    <td>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtNewPassword"
                                            ControlToValidate="txtNewPasswordConfirm" ErrorMessage="二次密码输入不一致！"></asp:CompareValidator>
                                    </td>
                                </tr>
                            </tbody>
                            <tbody id="tbEdit" runat="server" visible="true">
                                <tr>
                                    <th>
                                    </th>
                                    <td style="width: 200px;" class="clearfix">
                                        <a href="javascript:void(0)" style="text-decoration: none;" onclick="javascript:document.getElementById('<%=bttnEdit.ClientID %>').click()"
                                            class="at_but b_1_y"><u></u>确认</a>
                                        <div style="display: none;">
                                            <asp:Button runat="server" ID="bttnEdit" Text="确认" OnClick="bttnEdit_Click" />
                                            <asp:Button runat="server" ID="bttnBack" Text="返回" OnClick="bttnBack_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
