<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CommentDetail.aspx.cs"
    Inherits="We7.CMS.Web.Admin.Addins.CommentDetail" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
<script src="<%=AppPath%>/cgi-bin/setday.js" type="text/javascript"></script>
        <h2>
          <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_comment.gif" />
                            <asp:Label ID="Label1" runat="server" Text="管理评论">
                            </asp:Label>
                           <span class="summary">
                                <asp:Label ID="SummaryLabel" runat="server" Text="">
                                </asp:Label>
                            </span>  
                        </h2>
                        <div class="toolbar">
                            <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="" runat="server">
                                <asp:Image ID="RefreshImage" runat="server" ImageUrl="~/admin/Images/icon_refresh.gif" />
                                刷新</asp:HyperLink>
                            <span>|</span>
                        </div>
                        <asp:Panel ID="MessagePanel" runat="Server">
                            <asp:Image ID="WarningImage" runat="server" ImageUrl="/admin/images/icon_warning.gif" />
                            <asp:Label ID="MessageLabel" runat="server" Text=""></asp:Label>
                        </asp:Panel>
                        <table>
                            <tr>
                                <th style="width: 20%">
                                    项目</th>
                                <th>
                                    内容</th>
                            </tr>
                            <tr>
                                <td>
                                    评论发表人</td>
                                <td>
                                    <asp:Literal ID="AuthorLiteral" runat="server"></asp:Literal></td>
                            </tr>
                           <tr>
                                <td>
                                    发表人IP</td>
                                <td>
                                 <asp:Literal ID="IPLiteral" runat="server"></asp:Literal>
                                </td>
                            </tr>
                           <tr>
                                <td>
                                    评论内容</td>
                                <td>
                                <asp:Literal ID="ContentLiteral" runat="server"></asp:Literal>
                                </td>
                            </tr>
                           <tr>
                                <td>
                                    评论状态</td>
                                <td>
                                <asp:Literal ID="StateLiteral" runat="server"></asp:Literal>
                                </td>
                            </tr>
                           <tr>
                                <td>
                                    评论时间</td>
                                <td>
                                <asp:Literal ID="TimeLiteral" runat="server"></asp:Literal>
                                </td>
                            </tr>    
                        </table>
</asp:Content>