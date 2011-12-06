function MaskWin(params) {
	this.win = (params && params.win) ? params.win : window;
	this.doc = this.win.document;
	this.mask = null;
	this.content = null;
}

MaskWin.prototype = {
	$: function (el) {
		return win.document.getElementById(el);
	},
	show: function () {
		if (!this.mask) {
			this.createMask();
			this.content.innerHTML = "abcde";
		}
	},
	autofit: function (w, h) {
		with (this.contentInner.style) {
			width = w + "px";
			marginLeft = "auto";
			marginRight = "auto";
			height = h + "px";
			var outerHeight = document.viewport.getHeight();
			marginTop = (outerHeight - h) / 2 + "px";
		}
	},
	createMask: function () {

		this.hide();

		this.mask = $(MaskWin.maskId) ? $(MaskWin.maskId) : this.doc.createElement("DIV");
		this.contentOuter = $(MaskWin.contentOuterId) ? $(MaskWin.contentOuterId) : this.doc.createElement("DIV");
		this.contentInner = this.doc.createElement("DIV");


		this.mask.id = "_mask_mask";
		this.contentOuter.id = "_mask_contentOuter";


		this.doc.body.appendChild(this.contentOuter);
		this.doc.body.appendChild(this.mask);
		this.contentOuter.appendChild(this.contentInner);


		//$("scrollshow").styleSheet.cssText="";
		//$("scrollshow").styleSheet.addImport("/Admin/Ajax/masknoscroll.css");

		try {
			$("scrollshow").styleSheet.cssText = "html{overflow:hidden}";
		}
		catch (e) {
			document.styleSheets[0].cssRules[0].style.cssText = "html,body{overflow:hidden}";
			document.body.style.overflow = "hidden";
		}

		with (this.mask.style) {
			backgroundColor = "#000000";
			width = "100%";
			height = this.doc.documentElement.clientHeight + "px";
			top = 0;
			position = "absolute";
			zIndex = 99;
			top = this.doc.documentElement.scrollTop + "px";
			left = 0;
			filter = "alpha(opacity = 30)";
			opacity = 0.3;
		}

		with (this.contentOuter.style) {
			width = "100%";
			height = this.doc.documentElement.clientHeight + "px";
			position = "absolute";
			zIndex = 100;
			top = this.doc.documentElement.scrollTop + "px";
			left = 0;
		}

		with (this.contentInner.style) {
			backgroundColor = "#FFFFFF";
			width = "500px";
			marginLeft = "auto";
			marginRight = "auto";
			height = "500px";
			marginTop = (this.parseUnit(this.contentOuter.style.height) - this.parseUnit(this.contentInner.style.height)) / 2 + "px";
			//opacity = 0.5;
			//-moz-opacity:0.5;
			//border = "dashed 2px yellow";
			border = "solid 1px #555555";
		}

	},
	createContent: function (message) {
		this.header = this.doc.createElement("DIV");
		this.content = this.doc.createElement("DIV");
		this.title = this.doc.createElement("DIV");
		this.closebutton = this.doc.createElement("DIV");

		this.contentInner.appendChild(this.header);
		this.contentInner.appendChild(this.content);
		this.header.appendChild(this.title);
		this.header.appendChild(this.closebutton);

		with (this.header.style) {
			backgroundColor = "#333333";
			height = "20px";
			textAlign = "right";
			paddingTop = "5px";
			paddingLeft = "10px";
			paddingRight = "10px";
		}

		with (this.content.style) {
			height = (this.contentInner.clientHeight - this.header.clientHeight) + "px";
			padding = "0px";
			margin = "0px";
			width = "100%"
		}

		with (this.title.style) {
			styleFloat = "left";
			cssFloat = "left";
			color = "#777777";
		}

		with (this.closebutton.style) {
			styleFloat = "right";
			cssFloat = "right";
			cursor = "pointer";
		}

		this.title.innerHTML = message ? message : "";
		this.closebutton.innerHTML = "<img src='/Admin/Ajax/Images/close.png' />";
		var obj = this;
		this.closebutton.onclick = function () {
			obj.hide();
		}

	},
	showProgressBar: function (param) {
		this.createMask();
		var obj = this;
		this.contentInner.innerHTML = "";
		var imgContext = this.doc.createElement("DIV");
		var sl = this.doc.getElementsByTagName("SELECT");
		for (var i = 0; i < sl.length; i++) {
			sl[i].style.display = "none";
		}

		this.contentInner.appendChild(imgContext);
		imgContext.innerHTML = "<img src='/admin/Ajax/Images/loading.gif'></img>";

		this.autofit(100, 20);

		if (param) {
		}
	},
	//showMessageProgressBar:function(url,param)
	showMessageProgressBar: function (param) {
		var args = {};
		Ext.apply(args, param);
		args.timeout = (param && param.timeout) ? param.timeout : 1000;
		args.width = (param && param.width) ? param.width : 300;
		args.height = (param && param.height) ? param.height : 200;
		args.message = (param && param.message) ? param.message : "操作成功！";
		args.title = (param && param.title) ? param.title : "安装插件！";
		args.submitbutton = (param && param.submitbutton) ? param.submitbutton : null;
		args.action = param.action;
		args.plugin = param.plugin;
		args.isPluginList = param.isPluginList;
		args.menu = param.menu;

		this.createMask();

		this.autofit(args.width, args.height);
		this.createContent("<b>" + args.title + "</b>");

		var obj = this;
		var txtContext = this.doc.createElement("DIV");
		var imgContext = this.doc.createElement("DIV");
		var textarea = this.doc.createElement("textarea");
		txtContext.appendChild(textarea);

		txtContext.style.textAlign = "center";
		textarea.css = "formValue";

		this.content.appendChild(imgContext);
		this.content.appendChild(txtContext);
		this.content.style.backgroundColor = "#f0f0f0";

		with (textarea.style) {
			width = args.width - 6 + "px";
			//height=args.height-22+"px";
			height = (this.content.clientHeight - imgContext.clientHeight) + "px";
			if (getNav().firefox || parseInt(getNav().ie) > 6.0) {
				height = (this.content.clientHeight - imgContext.clientHeight - 19) + "px";
			}
			fontSize = "12px";
			padding = "0px";
			margin = "0px";
		}

		imgContext.innerHTML = "<img src='/admin/Ajax/Images/messageloader.gif'></img>";

		if (!args.isPluginList) {
			SinglePlugin();
		}
		else {
			if (args.plugin && args.plugin.length > 0)
				MultiPlugin();
		}

		function SinglePlugin() {
			var currentList = getInstallList(args.action);
			currentList.plugin = args.plugin;
			currentList.pltype = args.pltype;
			currentList.menu = args.menu;
			//debugger;
			install(currentList, function (message, flag) {
				textarea.value += message + "\r\n";
				textarea.scrollTop = textarea.scrollHeight;
				if (flag) {
					imgContext.innerHTML = "<b>&nbsp;&nbsp;" + args.message + "</b>";
					imgContext.style.padding = "2px 0px 0px;";
					obj.closebutton.onclick = function () {
						if (args.submitbutton) {
							obj.contentOuter.innerHTML = "";
							args.submitbutton.click();
						}
						else
							obj.hide();
					}
				}
				else if (flag == false) {
					imgContext.innerHTML = "<b>&nbsp;&nbsp;" + "操作失败" + "</b>";
				}
			});
		}

		function MultiPlugin(i) {
			i = i || 0;
			var currentList = getInstallList(args.action);
			currentList.plugin = args.plugin[i];
			currentList.pltype = args.pltype;

			install(currentList, function (message, flag) {
				textarea.value += message + "\r\n";
				textarea.scrollTop = textarea.scrollHeight;
				if (flag) {
					if (i == args.plugin.length - 1) {
						imgContext.innerHTML = "<b>" + args.message + "</b>";
						imgContext.style.padding = "2px 10px 0px;";
						obj.closebutton.onclick = function () {
							if (args.submitbutton) {
								obj.contentOuter.innerHTML = "";
								args.submitbutton.click();
							}
							else
								obj.hide();
						}
					}
					else {
						textarea.value += "==================" + "\r\n";
						MultiPlugin(i + 1);
					}
				}
			});
		}
	},
	showDetails: function (url, title) {
		this.createMask();
		this.contentInner.innerHTML = "<img src='/admin/Ajax/Images/loading.gif'></img>";
		var obj = this;

		this.autofit(100, 10);

		setTimeout(function () {
			new Ajax.Request(url, {
				onSuccess: function () {
					obj.autofit(550, 450);
					obj.contentInner.innerHTML = "";

					var iframe;
					try {
						iframe = obj.doc.createElement('<iframe id="contentFrame" src="' + url + '" scrolling="no" frameborder="0" width="100%" height="100%" style="margin:0; padding:0;" ></iframe>');
					}
					catch (e) {
						iframe = obj.doc.createElement('IFRAME');
						iframe.id = "contentFrame";
						iframe.src = url;
						iframe.scrolling = "no";
						iframe.frameborder = 0;
						iframe.width = '100%';
						iframe.height = '100%';
						iframe.style.margin = "0";
						iframe.style.padding = "0";
					}
					obj.createContent("<b>插件信息：</b>" + (title ? title : ""));
					obj.content.appendChild(iframe);
					//obj.content.appendChild(obj.doc.createElement('<iframe id="contentFrame" src="'+url+'" scrolling="no" frameborder="0" width="100%" height="100%" style="margin:0; padding:0;" ></iframe>'));
					//obj.content.innerHTML = '<iframe src="'+url+'" frameborder="0" style="width:100%;height:'+obj.content.clientHeight + '" ></iframe>';
				}
			});
		}, 1000);
	},
	showFrame: function (url, title, param) {
		this.createMask();
		this.autofit(10, 10);
		this.contentInner.innerHTML = "<img src='/admin/Ajax/Images/loading.gif'></img>";
		var obj = this;
		var width = (param && param.width) ? param.width : 300;
		var height = (param && param.height) ? param.height : 50;
		title = title ? title : "插件信息";
		this.autofit(width, height);
		this.contentInner.innerHTML = "";
		this.createContent(title);
		try {
			iframe = obj.doc.createElement('<iframe id="contentFrame" src="' + url + '" scrolling="auto" frameborder="0" width="100%" height="100%" style="margin:0; padding:0;" ></iframe>');
		}
		catch (e) {
			iframe = obj.doc.createElement('IFRAME');
			iframe.id = "contentFrame";
			iframe.src = url;
			iframe.scrolling = "auto";
			iframe.frameborder = 0;
			iframe.width = '100%';
			iframe.height = '100%';
			iframe.style.margin = "0";
			iframe.style.padding = "0";
		}
		if (param.refresh == 'yes')
			obj.closebutton.onclick = function () {
				document.getElementById(refreshbutton).click();
			}
		obj.content.appendChild(iframe);
	},
	hide: function () {
		MaskWin.hide();
	},
	parseUnit: function (param) {
		param = param.replace("px", "");
		return parseInt(param);
	}
}

MaskWin.contentOuterID = "_mask_contentOuter";
MaskWin.maskID = "_mask_mask";

MaskWin.hide = function (win) {
	win = win || window;
	var outer = $("_mask_contentOuter");
	var mask = $("_mask_mask");
	var css = $("scrollshow");
	if (outer && mask && css) {
		win.document.body.removeChild($("_mask_contentOuter"));
		win.document.body.removeChild($("_mask_mask"));
		try {
			$("scrollshow").styleSheet.cssText = "";
		}
		catch (e) {
			document.styleSheets[0].cssRules[0].style.cssText = "";
			document.body.style.overflow = "auto";
		}
	}

}

function LoopRequest(param, isDirty) {

	if (!param || !param.url) {
		alert("LoopRequest::参数不正确");
		return;
	}
	var onSuccessBak = param.onSuccess;
	param.onSuccess = param.onSuccess ? function (xmlhttp) {
		try {
			var result = eval("(" + xmlhttp.responseText + ")");
			if (result != null) {
				if (onSuccessBak(result))
					setTimeout(function () { loop(param) }, param.timeout);
			}
		}
		catch (e) {
			setTimeout(function () { loop(param) }, param.timeout);
		}
	} : function () { alert("LoopRequest::没有实现循环方法!") };

	function loop(param) {
		if (!isDirty) {
			new Ajax.Request(param.url + "&isFirst=1", param);
			isDirty = true;
		}
		else {
			new Ajax.Request(param.url, param);
		}
	};

	loop(param);
}


function getNav() {
	var Sys = {};
	var ua = navigator.userAgent.toLowerCase();
	var s;
	(s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
    (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
    (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
    (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
    (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;
	return Sys;
}
