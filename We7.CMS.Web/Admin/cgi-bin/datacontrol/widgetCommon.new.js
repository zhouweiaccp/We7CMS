/* ***************************************************
* RadioSelector/vbRadioSelectorItem
*/
function vbRadioSelectorItem() {
    this.base = vbBaseItem;
    this.base();

    var me = this;
    var _ct;


    this.createHtmlObject = function (id) {
        var _ct = $("<div></div>")[0];
        var name = "r_" + new Date().getTime();
        var data = me.getField().getData();
        var sl = data.split(",");
        for (var i = 0; i < sl.length; i++) {
            var kvp = sl[i].split('|');
            if (kvp.length != 2)
                continue;
            var c = $('<input id="' + kvp[0] + '" style="float:left;" type="radio" ' + (kvp[0] == me.getField().getDefaultValue() ? 'checked' : '') + ' name="' + name + '"></input>')[0];
            $(c).val(kvp[0]);
            _ct.appendChild(c);
            _ct.appendChild($("<label for='" + kvp[0] + "' style='float:left;' >" + kvp[1] + "&nbsp;&nbsp;</label>")[0]);
        }
        return _ct;
    }

    this.getValue = function () {
        return $("input[@checked]", _ct).val();
    }

    this.setValue = function (v) {
        if ($("input[@value=" + v + "]", _ct).length > 0)
            $("input[@value=" + v + "]", _ct).get(0).checked = true;
    }
}

/* ***************************************************
* KeyValueSelector/vbKeyValueSelectorItem
*/
function vbKeyValueSelectorItem() {
    this.base = vbBaseItem;
    this.base();

    var me = this;
    var _input;

    this.createOption = function (txt, value) {
        var op = document.createElement("OPTION");
        op.text = txt;
        op.value = value;
        return op;
    }

    this.createHtmlObject = function (id) {
        _input = $("<SELECT></SELECT>")[0];
        $.ajax({
            url: '/Admin/cgi-bin/controls/KeyValueSelector.aspx',
            async: false,
            data: { cmd: me.getField().getData() },
            success: function (result, op) {
                try {
                    result = eval("(" + result + ")");
                    var dv = me.getField().getDefaultValue();
                    for (var i = 0; i < result.length; i++) {
                        _input[i] = me.createOption(result[i].k, result[i].v);
                        if (dv == result[i].v) {
                            _input[i].selected = true;
                        }
                    }
                }
                catch (e) {
                    alert('数据读取错误!错误信息：' + result);
                }
            },
            error: function () {
                alert('数据读取错误!');
            }
        })
        return _input;
    }

    this.getValue = function () {
        return _input.value;
    }

    this.setValue = function (v) {
        _input.value = v;
    }
}

/* ***************************************************
* OrderFields/vbOrderFieldsItem
*/
function vbOrderFieldsItem() {
    this.base = vbBaseItem;
    this.base();

    var me = this;
    var _viewInput;
    var _valueInput;
    var _bttn;
    var _ct;
    var _data;

    function ParseData(data) {
        var result = {};
        try {
            var kvpList = data.split(",");

            for (var i = 0; i < kvpList.length; i++) {
                kvp = kvpList[i].split("|");
                if (kvp.length > 1)
                    result[kvp[0]] = kvp[1];
                else
                    result[kvp[0]] = kvp[0];
            }
        }
        catch (e) {
        }
        return result;
    }

    function BindValue(value) {
        _valueInput.value = value || "";
        _viewInput.value = "";
        if (_valueInput.value != "") {
            var list = _valueInput.value.split(",");
            for (var i = 0; i < list.length; i++) {
                var kvp = list[i].split("|");
                var k = kvp[0];
                k = _data[k];
                if (k) {
                    var v = kvp[1] ? kvp[1] : "Asc";
                    v = v == "Desc" ? "↓" : "↑";
                    k += v;
                    _viewInput.value += k + ",";
                }
            }
            if (_viewInput.value.length > 0)
                _viewInput.value = _viewInput.value.substr(0, _viewInput.value.length - 1);
        }
    }

    this.createHtmlObject = function (id) {

        _data = ParseData(me.getField().getData());

        _viewInput = document.createElement("INPUT");
        _viewInput.disabled = "disabled";

        try {
            _valueInput = document.createElement("<intput type='hidden' />");
        }
        catch (e) {
            _valueInput = document.createElement("INPUT");
            _valueInput.type = "Hidden";
        }

        _ct = document.createElement("DIV");
        _ct.appendChild(_valueInput);
        _ct.appendChild(_viewInput);
        _bttn = document.createElement("A");
        _bttn.innerHTML = "【选择】";
        _bttn.href = "#";

        _bttn.onclick = function () {
            var l = [];
            var cl = $.trim(_valueInput.value);
            if (cl.length > 0) {
                cl = cl.split(",");
                for (var i = 0; i < cl.length; i++) {
                    var kv = cl[i].split('|');
                    if (kv.length > 1) {
                        l.push({ k: kv[0], v: kv[1].toUpperCase(), o: (cl.length - i) });
                    }
                }
            }
            var data = { items: _data, checkitem: l };
            var result = window.showModalDialog("/Admin/DataControlUI/FieldControl/ShowOrderFields.htm", data, 'scrollbars=no;resizable=no;help=no;status=no;center=1; dialogHeight=250px;dialogwidth=350px;');
            if (result != null)
                BindValue(result);
            return false;
        }
        _ct.appendChild(_bttn);
        BindValue(me.getField().getDefaultValue());
        return _ct;
    }

    this.getValue = function () {
        return _valueInput.value;
    }

    this.setValue = function (v) {
        BindValue(v);
    }
}

/* ***************************************************
* Fields/vbFieldsItem
*/
function vbFieldsItem() {
    this.base = vbBaseItem;
    this.base();

    var me = this;
    var _viewInput;
    var _valueInput;
    var _bttn;
    var _ct;
    var _data;

    function ParseData(data) {
        var result = {};
        try {
            var kvpList = data.split(",");

            for (var i = 0; i < kvpList.length; i++) {
                kvp = kvpList[i].split("|");
                if (kvp.length > 1)
                    result[kvp[0]] = kvp[1];
                else
                    result[kvp[0]] = kvp[0];
            }
        }
        catch (e) {
        }
        return result;
    }

    function BindData(v) {
        _valueInput.value = v || "";
        _viewInput.value = "";
        _viewInput.disabled = "disabled";

        if (_valueInput.value != "") {
            var list = _valueInput.value.split(",");
            for (var i = 0; i < list.length; i++) {
                _viewInput.value += _data[list[i]] ? (_data[list[i]] + ",") : "";
            }
            if (_viewInput.value.length > 0)
                _viewInput.value = _viewInput.value.substr(0, _viewInput.value.length - 1);
        }
    }

    this.createHtmlObject = function (id) {

        _data = ParseData(me.getField().getData());

        _viewInput = document.createElement("INPUT");
        _viewInput.disabled = true;

        try {
            _valueInput = document.createElement("<intput type='hidden' />");
        }
        catch (e) {
            _valueInput = document.createElement("INPUT");
            _valueInput.type = "Hidden";
        }

        _ct = document.createElement("DIV");
        _ct.appendChild(_valueInput);
        _ct.appendChild(_viewInput);
        _bttn = document.createElement("A");
        _bttn.innerHTML = "【选择】";
        _bttn.href = "#";

        _bttn.onclick = function () {
            var vl = $.trim(_valueInput.value);
            if (vl.length > 0)
                vl = vl.split(',');
            else
                vl = [];
            var data = { items: _data, checkitem: vl };
            var result = window.showModalDialog("FieldControl/ShowFields.htm", data, 'scrollbars=no;resizable=no;help=no;status=no;center=1; dialogHeight=200px;dialogwidth=300px;');
            if (result != null) {
                BindData(result);
            }
            return false;
        }
        _ct.appendChild(_bttn);

        BindData(me.getField().getDefaultValue());

        return _ct;
    }

    this.getValue = function () {
        return _valueInput.value;
    }

    this.setValue = function (v) {
        BindData(v);
    }
}

/* ***************************************************
* Tags/vbTagsItem
*/
function vbTagsItem() {
    this.base = vbBaseItem;
    this.base();

    var me = this;
    var _valueInput;
    var _bttn;
    var _ct;
    var _data;

    function BindData(v) {
        _valueInput.value = v || "";
    }

    this.createHtmlObject = function (id) {


        _valueInput = document.createElement("INPUT");
        _valueInput.disabled = true;

        _ct = document.createElement("DIV");
        _ct.appendChild(_valueInput);
        _bttn = document.createElement("A");
        _bttn.innerHTML = "【选择】";
        _bttn.href = "#";

        _bttn.onclick = function () {
            var vl = $.trim(_valueInput.value);
            var result = window.showModalDialog("/Admin/DataControlUI/FieldControl/TagsSelector.aspx", vl, 'scrollbars=no;resizable=no;help=no;status=no;center=1; dialogHeight=200px;dialogwidth=300px;');
            if (result != null) {
                BindData(result);
            }
            return false;
        }
        _ct.appendChild(_bttn);

        BindData(me.getField().getDefaultValue());

        return _ct;
    }

    this.getValue = function () {

        return _valueInput.value;
    }

    this.setValue = function (v) {
        BindData(v);
    }
}

/* ***************************************************
* Boolean
*/
function vbBooleanItem() {
    this.base = vbBaseItem;
    this.base();

    var me = this;
    var _input;

    this.createHtmlObject = function (id) {
        _input = document.createElement("INPUT");
        _input.type = "checkbox";
        if (me.getField().getValue() == "true") {
            _input.checked = true;
        }

        return _input;
    }

    this.getValue = function () {
        return _input.checked;
    }

    this.setValue = function (v) {
        if (typeof (v) == "string") {
            if (v.toLowerCase() == "true") {
                _input.checked = true;
                return;
            }
            else if (v.toLowerCase() == "false") {
                _input.checked = false;
                return;
            }
        }
        if (v == "-1" || eval(v)) {
            _input.checked = true;
        }
        else
            _input.checked = false;
    }
}

/* ***************************************************
* vbStyleSelectorItem
*/
function vbStyleSelectorItem() {
    this.base = vbBaseItem;
    this.base();
    this.input;

    var me = this;
    var _input;
    var _div;
    var _a;

    this.createOption = function (txt, value) {
        var op = document.createElement("OPTION");
        op.text = txt;
        op.value = value;
        return op;
    }
    this.refresh = function (id) {
        _input = this.el;
        var dc = arguments[1];
        _input.data = dc;
        _input.options.length = 0;
        for (var i = 0; i < dc.styles.length; i++) {
            _input.options[i] = this.createOption(dc.styles[i].value, dc.styles[i].key);
        }
        if (me.getField().getDefaultValue()) {
            _input.value = me.getField().getDefaultValue();
        }
    }

    this.createHtmlObject = function (id) {
        var dc = arguments[1];

        _div = document.createElement("DIV");
        _input = document.createElement("SELECT");
        var selected = false;
        _input.data = dc;

        for (var i = 0; i < dc.styles.length; i++) {
            _input.options[i] = this.createOption(dc.styles[i].value, dc.styles[i].key);
        }
        if (me.getField().getDefaultValue()) {
            _input.value = me.getField().getDefaultValue();
        }

        _input.id = id;
        _div.appendChild(_input);
        this.el = _input;

        _a = document.createElement("A");
        _a.innerHTML = "【修改】";
        var obj = this;
        _a.onclick = function () {
            var result = window.showModalDialog("DataControlCssEditor.aspx?cmd=edit&ctr=" + _input.data.fileName + "&style=" + _input.value + "&gp=" + dc.gp, "修改样式", 'scrollbars=no;resizable=no;help=no;status=no;center=1; dialogHeight=490px;dialogwidth=700px;');
            if (typeof result == "object") {
                i = _input.options.length;
                _input.options[i] = me.createOption(result.value, result.key);
            }
            return false;
        }
        _a.href = "#";
        _div.appendChild(_a);

        return _div;
    }

    this.getValue = function () {
        return _input.value;
    }

    this.setValue = function (v) {
        var f = false;
        for (var i = 0; i < _input.options.length; i++) {
            if (v.toLowerCase() == _input.options[i].value) {
                f = true;
                break;
            }
        }
        if (!f) {
            var l = _input.options.length;
            _input.options[l] = this.createOption(v, v);
        }
        _input.value = v;
    }
}

/* ***************************************************
* vbStyleSelectItem
*/
function vbStyleSelectItem() {
    this.base = vbBaseItem;
    this.base();
    this.input;

    var me = this;
    var _input;
    var _div;
    var _a;

    this.createOption = function (txt, value) {
        var op = document.createElement("OPTION");
        op.text = txt;
        op.value = value;
        return op;
    }
    this.refresh = function (id) {
        _input = this.el;
        var dc = arguments[1];
        _input.data = dc;
        _input.options.length = 0;
        if (dc.styles) {
            for (var i = 0; i < dc.styles.length; i++) {
                _input.options[i] = this.createOption(dc.styles[i].value, dc.styles[i].key);
            }
        }
        if (me.getField().getDefaultValue() && me.getField().getDefaultValue() != "") {
            _input.value = me.getField().getDefaultValue();
        }
    }

    this.createHtmlObject = function (id) {
        var dc = arguments[1];

        _div = document.createElement("DIV");
        _input = document.createElement("SELECT");
        var selected = false;
        _input.data = dc;

        if (dc.styles) {
            for (var i = 0; i < dc.styles.length; i++) {
                _input.options[i] = this.createOption(dc.styles[i].value, dc.styles[i].key);
            }
        }
        if (me.getField().getDefaultValue() && me.getField().getDefaultValue() != "") {
            _input.value = me.getField().getDefaultValue();
        }

        _input.id = id;
        _div.appendChild(_input);
        this.el = _input;

        _a = document.createElement("A");
        _a.innerHTML = "【编辑样式】";
        var obj = this;
        _a.onclick = function () {
            var result = window.showModalDialog("DataControlCssEditorEx.aspx?cmd=edit&ctr=" + escape(dc.control) + "&template=" + dc.template + "&style=" + _input.value + "&gp=" + dc.gp, "修改样式", 'scrollbars=no;resizable=no;help=no;status=no;center=1; dialogHeight=520px;dialogwidth=800px;');
            if (typeof result == "object") {
                i = _input.options.length;
                for (var o in _input.options) {
                    if (_input.options[o].value == result.value) {
                        _input.options[o].selected = true;
                        return false;
                    }
                }
                _input.options[i] = me.createOption(result.value, result.key);
                _input.options[i].selected = true;
            }
            return false;
        }
        _a.href = "#";
        _div.appendChild(_a);

        return _div;
    }

    this.getValue = function () {
        return _input.value;
    }

    this.setValue = function (v) {
        var f = false;
        for (var i = 0; i < _input.options.length; i++) {
            if (v.toLowerCase() == _input.options[i].value) {
                f = true;
                break;
            }
        }
        if (!f) {
            var l = _input.options.length;
            _input.options[l] = this.createOption(v, v);
        }
        _input.value = v;
    }
}

/* ***************************************************
* vbTargetSelectorItem
*/
function vbTargetSelectorItem() {
    this.base = vbBaseItem;
    this.base();

    var me = this;
    var _input;

    this.createOption = function (txt, value) {
        var op = document.createElement("OPTION");
        op.text = txt;
        op.value = value;
        return op;
    }

    this.createHtmlObject = function (id) {
        _input = document.createElement("SELECT");
        _input.add(this.createOption("弹出新窗口", "_blank"));
        _input.add(this.createOption("取代本窗口", "_self"));
        _input.add(this.createOption("取代父窗口", "_parent"));
        _input.add(this.createOption("取代顶级窗口", "_top"));
        _input.value = me.getField().getValue();
        _input.id = id;
        return _input;
    }

    this.getValue = function () {
        return _input.value;
    }

    this.setValue = function (v) {

        _input.value = v;
    }
}

/* ***************************************************
* String
*/
function vbTextItem() {
    this.base = vbBaseItem;
    this.base();

    var me = this;
    var _input;

    this.createHtmlObject = function (id) {
        _input = document.createElement("INPUT");
        _input.size = 30;
        _input.id = id;
        return _input;
    }

    this.getValue = function () {
        var v = CharFilter(_input.value);
        return v;
    }

    this.setValue = function (v) {
        v = CharConvert(v);
        _input.value = v;
    }
}

/* ***************************************************
* Channel Item
*/
function vbChannelItem() {
    this.base = vbBaseItem;
    this.base();

    var me = this;
    var _input;
    var _href;
    var _div;
    this.createHtmlObject = function (id) {
        _div = document.createElement("DIV");
        _br = document.createElement("br");
        _input = document.createElement("INPUT");
        _input.size = 30;
        _input.id = id;
        _href = document.createElement("A");
        _href.innerHTML = "<img src='/admin/images/icon_column.gif'>选择栏目";
        _href.href = "javascript:__doSelectChannels('" + me.getUniqueID() + "');";

        _div.appendChild(_input);
        _div.appendChild(_br);
        _div.appendChild(_href);
        return _div;
    }

    this.getValue = function () {
        return _input.value;
    }

    this.setValue = function (v) {
        _input.value = v;
    }
}

/* ***************************************************
* CustomImage Item
*/
function vbCustomImage() {
    this.base = vbBaseItem;
    this.base();
    this.input;

    var me = this;
    var _input;
    var _div;
    var _a;

    this.createHtmlObject = function (id) {
        _div = document.createElement("DIV");

        _input = document.createElement("INPUT");
        _input.size = 15;
        _input.id = id;

        _div.appendChild(_input);

        _a = document.createElement("A");
        _a.innerHTML = "【选择图片】";
        _a.onclick = function () {
            var finder = new CKFinder();
            finder.basePath = '/scripts/ckfinder/';
            finder.selectActionFunction = SetFileField;
            finder.selectActionData = id;
            finder.popup();
        }
        _a.href = "#";
        _div.appendChild(_a);

        return _div;
    }
    
    this.getValue = function () {
        var v = CharFilter(_input.value);
        return v;
    }

    this.setValue = function (v) {
        v = CharConvert(v);
        _input.value = v;
    }
}

/* ***************************************************
* ColorSelector Item
*/
function vbColorSelectorItem() {
    this.base = vbBaseItem;
    this.base();
    this.input;

    var me = this;
    var _input;
    var _div;
    var _a;
    var _colorImg;
    var _colorA;

    this.createHtmlObject = function (id) {
        _div = document.createElement("DIV");

        _input = document.createElement("INPUT");
        _input.size = 15;
        _input.id = id;

        _colorImg = document.createElement("IMG");
        _colorImg.src = "/Admin/images/icon_personall.gif";

        _colorA = document.createElement("A");
        _colorA.href = "javascript:void(0);";
        _colorA.title = "←拾色器";
        _colorA.appendChild(_colorImg);

        _div.appendChild(_input);
        _div.appendChild(_colorA);

        $(_colorA).click(function () {
            $(_input).focus();
            $(_input).click();
        });

        $(_input).ColorPicker({
            onSubmit: function (hsb, hex, rgb, el) {
                $(el).val("#" + hex).css("background-color", "#" + hex);
                $(el).ColorPickerHide();
            },
            onBeforeShow: function () {
                $(this).ColorPickerSetColor(this.value);
            }
        }).bind('keyup', function () {
            $(this).ColorPickerSetColor(this.value);
        });

        return _div;
    }
    
    this.getValue = function () {
        var v = CharFilter(_input.value);
        return v;
    }

    this.setValue = function (v) {
        v = CharConvert(v);
        _input.value = v;
    }
}

var __CallerID = -1;
function __doSelectChannels(uid) {
    __CallerID = uid;
    //showDialog("ChannelList.aspx", __OnChannelListCallback);
    weShowModelDialog("/Admin/ChannelList.aspx", __OnChannelListCallback);
}

function __OnChannelListCallback(v, t) {
    if (v) {
        var item = $("#" + __CallerID);
        if (item) {
            item.val(v);
        }
    }
    __CallerID = -1;
}

/* *******************************************************
* Number
*/
function vbNumberItem() {
    this.base = vbBaseItem;
    this.base();

    var me = this;
    var _input;

    this.createHtmlObject = function (id) {
        _input = document.createElement("INPUT");
        _input.size = 10;
        _input.id = id;
        return _input;
    }

    this.getValue = function () {
        if (_input.value == "")
            return "0";
        else
            return _input.value;
    }

    this.setValue = function (v) {
        _input.value = v;
    }


    this.validate = function () {
        var v = me.getValue();
        var reg = /^\+?[0-9][0-9]*$/;
        return v.match(reg);
    }
}

/* ***************************************************
* PhotoSelect
*/
function vbPhotoSelectorItem() {
    this.base = vbBaseItem;
    this.base();

    var me = this;
    var _input;

    var xmlUrl = "/Config/thumbnail.xml";
    this.createHtmlObject = function (id) {
        _input = document.createElement("SELECT");

        $.ajax({
            url: xmlUrl,
            async: false,
            success: function (xmldoc, status) {
                var updates = xmldoc.getElementsByTagName("configuration");
                if (updates && updates.length > 0) {
                    updates = updates[0];
                    for (var i = 0; i < updates.childNodes.length; i++) {
                        var attrs = updates.childNodes[i].attributes;
                        if (attrs != null) {
                            var strname;
                            var strvalue;
                            for (var j = 0; j < attrs.length; j++) {
                                if (attrs[j].name == "value") strname = attrs[j].value;
                                if (attrs[j].name == "identityChar") strvalue = attrs[j].value;

                            }
                            var op = document.createElement("OPTION");
                            op.text = strname;
                            op.value = strvalue;
                            _input[_input.length] = op;
                        }

                    }
                }
                _input.value = me.getField().getValue();
                _input.id = id;
            },
            error: function (xh, status, error) {
                alert(status);
            }
        });
        return _input;
    }

    this.getValue = function () {
        alert(_input.value);
        return _input.value;
    }

    this.setValue = function (v) {
        _input.value = v;
    }
}

function SetFileField(fileUrl, data) {
    document.getElementById(data["selectActionData"]).value = fileUrl;
}

function CharFilter(s) {
    s = s.replace(/</ig, '&lt;').replace(/>/ig, '&gt;').replace(/"/ig, '&quot;').replace(/\s/ig, '&nbsp;').replace(/'/ig, '&acute;').replace(/&/ig, '&amp;');
    return s;
}

function CharConvert(s) {
    s = s.replace(/&amp;/ig, '&').replace(/&lt;/ig, '<').replace(/&gt;/ig, '>').replace(/&quot;/ig, '"').replace(/&nbsp;/ig, " ").replace(/&acute;/ig, "'");
    return s;
}

/* *******************************************************
* Add Item to Builder factory
*给构造器添加各种可构造类型
*/
//
function InitialBuilder(BUILDER) {
    BUILDER.addCreator("String", "vbTextItem");
    BUILDER.addCreator("Boolean", "vbBooleanItem");
    BUILDER.addCreator("Channel", "vbChannelItem");
    BUILDER.addCreator("Number", "vbNumberItem");
    BUILDER.addCreator("CssFile", "vbCssFileItem");
    BUILDER.addCreator("StyleSelector", "vbStyleSelectorItem");
    BUILDER.addCreator("StyleSelect", "vbStyleSelectItem");
    BUILDER.addCreator("TargetSelector", "vbTargetSelectorItem");
    BUILDER.addCreator("PhotoSelect", "vbPhotoSelectorItem");
    BUILDER.addCreator("Fields", "vbFieldsItem");
    BUILDER.addCreator("OrderFields", "vbOrderFieldsItem");
    BUILDER.addCreator("KeyValueSelector", "vbKeyValueSelectorItem");
    BUILDER.addCreator("RadioSelector", "vbRadioSelectorItem");
    BUILDER.addCreator("Tags", "vbTagsItem");
    BUILDER.addCreator("CustomImage", "vbCustomImage");
    BUILDER.addCreator("ColorSelector", "vbColorSelectorItem");
};
//初始化构造器
InitialBuilder(BUILDER);

//创建构造器
function CreateBuilder() {
    var BUILDER = new vbFactory();
    BUILDER.Event = {};
    BUILDER.Event.add = function (cmd, fn) {
        if (BUILDER.Event[cmd] == null) {
            BUILDER.Event[cmd] = new Array();
        }
        BUILDER.Event[cmd].push(fn);
    }
    BUILDER.Event.dispatch = function (cmd) {
        var o = {}
        for (var i = 1; i < arguments.length; i++) {
            o[i - 1] = arguments[i];
        }
        if (BUILDER.Event[cmd] != null) {
            for (var i = 0; i < BUILDER.Event[cmd].length; i++)
                BUILDER.Event[cmd][i](o);
        }
    }
    InitialBuilder(BUILDER);
    return BUILDER;
}



