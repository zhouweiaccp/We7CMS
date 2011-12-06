<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserRegister.Simple.cs"
	Inherits="We7.CMS.Web.Widgets.UserRegister" %>
<%@ Register Src="~/ModelUI/Panel/system/SimpleEditorPanel.ascx" TagName="EditorPanel"
	TagPrefix="uc1" %>
<script type="text/C#" runat="server">
	[ControlDescription(Desc = "用户注册", Author = "系统")]
	string MetaData;
</script>
<style type="text/css">
	.button_style
	{
		background: url("<%=TemplateSourceDirectory %>/images/bg_admin.jpg") repeat-x scroll 0 -51px transparent;
	}
	.table_reg
	{
		line-height: 30px;
		width: 480px;
	}
	.table_reg caption
	{
		background: url("<%=TemplateSourceDirectory %>/images/login_bg.gif") no-repeat scroll -1px -77px transparent;
	}
	span.yes, span.no
	{
		background: url("<%=TemplateSourceDirectory %>/images/member_bg_1.gif") no-repeat scroll 0 -418px transparent;
		color: #007700;
		height: 18px;
		line-height: 18px;
		padding: 0.4em 0 0 1.6em;
	}
	span.no
	{
		background-position: 0 -450px;
		color: #CC0000;
	}
</style>
<div class="<%=CssClass %>">
	<div id="protocol" style="display: none">
		<table cellspacing="1" cellpadding="0" class="table_reg">
			<caption>
				注册协议
			</caption>
			<tr>
				<td>
					<textarea readonly="readonly" cols="50%" rows="10"><%=Protocol %></textarea>
				</td>
			</tr>
			<tr>
				<td id="tdAgreen">
					<input type="checkbox" id="agreen" />
					同意注册协议
				</td>
			</tr>
			<tr>
				<td>
					<input type="button" value="下一步" id="agreenNext" class="button_style" />&nbsp;&nbsp;<span
						class="no">选择同意注册协议后,才能进行注册。</span>
				</td>
			</tr>
		</table>
	</div>
	<div id="register">
		<table cellspacing="1" cellpadding="0" class="table_reg">
			<caption>
				<span style="float: left; margin-left: 150px;">新用户注册</span>
			</caption>
			<tbody>
				<tr>
					<th width="120px;">
						用户名：
					</th>
					<td width="*">
						<input id="txtUserName" maxlength="20" require="true" datatype="limit|ajax" min="3"
							url="<%=TemplateSourceDirectory %>/Validate.ashx?action=user" max="20" msg="3—20个字符|" />
					</td>
				</tr>
				<tr>
					<th>
						密 码：
					</th>
					<td>
						<input id="txtPassword" name="txtPassword" type="password" require="true" datatype="limit"
							min="6" max="16" msg="6—16个字符!" />
					</td>
				</tr>
				<tr>
					<th>
						确认密码：
					</th>
					<td>
						<input id="txtRePassword" type="password" require="true" datatype="repeat" msg="密码不一致"
							to="txtPassword" maxlength="20" />
					</td>
				</tr>
				<tr>
					<th>
						Email地址：
					</th>
					<td>
						<input id="txtEmail" require="true" datatype="email|ajax" msg="格式不正确！" url="<%=TemplateSourceDirectory %>/Validate.ashx?action=email" />
					</td>
				</tr>
				<tr>
					<th>
						验证码：
					</th>
					<td valign="top">
						<input id="txtValCode" require="true" datatype="limit" min="5" max="5" msgid="code"
							msg="验证码不正确" /><img id="imgVerify" src="/Install/VerifyCode.aspx?" alt="看不清？点击更换"
								onclick="this.src=this.src+'?'" style="margin-bottom: -18px; cursor: pointer;" />&nbsp;<span
									class="color_1"></span><span id="code">&nbsp;</span>
					</td>
				</tr>
				<tr>
					<th>
					</th>
					<td height="40px;">
						<input id="bttnRegister" value="  注  册  " type="button" class="button_style" />&nbsp;&nbsp;&nbsp;
						<input type="reset" value="  重  置  " class="button_style" /><br />
						<label id="lblError" class="error">
						</label>
					</td>
				</tr>
			</tbody>
		</table>
	</div>
	<div id="userinfo" style="display: none">
		<table cellspacing="1" cellpadding="0" class="table_reg">
			<caption>
				用户详细信息
			</caption>
			<tbody>
				<tr>
					<td>
					</td>
				</tr>
			</tbody>
		</table>
		<%-- <input id="feid" name="feid" type="hidden" value="<%=AccountID %>" />--%></div>
	<div id="registermsg" style="display: none">
		<table cellspacing="1" cellpadding="0" class="table_reg">
			<caption>
				新用户注册
			</caption>
			<tbody>
				<tr>
					<td>
						
					</td>
				</tr>
			</tbody>
		</table>
	</div>
	<script type="text/javascript" src="<%=TemplateSourceDirectory %>/JS/common.js"></script>
	<script type="text/javascript" src="<%=TemplateSourceDirectory %>/JS/validator.js"></script>
	<script type="text/javascript">
		var set_show = '<%=ShowProtocol %>';
		var accountID = '<%=AccountID %>';
		$("#agreenNext").click(function () {
			if ($("#tdAgreen input").attr("checked") == false)
				$("#tdAgreen").css("color", "red");
			else {
				$("#tdAgreen").css("color", "black");
				$("#protocol").hide();
				$("#register").show();
			}
		});

		if (set_show.toLowerCase() == "true") {
			$("#protocol").show();
			$("#register").hide();
		}
		$(function () {
			$('#register').checkForm(1);
		});
		if (accountID != '') {
			$.ajax({
				url: '<%=TemplateSourceDirectory %>/Validate.ashx?action=validate',
				type: "POST",
				data: 'AccountID=' + accountID,
				cache: false,
				async: false,
				success: function (response) {
					if (response == "success") {
						$("#protocol").hide();
						$("#registermsg").show();
						$("#register").hide();
						$("#registermsg table tr td").html('恭喜您，验证成功！<a href="/">返回首页</a>');
					}
					else $("#registermsg table tr td").html(response);
				}
			});
		}
		$("#bttnRegister").click(function () {

			$.ajax({
				url: '<%=TemplateSourceDirectory %>/Validate.ashx?action=submit&url=<%=HttpUtility.UrlEncode(Request.RawUrl)%>',
				type: "POST",
				data: 'name=' + $("#txtUserName").val() + '&pwd=' + $("#txtPassword").val() + '&email=' + $("#txtEmail").val(),
				cache: false,
				async: false,
				success: function (response) {
					var msg = '';
					if (response == "success") {
						$("#registermsg").show();
						$("#register").hide();
						msg = '恭喜您，注册成功！<a href="/">返回首页</a>';
					}
					else if(response=="success:email") {
						$("#registermsg").show();
						$("#register").hide();
						msg = '恭喜您，注册成功！系统已发验证邮件至您的注册邮箱中，请确认。<a href="/">返回首页</a>';
					}
					else { $("#lblError").text(response); }
					$("#registermsg table tr td").html(msg);
				}
			});
		});
	</script>
</div>
