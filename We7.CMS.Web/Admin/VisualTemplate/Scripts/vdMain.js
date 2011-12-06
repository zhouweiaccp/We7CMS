/**
*拖拽页面主页面操作
*/
var showShop = false;

function GetChildLoading() {
    var frameId = "pageframe";


    if (document.all) {

        return window.frames(frameId).CreateLoading();

    }
    else {

        return document.getElementById(frameId).contentWindow.CreateLoading();

    }


}
var frameName = "pageframe";
var dd = new DragNewWidget({ add: function (event, ui) {

    var loading = GetChildLoading(); //$('<img id="vdloading" src="/Admin/VisualTemplate/images/ajax-loader.gif"/>').appendTo($("#pageframe").contents().find("body"))// document.getElementById("pageframe").contentWindow.CreateLoading();  //$('<img id="vdloading" src="/Admin/VisualTemplate/images/ajax-loader.gif"/>');
    //var loadingHtml='<img id="vdloading" src="/Admin/VisualTemplate/images/ajax-loader.gif"/>';
    var item = ui.item;

    var nextid = $(ui.placeholder).next(".RadDock").attr("controlid");
    var target = $(ui.placeholder).parent(".RadDockZone").attr("controlid");
    if (!nextid || nextid == "undefined") {
        nextid = "";
    }
    if (!target || target == "undefined") {
        target = "body";
    }


    $(ui.placeholder).replaceWith(loading);
    //writeCapture.replaceWith(ui.placeholder,loading.html());
    if ($(item).hasClass("layoutitem")) {
        //添加布局
        var id = generatorId("we7layout");
        var data = $(item).attr("data");
        //转换成json格式
        data = stringToJSON(data);
        Layout.add(id, data.path, target, nextid, function (html) {

            //替换对象
            writeCapture.replaceWith(loading, html);
            document.getElementById("pageframe").contentWindow.refresh();

        });
    }
    if ($(item).hasClass("widget")) {
        var data = $(item).attr("data");

        data = unescape(data);
        var Me = this;
        var params = stringToJSON(data);

        params.id = generatorId(params.control);
        WidgetController.add(target, nextid, params, function (json) {
            document.getElementById("pageframe").contentWindow.Replace(loading, json);
            loading.remove();
            document.getElementById("pageframe").contentWindow.refresh();


        }, function (XMLHttpRequest, textStatus, errorThrown) {
            //错误处理
            loading.remove();
            alert(textStatus);
        });
    }
}
});

//iframe 自适应高度

function resizeContent() {
    $("#pageframe").height($("#pageframe").contents().height() + 20);
}

var widgetGroup = null;


function GroupShow(target) {
    if ($("#ul" + target).css("display") != "none") {
        // $("#" + target).slideUp("slow");
        $("#ul" + target).hide(500);
        $("#img" + target).attr("src", "images/folder.png");
    }
    else {
        $("#ul" + target).show(500);
        $("#img" + target).attr("src", "images/folder-open.png");
        // $("#" + target).slideDown("slow");
    }
}
/*加载控件列表*/
function LoadWidgetList() {

    var loading = $('<img  src="/Admin/VisualTemplate/images/ajax-loader.gif"/>'); ;
    $("#widgets").append(loading);

    $.ajax({
        async: true,
        type: "POST",
        url: "/Admin/VisualTemplate/VisualService.asmx/GetSystemWidgets",
        dataType: "json",
        data: {},
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            try {
                json = eval('(' + json + ')');
                if (json.Success) {
                    //返回成功
                    $("#widgets").empty(); //清空

                    var groups = json.Data.Groups;
                    widgetGroup = groups;

                    $(groups).each(function () {
                        var widgetGropusDiv = $('<ul></ul>');
                        $(this.Widgets).each(function () {
                            var defaultType = this.DefaultType;
                            var path;
                            var widgeDes = $('<li height="auto;"><a onclick="javascirpt:GroupShow(\'' + this.Name + '\')"   target="_self" href="javascript:void(0)" title="' + this.Label + '"><h3><samp><img src="images/folder.png"  id="img' + this.Name + '" style="border:0px;" /></samp><span>' + this.Label + '</span></h3></a></li>');
                            var widgeDiv = $('<ul id="ul' + this.Name + '" style="display:none"></ul>');

                            $(this.Types).each(function () {
                                path = this.File; //路径
                                var data = { control: this.Control, filename: path };
                                var dataStr = escape(jsonToString(data)); //编码
                                widgeDiv.append('<li style="margin-left:5px;" class="widget dragitem" widgetname="' + this.Name + '" data="' + dataStr + '">' + this.Label + '</li>');
                                widgeDes = widgeDes.append(widgeDiv);
                                widgetGropusDiv.append(widgeDes);
                            });
                        });
                        $("#widgets").append(widgetGropusDiv);
                    });
                }
                else {
                    alert(json.Message);
                }
            }
            catch (err) {
                alert(err);
            }
            dd.refresh();
            loading.remove();
            if (showShop) {
                $("#widgets a").each(function (i) {
                    if (this.title === "商城下载类"){
                        this.click();                        
                    }
                });
                showShop = false;
            }
        }
    });
}
/*检查商城绑定状态*/
function CheckBindShopState() {
    var loading = $('<img  src="/Admin/VisualTemplate/images/ajax-loader.gif"/>'); ;
    $("#shopproducts").append(loading);

    $.ajax({
        async: true,
        type: "POST",
        url: "/Admin/VisualTemplate/VisualService.asmx/GetSiteAuthroizeState",
        dataType: "json",
        data: {},
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            try {
                json = eval('(' + json + ')');
                //是否绑定商城帐号
                if (json.Success) {
                    //返回成功
                    $("#shopproducts").empty(); //清空
                    if (json.Data) {
                        var html = "<ul class='shoplist'>";
                        for (var i = 0; i < json.Data.length; i++) {
                            var product = json.Data[i];
                            html += "<li>";
                            html += "<table><tr><td width='80'>"
                            html += "<img src='" + product.Thumbnail + "' width='80' height='80' />";
                            html += "</td>";
                            html += "<td valign=top>";
                            html += "<div><a href='" + product.PageUrl + "' title='" +
                                product.Description + "' target='_blank'>" + product.Name + "</a></div>";
                            html += "<div class='fl half'><b>销量：</b><em>" + product.Sales + "</em></div>";
                            html += "<div class='fl half'><b>人气：</b><em>" + product.Point + "</em></div>";
                            html += "<div class='clear'></div>";
                            html += "<div><b>店铺：</b><a href='" + product.StoreUrl + "' title='" + 
                                    product.StoreName + "' target='_blank'>" + product.StoreName + "</a></div>";
                            if (product.State == 0)
                                html += "<input type='button' onclick='InstallWidget(\"" +
                                    product.ID + "\",\"" + product.Name + "\",\"" + product.Url + "\");' value='安装' />";
                            else if (product.State == 1)
                                html += "<input type='button' onclick='UpdateWidget(\"" +
                                    product.ID + "\",\"" + product.Name + "\",\"" + product.Url + "\");' value='更新' />";
                            html += "</td></tr></table>";
                            html += "</li>";
                        }
                        html += "</ul>";
                        $("#shopproducts").html(html);
                    }
                    else {
                        $("#shopproducts").html(json.Message);
                    }
                }
                else {
                    $("#shopproducts").html("尚未绑定商城登录名，请先进行<a href='/admin/Plugin/PluginAdd.aspx?tab=2' target='_blank'>绑定</a>");
                }
            }
            catch (err) {
                alert(err);
            }
        },
        error: function(jqXHR, textStatus, errorThrown){
            alert(jqXHR.responseText);
        }
    });
}
/*安装部件*/
function InstallWidget(id, name, url) {
    if (confirm('确认安装此部件？')) {
        $.ajax({
            async: true,
            type: "POST",
            url: "/Admin/VisualTemplate/VisualService.asmx/Install?id=" + id, //+"&name="+name+"&url="+url
            dataType: "json",
            data: {},
            contentType: "application/json; charset=utf-8",
            success: function (json) {
                try {
                    json = eval('(' + json + ')');
                    //是否绑定商城帐号
                    if (json.Success) {
                        var fillUrl = json.Message;
                        var remoteUrl = "/admin/VisualTemplate/InstallWidget.aspx?action=remoteinstall&purl=" + fillUrl;
                        $.colorbox({ width: "70%", height: "80%", href: remoteUrl, iframe: true,
                            overlayClose: false, escKey: false,
                            onClosed: function () {
                                CheckBindShopState();
                                $("#widgettab").click();
                                $("#window_refreshButton_3").click();
                                 showShop = true;
                            }
                        });
                    }
                    else {
                        alert(json.Message);
                    }
                } catch (err) {
                    alert(err);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR.responseText);
            }
        });
    }
}
/*更新插件*/
function UpdateWidget(id, name, url) {
    if (confirm('确认更新此部件？')) {
        var remoteUrl = "/admin/VisualTemplate/InstallWidget.aspx?action=remoteupdate&purl=" + url;
        $.colorbox({ width: "70%", height: "80%", href: remoteUrl, iframe: true,
            overlayClose: false, escKey: false,
            onClosed: function () {
                CheckBindShopState();
                $("#widgettab").click();
                $("#window_refreshButton_3").click();
                showShop = true;
            }
        });
    }
}
/*布局添加修改*/
var Layout = {
    add: function (id, path, target, nextid, success) {
        var url = "/Admin/VisualTemplate/RendLayout.aspx";
        url = setUrlParam(url, "id", id);
        url = setUrlParam(url, "path", path);
        url = setUrlParam(url, "target", target);
        url = setUrlParam(url, "nextid", nextid);
        url = setUrlParam(url, "state", "design");
        url = setUrlParam(url, "file", file);
        url = setUrlParam(url, "folder", folder);
        $.ajax(
         {
             async: true,
             type: "GET",
             url: url,
             dataType: "html",
             error: function () { alert("创建布局出错啦!"); },
             success: function (html) {
                 //回调函数
                 if (success && jQuery.isFunction(success)) {
                     success(html);
                 }
             }

         });
    }

}

var TempLayout = null;
function EditLayout(params) {
    var winId = "__layoutWin", frameId = "__layoutFrame";
    var modelWin, frame;
    var ShowBuilderSuccess, ShowBuilderFailure;
    var Me = this;
    function Init() {

        params = params || {};

        ShowBuilderSuccess = params.ShowBuilderSuccess;

        ShowBuilderFailure = params.ShowBuilderFailure;

        if (!modelWin) {
            modelWin = $("<div id='" + winId + "' style='display:none'></div>");
            $("body").append(modelWin);
        }

        if (!frame) {
            frame = $("<iframe id='" + frameId + "' name='" + frameId + "' style='width:100%; height:100%' frameborder='0' scrolling='auto'></iframe>");
            modelWin.append(frame);
        }
        function GetTicks() {
            return new Date().getMilliseconds() + Math.random();
        }
        Me.showBuilder = function (dc) {
            TempLayout = dc;
            frame.attr("src", "/Admin/VisualTemplate/Windows/LayoutProperty.html?file=" + file + "&folder=" + folder + "&t=" + GetTicks());
            var dlg = modelWin.dialog({
                height: 500,
                width: 700,
                modal: true,
                resizable: false,
                title: "布局编辑",
                buttons: {                    
                    "确定": function () {
                        var innerIfr;
                        var iframeWin;
                        if (document.all) {
                            //IE
                            innerIfr = document.frames[frameId];
                            iframeWin = document.frames[frameId].window;
                        }
                        else {
                            //FF
                            innerIfr = document.getElementById(frameId);
                            iframeWin = document.getElementById(frameId).contentWindow;
                        }


                        if (innerIfr && iframeWin && iframeWin.Ok) {
                            var result = iframeWin.Ok();
                            if (result) {
                                if (ShowBuilderSuccess) {
                                    ShowBuilderSuccess(result, file, folder);
                                }
                                $(this).dialog("close");

                                return;
                            }
                        }

                        if (ShowBuilderFailure) {
                            ShowBuilderFailure();
                        }
                        $(this).dialog("close");
                        window["DC"] = null;
                    }
                    ,
                    "取消": function () {
                        $(this).dialog("close");
                    }
                }
            });
            dlg.parent().appendTo(jQuery("form:first"));
        }
    }
    Init();
}
/**
*widget操作
*/

var tempControlid;
var WidgetController = {
    /**
    **添加
    */
    add: function (target, nextid, params, callback, error) {
        var attributes = jsonToString(params);
        attributes = StringUtil.Base64Encode(attributes);
        $.ajax(
         {
             async: true,
             type: "POST",
             url: "/Admin/VisualTemplate/RenderWidget.aspx?action=add&file=" + file + "&folder=" + folder + "&target=" + target + "&nextid=" + nextid + "&virtualdata=" + virtualdata + "&state=design1", //&params=" + paramsString,
             dataType: "text",
             data: { "params": attributes },
             error: function (XMLHttpRequest, textStatus, errorThrown) { error(XMLHttpRequest, textStatus, errorThrown); },
             success: function (json) {
                 var cssClass = params.filename.substr(params.filename.lastIndexOf("/") + 1);
                 cssClass = cssClass.substr(0, cssClass.lastIndexOf("."));
                 var style = params.filename.substr(0, params.filename.lastIndexOf("/")) + "/Style/" + cssClass + ".css";
                 var head = document.getElementById("pageframe").contentWindow.document.getElementsByTagName('head').item(0);
                 var link = head.getElementsByTagName("link");
                 for (var i in link) {
                     if (link[i].getAttribute) {
                         if (link[i].getAttribute("href") == style) {
                             head.removeChild(link[i]);
                             break;
                         }
                     }
                 }

                 var css = document.createElement('link');
                 css.href = style + "?" + Math.random();
                 css.rel = 'stylesheet';
                 css.type = 'text/css';
                 head.appendChild(css);
                 //进行处理
                 callback(json);
             }
         });
    }
   , edit: function (data) {
       tempControlid = data.atts.id;

       builder.showBuilder(data.atts.filename, file, folder, data.atts);
   },
    editapp: function (data) {
        tempControlid = data.id;
        var div = AddMask($("#pageframe").contents().find("#" + tempControlid + "_widget"));

        var attributes = StringUtil.Base64Encode(jsonToString(data));
        $.ajax(
            {
                async: true,
                error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); div.remove(); },
                type: "POST",
                url: "/Admin/VisualTemplate/RenderWidget.aspx?action=edit&file=" + file + "&original=" + tempControlid + "&folder=" + folder + "&virtualdata=" + virtualdata + "&state=design1", //&params=" + attributes,
                data: { "params": attributes },
                dataType: "text",
                success: function (json) {
                    var cssClass = data.filename.substr(data.filename.lastIndexOf("/") + 1);
                    cssClass = cssClass.substr(0, cssClass.lastIndexOf("."));
                    var style = data.filename.substr(0, data.filename.lastIndexOf("/")) + "/Style/" + cssClass + ".css";
                    var head = document.getElementById("pageframe").contentWindow.document.getElementsByTagName('head').item(0);
                    var link = head.getElementsByTagName("link");
                    for (var i in link) {
                        if (link[i].getAttribute) {
                            if (link[i].getAttribute("href") == style) {
                                head.removeChild(link[i]);
                                break;
                            }
                        }
                    }

                    var css = document.createElement('link');
                    css.href = style + "?" + Math.random();
                    css.rel = 'stylesheet';
                    css.type = 'text/css';
                    head.appendChild(css);
                    //进行处理
                    document.getElementById("pageframe").contentWindow.Replace($("#pageframe").contents().find("#" + tempControlid + "_widget"), json);

                    document.getElementById("pageframe").contentWindow.refresh();

                    div.remove();
                }
            });
    },
    editwrapper: function (data) {
        tempControlid = data.data.atts.id;
        var div = AddMask($("#pageframe").contents().find("#" + tempControlid + "_widget"));

        var attributes = StringUtil.Base64Encode(jsonToString(data.data.atts)); 
        $.ajax(
            {
                async: true,
                error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); div.remove(); },
                type: "POST",
                url: "/Admin/VisualTemplate/RenderWidget.aspx?action=edit&file=" + file + "&original=" + tempControlid + "&folder=" + folder + "&virtualdata=" + virtualdata + "&state=design1", //&params=" + attributes,
                data: { "params": attributes },
                dataType: "text",
                success: function (json) {
                    //进行处理


                    document.getElementById("pageframe").contentWindow.Replace($("#pageframe").contents().find("#" + tempControlid + "_widget"), json);

                    document.getElementById("pageframe").contentWindow.refresh();

                    div.remove();
                }
            });
    }
};
var builder = null;
$(function () {

    builder = new TemplateBuilder({
        ShowListSuccess: function (result) {
            var data = jsonToString(result);
        },
        ShowListFailure: function () { alert("ShowListFailure"); },
        ShowBuilderSuccess: function (result) {
            var div = AddMask($("#pageframe").contents().find("#" + tempControlid + "_widget"));

            var attributes = StringUtil.Base64Encode(jsonToString(result));
            $.ajax(
            {
                async: true,
                error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); div.remove(); },
                type: "POST",
                url: "/Admin/VisualTemplate/RenderWidget.aspx?action=edit&file=" + file + "&original=" + tempControlid + "&folder=" + folder + "&virtualdata=" + virtualdata + "&state=design1", //&params=" + attributes,
                data: { "params": attributes },
                dataType: "text",
                success: function (json) {
                    var cssClass = result.filename.substr(result.filename.lastIndexOf("/") + 1);
                    cssClass = cssClass.substr(0, cssClass.lastIndexOf("."));
                    var style = result.filename.substr(0, result.filename.lastIndexOf("/")) + "/Style/" + cssClass + ".css";
                    var head = document.getElementById("pageframe").contentWindow.document.getElementsByTagName('head').item(0);
                    var link = head.getElementsByTagName("link");
                    for (var i in link) {
                        if (link[i].getAttribute) {
                            if (link[i].getAttribute("href") == style) {
                                head.removeChild(link[i]);
                                break;
                            }
                        }
                    }

                    var css = document.createElement('link');
                    css.href = style + "?" + Math.random();
                    css.rel = 'stylesheet';
                    css.type = 'text/css';
                    head.appendChild(css);
                    //var head = $(window.frames["pageframe"].document).find("head").html()
                    //$(window.frames["pageframe"].document).find("head").html(head + "<link href=\"" + style + "\" type=\"text/css\" rel=\"stylesheet\" />");

                    //进行处理
                    document.getElementById("pageframe").contentWindow.Replace($("#pageframe").contents().find("#" + tempControlid + "_widget"), json);

                    document.getElementById("pageframe").contentWindow.refresh();

                    div.remove();
                }
            });

        },
        ShowBuilderFailure: function () { alert("ShowBuilderFailure"); }
    });
});

function GetChildMask() {
    var frameId = "pageframe";
    var innerIfr;
    var iframeWin;

    if (document.all) {

        return window.frames(frameId).CreateMask();

    }
    else {

        return document.getElementById(frameId).contentWindow.CreateMask();

    }
}
function AddMask(el) {


    var maskDiv = GetChildMask(); // $('<div class="maskdiv"></div>');
    var loading = GetChildLoading(); //$('<img src="/Admin/VisualTemplate/images/ajax-loader.gif"/>');
    var select = el;
    var offset = select.offset();
    var left = offset.left;
    var top = offset.top;
    var width = select.width();
    var height = select.height();
    maskDiv.css({ "left": left, "top": top, "position": "absolute", "width": width,
        opacity: 0.8,
        "z-index": 999,
        "height": height, "text-align": "center", background: 'url("/admin/visualtemplate/images/ui-bg_flat_50_5c5c5c_40x100.png") repeat-x scroll 50% 50% #5C5C5C'
    });
    loading.css({ "margin-top": height / 2 }).appendTo(maskDiv);
    maskDiv.appendTo(select);
    return maskDiv;
}

//删除Wideget以及layout
function DeleteWidget(id) {
    var c = confirm("你确定要删除么?");
    if (!c) {
        return;
    }
    //添加遮罩
    var div = AddMask($("#" + frameName).contents().find("#" + id + "_widget"));
    $.ajax(
         {
             async: true,
             type: "POST",
             url: "/Admin/VisualTemplate/WidgetAjaxService.ashx?action=delete&file=" + file + "&folder=" + folder + "&id=" + id,
             dataType: "json",
             data: {},
             error: function () { alert("删除作失败!"); div.remove(); },
             success: function (json) {
                 if (json.Success) {
                     $("#" + frameName).contents().find("#" + id + "_widget").remove();
                     alert("删除成功!");
                 }
                 else {
                     alert(json.Message);
                 }
                 div.remove();
                 document.getElementById("pageframe").contentWindow.refresh();
             }
         });

}


function MoveWidget(target, id, nextid) {
    $.ajax(
         {
             async: true,
             type: "POST",
             url: "/Admin/VisualTemplate/WidgetAjaxService.ashx?action=move&target=" + target + "&nextid=" + nextid + "&file=" + file + "&folder=" + folder + "&id=" + id,
             dataType: "json",
             success: function (json) {
                 if (json.Success) {
                 }
                 else {
                     alert(json.Message);
                 }
             }
         });
}


function MessageTips(target) {
    var _warp = $('<div style="height:15px;line-height:15px;text-align:center;"></div>');
    var _closebtn = $('<img title="关闭" src="/admin/images/icon_del.gif"/>').click(function () {

        $(target).empty().fadeOut();
    });


    this.showInfo = function (msg) {
        $(target).empty();
        var info = '<div style="color:green;">' + msg + '</div>';
        _warp.append(info).append(_closebtn);
        $(target).append(_warp).fadeIn();
    }
}
var MessageLable = {

    showLoading: function () {
        var loading = $('<img  src="/Admin/VisualTemplate/images/ajax-loader.gif"/>');
        $("#messagelabel").append(loading);
    },
    showLoadingWithMsg: function (msg) {
        $("#messagelabel").removeClass("warning").addClass("publishInfo").html('<img  src="/Admin/VisualTemplate/images/ajax-loader.gif" />' + msg).show();
    },
    showInfo: function (msg) {
        $("#messagelabel").html('<img style="border-width:0px;" src="/admin/images/ico_info.gif">' + msg).show();

        // $("#messagelabel").removeClass("warning").addClass("publish").html(msg).fadeIn();
    },
    showError: function (msg) {
        $("#messagelabel").removeClass("info").addClass("warning").html(msg).fadeIn();
    },
    hide: function () {
        $("#messagelabel").empty().hide();
    }
};
function BindEvent() {
    $("#btnPublish").click(function () {

        if (CommonData && CommonData.IsDemoSite) {
            alert("演示站点禁止发布");
            return;
        }
      
       MessageLable.showLoadingWithMsg("正在发布,请稍候....");
        $.ajax(
         {
             async: false,
             type: "POST",
             url: "/Admin/VisualTemplate/WidgetAjaxService.ashx?action=publish&file=" + file + "&folder=" + folder,
             dataType: "json",
             success: function (json) {
                 if (json.Success) {
                     MessageLable.showInfo("发布成功!");
                     setTimeout(MessageLable.hide, 2000);
                 }
                 else {

                     MessageLable.showError("发布失败!原因可能是:" + json.Message);
                     setTimeout(MessageLable.hide, 2000);
                 }
             }
         });
    });
}


/**
*模板操作
*/
function VisualLayout(warp) {
    var _oldSrc = "";
    var _newSrc = "";
    var theme;
    var Me = this;
    var change = false;
    /**
    *生成列表
    */
    var _create = function () {

        $.ajax({
            url: '/Widgets/Themes/Themes.xml',
            success: function (xml) {
                $(xml).find("item").each(function () {
                    var item = $('<div class="layoutskin" data="' + $(this).attr("name") + '"><img src="' + $(this).attr("img") + '" alt="' + $(this).attr("name") + '"/><br/>'+$(this).attr("name")+'</div>');
                    if ($(this).attr("name") == Common.Theme) {
                        item.addClass("selectskin");
                        theme = Common.Theme;
                    }
                    item.click(function () {
                        $(".selectskin").removeClass("selectskin");
                        $(this).addClass("selectskin");
                        _newSrc = "/Widgets/Themes/" + $(this).attr("data") + "/Style.css";
                        theme = $(this).attr("data");
                        Me.change(_newSrc);
                    });
                    $(".items", warp).append(item);
                });
            }
        });

    };
    /**
    *更换新模板
    */
    this.change = function (newUrl) {
        //设置新的更换模板名称
        _newSrc = newUrl;
        var styleDom = $("#pageframe").contents().find(".themestyle")[0];
        if (!change) {

            _oldSrc = $(styleDom).attr("href");
            change = true;
        }
        //查找iframe中的主题进行更换操作
        $(styleDom).attr("href", newUrl).attr("type", "").attr("type", "text/css");
    };
    /**
    *确认更换
    */
    this.submit = function () {

        if (theme == 'undefined' || theme == undefined || theme == "" || theme == null) {
            alert("请选择一个主题!");
            return;
        }
        //判断是否改变
        $.ajax(
         {
             async: true,
             type: "POST",
             url: "/Admin/VisualTemplate/WidgetAjaxService.ashx?action=changetheme&file=" + file + "&folder=" + folder + "&theme=" + theme,
             dataType: "json",
             success: function (json) {
                 if (json.Success) {
                     alert("成功啦");
                     $("#LayoutList").slideUp("slow");
                     $("img", "#changelayout").attr("src", "images/theme-button.png");
                 }
                 else {
                     alert(json.Message);
                 }
             }
         });

    };
    /**
    *取消更换
    */
    this.cancle = function () {
        var styleDom = $("#pageframe").contents().find(".themestyle")[0];
        $(styleDom).attr("href", _oldSrc).attr("type", "").attr("type", "text/css");
        $(".selectskin").removeClass("selectskin");
        $("#LayoutList").slideUp("slow");
        $("img", "#changelayout").attr("src", "images/theme-button.png");
        change = false;
    };

    _create();

    $(".submit", warp).click(function () {
        Me.submit();
    });

    $(".cancle", warp).click(function () {

        Me.cancle();
    });
}



var folder;
var file;
var virtualdata;
var messagebox;

$(document).ready(function () {
    folder = Request.parameter("folder");
    file = Request.parameter("file");
    virtualdata = Request.parameter("virtualdata");
    //加载控件
    LoadWidgetList();
    //获取绑定状态
    CheckBindShopState();
    BindEvent();
    messagebox = new MessageTips("#messagelabel");
    /**实现DIV最大化最小化**/
    $("#btnmax").click(function () {

        $("#toolwarp").animate({ opacity: "1" }, "fast", function () {
            $("#showtoolwarp").hide();

            $("#toolwarp").css("top", ($(window).height() - $(this).height()) / 2 + $(window).scrollTop()).css("left", ($(window).width() - $(this).width()) / 2);

            $("#toolwarp").show();
        })
    });
    $("#btnmin").click(function () {
        $("#toolwarp").animate({ opacity: "0", top: $(document).height(), left: "0" },
        "fast", function () {
            $("#showtoolwarp").show();
            $("#toolwarp").hide();
        });
    });
    $("#LayoutList").hide();
    $("#changelayout").click(function () {
        if ($("#LayoutList").css("display") == "none") {
            $("img", this).attr("src", "images/theme-button-focus.png");
            $("#LayoutList").slideDown("slow");
        }
        else {
            $("img", this).attr("src", "images/theme-button.png");
            $("#LayoutList").slideUp("slow");
        }
    });
    var vl = new VisualLayout($("#LayoutList"));

    /*刷新部件列表*/
    $("#window_refreshButton_3").click(function () {
        $("#widgets").html("");
        LoadWidgetList();
        CheckBindShopState();
    });
});

