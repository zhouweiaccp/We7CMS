<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractInfo.aspx.cs" MasterPageFile="~/User/DefaultMaster/content.Master"
    Inherits="We7.CMS.Web.User.ContractInfo" %>

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
                个人资料</div>
            <div class="con">
                <div class="pCenter" id="pCenter">
                    <div class="at_tab_t">
                        <a href="/Plugins/ShopPlugin/UI/UserInfo.aspx" class="at_tab_i">基本资料</a> <a href="/Plugins/ShopPlugin/UI/ContractInfo.aspx"
                            class="at_tab_i at_current">联系方式</a> <a href="/Plugins/ShopPlugin/UI/PhotoUpload.aspx"
                                class="at_tab_i">头像照片</a>
                    </div>
                    <form id="form2" runat="server">
                    <div class="at_tab_c">
                        <div class="at_tab_l">
                            <table class="basicInfor">
                                <tbody>
                                    <tr>
                                        <th>
                                            通讯地址：
                                        </th>
                                        <td>
                                            <span class="at_text t_1_d">
                                                <asp:TextBox ID="txtAddress" Width="400px" runat="server"></asp:TextBox></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            固定电话：
                                        </th>
                                        <td class="fc9">
                                            <span class="at_text t_1_d">
                                                <asp:TextBox ID="txtPrePhone" Width="30px" runat="server"></asp:TextBox>
                                            </span>- <span class="at_text t_1_d">
                                                <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            手机：
                                        </th>
                                        <td class="fc9">
                                            <span class="at_text t_1_d">
                                                <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            QQ：
                                        </th>
                                        <td>
                                            <span class="at_text t_1_d">
                                                <asp:TextBox ID="txtQQ" runat="server"></asp:TextBox>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            Email：
                                        </th>
                                        <td>
                                            <span class="at_text t_1_d">
                                                <asp:Literal ID="ltlEmail" runat="server"></asp:Literal>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                        </th>
                                        <td>
                                            <asp:Button ID="bttnSubmit" runat="server" CssClass="default_btn" Text="保存修改" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
