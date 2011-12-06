<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="We7.Model.UI.Container.we7.Editor" %>
<div id="mycontent">
	<div class="Tab menuTab">
		<ul class="Tabs">
			<asp:Label runat="server" ID="MenuTabLabel"></asp:Label>
		</ul>
		<span style="float: right;margin-top:-40px;margin-right:50px; padding: 5px;" id="editMode" runat="server"></span>
		<a href="<%=BackUrl %>" style="float: right; margin-right: 20px; margin-top: -40px;">
			<img alt="" title="返回" src="/admin/images/back.png" /></a>
	</div>
	<div class="clear">
	</div>
	<div id="rightWrapper">
		<div id="container">
			<div id="conbox">
				<dl>
					<dt>»<asp:Label runat="server" ID="ModelLabel"></asp:Label>基本信息<br />
					</dt>
				</dl>
			</div>
			<table cellpadding="0" cellspacing="1" class="table_form">
				<asp:Repeater ID="rpEditor" runat="server" OnItemDataBound="rpEditor_ItemDataBound">
				</asp:Repeater>
				<tr id="trBtn" runat="server">
					<th>
					</th>
					<td>
						<asp:Button ID="bttnSave" runat="server" CommandName="add" Text="保存" OnClick="bttnSave_Click"
							CssClass="Btn" />
						<asp:Button ID="bttnEdit" runat="server" CommandName="edit" Text="编辑" OnClick="bttnEdit_Click"
							CssClass="Btn" />
						<input type="button" id="modelreset" value="清空" class="Btn" />
					</td>
				</tr>
			</table>
		</div>
	</div>
</div>
<script type="text/javascript">
	if ($("#<%=editMode.ClientID %> a").length <= 1) $("#<%=editMode.ClientID %> a").hide();

    //<![CDATA[
	$(function () {
		$("#modelreset").click(function () {
			window.location = '<%=NewUrl %>';
		});
	});
    //]]>
</script>
