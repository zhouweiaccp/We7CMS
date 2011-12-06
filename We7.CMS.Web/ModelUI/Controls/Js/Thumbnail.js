//图下构建
function ImageBuilder(params) {

    var winId = "__modelWin_Uploader",
            frameId = "__modelFrame_Uploader",
            baseFrameId = "__modelFrame_Uploader_Attachment",
            Obj = this,
            AppendButton,
            modelWin,
            frame,
            Success,
            Src,
            ImageShowCt,
            ImageShow,
            ImageType,
            HiddenValue = params.HiddenValue,
            Body = $("body"),
            DataList = [],
            FrameWidth = params.FrameWidth||500,
            FrameHeight = params.FrameHeight||400,
            Container = params.Ct,
            ArticleID = params.ArticleID;

    function Init() {

        params = params || {};
        Success = params.Success;
        Src = params.Src;

        if (!modelWin) {
            modelWin = $("<div id='" + winId + "' style='display:none'></div>");
            Body.append(modelWin);
        }

        if (!frame) {
            NewIframe();
        }

        var data = Eval("(" + HiddenValue.val() + ")");

        DataList = data ? data : [];

        CreateAppendButton();

        ShowThumbnail(data);

        InitImageShow();

    }

    function InitImageShow() {
        ImageShowCt = $("<div style='position:absolute; display:block; background:#f6f6f6;padding:4px;border:solid 1px #e0e0e0'></div>");
        ImageShow = $("<img style='width:200px; height:200px;' />");
        ImageType=$("<div style='text-align:center;padding:2px;'></div>")
        var inner = $("<div style='padding:1px; border:#F2f2f2; background:#fff'></div>");
        ImageShowCt.append(inner);
        inner.append(ImageShow);
        inner.append(ImageType);
        Body.append(ImageShowCt);
        ImageShowCt.hide();
    }

    function DisplayImageShow(src, x, y,desc) {
        ImageShowCt.css("top", y);
        ImageShowCt.css("left", x);
        ImageShowCt.show();
        ImageShow.attr("src", src);
        ImageType.text(desc);
    }

    function HiddenImageShow() {
        ImageShowCt.hide();
    }

    function CreateAppendButton() {

        var item = { src: '/ModelUI/skin/images/flower.jpg', size: '100*100', type: 'orign' };

        AppendButton = $("<div style='float:left;cursor:pointer'><img src='" + item.src + "' style='width:56px; height:56px;' title='点击添加图片'/></div>").data("item", item).click(function() {
            ImageShowCt.hide();
            Obj.ShowUploader();
        });

        Container.append(AppendButton);
    }

    function NewIframe() {

        if (frame) frame.remove();

        frameId = baseFrameId + (new Date().getMilliseconds());
        frame = $("<iframe id='" + frameId + "' name='" + frameId + "' style='width:100%; height:100%' frameborder='0' scrolling='auto'></iframe>");
        modelWin.append(frame);

    }

    function GetDisplayImageItem(data) {
        if (isArray(data)) {
            for (var i in data) {
                if (data[i].type && data[i].type!="orign") {
                    return data[i];
                }
            }
            if (data.length > 0) {
                return data[0];
            }
        }
    }

    function ShowThumbnail(data) {
        if (isArray(data)) {
            for (var i in data) {
                AppendBefore(data[i]);
            }
        }
    }

    function AppendBefore(param) {

        if (!isArray(param) && param.length)
            return;

        var item = GetDisplayImageItem(param);

        var ct = $("<div style='float:left;margin:1px;cursor:pointer'></div>");
        var ctInner = $("<div style='border:#e0e0e0 1px solid;padding:2px; background:#f4f4f4'><div style='border:solid 1px #f4f4f4; background:#fff'><img src='" + item.src + "' title='"+item.type+"' style='width:50px; height:50px;' /></div></div>").data("item", item);
        ct.append(ctInner)
        ct.append($("<div style='text-align:center; padding:5px 0 0 0'><image src='/Admin/images/icon_cancel.gif' /></div>").data("ct", ct).data("param", param).click(function() {

            RemoveItem($(this).data("param"));
            var c = $(this).data("ct");
            c.remove();

        }));

        ctInner.hover(function() {
            var offset = $(this).offset();
            window.status = "x:" + offset.left + "y:" + offset.top;
            var item=$(this).data("item");
            DisplayImageShow(item.src, offset.left, (offset.top - ImageShowCt.height()),item.desc);
        }, function() {
            HiddenImageShow();
        });

        AppendButton.before(ct)
    }

    function GetTicks() {
        return new Date().getMilliseconds() + Math.random();
    }

    function isArray(obj) {
        return obj && Object.prototype.toString.call(obj) === "[object Array]";
    }

    function Eval(s) {
        var retval = null;
        try {
            retval = eval("(" + s + ")");
        }
        catch (e) { }
        return retval;
    }

    function AppendValue(item) {
        if (isArray(item)) {
            DataList = DataList ? DataList : [];
            DataList.push(item);
            ToJson(DataList);
        }
    }

    function RemoveItem(item) {
        if (isArray(item)) {
            var i = 0;
            for (; i < DataList.length; i++) {
                if (DataList[i] == item)
                    break;
            }
            if (i < DataList.length) {
                DataList.splice(i, 1);
                ToJson(DataList);
            }
        }
    }

    function ToJson(result) {
        if (result) {
            var s = "[";
            for (var i in result) {
                var data = result[i];
                if (isArray(data)) {
                    s += "[";
                    for (var j in data) {
                        s += "{";
                        for (var n in data[j]) {
                            s += n + ":'" + data[j][n] + "',";
                        }
                        s = s.replace(/,$/ig, "");
                        s += "},";
                    }
                    s = s.replace(/,$/ig, "");
                    s += "],";
                }
            }
            s = s.replace(/,$/ig, "");
            s += "]";
            HiddenValue.val(s);
        }
    }

    this.ShowUploader = function() {
    
        if (!$.browser.msie) {
            NewIframe();
        }
    
        frame.attr("src", Src + "?aid=" + ArticleID + "&t=" + GetTicks());
        modelWin.dialog({
            height: FrameHeight,
            width: FrameWidth,
            modal: true,
            //sresizable: false,
            title: "图片上传",
            buttons: {
                "取消": function() {
                    frame.attr("src", "");
                    $(this).dialog("close");
                },
                "确定": function() {

                    if (window.frames[frameId] && window.frames[frameId].window) {

                        var doc = window.frames[frameId].window.document;

                        var val = $("#ImageValue", doc).val();
                        if (val) {

                            var v = eval("(" + val + ")");
                            if (v) {
                                AppendBefore(v);
                                AppendValue(v);
                            }
                        }
                    }
                    frame.attr("src", "");
                    $(this).dialog("close");
                }
            }
        });
    }

    Init();
}