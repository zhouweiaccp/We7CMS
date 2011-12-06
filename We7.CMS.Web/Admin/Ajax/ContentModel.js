/// <reference path="jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="jquery/ui1.8.1/jquery.json-2.2.js" />

Array.prototype.contains = function(element) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == element) {
            return true;
        }
    }
    return false;
}
; (function($) {
    $.extend({
        //构造一个Input类型的表单元素
        Input: function(options) {
            //默认的选项
            var defaults = {};
            options = $.extend(defaults, options);
            var props = [];
            props.push("label");
            var el = $("<input />");
            //添加属性和时间
            for (var p in options) {
                if (props.contains(p)) {
                    continue;
                }
                if (typeof (options[p]) == 'function') {
                    //事件
                    el.bind(p, options[p]);
                }
                else {
                    //属性
                    el.attr(p, options[p]);
                }
            }
            return el;
        },
        //构造一个标签
        Label: function(opetions) {
            //默认的选项
            var defaults = {};
            options = $.extend(defaults, options);

            var el = $("<label />");
            //添加属性和时间
            for (var p in options) {
                if (typeof (options[p] == 'function')) {
                    //事件
                    el.bind(p, options[p]);
                }
                else {
                    //属性
                    el.attr(p, options[p]);
                }
            }
            return el;
        },
        //构造一个Submit
        Submit: function(options) {
            //默认的选项
            var defaults = {};
            options = $.extend(defaults, options);

            var el = $('<input type="submit" />');
            //添加属性和时间
            for (var p in options) {
                if (typeof (options[p] == 'function')) {
                    //事件
                    el.bind(p, options[p]);
                }
                else {
                    //属性
                    el.attr(p, options[p]);
                }
            }
            return el;
        },
        //构造一个Button
        Button: function(options) {
            //默认的选项
            var defaults = {};
            options = $.extend(defaults, options);

            var el = $('<input type="button" />');
            //添加属性和时间
            for (var p in options) {
                if (typeof (options[p] == 'function')) {
                    //事件
                    el.bind(p, options[p]);
                }
                else {
                    //属性
                    el.attr(p, options[p]);
                }
            }
            return el;
        },
        //构造一个Radio
        //选项为items数组
        Radio: function(options) {
            //默认的选项
            //TODO::能不能返回一个radio组？
            var defaults = { items: [] };
            options = $.extend(defaults, options);

            var el = $('<input type="radio" />');
            //添加属性和时间
            for (var p in options) {
                if (typeof (options[p]) == 'function') {
                    //事件
                    el.bind(p, options[p]);
                }
                else {
                    //属性
                    el.attr(p, options[p]);
                }
            }
            return el;
        }

    });
})(jQuery);


//FormBuilder


function CreateControl(type,options) {
    if (type == "TextInput") {
        return CreateTextBox(options);
    }
}

function CreateTextBox(settings) {
    var defaults = {label:"..."};
    settings = $.extend(defaults, settings);
    var tr = $('<tr class="p"></tr>');
    var td1 = $("<td></td>");
    var td2 = $("<td></td>");
    var td3 = $('<td ><a href="#" class="edit1">编辑</a></td>');
    var td4 = $('<td><a href="#" onclick="deleteControl()">删除</a><input type="hidden" class="controlvalue" value="'+escape($.toJSON(settings))+'"/></td>');
    td1.append("<label>"+settings["label"]+"</label>");
    td2.append($.Input(settings));

    tr.append(td1);
    tr.append(td2);
    tr.append(td3);
    tr.append(td4);
    return tr;

}
function editbind() {
    $(".edit1:last").click(function() {
        alert("222");
        $(".current").removeClass("current");
        $(this).parents("tr").addClass("current");
    });
}
function deleteControl() {
    $(this).parents().remove();
}
function CreateTextBoxProp(settings) {
    var defaults = {name:"ceshi",label:"..." };
    settings = $.extend(defaults, settings);

    var tabel = '<tr><td>最大字符数:</td><td><input id="formbuilder_props_mapping" type="text"/></td></tr>';
    $("#props").html(tabel);

}

function SetDefaultProps(settings) {
    if (settings["name"] != null) {
        $("#mappingSelect").attr("value", settings["name"]);
    }
}
function Save() {

}