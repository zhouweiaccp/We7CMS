<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Plugin_Start.ascx.cs"
	Inherits="We7.CMS.Web.Admin.Modules.Plugin_Start" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<script src="/Admin/Ajax/Mask.js" type="text/javascript"></script>
<script src="/Admin/Ajax/Prototype/prototype.js" type="text/javascript"></script>
<meta http-equiv="Content-Type" content="html/text; charset=utf-8" />
<script type="text/javascript" src="/Admin/Ajax/Ext2.0/adapter/ext/ext-base.js"></script>
<script type="text/javascript" src="/Admin/Ajax/Ext2.0/ext-all.js"></script>
<script type="text/javascript" src="/Install/js/Plugin.js"></script>
<link type="text/css" rel="Stylesheet" id="scrollshow" href=""></link>
<script type="text/javascript">
	var mask = new MaskWin();
	//    function install()
	//    {
	//        //mask.showMessageProgressBar("/Admin/Ajax/PluginAjaxHandler.aspx?action=install&type=<%=PluginName %>");
	//        mask.showMessageProgressBar("/Admin/Ajax/PluginAjaxHandler.aspx?action=install&type=<%=PluginName %>");
	//        return false;
	//    }
	var refreshbutton = '<%=refresh.ClientID %>';
	function LocalInstall() {
		var param = {};
		param.action = "install";
		param.plugin = '<%=PluginName %>';
		param.submitbutton = document.getElementById('<%=refresh.ClientID %>');
		param.title = "安装插件";
		param.message = "安装成功！";
		param.menu = '<%=HasMenu %>';
		new MaskWin().showMessageProgressBar(param);
		return false;
	}
</script>
<div id="conbox">
	<dl>
		<dt>»搜索<br>
			<dd>
				<div>
					<table>
						<tr>
							<td style="width: 50px">
								<asp:DropDownList ID="QueryDropDownList" runat="server" Style="font-size: 12px">
									<asp:ListItem Value="1">关键字</asp:ListItem>
									<%--<asp:ListItem Value="2">作者</asp:ListItem>--%>
								</asp:DropDownList>
							</td>
							<td style="width: 50px">
								<asp:TextBox ID="QueryTextBox" runat="server" Style="font-size: 12px;"></asp:TextBox>
							</td>
							<td style="width: 50px">
								<asp:Button ID="QueryButton" runat="server" Text="搜索" CssClass="button" OnClick="QueryButton_Click" />
							</td>
							<td>
							</td>
						</tr>
					</table>
				</div>
			</dd>
		</dt>
	</dl>
</div>
<div id="conbox">
	<dl>
		<dt>»使用.Zip格式安装一个插件<br>
			<dd>
				<div>
					<table>
						<tr>
							<td>
								如果您有一个 .zip 格式的插件文件，您可以在这里通过上传并安装它。
							</td>
						</tr>
						<tr>
							<td>
								<asp:FileUpload ID="ZipFileUpload" runat="server" />
								<asp:Button ID="UploadButton" runat="server" Text="添加插件" OnClick="UploadButton_Click"
									CssClass="button" />
							</td>
						</tr>
						<tr>
							<td>
								<WEC:MessagePanel ID="messages" runat="server">
								</WEC:MessagePanel>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Button ID="InstallButton" Visible="false" Style="margin: 0; width: 100px;" CssClass="button"
									runat="server" Text="马上安装" OnClientClick='return  LocalInstall();' />
								<asp:Button ID="BackButton" Visible="false" Style="margin: 0; width: 100px;" CssClass="button"
									runat="server" Text="返回" OnClientClick='location.href="PluginList.aspx";return false' />
							</td>
						</tr>
					</table>
				</div>
			</dd>
		</dt>
	</dl>
	<asp:Button ID="refresh" runat="server" OnClick="OnClearMenuClick" Style="display: none" />
</div>
