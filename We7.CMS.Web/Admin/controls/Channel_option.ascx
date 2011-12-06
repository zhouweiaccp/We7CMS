<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Channel_option.ascx.cs"
    Inherits="We7.CMS.Web.Admin.controls.Channel_option" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>

<WEC:MessagePanel ID="Messages" runat="server">
</WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»栏目的高级选项<br>
            <img src="/admin/images/bulb.gif" align="absmiddle" /><label class="block_info">设置栏目高级选项。</label>
            <input class="Btn" id="SaveButton" runat="server" type="submit" value="更新栏目选项" onserverclick="SaveButton_ServerClick"
                visible="false">
        </dt>
        <dd>
            <table id="personalForm" cellpadding="0" cellspacing="0" style="margin-bottom: 50px">
                <tr id="linkSpan" runat="server">
                    <td class="formTitle">
                        标题图
                    </td>
                    <td class="formValue">
                        <asp:HyperLink runat="server" ImageUrl="" ID="TitleImage"></asp:HyperLink>
                        <asp:FileUpload Style="position: relative" ID="TitleImageFileUpload" runat="server">
                        </asp:FileUpload>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        安全
                    </td>
                    <td class="formValue">
                        <asp:DropDownList ID="SecurityDropDownList" runat="server">
                            <asp:ListItem Value="0">低 - 任何人均可查看该栏目</asp:ListItem>
                            <asp:ListItem Value="1">中 - 内容只允许登录用户查看</asp:ListItem>
                            <asp:ListItem Value="2">高 - 只有管理员能进行管理和浏览</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        发布流程
                    </td>
                    <td class="formValue">
                        <asp:DropDownList ID="ProcessDropDownList" runat="server">
                            <asp:ListItem Value="0">直接发布</asp:ListItem>
                            <asp:ListItem Value="1">审核发布</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ProcessLayerDropDownlist" runat="server" Style="display: none">
                            <asp:ListItem Value="1">一级审核</asp:ListItem>
                            <asp:ListItem Value="2">二级审核</asp:ListItem>
                            <asp:ListItem Value="3">三级审核</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ProcessEndingDropDownList" runat="server" Style="display: none">
                            <asp:ListItem Value="0">终审后禁用文章</asp:ListItem>
                            <asp:ListItem Value="1">终审后直接启用文章</asp:ListItem>
                            <asp:ListItem Value="2">终审后送上级站点审核</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        页面关键字<br />Keyword
                    </td>
                    <td class="formValue">
                        <asp:TextBox ID="KeywordTextBox" runat="server" Columns="40" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        页面描述<br />Description
                    </td>
                    <td class="formValue">
                        <asp:TextBox ID="DescriptionTextBox" runat="server" Columns="40" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        查询参数<br />Parameter
                    </td>
                    <td class="formValue">
                        <asp:TextBox ID="ParameterTextBox" runat="server" Columns="40" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                    </td>
                    <td class="formValue">
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="padding-top: 15px; padding-left: 0px">
                        <input class="Btn" id="SaveButton2" runat="server" type="submit" value="更新栏目选项" onserverclick="SaveButton_ServerClick">
                    </td>
                </tr>
            </table>
            <div style="display: none">
                <asp:TextBox ID="ParentIDTextBox" runat="server"></asp:TextBox>
                <asp:TextBox ID="ChannelNameHidden" runat="server"></asp:TextBox>
                <asp:TextBox ID="MoveToIDTextBox" runat="server"></asp:TextBox>
                <asp:TextBox ID="ReferenceIDTextBox" runat="server"></asp:TextBox>
                <asp:TextBox ID="AreaIDTextBox" runat="server"></asp:TextBox>
                <asp:TextBox ID="SouceIDTextBox" runat="server"></asp:TextBox>
            </div>
        </dd>
    </dl>
</div>
