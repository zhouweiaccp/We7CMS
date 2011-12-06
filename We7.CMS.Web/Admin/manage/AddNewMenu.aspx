<%@ Page Title="" Language="C#" MasterPageFile="~/admin/theme/classic/content.Master"
    AutoEventWireup="true" CodeBehind="AddNewMenu.aspx.cs" Inherits="We7.CMS.Web.Admin.AddNewMenu" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script type="text/javascript">
        function SubmitClick() {
            var submitBtn = document.getElementById("<%=SubmitsButton.ClientID %>");
            submitBtn.click();
        }
        function SaveClick() {
            var SaveBtn = document.getElementById("<%=SaveButton.ClientID %>");
            SaveBtn.click();
        }
        function ShowMenuTypePanel() {
            var objFirst = document.getElementById("firstDiv");
            var objSecond = document.getElementById("secondDiv");
            var submitBtn = document.getElementById("AddHyperLink");
            var saveBtn = document.getElementById("SaveHyperLink");
            var obj1 = document.getElementById('<%=MenuRadioButtonList.ClientID %>_0');
            var obj2 = document.getElementById('<%=MenuRadioButtonList.ClientID %>_1');
            if (obj1.checked) {
                objFirst.style.display = '';
                objSecond.style.display = 'none';
                submitBtn.style.display = '';
                saveBtn.style.display = 'none';
            }
            else {
                objFirst.style.display = 'none';
                objSecond.style.display = '';
                submitBtn.style.display = 'none';
                saveBtn.style.display = '';
            }
        }
    </script>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server"><%= MenuText %></asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server"><%= MenuLastText%>
            </asp:Label>
        </span>
    </h2>
    <div class="toolbar">
        <a id="AddHyperLink" href="javascript:SubmitClick();" style="display: <%= MenuMainvisble %>">
            保存</a> <a id="SaveHyperLink" href="javascript:SaveClick();" style="display: <%= MenuChildvisble %>">
                保存</a>
        <asp:HyperLink ID="ReturnHyperLink" NavigateUrl="Menulist.aspx" runat="server">
            返回
        </asp:HyperLink>
    </div>
    <WEC:MessagePanel runat="Server" ID="Messages">
    </WEC:MessagePanel>
    <div>
        <table class="personalForm" cellpadding="0" cellspacing="0" style="display: <%= Menuvisble %>">
            <tr>
                <td class="formTitle">
                    <strong>请选择菜单类型：</strong><br />
                </td>
                <td class="formValue">
                    <asp:RadioButtonList ID="MenuRadioButtonList" runat="server" RepeatDirection="Horizontal"
                        EnableViewState="true" AutoPostBack="false">
                        <asp:ListItem Selected="True" Value="1" onclick="ShowMenuTypePanel();">第一级主菜单设置</asp:ListItem>
                        <asp:ListItem Value="2" onclick="ShowMenuTypePanel();">第二级三级子菜单设置</asp:ListItem>
                    </asp:RadioButtonList>
            </tr>
        </table>
    </div>
    <div id="firstDiv" style="display: <%= MenuMainvisble %>">
        <h3 style="padding-left: 200px" style="display: <%= Menuvisble %>">
            一级主菜单设置：
        </h3>
        <table class="personalForm" cellpadding="0" cellspacing="0">
            <tr style="display: <%= Menuvisble %>">
                <td class="formTitle">
                    主菜单名称：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="MainTitleTextBox" runat="server" Columns="60"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="MainTitleTextBox"
                        ErrorMessage="不能为空" ValidationGroup="SubmitButton"></asp:RequiredFieldValidator>
                    （菜单全名）
                </td>
            </tr>
            <tr>
                <td class="formTitle">
                    菜单描述：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="MainDesTextBox" runat="server" Columns="60"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator10" runat="server" ControlToValidate="MainDesTextBox"
                        ErrorMessage="不能为空" ValidationGroup="SubmitButton"></asp:RequiredFieldValidator>
                    （显示在菜单中的文字）
                </td>
            </tr>
            <tr style="display: none;">
                <td class="formTitle">
                    菜单图标：
                </td>
                <td class="formValue">
                    <asp:FileUpload ID="IconFileUpload" runat="server" />(请上传.gif/.png格式图片)
                </td>
            </tr>
            <tr style="display: none;">
                <td class="formTitle">
                    菜单鼠标滑过（hover）图标：
                </td>
                <td class="formValue">
                    <asp:FileUpload ID="HoverIconFileUpload" runat="server" />(请上传.gif/.png格式图片)
                </td>
            </tr>
            <tr>
                <td class="formTitle">
                    菜单Url：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="MianUrlTextBox" runat="server" Columns="60"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="MianUrlTextBox"
                        ErrorMessage="不能为空" ValidationGroup="SubmitButton"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr  style="display:none;">
                <td class="formTitle">
                    菜单位置：
                </td>
                <td class="formValue">
                    <asp:DropDownList ID="DropDownListType" runat="server">
                    </asp:DropDownList>
                    （菜单在此菜单之前）
                </td>
            </tr>
        </table>
    </div>
    <div id="secondDiv" style="display: <%= MenuChildvisble %>">
        <h3 style="padding-left: 200px" style="display: <%= Menuvisble %>">
            二三级子菜单设置：
        </h3>
        <table class="personalForm" cellpadding="0" cellspacing="0">
            <tr style="display: <%= Menuvisble %>">
                <td class="formTitle">
                    菜单名称：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="TitleTextBox" runat="server" Columns="60"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TitleTextBox"
                        ErrorMessage="不能为空" ValidationGroup="SaveButton"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="formTitle">
                    菜单描述：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="DesTextBox" runat="server" Columns="60"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator2" runat="server" ControlToValidate="DesTextBox" ErrorMessage="不能为空"
                        ValidationGroup="SaveButton"></asp:RequiredFieldValidator>
                    （显示在菜单中的文字）
                </td>
            </tr>
            <tr>
                <td class="formTitle">
                    菜单位置：
                </td>
                <td class="formValue">
                    <asp:DropDownList ID="SecondIndexDropDownList" runat="server">
                    </asp:DropDownList>
                    （菜单在此菜单之前,创建后不可修改）&nbsp;
                </td>
            </tr>
            <tr>
                <td class="formTitle">
                    菜单Url：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="UrlTextBox" runat="server" Columns="60"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="UrlTextBox"
                        ErrorMessage="不能为空" ValidationGroup="SaveButton"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <asp:TextBox ID="DisplayTextBox" runat="server"></asp:TextBox>
        <asp:Button ID="SubmitsButton" runat="server" Text="Save" OnClick="SubmitButton_Click"
            ValidationGroup="SubmitButton" />
        <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click"
            ValidationGroup="SaveButton" />
    </div>
</asp:Content>
