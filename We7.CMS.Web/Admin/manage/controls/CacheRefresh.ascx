<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CacheRefresh.ascx.cs"
    Inherits="We7.CMS.Web.Admin.manage.controls.CacheRefresh" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<WEC:MessagePanel runat="Server" ID="Messages">
</WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»更新缓存<br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" /><label class="block_info">更新网站缓存，让网站启用修改后的内容。</label>
        </dt>
    </dl>
    <div style="display: table; min-width:600px">
        <div>
            <asp:CheckBoxList ID="chkItem" runat="server" RepeatColumns="5" RepeatDirection="Horizontal">
            </asp:CheckBoxList>
        </div>
        <div>
            <asp:Button ID="bttnGenerate" runat="server" Text="刷新所选项缓存" OnClick="bttnGenerate_Click" />
        </div>
    </div>
