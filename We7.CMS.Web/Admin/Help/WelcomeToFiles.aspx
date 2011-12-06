<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/ContentNoMenu.Master" AutoEventWireup="true" CodeBehind="WelcomeToFiles.aspx.cs" Inherits="We7.CMS.Web.Admin.WelcomeToFiles" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <table cellpadding="2" cellspacing="1" class="List"  >
        <tr >
            <th   colspan="2">
                数据文件夹说明&nbsp;</th>
        </tr>
        <tr  >
            <td  style="width: 15%">
                <a href="../Folder.aspx?folder=_data" ><span style="color: #0000ff">
                    _data</span></a>
            </td>
            <td>
                存放图片及附件。 子栏目channel下为每个栏目对应的文件夹，存放该栏目下文章图片及附件。</td>
        </tr>
        <tr >
            <td >
                <a  href="../Folder.aspx?folder=_skin"><span style="color: #0000ff">
                    _skin</span></a>
            </td>
            <td>
                存放模板组文件。每个模板组一个独立子目录。
            </td>
        </tr>
        <tr >
            <td >
                <a href="../Folder.aspx?folder=_templates" ><span style="color: #0000ff">
                    _templates</span></a>
            </td>
            <td>
                单模板组的模板存放地。&nbsp;</td>
        </tr>
        <tr >
            <td >
                <a ><span style="color: #0000ff">
                    </span></a>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>

</asp:Content>
