<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PluginMenu.aspx.cs" Inherits="We7.CMS.Web.Admin.Plugin.PluginMenu" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<meta http-equiv="pragma" content="no-cach" />
	<meta http-equiv="cache-control" content="no-cache" />
	<meta http-equiv="expires" content="0" />
	<script src="/Scripts/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
	<script type="text/javascript">

		function SubmitClick() {
			var data = '';
			$("#formtable .tableDiv.copy").each(function (i) {
				if ($(this).hasClass("user") && !$("#enableUser input:eq(0)").attr("checked")) return;
				var id = $.trim($("#allmenus tr:eq(" + i + ") td:eq(4)").text());
				var name = $.trim($(this).find("input:text:eq(0)").val());
				var title = $.trim($(this).find("input:text:eq(1)").val());
				var url = $.trim($(this).find("input:text:eq(2)").val());
				var entityID = $.trim($("#allmenus tr:eq(" + i + ") td:eq(5)").text());
				var parent = "{00000000-0000-0000-0000-000000000000},0";
				var parentUser = "{00000000-0001-0000-0000-000000000000},0";
				if ($(this).find("input:checkbox:eq(0)").attr("checked") == false) {
					parent = $.trim($(this).find("select:eq(0)").val());
					parentUser = $.trim($(this).find("select:eq(1)").val());
				}
				if ($(this).hasClass("user")) parent = parentUser;
				data += id + "$" + name + "$" + title + "$" + url + "$" + parent + "$" + entityID + "|";
			});
			$("#resultValue").val(data);
			var submitBtn = document.getElementById("<%=SubmitsButton.ClientID %>");
			$("#Messages").text("正在更新菜单……");
			$("#formtable").hide();
			submitBtn.click();
		}
		function changeTab(on, off) {
			$("#formtype span:eq(" + on + ")").addClass("on");
			$("#formtype span:eq(" + off + ")").removeClass("on");
			$(".toolbar").hide();
			var $menuitems = $("#formtable .tableDiv.copy");
			$("#formtable .tableDiv").each(function (i) {
				$(this).find("select:eq(" + on + ")").show();
				$(this).find("select:eq(" + off + ")").hide();
			});
			var $enableusercheckbox = $("#enableUser input:eq(0)");
			$menuitems.slideUp();
			switch (on) {
				case 0: $menuitems = $("#formtable .tableDiv.admin");
					if ($enableusercheckbox.attr("checked"))
						$enableusercheckbox.attr("disabled", true);
					else
						$("#enableUser").hide();
					$("span.tiptext").hide();
					$menuitems.slideDown();
					$(".toolbar").show("slow");
					break;
				case 1: $menuitems = $("#formtable .tableDiv.user");
					$("#enableUser").show();
					$enableusercheckbox.attr("disabled", false);
					if ($enableusercheckbox.attr("checked")) {
						$menuitems.slideDown();
						$("span.tiptext").show();
						$(".toolbar").show("slow");
					}
					break;
			}
		}
		function enableUser(obj) {
			if ($(obj).attr("checked")) {
				$("#formtable .tableDiv.user").slideDown();
				$("span.tiptext").show();
				$(".toolbar").show("slow");
			}
			else {
				changeTab(0, 1);
			}
		}
	</script>
	<style type="text/css">
		#formtable
		{
			font-size: 14px;
		}
		td
		{
			line-height: 2em;
		}
		.textbox
		{
			width: 350px;
		}
		.tableDiv
		{
			border-bottom: 1px solid grey;
			display: none;
			height: 120px;
		}
		.firstMenu
		{
			margin-left: 100px;
			font-size: 14px;
			font-weight: normal;
		}
		#formtype
		{
			border-bottom: 3px solid #A8A8A8;
		}
		#formtype span
		{
			padding: 2px 10px;
			font-size: 12px;
			margin-left: 10px;
			background-color: #A8A8A8;
			border: 1px solid #A8A8A8;
			border-bottom: none;
			color: White;
			cursor: pointer;
		}
		#formtype span.on
		{
			background-color: White;
			font-weight: bold;
			color: #EB976B;
			padding-bottom: 3px;
		}
		span.tiptext
		{
			font-size: 12px;
			color: #6C9F1E;
			margin-right: 20px;
			display:none;
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
	<WEC:MessagePanel runat="Server" ID="Messages">
	</WEC:MessagePanel>
	<div id="formtable" runat="server">
		<div id="formtype">
			<span onclick="changeTab(0,1);" class="on">后台菜单</span><span onclick="changeTab(1,0);">会员中心菜单</span>
			<div id="enableUser" style="display: none; float: right">
				<input type="checkbox" onclick="enableUser(this);" runat="server" id="UserCheckBox" />生成会员中心菜单</div>
		</div>
		<div class="tableDiv">
			<input type="checkbox" disabled="disabled" style="display:none"  autocomplete="off" onclick="toggleSelect(this);" />
			<table cellpadding="0" cellspacing="0">
				<tr>
					<td class="formTitle">
						菜单名称：
					</td>
					<td>
						<input class="textbox" />
					</td>
				</tr>
				<tr>
					<td>
						菜单描述：
					</td>
					<td>
						<input class="textbox" />
					</td>
				</tr>
				<tr>
					<td>
						菜单位置：
					</td>
					<td class="menuselect">
						<asp:DropDownList ID="SecondIndexDropDownList" runat="server">
						</asp:DropDownList>
						<asp:DropDownList ID="UserMenusDropDownList" runat="server" Style="display: none">
						</asp:DropDownList>
						（菜单在此菜单之前）
					</td>
					<td style="display:none" class="isTop"><span>是一级菜单</span></td>
				</tr>
				<tr>
					<td>
						菜单Url：
					</td>
					<td>
						<input class="textbox" />
						<input style="display: none" />
					</td>
				</tr>
			</table>
		</div>
		<div class="toolbar" style="direction: rtl; padding-top: 5px">
			<input type="button" onclick="SubmitClick();" value="生成菜单" />
			<input type="button" onclick="window.parent.document.getElementById(window.parent.refreshbutton).click();"
				value="不生成菜单" />
			<span class="tiptext">会员中心菜单一律生成在“我的We7”主菜单下</span>
		</div>
		<div style="display: none">
			<asp:Table ID="allmenus" runat="server">
			</asp:Table>
			<input type="hidden" id="resultValue" runat="server" />
			<asp:TextBox ID="DisplayTextBox" runat="server"></asp:TextBox>
			<asp:Button ID="SubmitsButton" runat="server" Text="Save" OnClick="SubmitButton_Click"
				ValidationGroup="SubmitButton" />
		</div>
	</div>
	</form>
	<script type="text/javascript">
		var count = '<%=PluginMenus.Count %>';
		var $first = $("#formtable .tableDiv:eq(0)");
		for (var i = 0; i < parseInt(count); i++) copyTable(i);
		function copyTable(i) {
			var admin = $.trim($("#allmenus tr:eq(" + i + ") td:eq(5)").text()) == "System.Administration";
			var user = $.trim($("#allmenus tr:eq(" + i + ") td:eq(5)").text()) == "System.User";
			var $copy = $first.clone();
			$first.before($copy);
			$copy.addClass("copy");
			if (admin) $copy.addClass("admin");
			else if (user) $copy.addClass("user");
			$copy.find("input:text:eq(0)").val($.trim($("#allmenus tr:eq(" + i + ") td:eq(0)").text()));
			$copy.find("input:text:eq(1)").val($.trim($("#allmenus tr:eq(" + i + ") td:eq(1)").text()));
			$copy.find("input:text:eq(2)").val($.trim($("#allmenus tr:eq(" + i + ") td:eq(2)").text()));
			$copy.find("input:text:eq(3)").val($.trim($("#allmenus tr:eq(" + i + ") td:eq(4)").text()));
			if ($.trim($("#allmenus tr:eq(" + i + ") td:eq(3)").text()) != '')
				if (admin && $.trim($("#allmenus tr:eq(" + i + ") td:eq(3)").text()) == "<%=We7Helper.EmptyGUID %>,0") {
					$copy.find("input:checkbox:eq(0)").attr("checked", true);
					$copy.find(".menuselect").hide();
					$copy.find(".isTop").show();
					$copy.find("select:eq(0)").append("<option value='" + $copy.find("input:text:eq(3)").val() + ",0'>├" + $copy.find("input:text:eq(0)").val() + "</option>");
					$first.find("select:eq(0)").append("<option value='" + $copy.find("input:text:eq(3)").val() + ",0'>├" + $copy.find("input:text:eq(0)").val() + "</option>");
				}
				else if (user && $.trim($("#allmenus tr:eq(" + i + ") td:eq(3)").text()) == "{00000000-0001-0000-0000-000000000000},0") {
					$copy.find("input:checkbox:eq(0)").attr("checked", true);
					$copy.find(".menuselect").hide();
					$copy.find(".isTop").show();
					$copy.find("select:eq(1)").append("<option value='" + $copy.find("input:text:eq(3)").val() + ",0'>├" + $copy.find("input:text:eq(0)").val() + "</option>");
					$first.find("select:eq(1)").append("<option value='" + $copy.find("input:text:eq(3)").val() + ",0'>├" + $copy.find("input:text:eq(0)").val() + "</option>");
				}
				else {
					if (admin) $copy.find("select:eq(0)").val($.trim($("#allmenus tr:eq(" + i + ") td:eq(3)").text()));
					else if (user) $copy.find("select:eq(1)").val($.trim($("#allmenus tr:eq(" + i + ") td:eq(3)").text()));
				}
		}
		var hasusermenu = '<%=HasUserMenu %>';
		if (hasusermenu.toLowerCase() == "true") {
			$("#enableUser input:eq(0)").attr("checked", true);
			$("#enableUser").show();
		}
		changeTab(0, 1);
		function toggleSelect(obj) {
			if ($(obj).attr("checked") == true) {
				$(obj).next().find(".menuselect").hide();
				$(".tableDiv select").append("<option value='" + $(obj).next().find("input:text:eq(3)").val() + ",0'>├" + $(obj).next().find("input:text:eq(0)").val() + "</option>");
			}
			else {
				$(obj).next().find(".menuselect").show();
				$(".tableDiv select option").each(function (i) { if (this.value.split(',')[0] == $(obj).next().find("input:text:eq(3)").val()) $(this).remove(); });
			}
		}
//		$("#formtable .tableDiv input:checkbox").each(function (i) {
//			if ($(this).attr("checked") == false) $(this).next().hide();
//		});
	</script>
</body>
</html>
