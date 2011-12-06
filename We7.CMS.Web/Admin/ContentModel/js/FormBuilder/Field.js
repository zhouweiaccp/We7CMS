/// <reference path="../../Scripts/jquery-1.4.1-vsdoc.js" />

; (function() {

    /**
    所有控件的基类，包含一些公共属性和方法,不可被实例化(abstract)
    参数:1)Name:控件名称
    2)Required:boolean,当为true是控件不可为null
    3)className:创建包裹层(wrapper)的Css class Name (默认值:we7_field)
    4)value:初始化时的值
    5)parentEl:该控件添加的父节点DOM(可为HTMLElement或者ID)
    ***/

    We7.Controls.Field = function(options) {
        //设置默认参数值(set the default value of the options)
        this.setOptions(options || {});
        ;
        //渲染dom节点(call the render of the dom)
        this.render();

        //初始化事件
        this.initEvents();

        //初始化默认值
        if (!We7.isUndefined(this.options.DefaultValue)) {
            //如果存在默认值
            this.setValue(this.options.DefaultValue);
        }

        //将该控件添加到父节点
        if (options.parentEl) {
            if (We7.isString(options.parentEl)) {
                //父节点ID
                jQuery(options.parentEl).append(this.getEl());
            }
            else {
                //父节点Jquery对象
                options.parentEl.append(this.getEl());
            }
        }

    };

    We7.Controls.Field.prototype = {
        //设置默认参数(参数[obejct]options将传递为构造函数)
        setOptions: function(options) {

            //本身类和父类构造函数设置默认参数
            this.options = {};
            this.options = $.extend({}, options, this.options);

            this.options.ID = options.ID || options.Name;
            this.options.ID = this.options.ID || We7.generateId();
            this.options.Name = options.Name || "";
            this.options.Label = options.Label || "";
            this.options.Type = options.Type || ""; //defaulttype
            this.options.Required = options.Required;
            this.options.Width = options.Width
            this.options.Height = options.Height;
            this.options.DefaultValue = options.DefaultValue;
            this.options.Visible = options.Visible;
            this.options.Desc = options.Desc;
            this.options.CssClass = options.CssClass;
            this.options.Params = this.options.Params || [];
        },

        //默认控件渲染，创建一个Div包裹这个控件
        render: function() {
            //创建一个div warp
            this.divEl = jQuery.create('div', { className: 'we7-fieldwarpper' });

            //Label element
            if (this.options.Label) {
                //                this.labelDiv = jQuery.create('div', { className: 'we7-Label-warp' });

                this.labelEl = jQuery.LABEL(this.options.Label);

                //    $(this.labelDiv).append(this.labelEl);
                $(this.divEl).append(this.labelEl);
            }

            this.fieldContainer = jQuery.create('div', { className: 'we7-field-container' });

            //直接内部(abstract)
            this.renderComponent();

            //add fieldContainer
            $(this.divEl).append($(this.fieldContainer));
            //插入一个去除浮动的DIV
            var clearDiv = jQuery.create('div', { className: 'clear' });
            $(this.divEl).append(clearDiv);

        },

        //渲染控件虚方法
        renderComponent: function() {
            //override me
        },
        //返回Wrap div
        getWarpEl: function() {
            return this.divEl;
        },
        //返回表单元素
        getEl: function() {
            //override 
        },
        //初始化事件虚方法
        initEvents: function() {
            //override me
        },

        //返回值
        getValue: function() {
            //override me
        },

        setValue: function(value) {
            //to be inherited
        },
        isEmpty: function() {
            return this.getValue() === '';
        },
        setParentField: function(parentField) {
            this.parentField = parentField;
        },

        getParentField: function() {
            return this.parentField;
        },
        getParam: function(name) {
            for (var p in this.options.Params) {
                if (this.options.Params[p].Name == name) {
                    return this.options.Params[p].Value;
                }

            }
            return null;
        }

    };

    We7.Controls.Field.groupOptions = [{ Type: "Text", ReadOnly: true, Label: "控件类型", Name: "Type", value: '' }, { Type: "TextInput", Label: "标签", Name: "Label", value: '' }, { Type: "TextInput", ReadOnly: true, Label: "名称", Name: "Name", value: '' }
    , { Type: "Select", Label: "必须", Name: "Required", Params: [{ Name: "data", Value: "true|是,false|否"}] }, { Type: "Select", Label: "可见性", Name: "Visible", Params: [{ Name: "data", Value: "true|是,false|否"}] }, { Type: "TextInput", Label: "Css类", Name: "CssClass" }, { Type: "TextInput", Label: "宽度", Name: "Width", value: '' },
     { Type: "TextInput", Label: "高度", Name: "Height", value: '' }, { Type: "TextInput", Label: "默认值", Name: "DefaultValue", value: '' }, { Type: "TextInput", Label: "描述", Name: "Desc", value: '' }
    ];
})();