<%@ Page Language="C#" AutoEventWireup="true" Codebehind="UpdatePassword.aspx.cs" Inherits="We7.CMS.Web.Admin.UpdatePassword" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
<script type="text/javascript" src="/Scripts/we7/we7.loader.js"></script>
    <h2 class="title">
    <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_tools.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="修改密码"></asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="修改登录用户密码"></asp:Label>
        </span>
    </h2>
    <p>
            <span >
                <table id="update-passowrd-form" style="width: 295px; height: 156px;border-width:0px">
                    <tr>
                        <td style="text-align: right">
                            登录名：
                        </td>
                        <td style="width: 197px">
                            <asp:TextBox ID="LoginNameTextBox" runat="server" Columns="40" 
                                Width="165px" required="required" errmsg="请输入用户名"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            密码：
                        </td>
                        <td style="width: 197px">
                            <asp:TextBox ID="PasswordTextBox" runat="server" Columns="40" TextMode="Password"
                                Width="165px" required="required" errmsg="请输入当前密码"></asp:TextBox>
						</td>
                    </tr>
                    <tr>
                        <td style="height: 19px; text-align: right">
                            新密码：</td>
                        <td style="width: 197px; height: 19px">
                            <asp:TextBox ID="NewPasswordTextBox" runat="server" Columns="40" TextMode="Password"
                                Width="165px" required="required"></asp:TextBox>
                         </td>
                    </tr>
                    <tr>
                        <td style="height: 19px; text-align: right">
                            再输入一遍：</td>
                        <td style="width: 197px; height: 19px">
                            <asp:TextBox ID="AgainPasswordTextBox" runat="server" Columns="40" TextMode="Password"
                                Width="165px" required="required"></asp:TextBox>
                    </tr>
                    <tr>
                        <td style="height: 19px; text-align: right">
                        </td>
                        <td style="width: 197px; height: 19px">
                            <asp:CheckBox ID="IsHashedPasswordCheckBox"  runat="server" Checked="True"></asp:CheckBox><label for="<%=IsHashedPasswordCheckBox.ClientID %>"> 密码加密</label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 19px; text-align: right;">
                            <asp:Button ID="UpdateButton" runat="server" OnClick="UpdateButton_Click" Text="修改密码" OnClientClick="return checkEnable();" /></td>
                        <td style="width: 197px; height: 19px">
                            &nbsp;<asp:Button ID="CancelButton" runat="server" OnClick="CancelButton_Click" Text="取消" CausesValidation="False" /></td>
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
<script type="text/javascript">
	function checkEnable() {
		var form = $("#update-passowrd-form");
		if (we7 && we7.fn.validate && we7(form).validate()) {
			return true;
		} else {
			return false;
		}
	}

	we7.load.ready(function () {
		var form = $("#update-passowrd-form"), newPw = $("#<%=NewPasswordTextBox.ClientID %>"), cfmNewPw = $("#<%=AgainPasswordTextBox.ClientID %>");
		we7(cfmNewPw).customValidator(function (validator) {
			var val = this.val();
			if (val !== newPw.val()) {
				return "两次输入的密码不一致";
			} else {
				return true;
			}
		});

		we7(form).attachValidator({
			inputEvent: null,
			errorInputEvent: null
		});
	});
</script>
</asp:Content>