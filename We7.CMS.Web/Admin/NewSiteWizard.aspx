<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewSiteWizard.aspx.cs"
	Inherits="We7.CMS.Web.Admin.NewSiteWizard" MasterPageFile="~/admin/theme/classic/ContentNoMenu.Master" %>

<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
	<style type="text/css">
        .styleName
        {
            height: 22px;
            width: 100px;
            text-align: right;
        }
        .styleText
        {
            height: 22px;
            text-align: left;
        }
    </style>
	<script language="javascript">
        function check(radio) {
            var radioButtons = document.getElementsByTagName("input");
            for (var i = 0; i < radioButtons.length; i++) {
                if (radioButtons[i].type == "radio") {
                    if (radioButtons[i] == radio) {
                        continue;
                    }
                    else {
                        radioButtons[i].checked = false;
                    }
                }
            }
        }
	</script>
	<center>
		<div id="dvContent" style="width: 620px; height: 100%; margin-top: 80px; border: solid 10px #f0f0f0;
			text-align: left; padding-left: 15px; display: table;">
			<h2 class="title">
				<asp:Image ID="Image1" runat="server" ImageUrl="~/admin/Images/icons_look.gif" />
				<asp:Label ID="TitleLabel" runat="server" Text="新建站点向导">
				</asp:Label>
				<span class="summary">
					<asp:Label ID="SummaryLabel" runat="server" Text="分三步创建新站点">
					</asp:Label></span>
			</h2>
			<asp:Panel runat="Server" ID="ContentPanel">
				<div id="breadcrumb">
					<div class="Content">
						<asp:Panel ID="pnlSiteConfig" runat="server" Visible="true">
							<h2 class="title">
								<ul style="font-size: 16px; margin-top: 20px; padding-left: 30px;">
									<span style="color: Red">①设置站点参数</span> ②选择模板组 ③完成
								</ul>
							</h2>
							<div style="padding-left: 10px; padding-top: 3px">
								<table width="95%" align="center" cellpadding="3" cellspacing="3">
									<tr>
										<td width="100" align="left">
											站点名称：
										</td>
										<td align="left">
											<asp:TextBox class="colorblur" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';"
												Columns="35" MaxLength="50" ID="txtSiteName" runat="server" /><font color="red">*</font>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSiteName"
												ErrorMessage="请填写站点名称" Display="Dynamic" runat="server" />
											<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSiteName"
												ValidationExpression="[^']+" ErrorMessage="*" Display="Dynamic" />
										</td>
									</tr>
									<tr>
										<td align="left">
											公司名称：
										</td>
										<td align="left">
											<asp:TextBox ID="txtSiteFullName" runat="server" class="colorblur" Columns="35" MaxLength="100"
												onblur="this.className='colorblur';" onfocus="this.className='colorfocus';" /><font
													color="red">*</font>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSiteFullName"
												ErrorMessage="请填写公司名称"></asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr>
										<td align="left">
											站点logo：
										</td>
										<td align="left">												
												<img id="logo_preview" src="<%=ImageValue.Text%>" alt="logo预览图" style="max-width:175px;" /><br/>
											<asp:TextBox ID="ImageValue" runat="server" Width="238" MaxLength="400" /><font color="red">*</font><asp:RequiredFieldValidator
												ID="RequiredFieldValidator4" runat="server" ControlToValidate="ImageValue" ErrorMessage="请您上传站点logo"></asp:RequiredFieldValidator><br />
											<asp:FileUpload ID="fuImage" runat="server" CssClass="file_style" Style="width: 200px;" />&nbsp;&nbsp;
											<asp:Button ID="bttnUpload" runat="server" Text="上传" CssClass="button_style" OnClick="bttnUpload_Click"
												CausesValidation="false" />
										</td>
									</tr>
									<tr>
										<td align="left">
											版权：
										</td>
										<td align="left">
											<asp:TextBox class="colorblur" Style="float: left;" onfocus="this.className='colorfocus';"
												onblur="this.className='colorblur';" Columns="35" MaxLength="100" ID="txtCopyright"
												runat="server" TextMode="MultiLine" Width="380" Height="80" /><font color="red">*</font>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtCopyright"
												ErrorMessage="请填写版权" Display="Dynamic" runat="server" />
										</td>
									</tr>
									<tr>
										<td align="left">
											网站备案信息：
										</td>
										<td align="left" valign="top">
											<asp:TextBox ID="txtIcpInfo" Style="float: left;" runat="server" class="colorblur"
												Columns="35" MaxLength="100" onblur="this.className='colorblur';" onfocus="this.className='colorfocus';"
												TextMode="MultiLine" Width="380" Height="80" /><font color="red">*</font>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtIcpInfo"
												ErrorMessage="请填写网站备案信息" Display="Dynamic" runat="server" />
										</td>
									</tr>
									<tr>
										<td align="left">
											&nbsp;<asp:Literal ID="ltlMsg" runat="server"></asp:Literal>
										</td>
										<td align="left">
											&nbsp;
										</td>
									</tr>
								</table>
							</div>
						</asp:Panel>
						<asp:Panel ID="pnlSiteTemplate" runat="server" Visible="false">
							<h2 class="title">
								<ul style="font-size: 16px; margin-top: 20px; padding-left: 30px;">
									①设置站点参数<span style="color: Red">②选择模板组</span> ③完成
								</ul>
							</h2>
							<div class="toolbar">
								<li style="display: none" class="smallButton4"><a class="editAction" href="Template/TemplateGroupInfo.aspx">
									创建模板组</a></li>
								<li class="smallButton4">
									<asp:HyperLink Enabled="false" ID="UploadHyperLink" NavigateUrl="~/admin/Plugin/PluginAdd.aspx"
										runat="server" Visible="false">
                                                    上传模板组</asp:HyperLink></li>
							</div>
							<br />
							<div id="conbox">
								<dl>
									<dt>»当前模板组
										<div id="fragment-1">
											<h3>
												<span>
													<asp:Label ID="UseTemplateGroupsLabel" runat="server" Text=""> </asp:Label>
												</span>
											</h3>
											<asp:DataList ID="UseTemplateGroupsDataList" Width="15%" BorderWidth="0" CellSpacing="10"
												CellPadding="2" runat="server" ShowFooter="False" ShowHeader="False" RepeatDirection="Horizontal"
												ItemStyle-VerticalAlign="Top" RepeatColumns="3">
												<ItemTemplate>
													<table width="100">
														<tr>
															<td align="Center">
																<asp:Literal runat="server" Text='<%# CheckLength(DataBinder.Eval(((DataListItem)Container).DataItem,"Name").ToString(),15) %>'
																	ID="LiteralName" />
															</td>
														</tr>
														<tr>
															<td valign="top" align="center">
																<img style="border-color: Black; border-width: 1px" src='<%# GetImageUrl((string)DataBinder.Eval(Container.DataItem, "FileName")) %>'
																	border="1" width="200" height="140" />
															</td>
														</tr>
														<tr style="display: none;">
															<td align="center">
																<asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),"编辑")%>'
																	Text="编辑" ID="HyperLinkEdit" />
																<asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),DataBinder.Eval(((DataListItem)Container).DataItem, "Name").ToString(),"删除")%>'
																	Text="删除" ID="HyperLinkDelete" />
																<asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),"打包")%>'
																	Text="打包下载" ID="HyperLinkDown" />
															</td>
														</tr>
													</table>
												</ItemTemplate>
												<ItemStyle VerticalAlign="Top" />
											</asp:DataList>
										</div>
										<dt>»可选模板组
											<div id="fragment-2">
												<asp:DataList ID="TemplateGroupsDataList" Width="15%" BorderWidth="0" CellSpacing="20"
													CellPadding="2" runat="server" ShowFooter="False" ShowHeader="False" RepeatDirection="Horizontal"
													ItemStyle-VerticalAlign="Top" RepeatColumns="3">
													<ItemTemplate>
														<table width="100">
															<tr>
																<td align="Center">
																	<asp:Literal runat="server" Text='<%# CheckLength(DataBinder.Eval(((DataListItem)Container).DataItem,"Name").ToString(),8) %>'
																		ID="LiteralName" />
																</td>
															</tr>
															<tr>
																<td valign="top" align="center">
																	<img style="border-color: Black; border-width: 1px" src='<%# GetImageUrl((string)DataBinder.Eval(Container.DataItem, "FileName")) %>'
																		border="1" width="200" height="140" />
																</td>
															</tr>
															<tr>
																<td align="center">
																	<asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),DataBinder.Eval(((DataListItem)Container).DataItem, "Name").ToString(),"应用")%>'
																		Text="使用此模板" ID="HyperLinkUsed" />
																	<span style="display: none">
																		<asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),DataBinder.Eval(((DataListItem)Container).DataItem, "Name").ToString(),"删除")%>'
																			Text="删除" ID="HyperLinkDelete" />
																		<asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),"打包")%>'
																			Text="打包下载" ID="HyperLink1" />
																		使用此模板 </span>
																</td>
															</tr>
														</table>
													</ItemTemplate>
													<ItemStyle VerticalAlign="Top" />
												</asp:DataList>
											</div>
								</dl>
							</div>
							<div style="display: none">
								<asp:Button runat="server" ID="deleteGroupButton" OnClick="deleteGroupButton_Click" />
								<asp:Button runat="server" ID="applyGroupButton" OnClick="applyGroupButton_Click" />
								<asp:TextBox runat="server" ID="currentGroup" />
							</div>
						</asp:Panel>
					</div>
					<div style="display: none">
						<asp:TextBox ID="ParentTextBox" runat="server" Text="0"></asp:TextBox>
					</div>
				</div>
			</asp:Panel>
			<asp:Panel runat="server" ID="pnlSuccess" Visible="false">
				<h2 class="title">
					<ul style="font-size: 16px; margin-top: 20px; padding-left: 30px;">
						①设置站点参数 ②选择模板组 <span style="color: Red">③完成</span>
					</ul>
				</h2>
				<table width="100%" style="line-height: 32px;">
					<tr>
						<td style="width: 60px;" valign="top" align="right">
							<img src="/Admin/images/success.jpg" />
						</td>
						<td align="left">
							<% if (We7.Framework.AppCtx.IsDemoSite)
                               { %>
							<strong>演示站点不能修改数据！</strong>
							<%}
                               else
                               { %>
							<strong>恭喜，站点[<asp:Label ID="lblSiteName" runat="server" Text=""></asp:Label>]创建成功！</strong>
							<%} %>
						</td>
					</tr>
					<tr>
						<td valign="top" align="center" colspan="2">
							<a href="default.aspx" style="font-weight: bolder; font-size: 16px;">进入管理后台</a>
						</td>
					</tr>
				</table>
			</asp:Panel>
			<div style="background-position: center 50%; background-image: url(images/line.gif);
				background-repeat: repeat-x">
			</div>
			<div>
				<table cellpadding="0" cellspacing="0" width="60%" border="0">
					<tr>
						<td align="center">
							<span style="padding-right: 30"><span style="padding-right: 30"></span><span style="padding-right: 30">
							</span>
								<asp:Button class="button" ID="btnPrevious" OnClick="PreviousPanel" CausesValidation="false"
									runat="server" Text="< 上一步"></asp:Button>
								&nbsp;
								<asp:Button class="button" ID="btnNext" OnClick="btnNextPanel" runat="server" Text="下一步 >">
								</asp:Button>
								<span style="padding-right: 30"></span></span>
						</td>
					</tr>
				</table>
			</div>
			<div style="">
				<br />
				<asp:Label ID="MessageLabel" runat="server" Text=""> </asp:Label>
			</div>
		</div>
	</center>
	<script type="text/javascript">
        function deleteGroup(name, filename) {
            if (confirm("您确认要删除模板组 " + name + " 吗？")) {
                var btn = document.getElementById("<%=deleteGroupButton.ClientID %>");
                document.getElementById("<%=currentGroup.ClientID %>").value = filename;
                if (btn) btn.click();
            }
        }
        function applyGroup(name, filename) {
            if (confirm("您确认要使用模板组 " + name + " 吗？")) {
                var btn = document.getElementById("<%=applyGroupButton.ClientID %>");
                document.getElementById("<%=currentGroup.ClientID %>").value = filename;
                if (btn) btn.click();
            }
              }
              $("#<%=ImageValue.ClientID %>").blur(function () { $("#logo_preview").attr("src", $(this).val()); });      
	</script>
</asp:Content>
