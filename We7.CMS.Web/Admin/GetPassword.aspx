<%@ Page Language="C#" AutoEventWireup="true" Codebehind="GetPassword.aspx.cs" Inherits="We7.CMS.Web.Admin.GetPassword" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <h2 class="title">
    <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_tools.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="获取密码"></asp:Label>
            <span class="summary">
                <asp:Label ID="SummaryLabel" runat="server" Text="取回用户密码"></asp:Label>
            </span>
        </h2>
        <p>
            <span >
                <table style="width: 295px; height: 156px;border-width:0px">
                    <tr>
                        <td style="text-align: right">
                            注册用户：
                        </td>
                        <td style="width: 197px">
                            <asp:TextBox ID="LoginNameTextBox" runat="server" Columns="40" 
                                Width="165px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="LoginNameTextBox"
                                ErrorMessage="请输入用户名">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            邮箱地址：
                        </td>
                        <td style="width: 197px">
                            <asp:TextBox ID="MailTextBox" runat="server" Columns="40"
                                Width="165px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="MailTextBox"
                                ErrorMessage="请输入您注册时填写的邮箱地址。">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="MailTextBox"
                                ErrorMessage="邮箱地址不合法！" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td style="height: 19px; text-align: right">
                            </td>
                        <td style="width: 197px; height: 19px">
                            </td>
                    </tr>
                    <tr>
                        <td style="height: 19px; text-align: right;">
                            </td>
                        <td style="width: 197px; height: 19px">
                            &nbsp;<asp:Button ID="SendButton" runat="server" OnClick="SendButton_Click" Text="发送密码到邮箱" /></td>
                    </tr>
                    <tr>
                        <td style="height: 42px">
                        </td>
                        <td style="width: 197px; height: 42px">
                            <asp:Label ID="MessageLabel" runat="server" ForeColor="red" Text=""></asp:Label></td>
                    </tr>
                </table>
            </span>
        </p>
        <div>
            &nbsp;<asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            &nbsp;&nbsp;<br />
            &nbsp;</div>
</asp:Content>
