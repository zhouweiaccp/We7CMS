<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateStaticize.ascx.cs"
    Inherits="We7.CMS.Web.Admin.manage.controls.TemplateStaticize" %>
<div id="conbox">
    <dl>
        <dt>»生成网站静态页<br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" /><label class="block_info">绿色代表成功生成的模板，红色代表生成出错的模板。</label>
        </dt>
    </dl>
    <div style="display: none; min-width:600px">
        <div>
            <asp:Button ID="bttnGenerate" runat="server" Text="重新生成静态化" OnClick="bttnGenerate_Click" />
            <asp:Button ID="bttnQuery" runat="server" Text="查询生成结果" OnClick="bttnQuery_Click" /><br />
        </div>
        <hr style="width: 100%" />
        <div>
            <asp:Literal ID="lblMsg" runat="server"></asp:Literal>
        </div>
    </div>
