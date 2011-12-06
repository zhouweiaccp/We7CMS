<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/theme/classic/content.Master"
    AutoEventWireup="true" EnableEventValidation="false" CodeBehind="EditModel.aspx.cs"
    Inherits="We7.CMS.Web.Admin.ContentModel.EditModel" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
<script type="text/javascript" src="/scripts/we7/we7.loader.js">
	$(document).ready(function () {
		we7('span[rel=xml-hint]').help();
		we7("#form-region").attachValidator({
			inputEvent: 'keyup'
		});
		we7.addValidateType("[rel=letters]", "标识符必须为数字或英文字母", function (input, value) {
			return /^[a-z0-9]+$/gi.test(value);
		});

		var msg = $("#message_panel").find("div.MessagePanel td:last").html();

		if ($.trim(msg)) {
			we7.info(msg, { autoHide: false });
		}
	});
</script>
    <% if (!IsEdit)
       { %>
<script type="text/javascript">
	$(function () {
		$("#ctAdd,#ctDel").css({ "background": '#FFFFFF', 'display': 'none' });
		$("#addgroup").click(function (e) {
			$("#msg").text("");
			$("#ctAdd").fadeIn();
			$("#ctDel").fadeOut();
		});
		we7.load.ready(function () {
			we7("#ctAdd").attachValidator({
				onInputSuccess: function (e, input) {
					setTimeout(function () {
						var msg = input.data("msg.el");
						if (msg) {
							msg.css("z-index", 2000);
						}
					}, 10);
				}
				, onInputFail: function (e, input) {
					setTimeout(function () {
						var msg = input.data("msg.el");
						if (msg) {
							msg.css("z-index", 2000);
						}
					}, 20);
				}
			});
		});
		function resetValidator() {
			var validator = we7("#ctAdd").getValidator();
			if (validator) {
				validator.reset(); 	// 重置验证器
			}
		}
		var delMsg = $("#delmsg");
		$("#delgroup").click(function (e) {
			function DeleteItem(val, successhandler) {
				delMsg.show().text("数据处理中");
				$.ajax({
					url: '/Admin/ContentModel/ajax/ContentModel.asmx/DeleteModelGroup',
					data: "{'group':'" + val + "'}",
					dataType: 'json',
					contentType: 'application/json; charset=utf-8',
					type: 'post',
					success: function (doc, status, xh) {
						if (doc.indexOf('true') > -1) {
							if (successhandler != null)
								successhandler();
							delMsg.show().text("删除成功");
							setTimeout(function () { delMsg.hide(); }, 3500); /*TODO:如果用户继续操作后刚好到达3500时需要显示其新的操作之结果时，可能导致新的操作结果被隐藏*/
						} else {
							delMsg.show().text("组中可能含有模型，无法删除");
							setTimeout(function () { delMsg.hide(); }, 3500);
						}
					},
					error: function () {
						delMsg.show().text("网络错误，删除失败");
						setTimeout(function () { delMsg.hide(); }, 3500);
					}
				});
			}
			function LoadData() {
				$("#ctList").text('');
				delMsg.show().text("加载数据...");
				$.ajax({
					url: '/Admin/ContentModel/ajax/ContentModel.asmx/LoadModelGroup',
					dataType: 'json',
					contentType: 'application/json; charset=utf-8',
					type: 'post',
					success: function (doc, status, xh) {
						if (doc) {
							var result = eval("(" + doc + ")");
							if (result && result.success && result.data) {
								for (var i = 0; i < result.data.length; i++) {
									var row = $("<div />");
									row.css("border-bottom", "solid 1px #e0e0e0").append($("<span>" + result.data[i].txt + "&nbsp;&nbsp;&nbsp;&nbsp;</span>")).append($("<span>X</span>").data('val', result.data[i].val).css({ cursor: 'pointer' }).data("row", row).click(function () {
										var val = $(this).data("val");
										var obj = this;
										if (obj.locked) return;
										obj.locked = true;
										DeleteItem(val, function () {
											$(obj).data("row").remove();
											var sl = $("#<%=GroupDropDownList.ClientID %>")[0];
											var ops = sl.options;
											for (var j = 0; j < ops.length; j++) {
												if (ops[j].value == val) {
													if (ops.remove)
														ops.remove(j);
													else
														sl.remove(j);
													break;
												}
											}
											if (ops.length > 0) ops[0].selected = true;
										});
									}));
									$("#ctList").append(row);
								}
							}
							else {
								$("#ctList").innerText = "当前模型组为空";
							}
						}
						else {
							$("#ctList").innerText = "当前模型组为空";
						}
						delMsg.hide().text("");
					},
					error: function (xhr, status, err) {
						var e = err.message ? err.message : err.toString();
						if (we7.alert) { we7.alert(e, "载入出错", {autoClose:false}) }
						else { alert(e); }
					}
				});
			}
			LoadData();
			$("#ctDel").fadeIn();
			$("#ctAdd").fadeOut();
		});
		$("#btnAddGroup").click(function () {
			if (!we7("#ctAdd").validate()) { return; }
			$("#msg").text('数据处理中');
			$.ajax({
				url: '/Admin/ContentModel/ajax/ContentModel.asmx/CreateModelGroup',
				data: "{cnName:'" + $("#cngn").val() + "',enName:'" + $("#engn").val() + "'}",
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				type: 'post',
				success: function (doc, status, xh) {
					if (doc) {
						var result = eval("(" + doc + ")");
						if (result && result.success) {
							var op = new Option();
							op.text = $("#cngn").val();
							op.value = $("#engn").val();
							op.selected = true;
							$("#<%=GroupDropDownList.ClientID %>")[0].options.add(op);
							resetValidator();
							$("#ctAdd").fadeOut();
							$("#engn").val("");
							$("#cngn").val("");
						}
						else {
							$("#msg").text("添加失败:" + result.msg);
						}
					}
					else {
						$("#msg").text("添加失败,应用程序处理错误!");
					}
				},
				error: function () {
					$("#msg").text("添加失败");
				}
			});
		});
		$("#btnCancelGroup").click(function () {
			resetValidator();
			$("#ctAdd").fadeOut();
		});
		$("#delClose").click(function () {
			$("#ctDel").fadeOut();
			delMsg.text("").hide();
		});
	});

   </script>
    <%} %>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text='添加<%= ModelTypeName %>' />
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text='<%= ModelTypeName %>的描述信息' />
        </span>
    </h2>
	<div id="message_panel" style="display:none"><WEC:MessagePanel ID="Messages" runat="server"></WEC:MessagePanel></div>
    <div id="form-region">
        <table>
            <tr>
                <td align="right">
                    分组
                </td>
                <td>
                    <asp:DropDownList ID="GroupDropDownList" runat="server" Width="200px" >
                    </asp:DropDownList>
                </td>
                <td>
                    <% if (!IsEdit)
                       { %>
                    <a id="addgroup" style="cursor: pointer; font-size: 14px; color: Red; text-decoration: underline;">
                        新增</a>&nbsp;&nbsp; <a id="delgroup" style="cursor: pointer; font-size: 14px; color: Red;
                            text-decoration: underline">删除</a>
                   <%} %>
                </td>
            </tr>
            <tr>
                <td align="right">
                    名称
                </td>
                <td>
                    <asp:TextBox ID="ModelLabelTextBox" runat="server" Width="200px" MaxLength="50" required="required"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="right">
                    配置文件
                </td>
                <td>
                    <asp:TextBox ID="ModelNameTextBox" rel="letters" runat="server" Width="200px" MaxLength="50" required="required"  pattern="[a-zA-Z0-9]+" errmsg="模型配置文件名称，请使用字母与数字"></asp:TextBox>
                </td>
                <td>
                    .xml<span rel="xml-hint" title="模型配置文件名称，请使用字母与数字" style="margin-left:10px" ></span>
                </td>
            </tr>
            <tr>
                <td align="right">
                    详细描述
                </td>
                <td>
                    <asp:TextBox ID="DescriptionTextBox" runat="server" TextMode="MultiLine" Width="200px" Height="100px" MaxLength="500"></asp:TextBox>
                </td>
                 <td>
                </td>
            </tr>
            <%if (MyModelType == "Template.AccountModel")
              { %>
            <tr>
                <td align="right">
                    用户默认角色
                </td>
                <td>
                    <asp:TextBox ID="RoleTextBox" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <%}

              if (MyModelType == "Template.ArticleModel")
              { %>
            <tr>
                <td align="right">
                    启用任意浏览权限
                </td>
                <td>
                    <asp:CheckBox ID="AuthorityTypeCheckBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    关联反馈模型
                </td>
                <td>
                    <asp:DropDownList ID="ddlRelationModelName" runat="server" Width="200px">
                    </asp:DropDownList>
                </td>
            </tr>
            <%} %>
            <tr>
                <td align="right">
                    状态
                </td>
                <td>
                    <asp:DropDownList ID="ModelStateDropDownList" runat="server" Width="200px">
                        <asp:ListItem Selected="True" Text="开启" Value="1"></asp:ListItem>
                        <asp:ListItem Text="关闭" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <div class="toolbar" style="margin-left: 100px">
        <asp:HyperLink ID="SaveHyperLink" NavigateUrl="javascript:SubmitClick();" runat="server">
            保存
        </asp:HyperLink>
        <a href="/Admin/ContentModel/Models.aspx?modelType=<%=MyModelType %>" >返回列表</a>
    </div>
    <div style="display: none">
        <asp:Button ID="SubmitsButton" runat="server" Text="Save" OnClick="SubmitButton_Click"
            ValidationGroup="SubmitButton" />
    </div>
	<%if(!IsEdit) {%>
	<div id="ctAdd" style="position:absolute;left:445px;top:150px;padding:3px; border:solid 1px #e0e0e0;z-index:1000">
        <div style="border:solid 1px #f0f0f0;">
            <table>
                <tr>
                    <td>
                        模型组名称:
                    </td>
                    <td>
                        <input id="cngn" type="text" required="required"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        标识符：
                    </td>
                    <td>
                        <input id="engn" type="text" required="required" rel="letters" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input id="btnAddGroup" style="width: 60px; padding: 0;" class="Btn" type="button"
                            value="添加" />
                        <input id="btnCancelGroup" style="width: 60px; padding: 0" class="Btn" type="button"
                            value="取消" />
                        <label id="msg">
                        </label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="ctDel" style="position:absolute;left:485px;top:150px;padding: 3px; border: solid 1px #e0e0e0;z-index:1000">
        <div style="border: solid 1px #f0f0f0;">
            <div id="delClose" style="text-align: right; cursor: pointer; border-bottom: solid 1px #e0e0e0">
                X</div>
            <div id="ctList" style="padding: 0 3px 3px 3px; width: 200px">
            </div>
            <div id="delmsg" style="font-weight: bold;">
                数据处理中...</div>
        </div>
    </div>
	<%}%>
    <script type="text/javascript">
        function SubmitClick() {
            var submitBtn = document.getElementById("<%=SubmitsButton.ClientID %>");
			var div = $("#form-region");
			var enable = we7(div).validate();
			if (enable) {
				submitBtn.click();
			}
        }
    </script>
</asp:Content>
