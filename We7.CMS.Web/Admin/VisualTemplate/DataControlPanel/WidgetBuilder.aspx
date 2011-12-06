<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WidgetBuilder.aspx.cs"
    Inherits="We7.CMS.Web.Admin.VisualTemplate.WidgetBuilder" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>数据控件</title>
<base target="_self" />
<style type="text/css"></style>
<link rel="stylesheet" type="text/css" href="/Admin/theme/classic/css/main.css" media="screen" />
<meta content="noindex, nofollow" name="robots" />
<script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
<script src="/Admin/cgi-bin/base64.js" type="text/javascript"></script>
<script src="/Admin/cgi-bin/datacontrol/widget.new.js" type="text/javascript"></script>
<script src="/Admin/cgi-bin/datacontrol/widgetCommon.new.js?20110630001" type="text/javascript"></script>
<script src="<%=AppPath%>/ajax/jquery/jquery1.2.6.pack.js" type="text/javascript"></script>
<script src="<%=AppPath%>/ajax/jquery/jquery.simplemodal-1.1.1.js" type="text/javascript"></script>
<script src="/Scripts/ckfinder/ckfinder.js?20110422004" type="text/javascript"></script>
<link href="/Scripts/jQuery/plugin/colorpicker/css/colorpicker.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jQuery/plugin/colorpicker/colorpicker.js" type="text/javascript"></script>
<script type="text/javascript">
        var dialog = window.parent;
        function Ok() {
            var result = {};
            $("#fieldsetContainer > div").each(function (j) {
                if (j == 0) {
                    AllBUILDER.getI(j).setID(document.mainForm.IDTextBox.value);
                }
                jQuery.extend(result, AllBUILDER.getI(j).getProperties());
            });
            return result;
        }
    </script>
<script type="text/javascript">
        var datamode;
        function QueryString(name) {
            var qs = name + "=";
            var str = location.search;
            if (str.length > 0) {
                begin = str.indexOf(qs);
                if (begin != -1) {
                    begin += qs.length;
                    end = str.indexOf("&", begin);
                    if (end == -1) end = str.length;
                    return (str.substring(begin, end));
                }
            }
            return null;
        }

        function reloadData(fn) {
            $.ajax({
                url: '/Admin/Ajax/TemplateCheck.aspx',
                data: { cmd: 'reload', ctr: $("#ControlSelect").val() },
                success: function (result, op) {
                    datamode = eval("(" + result + ")");
                    if (fn) fn();
                },
                error: function (xh, st, op) {
                    alert("加载数据出错");
                }
            });
        }

        function createOp(txt, value) {
            var op = document.createElement("OPTION");
            op.text = txt;
            op.value = value;
            return op;
        }

        function onBodyLoad() {
            var txt = document.mainForm.FieldsTextBox.value;
            datamode = eval("(" + txt + ")");
            //创建选项卡    
            CreateTabPannel();
            //创建选项卡面板
            CreateTabContent();
        }
        //创建tab选项卡
        function CreateTabPannel() {
            var tablPannel = $("#ul_tabContorlName"); //页面中定义好的选项卡容器
            tablPannel.html(""); //将容器清空
            var items = datamode.parts; //得到选项卡数据
            for (var i = 0; i < items.length; i++)//遍历选项卡数据
            {
                items[i].li = $('<li class="rtsLI"></li>'); //创建li对象
                if (i == 0) {
                    items[0].li.addClass("rtsIn"); //给第一个面板添加默认选中样式  
                }
                //创建li内的超链接对象
                var liA = $("<a href='javascript:void(0);' class='rtsLink rtsSelected'></a>").click(function () { //添加点击事件               
                    $("#fieldsetContainer > div").each(function (j) {
                        $("#mainDiv_" + j).hide();
                        items[j].li.removeClass("rtsIn");
                    });
                    $("#mainDiv_" + $(this).data("index")).show();
                    items[$(this).data("index")].li.addClass("rtsIn");
                }).data("index", i);
                //将超链接对象添加到li对象，然后将li对象添加到容器
                tablPannel.append(items[i].li.append(liA.append("<span class='rtsTxt'>" + items[i].name + "</span>")));
            }
        }

        //创建tab选项卡内容
        function CreateTabContent() {
            $("#fieldsetContainer").html("");
            $("#fieldsetContainer").append("<legend>属性设置</legend>");
            var ctr = document.getElementById("ControlHidden").value;
            var dc;
            var items = datamode.parts;
            var IDTextBox;
            
            for (var i = 0; i < items.length; i++) {
                BUILDER = CreateBuilder();
                dc = items[i];
                dc.control = ctr;
                dc.template = '<%=Template %>';
                
                if (!dc) {
                    alert('当前控件不存在');
                    return;
                }
                dc.gp = $("#GroupHidden").val();
                //容器
                var div = $("<div id='mainDiv_" + i + "'  style='display:block;'></div>");
                //基本属性table
                var tableBase = $("<table id='mainTable_" + i + "_Base'></table>");
                //高级属性table
                var tableAdvance = $("<table id='mainTable_" + i + "_Advance' style='display:none;'></table>");
                //tableBase.append("<tr><th style='width: 100px;'>项目</th><th>内容</th></tr>");                             
                if (i == 0) {
                    trIDTextBox = $("<tr title='控件的标识符' style='cursor: pointer'><td>控件名称：</td></tr>");
                    var tdIDTextBox = $("<td></td>");
                    IDTextBox = $("<input type='input' ID='IDTextBox'  Text=''/>");
                    tableBase.append(trIDTextBox.append(tdIDTextBox.append(IDTextBox)));
                }
                tableBase.appendTo(div);
                var advance = $("<div style='display:block;'><p/><span style='font-weight:bold;'>高级选项　&nbsp;&nbsp;</span></div>");

                var advanceLable = $("<span>显示高级选项</span>");
                var advanceInput = $('<input type="checkbox"/>').click(function () {
                    if ($(this).attr("checked") == true) {
                        //debugger
                        $("#mainTable_" + $(this).data("index") + "_Advance").show();
                        $(this).data("lable").html("关闭高级选项");
                    } else {
                        //debugger
                        $("#mainTable_" + $(this).data("index") + "_Advance").hide();
                        $(this).data("lable").html("显示高级选项");
                    }
                }).data("index", i).data("lable", advanceLable);
                advance.append(advanceInput);
                advance.append(advanceLable);
                advance.appendTo(div); 
                tableAdvance.appendTo(div);
                div.append($("<div>&nbsp;<p/></div>"));
                //构造器
                BUILDER.clear();
                BUILDER.loadData(dc);
                BUILDER.createHtmlObject(div); 
                if (tableAdvance[0].rows.length > 0) {
                    advance.show();
                }
                else {
                    advance.hide();
                }
                SetIDTextBoxValue(IDTextBox); 
                if (i == 0) {
                    div.show(); 
                }
                else {
                    div.hide();
                }
                $("#fieldsetContainer").append(div); 
                AllBUILDER.setI(i, BUILDER);
            }
            
            $("#CssClass").html("");
            $.ajax({
                contentType: "application/json",
                url: "/Admin/VisualTemplate/VisualService.asmx/GetStyleByControl",
                data: '{"controlPath":"' + ctr + '"}',
                type: "POST",
                success: function (_result_) {
                    var array = eval(_result_);
                    for (var i in array) {
                        if (array[i].replace(".","_") == '<%=CssClass %>')
                            $("#CssClass").append("<option value='" + array[i] + "' selected='selected'>" + array[i] + "</option>");
                        else
                            $("#CssClass").append("<option value='" + array[i] + "'>" + array[i] + "</option>");
                    }
                }
            });
        }

        //设置控件名称
        function SetIDTextBoxValue(IDTextBox) {
            if (parent.window["DC"]) {
                BUILDER.loadDC(parent.window["DC"]); //加载json数据
                var idv = BUILDER.getID(); //当前控件id
                if (idv == null) {
                    var key = QueryString("file");
                    if (key && key != "null") {
                        idv = key.replace('.', '_');
                    }
                    else {
                        idv = "WECDControl_01";
                    }
                }
                IDTextBox.attr("value", idv); //给控件赋值
            }
            // debugger
            var isFirst = document.getElementById("IsFirstTextBox").value;
            if (isFirst == "1") {
                BUILDER.loadDefaultValues();
                var idName = "WECDControl_01";
                var key = QueryString("file");
                if (key && key != "null") {
                    idName = key.replace('.', '_') + "_01";
                }
                IDTextBox.attr("value", idName);
            }
            IDTextBox.focus();
        }

        function showErrorMessage(m) {
            var ml = document.getElementById("MessageLabel");
            ml.innerHTML = m;
        }

        function InitCtrEditor(editctr) {
            $(editctr).bind("click", function () {
                //<% //if(IsModel){ %>
                var result = window.showModalDialog("DataControlEditor.aspx?cmd=edit&ctr=" + $("#ControlSelect").val(), "修改控件", 'scrollbars=no;resizable=no;help=no;status=no;dialogHeight=490px;dialogwidth=700px;');
                //	    <%// }else{ %>
                //	    var result=window.showModalDialog("DataControlModelEditor.aspx?cmd=edit&ctr="+$("#ControlSelect").val(),"修改控件",'scrollbars=no;resizable=no;help=no;status=no;dialogHeight=490px;dialogwidth=750px;');
                //	    <%//} %>
                if (result && result.key && result.value) {
                    var cdc = {};
                    var ops = $("#ControlSelect")[0].options;
                    var op = ops[ops.length] = createOp(result.key, result.value);
                    op.selected = true;
                    reloadData(function (obj) {
                        $("#ControlSelect")[0].onchange();
                    });
                }
                return false;
            });
        }
        
        $(document).ready(function () {
            InitCtrEditor("#editctr");
        });
    </script>
<style>
fieldset {
	border: 1px solid;
	padding-left: 10px;
	padding-right: 10px;
	margin-left: 10px;
	float: left;
}
hr {
	height: 2px;
}
input {
	font-size: 12px;
}
.rtsUL {
	float: left;
	margin: 0;
	overflow-x: hidden;
	overflow-y: hidden;
	padding: 0;
	margin-bottom: 15px;
}
.rtsUL .rtsLI {
	float: left;
	margin-right: 15px;
}
.rtsUL .rtsIn {
	background-position: top right;
	background-image: url("/admin/Images/WebResource.axd.png");
	line-height: 20px;
	padding-right: 9px;
}
.rtsIn .rtsLink {
	background-position: top left;
	background-image: url("/admin/Images/WebResource.axd.png");
	padding-left: 9px;
	display: block;
	color: #fff;
	text-decoration: none;
	font-weight: bold;
}
.rtsIn .rtsTxt {
	background-image: url("/admin/Images/WebResource.axd.png");
	line-height: 20px;
	display: block;
	background-position: 50% 0;
	padding-bottom: 5px;
	text-decoration: none;
}
.box {
	border-top: solid 1px #ddd;
	padding-top: 10px;
	position: relative;
}
.box_hr {
	padding: 0px;
	margin: 0px;
	color: #666;
}
.box_bottom {
	height: 1px;
	width: 100%;
	border-bottom: solid 1px #ddd;
	position: absolute;
	left: 0px;
	top: 45px;
}
.advanced span {
	display: block;
	width: 53px;
	height: 20px;
	background-image: url("/admin/Images/advanced.jpg");
	text-align: center;
	padding-top: 3px;
}
</style>
</head>
<body onload="onBodyLoad()" style="padding: 0; margin: 0; x-overflow: hidden; width: 630px; padding: 0 10px">
<form id="mainForm" runat="server">
  <h2>
    <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/logo_listset.gif" />
    <asp:Label ID="NameLabel" runat="server" Text="设置部件的属性"> </asp:Label>
    <span class="summary">
      <asp:Label ID="SummaryLabel" runat="server" Text=""> </asp:Label>
    </span>
  </h2>
  <div class="box">
    <div class="box_bottom"> </div>
    <table cellpadding="0" cellspacing="0" border="0">
      <tr>
        <td valign="top"><div class="tabPanelWrapper">
            <div class="rtsLevel rtsLevel1">
              <ul class="rtsUL" id="ul_tabContorlName">
              </ul>
            </div>
          </div></td>
      </tr>
      <tr>
        <td><fieldset style="minheight: 340px; height: auto !important; height: 340px; width: 380px;" id="fieldsetContainer">
          </fieldset></td>
      </tr>
    </table>
    <div id="messageLayer" style="display: none">
      <asp:Image ID="MessageImage" runat="server" ImageUrl="~/admin/Images/icon_info.gif" />
      <asp:Label ID="MessageLabel" runat="server" Text=""> </asp:Label>
    </div>
  </div>
  <div style="display: none">
    <asp:TextBox ID="ContorlsInfo" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="FieldsTextBox" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="IsFirstTextBox" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="CssFileTextBox" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="StylePathTextBox" runat="server" Text=""></asp:TextBox>
    <asp:HiddenField ID="GroupHidden" runat="server" />
    <asp:HiddenField ID="ControlHidden" runat="server" />
  </div>
  <div id="mask" class="mask"> </div>
</form>
</body>
</html>
