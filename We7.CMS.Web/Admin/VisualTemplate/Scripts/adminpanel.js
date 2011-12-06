
var iframename = "pageframe";     //iframe的名称
var folder;
var file;


$(document).ready(function() {

    folder = Request.parameter("folder");
    file = Request.parameter("file");
    //设置主按钮事件
    $("#pagebglayout").click(function() {
        var adminpanelshow = $("#admin_panle_container").css("display");
        if (adminpanelshow == "none") {
            $("img", this).attr("src", "images/bk-button-focus.png");
            SetBg();
            SetContainer();
            $("#admin_panle_container").slideDown("slow");
        } else {
            $("img", this).attr("src", "images/bk-button.png");
            $("#admin_panle_container").slideUp("slow");
        }
    });
    //绑定透明按钮事件
    $("#btnbgtransparent").click(function() {
        $("#bgcolor").val("transparent").css("background-color", "transparent");
    });

    $('#bgcolor').ColorPicker({
        onSubmit: function(hsb, hex, rgb, el) {
            $(el).val("#" + hex).css("background-color", "#" + hex);
            $(el).ColorPickerHide();
        },
        onBeforeShow: function() {
            $(this).ColorPickerSetColor(this.value);
        }
    })
.bind('keyup', function() {
    $(this).ColorPickerSetColor(this.value);
});

    //背景图

    $("#selectImage").click(function() {
        BrowseServer("bgimageurl");
    });

    //绑定清空

    $("#btnclearbgimage").click(function() {
        $("#bgimageurl").val("");
    });
    //自定义宽度
    $("#customersize").click(function() {
        var checked = $(this).attr("checked");
        if (checked) {
            $("#pagecontainwidth").attr("disabled", "disabled");
            $("#cssize").attr("disabled", "")
        }
        else {
            $("#pagecontainwidth").attr("disabled", "");
            $("#cssize").attr("disabled", "disabled")
        }
    });

    $("#pagebgsave").click(function() {

        var bodyBg = GetBgValue();

        var objBody = $("#pageframe").contents().find("body");

        SetBackground(objBody, bodyBg);
        var cValue = GetContainerValue();
        var container = $("#pageframe").contents().find("body>#pagecontainer").eq(0);
        container.css("width", cValue.width);
        container.css("margin", "0 " + cValue.align);

        var bgattr = "background-color:" + bodyBg.color + ";background-image:url('" + bodyBg.image + "');background-position:" + bodyBg.postion + ";background-repeat:" + bodyBg.repeat + "; background-attachment:" + bodyBg.scroll + ";";
        var ccattr = "width:" + cValue.width + "px;margin:0 " + cValue.align + "";
        SaveBgData(StringUtil.Base64Encode(bgattr), StringUtil.Base64Encode(ccattr));
        $("#admin_panle_container").slideUp("slow");
    });
});


function SaveBgData(bodyattr, ccattr) {
    $.ajax(
         {
             async: true,
             type: "POST",
             url: "/Admin/VisualTemplate/WidgetAjaxService.ashx?action=editbg&file=" + file + "&folder=" + folder + "&bodyattr=" + bodyattr + "&ccattr=" + ccattr,
             dataType: "json",
             data: {},
             error: function() { alert("失败!"); },
             success: function(json) {

                 if (json.Success) {
                     $("img", "#pagebglayout").attr("src", "images/bk-button.png");

                 }
                 else {
                     alert(json.Message);
                 }
             }
         });
}


//打开ckfinder
function BrowseServer(inputId) {
    var finder = new CKFinder();
    finder.basePath = '/scripts/ckfinder/';
    finder.selectActionFunction = SetFileField;
    finder.selectActionData = inputId;
    finder.popup();
}
//设置后的回调函数
function SetFileField(fileUrl, data) {
    //alert(data["selectActionData"]);
    document.getElementById(data["selectActionData"]).value = fileUrl;
}

function SetBackground(obj, bg) {
    $(obj).css("background-color", bg.color);

    $(obj).css("background-image", "url('" + bg.image + "')");

    $(obj).css("background-repeat", bg.repeat);

    $(obj).css("background-attachment", bg.scroll);
    $(obj).css("background-position", bg.postion);

}
//获取背景色
function GetBgValue() {
    var background = {};
    background.image = $('#bgimageurl').val();
    background.color = $("#bgcolor").val();
    background.repeat = $("#bgrepeat").val();
    background.scroll = $("#bgscroll").val();
    background.postion = $("#bgposition").val();

    return background;
}
//获取容器
function GetContainerValue() {
    var c = {};
    var checked = $("#customersize").attr("checked");
    if (checked) {

        c.width = $("#cssize").val();
    }
    else {
        c.width = $("#pagecontainwidth").val();

    }
    c.align = $("#containcentersel").val();


    return c;
}

function SetContainer() {
    var container = $("#pageframe").contents().find("body>#pagecontainer").eq(0);
    var center = "";
    if (container.css("margin-left"))
        center = container.css("margin-left").split(' ')[1];
    var width = container.width();

    $("#pagecontainwidth").val(width);

    if (width != 900 && $("#pagecontainwidth").val() == 900) {
        $("#customersize").attr("checked", "checked");
        $("#cssize").val(width);
        $("#pagecontainwidth").attr("disabled", "disabled");
        $("#cssize").attr("disabled", "");
    }
    else {
        $("#customersize").attr("checked", "");
        $("#pagecontainwidth").attr("disabled", "");
        $("#cssize").attr("disabled", "disabled");
    }

}
function SetBg() {
    var bodyEl = $("#pageframe").contents().find("body").eq(0);
    var color = bodyEl.css("background-color");
    var image = bodyEl.get(0).style.backgroundImage.replace("url(", "").replace(")", "").replace("'", "").replace('"', "").replace('"', ""); //.css("background-image");
    if (image == "none") {
        image = "";
    }
    var scroll = bodyEl.css("background-attachment");

    var repeat = bodyEl.css("background-repeat");


    var position = bodyEl.get(0).style.backgroundPosition; //bodyEl.css("background-position");

    $("#bgcolor").val(color).css("background-color", color);
    $("#bgscroll").val(scroll)
    $("#bgrepeat").val(repeat)
    $("#bgimageurl").val(image);

    $("#bgposition").val(position)
}