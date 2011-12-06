
; (function() {

    We7.Controls.GUID = We7.extend(We7.Controls.Field, {

        setOptions: function(options) {
            We7.Controls.GUID.superclass.setOptions.call(this, options);

        },

        //render input dom node

        renderComponent: function() {
            //the wrap element div
            this.warpEl = $.create('div', { className: 'we7-guid-wrap' });

            //attributes of the input field
            var attributes = {};
            attributes.id = this.options.ID || this.options.Name;
            if (this.options.Name) { attributes.name = this.options.Name; }

            //create the input element

            this.el = $.TEXT(attributes);
            //append it to the warp

            $(this.warpEl).append(this.el);

            $(this.fieldContainer).append(this.warpEl);

        },
        getEl: function() {
            return this.el;
        }

    });

    // Register this class as "TextBox" type
    We7.Control.registerType("GUID", We7.Controls.GUID, []);
})();
