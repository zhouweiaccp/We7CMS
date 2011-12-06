/// <reference path="../jquery-1.4.1-vsdoc.js" />

; (function() {
    We7.Controls.CheckBoxList = We7.extend(We7.Controls.Field, {

        setOptions: function(options) {
            We7.Controls.CheckBoxList.superclass.setOptions.call(this, options);

            this.options.Direction = this.getParam("direction");
            this.options.Direction = this.options.Direction || "h"; //默认为横向排

            this.items = [];

        },
        renderComponent: function() {
            //the warp element tabel

            this.warpEl = $.create('ul', { className: 'we7-checkboxlist-wrap' });

            if (this.options.Direction === "v") {
                $(this.warpEl).addClass("we7-checkboxlist-v");
            }
            else {
                $(this.warpEl).addClass("we7-checkboxlist-h");
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
                        _itemgrouth++;
                        var _temp_item_name = op.ID+"_item_group_id" + _itemgrouth;
                        if (item.length == 2) {
                            var li = $.create("span");
                            var radio = $.CHECKBOX({ name: op.Name, value: item[0], id: _temp_item_name });
                            //$('<label for="'+_temp_item_name+'">'+item[1]+'</label>');
                            var label = $('<label for="' + _temp_item_name + '">' + item[1] + '</label>'); //$.LABEL(item[1], { labelfor: _temp_item_name });
                            tempItems.push(radio);
                            $(li).append(radio);
                            $(li).append(label);
                            $(tempWarp).append(li);
                        }
                        else {
                            var li = $.create("span");
                            var radio = $.CHECKBOX({ name: op.Name, value: item[0] });
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
            var tempvalue = [];
            $("input:checkbox[name='" + this.options.Name + "']:checked").each(function() {
                tempvalue.push($(this).val());
            });
        },
        getEl: function() {
            return this.items;
        },
        setValue: function(Value) {

            var items = this.items;
            if (We7.isArray(Value)) {
                jQuery.each(items, function(i) {
                    jQuery.each(Value, function(i) {
                        if ($(items[i]).val() == Value[i]) {
                            $(items[i]).get(0).checked = true;
                        }
                    });
                });
            }
            else {
                jQuery.each(items, function(i) {
                    if ($(items[i]).val() == Value) {
                        $(items[i]).get(0).checked = true;
                    }
                });
            }
        }
    });
})(jQuery);
We7.Control.registerType("CheckBox", We7.Controls.CheckBoxList,
 [{ "Name": "Params.direction", "Label": "方向", "Type": "Select", "Params": [{ "Name": "data", "Value": "h|横向,v|纵向"}] }, { Type: "TextBox", Label: "数据项", Name: "Params.data", value: ''}]);