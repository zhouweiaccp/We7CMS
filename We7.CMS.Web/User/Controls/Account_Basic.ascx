<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Account_Basic.ascx.cs"
    Inherits="We7.CMS.Web.User.Controls.Account_Basic" %>
<style>
    th
    {
        text-align: right;
        font-weight: bold;
    }
    td
    {
    }
</style>
<table>
    <tbody id="tbDetails" runat="server" visible="false">
        <tr>
            <th>
                登陆名：
            </th>
            <td>
                <asp:Label ID="lblLoginName" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="lnkbttnEdit" runat="server" Text="修改" OnClick="lnkbttnEdit_Click"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th>
                头像：
            </th>
            <td>
                <div style="border: solid #e5e5e5 1px; background: #f5f5f5; padding: 5px; float: left">
                    <div style="border: solid #f0f0f0 1px; background: #fff; float: left">
                        <asp:Image ID="imgHead" runat="server" Width="100" Height="79" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <th>
                姓名：
            </th>
            <td>
                <asp:Label ID="lblFirstName" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lblMiddleName" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lblLastName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                用户类型：
            </th>
            <td>
                <asp:Label ID="lblModel" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                用户组：
            </th>
            <td>
                <asp:Label ID="lblType" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                电子邮件：
            </th>
            <td>
                <asp:Label ID="lblEmail" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                状态：
            </th>
            <td>
                <asp:Label ID="lblState" runat="server"></asp:Label>
            </td>
        </tr>
    </tbody>
    <tbody id="tbEdit" runat="server" visible="true">
        <tr>
            <th>
                登陆名：
            </th>
            <td>
                <asp:Label ID="lblLoginName2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <th valign="top">
                头像：
            </th>
            <td valign="">
                <asp:Panel runat="server" ID="fileUpload" runat="server">
                    <asp:FileUpload ID="upload" runat="server" />
                    <asp:Button ID="bttnUpload" runat="server" Text="上传图片" OnClick="bttnUpload_Click" />
                </asp:Panel>
                <div style="border: solid #e5e5e5 1px; background: #f5f5f5; padding: 5px; float: left">
                    <div style="border: solid #f0f0f0 1px; background: #fff; float: left">
                        <asp:Image ID="imgHeader2" runat="server" Width="100" Height="79" Style="padding: 0;
                            margin: 0;" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <th>
                姓名：
            </th>
            <td>
                <asp:TextBox ID="txtFirstName" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtMiddleName" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                用户类型：
            </th>
            <td>
                <asp:Label ID="lblModel2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                用户组：
            </th>
            <td>
                <asp:Label ID="lblType2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                电子邮件：
            </th>
            <td>
                <asp:TextBox runat="server" ID="txtEmail"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                状态：
            </th>
            <td>
                <asp:Label ID="lblState2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
            </th>
            <td>
                <asp:Button runat="server" ID="bttnEdit" Text="保存" OnClick="bttnEdit_Click" />
                <asp:Button runat="server" ID="bttnBack" Text="返回" OnClick="bttnBack_Click" />
            </td>
        </tr>
    </tbody>
</table>
