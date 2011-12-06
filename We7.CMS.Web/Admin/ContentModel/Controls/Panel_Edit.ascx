<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Panel_Edit.ascx.cs"
	Inherits="We7.CMS.Web.Admin.ContentModel.Controls.UserCenter_Edit" %>

<div id="stage" class="afi">
	<div id="side" style="cursor: pointer;">
		<ul id="tabs">
			<li><a id="addControl" href="#controlsContainer">添加控件</a></li>
			<li><a id="setting" href="#fieldProperties">属性设置</a></li>
			<li><a id="pageProperty" href="#pageProperties">页面属性</a></li>
		</ul>
		<div id="controlsContainer">
			<ul id="addFields">
			</ul>
		</div>
		<div id="fieldProperties">
			<div id="props">
			</div>
		</div>
		<div id="pageProperties">
			<div id="pProperties">
				<table>
					<tr>
						<td>
							页面标记：
						</td>
						<td>
							<select>
							</select>
						</td>
					</tr>
					<tr style="display:none;">
						<td>
							后继页面：
						</td>
						<td>
							<select>
							</select>
						</td>
					</tr>
					<tr>
						<td>
							启用：
						</td>
						<td>
							<select>
								<option value="false">否</option>
								<option value="true">是</option>
							</select>
						</td>
					</tr>
				</table>
			</div>
		</div>
	</div>
	<div id="main">
		<div class="info">
			<h2>
				<asp:Literal runat="server" ID="FormTitleLiteral"></asp:Literal></h2>

               <!-- <div class="" id="formButtons"> -->
               <div >
			<table cellspacing="0">
				<tbody>
					<tr>
						<td style="width: 180px">
							<table id="copyControls" runat="server" visible="false" style="width: 180px">
								<tr>
									<td style="width: 12px">
										<input type="checkbox" id="copyToUser" />
									</td>
									<td>
										<label for="copyToUser">
											同步更新会员中心表单</label>
									</td>
								</tr>
							</table>
						</td>
						<td align="left" style="width: 100px">
							<a class="button" id="btnSave" href="javascript:void(0);">保存</a>
						</td>
						<td>
							<a class="button" id="btnCopy" href="javascript:void(0);" style="display: none;">复制</a>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
			<div>
				<asp:Literal runat="server" ID="FormDesciptionLiteral"></asp:Literal></div>
		</div>
		<ul id="formFields" style="min-height: 400px;">
		</ul>
		
	</div>
	<div class="clear">
	</div>
</div>
<style type="text/css">
	.we7-dialog_warp
	{
		width: 350px;
		height: 30px;
		line-height: 20px;
	}
	.we7-dialog_warp > input
	{
	}
	#groupList span
	{
		padding: 5px 20px 5px 5px;
		display: inline-block;
	}
	#groupList span img
	{
		cursor: hand;
	}
</style>
<div id="dialog" style="padding: 15px; display: none; background-color: #cad5eb;
	font-size: 12px; color: #000000; font-weight: bold;">
	<div class="we7-dialog_warp">
		<label>
			显示名称</label><input id="dialog_label" type="text" />（尽量使用中文）</div>
	<div class="we7-dialog_warp">
		<label>
			英文名称</label><input id="dialog_name" type="text" />（仅允许数字与字母）</div>
	<div class="we7-dialog_warp">
		<label for="dialog_checkTitleField">
			是否为标题</label>
		<input type="checkbox" name="dialog_checkTitleField" id="dialog_checkTitleField" /></div>
	<div class="we7-dialog_warp">
		<label for="dialog_checkSearchField">
			查询中显示</label>
		<input type="checkbox" name="dialog_checkSearchField" id="dialog_checkSearchField" /></div>
	<div class="we7-dialog_warp">
		<label for="dialog_dataType">
			数据类型</label><select id="dialog_dataType" name="dataType">
				<option value="String">文本</option>
				<option value="Int32">整数</option>
				<option value="Decimal">小数</option>
				<option value="Boolean">是否</option>
				<option value="DateTime">日期</option>
			</select></div>
	<div class="we7-dialog_warp" id="div_maxlength">
		<label>
			字段长度</label>
		<input type="text" id="dialog_maxlength" name="dialog_maxlength" value="25" />
	</div>
	<div class="we7-dialog_warp" style="text-align: center;">
		<input type="button" id="btnConfirm" style="width: 50px;" value="确定" />
	</div>
</div>
<div id="groupManage" style="padding: 15px; display: none; background-color: #cad5eb;
	font-size: 12px; color: #000000; font-weight: bold;">
	<div class="we7-dialog_warp">
		<label>
			名称</label><input id="groupName" type="text" /><input type="button" id="groupConfirm"
				style="width: 50px;" value="添加" /></div>
	<div id="groupList" class="we7-dialog_warp">
	</div>
</div>
<script type="text/javascript">
	//maxlength
	//init
	$(document).ready(function () {
		$("#dialog_dataType").change(function () {
			var selectType = $(this).val();
			if (selectType == 'String') {
				$("#div_maxlength").show();
			}
			else {
				$("#div_maxlength").hide();
			}
		});
	});
</script>
<script type="text/javascript" src="/Admin/ContentModel/js/EditPanel.js"></script>
<script type="text/javascript" src="/Admin/Ajax/jquery/jquery.dimensions.js"></script>
<script type="text/javascript" src="/Admin/Ajax/jquery/jquery.boxy.js"></script>
<!--<script type="text/javascript">
	$(function(){
		$(".boxy").boxy();
	});
</script>
<script language="javascript" type="text/jscript">
	var name = "#controlsContainer";
	var menuYloc = null;

	$(document).ready(function () {
		menuYloc = parseInt($(name).css("top").substring(0, $(name).css("top").indexOf("px")))
		$(window).scroll(function () {
			offset = menuYloc + $(document).scrollTop() + "px";
			$(name).animate({ top: offset }, { duration: 500, queue: false });
		});
	}); 
</script>-->
