
; (function() {

    We7.Controls.TypeProperty = We7.extend(We7.Controls.Field, {

        setOptions: function(options) {
            We7.Controls.TypeProperty.superclass.setOptions.call(this, options);
            this.inputs = [];
            this.inputsNames = {};
            this.options.ControlType = options.ControlType;
            this.options.change = options.change;
        },

        //render

        render: function() {
            //the wrap element div
            this.divEl = $.create("div", { className: "we7-typeproperty-wrap" });
            var class0 = We7.Control.getFieldClass(this.options.ControlType).groupOptions;
            for (var i = 0; i < class0.length; i++) {
                var input = class0[i];

                if (!input) {
                    throw new Error("error");
                }

                var control = this.renderFiled(input);

                $(this.divEl).append(control.getWarpEl());
            }

            this.setValue(this.options);
        },
        getWarpEl: function() {
            return this.divEl;
        },
        renderFiled: function(options) {

            var filedInstance = new We7.Control(options);

            //            if (options.Name && this.options[options.Name]) {
            //                filedInstance.setValue(this.options[options.Name]);
            //            }
            this.inputs.push(filedInstance);

            if (filedInstance.options.Name) {
                this.inputsNames[filedInstance.options.Name] = filedInstance;
            }

            return filedInstance;
        },
        initEvents: function() {
            for (var p in this.inputs) {

                $(this.inputs[p].getEl()).click(this.options.click);
                $(this.inputs[p].getEl()).change(this.options.change);
                $(this.inputs[p].getEl()).blur(this.options.blur);
                $(this.inputs[p].getEl()).keyup(this.options.keyup);
                $(this.inputs[p].getEl()).keydown(this.options.keydown);
                $(this.inputs[p].getEl()).keypress(this.options.keypress);
            }
        },
        setValue: function(ovalue) {
            if (!ovalue) {
                return;
            }

            for (var i = 0; i < this.inputs.length; i++) {
                var field = this.inputs[i];
                var name = field.options.Name;

                var tempName = name.split(".");

                if (tempName.length == 2) {
                    var tt = ovalue[tempName[0]];
                    for (var t in tt) {
                        if (tt[t].Name == tempName[1]) {
                            field.setValue(tt[t].Value);
                        }

                       
                    }
                }
                else {
                    field.setValue(ovalue[name]);
                }
            }
        },
        getValue: function() {
            var o = {};
            for (var i = 0; i < this.inputs.length; i++) {
                var v = this.inputs[i].getValue();
                if (We7.isObject(v)) {
                    //TODO::?
                    We7.extend(o, v);
                }
                else {
                    o[this.inputs[i].options.Name] = v;
                }
            }
            return o;
        }

    });

    // Register this class as "TypeProperty" type
    We7.Control.registerType("TypeProperty", We7.Controls.TypeProperty, []);
})();
