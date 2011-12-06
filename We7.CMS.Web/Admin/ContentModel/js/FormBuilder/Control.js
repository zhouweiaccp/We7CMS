/// <reference path="../jquery-1.4.1-vsdoc.js" />

; (function() {
    /****
    * 创建控件工具类
    * 参数:1)[object]fieldOptions 配置信息
    2)[We7.Controls.Container] 容器类
    ****/
    We7.Control = function(fieldOptions, parentField) {
        var fieldClass = null, inputInstance;
        if (fieldOptions.Type) {

            fieldClass = We7.Control.getFieldClass(fieldOptions.Type);
        }
        if (fieldClass === null) {
            //默认为input类型
            fieldClass = We7.Controls.TextBox;
        }
        else {
            //fieldClass = fieldOptions.fieldClass ? fieldOptions.fieldClass : We7.Controls.StringField;
        }

        if (typeof fieldOptions.inputParams === 'Object') {
            inputInstance = new fieldClass(fieldOptions.inputParams);
        }
        else {

            inputInstance = new fieldClass(fieldOptions);
        }
        if (parentField) {
            inputInstance.setParentField(parentField);
        }

        return inputInstance;
    };
    jQuery.extend(true, We7.Control, {
        typeClasses: {},

        //注册控件类型
        registerType: function(type, fieldClass, groupOptions, dontInherit) {
    
            if (!We7.isString(type)) {
                throw new Error("registerType: first argument must be a string");
            }
            if (!We7.isFunction(fieldClass)) {
                throw new Error("registerType: second argument must be a function");
            }
            this.typeClasses[type] = fieldClass;

            // Setup the groupOptions property on the class
            var opts = [];
            if (We7.isArray(groupOptions)) { opts = groupOptions; }
            if (fieldClass.superclass && !dontInherit && We7.isArray(fieldClass.superclass.constructor.groupOptions)) {
                var tempOpts = fieldClass.superclass.constructor.groupOptions.concat();

                opts = tempOpts.concat(opts);
                //  opts = opts.concat(fieldClass.superclass.constructor.groupOptions);

            }
            fieldClass.groupOptions = opts;
        },
        getFieldClass: function(type) {
            //TODO::设置默认
            return We7.isFunction(this.typeClasses[type]) ? this.typeClasses[type] : this.typeClasses['TextInput'];
        },
        getType: function(FieldClass) {
            for (var type in this.typeClasses) {
                if (this.typeClasses.hasOwnProperty(type)) {
                    if (this.typeClasses[type] == FieldClass) {
                        return type;
                    }
                }
            }
            return null;
        },
        buildField: function(fieldOptions) {
            return We7.Control(fieldOptions);
        }
    });
})();
