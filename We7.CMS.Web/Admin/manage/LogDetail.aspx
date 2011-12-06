<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogDetail.aspx.cs" Inherits="We7.CMS.Web.Admin.LogDetail" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">

                        <h2 class="title" >
                            <asp:Image ID="LogImage" runat="server" ImageUrl="~/admin/Images/icons_tools.gif" />
                            <asp:Label ID="NameLabel" runat="server" Text="">
                            </asp:Label>
                            <span class="summary">
                                <asp:Label ID="SummaryLabel" runat="server" Text="">
                                </asp:Label>
                            </span>
                        </h2>
                        <div class="toolbar">
                            <asp:HyperLink ID="ReturnHyperLink" runat="server" NavigateUrl="Logs.aspx">
                                <asp:Image ID="ReturnImage" runat="server" ImageUrl="~/admin/Images/icon_prev.gif" />
                                返回</asp:HyperLink>
                        </div>
                        <h3>
                            <span>日志详细信息</span>
                        </h3>
                        <asp:Label ID="ContentLabel" runat="server" Text="">
                                </asp:Label>
</asp:Content>
