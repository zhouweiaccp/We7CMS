function TemplateBuilder(params) {

    var winId = "__modelWin", frameId = "__modelFrame";
    var modelWin, frame;
    var ShowListSuccess, ShowBuilderSuccess, ShowListFailure, ShowBuilderFailure;

    function Init() {

        params = params || {};
        ShowListSuccess = params.ShowListSuccess;
        ShowBuilderSuccess = params.ShowBuilderSuccess;
        ShowListFailure = params.ShowListFailure;
        ShowBuilderFailure = params.ShowBuilderFailure;

        if (!modelWin) {
            modelWin = $("<div id='" + winId + "' style='display:none'></div>");
            $("body").append(modelWin);
        }
        if (!frame) {
            frame = $("<iframe id='" + frameId + "' name='" + frameId + "' style='width:100%; height:100%' frameborder='0' scrolling='auto'></iframe>");
            modelWin.append(frame);
        }

    }

    function GetTicks() {
        return new Date().getMilliseconds() + Math.random();
    }

    this.showList = function () {
        frame.attr("src", "/Admin/VisualTemplate/DataControlPanel/DataControlList.aspx?t=" + GetTicks());
        var dlg = modelWin.dialog({
            height: 500,
            width: 700,
            modal: true,
            resizable: false,
            title: "部件编辑",
            buttons: {
                "取消": function () {
                    $(this).dialog("close");
                },
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
                    //if (document.frames[frameId] && document.frames[frameId].window && document.frames[frameId].window.Ok) {
                    if (innerIfr && iframeWin && iframeWin.Ok) {
                        var result = iframeWin.Ok(); // document.frames[frameId].window.Ok();
                        if (result) {
                            if (ShowListSuccess) {
                                ShowListSuccess(result);
                            }
                            $(this).dialog("close");
                            return;
                        }
                    }

                    if (ShowListFailure) {
                        ShowListFailure();
                    }
                    $(this).dialog("close");
                }
            }
        });
        dlg.parent().appendTo(jQuery("form:first"));
    }

    this.showBuilder = function (ctr, template, folder, dc) {
        window["DC"] = dc;
        frame.attr("src", "/Admin/VisualTemplate/DataControlPanel/WidgetBuilder.aspx?isFirst=0&file=" + escape(ctr) + "&template=" + template + "&folder=" + folder + "&cssclass=" + dc.cssclass + "&t=" + GetTicks());
        modelWin.dialog({
            height: 500,
            width: 700,
            modal: true,
            resizable: false,
            title: "部件编辑",
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

                    //if(document.frames[frameId]&&document.frames[frameId].window&&document.frames[frameId].window.Ok)
                    if (innerIfr && iframeWin && iframeWin.Ok) {
                        var result = iframeWin.Ok(); // document.frames[frameId].window.Ok();	                
                        if (result) {
                            if (ShowBuilderSuccess) {
                                result.filename = ctr;
                                ShowBuilderSuccess(result);
                            }
                            $(this).dialog("close");
                            window["DC"] = null;
                            return;
                        }
                    }

                    if (ShowBuilderFailure) {
                        ShowBuilderFailure();
                    }
                    $(this).dialog("close");
                    window["DC"] = null;
                },
                "取消": function () {
                    $(this).dialog("close");
                }
            }
        });
    }

    Init();
}