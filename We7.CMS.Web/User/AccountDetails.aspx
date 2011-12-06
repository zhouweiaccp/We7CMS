<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountDetails.aspx.cs"
    MasterPageFile="~/User/DefaultMaster/content.Master" Inherits="We7.CMS.Web.User.AccountDetails" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
    <script src="/Admin/Ajax/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/article.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/cookie.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/tags.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <form runat="server" id="form1">
    <div>
        <WEC:MessagePanel ID="Messages" runat="server">
        </WEC:MessagePanel>
    </div>
    <div class="realRight ml10">
        <div class="mybox">
            <div class="mytit">
                资料显示</div>
            <div class="con">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td valign="top">
                            <div style="border: solid #e5e5e5 1px; background: #f5f5f5; padding: 5px; float: left">
                                <div style="border: solid #f0f0f0 1px; background: #fff; float: left">
                                    <asp:Image ID="imgPhoto" runat="server" Width="100" Height="79" />
                                </div>
                            </div>
                        </td>
                        <td align="left">
                            <div style="width: 100%;">
                                <table align="left">
                                    <tbody id="tbDetails" runat="server">
                                        <tr>
                                            <td>
                                                登陆名：
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLoginName" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkbttnEdit" runat="server" Text="修改" OnClick="lnkbttnEdit_Click"></asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                姓名：
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFirstName" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblMiddleName" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblLastName" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                用户类型：
                                            </td>
                                            <td>
                                                <asp:Label ID="lblModel" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                用户组：
                                            </td>
                                            <td>
                                                <asp:Label ID="lblType" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                电子邮件：
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmail" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                状态：
                                            </td>
                                            <td>
                                                <asp:Label ID="lblState" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div>
                                <%--                    <WED:ContentModelControl ID="UserContentModelControl" runat="server" AppMode="BrowseSimple"
                        ColMode="Single" CssClass="contentForm" />--%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
