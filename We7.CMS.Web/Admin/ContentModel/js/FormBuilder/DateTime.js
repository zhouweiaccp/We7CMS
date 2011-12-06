We7.Controls.DateTime = We7.extend(We7.Controls.Field, {

        setOptions: function(options) {
            We7.Controls.DateTime.superclass.setOptions.call(this, options);
            this.options.dataFormat = this.getParam("dateFormat") || 'mm/dd/yy';
        },
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
            $(this.el).datepicker({dataFormat:this.options.dataFormat});
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

    // Register this class as "DateTime" type
 We7.Control.registerType("DateTime", We7.Controls.DateTime, [{ Type: "TextInput", Label: "时间格式", Name: "Params.dataFormat", Value: ''}]);
