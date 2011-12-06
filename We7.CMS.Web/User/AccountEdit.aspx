<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountEdit.aspx.cs" MasterPageFile="~/User/DefaultMaster/content.Master"
    Inherits="We7.CMS.Web.User.AccountEdit" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
    <script src="/Admin/Ajax/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/article.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/cookie.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/tags.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <div style="float: left;">
        <form runat="server" id="form1">
        <div class="realRight ml10">
            <div class="mybox">
                <div class="mytit">
                    资料修改</div>
                <div class="con">
                    <div id="mycontent">
                        <div class="Tab menuTab">
                            <ul class="Tabs" style="font-size: 12px;">
                                <asp:Label runat="server" ID="MenuTabLabel"></asp:Label>
                            </ul>
                        </div>
                        <div class="clear">
                        </div>
                        <div id="rightWrapper">
                            <div id="container">
                                <asp:PlaceHolder runat="server" ID="ContentHolder"></asp:PlaceHolder>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </form>
    </div>
</asp:Content>
