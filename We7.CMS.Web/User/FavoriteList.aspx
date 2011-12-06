<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FavoriteList.aspx.cs" MasterPageFile="~/User/DefaultMaster/content.Master"
    Inherits="We7.CMS.Web.User.FavoriteList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
    <style>
        td
        {
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <form runat="server" id="form1">
    <div class="realRight ml10">
        <div class="mybox">
            <div class="mytit">
                收藏夹</div>
            <div class="con">
                <div>
                    <table>
                        <tr>
                            <td>
                                个性标签：
                            </td>
                            <td>
                                <asp:DataList ID="dlTagList" runat="server" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <a href="FavoriteList.aspx?tag=<%# Eval("Tag")%>">
                                            <%# Eval("Tag") %></a>&nbsp;
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:GridView ID="gvList" runat="server" AllowPaging="true" PageSize="2" AutoGenerateColumns="false"
                    Width="100%" OnPageIndexChanging="gvList_PageIndexChanging" DataKeyNames="FavoriteID"
                    OnRowDeleting="gvList_RowDeleting" OnRowEditing="gvList_RowEditing" ShowHeader="true">
                    <HeaderStyle Font-Size="14px" BackColor="#f0f0f0" Height="25px" />
                    <Columns>
                        <asp:TemplateField HeaderText="缩略图">
                            <ItemStyle Width="70px" />
                            <ItemTemplate>
                                <a target="_blank" href='<%# Eval("Url") %>'>
                                    <%# Eval("Thumbnail") == null ? "暂无" : "<img Height='100px' Width='100px' src='" + Eval("Thumbnail") + "' alt='" + Eval("Title") + "' />"%>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField DataTextField="Title" DataNavigateUrlFields="Url" Target="_blank"
                            ItemStyle-HorizontalAlign="Center" DataNavigateUrlFormatString="{0}" HeaderText="标题"
                            ItemStyle-Width="250px" />
                        <asp:HyperLinkField DataTextField="Description" DataNavigateUrlFields="Url" Target="_blank"
                            DataNavigateUrlFormatString="{0}" HeaderText="描述" ItemStyle-HorizontalAlign="Left" />
                        <asp:TemplateField HeaderText="操作">
                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEdit" CommandName="Edit" runat="server">编辑</asp:LinkButton>
                                <asp:LinkButton ID="lbAdd" CommandName="Delete" runat="server">删除</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <div style="float: right">
                            第<asp:Label ID="lblPageIndex" runat="server" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1  %>' />页
                            共/<asp:Label ID="lblPageCount" runat="server" Text='<%# ((GridView)Container.Parent.Parent).PageCount  %>' />页
                            <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" CommandArgument="First"
                                CommandName="Page" Text="首页" />
                            <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" CommandArgument="Prev"
                                CommandName="Page" Text="上一页" />
                            <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" CommandArgument="Next"
                                CommandName="Page" Text="下一页" />
                            <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" CommandArgument="Last"
                                CommandName="Page" Text="尾页" />
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        document.forms["aspnetForm"].action = '<%= Request.Url %>';
    </script>
</asp:Content>
