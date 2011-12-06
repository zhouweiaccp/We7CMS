<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticleProcess.aspx.cs"
    MasterPageFile="~/User/DefaultMaster/content.Master" Inherits="We7.CMS.Web.User.ArticleProcess" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="/favicon.ico" />
    <link rel="stylesheet" href="/Admin/theme/classic/css/global.css" type="text/css"
        media="all" />
    <link rel="stylesheet" href="/Admin/theme/classic/css/we7-admin.css" type="text/css"
        media="all" />
    <link rel="stylesheet" href="/Admin/theme/classic/css/colors-fresh.css" type="text/css"
        media="all" />
    <link rel="stylesheet" type="text/css" href="/Admin/theme/classic/css/main.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="/Admin/theme/classic/css/article.css"
        media="screen" />
    <script src="/admin/ajax/jquery/jquery.latest.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/admin/theme/classic/js/hoverIntent.js"></script>
    <script type="text/javascript" src="/admin/theme/classic/js/common.js"></script>
    <script src="/admin/cgi-bin/DialogHelper.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Admin/cgi-bin/search.js"></script>
    <script type="text/javascript" src="/Admin/ajax/jquery/jquery.DMenu.js"></script>
    <script type="text/javascript">
            /* <![CDATA[ */
            userSettings = {
                url: "/",
                uid: "1",
                time: "1235697253"
            }
            /* ]]> */
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <form runat="server" id="form1">
    <script type="text/javascript">
        function SelectAll(tempControl) {
            var theBox = tempControl;
            xState = theBox.checked;

            elem = theBox.form.elements;
            for (i = 0; i < elem.length; i++)
                if (elem[i].type == "checkbox" && elem[i].id != theBox.id) {
                    if (elem[i].checked != xState)
                        elem[i].click();
                }
        }
    </script>
    <div class="realRight ml10">
        <div class="mybox">
            <div class="mytit">
                文章待审核</div>
            <div class="con">
                <h2 class="title">
                    <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_home.gif" />
                    <asp:Label ID="NameLabel" runat="server" Text="文章审核">
                    </asp:Label>
                    <span class="summary">
                        <asp:Label ID="SummaryLabel" runat="server" Text="">
                        </asp:Label>
                    </span>
                </h2>
                <div class="toolbar">
                    <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="ArticleProcess.aspx" runat="server">
                刷新</asp:HyperLink>
                </div>
                <WEC:MessagePanel ID="Messages" runat="server">
                </WEC:MessagePanel>
                <div>
                    <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                        CssClass="List" GridLines="Horizontal">
                        <AlternatingRowStyle CssClass="alter" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle Width="5px" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:SelectAll(this);"
                                        AutoPostBack="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItem" runat="server" />
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="False"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID" HeaderText="标题" DataNavigateUrlFormatString="ArticleEdit2.aspx?id={0}"
                                DataTextField="Title" DataTextFormatString="{0}" Target="_blank" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    所属栏目
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# GetChannelText(Eval("ID").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    状态
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# GetProcessState(Eval("ID").ToString())%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    最后提交时间
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("Updated")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    操作
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <a href="/admin/manage/ProcessSign.aspx?id=<%# Eval("ID") %>&t=<%# Eval("Title")%>"
                                        target="_blank">签署意见</a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div class="pagination">
                    <p>
                        <WEC:Pager ID="Pager" PageSize="20" PageIndex="0" runat="server" OnFired="Pager_Fired" />
                    </p>
                </div>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
