<%@ Page Title="" Language="C#" MasterPageFile="~/admin/theme/classic/content.Master" AutoEventWireup="true"  CodeBehind="AddFeedbackMenu.aspx.cs" Inherits="We7.CMS.Web.Admin.AddFeedbackMenu" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
 <script type="text/javascript">
        function SubmitClick() {
            var submitBtn = document.getElementById("<%=SubmitsButton.ClientID %>");
            submitBtn.click();
        }
    </script>
    
        <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="反馈模型自定义菜单"></asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="反馈模型菜单包括调查、留言等">
            </asp:Label>
        </span>
    </h2>
    <div class="toolbar">
        <asp:HyperLink ID="AddHyperLink" NavigateUrl="javascript:SubmitClick();" runat="server">
            生成菜单
        </asp:HyperLink>
         <asp:HyperLink ID="ReturnHyperLink" NavigateUrl="Menulist.aspx" runat="server">
            返回
        </asp:HyperLink>
    </div>
    <wec:messagepanel runat="Server" ID="Messages">
    </wec:messagepanel>
    <div style="display: <%= MenuVisble %>" >
         <table class="personalForm" cellpadding="0" cellspacing="0">
          <tr><td class="formTitle">
反馈模型选择：
  </td><td class="formValue" > <asp:DropDownList ID="MenuDropDownList" runat="server" style="width:100px;" >
            </asp:DropDownList>
                <input  class="button" id="SaveButton" runat="server" type="submit" value="自动生成初始化数据" onserverclick="InitButton_Click" />
           </td></tr>
           </table>
           </div>
           <div>
                       <table  class="personalForm" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="formTitle">
                        菜单名称：
                    </td>
                    <td class="formValue">
                        <asp:TextBox ID="TitleTextBox" runat="server" Columns="60"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TitleTextBox"
                            ErrorMessage="不能为空" ValidationGroup="SubmitButton"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        菜单描述：
                    </td>
                    <td class="formValue">
                        <asp:TextBox ID="DesTextBox" runat="server" Columns="60"></asp:TextBox><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator2" runat="server" ControlToValidate="DesTextBox" ErrorMessage="不能为空"
                            ValidationGroup="SubmitButton"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                       菜单位置：
                    </td>
                    <td class="formValue">
                        <asp:DropDownList ID="SecondIndexDropDownList" runat="server">
                        </asp:DropDownList> （菜单在此菜单之前）
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        菜单Url：
                    </td>
                    <td class="formValue">
                        <asp:TextBox ID="UrlTextBox" runat="server" Columns="60"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="UrlTextBox"
                            ErrorMessage="不能为空" ValidationGroup="SubmitButton"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
           </div>
            <div style="display: none">
        <asp:Button ID="SubmitsButton" runat="server" Text="Save" OnClick="SubmitButton_Click"
            ValidationGroup="SubmitButton" />
    </div>
</asp:Content>
