<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataControlDownload.aspx.cs" Inherits="We7.CMS.Web.Admin.DataControlDownload" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">

                    <asp:Panel runat="Server" ID="ContentPanel">
                        <div id="breadcrumb">
                           <h2>
                            <asp:Image ID="TemplateImage" runat="server" ImageUrl="~/admin/Images/logo_task.gif" />
                            <span>下载数据组件"</span>
                            <asp:Label ID="TemplateGroupNameLabel" runat="server" Text="">
                            </asp:Label>
                            <span></span>
                          </h2>
                          <span><span>点击 “ </span>
                                <asp:HyperLink ID="DonwloadHyperLink" runat="server">
                                    <asp:Image ID="DonwloadImage" runat="server" ImageUrl="~/admin/Images/icon_donwload.gif" />
                                    下载数据组件</asp:HyperLink>
                            </span><span>”完成下载操作。</span>
                        </div>
                    
                    </asp:Panel>
</asp:Content>
