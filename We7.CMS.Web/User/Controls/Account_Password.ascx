<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Account_Password.ascx.cs" Inherits="We7.CMS.Web.User.Controls.Account_Password" %>
<table>
    <tbody id="tbDetails" runat="server" >
        <tr>
            <th>
                原始密码：
            </th>
            <td>
                <asp:TextBox ID="txtPritivePassword" runat="server" TextMode="Password"  ></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtPritivePassword" ErrorMessage="请先输入原始密码！"></asp:RequiredFieldValidator>
                <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Text="原始密码错误！" 
                    Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                新密码：
            </th>
            <td>
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" ></asp:TextBox>
            </td>
            <td>
            
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtNewPassword" ErrorMessage="密码不能为空！"></asp:RequiredFieldValidator>
            
            </td>
        </tr>
        <tr>
            <th>
                确认新密码：
            </th>
            <td>
                <asp:TextBox ID="txtNewPasswordConfirm" runat="server" TextMode="Password" ></asp:TextBox>
            </td>
            <td>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="txtNewPassword" ControlToValidate="txtNewPasswordConfirm" 
                    ErrorMessage="二次密码输入不一致！"></asp:CompareValidator>
            </td>
        </tr>
    </tbody>
    <tbody id="tbEdit" runat="server" visible="true">
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