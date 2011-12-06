<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/admin/theme/classic/content.Master"
	CodeBehind="system.aspx.cs" Inherits="We7.CMS.Web.Admin.system" %>

<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<%@ Register Src="controls/System_image.ascx" TagName="System_image" TagPrefix="uc1" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
	<link rel="stylesheet" type="text/css" href="<%=AppPath%>/ajax/jquery/css/ui.tabs.css"
		media="all" />
	<script src="<%=AppPath%>/ajax/jquery/ui.tabs.pack.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(function () {
			$('#container-1 > ul').tabs();
		});
	</script>
	<script type="text/javascript">
		function selectTemplate() {
			showDialog("TemplateGroupList.aspx", hanldeTempalteListCallback);
		}

		function hanldeTempalteListCallback(v, t) {
			document.getElementById("DefaultTemplateGroupTextBox").value = t;
			document.mainForm.DefaultTemplateGroupFileNameTextBox.value = v;
		}
		function doStyle() {
			showDialog("<%=AppPath%>/cgi-bin/controls/StyleEditor/StyleEditor.aspx", onStyleEditorCallback, "color:#ffffff");
		}
		function onStyleEditorCallback(v, t) {
		}
		function onSaveButtonClick() {
			document.getElementById("<%=SaveButton.ClientID %>").click();
		}

		function SelectAll(tempControl) {
			var theBox = tempControl;
			xState = theBox.checked;

			elem = theBox.form.elements;
			for (i = 0; i < elem.length; i++)
				if (elem[i].type == "checkbox" && elem[i].id != theBox.id) {
					if (elem[i].checked != xState)
						elem[i].click();
				}
	}

	function startCache() {
		var EnableCache = document.getElementById("<%=EnableCache.ClientID %>");
		var CacheTimeSpanTextBox = document.getElementById("<%=CacheTimeSpanTextBox.ClientID %>");
		if (EnableCache.checked) {
			CacheTimeSpanTextBox.removeAttribute("disabled");
		} else {
			CacheTimeSpanTextBox.setAttribute("disabled", "disabled");
		}
	}

	function urlFormat(obj) {
		var item = obj.options[obj.selectedIndex].value;
		if (item == "custom")
			$("#customUrl").css("display", "");
		else
			$("#customUrl").css("display", "none");
	}
	var IDIPStrategy = "<%=hddnIPStrategy.ClientID %>";
	</script>
	<input id="xyz" type="text" style="display: none" />
	<h2 class="title">
		<asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_img.gif" />
		<asp:Label ID="NameLabel" runat="server" Text="网站基本设置">
		</asp:Label>
		<span class="summary">
			<asp:Label ID="SummaryLabel" runat="server" Text="We7系统的各类参数配置。">
			</asp:Label>
		</span>
	</h2>
	<div class="toolbar">
		<asp:HyperLink ID="UpdateHyperLink" NavigateUrl="javascript:onSaveButtonClick()"
			runat="server">
            保存更改</asp:HyperLink>
		<span></span>
		<asp:HyperLink ID="RefreshHyperLink" NavigateUrl="system.aspx" runat="server">
            刷新</asp:HyperLink>
	</div>
	<WEC:MessagePanel ID="Messages" runat="server">
	</WEC:MessagePanel>
	<div style="margin-top: 10px">
		<div id="container-1">
			<ul>
				<li><a href="#fragment-1"><span>本站配置</span></a></li>
				<li><a href="#fragment-2"><span>搜索引擎优化</span></a></li>
				<li><a href="#fragment-3"><span>系统初始值</span></a></li>
				<li><a href="#fragment-4"><span>邮件配置</span></a></li>
				<li><a href="#fragment-5"><span>功能启用/禁用</span></a></li>
				<li><a href="#fragment-6"><span>图片设置</span></a></li>
				<li id="ipset" runat="server"><a id="#fragment-7" href="#fragment-7"><span>IP过滤</span></a></li>
			</ul>
			<div id="fragment-1">
				<table style="border: solid 0px #fff;">
					<tr>
						<td align="right">
							站点名称：
						</td>
						<td>
							<asp:TextBox ID="SiteNameTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
						<td>
						</td>
					</tr>
					<tr>
						<td align="right">
							本站网址：
						</td>
						<td>
							<asp:TextBox ID="RootUrlTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							公司名称：
						</td>
						<td align="left">
							<asp:TextBox ID="txtSiteFullName" runat="server" class="colorblur" Columns="35" MaxLength="100"
								onblur="this.className='colorblur';" onfocus="this.className='colorfocus';" />
							<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSiteFullName"
								ErrorMessage="请填写公司名称"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td align="right">
							站点logo：
						</td>
						<td align="left">
							<img id="logo_preview" src="<%=ImageValue.Text%>" alt="logo预览图" style="max-width: 175px;max-height:175px;" /><br />
							<asp:TextBox ID="ImageValue" runat="server" Width="238" MaxLength="400" /><asp:RequiredFieldValidator
								ID="RequiredFieldValidator4" runat="server" ControlToValidate="ImageValue" ErrorMessage="请您上传站点logo"></asp:RequiredFieldValidator><br />
							<asp:FileUpload ID="fuImage" runat="server" CssClass="file_style" Style="width: 200px;" />&nbsp;&nbsp;
							<asp:Button ID="bttnUpload" runat="server" Text="上传" CssClass="button_style" OnClick="bttnUpload_Click"
								CausesValidation="false" />
						</td>
					</tr>
					<tr>
						<td align="right">
							静态URL文章后缀：
						</td>
						<td>
							<asp:RadioButton ID="AshxRadioButton" runat="server" Text="aspx" GroupName="urlFormat"
								Checked="true" />
							<asp:RadioButton ID="HtmlRadioButton" runat="server" Text="html" GroupName="urlFormat" />
						</td>
					</tr>
					<tr>
						<td align="right">
							静态URL重写级别：
						</td>
						<td>
							<asp:RadioButton ID="IISRadioButton" runat="server" Text="IIS" GroupName="urlRewriter" />
							<asp:RadioButton ID="ASPNETRadioButton" runat="server" Text="Asp.net" GroupName="urlRewriter"
								Checked="true" />
						</td>
					</tr>
					<tr>
						<td colspan="2" style="color: Red; padding-left: 50px; padding-right: 200px">
							（1）如果选用IIS级别的话，需要首先在IIS站点属性中进行“ISAPI过滤器”设置
							<br />
							（2）如果选用Asp.net级别，并希望使用HTML扩展名的话，需要设置通配符<br />
							详见 <a href="http://help.we7.cn/library/94.html" target="_blank">We7设置：设置静态URL</a>
							。
						</td>
					</tr>
					<tr>
						<td align="right">
							文章静态URL样式：
						</td>
						<td>
							<asp:DropDownList ID="ArticleUrlGeneratorDropDownList" runat="server" onchange="urlFormat(this)">
								<asp:ListItem Selected="True" Value="yyyy-MM-dd">例如：2010-01-27-372.html</asp:ListItem>
								<asp:ListItem Value="0">例如：372.html</asp:ListItem>
								<asp:ListItem Value="">例如：cca652ee_f166_4787_8cd5_19b37342e152.html</asp:ListItem>
								<asp:ListItem Value="custom">自定义</asp:ListItem>
							</asp:DropDownList>
							<div id="customUrl" style="display: none">
								<asp:TextBox ID="ArticleUrlGeneratorTextBox" runat="server" Columns="60"></asp:TextBox>
								<input id="Button1" type="button" value="设为默认值" onclick="javascript:<%=ArticleUrlGeneratorTextBox.ClientID %>.value='yyyy-MM-dd';" />
								<br />
								文章Url的生成格式：按流水号。前面可加前缀，以上为更新日期前缀格式y - 年，M-月，d-日， 填0为不需要前缀；不填或缺省为按GUID生成
							</div>
						</td>
					</tr>
					<tr>
						<td align="right">
							网站状态：
						</td>
						<td>
							<asp:DropDownList ID="SiteStateDropDownList" runat="server">
								<asp:ListItem Value="debug">调试：程序员正在调试控件</asp:ListItem>
								<asp:ListItem Selected="True" Value="edit">建设：制作人员正在建设网站，未发布</asp:ListItem>
								<asp:ListItem Value="run">运行：网站已开通</asp:ListItem>
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right">
							插件商店序列号：
						</td>
						<td>
							<asp:TextBox ID="txtSN" runat="server" TextMode="MultiLine" Width="300px" Rows="1"></asp:TextBox>
						</td>
					</tr>
					<tr style="display: none">
						<td align="right">
							单点登陆同步验证站点：
							<br />
							(格式：http://www.a.com;http://www.b.com)
						</td>
						<td>
							<asp:TextBox ID="txtSSOUrls" runat="server" TextMode="MultiLine" Rows="1" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							后台版本信息：
							<br />
							(修改只对OEM版生效)
						</td>
						<td>
							<asp:TextBox ID="txtCopyright" runat="server" TextMode="MultiLine" Rows="2" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							网站备案信息
							<br />
							(修改只对OEM版生效)
						</td>
						<td>
							<asp:TextBox ID="txtIcpInfo" runat="server" TextMode="MultiLine" Rows="2" Columns="60" />
							<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtIcpInfo"
								ErrorMessage="请填写网站备案信息" Display="Dynamic" runat="server" />
						</td>
					</tr>
					<tr>
						<td align="right">
							后台链接信息：
							<br />
							(修改只对OEM版生效)
						</td>
						<td>
							<asp:TextBox ID="txtLinks" runat="server" TextMode="MultiLine" Rows="2" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<%--<tr>
                        <td align="right">
                            后台界面风格：
                        </td>
                        <td>
                            &nbsp;<asp:DropDownList ID="ThemeDropDownList" runat="server">
                                <asp:ListItem Selected="True" Value="default">现代</asp:ListItem>
                                <asp:ListItem Value="classic">经典</asp:ListItem>
                                </asp:DropDownList></td>
                    </tr>--%>
				</table>
			</div>
			<div id="fragment-2">
				<table style="border: solid 0px #fff;">
					<tr>
						<td align="right">
							网站首页标题：
						</td>
						<td>
							<asp:TextBox ID="HomePageTitleTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							栏目索引页标题：
						</td>
						<td>
							<asp:TextBox ID="ChannelPageTitleTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							内容页标题：
						</td>
						<td>
							<asp:TextBox ID="ContentPageTitleTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							页面关键词(keyword)：
						</td>
						<td>
							<asp:TextBox ID="KeywordPageMetaTextBox" runat="server" TextMode="MultiLine" Rows="5"
								Columns="48"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							页面描述(description)：
						</td>
						<td>
							<asp:TextBox ID="DescriptionPageMetaTextBox" runat="server" TextMode="MultiLine"
								Rows="5" Columns="48"></asp:TextBox>
						</td>
					</tr>
				</table>
			</div>
			<div id="fragment-3">
				<table>
					<tr>
						<td align="right">
							文章审核后是否自动发布：
						</td>
						<td>
							<asp:CheckBox ID="ArticleAutoPublish" runat="server" />
						</td>
					</tr>
					<tr>
						<td align="right">
							文章是否自动共享同步：
						</td>
						<td>
							<asp:CheckBox ID="ArticleAutoShare" runat="server" />
						</td>
					</tr>
					<tr>
						<td colspan="2" style="color: Red; padding-left: 20px; padding-right: 200px">
							初始化默认角色：此设置为用户注册企业会员或注册个人会员时默认的角色
						</td>
					</tr>
					<%--                    <tr>
                        <td align="right">
                            初始化企业会员默认角色：
                        </td>
                        <td>
                            <asp:DropDownList ID="CompanyDropDownList" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            初始化个人会员默认角色：
                        </td>
                        <td>
                            <asp:DropDownList ID="PersonDropDownList" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>--%>
					<tr>
						<td align="right">
							文章来源默认：
						</td>
						<td>
							<asp:TextBox ID="ArticleSourceDefaultTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
				</table>
			</div>
			<div id="fragment-4">
				<table style="border: solid 0px #fff;">
					<tr>
						<td align="right">
							发送邮箱服务器：
						</td>
						<td>
							<asp:TextBox ID="SysMailServerTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							接收邮箱服务器：
						</td>
						<td>
							<asp:TextBox ID="SysPopServerTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							系统邮箱：
						</td>
						<td>
							<asp:TextBox ID="SystemMailTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							邮箱用户：
						</td>
						<td>
							<asp:TextBox ID="SySMailUserTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							邮箱密码：
						</td>
						<td>
							<asp:TextBox ID="SysMailPassTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							管理员消息通知邮箱：
						</td>
						<td>
							<asp:TextBox ID="NotifyMailTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
				</table>
			</div>
			<div id="fragment-5">
				<table style="border: solid 0px #fff;">
					<tr>
						<td align="right">
							用户注册验证模式：
						</td>
						<td>
							<asp:DropDownList ID="drpUserRegiseterMode" runat="server" DataTextField="Text" DataValueField="Value" />
						</td>
					</tr>
					<tr>
						<td>
						</td>
						<td>
							<asp:CheckBox ID="AllowSignupCheckBox" runat="server" />允许用户自助注册帐户。
						</td>
					</tr>
					<tr>
						<td>
						</td>
						<td>
							<asp:CheckBox ID="AllCutCheckBox" runat="server" />允许用户裁剪图片。
						</td>
					</tr>
					<tr>
						<td>
						</td>
						<td>
							<asp:CheckBox ID="IsHashedPasswordCheckBox" runat="server" Enabled="False" />管理员密码加密
						</td>
					</tr>
					<tr>
						<td>
						</td>
						<td>
							<asp:CheckBox ID="IsAddLogCheckBox" runat="server" />开启日志记录功能
						</td>
					</tr>
					<tr>
						<td>
						</td>
						<td>
							<asp:CheckBox ID="IsAuditCommentCheckBox" runat="server" />站点评论审核后发布
						</td>
					</tr>
					<tr style="display: none;">
						<td align="right">
							统一用户管理类型：
						</td>
						<td>
							<asp:TextBox ID="GenericUserManageTextBox" runat="server" Columns="60"></asp:TextBox>
						</td>
					</tr>
					<tr style="display: none;">
						<td align="right">
							广告站点地址：
						</td>
						<td>
							http://<asp:TextBox ID="ADUrlTextBox" runat="server" Columns="60"></asp:TextBox>
							<a href="javascript:showNotice(divADUrlTextBoxExp)">[?]</a>
							<br />
							<span id="divADUrlTextBoxExp" class="notice" style="display: none">此处请填写站群广告管理的网站地址。
							</span>
						</td>
					</tr>
					<tr style="display: none;">
						<td align="right">
							开启缓存系统：
						</td>
						<td>
							<input type="checkbox" id="EnableCache" checked="checked" runat="server" onclick="startCache();" />
							<asp:TextBox ID="CacheTimeSpanTextBox" runat="server" Text="60" Enabled="false"></asp:TextBox>秒
						</td>
					</tr>
					<tr>
						<td>
						</td>
						<td>
							<asp:CheckBox ID="OnlyLoginUserCanVisitCheckBox" runat="server" />仅允许登录用户访问网站
						</td>
					</tr>
					<tr>
						<td align="right">
							启用模板地图：
						</td>
						<td>
							<input type="checkbox" id="StartTemplateMapCheckbox" runat="server" />
						</td>
					</tr>
					<tr>
						<td align="right">
							登录是否需要验证：
						</td>
						<td>
							<asp:CheckBox ID="EnableLoginAuhenCodeCheckBox" runat="server" />
						</td>
					</tr>
					<tr>
						<td align="right">
							允许文章具备父子关系：
						</td>
						<td>
							<asp:CheckBox ID="AllowParentArticleCheckBox" runat="server" />
						</td>
					</tr>
					<tr>
						<td align="right">
							启用内容模型单表存储：
						</td>
						<td>
							<asp:CheckBox ID="EnableSingleTable" runat="server" />
						</td>
					</tr>
					<tr>
						<td align="right">
							默认模板编辑器使用可视化编辑器：
						</td>
						<td>
							<asp:CheckBox ID="UseVisualTemplateCheckBox" runat="server" />
						</td>
					</tr>
					<tr>
						<td align="right">
							启用Html静态化的模板：
						</td>
						<td>
							<asp:CheckBox ID="EnableHtmlTemplate" runat="server" />
						</td>
					</tr>
				</table>
			</div>
			<div id="fragment-6" style="min-height: 800px;">
				&nbsp;<uc1:System_image ID="System_image1" runat="server" />
			</div>
			<div id="fragment-7" style="min-height: 800px;">
				<iframe id="ipstrategy" width="500px" height="400px" frameborder="0" runat="server"
					src="SystemStrategy.aspx" scrolling="no"></iframe>
				<asp:HiddenField ID="hddnIPStrategy" runat="server" />
			</div>
		</div>
	</div>
	<div style="display: none">
		<asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
		<asp:TextBox ID="DefaultTemplateGroupFileNameTextBox" runat="server"></asp:TextBox>
	</div>
	<script type="text/javascript">		$("#container-1 ul:eq(0) li").length == 7 ? $("#fragment-7").show() : $("#fragment-7").hide();</script>
</asp:Content>
