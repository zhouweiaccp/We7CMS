
<%@ Page Language="C#" AutoEventWireup="true" Codebehind="DataControls.aspx.cs" Inherits="We7.CMS.Web.Admin.DataControls" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">

                        <h2   class="title">
                            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_look.gif" />
                            <asp:Label ID="NameLabel" runat="server" Text="数据控件管理">
                            </asp:Label>
                            <span class="summary">
                                <asp:Label ID="SummaryLabel" runat="server" Text="">
                                </asp:Label>
                            </span>
                        </h2>
                        <div class="toolbar2">
                        <li class="smallButton4">
                            <asp:HyperLink ID="UploadHyperLink" NavigateUrl="DataControlUpload.aspx" runat="server">
                                上传控件</asp:HyperLink>
                          </li>
                            <li  class="smallButton8">
                            <asp:HyperLink ID="DownLoadHyperLink" runat="server" NavigateUrl="DataControlDownload.aspx"
                                Width="125px"> 
                                打包下载全部控件</asp:HyperLink></li>
                            <li class="smallButton4">
                            <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="DataControls.aspx" runat="server">
                                刷新</asp:HyperLink></li>
                        </div>
                        <br />
                        <div>
                            <asp:GridView ID="DataControlsGridView" runat="server" AutoGenerateColumns="False"    ShowFooter="True"  CssClass="List"  GridLines="Horizontal">
                                <Columns>
                                    <asp:HyperLinkField DataNavigateUrlFields="FileName" DataNavigateUrlFormatString="DataControlDetail.aspx?file={0}"
                                        DataTextField="Description" DataTextFormatString="{0}" HeaderText="名称" />
                                    <asp:BoundField DataField="FileName" HeaderText="文件" />
                                    <asp:BoundField DataField="Created" HeaderText="创建时间" />
                                    <asp:HyperLinkField DataNavigateUrlFields="FileName" DataNavigateUrlFormatString="DataControlDelete.aspx?file={0}"
                                        DataTextField="FileName" DataTextFormatString="删除" />
                                </Columns>
                            </asp:GridView>
                        </div>
</asp:Content>
