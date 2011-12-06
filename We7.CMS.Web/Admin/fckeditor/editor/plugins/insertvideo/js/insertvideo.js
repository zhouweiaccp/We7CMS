var dialog = window.parent;
var oEditor = dialog.InnerDialogLoaded();
var FCKConfig = oEditor.FCKConfig;
var FCKLang = oEditor.FCKLang;
window.onload = function() {
    // First of all, translate the dialog box texts
    oEditor.FCKLanguageManager.TranslatePage(document);


    // Show the "Ok" button.
    dialog.SetOkButton(true);

    // Select text field on load.
    SelectField('txtUrl');

}
function SetUrl(url, width, height, alt) {
    if (sActualBrowser == 'Link') {

    }
    else {
        GetE('txtUrl').value = url;

        if (alt)
            GetE('txtAlt').value = alt;
    }
}
function Validate(s) {
    var patrn = /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\’:+!]*([^<>\"\"])*$/;
    if (!patrn.exec(s)) {
        return false;
    }
    else {
        return true;
    }
}

function BrowseServer() {
    OpenServerBrowser(
		'Flash',
		FCKConfig.FlashBrowserURL,
		FCKConfig.FlashBrowserWindowWidth,
		FCKConfig.FlashBrowserWindowHeight);
}

function OpenServerBrowser(type, url, width, height) {
    sActualBrowser = type;
    OpenFileBrowser(url, width, height);
}

var sActualBrowser;

function Ok() {
    var sMusicUrl = GetE('txtUrl').value;
    var link = GetE("videoLink").value;
    var height = GetE("height").value;
    var width = GetE("weight").value;
    var flashvars;
    flashvars = "file=" + sMusicUrl;

    var str = self.location.pathname.split("/");
    if (sMusicUrl.length == 0) {
        alert("路径不能为空");
        return false;
    }
    if (link != "") {
        if (!Validate(link)) {
            alert("视频链接错误");
            return false;
        }
        flashvars = flashvars + "&link=" + link;
    }
    flashvars = flashvars + "&autostart=" + GetE("autostart").value;
    var sMusicHtml; //这里mp3player.swf请使用你的mp3player.swf的绝对路径
    sMusicHtml = '<embed src="' + str[2] + '/editor/plugins/insertvideo/player/jwplayer.swf" width="' + width + '" height="' + height + '" bgcolor="#ffffff" allowscriptaccess="always" allowfullscreen="true" flashvars="' + flashvars + '"/>';

    oEditor.FCK.InsertHtml(sMusicHtml);
    return true;
} 