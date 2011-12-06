<%@ Page Language="C#" AutoEventWireup="true" Codebehind="TemplateUpload.aspx.cs"
    Inherits="We7.CMS.Web.Admin.TemplateUpload" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
<script type="text/javascript">
 function ItemSave()
        {
            var ItemSaveBtn=document.getElementById("<%=UploadButton.ClientID%>");
            ItemSaveBtn.click();
        }

        function ItemSaveButton() {
            var ItemSaveBtn = document.getElementById("<%=SaveButton.ClientID%>");
            ItemSaveBtn.click();
        }
             </script>
                           <h2 class="title">
                            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_look.gif" />
                            <span>上传模板</span>
                        </h2>
                        <div class="toolbar">
                            <asp:HyperLink ID="BackHyperLink" NavigateUrl="TemplateGroups.aspx" runat="server">
                                <asp:Image ID="BackImage" runat="server" ImageUrl="~/admin/Images/icon_prev.gif" />
                                返回</asp:HyperLink>
                        </div>
                        <asp:Panel runat="server" ID="UploadPanel">
                            <div>
                                <h4>
                                    第一步: 选择一个模板数据包：</h4>
                                <asp:FileUpload ID="DataControlFileUpload" runat="server" Style="width: 300px" />
                                <br />
                                <span>点击“ </span>
                                    <asp:HyperLink ID="DeleteHyperLink" runat="server" NavigateUrl="javascript:ItemSave();">
                                    <asp:Image ID="DeleteImage" runat="server" ImageUrl="~/admin/Images/icon_fx.jpg" />
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
                                        <asp:BoundField DataField="Name" DataFormatString="{0}" HeaderText="名称" />
                                        <asp:BoundField DataField="Description" DataFormatString="{0}" HeaderText="描述" />
                                        <asp:BoundField DataField="FileName" HeaderText="文件" />
                                        <asp:BoundField DataField="Created" HeaderText="创建日期" />
                                    </Columns>
                                </asp:GridView>
                                <span>点击“</span>
                                <asp:HyperLink ID="SaveHyperLink" runat="server" NavigateUrl="javascript:ItemSaveButton();">
                                    <asp:Image ID="SaveImage" runat="server" ImageUrl="~/admin/Images/icon_save.gif" />
                                    保存</asp:HyperLink>
                                <span>”保存数据控件。</span>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="DonePanel">
                        <div>
                        <h4>
                        <asp:Label ID="DoneLabel" runat="server" Text=""></asp:Label></h4>
                        <asp:HyperLink ID="MakeTPGHyperLink" runat="server" NavigateUrl="~/admin/TemplateGroups.aspx" Visible="false" Target="_blank">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/admin/Images/logo_setting.jpg" />
                                    立即设置</asp:HyperLink>
                                    
                                   </div> 
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
