/// <reference path="../jQuery/jquery-1.4.2.js"/>

/* we7.core.js 依赖 jQuery */
(function ($, win_we7) {
	var isArray = function (a) { return a && typeof a == "object" && typeof a.length == "number" && typeof a.splice == "function" && !a.propertyIsEnumerable("length") };
	var we7 = function (obj) {
		if (typeof obj === "undefined" || typeof obj === null) {
			return null;
		}
		return new we7.fn.init(obj);
	};
	$.extend(we7, {
		we7: 2011112209,
		isArr: isArray,
		isArray: isArray,
		isUndef: function (a) { return typeof a === "undefined" },
		isNull: function (a) { return typeof a === "object" && !a },
		isN: function (a) { return typeof a === "number" && window.isFinite(a) },
		isFunc: function (a) { return typeof a === "function" },
		isObj: function (a) { var t = typeof a; return a && t === "object" || t === "function" },
		isStr: function (a) { return typeof a === "string" }
	});
	function format(a) { if (!a) return undefined; var b, c, d, e = arguments.length - 1, f = new String(a); for (b = 0; b < e; b++) c = new RegExp("{[" + b + "]}", "g"), f = f.replace(c, arguments[b + 1]); return f }
	String.prototype.trim = function () { var rnotwhite = /\S/, trimLeft = /^\s+/, trimRight = /\s+$/; if (rnotwhite.test("\xA0")) { trimLeft = /^[\s\xA0]+/; trimRight = /[\s\xA0]+$/; } return this.replace(trimLeft, "").replace(trimRight, "") }

	$.extend(we7, {
		indexOfArray: function (arr, elem) {
			if (!arr || !arr.length) { return -1; }
			var fn = we7.isFunc(elem);
			if (fn || (we7.isUndef(Array.prototype.indexOf))) {
				for (var i = 0; i < arr.length; i++) {
					if ((fn && elem.call(arr[i], i) === true) || arr[i] === elem) {
						return i;
					}
				}
				return -1;
			} else {
				return arr.indexOf(elem);
			}
		},
		findInArray: function (arr, fn, i) {
			if (!we7.isFunc(fn) || !arr || !arr.length) return null; var b, c = i || 0; for (; c < arr.length; c++) if (fn.call(arr, b = arr[c])) return b
		}
	});

	var _nu = window.navigator.userAgent;
	var _b = {
		userAgent: _nu,
		msie: false,
		ff: (/firefox/i).test(_nu),
		chrome: (/chrome/i).test(_nu),
		opera: (/opera/i).test(_nu),
		ie: 0
	};
	(function () {
		var a = /msie\s([\d\.]+)/i.exec(_nu);
		_b.msie = (a !== null);
		_b.ie = a === null ? !1 : parseFloat(a[1]);
		_b.firefox = _b.ff;
	})();
	we7.browser = _b;
	we7.log = function log(txt, source) {
		var occurred = new Date();
		txt = occurred.getHours() + ':' + occurred.getMinutes() + ':' + occurred.getSeconds() + '\t' + txt;
		if (window.log) {
			window.log(txt);
		}
		if (window.console && window.console.log) {
			window.console.log(txt);
		}
	};
	we7.formatStr = function (a) { return format.apply(null, arguments) }
	we7.trimStr = function (text) { var s = text; return s.trim() };

	we7.fn = we7.prototype = {
		init: function (o) {
			this.original = o || {};
			if ((we7.isStr(o)) || (!this.jquery && o.nodeType)) {
				this.jquery = $(o);
			} else {    // o is a jQuery object
				this.jquery = we7.isStr(o.jquery) ? o : (o.jquery || null);
			}
		},
		clone: function () {
			var obj = this.original;
			var objClone;
			if (obj.constructor == Object) {
				objClone = new obj.constructor();
			} else {
				objClone = new obj.constructor(obj.valueOf());
			}
			for (var key in obj) {
				if (objClone[key] != obj[key]) {
					if (typeof (obj[key]) == 'object') {
						objClone[key] = we7(obj[key]).clone();
					} else {
						objClone[key] = obj[key];
					}
				}
			}
			objClone.toString = obj.toString;
			objClone.valueOf = obj.valueOf;
			return objClone;
		},
		rename: function (memberName, newMemberName) {
			var self = this, isObj = we7.isUndef(newMemberName) && we7.isObj(memberName);

			function replaceName(n, nn) {
				var o = self.original[n];
				if (o) {
					self.original[newMemberName] = we7(o).clone();
				}
			}

			if (isObj) {
				$.each(memberName, function (name) {
					replaceName(name, this);
				});
			} else {
				replaceName(memberName, newMemberName);
			}
			return self.original;
		}
	};
	we7.fn.init.prototype = we7.fn;
	we7.extend = function (ext) {       // 将成员绑定到实例对象上
		$.extend(we7.prototype, ext);
		return we7;
	};
	we7.extend({
		we7: we7.we7,
		extend: function (ext) {
			$.extend(this, ext);
			return this;
		}
	});
	$.extend(we7, {
		beforeUnload: function (message, func) {
			window.onbeforeunload = function () {
				if (we7.isFunc(func) && func.call() != false) return message
				else if (we7.isStr(func)) return func;
			}
		},
		removeBeforeUnload: function (func) {
			window.onbeforeunload = null;
			if (we7.isFunc(func)) func.call();
		},
		queryString: function (loc, key) {
			if (loc && loc.indexOf('/') === -1) {
				key = loc;
				loc = undefined;
			}
			loc = loc || window.location;
			var p = (function parseQueryParams(location) {
				var search = typeof location.search !== "string" ? (location.indexOf('?') < 0 ? "" : location.substr(location.indexOf('?'))) : location.search;
				var ic = 0, params = {};
				var query = unescape(search.substr(1));
				var vars = query.split("&");
				for (var i = 0; i < vars.length; i++) {
					var pair = vars[i].split("=");
					if (pair[0]) {
						ic++;
						params[pair[0]] = pair[1];
					}
				}
				return ic > 0 ? params : null;
			})(loc);
			return key ? p && p[key] : p;
		}
	});
	$.extend(we7, win_we7);
	window.we7 = we7;
	if (window.we7loader) {
		window.we7.load = window.we7loader;
	}
})(jQuery, window.we7);


/****************************************************/


/*
we7 loader
Intergrated with we7.core[always the newest version]
*/
(function () {
	var version = (we7 && we7.we7) || 2011112209, basePath = '/scripts/', jsConfigUrl = '/scripts/we7/_ResourceSets.js',
    preventCache = true, scriptRel = 'ref', scriptConfig = 'config', debugMode = false;

	var log = function log(txt, source) {
		if (!debugMode) { return; }
		var occurred = new Date();
		txt = occurred.getHours() + ':' + occurred.getMinutes() + ':' + occurred.getSeconds() + '\t' + txt;
		if (window.console && window.console.log) {
			window.console.log(txt);
		}
	}
    , innerResource = {
		jQueryUI:[
				{ name: 'jqueryui', src: '/scripts/jquery/jQueryUI/jquery-ui.js'},
                { name: '_jquicss', src: '/scripts/jQuery/jQueryUI/css/jquery-ui.css' }
		]
    	,we7tip: [
                { name: 'jqtools', src: basePath + 'we7/jquery.tools.min.js'},
                { name: 'we7tools', src: basePath + 'we7/we7.tools.js', need: 'jqtools' },
				{ name: '_tip_css', src: basePath + 'we7/css/we7.tip.css' }
            //, { name: 'jquery', src: basePath + 'jquery/jquery-1.4.2.min.js'} // basePath + jquery.js     // 永远把 jQuery 置于最后一个，程序会自动判断是否已载入
         ]
        , we7form: [
                { name: 'we7form', src: basePath + 'we7/we7.form.js', need: "jqueryui" }
        ]
        , we7bind: [		/* 需要在已入的文件中作此更改（添加） */
			{ name: 'jgrid_css', src: '/scripts/we7/css/ui.jqgrid.css' },
            { name: 'jgrid_lang', src: '/scripts/we7/i18n/grid.locale-cn.js' },
			{ name: 'jgrid', src: '/scripts/we7/jquery.jqGrid.min.js', need: 'jgrid_lang' },
			{ name: 'we7bind', src: basePath + 'we7/we7.bind.js', need: "jgrid" }
        ]
    };

	// { name: 'we7', src: basePath + 'we7/we7.core.js', need: 'jquery' }, // we7.core 已整合到当前脚本文件中
	/*
	<script type="text/javascript" src="/scripts/we7/jquery.tools.min.js"></script>
	<script type="text/javascript" src="/scripts/we7/we7.loader.js"></script>
	<script type="text/javascript" src="/scripts/we7/we7.tools.js"></script>
	<script type="text/javascript" src="/scripts/we7/we7.form.js"></script>
	*/


//	var jQuery_Loaded = false;          // TODO: add some fondation methods to check if one resource has been loaded already
//	if (jQuery && jQuery.prototype.jquery && typeof (jQuery.prototype.selector) !== "undefined") {
//		jQuery_Loaded = true;           // jQuery 文档已引用jQuery
//		innerResource.we7tip.splice(innerResource.we7tip.length - 1, 1);
//		log("jQuery has already been imported to this document");
//	}

	if (jQuery && typeof jQuery.ui !== "undefined") {  // jQuery UI has been loaded
		delete innerResource.jQueryUI;
		log("jQuery UI has already been imported to this document");
	}

	var loadInner, scriptHref = (function () {		// 获取当前脚本文件的引用 URL（在页面中的 SRC），用于在页面上寻找定位到当前脚本块
		var scripts = document.getElementsByTagName('SCRIPT');
		var script = scripts[scripts.length - 1];
		loadInner = script.getAttribute("inner");
		return script.src;
	})();

	function ajaxCreate() {
		var xhrObj = false;
		try {
			xhrObj = new XMLHttpRequest();
		} catch (e) {
			var aTypes = ["Msxml2.XMLHTTP.6.0",
            			  "Msxml2.XMLHTTP.3.0",
						  "Msxml2.XMLHTTP",
						  "Microsoft.XMLHTTP"];
			var len = aTypes.length;
			for (var i = 0; i < len; i++) {
				try {
					xhrObj = new ActiveXObject(aTypes[i]);
				}
				catch (e) {
					continue;
				}
				break;
			}
		}
		finally {
			if (!xhrObj) {
				var e = 'Error: this browser does not support XmlHttpRequest';
				log(e);
				throw new Error(e);
			}
			return xhrObj;
		}
	}
	function addParam(url, name, value) {
		name = ((url.indexOf('?') > -1) ? '&' : '?') + encodeURIComponent(name) + '=' + encodeURIComponent(value);
		var index = url.indexOf('#');
		index = (index === -1) ? url.length : index;
		url = url.substr(0, index) + name + url.substr(index);
		return url;
	}
	function injectDom(txt, bKeep) {
		var se = document.createElement('SCRIPT'), head = document.getElementsByTagName('head')[0];
		se.setAttribute("type", "text/javascript");
		se.setAttribute("injected", Number(new Date));
		head.appendChild(se);
		se.text = txt;
		if (!bKeep) {
			setTimeout(function () {                        // NOTE: 一段时间之后在 DOM 中去除这个 Script 标记有利于提高之后 DOM 检索的效率，减少浏览器内存占用
				head.removeChild(se);
			}, 1500);
		}
	}
	function addStyle(style) {
		var fileref = document.createElement("link");       // NOTE: CSS载入并未考虑依赖关系，而仅只是动态添加到页面 Head 中
		fileref.setAttribute("rel", "stylesheet");
		fileref.setAttribute("type", "text/css");
		fileref.setAttribute("href", style);
		document.getElementsByTagName('head')[0].appendChild(fileref);
	}
	function addEvent(ev, e, bTrigger, bDelete) {
		if (!e) { return; }
		if (ev) {
			ev.push(e);
		} else {
			ev = [e];
		}
		if (bTrigger) {
			return triggerEvent(ev, bDelete);
		}
		return ev;
	}
	function triggerEvent(ev, bDelete) {
		if (!ev) { return; }
		var len = ev.length, e;
		for (var i = 0; i < len; i++) {
			e = ev[i];
			if (typeof e === "string") {
				injectDom(e, true);
			} else if (e) {
				e.call(window);
			}
			if (bDelete) {
				ev[i] = undefined;
			}
		}
		if (bDelete) {
			ev.splice(0, ev.length);
		}
	}

	function Context() {
		this.q = [];
		this.injectScripts = function () {
			var len = this.q.length;
			log('Injecting scripts with ' + len + ' length of queue');
			for (var i = 0; i < len; i++) {
				var qScript = this.q[i];
				if (!qScript.scriptText) {
					// 等待其他请求完成
					log('Script ' + qScript.name + ' is not ready yet when injecting scripts queue');
					break;
				}
				else if (qScript.status < 2) {
					qScript.status = 2;
					injectDom(qScript.scriptText);

					log(qScript.name + ' is now loaded by queue');
					if (qScript.onComplete) {
						qScript.onComplete.call(qScript);
					}
				}
			}
		}
	}
	function Script(name, src, need, context, onComplete, onError) {
		name = name || 'script_' + (new Number(new Date()));
		var tNeed = (typeof need);
		switch (tNeed) {
			case "string":
				need = need.replace(' ', '').split(',');
				break;
			case "object":
				if (!need) {
					need = [];
				}
				break;
			default:
				need = [];
		}

		this.name = name;
		this.src = src;
		this.need = need || [];
		this.index = -1;
		this.context = context;
		this.onComplete = onComplete;
		this.onError = onError;
		this.scriptText = null;

		this.status = 0;        // -1： 发生错误     0: 初始化   1: 正在载入    2: 载入完成

		if (this.src && preventCache) {
			this.src = addParam(this.src, '_we7ver', version);
		}

		this.load = function (onC, onE) {
			if (onC) {
				this.onComplete = onC;
			}
			if (onE) {
				this.onError = onE;
			}
			var script = this, xhrObj = ajaxCreate();

			if (this.context) {
				this.index = this.context.q.length;
				this.context.q.push(this);
			}

			this.status = 1;
			xhrObj.onreadystatechange = function () {
				if (xhrObj.readyState === 4) {
					if (xhrObj.status < 200 || xhrObj.status >= 400) {
						log('Error when loading script ' + script.name + ', server returned a bad status code: ' + xhrObj.status);
						script.status = -1;
						if (script.context) {
							script.context.q[script.index].status = -1;
						}
						if (script.onError) {
							script.onError.call(script);
						}
					} else {
						if (script.context) {
							script.context.q[script.index].scriptText = xhrObj.responseText;
							script.context.injectScripts();
						}
						else {
							script.status = 2;
							injectDom(xhrObj.responseText);
							log(script.name + ' is now loaded');
							if (script.onComplete) {
								script.onComplete.call(script);
							}
						}
					}
				}
			};
			xhrObj.open('GET', this.src, true);
			xhrObj.send(null);
		};
	};
	var Manager = {
		had: [],
		add: function (script) {
			Manager.had.push(script);
		},
		get: function (script) {
			var name = typeof script === 'string' ? script : script.name;
			var hadL = Manager.had.length;
			for (var i = 0; i < hadL; i++) {
				if (Manager.had[i].name == name)
					return Manager.had[i];
			}
			return null;
		}
	}
//	if (jQuery_Loaded) {
//		jQuery_Loaded = new Script("jquery", "jquery.js");
//		jQuery_Loaded.status = 2;
//		Manager.add(jQuery_Loaded);
//	}
	function loadResourceSets(task) {
		var sets = task.tasks, result = [];

		function resoureceSetReady() {
			var ts, go = true, suc = true;

			function getScriptIndexOf(scr) {
				for (var i = 0; i < result.length; i++) {
					if (result[i].name === scr.name) {
						return i;
					}
				}
				return -1;
			}

			var index = getScriptIndexOf(this);
			if (index > -1) {
				result[index].status = this.status;
			}

			for (var i = 0; i < result.length; i++) {
				ts = result[i];
				if (ts.status > -1 && ts.status < 2) {
					go = false;
					break;
				} else if (ts.status === -1) {
					suc = false;
				}
			}
			if (go) {
				task.isReady = true;
				for (var i = 0; i < result.length; i++) {
					Manager.add(result[i]);
				}
				log('All scripts are now loaded');
				if (suc) {
					task.successed = true;
					log('All scripts are now loaded successfully');
				}
				triggerEvent(task.onReady, true);
				if (suc) {
					triggerEvent(task.onSuccess, true);
				}
				if (typeof this.index !== "undefined") {
					try { delete this.context; } catch (err) { }
				}
				delete result;
			}
		}
		function findScript(script, index) {
			if (typeof script === 'object') {
				return script;
			}
			if (!index) {
				index = sets.length;
			}

			for (var i = 0; i < index; i++) {
				if (sets[i].name === script)
					return sets[i];
			}
			return false;
		}
		function needNotContained(needs) {
			var need, notContained = [];
			for (var i = 0; i < needs.length; i++) {
				need = needs[i];
				if (!findScript(need) && !Manager.get(need)) {
					needs.splice(i, 1);
					notContained.push(need);
				}
			}
			return notContained.length ? notContained : true;       // true 表示无遗漏
		}
		function isNeeded(left, right, nested) {
			var notRight = false;
			if (result.length === 1) {
				return false;
			}
			left = findScript(left);
			if (!right) {
				notRight = true;
				right = new Script('_Global' + Math.random(), '', result);
			} else {
				right = findScript(right);
			}

			if (left.name === right.name) {
				return false;
			}

			var l = right.need ? right.need.length : 0, needNow;

			for (var ni = 0; ni < l; ni++) {
				needNow = right.need[ni];
				if (needNow.name === left.name) {       // need left<如果右边是此过程创造的，则不允许直接相等，仅允许其子集相等>
					if (!notRight) {
						return true;
					}
				} else if (!nested) {
					if (isNeeded(left, needNow, notRight ? true : false)) {                 // need's need need left
						return true;
					}
				}
			}
			return false;
		}
		function contrast(left, right) {
			var sR = 0, li = left.need.length, ri = right.need.length;
			if (!li && !ri) {
				return 0;
			} else {
				var L_R = false, R_L = false;       // 左边依赖右边；右边依赖左边
				L_R = isNeeded(left, right);
				R_L = isNeeded(right, left);

				if (L_R && R_L) {
					throw new Error('Can not load scripts which require each other');
				}

				if (L_R) {
					sR = -1;            // 右边依赖左边，则 L < R <
				} else if (R_L) {
					sR = 1;             // 左边依赖右边，则 R < L <
				} else if (!li) {
					sR = -1;            // 左边没有依赖，则左边提前 L < R <
				} else if (!ri) {
					sR = 1;             // 右边没有依赖，则右边提前 R < L <
				} else {                //  都没有依赖
					return 0;
				}
			}

			return sR;
		}

		var script, need, nl, needInResult, bStyle;
		for (var i = 0; i < sets.length; i++) {
			script = sets[i];
			var needLack, li = script.src.lastIndexOf('.');
			bStyle = (li > -1) ? script.src.substr(li).substr(0,4) === '.css' : false;
			if (!bStyle) {
				script = (typeof script.scriptText !== "undefined") ? script : new Script(script.name, script.src, script.need);
				if (!Manager.get(script)) {
					need = script.need;
					if (need.length) {
						needLack = needNotContained(need);
						if (needLack !== true) {
							log('Some required scripts (' + needLack.join(',') + ') can not be found for script "' + script.name + '", it may not work properly');
						}
					}
					for (var _n = 0; _n < need.length; _n++) {
						script.need[_n] = findScript(need[_n]) || Manager.get(need[_n]);
					}

					result.push(script);
				}
			}
			else {
				addStyle(script.src);
			}
		}
		if (!result.length) {
			resoureceSetReady.call(null);
			return;
		}
		result.sort(contrast);

		var resultNow, context = new Context();

		for (var i = 0; i < result.length; i++) {
			resultNow = result[i];
			if (resultNow.need.length || isNeeded(resultNow)) {
				resultNow.context = context;
			}
		}

		for (var i = 0; i < result.length; i++) {
			resultNow = result[i];
			resultNow.load(resoureceSetReady, function () {
				triggerEvent(task.onError, true);
			});
		}
	}
	var jsConfigUrlLoaded = false;
	function LoadTask(script, onReady, onError, resConfig) {
		if(!script){return null;}
		var  httpUrl = /^(https?:\/\/.+)?\/.*\.js(\?.*)?/;
		var viaRes = false, task = this;
		
		function boxScript(s){
			if(typeof s.context === "undefined"){
				try{
					s = new Script(s.name, s.src, s.need);
				}catch(x){}
			}
			return s;
		}

		if (typeof script === "string" ){
			if(httpUrl.test(script)){
				script = new Script('we7_script_' + Number(new Date()), script);
			}else{
				viaRes = true;
			}
		}else if(typeof script === "object"){
			if(typeof script.length === "undefined"){	// 不要是数组
				script = boxScript(script);
			}else{
				 for(var _i=0;_i<script.length;_i++){
					script[_i] = boxScript(script[_i]);
				 }
			}
		}
	
		this.isReady = false;
		this.successed = false;
		this.onReady = addEvent(null, onReady); 		// 		当完成时（而不管是否均已成功）
		this.onSuccess = null; 							//		当成功时 （要求所有的均已成功）
		this.onError = addEvent(null, onError); 		//		每次发生错误时
		this.resConfig = resConfig;

		this.tasks = viaRes ? null : ((typeof script.length !== "undefined") ? script : [script]);
		this.load = function () {
			if (viaRes) {
				if (task.tasks = loaderProxy.resoureSets[script]) {     // 已注册过此资源集
					loadResourceSets(task);
				} else {                                                // 未注册过（试图加载资源集文件）
					if (resConfig === jsConfigUrl && jsConfigUrlLoaded) {  //  已读取过配置
						var t = 'No such resource set found: ' + script; log(t); throw new Error(t);
					}
					var resScript = new Script('_Res_' + resConfig.substr(resConfig.lastIndexOf('/') + 1).replace('.', ''), resConfig, null);
					var resTask = new LoadTask(resScript, function () {
						jsConfigUrlLoaded = true;
						if (!(loaderProxy.resoureSets[script])) {
							var t = 'No such resource set found: ' + script; log(t); throw new Error(t);
						}
						// 开始排序并发动载入
						task.tasks = loaderProxy.resoureSets[script];
						loadResourceSets(task);
					}, function () {
						var t = 'Failed to load resource configuration'; log(t); throw new Error(t);
					});
					resTask.load();
				}
			} else {
				var exsits = Manager.get(script);
				if (exsits) {
					this.isReady = exsits.status === 2 || exsits.status === -1;
					this.successed = exsits.status === 2;

					if (this.isReady && this.onReady) {
						triggerEvent(this.onReady, true);
					}
					if (this.successed && this.onSuccess) {
						triggerEvent(this.onSuccess, true);
					}
				} else {
					loadResourceSets(this);
				}
			}
			return this;
		}
		this.ready = function (func) {
			this.onReady = addEvent(this.onReady, func);
			if (this.isReady) {
					triggerEvent(this.onReady, true);
			}
			return this;
		};
		this.success = function (func) {
			this.onSuccess = addEvent(this.onSuccess, func);
			if (this.successed) {
				triggerEvent(this.onSuccess, true);
			}
			return this;
		};
		this.error = function (func) {
			this.onError = addEvent(this.onError, func);
			return this;
		};
	}
	function loaderProxy() {
		var loader = new LoadTask(arguments[0], arguments[1], arguments[2], arguments[3] || jsConfigUrl);
		loader.load();
		return loader;
	}
	loaderProxy._readyFunc = null;      // Ready 事件
	loaderProxy.ready = function (func, donotTigger) {
		loaderProxy._readyFunc = addEvent(loaderProxy._readyFunc, func);
		if (!donotTigger && loaderProxy._ready) {
			triggerReady();
		}
	}
	function triggerReady() {
		triggerEvent(loaderProxy._readyFunc, true);
	}
	loaderProxy.resoureSets = {};
	loaderProxy.addResource = function (obj) {
		for (var a in obj) {
			if (obj.hasOwnProperty(a)) {
				if (loaderProxy.resoureSets[a]) {
					log('Original resource ' + a + ' has been overwrite');
				}
				loaderProxy.resoureSets[a] = obj[a];
			}
		}
	};
	
	if (!window.we7) {
		window.we7 = {};
	}
	window.we7.load = loaderProxy;
	function init() {
		if (loaderProxy._ready) {
			return; // 防止重复提交
		}
		loaderProxy._ready = true;

		log('Initializing loader callbacks, typeof we7 is:' + (typeof we7));

		var loads = [];
		// 遍历脚本并执行其中代码
		var scripts = [], docScripts = document.getElementsByTagName('SCRIPT'), len = docScripts.length;
		for (var _si = 0; _si < len; _si++) { //当此时向 DOM 中注入 script 时，get.byTagName 的结果会被修改，因此需要复制出来
			scripts.push(docScripts[_si]);
		}
		var script, loaderCommand, loadRef, task, config, injed = 'injected', inj;

		for (var i = 0; i < len; i++) {
			script = scripts[i];
			inj = script.getAttribute(injed);
			if (inj) {
				continue;
			}
			loadRef = script.getAttribute(scriptRel);
			config = script.getAttribute(scriptConfig);
			if (script.src && ((script.src.indexOf(scriptHref) > -1) || loadRef)) {
				if (config) {                           //NOTE: 仅 loader.js 脚本上的 config 会更改全局（其他的对应各自）
					jsConfigUrl = config;
				}
				loaderCommand = script.innerHTML.replace(/^\s+/, '').replace(/\s+$/, '');
			}
			if (loadRef) {
				loads.push({
					name: loadRef,
					callback: loaderCommand ? loaderCommand : null,
					config: config ? config : jsConfigUrl
				});
			} else {
				if (script.src && loaderCommand) {
					loaderProxy.ready(loaderCommand, true);             //没有载入任务时为 window；有载入任务时为 LoadTask 本身
				}
			}
			loaderCommand = undefined;
		}
		triggerReady();
		for (var i = 0; i < loads.length; i++) {
			task = loaderProxy(loads[i].name, loads[i].callback, null, loads[i].config);
		}
	}
	var irCount = 0, irdCount = 0, readyRun = false;
	if (loadInner !== "false") {
		function checkReady() {
			irdCount++;
			if (irdCount >= irCount && !readyRun) {
				readyRun = true;
				init();
			}
		}
		for (var p in innerResource) {
			if (innerResource.hasOwnProperty(p)) {
				irCount++;
				(function (index) {
					setTimeout(function () {
						(new LoadTask(innerResource[index], checkReady)).load();
					}, 1);
				})(p);
			}
		}
	} else {
		init();
	}

})();


/*
<script type="text/javascript" ref="aspnet" config="/scripts/we7/_ResourceSets.js" src="/scripts/loader.js">
alert('done')           // 保证所有核心组件都可用？
</script>
*/

/* 注册内置组件的资源 */

