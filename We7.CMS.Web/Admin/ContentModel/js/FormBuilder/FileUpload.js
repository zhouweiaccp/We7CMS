/// <reference path="../jquery-1.4.1-vsdoc.js" />

; (function() {

    We7.Controls.File = We7.extend(We7.Controls.Field, {

        setOptions: function(options) {
            We7.Controls.File.superclass.setOptions.call(this, options);
        },

        //render input dom node

        renderComponent: function() {
            //the wrap element div
            this.warpEl = $.create('div', { className: 'we7-file-wrap' });

            //attributes of the input field
            var attributes = {};


            attributes.ID = this.options.ID || this.options.Name;
            if (this.options.Name) { attributes.Name = this.options.Name; }


            //create the input element

            this.el = $.FILE(attributes);
            //set css
            if (this.options.Width) { $(this.el).css("width", this.options.Width); };
            if (this.options.Height) { $(this.el).css("height", this.options.Height); };
            //append it to the warp

            $(this.warpEl).append(this.el);

            $(this.fieldContainer).append(this.warpEl);

        },
        initEvents: function() {
            $(this.el).change(this.options.change);
    
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

    // Register this class as "File" type
    We7.Control.registerType("File", We7.Controls.File, []);
})();
