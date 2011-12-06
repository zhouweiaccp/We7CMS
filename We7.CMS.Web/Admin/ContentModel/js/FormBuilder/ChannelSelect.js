/// <reference path="../jquery-1.4.1-vsdoc.js" />

We7.Controls.ChannelSelect = We7.extend(We7.Controls.Field, {

    setOptions: function(options) {

        We7.Controls.ChannelSelect.superclass.setOptions.call(this, options);
    },
    renderComponent: function() {
        //the wrap element div
        this.warpEl = $.create('div', { className: 'we7-channel-wrap' });

        //attributes of the input field
        var attributes = {};
        attributes.ID = this.options.ID || this.options.Name;
        if (this.options.Name) { attributes.Name = this.options.Name; }

        //create the input element

        this.el = $.SELECT(attributes);
        var elm = this.el;
        if (this.options.Width) { $(this.el).css("width", this.options.Width); };
        if (this.options.height) { $(this.el).css("height", this.options.height); };
        $(this.el).append('<option>请选择栏目</option>');

        //append it to the warp

        $(this.warpEl).append(this.el);

        $(this.el).attr("value", this.options.Value);

        $(this.fieldContainer).append(this.warpEl);
    },
    initEvents: function() {
        $(this.el).change(this.options.change);
    },
    setValue: function(Value) {

        $(this.el).attr("value", Value);
    },
    getEl: function() {
        return this.el;
    },
    getValue: function() {
        return $(this.el).val();
    },
    getText: function() {
        return $(this.el).find("option:selected").text();
    }
});
We7.Control.registerType("We7.ChannelSelect", We7.Controls.ChannelSelect, []);