/*
    脚本依赖：jQuery-ui, we7.core, we7.load
	// http://jqueryui.com/demos/dialog/#options
*/
/*
TODO: form 显示时自动定位表单；如果立即触发 blur（如关闭 form）会引发验证而显示出验证出错的提示（请在关闭之前检查所有 form，并将其验证器 reset 掉）
*/
(function () {
	function we7form(elem, options) {
		var self = this, $dialog, uiDialog, we7formOptions, oop = $.extend({}, options);

		function getOption(name, defaultValue, bKeep) {
			var val = oop[name];
			if (!bKeep) {
				delete options[name];
			}
			return val === undefined ? defaultValue : val;
		}

		we7formOptions = {			// 自有参数[先剥离出来，防止被覆盖]
			overlay: getOption("overlay", false),
			autoOpen: getOption("autoOpen", false),
			showTitleBar: getOption("showTitleBar", true),
			autoClose: getOption("autoClose", false),
			closeOnClick: getOption("closeOnClick", true),
			cssClass: getOption("cssClass", null),
			fixed: getOption("fixed", true)
			// ,show :      //TODO: add some effects to show and hide/close
			// ,hide:
		};

		options = $.extend(options, {
			closeText: '关闭',
			modal: we7formOptions.overlay !== false,
			closeOnEscape: we7formOptions.overlay !== false,
			dialogClass: we7formOptions.cssClass,
			autoOpen: false
		});

		if (options.size) {
			!we7.isUndef(options.size.width) && (options.width = options.size.width);
			!we7.isUndef(options.size.height) && (options.height = options.size.height);
		}
		this.element = elem;
		$dialog = this.element.dialog(options).data("dialog");      // ui-dialog 对象
		uiDialog = $dialog.uiDialog;                                // ui-dialog DOM 元素
		function dialogCmd(cmd, args) {
			Array.prototype.unshift.call(args, cmd);
			return $dialog[cmd].apply($dialog, args);
		}
		function opt(key, value) {
			if (we7.isObj(key)) {
				return opts(key);
			}

			if (we7.isUndef(value)) {
				return we7.isUndef(we7formOptions[key]) ? $dialog.options[key] : we7formOptions[key];
			}
			if (key in we7formOptions) {
				we7formOptions[key] = value;
			}
			dialogCmd('option', [key, value]);
			return self;
		}
		function opts(ops) {
			$.each(ops, function (key, value) {
				opt(key, value);
			});
		}

		$.extend(this, {
			option: opt,
			show: function () { return dialogCmd.call(null, 'open', arguments) }
		});
		$.each('close,disable,widget,enable,isOpen,moveToTop,open,destroy'.split(','), function (i, met) {
			var method = this;
			self[met] = function () {
				return dialogCmd.call(null, met, arguments);
			};
		});
		$.each("create,beforeClose,open,focus,dragStart,drag,dragStop,resizeStart,resize,resizeStop,close".split(','), function (i, ev) {
			var dEvent = ev, event = 'on' + dEvent.charAt(0).toUpperCase() + dEvent.substr(1);
			self[event] = function (func) {
				self.element.bind('dialog' + dEvent, func);
				return self;
			};
		});
		self.onShow = self.onOpen;
		self.isShown = self.isOpen;
		self.position = function (pos) {
			return opt('position', pos);
		};
		self.getOptions = function () {
			return $dialog.options;
		}
		if (we7formOptions.fixed) {
			uiDialog.css("position", "fixed");
		}
		if (!we7formOptions.showTitleBar) {
			self.onShow(function () {
				$dialog.uiDialogTitlebar.hide();
			});
		}
		var _clickClose;
		if (we7formOptions.closeOnClick) {
			self.onShow(function () {
				if (!_clickClose && $dialog.overlay) {
					_clickClose = true;
					$dialog.overlay.$el.click(function () {
						self.close();
					});
				}
			});
		}

		if (we7formOptions.autoOpen) {
			this.show();
		}
		if ($dialog.overlay && typeof options.overlay !== "boolean") {
			$dialog.overlay.$el.css("opacity", options.overlay);
		}
		var closeHwnd;
		function launchClose() {
			closeHwnd = setTimeout(function () {
				self.close();
				closeHwnd = undefined;
			}, 3500);
		}
		function cancelClose() {
			if (closeHwnd) {
				try { clearTimeout(closeHwnd) } catch (ex) { }
			}
		}
		if (we7formOptions.autoClose) {
			uiDialog.mousemove(cancelClose).mouseout(launchClose);
			launchClose();
		}

		elem.data("we7.form", this);
		return this;
	}

	we7.extend({ "form": function (options) {               // return this ? return form ?  form returned
		return new we7form(this.jquery, options);
	}
	});

	we7.form = function (source, options) {
		// source is selector, image, html or url

		var urlRegex = /^(https?:\/)?\/.+/i, imgRegex = /(jpg|bmp|png|gif|ico)$/i;
		var _sTindex = source.indexOf(';'), _sHead;
		if (_sTindex < 0) {
			if (urlRegex.test(source)) {
				if (imgRegex.test(source)) {
					_sHead = "image";
				} else {
					_sHead = "url";
				}
			} else {
				_sHead = "selector";
			}
		} else {
			_sHead = _sTindex === -1 ? "selector" : source.substr(0, _sTindex).toLowerCase();
			if (_sHead != "selector") {
				source = source.substr(_sTindex + 1);
			}
		}

		var elem,
			basicLayout = '<div title="loading" style="overflow:hidden;width:100%;height:100%"><img src="/scripts/we7/css/res/ajax-loading.gif" alt="loading" />&nbsp;&nbsp;请稍后...</div>';
		errorLoyout = '<div title="error" style="overflow:hidden;width:100%;height:100%"><img src="" alt="error" />&nbsp;&nbsp;很抱歉，载入出错...请&nbsp;&nbsp;<a>重试</a></div>';
		/*
		options.iframe : false,
		options.scrolling : false,
		// options.autoResize
		*/
		!options && (options = {});
		function trigger(ev, obj, args) {
			if (!ev) { return }
			if (we7.isArr(ev)) {
				$.each(ev, function (i, e) {
					e.apply(obj, args);
				});
			} else {
				ev.apply(obj, args);
			}
		}
		function addEvent(ev, ok) {
			var e = ok ? okCallback : cancelCallBack;
			if (we7.isFunc(e)) {
				e = [e];
			} else {
				e = e || [];
			}
			e.push(ev);
		}
		var iframe, img, loaded = false, loadingTimer;
		function completeReady(ajaxData) {
			if (loaded) { return; }
			loaded = true;
			if (iframe) {
				elem.empty().append(iframe.show());
			} else if (img) {
				elem.empty().append($(img).css({
					"width": "100%",
					"height": "100%"
				}));
			} else {
				elem.empty().append(ajaxData);
			}
		}
		function loadError() {
			if (loaded) { return; }
			elem.html(errorLoyout);
			$('a', elem).click(loadNow);
		}
		function loadNow() {
			if (_sHead != "selector") {
				elem = $(basicLayout);
			}
			if (loadingTimer) { try { clearTimeout(loadingTimer); loadingTimer = undefined; } catch (ex) { } }
			switch (_sHead) {
				case "image":
					img = new Image();
					img.src = source;
					if (img.complete) {
						completeReady();
					} else {
						img.onload = function () {
							img.onload = null;
							completeReady();
						};
						img.onerror = img.onabort = loadError;
					}
					break;
				case "url":
					if (options.iframe) {
						iframe = $('<iframe />').attr({
							"frameBorder": 0,
							"allowTransparency": "true",
							"name": "wf_" + Number(new Date()), // give the iframe a unique name to prevent caching
							"scrolling": options.scrolling ? "yes" : "no",
							"rel": "we7form_iframe",
							"width": "100%",
							"height": "100%"
						});
						iframe.one('load', completeReady).attr("src", source).hide().appendTo(document.body);
						setTimeout(loadError, 15 * 1000); 	// 超时时间
					} else {
						$.ajax({
							url: source,
							type: 'GET',
							timeout: 15 * 1000,
							success: function (data) {
								completeReady(data);
							},
							error: loadError
						});
					}
					break;
				default:        // html、selector
					elem = $(source);
					break;
			}
		}
		loadNow();
		var form = we7(elem).form(options);

		return form;
	};
	we7.overlay = function (source, options) {
		options = $.extend({
			closeOnEscape: true,
			closeOnClick: true,
			overlay: true,
			showTitleBar: false,
			position: "center",
			draggable: true,
			autoOpen: true,
			resize: function () {
				$(this).dialog('option', 'position', 'center');
			}
		}, options);
		return we7.form(source, options);
	}
	we7.confirm = function (text, title, options) {
		!options && (options = {});
		var customPosition = options && !!(options.position || options.containerArea),
	    layout = '<div style="display:none; margin:0 auto;text-align:center" title="{0}"><div>{1}</div></div>';
		we7.isObj(title) && (options = title, title = undefined);
		var form, okCallback = options.ok, cancelCallBack = options.cancel;

		function trigger(ev, obj, args) {
			if (!ev) { return }
			if (we7.isArr(ev)) {
				$.each(ev, function (i, e) {
					e.apply(obj, args);
				});
			} else {
				ev.apply(obj, args);
			}
		}
		function addEvent(ev, ok) {
			var e = ok ? okCallback : cancelCallBack;
			if (we7.isFunc(e)) {
				e = [e];
			} else {
				e = e || [];
			}
			e.push(ev);
			if (ok) {
				okCallback = e;
			} else {
				cancelCallBack = e;
			}
		}

		options = $.extend({
			autoTip:true,
			overlay: true,
			autoOpen: true,
			showTitleBar: true,
			closeOnClick: true,
			closeOnEscape: true,
			resizable: false,
			buttons: {
				"确定": function () {
					form._ok = true;
					$(this).dialog("close");
				},
				"取消": function () {
					$(this).dialog("close");
				}
			}
		}, options);

		layout = we7.formatStr(layout, title || options.title || text, options.autoTip ? '您确定要 ' + text + ' 吗？' : text);

		form = we7(layout).form(options);
		$.extend(form, {
			ok: function (fn) {
				addEvent(fn, true);
			},
			cancel: function (fn) {
				addEvent(fn, false);
			}
		});
		form.onClose(function () {
			var e = form._ok ? okCallback : cancelCallBack;
			try { delete form._ok } catch (ex) { }
			trigger(e, form, []);
		});

		return form;
	};
	we7.alert = function (text, title, options) {
		var layout = '<div style="display:none;margin:0 auto;text-align:center" title="{0}"><div>{1}</div></div>';
		we7.isObj(title) && (options = title, title = undefined);
		options = $.extend({
			autoClose: true, 	// 是否自动关闭
			autoOpen: true,
			showTitleBar: true,
			closeOnEscape: true,
			resizable: false,
			buttons: {
				"确定": function () {
					$(this).dialog('close');
				}
			},
			overlay: false
		}, options);

		layout = we7.formatStr(layout, title || options.title || text, text);

		return we7(layout).form(options);
	};
})();
