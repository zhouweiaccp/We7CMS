/*
* FlashShow 1.0
* Copyright (c) 2011 WestEngine http://www.we7.cn
* Date:2011-4-10
* Desc:图片轮显插件
*/

//插件编写规范
(function ($) {

    //名称插件名称
    $.fn.FlashShow = function (options) {

        //设置默认值
        var defaults = {
            focus_width: 200,
            focus_height: 70,
            pics: "",
            links: "",
            texts: "",
            interval_time: 0,
            text_height: 0,
            text_align: "center",
            focusviewerPath: "/Widgets/WidgetCollection/图文类/FlashShow.Default/Swf/focusviewer.swf"
        };

        defaults = $.extend(defaults, options);
        defaults.swf_height = defaults.focus_height + defaults.text_height;

        //对每一个选中的元数进行处理
        this.each(function () {

            var focusviewer = "";
            focusviewer += '<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0" width="' + defaults.focus_width + '" height="' + defaults.swf_height + '">';
            focusviewer += '<param name="allowScriptAccess" value="sameDomain" />';
            focusviewer += '<param name="movie" value="' + defaults.focusviewerPath + '" />';
            focusviewer += '<param name="quality" value="high" />';
            focusviewer += '<param name="bgcolor" value="#F0F0F0">';
            focusviewer += '<param name="menu" value="false">';
            focusviewer += '<param name=wmode value="opaque">';
            focusviewer += '<param name="FlashVars" value="pics=' + defaults.pics + '&links=' + defaults.links + '&texts=' + defaults.texts + '&borderwidth=' + defaults.focus_width + '&borderheight=' + defaults.focus_height + '&textheight=' + defaults.text_height + '">';
            focusviewer += '<embed src="' + defaults.focusviewerPath + '" wmode="opaque" FlashVars="pics=' + defaults.pics + '&links=' + defaults.links + '&texts=' + defaults.texts + '&borderwidth=' + defaults.focus_width + '&borderheight=' + defaults.focus_height + '&textheight=' + defaults.text_height + '" menu="false" bgcolor="#F0F0F0" quality="high" width="' + defaults.focus_width + '" height="' + defaults.swf_height + '" allowScriptAccess="sameDomain" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />';
            focusviewer += '</object>';

            $(this).append(focusviewer);
        });
    }

})(jQuery); 