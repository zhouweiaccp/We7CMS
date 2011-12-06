//附件构造者
function AttachmentBuilder(params) {
    params = params || {};

    var winId = "__modelWin_Uploader_Attachment",
            frameId = "__modelFrame_Uploader_Attachment",
            baseFrameId = "__modelFrame_Uploader_Attachment",
            Obj = this,
            modelWin,
            frame,
            Src = params.Src,
            TriggerElement = params.TriggerElement,
            HiddenValue = params.HiddenValue,
            Body = $("body"),
            DataList = [],
            FrameWidth = params.FrameWidth || 500,
            FrameHeight = params.FrameHeight || 400,
            Container = params.Ct,
            ArticleID = params.ArticleID;

    function Init() {

        if (!modelWin) {
            modelWin = $("<div id='" + winId + "' style='display:none'></div>");
            Body.append(modelWin);
        }

        if (!frame) {
            NewIframe();
        }

        var data = Eval("(" + HiddenValue.val() + ")");

        DataList = data ? data : [];

        InitTrigger();

        InitAttachmentList();

    }
    
    function NewIframe(){
    
         if(frame)frame.remove();

         frameId = baseFrameId + (new Date().getMilliseconds());
         frame = $("<iframe id='" + frameId + "' name='" + frameId + "' style='width:100%; height:100%' frameborder='0' scrolling='auto'></iframe>");
         modelWin.append(frame);
         
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

    function InitTrigger() {
        TriggerElement.click(function() {
            ShowUploader();
            return false;
        });
    }

    function InitAttachmentList() {

        AppendBefore(DataList);
    }

    function AppendBefore(v) {
        var regex = new RegExp("[^\/]*?$");
        for (var i in v) {
            var item = v[i];
            var lbl = item.match(regex);
            var itemct = $("<div style='clear:both;padding-top:2px;'><div style='float:left;'><a href='" + item + "' target='_blank'>" + lbl + "</a></div></div>")
            var btnDel = $("<div style='float:left;padding-left:20px; padding-top:2px;cursor:pointer'><img src='/Admin/images/icon_cancel.gif' /></div>").click(function() {

                RemoveValue($(this).data("item"));
                var ct = $(this).data("ct");
                ct.remove();

            }).data("ct", itemct).data('item', item);
            itemct.append(btnDel);
            Container.append(itemct)
        }
    }

    function AppendValue(v) {
        DataList = DataList.concat(v);
        HiddenValue.val(ToJson(DataList));
    }

    function RemoveValue(v) {
        var index = -1;
        for (var i = 0; i < DataList.length; i++) {
            if (DataList[i] == v) {
                index = i;
                break;
            }
        }
        if (index > -1) {
            DataList.splice(index, 1);
            HiddenValue.val(ToJson(DataList));
        }
    }

    function ToJson(result) {
        var s = "[";
        if (isArray(result)) {
            for (var i in result) {
                s += "'" + result[i] + "',";
            }
            s = s.replace(/,$/ig, "");
        }
        s += "]";
        return s;
    }

    function ShowUploader() {

        if (!$.browser.msie) {
            NewIframe();
        }
        frame.attr("src", Src + "?aid=" + ArticleID + "&t=" + GetTicks());
        
        modelWin.dialog({
            height: FrameHeight,
            width: FrameWidth,
            modal: true,
            //sresizable: false,
            title: "附件上传",
            buttons: {
                "取消": function() {
                    frame.attr("src", "");
                    $(this).dialog("close");
                },
                "确定": function() {

                    if (window.frames[frameId] && window.frames[frameId].window) {

                        var doc = window.frames[frameId].window.document;
                        var val = $("#AttachmentValue", doc).val();
                        if (val) {

                            var v = Eval(val);
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