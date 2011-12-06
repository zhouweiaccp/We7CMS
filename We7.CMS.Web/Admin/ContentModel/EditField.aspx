<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/theme/classic/content.Master"
    AutoEventWireup="true" CodeBehind="EditField.aspx.cs" Inherits="We7.CMS.Web.Admin.ContentModel.EditField" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="添加字段"></asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="用于新建字段">
            </asp:Label>
        </span>
    </h2>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
    <div>
        <table>
            <tbody>
                <tr>
                    <td class="title">
                        字段中文名称(标签):
                    </td>
                    <td class="left">
                        <asp:TextBox ID="FieldLabelTextBox" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        字段名称:
                    </td>
                    <td class="left">
                        <asp:TextBox ID="FieldNameTextBox" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        字段类型:
                    </td>
                    <td>
                        <asp:DropDownList ID="FieldDataTypeDropDownList" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ChangeType">
                            <asp:ListItem Selected="True" Text="文本" Value="String"></asp:ListItem>
                            <asp:ListItem Text="整数" Value="Int32"></asp:ListItem>
                            <asp:ListItem Text="小数" Value="Decimal"></asp:ListItem>
                            <asp:ListItem Text="是否" Value="Boolean"></asp:ListItem>
                            <asp:ListItem Text="日期" Value="DateTime"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="div_maxlength" runat="server">
                    <td>
                        字段长度
                    </td>
                    <td>
                        <asp:TextBox ID="MaxlengthTextBox" runat="server" Text="25"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="MaxlengthTextBox"
                            ErrorMessage="必须为正整数!" ValidationExpression="^[0-9]*[1-9][0-9]*$"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        是否标题:
                    </td>
                    <td>
                        <asp:CheckBox ID="TitleCheckBox" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        是否查询字段:
                    </td>
                    <td>
                        <asp:CheckBox ID="SearchFieldCheckBox" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="toolbar">
            <asp:HyperLink ID="SaveHyperLink" NavigateUrl="javascript:SubmitClick();" runat="server">
            保存
            </asp:HyperLink>
            <a href="field.aspx?modelType=<%=modelType %>&modelname=<%=ModelName%>">返回列表</a>
        </div>
        <div style="display: none">
            <asp:Button ID="btnSaveField" runat="server" Text="保存" OnClick="SubmitButton_Click" />
        </div>
        <script type="text/javascript">
            function SubmitClick() {
                var submitBtn = document.getElementById("<%=btnSaveField.ClientID %>");
                submitBtn.click();
            }
        </script>
    </div>
</asp:Content>
