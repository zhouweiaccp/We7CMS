<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" MasterPageFile="~/User/DefaultMaster/content.Master"
    Inherits="We7.CMS.Web.User.UserInfo" %>

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
                        <a href="/Plugins/ShopPlugin/UI/UserInfo.aspx" class="at_tab_i at_current">基本资料</a>
                        <a href="/Plugins/ShopPlugin/UI/ContractInfo.aspx" class="at_tab_i">联系方式</a> <a href="/Plugins/ShopPlugin/UI/PhotoUpload.aspx"
                            class="at_tab_i">头像照片</a>
                    </div>
                  
                    <div class="at_tab_c">
                        <div class="at_tab_l">
                            <table class="basicInfor">
                                <tbody>
                                    <tr>
                                        <th>
                                            用户名：
                                        </th>
                                        <td>
                                            <strong>
                                                <asp:Literal runat="server" ID="ltlLoginName" runat="server"></asp:Literal></strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            真实姓名：
                                        </th>
                                        <td class="fc9">
                                            <span class="at_text t_1_d">
                                                <asp:TextBox ID="txtRealName" runat="server"></asp:TextBox></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            性别：
                                        </th>
                                        <td class="fc9">
                                            <asp:RadioButton ID="rdMale" runat="server" Text="男" GroupName="Sex" />
                                            <asp:RadioButton ID="rdFemale" runat="server" Text="女" GroupName="Sex" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            生日：
                                        </th>
                                        <td>
                                            <span class="at_text t_1_d">
                                                <asp:TextBox ID="txtBirthday" runat="server" Columns="30"></asp:TextBox>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            个人简介：
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtIntro" TextMode="MultiLine" runat="server" Width="300px" Height="50px"></asp:TextBox>
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
                   
                </div>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
