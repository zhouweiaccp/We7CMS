/// <reference path="../../Ajax/jquery/jquery-1.4.1-vsdoc.js" />



(function () {
    var conditionSetting = [{ Type: "TextInput", Label: "标签", Name: "Label", value: '' }, { Type: "TextInput", Label: "名称", ReadOnly: true, Name: "Name", value: '' },
    { Type: "Select", Label: "控件类型", Name: "Type", Params: [{ Name: "data", Value: "TextInput|文本框,Request|来自Url"}], Value: "TextInput" },
    { Type: "Select", Label: "可见性", Name: "Visible", Params: [{ Name: "data", Value: "true|是,false|否"}], Value: "true" }, { Type: "TextInput", Label: "Css类", Name: "CssClass" }, { Type: "TextInput", Label: "宽度", Name: "Width", value: '' },
    { Type: "TextInput", Label: "高度", Name: "Height", value: '' },
     { Type: "Select", Label: "操作符", Name: "Params.operater", Params: [{ Name: "data", Value: "equal|等于,like|包含"}], Value: "equal" },
     , { Type: "TextInput", Label: "参数名称", Name: "Params.param" },
     { Type: "TextInput", Label: "描述", Name: "Desc", value: '' }
    ];
    //global
    var panel = Request.parameter("panel") || 'list';
    var model = Request.parameter("modelname");

    var grouth = 0;

    function getGrouth() {
        grouth++;
        return "gen" + grouth;

    }
    var loading = $('<img src="/Admin/ContentModel/images/loading.gif" />');
    var pageSize = 10;

    var colsList = [];
    var conditions = [];

    //functions

    function NewConditionTag(name) {
        var col = conditions[name];
        var control = new We7.Control(col);
        var column = $(document.createElement("li")).attr("myID", name).append(control.getWarpEl())
    .click(function () {


        $(".colSelect").removeClass("colSelect");
        $(this).addClass("colSelect");
        CreateConditionProps(name);
        $("#fieldProperties").trigger("click");

    }).mouseover(function () { $(this).addClass("hover"); }).mouseout(function () { $(this).removeClass("hover"); });
        return column;

    }
    function GetListColumn() {
        $("#columns").empty();
        var data = { model: model, panel: panel, index: $("#ListMode").val() };
        //加载列表
        $.ajax({
            url: '/Admin/ContentModel/ajax/ContentModel.asmx/GetListCoulumn',
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: jsonToString(data),
            success: function (json) {
                var listInfo = stringToJSON(json);
                $("#IsEnable").val(listInfo.Enable.toString());
                var cols = listInfo.Columns;
                $.each(cols, function (i) {
                    var tag = getGrouth();
                    colsList[tag] = cols[i];
                    var col = NewColumnTag(tag);
                    $("#columns").append(col);
                    //$("#columns>li").last().trigger("click");
                });
            }
        });
    }
    $(document).ready(function () {
        //可排序
        $("#columns").sortable({
            start: function (event, ui) {
                //TODO::ie下无法删除DOM节点

                //             $(".colSelect").removeClass("colSelect");
                //               $(ui.item).addClass("colSelect");
            }
        });
        $("#queryFields").sortable({
            start: function (event, ui) {
                //               $(".colSelect").removeClass("colSelect");
                //               $(ui.item).addClass("colSelect");
            }
        });
        $("#side").tabs();
        var data = { model: model, panel: panel };
        //加载condition
        $.ajax({
            url: '/Admin/ContentModel/ajax/ContentModel.asmx/GetConditionControls',
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: jsonToString(data),
            success: function (json) {
                var conditionInfo = stringToJSON(json);
                var cols = conditionInfo.Controls;

                $.each(cols, function (i) {
                    var tag = getGrouth();
                    conditions[tag] = cols[i];
                    var col = NewConditionTag(tag);
                    $("#queryFields").append(col);

                });
            },
            error: function () {
                alert("加载控件失败！");
            }
        });
        GetListColumn();
        $("#ListMode").change(function () {
            if ($("#ListMode").val() == "1") {
                $("#closs li:eq(1)").hide();
                $("#drop").hide();
            }
            else {
                $("#closs li:eq(1)").show();
                $("#drop").show();
            }
            GetListColumn();
        });
        $("#btnSave").click(function () {

            var erroCount = 0;
            var tempColsList = [];
            $("#columns>li").each(function (i) {
                var colName = $(this).attr("myID");
                if (We7.isUndefined(colsList[colName]["Name"]) || We7.isEmpty(colsList[colName]["Name"])) {
                    erroCount++;

                    $(this).addClass("error_item");
                }
                colsList[colName]["Index"] = i;
                tempColsList.push(colsList[colName]);
            });
            $("#queryFields>li").each(function () {
                var colName = $(this).attr("myID");
                if (We7.isUndefined(conditions[colName]["Name"]) || We7.isEmpty(conditions[colName]["Name"])) {
                    erroCount++;

                    $(this).addClass("error_item").click(function () {
                        $(this).removeClass("error_item");
                    });
                }
            });
            if (erroCount > 0) {
                alert("你有" + erroCount + "个项未设置字段，请检查必填字段!");
                $(".error_item").first().find("input").focus();
                return;
            }
            var strCols = "[";
            for (var p in tempColsList) {


                var temp = jsonToString(tempColsList[p]);
                temp = stringToJSON(temp);
                for (var p1 in temp) {
                    if (We7.isUndefined(temp[p1]) || temp[p1] == null) {
                        delete temp[p1];
                    }
                    if (temp.IsThumb) temp.Type = "thumb";
                    //else if (temp.IsLink) temp.Type = "link";
                }
                strCols += jsonToString(temp);
                strCols += ",";
            }
            strCols += "]";

            var strCondition = "[";
            for (var p in conditions) {

                var temp = jsonToString(conditions[p]);
                temp = stringToJSON(temp);
                for (var p1 in temp) {
                    if (We7.isUndefined(temp[p1]) || temp[p1] == null) {
                        delete temp[p1];
                    }
                }
                strCondition += jsonToString(temp);
                strCondition += ",";
            }
            strCondition += "]";

            $.ajax({
                url: '/Admin/ContentModel/ajax/ContentModel.asmx/SaveList',
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: '{"model":"' + model + '","panel":"' + panel + '","list":"' + encodeURIComponent(strCols) + '","condition":"' +
					encodeURIComponent(strCondition) + '","pagesize":"' + pageSize.toString() + '","index":"' + $("#ListMode").val() + '","enable":"' + $("#IsEnable").val() + '"}',
                success: function (json) {
                    alert(json);
                },
                error: function () {
                    alert("error");
                }
            });
        });

        //分页点击事件

        $(".pagerbar").click(function () {
            $("#pros").html("");
            var load = loading;
            $("#pros").append(load);
            for (var p in pageConfig) {
                var control = new We7.Control(pageConfig[p]);
                $("#pros").append(control.getWarpEl());
                control.setValue(pageSize);


                $(control.getEl()).change(function () {
                    pageSize = $(this).val();
                });
                $("#fieldProperties").trigger("click");
            }
            $(".colSelect").removeClass("colSelect");
            $(this).addClass("colSelect");
            load.remove();

        }).mouseover(function () { $(this).addClass("hover"); }).mouseout(function () { $(this).removeClass("hover"); });

        $("#btnaddcls").click(function () {
            var tag = getGrouth();
            colsList[tag] = { "Label": "选择列", "Type": "html", "Visible": true };
            var col = NewColumnTag(tag);
            $("#columns").append(col);
            $("#columns>li").last().trigger("click");
        });
        $("#btnaddclss").click(function () {
            //columContent
            var t = $("#columContent");
            if (t.css("display") != "none") {
                t.hide("slow");
                return;
            }
            t.show("slow");
            t = $("#columContent >ul");


            if ($.trim(t.text()) == '') {

                $("#pros").html("");
                $("#pros").append(loading);
                var url = "/Admin/ContentModel/ajax/ContentModel.asmx/GetDataColumn";
                var data = { model: model };
                $.ajax({
                    url: url,
                    data: jsonToString(data),
                    type: "POST",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    error: function () { alert("An error occurred") },
                    success: function (json) {
                        //alert(json);
                        $("#pros").html("");
                        json = stringToJSON(json);
                        var chkall = $("<input type='checkbox'  name='chkclssAll' />");
                        chkall.click(function () {
                            $("input[name='chkclss']").each(function (i) {
                                $(this).attr("checked", chkall.attr("checked"));
                            });
                        });
                        t.append("<li>");
                        t.append(chkall);
                        t.append("全选");
                        t.append("</li>");
                        $.each(json, function (i) {
                            t.append("<li class='clss' style='width:40px;height:20px;float:left'><input type='checkbox' value='" + json[i].Name + "' name='chkclss' title='" + json[i].Label + "'>" + json[i].Label + "</li>");
                        });
                        var btnSave = $("<input type='button' value='添加'>");

                        btnSave.click(function () {

                            $("[name='chkclss']:checked").each(function (i) {

                                //                                var tag = getGrouth();
                                //                                colsList[tag] = { "Label": "选择列", "Type": "html", "Visible": true };
                                //                                var col = NewColumnTag(tag);
                                //                                $("#columns").append(col);
                                //                                $("#columns>li").last().trigger("click");



                                var tag = getGrouth();
                                colsList[tag] = { "Label": $(this).attr("title"), "Type": "html", "Visible": true };
                                var col = NewColumnTag(tag);
                                $("#columns").append(col);
                                $("#columns>li").last().trigger("click");


                                col["Name"] = $(this).val();
                                col["Label"] = $(this).attr("title");
                                colsList[tag] = col;

                            });

                            $("#columns>li").last().trigger("click");


                        });
                        t.append(btnSave);
                    }
                });
            }


        });
        $("#ddt").click(function () {
            var tag = getGrouth();

            conditions[tag] = { Label: "查询条件", Type: "TextInput", "Visible": true, Params: [] };
            var col = NewConditionTag(tag);
            $("#queryFields").append(col);
            $(col).trigger("click");
        });
        //drop

        $("#drop").droppable({
            accept: 'li.colSelect',
            // activeClass: 'test',
            hoverClass: 'drophover',
            over: function (event, ui) {
                $("#dropBag").attr("src", "images/del_open.png");
            },
            out: function (event, ui) {

                $("#dropBag").attr("src", "images/del.png");
            },

            drop: function (event, ui) {


                var cfg = confirm("你确定要删除该控件么?");
                if (cfg) {

                    var cpt = $(".colSelect");



                    var tempArrayName = cpt.attr("myID");

                    if (cpt.hasClass("column")) {
                        //列表控件
                        delete colsList[tempArrayName];
                    }
                    else {
                        delete conditions[tempArrayName];
                    }
                    //1.8.1-1.8.5的Jquery Ui 需要更改方法才可使用cpt.remove().否则FireFox及Chrome下异常
                    cpt.remove(); 
                }
                $("#dropBag").attr("src", "images/del.png");

            }
        });

    });

    function NewColumnTag(name) {

        var col = colsList[name];
        var column;
        var colWidth = "120px";
        var colHeight = "120px";
        if (col.Width) {
            colWidth = col.Width;
            if (colWidth.indexOf("px") < 0) colWidth += "px";
        }
        if (col.Height) {
            colHeight = col.Height;
            if (colHeight.indexOf("px") < 0) colHeight += "px";
        }
        if ($("#ListMode").val() == "0")
            column = $(document.createElement("li")).addClass("columnli").addClass("column").attr("myID", name).append(col.Label);
        else {
            switch (col.Type) {
                case "link":
                    column = $(document.createElement("li")).attr("myID", name).css("text-align", "center").append(col.Label); break;
                case "thumb":
                    column = $(document.createElement("li")).attr("myID", name).append($(document.createElement("img")).attr({ title: '图片', src: '/Admin/images/flower.jpg' }).css({ "height": colHeight, "width": colWidth })); break;
                case "action":
                    var statistics = $(document.createElement("span")).append("统计信息");
                    column = $(document.createElement("li")).attr("myID", name).append(statistics).append(
					$(document.createElement("img")).css("float", "right").attr({ "src": "/Admin/images/icon_del.gif", "title": "删除" })).append(
					$(document.createElement("img")).css({ "float": "right", "padding-right": "5px" }).attr({ "src": "/Admin/images/icon_edit.gif", "title": "编辑" })); break;
            }
        }
        column.click(function () {
      
            $(".colSelect").removeClass("colSelect");
            $(this).addClass("colSelect");
            CreateProps(name);
            $("#fieldProperties").trigger("click");

        });
        if (col.Width) {
            column.css("width", colWidth);
        }
        if (col.Height) {
            column.css("height", colHeight);
        }

        column.mouseover(function () { $(this).addClass("hover"); }).mouseout(function () { $(this).removeClass("hover"); });
        return column;

    }


    //config the Property Panel
    var pageConfig = [
            { Type: "TextInput", Label: "分页大小", Name: "PageSize" }
];

    var propConfig = [
{ Type: "TextInput", Label: "标签", Name: "Label", value: '' },

{ Type: "Select", Label: "有链接", Name: "IsLink", Params: [{ Name: "data", Value: "true|是,false|否"}], Value: 'false' },
{ Type: "Select", Label: "缩略图", Name: "IsThumb", Params: [{ Name: "data", Value: "true|是,false|否"}], Value: 'false' },
{ Type: "TextInput", Label: "名称", Name: "Name", ReadOnly: true },
{ Type: "Select", Label: "可见性", Name: "Visible", Params: [{ Name: "data", Value: "true|是,false|否"}], Value: 'true' },
{ Type: "TextInput", Label: "宽度", Name: "Width", value: '' }, { Type: "TextInput", Label: "高度", Name: "Height", value: '' },
{ Type: "Select", Label: "对齐方式", Name: "Align", Params: [{ Name: "data", Value: "0|未设置,1|左对齐,2|居中,3|右对齐,4|自动调整"}] }
];


    function CreateProps(name) {
        var col = colsList[name];
        var load = loading;
        $("#pros").html("");
        $("#pros").append(load);
        var url = "/Admin/ContentModel/ajax/ContentModel.asmx/GetDataColumn";
        var data = { model: model };

        $.ajax({
            url: url,
            data: jsonToString(data),
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (json) {
                $("#pros").html("");
                if (col.Type == "html" || col.Type == "link" || col.Type == "thumb") {
                    var label = $("<label>选择字段</label>");
                    json = stringToJSON(json);
                    var ddl = $('<select id="ddlSelect"></select>');
                    ddl.append('<option value="-1">请选择</option>');
                    $.each(json, function (i) {
                        $(ddl).append('<option value="' + json[i].Name + '">' + json[i].Label + '</option>');
                    });
                    ddl.change(function () {

                        $("#Name").val($(this).val());
                        $("#Label").val($(this).find("option:selected").text());
                        col["Name"] = $(this).val();
                        col["Label"] = $(this).find("option:selected").text();

                        var newColumn = NewColumnTag(name);
                        newColumn.addClass("colSelect");
                        $(".colSelect").replaceWith(newColumn);
                    });


                    $("#pros").append(label);
                    $("#pros").append(ddl);
                }
                for (var p in propConfig) {
                    if (propConfig[p].Name == "Height" && $("#ListMode").val() == "0") continue;
                    if ((propConfig[p].Name == "IsThumb" || propConfig[p].Name == "IsLink") && col.Type == "action") continue;
                    if (propConfig[p].Name == "Align" || propConfig[p].Name == "IsThumb" && $("#ListMode").val() == "1") continue;
                    var control = new We7.Control(propConfig[p]);
                    $("#pros").append(control.getWarpEl());
                    if (col[propConfig[p].Name] || We7.isBoolean(col[propConfig[p].Name])) {
                        control.setValue(col[propConfig[p].Name]);
                    }
                    $(control.getEl()).change(function () {
                        var tempValue = $(this).val();
                        if (tempValue === "true") {
                            col[$(this).attr("Name")] = true;
                        }
                        else if (tempValue === "false") {
                            col[$(this).attr("Name")] = false;
                        }
                        else {
                            col[$(this).attr("Name")] = tempValue;
                        }

                        var newColumn = NewColumnTag(name);
                        newColumn.addClass("colSelect");
                        $(".colSelect").replaceWith(newColumn);
                    });

                }
                $(ddl).val($("#Name").val());
                load.remove();
            },
            error: function () {
                alert("error");
            }
        });

    }



    function CreateConditionProps(name) {
        var col = conditions[name];

        var load = loading;
        $("#pros").html("");
        $("#pros").append(load);
        var url = "/Admin/ContentModel/ajax/ContentModel.asmx/GetSearchDataColumn";
        var data = { model: model };

        $.ajax({
            url: url,
            data: jsonToString(data),
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (json) {
                $("#pros").html("");

                var label = $("<label>选择字段</label>");
                json = stringToJSON(json);
                var ddl = $('<select id="ddlSelect"></select>');
                ddl.append('<option value="-1">请选择</option>');
                $.each(json, function (i) {
                    $(ddl).append('<option value="' + json[i].Name + '">' + json[i].Label + '</option>');
                });
                ddl.change(function () {
                    $("#Name").val($(this).val());
                    $("#Label").val($(this).find("option:selected").text());
                    col["Name"] = $(this).val();
                    col["Label"] = $(this).find("option:selected").text();
                    var newColumn = NewConditionTag(name);
                    newColumn.addClass("colSelect");
                    $(".colSelect").replaceWith(newColumn);
                });


                $("#pros").append(label);
                $("#pros").append(ddl);

                for (var p in conditionSetting) {
                    var control = new We7.Control(conditionSetting[p]);
                    $("#pros").append(control.getWarpEl());

                    var tempName = conditionSetting[p].Name;
                    var temp = tempName.split(".");

                    if (temp.length == 2) {

                        for (var index in col[temp[0]]) {
                            if (col[temp[0]][index].Name == temp[1]) {

                                control.setValue(col[temp[0]][index].Value);
                            }
                        }
                    }
                    else {
                        control.setValue(col[conditionSetting[p].Name]);
                    }

                    $(control.getEl()).change(function () {

                        var tempName2 = $(this).attr("Name");
                        var tt2 = tempName2.split(".");
                        if (tt2.length == 2) {
                            if (We7.isUndefined(col[tt2[0]])) {
                                col[tt2[0]] = [];
                            }


                            var tempvalue = {};
                            tempvalue.Name = tt2[1];
                            tempvalue.Value = this.value;

                            var taget = false;

                            for (var index in col[tt2[0]]) {
                                if (col[tt2[0]][index].Name == tt2[1]) {
                                    taget = true;
                                    col[tt2[0]][index] = tempvalue;
                                }
                            }

                            if (!taget) {
                                col[tt2[0]].push(tempvalue);
                            }
                        } else {
                            col[$(this).attr("Name")] = $(this).val();
                        }

                        var newColumn = NewConditionTag(name);
                        newColumn.addClass("colSelect");
                        $(".colSelect").replaceWith(newColumn);
                    });

                }
                $(ddl).val($("#Name").val());
                load.remove();
            },
            error: function () {
                alert("error");
            }
        });

    }

})();