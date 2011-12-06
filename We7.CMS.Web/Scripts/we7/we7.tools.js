///<reference name="../jQuery/jquery-1.4.2.min.js" assembly="$" />
///<reference path="jquery.tools.min.js" />
///<reference path="we7.core.js" />

(function (we7) {
	function ensureArgs(args, obj) {
		var tip = args[0], options = args[1] || {};
		if (we7.isFunc(tip)) {
			options.tip = tip.call(obj);
		} else if (we7.isObj(tip)) {
			options = $.extend(tip, options);
		} else {
			options.tip = tip;
		}
		if (we7.isFunc(options.tip)) {
			options.tip = options.tip.call(obj);
		}

		return options;
	}
	//添加实例方法：提示、气泡提示、状态栏提示
	$.extend(we7.fn, {
		tip: function (tip, options) {
			///<summary>为一个经 we7 包装成的对象按指定的参数提供提示信息</summary>
			///<param name="tip">提示的内容</param>
			///<param name="options">其他可选的参数（可直接将此参数放置在第一个位置，而将 tip 作为 options 的一个属性传递）</param>

			if (!this.jquery) { return; }
			options = ensureArgs(arguments, this.jquery);
			if (options) {
				tip = options.tip;      // 如果需要自定义提示内容（而不按照 title 指示）则应该在当前 we7().tip 方法里创建 tip 元素（就像上面的代码做的那样）
				if (tip) { delete options.tip }
			}

			var defaultClass = 'tooltip-follow';

			//指定默认选项属性
			options = $.extend({
				tipClass: defaultClass,
				cancelDefault: true,
				effect: 'fade',                 // use the fade effect instead of the default
				fadeOutSpeed: 100,              // make fadeOutSpeed similar to the browser's default
				predelay: 200,                  // the time before the tooltip is shown
				delay: 100,
				position: "center right",       // tweak the position
				offset: [0, 5]
			}, options);

			if (options.target) {
				options.tip = options.target;
				delete options.target;
			}

			this.jquery.each(function () {
				var jq = $(this), rtip, cancelDefault = options.cancelDefault, orgTitle = jq.attr("title");
				rtip = tip || orgTitle || "  ";        // 默认指定，否则会导致问题
				jq.attr("title", rtip);
				$(this).tooltip(options);
				if (cancelDefault) {
					jq.removeAttr("title");
				} else {
					jq.attr("title", orgTitle);
				}
			});

			return this;
		},
		status: function (tip, options) {
			var defaultClass = 'tooltip-status';
			options = $.extend({
				tipClass: defaultClass,
				cancelDefault: false,
				rePosition: false,
				layout: '<div><p rel="tipcontent"></p></div>'
			}, options);
			var ret = this.tip(tip, options), tip = this.jquery.data("tooltip");
			tip.onBeforeShow(function () {
				var t = this.getTip();
				t.css('left', Math.max(($(window).width() - t.width()) / 2, 0));
			});
			return ret;
		},
		popup: function (tip, options) {        // 由于 position 问题，此方法不兼容 IE6
			var wrapped = false, defaultClass = 'tooltip-popup';

			options = $.extend({
				tipClass: defaultClass,
				position: 'top center',
				offset: [10, 0],
				predelay: 150,
				delay: 250,
				effect: "slide",
				layout: '<div><div class="triangle triangle-top"></div><div class="popup-content" rel="tipcontent"></div><div class="triangle triangle-bottom"></div></div>'
			}, options);

			this.tip(tip, options);
			this.jquery.dynamic({ classNames: "t r popup-at-bottom l", bottom: { direction: 'down', bounce: true} });
			return this;
		},
		hint: function (tip, options) {
			var defaultClass = 'tooltip-hint';

			options = $.extend({
				tipClass: defaultClass,
				position: 'top center',
				offset: [-5, -40],
				fadeOutSpeed: 150,
				predelay: 350,
				effect: "fade",
				layout: '<div><div class="triangle triangle-top"></div><div class="hint-content" rel="tipcontent"></div><div class="triangle triangle-bottom"></div></div>'
			}, options);

			this.tip(tip, options);
			this.jquery.dynamic({ classNames: "t r hint-at-bottom l", bottom: { direction: 'down', bounce: true} });
			return this;
		},
		help: function (tip, options) {
			this.jquery.each(function () {
				var we7self = we7(this);
				var title, defaultClass = 'tooltip-help';
				if (title = we7self.jquery.attr('title')) {
					we7self.jquery.attr('title', '');
				}
				we7self.jquery = $('<acronym rel="we7_help">&nbsp;</acronym>').attr('title', title).addClass(defaultClass).insertAfter(we7self.jquery);
				options = $.extend({
					offset: [0, 10],
					tip: title,
					cancelDefault: true
				}, options);
				we7self.tip(tip, options);
			});
			return this;
		}
	});
	//添加实例方法：验证、附加验证器
	// TODO: 添加 效果 wall http://flowplayer.org/tools/demos/validator/custom-effect.html
	$.extend(we7.fn, {
		isInput: function () { return this.jquery ? this.jquery.is(":input") : false; },
		validate: function (func, options) {
			return this.getValidator(true, true, func, options).checkValidity();
		},
		customValidator: function (fn) {
			var dk = "validator-custom", el = this.jquery, o_fn = (el && el.data(dk)) || [];
			if (el) {
				if (we7.isFunc(o_fn)) {
					o_fn = [o_fn];
				}
				o_fn.push(fn);
				if (this.jquery.is(":input")) {
					el.data(dk, o_fn);
				} else {
					el.find(":input").each(function () {
						$(this).data(dk, o_fn);
					});
				}
			}
			this.getInputs(true);
			return this;
		},
		attachValidator: function (func, options) {
			if (this.validator && !func && !options) {
				return this;
			}

			var fn = we7.isFunc(func);
			if (!fn && we7.isObj(func)) {
				options = options ? $.extend(func, options) : func;
			}

			options = $.extend({
				effect: 'we7',
				errClass: 'validator-invalid',
				errMsgClass: 'validator-error',
				okMsgClass: 'validator-ok',
				offset: [0, 5],
				formEvent: null,
				errMsgAttr: 'errmsg', //错误提示属性
				okMsgAttr: 'okmsg'
			}, options);
			fn && !options.validator && (options.validator = func);
			if (we7.isFunc(options.validator)) { this.customValidator(options.validator); delete options.validator; }

			if (this.validator) {
				this.validator.destroy();
			}
			this.validator = this.jquery.validator(options).data("validator");
			return this;
		},
		getValidator: function (create, inputCreate, func, options) {
			var val = this.validator || (this.validator = this.jquery.data("validator"));
			if ((!val && create) && (inputCreate || !this.isInput())) {
				this.attachValidator(func, options);
			}
			return this.validator;
		},
		getInputs: function (validate, filter) {
			if (!this._inputs) {
				this._inputs = this.jquery.is(":input") ? this.jquery : this.jquery.find(":input");
				if (validate) { this._inputs.attr("we7-validate", "validate"); }
			}
			return filter ? this._inputs.filter(filter) : this._inputs;
		},
		validateRules: function (rules) {
			var r, inputs = this.getInputs();
			for (r in rules) {
				if (rules.hasOwnProperty(r)) {
					switch (r) {
						case "max":
						case "min":
						case "required":
							inputs.attr(r, rules[r]);
							break;
						case "type":
							inputs.attr("data-type", rules[r]);     // url、email、number
							break;
						case "pattern":
							inputs.attr("pattern", rules[r]);
							break;
						case "length":
							this.validateLength(rules[r]);
							break;
						default:
							we7.log && we7.log("Validate rule ignored:" + r);
							break;
					}
				}
			}
			return this;
		},
		validateMax: function (max) {
			this.getInputs().attr("max", max);
			return this;
		},
		validateMin: function (min) {
			this.getInputs().attr("min", min);
			return this;
		},
		validateEmail: function () {
			this.getInputs().attr("data-type", "email");
			return this;
		},
		validateUrl: function () {
			this.getInputs().attr("data-type", "url");
			return this;
		},
		validateNumber: function (min, max) {
			var ips = this.getInputs().attr("data-type", "number");
			if (!we7.isUndef(min)) {
				ips.attr("min", min);
			}
			if (!we7.isUndef(max)) {
				ips.attr("max", max);
			}
			return this;
		},
		validateRequired: function () {
			this.getInputs().attr("required", "required");
			return this;
		},
		validatePattern: function (pattern, msg) {        // msgfn = function(ok){ if(ok){} }
			if (we7.isStr(pattern)) {
				pattern = new RegExp("^" + pattern + "$");
			}
			this.getInputs().each(function () {
				var _s = we7(this);
				_s.customValidator(function (vctx) {
					var val = this.val(), ok = pattern.test(val);
					if (!msg) {
						var attr = vctx.getConf().messageAttr;
						if (attr) {
							msg = this.attr(attr);
						}
					}
					return ok ? true : (msg ? msg : "您的输入不符合规范");
				});
			});
			return this;
		},
		validateAjax: function (opt) {  //仅支持单个元素的 ajax 验证
			var opts, args = arguments;
			if (!we7.isFunc(opt) && we7.isObj(opt)) {
				opts = $.extend({}, opt);
			} else {
				opts = {
					url: args[0],
					callback: args[1],
					error: args[2],
					options: args[3]
				};
			}

			var fn = function (validator, onComplete) {
				// what is 'this'?
				var _url = we7.isFunc(opts.url) ? opts.url.call(this, validator) : opts.url;

				var el = this, options = {
					type: 'GET',
					url: _url,
					cache: false,
					error: function () {
						var tip = opts.error && opts.error.apply(el, arguments);
						onComplete && onComplete.call(el, (tip ? tip : "Network Error"), this);
					}
				};
				options = $.extend(options, opts.options);
				options.success = function () {
					onComplete && onComplete.call(el, opts.callback.apply(el, arguments), this);       // arguments 由 ajax 回调得来，此函数返回 true 表示验证通过；其他值表示错误
				};
				$.ajax(options);
			};
			this.getInputs(true).each(function () {
				$(this).data("validator-ajax", fn);
			});
			return this;
		},
		validateLength: function (min, max) {
			if (we7.isArr(min) && !max) {
				max = min[1];
				min = min[0];
			}
			var fn = function () {
				var msg = {
					min: '\u8bf7\u6309\u8981\u6c42\u8f93\u5165\u4e0d\u5c11\u4e8e ' + min + ' \u4e2a\u5b57\u7b26',
					max: '\u8bf7\u6309\u8981\u6c42\u8f93\u5165\u4e0d\u8d85\u8fc7 ' + max + ' \u4e2a\u5b57\u7b26',
					between: '\u8bf7\u6309\u8981\u6c42\u8f93\u5165 ' + min + ' \u81f3 ' + max + ' \u4e2a\u5b57\u7b26'
				};
				var el = this.val();
				if (!we7.isUndef(min) && !we7.isUndef(max) && (el.length < min || el.length > max)) {
					return msg.between;
				} else if (!we7.isUndef(min) && (el.length < min)) {
					return msg.min;
				} else if (!we7.isUndef(max) && el.length > max) {
					return msg.max;
				}
				return true;
			}
			return this.customValidator(fn);
		}
	});
	we7.extend({
		pickDate: function (afterPick, interval) {  // afterPick, interval/options
			var opts;
			if (we7.isObj(afterPick) && we7.isUndef(interval)) {
				opts = afterPick;
			} else if (we7.isFunc(afterPick)) {
				opts = {};
				opts.onClose = afterPick;		// function(dateText, instance){this = input}
				opts.interval = interval;
			}

			var options = $.extend({
				interval: ['-50y', '+50y'], 	// min-date and max-date
				dateFormat: 'yy-mm-dd',
				showOtherMonths: true,
				defaultDate: 0
			}, opts);
			this.jquery.datepicker(options);
		}
	});
	//添加静态方法：验证、状态栏提示
	$.extend(we7, {
		/* Tip and Validator Utils */
		status: function (tip, options) {            // 立即在状态栏提示
			var u = $("a[we7status]");
			if (!u.length) {
				$('<a we7status="we7status" />').appendTo(document.body).hide();
				u = $("a[we7status]");
			}
			u = we7(u);
			var t = u.jquery.data("tooltip");
			if (t) {
				t.destroy();
			}
			options = $.extend({
				cancelDefault: true,
				autoHide: true,
				events: {
					def: "nothing,nothing"
				},
				onShow: function () {
					var t = this, c = this.getConf();
					if (c.autoHide) {
						setTimeout(function () {
							t.hide();
							t = null;
						}, c.hideTimeout || 4500);
					}
				}
			}, options);
			u.status(tip, options);
			t = u.jquery.data("tooltip");
			t.show();
		},
		loading: function (tip, options) {
			if (we7.isUndef(tip)) {
				tip = "正在加载...";
			}
			tip = '<img src="/scripts/we7/css/res/ajax-loading.gif" style="width:16px;height:16px;margin-top:5px;"/><div style="padding-left:25px;margin-top:-20px;">' + tip + '</div>';
			options = $.extend({ autoHide: false }, options);
			return we7.status(tip, options);
		},
		info: function (tip, options) {
			tip = '<img src="/admin/images/ico_info.gif" style="width:16px;height:16px;margin-top:5px;" /><div style="padding-left:25px;margin-top:-20px;">' + tip + '</div>';
			return we7.status(tip, options);
		},
		validate: function (target, func, options) {
			return we7(target).validate(func, options);
		},
		addValidateType: function (matcher, message, fn) {
			$.tools.validator.fn(matcher, message, fn);
		}
	});

	$(document).ready(function () {
		$("form").attr("novalidate", "novalidate");
	});

})(window.we7);

// we7 validator 插件
(function(we7){
    var attachValidator_org = we7.fn.attachValidator;
    we7.fn.attachValidator = function(func,options){
        attachValidator_org.call(this,func,options);
        var el=this.jquery, loadClass="validator-ajax-loading";
        this.validator.onAjaxRequest(function(e, input){
            var msg = input.data("msg.el");
            if(msg){
                msg.remove();
                input.removeData("msg.el");
            }
            var tip = input.data("tooltip"), orgTip=!!tip, 
                content='Loading...'; 
            if(!orgTip){
                we7(input).tip(content,{
                    tipClass:loadClass,
                    offset:[0, 5],
                    events:{
                        input:"nothing,nothing"
                    }
                });
                input.data("vo-tooltip",true);
                tip = input.data("tooltip");
                tip.show();
            }else{
                var nowDataLoading = tip.dLoading;
                if(typeof nowDataLoading === "undefined"){
                    tip.onBeforeShow(function(){
                         var t = this.getTip(), lT = this.dLoading;
                         if(lT){
                            t.removeClass(this.getConf().tipClass).addClass(loadClass);
                         }else{
                            t.removeClass(loadClass).addClass(this.getConf().tipClass);
                         }
                         this.dLoading = false;
                    });
                }
                tip.dLoading = true;
                tip.show(content);
            }
        }).onAjaxResponse(function(e,input){
            var tip = input.data("tooltip"), newTip = input.data("vo-tooltip");
            tip.hide();
            if(newTip){
                input.removeData("vo-tooltip");
                tip.destroy();
            }
        });
        return this;
    }
	$.extend(we7,{
		validateTrigger:{
			blurAjax:{					//  blur 与 ajax 联合验证
				inputEvent:'blur',
				ajaxOnSoft:true,
				errorInputEvent:null
			},
			blur:{
				inputEvent:'blur',
				errorInputEvent:null	
			},
			keyup:{
				inputEvent:'keyup',
				errorInputEvent:null
			},
			keyupAjax:{
				inputEvent:'keyup',
				errorInputEvent:null,
				ajaxOnSoft:true
			}
		}
	});
})(window.we7);


//    $("#create-folder").validator({
//        onBeforeValidate: function (e, els) {
//            alert('validatoring');
//        },
//        position: 'top right',
//        offset: [10, 0],
//        message: '<div>真错了</div>'
//    });


    
/*
注入 CSS
var css = $('testframe').contentWindow.document.createElement('style');
css.type = 'text/css';
css.innerHTML = 'body {font-size: 7px; color: red;}';
$('testframe').contentWindow.document.getElementsByTagName('head')[0].appendChild(css);

*/
