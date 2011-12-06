
/**
options:Id,Name,Width,Height,Items:[],Buttons:[]
**/
We7.Controls.Form = function(options) {

    //设置参数
    this.setOptions(options || {});

    //渲染表单
    this.render();

    //初始化事件
    this.initEvents();

    //添加到父节点
};

//原型扩展
We7.Controls.Form.prototype = {
    setOptions: function(options) {
        this.options = {};
        this.options = $.extend({}, options, this.options);
    },

    render: function() {
        this.formWarp = $(document.createElement("form"));
        this.bodyWarp = $(document.createElement("div")).addClass("form-body");
        this.buttonWarp = $(document.createElement("div")).addClass("form-button");
        var items = this.options.Items;
        for (var i = 0; i < items.length; i++) {
            var input = items[i];

            if (!input) {
                throw new Error("error");
            }

            var control = this.renderFiled(input);

            $(this.bodyWarp).append(control.getWarpEl());
        }

        var buttons = this.options.Buttons;

        for (var i = 0; i < buttons.length; i++) {
            var buttonCfg = buttons[i];
            if (!button) {
                throw new Error("error!");
            }

            var button = this.renderButton(buttonCfg);

            $(this.buttonWarp).append(button);
        }
    },
    renderFiled: function(options) {

        var filedInstance = new We7.Control(options);

        return filedInstance;
    },
    renderButton: function(options) {

    },
    initEvents: function() {

    }
};