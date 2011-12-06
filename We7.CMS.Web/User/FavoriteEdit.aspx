<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FavoriteEdit.aspx.cs" MasterPageFile="~/User/DefaultMaster/content.Master"
    Inherits="We7.CMS.Web.User.FavoriteEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
    <style>
        th, td
        {
            font-size: 12px;
            font-family: 宋体 Arial;
        }
        th
        {
            width: 100px;
            background: #f0f0f0;
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <form runat="server" id="form1">
    <div class="realRight ml10">
        <div class="mybox">
            <div class="mytit">
                添加收藏</div>
            <div class="con">
                <table style="border-collapse: collapse" width="450px;" border="1" cellpadding="2">
                    <tr>
                        <th>
                            收藏地址：
                        </th>
                        <td>
                            <asp:TextBox ID="txtUrl" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            缩略图地址：
                        </th>
                        <td>
                            <asp:TextBox ID="txtThumbnail" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            标题：
                        </th>
                        <td>
                            <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            简介：
                        </th>
                        <td>
                            <asp:TextBox TextMode="MultiLine" ID="txtDesc" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style='<%= definedTag?"display:block": "display:none" %>'>
                        <th>
                            个性标签：
                        </th>
                        <td>
                            <asp:TextBox ID="txtTag" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style='<%= definedTag?"display:block": "display:none" %>'>
                        <th>
                            常用标签：
                        </th>
                        <td>
                            <asp:DataList ID="dlTagList" runat="server" RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <a id="tagHref_<%# Container.ItemIndex %>" href="javascript:SelectedTag('<%# Container.ItemIndex %>','<%= txtTag.ClientID %>')">
                                        <%# Eval("Tag") %></a>&nbsp;
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr style='<%= definedTag?"display:none": "display:block" %>'>
                        <th>
                            系统标签：
                        </th>
                        <td>
                            <asp:RadioButtonList RepeatDirection="Horizontal" RepeatLayout="Table" ID="rblSystemTags"
                                runat="server">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                        </th>
                        <td>
                            <asp:Button runat="server" ID="bttnSave" Text="保存" OnClick="bttnSave_Click" />
                            <asp:Button runat="server" ID="bttnReset" Text="返回" OnClick="bttnReset_Click" />
                            &nbsp;&nbsp;<asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        function SelectedTag(tagIndex, defineTag) {
            var tag = document.getElementById('tagHref_' + tagIndex).innerHTML;
            document.getElementById(defineTag).value = tag;
        }
    </script>
</asp:Content>
