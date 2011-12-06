/// <reference path="../jquery-1.4.1-vsdoc.js" />

; (function() {

    We7.Controls.ChildrenTableHorizontal = We7.extend(We7.Controls.Field, {
        setOptions: function(options) {
            We7.Controls.TextBox.superclass.setOptions.call(this, options);
            this.options.Maxcharater = this.getParam("maxcharater");
            this.options.ReadOnly = options.ReadOnly || false;

        },

        //render input dom node

        renderComponent: function() {

            //the wrap element div
            this.warpEl = $.create('div', { className: 'we7-textbox-wrap' });

            //attributes of the input field
            var attributes = {};
            attributes.id = this.options.ID || this.options.Name;
            if (this.options.Name) { attributes.name = this.options.Name; }

            if (this.options.Maxcharater) { attributes.maxLength = this.options.Maxcharater; }

            //create the input element

            this.el = $.TEXT(attributes);
            //set css
            if (this.options.Width) { $(this.el).css("width", this.options.Width); };
            if (this.options.Height) { $(this.el).css("height", this.options.Height); };
            //append it to the warp
            if (this.options.ReadOnly) { $(this.el).attr("readonly", "readonly"); }
            $(this.warpEl).append(this.el);

            $(this.fieldContainer).append(this.warpEl);

        },
        initEvents: function() {
            $(this.el).click(this.options.click);
            $(this.el).change(this.options.change);
            $(this.el).blur(this.options.blur);
            $(this.el).keyup(this.options.keyup);
            $(this.el).keydown(this.options.keydown);
            $(this.el).keypress(this.options.keypress);
        },


        getValue: function() {

            var value;

            value = $(this.el).val();

            if (this.options.trim) {

                value = jQuery.trim(value);
            }

            return value;
        },

        setValue: function(value) {
            $(this.el).val(value);
        },
        getEl: function() {
            return this.el;
        }

    });

    // Regist this class as "ChildrenTable" type
    We7.Control.registerType("ChildrenTableHorizontal", We7.Controls.ChildrenTableHorizontal, [{ Type: "TextBox", Label: "模型名称", Name: "Params.data", Value: ''}]);

})();