/// <reference path="../jquery-1.4.1-vsdoc.js" />

; (function() {

    We7.Controls.Editor = We7.extend(We7.Controls.Field, {

        setOptions: function(options) {
            We7.Controls.Editor.superclass.setOptions.call(this, options);
            this.options.Height = options.Height;
            this.options.Width = options.Width;
            this.options.Rows = options.Rows;

        },

        //render input dom node

        renderComponent: function() {

            //the wrap element div
            this.warpEl = $.create('div', { className: 'we7-textarea-wrap' });

            //attributes of the input field
            var attributes = {};

            if (this.options.Rows) { attributes.rows = this.options.Rows; }
            if (this.options.Name) { attributes.name = this.options.Name; }

            //create the input element

            this.el = $.TEXTAREA(attributes);

            $(this.el).css("height", this.options.Height);
            $(this.el).css("width", this.options.Width);
            //append it to the warp

            $(this.warpEl).append(this.el);

            $(this.fieldContainer).append(this.warpEl);

        },
        initEvents: function() {

            $(this.el).change(this.options.change);
            $(this.el).blur(this.options.blur);
            $(this.el).keyup(this.options.keyup);
            $(this.el).keydown(this.options.keydown);
            $(this.el).keypress(this.options.keypress);
        },
        getValue: function() {

            return $(this.el).val();
        },
        getEl: function() {
            return this.el;
        },
        setValue: function(value) {
            $(this.el).val(value);
        }
    });

    // Register this class as "TextBox" type
    We7.Control.registerType("Editor", We7.Controls.Editor, []);
})();
