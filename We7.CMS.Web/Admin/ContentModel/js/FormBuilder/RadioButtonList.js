/// <reference path="../jquery-1.4.1-vsdoc.js" />

; (function() {
    We7.Controls.RadioButtonList = We7.extend(We7.Controls.Field, {

        setOptions: function(options) {
            We7.Controls.RadioButtonList.superclass.setOptions.call(this, options);
            this.options.Direction = this.getParam("direction");
            this.options.Direction = this.options.Direction || "h"; //默认为横向排

            this.items = [];

        },
        renderComponent: function() {
            //the warp element tabel

            this.warpEl = $.create('div', { className: 'we7-radiobuttonlist-wrap' });

            if (this.options.Direction === "v") {
                $(this.warpEl).addClass("we7-radiobuttonlist-v");
            }
            else {
                $(this.warpEl).addClass("we7-radiobuttonlist-h");
            }

            var Params = this.options.Params;

            var op = this.options;
            var tempItems = this.items;
            var tempWarp = this.warpEl;
            $.each(Params, function(i) {
                if (Params[i].Name == "data") {
                    var string = Params[i].Value;
                    var items = string.split(",");
                    //自增长因子
                    var _itemgrouth = 0;
                    for (var p in items) {
                        var item = items[p].split("|");

                        if (item.length == 2) {
                            _itemgrouth++;
                            var _temp_item_name = op.ID + "_item_group_id" + _itemgrouth;
                            var li = $.create("span");
                            var radio = $.RADIO({ name: op.Name, value: item[0], id: _temp_item_name });
                            var label = $('<label for="' + _temp_item_name + '">' + item[1] + '</label>');
                            tempItems.push(radio);
                            $(li).append(radio);
                            $(li).append(label);
                            $(tempWarp).append(li);
                        }
                        else {
                            var li = $.create("span");
                            var radio = $.RADIO({ name: op.Name, value: item[0] });
                            var label = $.LABEL(item[0]);
                            tempItems.push(radio);
                            $(li).append(radio);
                            $(li).append(label);
                            $(tempWarp).append(li);
                        }
                    }
                }
            });

            $(this.fieldContainer).append(this.warpEl);

        },
        initEvents: function() {

            for (var p in this.items) {
                $(this.items[p]).change(this.options.change);
            }
        },
        getValue: function() {
            return $("input:radio[name='" + this.options.name + "']:checked").val();
        },
        getEl: function() {
            return this.items;
        },
        setValue: function(value) {

            var items = this.items;
            jQuery.each(items, function(i) {
                if ($(items[i]).val() == value) {
                    $(items[i]).get(0).checked = true;
                }
            });
        }
    });
})(jQuery);
We7.Control.registerType("RadioButton", We7.Controls.RadioButtonList,
 [{ "Name": "Params.direction", "Label": "方向", "Type": "Select", "Params": [{ "Name": "data", "Value": "h|横向,v|纵向"}] },
    { Type: "TextBox", Label: "数据项", Name: "Params.data", value: '' }]);