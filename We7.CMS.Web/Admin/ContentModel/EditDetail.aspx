<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/theme/classic/content.Master"
    CodeBehind="EditDetail.aspx.cs" Inherits="We7.CMS.Web.Admin.ContentModel.EditDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
    <link href="/Admin/Ajax/jquery/ui1.8.1/css/ui-lightness/jquery-ui-1.8.1.custom.css"
        rel="stylesheet" type="text/css" />
    <link media="screen" rel="stylesheet" href="<%=AppPath%>/ajax/jquery/colorbox/colorbox.css" />
    <script src="/Admin/ContentModel/js/Common.js" type="text/javascript"></script>
    <script src="<%=AppPath%>/ajax/jquery/colorbox/jquery.colorbox-min.js"></script>
    <script type="text/javascript" src="/scripts/we7/we7.loader.js">
      we7("span[rel=xml-hint]").help();
      we7('.tipit').tip();
    </script>
    <div >
        <div id="conbox">
            <%if (Action == "widget")
              { %>
            <dl id="createWidget">
                <dt>» <span class="stitle">创建部件</span><br />
                </dt>
                <dd>
                    <div class="detail">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="font-weight: bold;">
                                    <input type="checkbox" onclick="checkAll('#<%=chklstWidgetList.ClientID %>',this)" />
                                    列表部件显示字段
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 0 0 0 40px">
                                    <asp:CheckBoxList ID="chklstWidgetList" runat="server" RepeatColumns="5" RepeatDirection="Horizontal">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold;">
                                    <input type="checkbox" onclick="checkAll('#<%=chklstWidgetView.ClientID %>',this)" />
                                    详细信息控件显示字段
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 0 0 0 40px">
                                    <asp:CheckBoxList ID="chklstWidgetView" runat="server" RepeatColumns="5" RepeatDirection="Horizontal">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a href="javascript:void(0)" onclick="CreateModelControl()">创建前台部件</a>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="control">
                    </div>
                </dd>
            </dl>
            <%} %>
            <%if (isArticle && Action == "layout")
              { %>
            <dl id="createLayout">
                <dt>» <span class="stitle">自定义布局 </span><span rel="xml-hint" title="您可以通过自定义布局，自己定制内容模型的排列方式和样式等">
                </span>
                    <br />
                    <input type="hidden" id="lpath" />
                </dt>
                <dd >
                    <div class="detail">
                    </div>
                    <div class="op">
                        <br />
                        <br />
                        创建完的自定义布局可以与以下绑定：
                        <br />
                        <br />
                        <table cellpadding="0" cellspacing="0" border="0" >
                            <tr>
                                <td>
                                    后台录入：
                                </td>
                                <td>
                                    <input runat="server" class="htlr " id="chkAE" type="checkbox" v="Layout" /><span
                                        rel="xml-hint" title=" 勾选即可与该控件绑定，取消勾选即可取消绑定"> </span>
                                </td>
                                <td>
                                    <a href="javascript:void(0)" class="editCss" ref="EditCss">编辑样式</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    后台列表：
                                </td>
                                <td>
                                    <input runat="server" class="htlr" id="chkView" type="checkbox" v="ViewerLayout" /><span
                                        rel="xml-hint" title=" 勾选即可与该控件绑定，取消勾选即可取消绑定"> </span>
                                </td>
                                <td>
                                    <a href="javascript:void(0)" class="editCss" ref="ViewerCss">编辑样式</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    前台录入：
                                </td>
                                <td>
                                    <input runat="server" class="htlr " id="chkUC" type="checkbox" v="UcLayout" /><span
                                        rel="xml-hint" title=" 勾选即可与该控件绑定，取消勾选即可取消绑定"> </span>
                                </td>
                                <td>
                                    <a href="javascript:void(0)" class="editCss" ref="UcCss">编辑样式</a>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="control">
                    </div>
                </dd>
            </dl>
            <%} %>
        </div>
    </div>
    <script type="text/javascript">

        function CreateModelLayout() {
            var path = $("#lpath").val();
            if (path.length > 0) {
                if (!confirm("重新创建将覆盖目前已经存在的布局控件，您确定要重新创建吗？"))
                    return false;
            }
            we7.loading("操作中");
            var url = "/Admin/ContentModel/ajax/ContentModel.asmx/CreateModelLayout";
            var data = { model: model };
            var msg;
            $.ajax({
                url: url,
                data: jsonToString(data),
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (json) {
                    var result = stringToJSON(json);
                    if (result.success) {
                        we7.info("创建成功！");
                        $("#lpath").val(result.msg);
                        var t = $("#createLayout .op");
                        $("#createLayout .detail").html(we7.formatStr("{0}　{2}　　<a href=\"javascript:void(0);\" onclick=\"CreateModelLayout()\">{1}</a> ", "已创建", "<a href=\"javascript:void(0);\" onclick=\"EditUxLayout()\">编辑文件</a>", "<a href=\"javascript:void(0);\" onclick=\"CreateModelLayout()\">重新创建</a>"));
                        $("#createLayout .detail").append(t);
                    }
                    else {
                        we7.info("创建失败！错误信息：" + result.msg, { autoHide: true, hideTimeout: 6000 });
                    }
                },
                failure: function () {
                    we7.info("应用程序错误!", { autoHide: true, hideTimeout: 6000 });
                }
            });

        }

        function ModelLayoutBind(isBind, type) {
            var path = $("#lpath").val();
            if (path == "") {
                we7.info("请先创建自定义布局，<a href='javascript:void(0)' onclick='CreateModelLayout()' >点此创建</a>！", { autoHide: false });
                return;
            }
            if (!isBind)
                path = "";
            we7.loading("操作中");
            var url = "/Admin/ContentModel/ajax/ContentModel.asmx/ModelLayoutBind";
            var data = { model: model, path: path, type: type };
            var msg;
            $.ajax({
                url: url,
                data: jsonToString(data),
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (json) {
                    var result = stringToJSON(json);
                    if (result.success) {
                        we7.info("操作成功！");

                    }
                    else {
                        we7.info("操作失败！错误信息：" + result.msg, { autoHide: true, hideTimeout: 6000 });
                    }
                },
                failure: function () {
                    we7.info("应用程序错误!", { autoHide: true, hideTimeout: 6000 });
                }
            });
        }

        function CreateWidgetsIndex() {
            var url = "/Admin/ContentModel/ajax/ContentModel.asmx/CreateModelWidegetsIndex";
            $.ajax({
                url: url,
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (json) {
                    var result = stringToJSON(json);
                    if (result.success) {
                        we7.info("索引创建已重新创建！");
                    }
                    else {
                        we7.info("创建失败！错误信息：" + result.msg, { autoHide: true, hideTimeout: 6000 });
                    }
                },
                failure: function () {
                    we7.info("应用程序错误!", { autoHide: true, hideTimeout: 6000 });
                }
            });
        }

        function CreateModelControl() {
            var lfs = '', dfs = '';
            $("#<%=chklstWidgetList.ClientID  %> input:checkbox[checked]").each(function () {
                lfs += $(this).parent('span').attr("mvalue") + ",";
            });
            $("#<%=chklstWidgetView.ClientID %> input:checkbox[checked]").each(function () {

                dfs += $(this).parent('span').attr("mvalue") + ",";
            });
            if (lfs != '' || dfs != '') {

                var result = '';
                if (confirm("如果已经创建，将会被覆盖成最新，确定要创建吗?")) {
                    we7.loading("操作中");
                    var url = "/Admin/ContentModel/ajax/ContentModel.asmx/CreateModelControls";
                    var data = { model: model, widgetDetailFields: dfs, widgetListFields: lfs };
                    $.ajax({
                        url: url,
                        data: jsonToString(data),
                        type: "POST",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (json) {
                            result = stringToJSON(json);
                            if (result.success) {
                                we7.loading("创建成功,正在重新创建索引文件，请稍后...", { autoHide: false });
                                CreateWidgetsIndex();
                            }
                     
                            else {
                                we7.info("创建失败，错误信息：" + result.msg, { autoHide: true, hideTimeout: 6000 });
                            }
                        }
                    });

                }
            }
            else
                we7.info("您没有选择任何项！");
        }

        function EditUxLayout() {
            var filename = $("#lpath").val();
            if (filename.length > 0) {

                newForm('/Admin/VisualTemplate/WidgetEditor/WidgetEditor.aspx?source=contentmodel&ctr=' + filename + "&t=" + (new Date()).valueOf(), '编辑自定义布局', "100%", "100%");
            }
        }

        function EditLayoutCss(type) {
            var filename = $("#lpath").val().replace(".ascx", "." + type + ".css");
            if (filename.length > 0) {
                var url = "/Admin/ContentModel/UxStyleEditor.aspx?ctr=" + filename + "&model=" + model + "&type=" + type;
                newForm(url, '编辑样式', "100%", "100%");
            }
        }

        //    var form;
        function newForm(url, title, w, h) {
            $.colorbox({ width: w, height: h, href: url, iframe: true,
                overlayClose: false, escKey: false,
                onClosed: function () {
                }
            });

        }

        function CloseChild(msg) {
            $.fn.colorbox.close();
            we7.info(msg);
        }

        function ReloadMenu() {
            if (location.href.indexOf("?") > 0)
                location.href = location.href + '&reload=menu';
            else
                location.href = location.href + '?reload=menu';
        }

        function checkAll(id, el) {
            var checked = el.checked;
            $(id + " input").each(function () {
                this.checked = checked;
            });
        }
        var model;
        $(document).ready(function () {

            $(".htlr").live("change", function () {
                ModelLayoutBind($(this).attr("checked"), $(this).attr("v"));
            });

            $("#chkAll").live("change", function () {
                $(".operator").each(function () {
                    $(this).attr("checked", $("#chkAll").attr("checked"));
                });
            });

            $(".editCss").live("click", function () {
                EditLayoutCss($(this).attr("ref"));
            });
            existMsg=<%=strScript %>;
            existMsg = eval("(" + existMsg + ")");
            model = existMsg.modelName;
            $.each(existMsg.Data, function (i) {
                var t = existMsg.Data[i];
                var a, b = '';
                switch (t.name) {
                    case "createWidget":
                        if (t.exist) {
                            b = " (已创建)";
                        }
                        break;
                    case "createLayout":
                        a = "{0}　{2}　　<a href=\"javascript:void(0);\" onclick=\"CreateModelLayout()\">{1}</a> ";
                        if (t.exist) {
                            b = " (已创建)";
                            $("#lpath").val(t.path);
                            a = we7.formatStr(a, "已创建", "<a href=\"javascript:void(0);\" onclick=\"EditUxLayout()\">编辑文件</a>", "<a href=\"javascript:void(0);\" onclick=\"CreateModelLayout()\">重新创建</a>");
                            $("#" + t.name + " .detail").append($("#" + t.name + " .op"));

                        }
                        else {
                            a = we7.formatStr(a, "尚未创建", "创建", "");
                        }

                        break;
                }
                $("#" + t.name + " .detail").prepend(a);
                $("#" + t.name + " .stitle").append(b);
            });

        });

    </script>
</asp:Content>
