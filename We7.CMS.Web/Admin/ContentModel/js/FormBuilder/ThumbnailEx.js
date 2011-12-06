/// <reference path="../jquery-1.4.1-vsdoc.js" />

; (function() {

    We7.Controls.ThumbnailEx = We7.extend(We7.Controls.Field, {

        setOptions: function(options) {
            We7.Controls.ThumbnailEx.superclass.setOptions.call(this, options);

            this.options.Size = this.getParam("size") || "min";

            if (this.options.Size == "min") {
                this.options.imgWidth = 40;
                this.options.imgHeight = 40;
            }
            else if (this.options.Size == "middle") {
                this.options.imgWidth = 80;
                this.options.imgHeight = 80;
            }

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
            this.image = $('<img />');
            //set css
            if (this.options.Width) { $(this.image).css("width", this.options.imgWidth); };
            if (this.options.Height) { $(this.image).css("height", this.options.imgHeight); };
            //append it to the warp

            $(this.warpEl).append(this.el);
            $(this.warpEl).append(this.image);

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

    // Register this class as "ThumbnailEx" type
    We7.Control.registerType("We7.ThumbnailEx", We7.Controls.ThumbnailEx, [{ Type: "Select", Label: "选择类型", Name: "Params.uploader", DefaultValue: "Simple", Params: [{ Name: "data", Value: "Simple|简单上传,Thumbnail|缩略图上传,Multi|多文件上传"}]}]);
})();
