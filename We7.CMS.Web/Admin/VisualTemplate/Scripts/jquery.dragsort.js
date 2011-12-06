/// <reference path="jquery-1.4.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.5.custom.min.js" />

/**
*默认参数
*/
DragSort.prototype.defaultOptions = {
    columns: '.RadDockZone',
    items: '>.RadDock',
    handle: '>.RadDock-header',
    helper: function () { return $('<div class="helper" style="height:20px; width:100px; background-color:red;color:Yellow; border:solid 1px gray;">拖拽我吧!</div>'); },
    placeholder: "placeholder"

};

/**
*Drag Drap
*/
function DragSort(options) {
    var Me = this;
    var sort;
    //合并参数
    this.options = $.extend({}, this.defaultOptions, options);

    //可拖拽区域
    this.columns = this.options.columns;
    //可拖拽对象
    this.items = this.options.items;

    this.createPlaceHolder = function () {

        return $('<div></div>').addClass(Me.options.placeholder);
    }
    this.removePlaceholder = function () { $("." + Me.options.placeholder).remove(); };
    this.getPlaceholder = function () {
        return $("." + Me.options.placeholder);
    };
    //刷新
    this.refresh = function () {
        sort = $(Me.items, Me.columns).draggable({
            cursor: 'move',
            iframeFix: true,
            cursorAt: { left: 5 },
            handle: Me.options.handle,
            helper: Me.options.helper,
            start: function (event, ui) {
                ui.item = $(this);
                //隐藏
                ui.item.hide();
                dragStart(event, ui);

                ui.helper.css("left", event.pageX - ui.helper.width() / 2);
                ui.helper.css("top", event.pageY - ui.helper.height() / 2);

                if (Me.options.start, $.isFunction(Me.options.start)) {
                    Me.options.start(event, ui);
                }
            },
            drag: function (event, ui) {

                ui.item = $(this);
                drag(event, ui);
                if (Me.options.sort, $.isFunction(Me.options.sort)) {
                    Me.options.sort(event, ui);
                }

                if (Me.options.sort, $.isFunction(Me.options.sort)) {
                    Me.options.sort(event, ui);
                }
            },
            stop: function (event, ui) {
                ui.item = $(this);
                dragEnd(event, ui);
                //呈现
                ui.item.show();
                if (Me.options.stop, $.isFunction(Me.options.stop)) {
                    Me.options.stop(event, ui);
                }
                refresh();

            }

        }).disableSelection();

    };
    //计算鼠标是否在中容器中
    var checkMouseOver = function (event, column) {

        var pageX = event.pageX;
        var pageY = event.pageY;
        var left = $(column).offset().left;
        var top = $(column).offset().top;
        var width = $(column).outerWidth();
        var height = $(column).outerHeight();

        if (pageX >= left && pageX <= left + width && pageY >= top && pageY <= top + height) {
            return true;
        }
        else {
            return false;
        }
    }

    /**
    *检查鼠标在DOM元素的上半还是下半还是不在
    *@返回1表示在上，2在下，0则不在
    */
    var checkElement = function (event, element) {

        var pageY = event.pageY;
        var height = $(element).outerHeight();
        var top = $(element).offset().top;

        if (pageY >= top && pageY <= top + height) {

            if (pageY <= top + (height / 2)) {
                return 1;
            }
            else {
                return 2;
            }
        } else {
            if (pageY < top) {
                return -1;
            }
            else {
                return 0;
            }
        }
    }
    //开始拖拽
    var dragStart = function (event, ui) {
        $(".apprence").remove();
        //添加占位符
        Me.createPlaceHolder().insertAfter(ui.item).height($(ui.item).height());
    }
    var drag = function (event, ui) {
        //1.遍历所有容器,查看鼠标中心位于哪个column中,可以为套嵌

        var cols = $(Me.columns);
        var overCols = [];
        var hoverCol;
        //获取鼠标

        var pageX = event.pageX;
        var pageY = event.pageY;

        for (var i = 0; i < cols.length; i++) {
            var hover = checkMouseOver(event, cols[i]);
            if (hover) {
                //在容器中
                overCols.push(cols[i]);
            }
        }
        //2.可能为套嵌容器,判断最终在哪个容器:哪个比较高度比较低就在该上
        var tempHeight = 1000000; //一个比较大的值 用于初始比较

        for (var i = 0; i < overCols.length; i++) {
            //比较
            if ($(overCols[i]).outerHeight() < tempHeight) {
                hoverCol = overCols[i];
                tempHeight = $(overCols[i]).outerHeight();
            }
        }
        //3.如果容器不为空则插入占位符到起
        if (hoverCol != null) {

            //4.判断在哪个节点上
            var currentNode = null;
            var postion = 0;

            var children = $(Me.items, hoverCol);

            for (var i = 0; i < children.length; i++) {

                if ($(children[i]).html() == $(ui.item).html())
                    continue;
                var p = checkElement(event, children[i]);
                if (p > 0) {
                    currentNode = children[i];
                    postion = p;
                    break;
                }
            }
            var placeholder = Me.getPlaceholder();
            if (currentNode != null) {
                if (postion == 1) {
                    placeholder.insertBefore(currentNode);
                }
                else if (postion == 2) {
                    placeholder.insertAfter(currentNode);
                }
            } else {
                //没有节点 
                placeholder.appendTo(hoverCol);
            }
        }

    }
    var dragEnd = function (event, ui) {

        var placeholder = Me.getPlaceholder();
        if (placeholder.size() > 0) {

            var update = false;
            if ($(placeholder).next().attr("id") != $(ui.item).attr("id") && $(placeholder).prev().attr("id") != $(ui.item).attr("id")) {
                update = true;
            }
            //把元素插入当前位置
            $(placeholder).replaceWith(ui.item);


        }
        //删除占位符
        Me.removePlaceholder();


        if (update) {

            //执行Update
            if (Me.options.update, $.isFunction(Me.options.update)) {
                Me.options.update(event, ui);
            }
        }
    };

    this.refresh();
};

/**全局变量*/
var dragList;
var warplist;
function SetWarpList() {

    $.ajax({
        async: true,
        type: "POST",
        url: "/Admin/VisualTemplate/VisualService.asmx/GetWarpList",
        dataType: "json",
        data: {},
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            try {
                json = eval('(' + json + ')');
                if (json.Success) {
                    //返回成功

                    warplist = json.Data;
                }
                else {
                    alert(json.Message);
                }

            }
            catch (err) {
                alert(err);

            }

        }
    });
}

//$(".zeControlDock ").live("mouseover",function(){
//    if($("body").hasClass("zeContentMode")){
//    $(">.RadDock-header",this).show();
//    }
//}).live("mouseout",function(){
// if($("body").hasClass("zeContentMode")){
//    $(">.RadDock-header",this).hide();
//    }
//});
$(document).ready(function () {
    BindAppearanceEvent();
    SetWarpList();
    //默认为widget添加模式
    $("body").addClass("zeContentMode");
    //$("body").addClass("RadDockZone");
    dragList = new DragSort({ update: function (event, ui) {
        var id = $(ui.item).attr("controlid");
        var nextid = $(ui.item).next(".RadDock").attr("controlid");
        var parentid = $(ui.item).parent(".RadDockZone").attr("controlid");
        if (!nextid || nextid == "undefined") {
            nextid = "";
        }
        if (!parentid || parentid == "undefined") {
            parentid = "body";
        }
        parent.MoveWidget(parentid, id, nextid);
    }
    });

    //默认为widget添加模式
    $("body").addClass("zeContentMode");

    SetEmptyZone();
    layoutPanel = new parent.EditLayout({ ShowBuilderSuccess: function (data, file, folder) {

        var params = StringUtil.Base64Encode(jsonToString(data));
        $.ajax(
         {
             async: true,
             type: "POST",
             url: "/Admin/VisualTemplate/WidgetAjaxService.ashx?action=editlayout&params=" + params + "&file=" + file + "&folder=" + folder,
             dataType: "json",
             success: function (json) {
                 if (json.Success) {
                     var id = data.ID;
                     $("#" + id + "_ClientState").val(jsonToString(data));
                     $(">.sf_colsOut", "#" + id).each(function (i) {
                         $(this).css("width", data.Columns[i].width + data.Columns[i].widthunit);
                         //mxy修改(2011-03-22)
                         //$(this).attr("style", data.Columns[i].style);
                         //$(this).attr("class", "sf_colsOut " + data.Columns[i].cssclass);
                         $(">.sf_colsIn", this).attr("style", data.Columns[i].style);
                         $(">.sf_colsIn", this).attr("class", "sf_colsIn " + data.Columns[i].cssclass);
                     });
                 }
                 else {
                     alert(json.Message);
                 }
             }

         });


    }
    });
});

/*刷新*/
function refresh() {
    SetEmptyZone();
    parent.resizeContent();
    dragList.refresh();
}
/**设置空*/
function SetEmptyZone() {
    $(".RadDockZone").each(function () {
        if ($(".RadDock", this).size() == 0) {
            $(this).addClass("zeDockZoneEmpty");
        }
        else {
            $(this).removeClass("zeDockZoneEmpty");
        }
    });
}
function EditWidget(id) {
    var data = document.getElementById(id + "_ClientState").value;
    data = stringToJSON(data);
    parent.WidgetController.edit(data.data);
}

function DeleteWidget(id) {
    parent.DeleteWidget(id);

}

function Replace(node, newnode) {
    writeCapture.replaceWith(node, newnode);
}

var layoutPanel = null;

function EditLayout(id) {
    var data = $("#" + id + "_ClientState").val();
    data = stringToJSON(data);
    layoutPanel.showBuilder(data);
}

/**
*外观
*/

function BindAppearanceEvent() {
    $("*:not(.apprence)").live("click", function () {
        $(".apprence").remove();
    });
    $(".apprence .widget").live("click", function () {

        if ($(this).hasClass("current")) {
            $(".apprence").remove();
            return;
        }
        // $(".current").removeClass("current");
        //$(this).addClass("current");
        var data = $(this).attr("data");
        data = unescape(data);
        data = stringToJSON(data);
        parent.WidgetController.editapp(data);
        $(".apprence").remove();
    });

}
function showappearance(id) {

    $(".apprence").remove();
    var p = $("#" + id + "_widget>.RadDock-content");
    var pl = $(p).offset().left;
    var pt = $(p).offset().top;

    var data = document.getElementById(id + "_ClientState").value;
    data = stringToJSON(data);

    var apprence = $('<div class="apprence"></div>');

    var widgetDiv = $('<ul></ul>');
    var ctr = data.data.ctr;

    var ctrP = ctr.split(".")[0];

    var widgets = parent.widgetGroup;

    var currWidgets = null;
    for (var n in widgets) {
        for (var m in widgets[n].Widgets) {
            for (var o in widgets[n].Widgets[m].Types) {
                if (id.indexOf(widgets[n].Widgets[m].Types[o].Name.replace(".", "_")) > -1) {
                    currWidgets = widgets[n].Widgets[m].Types;
                    break;
                }
            }
        }
    }

    $(currWidgets).each(function () {
        //if (this.Name.toLowerCase().indexOf(ctrP.toLowerCase()) > -1) {
        //$(this.Types).each(function () {
        var itemdata = { id: id, control: this.Name, filename: this.File };
        var dataStr = escape(jsonToString(itemdata)); //编码

        if (this.Name.toLowerCase() == ctr.toLowerCase()) {
            widgetDiv.append('<li class="widget current" widgetname="' + this.Name + '" data="' + dataStr + '">' + this.Label + '</li>');
        } else {
            widgetDiv.append('<li class="widget" widgetname="' + this.Name + '" data="' + dataStr + '">' + this.Label + '</li>');

        }
        // });
        // }
    });

    //  var toolbar=$('<div><span><a class="submit">确定</a></span><span><a class="cancle">取消</a></span></div>');
    widgetDiv.appendTo(apprence);
    //  toolbar.appendTo(apprence);
    apprence.css("position", "absolute").css("left", pl).css("top", pt);
    apprence.appendTo("body");
}

function showwarp(id) {
    $(".apprence").remove();
    var p = $("#" + id + "_widget>.RadDock-content");
    var pl = $(p).offset().left;
    var pt = $(p).offset().top;

    var data = document.getElementById(id + "_ClientState").value;
    data = stringToJSON(data);

    var warp = data.data.atts.wrapper;

    var apprence = $('<div class="apprence"></div>');

    var widgetDiv = $('<ul></ul>');
    for (var p in warplist) {
        var pp = { data: { atts: { wrapper: warplist[p]}} };
        var temp = $.extend(true, {}, data, pp);

        var dataStr = escape(jsonToString(temp)); //编码

        var item = $('<li class="warpper" data="' + dataStr + '">' + p + '</li>').click(function () {
            if ($(this).hasClass("current")) {
                $(".apprence").remove();
                return;
            }
            var data2 = $(this).attr("data");
            data2 = unescape(data2);
            data2 = stringToJSON(data2);
            parent.WidgetController.editwrapper(data2);

        });

        if (warp == warplist[p]) {

            item.addClass("current");
        }
        widgetDiv.append(item);
    }
    widgetDiv.appendTo(apprence);
    apprence.css("position", "absolute").css("left", pl).css("top", pt);
    apprence.appendTo("body");
}

function CreateLoading() {
    var p = $('<img id="vdloading" src="/Admin/VisualTemplate/images/ajax-loader.gif"/>');
    p.appendTo("body");
    return p;
}

function CreateMask() {
    var mask = $('<div class="maskdiv"></div>');
    mask.appendTo("body");
    return mask;
}


function CopyParamenter(id) {
    var value = $("#" + id + "_ClientState").val();
    if (value) {
        var json = stringToJSON(value);

        var filename =escape( json.data.atts.filename);
        var folder = Request.parameter("folder");
        var file = Request.parameter("file");
        $("a.widgetpaste").each(function () {
            if ($(this).attr('id').indexOf(id.substring(0, id.indexOf("_"))) > -1 && $(this).attr('id') != id+"_paste")
                $(this).html("<img height='15px' src='/Admin/VisualTemplate/images/ajax-loader.gif' />Copying")
            else $(this).html("")
        })
        $("#" + id + "_copy").html("<img height='15px' src='/Admin/VisualTemplate/images/ajax-loader.gif' />Loading");
        $.ajax(
         {
             async: true,
             type: "POST",
             url: "/Admin/VisualTemplate/WidgetAjaxService.ashx?action=copy&file=" + file + "&folder=" + folder + "&filename=" + filename + "&id=" + id,
             dataType: "json",
             data: {},
             error: function () { alert("复制失败!"); },
             success: function (json) {

                 if (json.Success) {

                     $("a.widgetpaste").each(function () {
                         $(this).html('');
                         var thisId = $(this).attr("id").substring(0, $(this).attr("id").lastIndexOf("_"));
                         if (thisId.indexOf(id.substring(0, id.indexOf("_"))) > -1 && thisId != id) {
                             var t = stringToJSON(json.Message);
                             t.id = thisId;
                             $(this).unbind();
                             $(this).click(function () {
                                 PasteParamenter(thisId, StringUtil.Base64Encode(jsonToString(t)))
                             })
                             $(this).html("粘贴");

                         }
                     });
                     $("#" + id + "_copy").html("已复制");
                     setTimeout(function () { $("#" + id + "_copy").html("复制属性") }, "2000");
                 }
                 else {
                     alert(json.Message);
                 }
             }
         });
    }
}
function PasteParamenter(id, params) {
    $("#"+id+"_paste").html("<img height='15px' src='/Admin/VisualTemplate/images/ajax-loader.gif' />Loading")
    var folder = Request.parameter("folder");
    var file = Request.parameter("file");
    $.ajax(
         {
             async: true,
             type: "POST",
             dataType: "text",
             url: "/Admin/VisualTemplate/RenderWidget.aspx?action=paste&file=" + file + "&nextid=" + id + "&folder=" + folder + "&state=design1",
             data: { "params": params },
             error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); },
             success: function (html) {
                 $("#" + id + "_widget").html(html);
                 if (html.length) {
                     $("a.widgetpaste").each(function () {
                         if ($(this).attr('id').indexOf(id.substring(0, id.indexOf("_"))) > -1) {

                             if ($(this).attr('id') != id + "_paste")
                                 $(this).html("");
                             else
                                 $(this).html("粘贴成功!");
                         }
                     });
                     setTimeout(function () { $("#" + id + "_paste").html("") }, "2000");
                 }

             }
         });
}

function ReLoadWidget(id, params) {
    var folder = Request.parameter("folder");
    var file = Request.parameter("file");
    $.ajax(
         {
             async: true,
             type: "POST",
             dataType: "text",
             url: "/Admin/VisualTemplate/RenderWidget.aspx?file=" + file + "&nextid=" + id + "&folder=" + folder + "&state=design1",
             data: { "params": StringUtil.Base64Encode(jsonToString(params)) },
             error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); },
             success: function (html) {
                 $("#" + id + "_widget").html(html);
             }
         });
}

//编辑部件源码
function EditWidgetCode(id) {
    var value = document.getElementById(id + "_ClientState").value;
    if (value) {
        var json = stringToJSON(value);
        var filename = escape(json.data.atts.filename);
        var r=false;
        if (value.indexOf("静态类")!=-1) {
            r = window.showModalDialog(
            '/Admin/VisualTemplate/HtmlWidgets/HtmlWidgets.aspx?ctr=' + filename + "&t=" + (new Date()).valueOf()
            , '<%=Request["ctr"] %>'
            , 'scrollbars=no;resizable=no;help=no;status=no;center=1; dialogHeight=450px;dialogwidth=830px;');
        }
        else {
            r = window.showModalDialog(
            '/Admin/VisualTemplate/WidgetEditor/WidgetEditor.aspx?ctr=' + filename + "&t=" + (new Date()).valueOf()
            , '<%=Request["ctr"] %>'
            , 'scrollbars=no;resizable=no;help=no;status=no;center=1; dialogHeight=450px;dialogwidth=830px;');
        }
        if (r!=null&&r!='') {
            parent.LoadWidgetList(); //刷新we7工具箱
            //ReLoadWidget(id, json.data.atts);  //刷新部件
            json.data.atts.filename = r;
            parent.WidgetController.editapp(json.data.atts);
            refresh(); //刷新
        }
    }
}
