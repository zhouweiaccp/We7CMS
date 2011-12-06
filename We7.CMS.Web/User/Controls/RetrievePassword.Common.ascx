<!--### name="密码找回控件" type="system" version="1.0" created="2010/06/18" 
desc="密码找回，author="We7 Group" ###-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.RetrievePasswordProvider" %>
<div class="RetrievePassword_Common_one_<%= CssClass %>">
    <div class="reg-form">
        <form runat="server" id="frRetrievePassword">
        <table cellpadding="0" align="center" cellspacing="0" border="0" width="100%" style="line-height: 40px;">
            <tr id="tr1">
                <th colspan="2" style="width: 440px;">
                    <span class="title">密码找回</span> &nbsp; &nbsp;
                </th>
                <td align="left">
                    &nbsp;
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlRetrieve" runat="server">
            <table cellpadding="0" align="center" cellspacing="0" border="0" width="100%" style="line-height: 40px;">
                <tr id="trUserName">
                    <th style="width: 160px;">
                        会&nbsp; 员&nbsp; 名：
                    </th>
                    <td align="left" style="width: 280px;">
                        <asp:TextBox ID="txtUserName" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td align="left">
                        <div id="dvUserName">
                        </div>
                    </td>
                </tr>
                <tr id="trEmail">
                    <th style="width: 150px;">
                        电子邮箱：
                    </th>
                    <td style="width: 200px;" align="left">
                        <asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td align="left">
                        <div id="dvEmail">
                        </div>
                    </td>
                </tr>
                <tr id="trValidate">
                    <th>
                        验&nbsp; 证&nbsp; 码：
                    </th>
                    <td align="left">
                        <table width="100%">
                            <tr>
                                <td style="width: 100px">
                                    <asp:TextBox ID="txtValidate" runat="server" MaxLength="5" Width="100px"></asp:TextBox>
                                </td>
                                <td align="left">
                                    <div id="dvValidate">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="left">
                        <img id="imgVerify" src="/Install/VerifyCode.aspx?" alt="看不清？点击更换" onclick="this.src=this.src+'?'" />
                    </td>
                </tr>
                <tr>
                    <th>
                    </th>
                    <td>
                        <asp:Button ID="bttnRetrieve" runat="server" OnClick="bttnRetrieve_Onclick" Text="确定"
                            CssClass="button" Height="30px" Width="80px" /><!--disabled="disabled"-->
                    </td>
                    <td align="left">
                        &nbsp;
                        <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlSendEmail" runat="server" Visible="false">
            <table width="81%" style="line-height: 32px" align="center">
                <tr>
                    <td style="width: 60px;" valign="top" align="right">
                        <img src="/Admin/images/success.jpg" />
                    </td>
                    <td align="left">
                        <strong>
                            <asp:Label ID="Label4" runat="server" Text="密码修改链接已经发送到您的注册邮箱，请点击其中的链接重新设置您的密码！"></asp:Label>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td style="width: 60px;" valign="top">
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlUpdate" runat="server">
            <table cellpadding="0" align="center" cellspacing="0" border="0" class="reg-form"
                style="line-height: 30px;">
                <tr id="tr2">
                    <th style="width: 120px; font-weight: bold;">
                        会员名：
                    </th>
                    <td align="left" style="width: 200px;">
                        <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>
                    </td>
                    <td align="left">
                    </td>
                </tr>
                <tr id="tr3">
                    <th>
                        新密码：
                    </th>
                    <td align="left">
                        <asp:TextBox ID="txtNewPWD" runat="server" Width="200px" TextMode="Password"></asp:TextBox>
                    </td>
                    <td align="left">
                    </td>
                </tr>
                <tr id="tr4">
                    <th>
                        确认新密码：
                    </th>
                    <td align="left">
                        <asp:TextBox ID="txtReNewPWD" runat="server" Width="200px" TextMode="Password"></asp:TextBox>
                    </td>
                    <td align="left">
                    </td>
                </tr>
                <tr>
                    <th>
                    </th>
                    <td>
                        <asp:Button ID="btnChangePWD" runat="server" OnClick="btnChangePWD_Onclick" Text="保存"
                            CssClass="button" Height="30px" Width="80px" /><!--disabled="disabled"-->
                    </td>
                    <td align="left">
                        <asp:Label ID="lblUpdateMessage" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlSuccess" runat="server" Visible="false">
            <table width="100%" style="line-height: 32px">
                <tr>
                    <td style="width: 60px;" valign="top" align="right">
                        <img src="/Admin/images/success.jpg" />
                    </td>
                    <td align="left">
                        <strong>
                            <asp:Label ID="Label1" runat="server" Text="恭喜，密码找回成功！"></asp:Label>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td style="width: 60px;" valign="top">
                    </td>
                    <td align="left">
                        <asp:Label ID="Label2" runat="server" Text="新密码已经发送到您的注册邮箱，请打开您的邮箱查看新密码然后到用户中心更改您的密码！"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlError" runat="server" Visible="false" Width="742">
            <table width="61%" style="line-height: 32px" align="center">
                <tr>
                    <td style="width: 60px;" valign="top" align="right">
                        <img src="images/ico_critical.gif" alt="错误" />
                    </td>
                    <td align="left">
                        <strong>
                            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                        </strong>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        </form>
    </div>
