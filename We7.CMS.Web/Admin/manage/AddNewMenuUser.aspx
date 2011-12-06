<%@ Page Title="" Language="C#" MasterPageFile="~/admin/theme/classic/content.Master"
    AutoEventWireup="true" CodeBehind="AddNewMenuUser.aspx.cs" Inherits="We7.CMS.Web.Admin.AddNewMenuUser" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script type="text/javascript">
        String.prototype.replaceAll = function (s1, s2) {
            return this.replace(new RegExp(s1, "gm"), s2);
        }
        function SubmitClick() {
            var submitBtn = document.getElementById("<%=SubmitsButton.ClientID %>");
            submitBtn.click();
        }
        function SaveClick() {

            var SaveBtn = document.getElementById("<%=SaveButton.ClientID %>");
            SaveBtn.click();
        }
        function ChildSaveClick() {
            var submitBtn = document.getElementById("<%=btnChildSave.ClientID %>");
            submitBtn.click();
        }
        function ReferenceClick() {
            var SaveBtn = document.getElementById("<%=btnReferenceSave.ClientID %>");
            SaveBtn.click();
        }
        function ShowMenuTypePanel() {
            var objFirst = document.getElementById("firstDiv");
            var objSecond = document.getElementById("secondDiv");
            var objThird = document.getElementById("thirdDiv");
            var objFourth = document.getElementById("fourthDiv");

            var submitBtn = document.getElementById("AddHyperLink");
            var saveBtn = document.getElementById("SaveHyperLink");
            var childSaveBtn = document.getElementById("ChildSaveLink");
            var referenceBtn = document.getElementById("ReferenceSaveLink");


            var obj1 = document.getElementById('<%=MenuRadioButtonList.ClientID %>_0');
            var obj2 = document.getElementById('<%=MenuRadioButtonList.ClientID %>_1');
            var obj3 = document.getElementById('<%=MenuRadioButtonList.ClientID %>_2');
            var obj4 = document.getElementById('<%=MenuRadioButtonList.ClientID %>_3');


            if (obj1.checked) {
                objFirst.style.display = '';
                objSecond.style.display = 'none';
                objThird.style.display = 'none';
                objFourth.style.display = 'none';

                submitBtn.style.display = '';
                saveBtn.style.display = 'none';
                childSaveBtn.style.display = 'none';
                referenceBtn.style.display = 'none';
            }
            else if (obj2.checked) {
                objFirst.style.display = 'none';
                objSecond.style.display = '';
                objThird.style.display = 'none';
                objFourth.style.display = 'none';

                submitBtn.style.display = 'none';
                saveBtn.style.display = '';
                childSaveBtn.style.display = 'none';
                referenceBtn.style.display = 'none';
            }
            else if (obj3.checked) {
                objFirst.style.display = 'none';
                objSecond.style.display = 'none';
                objThird.style.display = '';
                objFourth.style.display = 'none';

                submitBtn.style.display = 'none';
                saveBtn.style.display = 'none';
                childSaveBtn.style.display = '';
                referenceBtn.style.display = 'none';
            }
            else {
                objFirst.style.display = 'none';
                objSecond.style.display = 'none';
                objThird.style.display = 'none';
                objFourth.style.display = '';

                submitBtn.style.display = 'none';
                saveBtn.style.display = 'none';
                childSaveBtn.style.display = 'none';
                referenceBtn.style.display = '';
            }
        }

        function ChangeInputBySelect() {
            var objDdlReferenceMenu = document.getElementById('<%=ddlReferenceMenu.ClientID %>');
            var selectValue = objDdlReferenceMenu.value;
            var selectText = objDdlReferenceMenu.options[objDdlReferenceMenu.selectedIndex].text;
            document.getElementById('<%=txtTitle_Reference.ClientID %>').value = selectText.replaceAll('├', '').replaceAll('─', '');
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
        <a id="AddHyperLink" href="javascript:SubmitClick();" style="display: <%= TopMenuVisible %>">
            保存</a> <a id="SaveHyperLink" href="javascript:SaveClick();" style="display: <%= GroupMenuVisible %>">
                保存</a> <a id="ChildSaveLink" href="javascript:ChildSaveClick();" style="display: <%= ChildMenuVisible %>">
                    保存</a> <a id="ReferenceSaveLink" href="javascript:ReferenceClick();" style="display: <%= ReferenceMenuVisible %>">
                        保存</a>
        <asp:HyperLink ID="ReturnHyperLink" NavigateUrl="UserMenulistNew.aspx" runat="server">
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
                        <asp:ListItem Selected="True" Value="1" onclick="ShowMenuTypePanel();">一级顶部菜单</asp:ListItem>
                        <asp:ListItem Value="2" onclick="ShowMenuTypePanel();">分组菜单</asp:ListItem>
                        <asp:ListItem Value="3" onclick="ShowMenuTypePanel();">子菜单</asp:ListItem>
                        <asp:ListItem Value="4" onclick="ShowMenuTypePanel();">引用菜单</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </div>
    <div id="firstDiv" style="display: <%=TopMenuVisible %>">
        <h3 style="padding-left: 200px">
            一级顶部菜单设置：
        </h3>
        <table class="personalForm" cellpadding="0" cellspacing="0">
            <tr>
                <td class="formTitle">
                    菜单名称：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="MainTitleTextBox" runat="server" Columns="60"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="MainTitleTextBox"
                        ErrorMessage="不能为空" ValidationGroup="SubmitButton"></asp:RequiredFieldValidator>（菜单全名）                    
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
            <!--<tr>
                <td class="formTitle">
                    菜单图标：
                </td>
                <td class="formValue">
                    <asp:FileUpload ID="IconFileUpload" runat="server" />(请上传.gif/.png格式图片)
                </td>
            </tr>
            <tr>
                <td class="formTitle">
                    菜单鼠标滑过（hover）图标：
                </td>
                <td class="formValue">
                    <asp:FileUpload ID="HoverIconFileUpload" runat="server" />(请上传.gif/.png格式图片)
                </td>
            </tr>-->
            <tr id="trIsGroupMenuUrl">
                <td class="formTitle">
                    菜单Url：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="MianUrlTextBox" runat="server" Columns="60"></asp:TextBox>
                </td>
            </tr>
            <tr id="trTopMenuPosition">
                <td class="formTitle">
                    菜单位置:
                </td>
                <td class="formValue">
                    <asp:DropDownList ID="DropDownListType" runat="server">
                    </asp:DropDownList>
                    （菜单在此菜单之前）
                </td>
            </tr>
        </table>
    </div>
    <div id="secondDiv" style="display: <%= GroupMenuVisible %>">
        <h3 style="padding-left: 200px">
            分组菜单设置：
        </h3>
        <table class="personalForm" cellpadding="0" cellspacing="0">
            <tr>
                <td class="formTitle">
                    分组菜单名称：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="txtTitle_Group" runat="server" Columns="60"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle_Group"
                        ErrorMessage="不能为空" ValidationGroup="SubmitButton"></asp:RequiredFieldValidator>
                    （菜单全名）
                </td>
            </tr>
            <tr>
                <td class="formTitle">
                    菜单描述：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="txtDes_Group" runat="server" Columns="60"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDes_Group"
                        ErrorMessage="不能为空" ValidationGroup="SubmitButton"></asp:RequiredFieldValidator>
                    （显示在菜单中的文字）
                </td>
            </tr>
            <tr id="trGroupMenuParent">
                <td class="formTitle">
                    父菜单:
                </td>
                <td class="formValue">
                    <asp:DropDownList ID="ddlParent" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div id="thirdDiv" style="display: <%= ChildMenuVisible %>">
        <h3 style="padding-left: 200px">
            子菜单设置：
        </h3>
        <table class="personalForm" cellpadding="0" cellspacing="0">
            <tr>
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
                    （菜单在此菜单之前）
                </td>
            </tr>
            <tr id="trChildMenuUrl">
                <td class="formTitle">
                    菜单Url：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="UrlTextBox" runat="server" Columns="60"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="fourthDiv" style="display: <%= ReferenceMenuVisible %>">
        <h3 style="padding-left: 200px">
            引用菜单设置：
        </h3>
        <table class="personalForm" cellpadding="0" cellspacing="0">
            <tr id="trReferenceMenu">
                <td class="formTitle">
                    引用菜单：
                </td>
                <td class="formValue">
                    <asp:DropDownList ID="ddlReferenceMenu" runat="server" onchange="ChangeInputBySelect()">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="formTitle">
                    菜单名称：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="txtTitle_Reference" runat="server" Columns="60"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtTitle_Reference"
                        ErrorMessage="不能为空" ValidationGroup="SaveButton"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="formTitle">
                    菜单描述：
                </td>
                <td class="formValue">
                    <asp:TextBox ID="txtDes_Reference" runat="server" Columns="60"></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtDes_Reference"
                        ErrorMessage="不能为空" ValidationGroup="SaveButton"></asp:RequiredFieldValidator>
                    （显示在菜单中的文字）
                </td>
            </tr>
            <tr>
                <td class="formTitle">
                    菜单位置：
                </td>
                <td class="formValue">
                    <asp:DropDownList ID="ddlIndex_Reference" runat="server">
                    </asp:DropDownList>
                    （菜单在此菜单之前）
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <asp:TextBox ID="DisplayTextBox" runat="server"></asp:TextBox>
        <asp:Button ID="SubmitsButton" runat="server" Text="Save" OnClick="SubmitButton_Click"/>
        <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
        <asp:Button ID="btnChildSave" runat="server" Text="Save" OnClick="btnChildSave_Click" />
        <asp:Button ID="btnReferenceSave" runat="server" Text="Save" OnClick="btnReferenceSave_Click" />
    </div>
</asp:Content>
