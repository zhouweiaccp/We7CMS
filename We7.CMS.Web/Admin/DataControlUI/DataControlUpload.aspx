<%@ Page Language="C#" AutoEventWireup="true" Codebehind="DataControlUpload.aspx.cs"
    Inherits="We7.CMS.Web.Admin.DataControlUpload" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">

                        <h2>
                            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_attach.gif" />
                            <span>上传数据控件</span>
                        </h2>
                        <div class="toolbar">
                            <asp:HyperLink ID="BackHyperLink" NavigateUrl="DataControls.aspx" runat="server">
                                <asp:Image ID="BackImage" runat="server" ImageUrl="~/admin/Images/icon_cancel.gif" />
                                返回</asp:HyperLink>
                        </div>
                        <asp:Panel runat="server" ID="UploadPanel">
                            <div>
                                <h4>
                                    第一步: 选择一个控件数据包：</h4>
                                <asp:FileUpload ID="DataControlFileUpload" runat="server" Style="width: 300px" />
                                <br />
                                <span>点击“ </span>
                                <asp:HyperLink ID="DeleteHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.UploadButton.click();">
                                    <asp:Image ID="DeleteImage" runat="server" ImageUrl="~/admin/Images/icon_attach.gif" />
                                    分析数据</asp:HyperLink>
                                <span>”开始进行处理。 </span>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="ReviewPanel" Visible="false">
                            <div>
                                <h4>
                                    第二步：确认控件数据信息</h4>
                                <asp:GridView ID="DataControlsGridView" runat="server" AutoGenerateColumns="False"
                                    ShowFooter="True">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="名称" />
                                        <asp:BoundField DataField="Description" HeaderText="描述" />
                                        <asp:BoundField DataField="FileName" HeaderText="文件" />
                                        <asp:BoundField DataField="Author" HeaderText="作者" />
                                        <asp:BoundField DataField="Created" HeaderText="创建时间" />
                                    </Columns>
                                </asp:GridView>
                                <span>点击“</span>
                                <asp:HyperLink ID="SaveHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.SaveButton.click();">
                                    <asp:Image ID="SaveImage" runat="server" ImageUrl="~/admin/Images/icon_save.gif" />
                                    保存</asp:HyperLink>
                                <span>”保存数据控件。</span>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="DonePanel">
                        </asp:Panel>
                        <div style="display: none">
                            <asp:Button ID="UploadButton" runat="server" Text="Upload" OnClick="UploadButton_Click" />
                            <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
                        </div>
                    <asp:Panel ID="MessagePanel" runat="server" Visible="false">
                        <asp:Image ID="MessageImage" runat="server" ImageUrl="~/admin/Images/icon_warning.gif" />
                        <asp:Label ID="MessageLabel" runat="server" Text="Ready to process."></asp:Label>
                    </asp:Panel>
</asp:Content>
