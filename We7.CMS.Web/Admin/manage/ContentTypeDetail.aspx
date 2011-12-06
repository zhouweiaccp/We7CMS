<%@ Page Language="C#" ValidateRequest="false"  AutoEventWireup="true" Codebehind="ContentTypeDetail.aspx.cs" Inherits="We7.CMS.Web.Admin.ContentTypeDetail"  %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script type="text/javascript">
function onCancelHyperLinkClick()
{
    window.close();
} 

function onSaveButtonClick() {
    document.getElementById("<%=SaveButton.ClientID %>").click();
}
 
    </script>

                 <h2 class="title" >
                            <asp:Image ID="LogImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />                            
                            <asp:Label ID="TitleLabel" runat="server" Text="编辑Config文件">
                            </asp:Label>
                            <span class="summary">
                                <asp:Label ID="SummaryLabel" runat="server" Text="创建一个新的Config配置文件">
                                </asp:Label>
                            </span>
                        </h2>
                        <div class="toolbar">
                            <asp:HyperLink ID="SaveHyperLink" NavigateUrl="javascript:onSaveButtonClick();"
                                runat="server">
                                保存</asp:HyperLink>
                            <asp:HyperLink ID="ReturnHyperLink" NavigateUrl="ContentTypeList.aspx" runat="server">
                                返回</asp:HyperLink>
                            <br />
                        </div>
                        
    <WEC:MessagePanel runat="Server" ID="Messages">
    </WEC:MessagePanel>
                    <table class='tree'>
                        <tr>
                            <td>
                                <asp:Label ID="ConfigNameLabel" runat="server" Text="Config文件名:" Visible="false">
                                </asp:Label>
                                <asp:TextBox ID="ConfigNameTextBox" runat="server"  Visible="false"></asp:TextBox>
                                <asp:Literal runat="Server" ID="ConfigFileExt" Text=".xml"  Visible="false"  ></asp:Literal></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="ConfigTextBox" runat="server" Columns="50" Rows="20" TextMode="MultiLine"
                                    Width="90%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="display: none">
                        <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
                        <asp:TextBox ID="OldFileTextBox" runat="server"></asp:TextBox>
                    </div>
</asp:Content>
