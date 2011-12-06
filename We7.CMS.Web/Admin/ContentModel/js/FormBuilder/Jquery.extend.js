; (function($) {

    $.defineTag = function(tag) {
        $[tag.toUpperCase()] = function() {
            var s = [tag];
            for (var i = 0; i < arguments.length; i++) {
                s[s.length] = arguments[i];
            }
            return $.create.apply(null, s);
        }
    };

    (function() {
        var tags = [
		'a', 'br', 'button', 'canvas', 'div', 'fieldset', 'form',
		'h1', 'h2', 'h3', 'hr', 'img', 'input', 'label', 'legend',
		'li', 'ol', 'optgroup', 'option', 'p', 'pre', 'select',
		'span', 'strong', 'table', 'tbody', 'td', 'textarea',
		'tfoot', 'th', 'thead', 'tr', 'tt', 'ul'];
        for (var i = tags.length - 1; i >= 0; i--) {
            $.defineTag(tags[i]);
        }
    })();
    //第一个参数为:tagName,第二参数:属性,第三个参数:innerText
    $.create = function(/*tag, arguments*/) {
    
        var fix = { 'class': 'className', 'Class': 'className' ,'for':'labelfor'};
        var e;
        var tag = arguments[0];
        var attrs = {}
        if (arguments.length >= 2)
            attrs = arguments[1];
        e = document.createElement(tag);
        for (var attr in attrs) {
            var a = fix[attr] || attr;
            e[a] = attrs[attr];
        }
        for (var i = 2; i < arguments.length; i++) {
            var arg = arguments[i];
            if (arg == null) continue;
            if (arg.constructor != Array) append(arg);
            else for (var j = 0; j < arg.length; j++)
                append(arg[j]);
        }

        function append(arg) {
            if (arg == null) return;
            var c = arg.constructor;
            switch (typeof arg) {
                case 'number': arg = '' + arg;  // fall through
                case 'string': arg = document.createTextNode(arg);
            }
            e.appendChild(arg);
        };

        return e;
    };

    safe_del = function(t, attr) {
        if (typeof (t[attr]) != 'undefined') delete t[attr];
    };

    $.LABEL = function(msg, o) { return $.create('label', o, msg) };
    $.INPUT = function(t, o) {
        var opt = $.extend({ 'type': t }, o || {});
        var b = $.create('input', opt);
        if (opt.value) b.value = opt.value;
        return b;
    };
    $.TEXT = function(o) { return $.INPUT('text', o) };
    $.PASSWORD = function(o) { return $.INPUT('password', o) };
    $.BUTTON = function(o) { return $.INPUT('button', o) };
    $.SUBMIT = function(o) { var b = $.INPUT('submit', o); b.type = 'submit'; return b; };
    $.RESET = function(o) { return $.INPUT('reset', o) };
    $.FILE = function(o) { return $.INPUT('file', o) };
    $.CHECKBOX = function(o) { return $.INPUT('checkbox', o) };
    $.RADIO = function(o) { return $.INPUT('radio', o) };
    $.HIDDEN = function(o) { return $.INPUT('hidden', o) };
    $.TEXTAREA = function(o) {
        var text = o.value || '';
        safe_del(o, 'value');
        return $.create('textarea', o, text);
    };
    $.SELECT = function(o/*value: optiones*/) {
        var select = $('<select></select>');
        var s = select.get(0);
        var value = o.value || [];
        safe_del(o, 'value');
        var options = o.options || [];
        safe_del(o, 'options');
        select.attr(o);
        for (var i = 0; i < options.length; i++) {
            var opt = options[i];
            if (typeof (opt.text) == 'undefined') opt.text = opt.value;
            var option = $.create('option', opt);
            var pos = s.options.length;
            s.options[pos] = option;
            if (value.index(opt.value) > -1) s.options[pos].selected = true;
        }
        return select;
    };

})(jQuery);