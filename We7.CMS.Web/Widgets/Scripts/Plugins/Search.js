/*
* SearchKeyWords 1.0
* Author:WestEngine
* Copyright (c) 2011 WestEngine http://www.we7.cn
* Desc：这是一个用户进行关键字插件的插件
*/

(function ($) {

    $.fn.SearchKeyWords = function (options) {

        var defaults = {
            url: "/Search.aspx",
            keywords: "#keywords",
            querybutton: "#querybutton"
        };

        $.extend(defaults, options);

        this.each(function () {
            var ct = $(this);
            ct.find(defaults.querybutton).bind("click", function () {
                var key = ct.find(defaults.keywords).val();
                window.location.href = defaults.url + "?keywords=" + encodeURI(key);
            });
        });
    }

})(jQuery);

alert("---------");