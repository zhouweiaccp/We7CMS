/// <reference path="../jquery-1.4.1-vsdoc.js" />

We7.Controls.RelationSelect = We7.extend(We7.Controls.Field, {

	setOptions: function (options) {
		We7.Controls.RelationSelect.superclass.setOptions.call(this, options);
		this.options.Url = options.Url;
		this.options.TextField = options.TextField;
		this.options.ValueField = options.ValueField;
		this.options.ParamData = options.ParamData;

	},
	renderComponent: function () {
		//the wrap element div
		this.warpEl = $.create('div', { className: 'we7-RelationSelect-wrap' });

		//attributes of the input field
		var attributes = {};

		attributes.ID = this.options.ID || this.options.Name;
		if (this.options.Name) { attributes.Name = this.options.Name; }

		//create the input element

		this.el = $.SELECT(attributes);
		var elm = this.el;
		if (this.options.Width) { $(this.el).css("width", this.options.Width); };
		if (this.options.height) { $(this.el).css("height", this.options.height); };
		//ajax Items
		if (this.options.Url) {
			$.getJSON(this.options.Url, this.options.ParamData, function (data) {
				$.each(data, function (i) {
					var attr = {};
					attr.Value = data[i].Value;
					var op = $.create("option", attr, data[i].text);
					$(elm).append(op);
				});
			});
		}
		var Params = this.options.Params;
		if (!this.options.Url && Params) {

			$.each(Params, function (i) {
				if (Params[i].Name == "data") {
					var string = Params[i].Value;
					var items = string.split(",");

					for (var p in items) {
						var item = items[p].split("|");

						if (item.length == 2) {
							var attr = {};
							attr.value = item[0];
							var op = $.create("option", attr, item[1]);
							$(elm).append(op);
						}
						else {
							var attr = {};
							attr.value = item[0];
							var op = $.create("option", attr, item[0]);
							$(elm).append(op);
						}
					}
				}
			});

		}
		//append it to the warp

		$(this.warpEl).append(this.el); $(this.warpEl).append("<a href='javascript:void(0);'>新增</a>");

		$(this.el).attr("value", this.options.Value);

		$(this.fieldContainer).append(this.warpEl);
	},
	initEvents: function () {
		$(this.el).change(this.options.change);
	},
	setValue: function (Value) {

		$(this.el).attr("value", Value);
	},
	getEl: function () {
		return this.el;
	},
	getValue: function () {
		return $(this.el).val();
	},
	getText: function () {
		return $(this.el).find("option:selected").text();

	}
});
We7.Control.registerType("RelationSelect", We7.Controls.RelationSelect,
 [{ Type: "TextBox", Label: "模型名称", Name: "Params.model" },
  { Type: "TextBox", Label: "文本字段", Name: "Params.textfield" },
 { Type: "TextBox", Label: "值字段", Name: "Params.valuefield" },
  { Type: "TextBox", Label: "关联文本字段", Name: "Params.df" },
  { Type: "Select", Label: "生成统计字段", Name: "Params.count", Params: [{ Name: "data", Value: "true|是,false|否"}], Value: 'false' }
  ]);