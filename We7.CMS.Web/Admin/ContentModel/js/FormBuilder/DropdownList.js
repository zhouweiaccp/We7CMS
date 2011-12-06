/// <reference path="../jquery-1.4.1-vsdoc.js" />

We7.Controls.DropdownList = We7.extend(We7.Controls.Field, {

    setOptions: function (options) {
        We7.Controls.DropdownList.superclass.setOptions.call(this, options);
        this.options.Url = options.Url;
        this.options.DefaultValue = options.DefaultValue;
        this.options.TextField = options.TextField;
        this.options.ValueField = options.ValueField;
        this.options.ParamData = options.ParamData;

    },
    renderComponent: function () {
        //the wrap element div
        this.warpEl = $.create('div', { className: 'we7-textbox-wrap' });

        //attributes of the input field
        var attributes = {};
        attributes.ID = this.options.ID || this.options.Name;
        if (this.options.Name) { attributes.Name = this.options.Name; }

        //create the input element

        this.el = $.SELECT(attributes);
        var elm = this.el;
        if (this.options.Width) { $(this.el).css("width", this.options.Width); };
        if (this.options.height) { $(this.el).css("height", this.options.height); };
        //ajax Items
        if (this.options.Url) {
            $.getJSON(this.options.Url, this.options.ParamData, function (data) {
                $.each(data, function (i) {
                    var attr = {};
                    attr.Value = data[i].Value;
                    var op = $.create("option", attr, data[i].text);
                    $(elm).append(op);
                });
            });
        }

        var Params = this.options.Params;
        if (!this.options.Url && Params) {

            $.each(Params, function (i) {
                if (Params[i].Name == "data") {
                    var string = Params[i].Value;
                    var items = string.split(",");

                    $.each(items, function (j) {
                        var item = items[j].split("|");
                        if (item.length == 2) {
                            var attr = {};
                            attr.value = item[0];
                            var op = $.create("option", attr, item[1]);
                            $(elm).append(op);
                        }
                        else {
                            var attr = {};
                            attr.value = item[0];
                            var op = $.create("option", attr, item[0]);
                            $(elm).append(op);
                        }

                    });
                }
            });

        }
        //append it to the warp

        $(this.warpEl).append(this.el);

        $(this.el).attr("value", this.options.Value);

        $(this.fieldContainer).append(this.warpEl);
        if (this.options.DefaultValue) {
            this.setValue(this.options.DefaultValue);
        }
    },
    initEvents: function () {
        $(this.el).change(this.options.change);
    },
    setValue: function (Value) {

        $(this.el).attr("value", Value);
    },
    getEl: function () {
        return this.el;
    },
    getValue: function () {
        return $(this.el).val();
    },
    getText: function () {
        return $(this.el).find("option:selected").text();

    }
});
We7.Control.registerType("Select", We7.Controls.DropdownList, [{ Type: "TextBox", Label: "数据项", Name: "Params.data", value: '选项1|选项2|选项3'}]);