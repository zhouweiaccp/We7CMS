//***************************************************
// 功能：录入表单生成的JS控制
//****************************************************************/

(function () {
    //define globle object
    var panel = Request.parameter("panel") || 'edit';
    var model = Request.parameter("modelname");
    var dataColumns = []; //保存数据列对象集合
    var editControls = []; //保存添加的编辑控件列表
    var controlsSetting = []; //保存control信息
    //自增长因子
    var _grouth = 0;
    //获取自增长标示
    function _getGrouth() {
        _grouth++;

        return "gen" + _grouth;
    }
    var loading = $('<img src="/Admin/ContentModel/images/loading.gif" />');
    var loading2 = $('<img src="/Admin/ContentModel/images/loading.gif" />');
    //获取所有数据列数组
    function getdataColumns() {
        var url = "/Admin/ContentModel/ajax/ContentModel.asmx/GetDataColumn";
        var data = { model: model };
        $.ajax({
            url: url,
            data: jsonToString(data),
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (json) {
                dataColumns = stringToJSON(json);
            }
        });
    }

    //创建一个新的数据下拉框
    function NewColumnDropdownlist() {
        var ddlWarp = $(document.createElement("div"));
        var ddl = $(document.createElement("select")).attr("id", "ddlSelect");
        ddl.append('<option value="-1">请选择对应数据列</option>');
        $.each(dataColumns, function (i) {
            ddl.append('<option value="' + dataColumns[i].Name + '">' + dataColumns[i].Label + '</option>');
        });
        ddl.change(function () {
            $("#Name", "#props").val($(this).val());
            $("#Label", "#props").val($(this).find("option:selected").text());
            var tempTarget = $(".editSelect").attr("myID");
            editControls[tempTarget]["Name"] = $(this).val();
            editControls[tempTarget]["Label"] = $(this).find("option:selected").text();
            $("#Name", "#props").trigger("change");
        });

        var btnDialog = $(document.createElement("a")).attr("href", 'javascript:void(0);').html("新增").click(function () {
            $("#dialog").dialog({ title: '新增数据列', modal: true, height: 300, width: 380 });
            $("#dialog_label").val('');
            $('#dialog_name').val('');
            $('#dialog_checkTitleField')[0].checked = false;
            $('#dialog_checkSearchField')[0].checked = false;
            $('#dialog_dataType')[0][0].selected = true;
            $('#dialog_maxlength').val('25');
            $("#div_maxlength").show();
        }
        );
        ddlWarp.append(ddl);
        ddlWarp.append(btnDialog);
        return ddlWarp;
    }

    //创建一个新控件 参数name为editControls数组中的标示
    function NewControl(name) {
        //获取配置
        var editSetting = editControls[name];
        if (We7.isUndefined(editSetting)) {
            //不存在
            throw new Error("标示为:" + name + "的项不存在！");
        }
        //创建DOM
        var warpLi = $(document.createElement("li")).attr("myID", name).attr("title", "点击可编辑. 拖拽可排序.").attr("class", "we7-fieldwarpper");
        var editControl = new We7.Control(editSetting);
        warpLi.append(editControl.getWarpEl());

        //点击标示为选择并创建对应属性面板
        warpLi.click(function () {
            $(".arrow").remove();
            $(document.createElement("img")).addClass("arrow").attr("src", "/Admin/ContentModel/images/arrow.png").prependTo(this);
            $("#setting").trigger("click");
            $(".editSelect").removeClass("editSelect");
            $(this).addClass("editSelect");

            $(".fieldActions").remove();
            createActionLinks($(this)).appendTo(this);
            var v = $(this).offset().top - $("#side").offset().top - 120;
            //            if (v < 0) v = 0;
            //            $("#fieldProperties").css("margin-top", v + "px");
            //            if ($("#fieldProperties").offset().top + $("#fieldProperties").height() > $("#btnSave").offset().top) {
            //                v = $("#btnSave").offset().top - $("#fieldProperties").height() - $("#side").offset().top - 50;
            ////                $("#fieldProperties").css("margin-top", v + "px");
            //            }
            if (v < 0) v = 0;
            $("#fieldProperties").css("margin-top", v + "px");
            //创建对应属性面板
            NewControlProperty(name);
        });
        warpLi.mouseover(function () { $(this).addClass("hover"); }).mouseout(function () { $(this).removeClass("hover"); });
        return warpLi;
    }

    function createActionLinks(liField) {
        var t = $(liField).offset().top + $(liField).height();
        var l = 390;
        var act = $(document.createElement("div")).addClass("fieldActions").css("top", t + "px").css("left", l + "px");
        var del = $(document.createElement("img")).addClass("delete").attr("title", "删除").attr("src", "/Admin/ContentModel/images/delete.png").click(function () {
            deleteSelect();
        }).appendTo(act);
        var del = $(document.createElement("img")).addClass("clone").attr("title", "复制").attr("src", "/Admin/ContentModel/images/add.png").click(function () {
            // alert("复制");
            copySelect();
        }).appendTo(act);
        return act;
    }
    //创建对应的属性面板
    function NewControlProperty(name) {
        var col = editControls[name];
        if (We7.isUndefined(col)) {
            //不存在
            throw new Error("标示为:" + name + "的项不存在！");
        }
        //清空属性面板
        $("#props").html("");
        var proploading = loading;
        $("#props").append(proploading);
        var propertySetting = We7.Control.getFieldClass(col.Type).groupOptions;
        $("#props").append(NewColumnDropdownlist());
        var ctrlHTML = '';
        for (var p in propertySetting) {
            var control = new We7.Control(propertySetting[p]);
            $("#props").append(control.getWarpEl());
            var tempName = propertySetting[p].Name;
            var temp = tempName.split(".");

            if (temp.length == 2) {
                for (var index in col[temp[0]]) {
                    if (col[temp[0]][index].Name == temp[1]) {
                        if (propertySetting[p].Type == "Select" && propertySetting[p].DataSource != '' && propertySetting[p].DataSource != undefined)
                            $.each(dataColumns, function (i) {
                                $(control.el[0]).append('<option value="' + dataColumns[i].Name + '">' + dataColumns[i].Label + '</option>');
                            });
                        control.setValue(col[temp[0]][index].Value);
                    }
                }
            }
            else {
                control.setValue(col[propertySetting[p].Name]);
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

                var newColumn = NewControl(name);
                newColumn.addClass("editSelect");
                $(".editSelect").replaceWith(newColumn);
            });

        }
        //设置
        $("#ddlSelect", "#props").val($("#Name", "#props").val());
        proploading.remove();
    }
    var index = 0;
    //初始化表单编辑容器中的控件列表
    function initEditControls() {
        var editloading = CloneObject(loading2);
        $("#formFields").append(editloading);
        var data = { panel: panel, model: model, index: index };
        $.ajax({
            data: jsonToString(data),
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            url: '/Admin/ContentModel/ajax/ContentModel.asmx/GetEditControls',
            // timeout: 1000,
            success: function (json) {
                if (typeof json == 'string') {
                    json = stringToJSON(json);
                }
                if (json.d != null) {
                    json = json.d;
                }
                if (!json.Success) {
                    alert(json.Message);
                }
                else {
                    var myidlist = "";
                    for (var p in json.Data.Controls) {
                        var tempName = _getGrouth();
                        editControls[tempName] = json.Data.Controls[p];

                        var control = NewControl(tempName);

                        if (editControls[tempName].ID == "ID")
                            control.hide();

                        if (control.attr("myid") != "undefined")
                            myidlist += control.attr("myid") + ","
                        $("#formFields").append(control);
                    }
                    var myidInput = $("<input>").attr("id", "myId").attr("type", "hidden").val(myidlist);
                    myidInput.appendTo($("#formFields"));
                }
                editloading.remove();
            },
            error: function () {
                alert("初始化控件出错！");
            }
        })
    }
    //页面属性面板初始化
    function initPageProperty() {
        var pagePropertyData = null;
        var data = { panel: panel, model: model };
        $.ajax({
            data: jsonToString(data),
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            url: '/Admin/ContentModel/ajax/ContentModel.asmx/GetGroups',
            success: function (json) {
                if (typeof json == 'string') {
                    json = stringToJSON(json);
                }
                if (json.d != null) {
                    json = json.d;
                }
                if (!json.Success) {
                    alert(json.Message);
                }
                else {
                    pagePropertyData = json.Data;
                    for (var p in json.Data) {
                        $("#pageProperties #pProperties tr:eq(0) select")[0].options.add(new Option(json.Data[p].Name, json.Data[p].Index));
                        $("#pageProperties #pProperties tr:eq(1) select")[0].options.add(new Option(json.Data[p].Name, json.Data[p].Index));
                        if (json.Data[p].Index == index) {
                            $("#pageProperties #pProperties tr:eq(1) select").val(json.Data[p].Next);
                            $("#pageProperties #pProperties tr:eq(2) select").val(json.Data[p].Enable.toString());
                        }
                    }
                    $("#pageProperties #pProperties tr:eq(0) select").val(index);
                }
            },
            error: function () {
                alert("初始化页面属性出错1！");
            }
        });
        //新增框
        var addGroupDialog = $(document.createElement("a")).attr("href", 'javascript:void(0);').html("管理").click(function () {
            $("#groupManage").dialog({ modal: true, width: 380 });
            $("#groupList").empty();
            for (var p in pagePropertyData) {
                var list = $(document.createElement('span'));
                list.append(pagePropertyData[p].Name);
                list.attr("curIndex", pagePropertyData[p].Index);
                if (pagePropertyData[p].Index != 0) {
                    var img = $(document.createElement('img'));
                    img.attr("src", "/Admin/images/icon_del.gif");
                    img.attr("title", "删除");
                    list.append(img);
                }
                $("#groupList").append(list);
            }
            //删除一个group
            $("#groupList span img").click(function () {
                var delIndex = $(this.parentNode).attr("curIndex");
                var data = { model: model, index: delIndex, panel: panel };

                $.ajax({
                    url: '/Admin/ContentModel/ajax/ContentModel.asmx/DelGroup',
                    type: "POST",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: jsonToString(data),
                    success: function (json) {
                        var ajaxMessage = stringToJSON(json);

                        if (ajaxMessage.Success) {
                            alert(ajaxMessage.Message);
                            //删除下拉框
                            $("#pageProperties #pProperties tr:eq(0) select:eq(0) option[value='" + delIndex + "']").remove();
                            $("#pageProperties #pProperties tr:eq(1) select:eq(0) option[value='" + delIndex + "']").remove();
                            $("#pageProperties #pProperties tr:eq(0) select").trigger("change");
                            $("#groupManage").dialog("close");
                            pagePropertyData = ajaxMessage.Data;
                        } else {
                            alert(ajaxMessage.Message);
                        }

                    },
                    error: function () {
                        alert("出错啦！");
                    }
                });
            });
        });


        $("#pageProperties #pProperties tr:eq(0) td:eq(1)").append(addGroupDialog);
        //保存新增
        $("#groupConfirm").click(function () {
            var name = $("#groupName").val();
            if (name == "") {
                alert("名称不能为空！");
                $("#groupName").focus();
                return;
            }
            var newIndex = $("#pageProperties #pProperties tr:eq(0) select")[0].options.length;
            var data = { model: model, name: name, index: newIndex, panel: panel };

            $.ajax({
                url: '/Admin/ContentModel/ajax/ContentModel.asmx/AddGroup',
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: jsonToString(data),
                success: function (json) {
                    var ajaxMessage = stringToJSON(json);

                    if (ajaxMessage.Success) {
                        alert(ajaxMessage.Message);
                        //插入到下拉框

                        $("#pageProperties #pProperties tr:eq(0) select")[0].options.add(new Option(name, newIndex));
                        $("#pageProperties #pProperties tr:eq(1) select")[0].options.add(new Option(name, newIndex));
                        $("#pageProperties #pProperties tr:eq(0) select option:last").attr("selected", "selected");
                        $("#pageProperties #pProperties tr:eq(1) select option:last").attr("selected", "selected");
                        $("#pageProperties #pProperties tr:eq(0) select").trigger("change");
                        $("#groupManage").dialog("close");
                        pagePropertyData = ajaxMessage.Data;

                    } else {
                        alert(ajaxMessage.Message);
                    }

                },
                error: function () {
                    alert("出错啦！");
                }
            });
        });

        //选择不同标记	
        var diffIndex = $("#pageProperties #pProperties tr:eq(0) select")[0];
        var diffNext = $("#pageProperties #pProperties tr:eq(1) select")[0];
        $(diffIndex).change(function () {
            $("#formFields").empty();
            index = $(diffIndex).val();
            for (var p in pagePropertyData) {
                if (pagePropertyData[p].Index == index) {
                    if ($("#pageProperties #pProperties tr:eq(1) select:eq(0) option[value='" + pagePropertyData[p].Next + "']").length == 0)
                        $(diffNext).val(index);
                    else
                        $(diffNext).val(pagePropertyData[p].Next);
                    $("#pageProperties #pProperties tr:eq(2) select:eq(0)").val(pagePropertyData[p].Enable.toString());
                    break;
                }
            }
            initEditControls();
        });
    }

    //加载控件类型
    function loadControlType() {
        var loadingcotrs = loading;
        $("#addFields").append(loadingcotrs);
        var controlPath = "/ModelUI/Config/Controls.xml";
        $.ajax({
            cache: false,
            dataType: "xml",
            url: controlPath,
            type: "GET",
            success: function (xml) {
                $(xml).find("control").each(function () {
                    var type = $(this).attr("type");
                    var label = $(this).attr("label");
                    var setting = $(this).attr("defaultValue");
                    $("<li></li>").html('<a><b></b>' + label + '</a>').attr("id", type).appendTo("#addFields");
                    //去除loading
                    controlsSetting[type] = stringToJSON(setting);

                });
                loadingcotrs.remove();
                //控件可拖拽
                $("#addFields>li").draggable({
                    opacity: 0.35,
                    revert: "valid",
                    helper: function () {
                        var c = $(document.createElement("div")).addClass('dragField').text($(this).text());
                        return c;
                    },
                    connectToSortable: '#formFields',
                    iframeFix: true,
                    scroll: false
                });
            },
            error: function () {
                alert("error");
            }
        });
    }

    //拖拽可放定义
    function DropControl() {
        $("#formFields").droppable({
            accept: '#addFields>li',
            hoverClass: 'drophover'
        });
    }

    //保存数据
    function SaveEditControl() {
       
        var copy;
        if ($("#copyToUser").length > 0) {
            copy = $("#copyToUser").attr("checked").toString();
        }
        else {
            copy = "false";
        }


        if (copy == "true") {
            var cfm = confirm("你已经选择了-更新到会员中心,将会覆盖,是否继续?");
            if (!cfm) {
                return;
            }
        }
        var erroCount = 0;
        var tempColsList = [], errorFields = '', myidList = '';
        $("#formFields>li").each(function (i) {
            var colName = $(this).attr("myID");

            if (We7.isUndefined(editControls[colName]["Name"]) || We7.isEmpty(editControls[colName]["Name"])) {
                erroCount++;
                errorFields += editControls[colName].Label + '\n';
                $(this).addClass("error_item");
            }
            else
                myidList += colName + ","
            tempColsList.push(editControls[colName]);
        });
        //点击保存后 取消绑定
        we7.removeBeforeUnload();

        if (erroCount > 0) {
            alert('请检查并继续完成下列设置项的数据列指定：\n\n' + errorFields);
            $("#setting").trigger("click");
            $(".error_item").first().trigger("click");
            return;
        }

        we7.loading("保存中，请稍候");
        var strCols = "[";
        for (var p in tempColsList) {
            var temp = jsonToString(tempColsList[p]);
            temp = stringToJSON(temp);
            for (var p1 in temp) {
                if (We7.isUndefined(temp[p1]) || temp[p1] == null) {
                    delete temp[p1];
                }
            }
            strCols += jsonToString(temp);
            strCols += ",";
        }
        strCols += "]";

        $.ajax({
            url: '/Admin/ContentModel/ajax/ContentModel.asmx/SaveEdit',
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: '{"model":"' + model + '","panel":"' + panel + '","editControls":"' + encodeURIComponent(strCols) + '","copy":"' + copy + '","index":"' + index + '","next":"' + $("#pageProperties #pProperties tr:eq(1) select")[0].value +
			 '","enable":"' + $("#pageProperties #pProperties tr:eq(2) select").val() + '"}',
            success: function (json) {

                //重新绑定BeforeUnload
                $("#myId").val(myidList);
                we7.beforeUnload("表单已经发生改变，您尚未保存，确定离开吗?", IsChange);
                we7.info(json);
            },
            error: function () {
                alert("error");
            }
        });
    }
    //复制控件
    function copySelect() {
        var control = $(".editSelect");
        var tempName = _getGrouth();
        var tempArrayName = control.attr("myID");
        var tempObj = CloneObject(editControls[tempArrayName]);
        editControls[tempName] = tempObj;

        var controlCopy = NewControl(tempName);

        controlCopy.insertAfter(control);


    }
    //删除控件
    function deleteSelect() {
        var cf = confirm("你确定要删除该行么？");
        if (cf) {
            var control = $(".editSelect");
            control.remove();
            var tempArrayName = control.attr("myID");
            delete editControls[tempArrayName];
        }
    }
    String.prototype.Trim = function () {

        return this.replace(/(^\s*)|(\s*$)/g, "");

    }
    function AddConfirm() {
        var label = $("#dialog_label").val();
        if (label == "") {
            alert("标签不能为空!");
            $("#dialog_label").focus();
            return;
        }
        var name = $("#dialog_name").val();
        if (name == "") {
            alert("名称不能为空！");
            $("#dialog_name").focus();
            return;
        }
        var dataType = $("#dialog_dataType").val();
        var maxLength = 0;
        if (dataType == "String") {
            maxLength = $("#dialog_maxlength").val().Trim();
            var r = /^[0-9]*[1-9][0-9]*$/;
            if (!r.test(maxLength)) {
                alert("请输入大于0的整数!");
                return;
            }
        }
        var title = $("#dialog_checkTitleField").attr("checked").toString();

        var search = $("#dialog_checkSearchField").attr("checked").toString();

        var data = { model: model, label: label, name: name,
            title: title, search: search,
            dataType: dataType,
            maxLength: maxLength
        };

        $.ajax({
            url: '/Admin/ContentModel/ajax/ContentModel.asmx/AddSingleDataColumn',
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: jsonToString(data),
            success: function (json) {
                var ajaxMessage = stringToJSON(json);

                if (ajaxMessage.Success) {
                    alert(ajaxMessage.Message);
                    //插入到下拉框

                    var newData = { Label: label, Name: name };
                    dataColumns.push(newData);
                    $("#ddlSelect").replaceWith(NewColumnDropdownlist());
                    $("#ddlSelect option:last").attr("selected", "selected");
                    $("#ddlSelect").trigger("change");
                    $("#dialog").dialog("close");
                    editControls[$(".editSelect").attr("myID")].ID = name;
                } else {
                    alert(ajaxMessage.Message);
                }

            },
            error: function () {
                alert("出错啦！");
            }
        });
    }

    //初始化
    $(document).ready(function () {
        index = 0;
        $("#formFields").sortable({
            axis: 'y',
            stop: function (event, ui) {
                if (ui.item.is('.ui-draggable')) {
                    var control = $(ui.item);
                    var type = control.attr("id");
                    var setting = CloneObject(controlsSetting[type]);
                    var tempName = _getGrouth();
                    editControls[tempName] = setting;
                    var controlNew = NewControl(tempName);
                    controlNew.insertAfter(ui.item);
                    ui.item.remove();
                }
            }
        });

        $("#side").tabs();
        loadControlType();
        DropControl();
        getdataColumns();
        /********/
        initPageProperty();
        /*******/
        initEditControls();
        $("#btnSave").click(function () { SaveEditControl(); });
        $("#addControl").click(function () {
            $(".editSelect").removeClass("editSelect");
            $("#stage").removeClass("cfi");
            $("#stage").addClass("afi");
            $(".arrow").remove();
            $(".fieldActions").remove();
        });
        $("#setting").click(function () {
            $("#stage").removeClass("afi");
            $("#stage").addClass("cfi");
            $("#gen1").trigger("click");
        });
        $("#btnConfirm").click(function () {
            AddConfirm();
        });



    });



    //已废除
    function copyToUserCenter() {
        var data = { model: model };
        var copy = confirm("复制到会员中心会覆盖会员中心原有控件,是否继续覆盖?");
        if (copy) {
            $.ajax({
                url: '/Admin/ContentModel/ajax/ContentModel.asmx/CopyToUserCenter',
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: jsonToString(data),
                success: function (json) {
                    var ajaxMessage = stringToJSON(json);

                    if (ajaxMessage.Success) {
                        alert(ajaxMessage.Message);

                    } else {
                        alert(ajaxMessage.Message);
                    }

                },
                error: function () {
                    alert("出错啦！");
                }
            });
        }
    }
})();