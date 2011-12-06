/// <reference path="../jquery-1.4.1-vsdoc.js" />
; (function() {
    We7.Controls.CheckBox = We7.extend(We7.Controls.Field, {

        setOptions: function(options) {
            We7.Controls.CheckBox.superclass.setOptions.call(this, options);
            this.options.Checked = options.Checked || false;
        },
        renderComponent: function() {
            //the warp element tabel

            this.warpEl = $.create('div', { className: 'we7-checkbox-wrap' });

            var attributes = {};
            if (this.options.Name) { attributes.name = this.options.Name; }
            if (this.options.ID) { attributes.id = this.options.ID; }
            if (this.options.Value) { attributes.value = this.options.Value; }
         
            if (this.options.Checked && this.options.Checked === true) { attributes.checked = "Checked"; }
            this.el = $.CHECKBOX(attributes);
            $(this.warpEl).append(this.el);
            $(this.fieldContainer).append(this.warpEl);

        },
        initEvents: function() {
            $(this.el).click(this.options.click);
            $(this.el).change(this.options.change);
        },
        getValue: function() {

            if ($(this.el).attr('checked') == true) {
                return true;
            }
            else {
                return false;
            }
        },
        getEl: function() {
            return this.el;
        },
        setValue: function(Checked) {
   
            if (Checked && Checked == true) {
                $(this.el).attr("checked", true);
            }
            else {
                $(this.el).attr("checked", "");
            }
        }
    });
})();
We7.Control.registerType("Check", We7.Controls.CheckBox, []);