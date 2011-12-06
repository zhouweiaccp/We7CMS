
/*文章缩略图 */
//缩略图大小规格重设
function ImageSizeDropDownList_onchange(ddl, imgObjct, sizeHidden) {
    var width = 0;
    var height = 0;
    var item = ddl.options[ddl.selectedIndex].value;
    if (item == '0') {
        var val = "";
        var invalid = true;
        while (invalid) {
            val = prompt("请输入您想要的图片宽度与高度的数值，中间用*号隔开，如 210*110", val);
            if (!val)
                invalid = false;
            else {
                if (check(val)) {
                    CallServer(ddl.id + ',' + imgObjct + ',' + val, 'context');

                    ddl.options[ddl.selectedIndex].value = val;
                    ddl.options[ddl.selectedIndex].text = "自定义：" + val;
                    var option = new Option("自定义：手动输入", "0");
                    ddl.options.add(option);
                    ddl.options.selectValue = val;
                    item = val;
                    invalid = false;
                }
            }
        }
    }

    if (list.length > 1) {
        width = list[0];
        height = list[1];
    }

    var imgObj = document.getElementById(imgObjct);
    imgObj.width = width;
    imgObj.height = width;

    document.getElementById(sizeHidden).value = item;
}

//客户端回调：获取服务器数据
function ReceiveServerData(receivedStr, context) {
    var paramList = receivedStr.split(',');
}

function check(value) {
    var list = value.split('*');
    if (list.length > 1) {
        width = list[0];
        height = list[1];
    }
    if (list.length > 1 && IsNumeric(list[0]) && IsNumeric(list[1])) {
        return true;
    }
    else {
        alert('请输入的图片宽高数值不合法，请重新输入，再试！');
        return false;
    }
}

function IsNumeric(port) {
    var pattern = /^\d+$/;
    if (!pattern.test(port))
        return false;
    else
        return true;
}

//上传图片验证
function articleImageCheck(userCtrlID, existPath) {
    var existName = true;
    var file = document.getElementById(userCtrlID + "_ImageFileUpload");
    if (!file.value) {
        alert("您还没有选择图片！");
        file.focus();
        return false;
    }

    if (existPath == '1') {
        var messages = "您确定覆盖原来的图片？"
        var ifConfirm = window.confirm(messages);
        if (ifConfirm) {
            $("#reName").val("edit");
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return true;
    }
    return true;
}

/*文章属性 */
//文章类型数值变化事件
function ActicleTypeDropDownList_onchange(ddl, userCtrlID) {
    var menuElements = document.getElementsByTagName("li");
    for (var i = 0; i < menuElements.length; i++) {
        menuElements[i].style.display = "";
    }
    var linkSpan = userCtrlID + "_linkSpan";
    document.getElementById(linkSpan).style.display = "none";
    if (document.getElementById("bodydiv")) {
        document.getElementById("bodydiv").style.display = "";
    }

    var item = ddl.options[ddl.selectedIndex].value;
    switch (item) {
        case "1": //原创文章
            break;
        case "8": //引用文章
            if (document.getElementById("tab2"))
                document.getElementById("tab2").style.display = "none";
            if (document.getElementById("tab3"))
                document.getElementById("tab3").style.display = "none";
            if (document.getElementById("tab5"))
                document.getElementById("tab5").style.display = "none";
            if (document.getElementById("tab6"))
                document.getElementById("tab6").style.display = "none";
            if (document.getElementById("tab7"))
                document.getElementById("tab7").style.display = "none";
            if (document.getElementById("bodydiv"))
                document.getElementById("bodydiv").style.display = "none";
            if (document.getElementById(linkSpan))
                document.getElementById(linkSpan).style.display = "";
            break;
        default:
            ;
    }

    contentEdited(true);
}

//文章属性 输入验证
function articleOptionCheck(userCtrlID) {

    var ddl = document.getElementById(userCtrlID + "_ActicleTypeDropDownList");
    var item = ddl.options[ddl.selectedIndex].value;
    if (!item || item == '') {
        alert("您还没有选择文章类型！");
        ddl.focus();
        return false;
    }
    else if (item == "1") {
        if (document.getElementById(userCtrlID + "_ContentUrlTextBox").value == '') {
            alert("源文件URL地址不能为空！");
            document.getElementById(userCtrlID + "_ContentUrlTextBox").focus();
            return false;
        }
    }

    if (document.getElementById(userCtrlID + "_TitleTextBox").value == '') {
        alert("文章标题不能为空！");
        document.getElementById(userCtrlID + "_TitleTextBox").focus();
        return false;
    }

    var p = /^[0-9]*$/;
    if (!p.exec(document.getElementById(userCtrlID + "_IndexTextBox").value)) {
        alert("排序号必须为数字！");
        document.getElementById(userCtrlID + "_IndexTextBox").focus();
        return false;
    }
}

//栏目属性 输入验证
function channelBasicCheck(userCtrlID) {

    if (document.getElementById(userCtrlID + "_NameTextBox").value == '') {
        alert("栏目标题不能为空！");
        document.getElementById(userCtrlID + "_NameTextBox").focus();
        return false;
    }

    if(document.getElementById(userCtrlID + "_ChannelNameTextBox"))
    {
        if (document.getElementById(userCtrlID + "_ChannelNameTextBox").value == '') {
            alert("栏目URL不能为空！");
            document.getElementById(userCtrlID + "_ChannelNameTextBox").focus();
            return false;
        }
        var p = /^[a-zA-Z0-9_-\u4e00-\u9fa5]+$/;
        if (!p.exec(document.getElementById(userCtrlID + "_ChannelNameTextBox").value)) {
            alert("栏目URL只能为英文或拼音！");
            document.getElementById(userCtrlID + "_ChannelNameTextBox").focus();
            return false;
        }
    }
    return true;
}

//栏目属性：自动填写channelname
function autoFillUrlName(userCtrlID) {
    var nameText = document.getElementById(userCtrlID + "_NameTextBox");
    var urlName = document.getElementById(userCtrlID + "_ChannelNameTextBox");
    if (urlName && nameText.value != '' && urlName.value == '') {
//        var obj_pinyin = new pinyin();
        // urlName.value = obj_pinyin.toPinyinFirst(nameText.value);
        ajaxGetPingying(nameText.value, urlName);
    }
}

//Ajax获取字串的首字母拼音
function ajaxGetPingying(str,ctrl) {
    $.ajax(
    {
        type: "get",
        url: "/admin/ajax/Pinying.ashx",
        data: "str=" + encodeURIComponent(str),
        success: function (msg) {
            ctrl.value = msg;
        },
        failure: function (msg, resp, status) {
            alert(msg);
            alert(resp);
            alert(status);
        }
    });
}

//栏目类型变化
function autoChangeTypeList(userCtrlID) {
    var ddl = document.getElementById(userCtrlID + "_TypeDropDownList");
    var item = ddl.options[ddl.selectedIndex].value;
     $("#modelRow").css("display","none");
     $("#ReturnUrlRow").css("display","");
//     debugger; 
    switch (item) {
    
        case "0":
            $("#modelRow").css("display","");
            $("#ReturnUrlRow").css("display","none");
            $("#typeMessage").html("可以选择不同内容模型，节点可以发布信息。");
            break;
            
        case "1": //专题
            $("#ReturnUrlRow").css("display","none");
            $("#typeMessage").html("专题信息是从其他栏目发布过来。");
            break;
            
        case "3": //跳转型
            $("#urlLabel").html("跳转地址");
            $("#typeMessage").html("访问时直接跳转到下面URL地址。");
            break;
            
        case "4": //Rss源
            $("#urlLabel").html("Rss源地址");
            $("#typeMessage").html("输入RSS地址，可以远程调用RSS列表。");
            break;
            
         case "5": //空节点
             $("#typeMessage").html("此节点不允许发布信息，将自动跳转到子节点。");
             $("#ReturnUrlRow").css("display","none");
             break;
             
        default:

    }
}

//目录名称
function autoChangeFolderName(userCtrlID) {
    var urlName = document.getElementById(userCtrlID + "_ChannelNameTextBox");
    var pathName = document.getElementById(userCtrlID + "_ChannelNameLabel");
    if (urlName.value != '') {
        pathName.innerHTML = urlName.value;
    }
}

//审核流程
function visibleFlowList(userCtrlID) {
    //debugger;
    var ddl = document.getElementById(userCtrlID + "_ProcessDropDownList");
    var item = ddl.selectedIndex;
    if (item == 0) {
        document.getElementById(userCtrlID + "_ProcessLayerDropDownlist").style.display = "none";
        document.getElementById(userCtrlID + "_ProcessEndingDropDownList").style.display = "none";
    }
    else {
        document.getElementById(userCtrlID + "_ProcessLayerDropDownlist").style.display = "";
        document.getElementById(userCtrlID + "_ProcessEndingDropDownList").style.display = "";
    }
}

//安全级别
function securityLevelChange(levelListID, ipPanelID) {
    var levelList = document.getElementById(levelListID);
    var ipPanel = document.getElementById(ipPanelID);

    if (levelList.value == "3")
        ipPanel.style.display = "";
    else
        ipPanel.style.display = "none";
}


//页面离开提醒保存
var g_blnCheckUnload = false;
function RunOnBeforeUnload() {
    if (g_blnCheckUnload) {
        window.event.returnValue = '您有修改内容尚未保存，离开页面修改将会丢失！您可点击取消，保存后再试。';
    }
}

function contentEdited(isEdit) {
    g_blnCheckUnload = isEdit;
}

function autoSetTitleValue(titleTextBox, editorID) {
    if (titleTextBox.value == "") {
        var editor1 = document.getElementById(editorID);
        //       debugger;
        // retrieving the content of CuteEditor as HTML 
        if (editor1.value) {
            var content = editor1.value();
            //        var content = editor1.innerText;
            if (content) {
                content = removeHTML(content);
                content = getFirstSeg(content);
                titleTextBox.value = content;
            }
        }
    }
}

function removeHTML(content) {
    var regEx = /<[^>]*>/g;
    var regBlank = /&nbsp;/g;
    content = content.replace(/(^\s*)|(\s*$)/g, "");

    content = content.replace(regBlank, " ");
    content = content.replace(regEx, "");
    return content;
}

function getFirstSeg(content) {
    var segs = content.split(/\s|\(|\)|\（|\）|\.|\。|\,|\，/g);
    for (i = 0; i < segs.length; i++) {
        if (segs[i].length > 2) return segs[i];
    }
}

/*
function bypassCheck() {  
g_blnCheckUnload  = false;
TempSave(document.getElementById('Editor_Edit_txbTitle'),document.getElementById('Editor_Edit_ftbBody'));  
}

function TempSave(title,body)
{
oPersistDiv.setAttribute("sPersistContent",body.vaue);
oPersistDiv.setAttribute("sPersistTitle",title.value);
oPersistDiv.save("oXMLStore");
}

function Restore(editor)
{
oPersistDiv.load("oXMLStore");
//FTB_InsertText('Editor_Edit_ftbBody',oPersistDiv.getAttribute("sPersistContent"));
var content=document.getElementById('Editor_Edit_ftbBody');
content.value=oPersistDiv.getAttribute("sPersistContent");
var title=document.getElementById('Editor_Edit_txbTitle');
title.value=oPersistDiv.getAttribute("sPersistTitle");
}

function ClearTemp()
{
oPersistDiv.setAttribute("sPersistValue","");
oPersistDiv.save("oXMLStore");
}

*/
//提取cookies值，恢复用户上次使用状态
function loadSettingFromCookies(userCtrlID) {
    var obj = document.getElementById(userCtrlID + "_SizesDropDownList");
    if (obj) {
        var imageSizeDropDownList = getCookie("ImageSizeDropDownList");
        if (imageSizeDropDownList) {
            obj.selectedIndex = imageSizeDropDownList;
        }
    }
    obj = document.getElementById(userCtrlID + "_CutTypeDropDownList");
    if (obj) {
        var CutTypeDropDownList = getCookie("CutTypeDropDownList");
        if (CutTypeDropDownList) {
            obj.selectedIndex = CutTypeDropDownList;
        }
    }
}

//保存控件状态值到cookiess
function saveImageSizeToCookies(obj) {
    var expires = new Date();
    expires.setTime(expires.getTime() + 3 * 30 * 24 * 60 * 60 * 1000);

    if (obj) {
        if (obj.selectedIndex == -1)
            deleteCookie("ImageSizeDropDownList");
        else
            setCookie("ImageSizeDropDownList", obj.selectedIndex, expires, null, null, false);
    }
}


function saveImageCutToCookies(obj) {
    var expires = new Date();
    expires.setTime(expires.getTime() + 3 * 30 * 24 * 60 * 60 * 1000);

    if (obj) {
        if (obj.selectedIndex == -1)
            deleteCookie("CutTypeDropDownList");
        else
            setCookie("CutTypeDropDownList", obj.selectedIndex, expires, null, null, false);
    }
}


