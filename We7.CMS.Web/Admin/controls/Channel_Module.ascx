<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Channel_Module.ascx.cs"
    Inherits="We7.CMS.Web.Admin.controls.Channel_Module" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<WEC:MessagePanel ID="Messages" runat="server">
</WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»栏目附加模块<br>
            <img src="/admin/images/bulb.gif" align="absmiddle" /><label class="block_info">设置栏目附加模块。</label>
            <input class="Btn" id="SaveButton" runat="server" type="submit" value="保存" onserverclick="SaveButton_ServerClick"
                visible="false">
        </dt>
        <dd>
            <div style="width:500px;"></div>
            <asp:CheckBoxList ID="chkModules" runat="server" RepeatColumns="4">
            </asp:CheckBoxList>
            <br />
            <input class="Btn" id="SaveButton2" runat="server" type="submit" value="更新栏目选项" onserverclick="SaveButton_ServerClick">
        </dd>
    </dl>
</div>
