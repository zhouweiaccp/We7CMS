/// <reference path="../jquery-1.4.1-vsdoc.js" />

; (function() {

    We7.Controls.Label = We7.extend(We7.Controls.Field, {
        setOptions: function(options) {
            We7.Controls.Label.superclass.setOptions.call(this, options);

        },

        //render input dom node

        renderComponent: function() {

            //the wrap element div
            this.warpEl = $.create('div', { className: 'we7-label-wrap' });

            //attributes of the input field
            var attributes = {};
            attributes.id = this.options.ID || this.options.Name;
            if (this.options.Name) { attributes.name = this.options.Name; }

            //create the input element

            this.el = $.LABEL(this.options.Value || "",attributes);
            //set css
            if (this.options.Width) { $(this.el).css("width", this.options.Width); };
            if (this.options.Height) { $(this.el).css("height", this.options.Height); };
            //append it to the warp

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

            value = $(this.el).text();

            if (this.options.trim) {

                value = jQuery.trim(value);
            }

            return value;
        },

        setValue: function(value) {
            $(this.el).text(value);
        },
        getEl: function() {
            return this.el;
        }

    });

    // Register this class as "Label" type
    We7.Control.registerType("Text", We7.Controls.Label, []);
    We7.Control.registerType("We7.IP", We7.Controls.Label, []);
})();
