/// <reference path="../jquery-1.4.1-vsdoc.js" />

We7.Controls.Number = We7.extend(We7.Controls.TextBox, {

    setOptions: function(options) {

        We7.Controls.Number.superclass.setOptions.call(this, options);
    },
    initEvents: function() {
        We7.Controls.Number.superclass.initEvents.call(this);
        $(this.el).css("ime-mode", "disabled");
        $(this.el).bind("keypress", function(e) {
            var code = (e.keyCode ? e.keyCode : e.which);  //兼容火狐 IE   

            if (e.keyCode == 46 && this.value.indexOf(".") == -1) {

                return true;
            }
            return code >= 48 && code <= 57;

        });
        $(this.el).bind("blur", function() {
            if (this.value.lastIndexOf(".") == (this.value.length - 1)) {
                this.value = this.value.substr(0, this.value.length - 1);
            } else if (isNaN(this.value)) {
                this.value = "";
            }
        });
        $(this.el).bind("paste", function() {
            var s = clipboardData.getData('text');
            if (!/\D/.test(s));
            value = s.replace(/^0*/, '');
            return false;
        });
        $(this.el).bind("dragenter", function() {
            return false;
        });
        $(this.el).bind("keyup", function() {
            if (/(^0+)/.test(this.value)) {
                this.value = this.value.replace(/^0*/, '');
            }
        });
    }
});

We7.Control.registerType("Number", We7.Controls.Number, []);