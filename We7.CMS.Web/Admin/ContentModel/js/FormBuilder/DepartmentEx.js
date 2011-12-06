/// <reference path="../jquery-1.4.1-vsdoc.js" />

We7.Controls.DepartmentEx = We7.extend(We7.Controls.Field, {

    setOptions: function(options) {
        We7.Controls.SelectInput.superclass.setOptions.call(this, options);
        this.options.SelectData = this.getParam("selectdata");
        this.options.SelectData = this.options.SelectData || "";

    },
    renderComponent: function() {
        //the wrap element div
        this.warpEl = $.create('div', { className: 'we7-selectinput-wrap' });

        //attributes of the input field
        var attributes = {};

        // attributes.ID = this.options.ID || this.options.Name;
        if (this.options.Name) { attributes.Name = this.options.Name; }

        //create the input element

        this.el = $.TEXT(attributes);
        this.select = $.SELECT(attributes);

        if (this.options.Width) { $(this.el).css("width", this.options.Width); };
        if (this.options.height) { $(this.el).css("height", this.options.height); };

        //append it to the warp

        $(this.warpEl).append(this.el);

        $(this.warpEl).append(this.select);
        if (this.options.SelectData) {
            this.setValue(this.options.SelectData);
        }
        $(this.fieldContainer).append(this.warpEl);
    },
    initEvents: function() {
        $(this.el).change(this.options.change);
    },
    setValue: function(Value) {
        $(this.el).val(Value);
    },
    getEl: function() {
        return this.el;
    },
    getValue: function() {
        return $(this.el).val();
    }
});
We7.Control.registerType("We7.DepartmentEx", 
                        We7.Controls.DepartmentEx,
                        [{ "Name": "Params.parentId",Type:"TextBox", "Label": "部门父ID"},{ "Name": "Params.role",Type:"TextBox", "Label": "可见的角色ID"},{ Type: "Select", Label: "格式化", Name: "Params.format", Params: [{ Name: "data", Value: "true|是,false|否"}] }]);