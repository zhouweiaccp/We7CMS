/*
* 用来注册Js脚本 1.0
* Author:WestEngine
* Copyright (c) 2011 WestEngine http://www.we7.cn
* Desc：用来注册插件脚本
*/

(function ($) {

    var registedUrls = [];

    function install(urls) {

        if (!$.isArray(urls)) {
            return;
        }

        if (registedUrls.length == 0) {
            var scripts = $("head script");
            scripts.each(function () {
                var src = $(this).attr("src");
                if (src != null) {
                    registedUrls.push(src);
                }
            });
        }

        for (var i in urls) {
            var include = false;
            for (var j in registedUrls) {
                var regex = new RegExp(urls[i].replace("?", "\\?").replace(".", "\\."), "ig");
                if (regex.test(registedUrls[j])) {
                    include = true;
                    break;
                }
            }
            if (!include) {
                registedUrls.push(urls[i]);
                $("head").append("\<script src='" + urls[i] + "' type='text/javascript'\>\<\/script\>");
            }
        }
    };

    function installPlugins(urls) {

        if (!$.isArray(urls)) {
            return;
        }

        for (var i in urls) {
            urls[i] = "/widgets/Scripts/plugins/" + urls[i];
        }

        install(urls);
    };

    $.extend({
        install: install,
        installPlugins: installPlugins
    });

})(jQuery);