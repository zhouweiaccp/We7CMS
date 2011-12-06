<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/ContentNoMenu.Master" AutoEventWireup="true" CodeBehind="WelcomeToChannel.aspx.cs" Inherits="We7.CMS.Web.Admin.WelcomeToChannel" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <table cellpadding="2" cellspacing="1" class="List">
        <tr>
            <th colspan="2">
                栏目通用操作
            </th>
        </tr>
        <tr >
            <td style="width: 15%">
                <span style="color: #0000ff">
                    添加栏目节点</span>
            </td>
            <td>
            点击任何栏目项，按右键，将出现菜单，进行创建栏目操作。<br />
                为方便归类管理，界面中将信息归类成“基本信息”、“栏目选项”、“栏目模板”、“栏目标签”、“栏目权限”等书签式管理选项，以方便按快捷分类设置信息选项。在填写好相关信息后，单击页面底部“添加”按钮保存所添加的栏目节点。
            </td>
        </tr>
        <tr >
            <td >
                <span style="color: #0000ff">
                    移动栏目</span>
            </td>
            <td>
                本操作是将一个栏目节点从原有位置移动到新的位置，改变其排序与父子关系。
            </td>
        </tr>
        <tr >
            <td >
                <span style="color: #0000ff">
                    批量删除</span>
            </td>
            <td>
                本操作是批量删除栏目节点数据，该操作将同时删除栏目下文章信息，所以，如不想删除文章信息应当先将文章信息移动到别的栏目再进行删除。
            </td>
        </tr>
         <tr >
            <td colspan="2" style="padding:10px" >
            <a href="/admin/ChannelEdit.aspx?pid=" class="NewBtn" >立刻创建一个顶级栏目</a>
            </td>
            </tr>
    </table>
</asp:Content>
