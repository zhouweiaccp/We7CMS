<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/theme/classic/content.Master" AutoEventWireup="true" CodeBehind="AdvanceEdit.aspx.cs" Inherits="We7.CMS.Web.Admin.ContentModel.AdvanceEdit" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
<h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="高级修改"></asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="直接修改模型文件">
            </asp:Label>
        </span>
    </h2>
      <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
    <div>
<asp:TextBox ID="ModelXMLTextBox" TextMode="MultiLine" Rows="40" Width="700" runat="server"></asp:TextBox>

</div>
<div class="toolbar">
        <asp:HyperLink ID="SaveHyperLink" NavigateUrl="javascript:SubmitClick();" runat="server">
            保存
        </asp:HyperLink>
         <asp:HyperLink ID="ReturnHyperLink" NavigateUrl="Models.aspx" runat="server">
            返回列表
        </asp:HyperLink>
    </div>
    <div style="display: none">
       <asp:Button ID="btnSaveField" runat="server" Text="保存" OnClick="SubmitButton_Click"/>
    </div>

    <script type="text/javascript">
        function SubmitClick() {
            var submitBtn = document.getElementById("<%=btnSaveField.ClientID %>");
            submitBtn.click();
        }
    </script>
</asp:Content>
