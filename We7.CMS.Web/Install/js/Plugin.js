var hasreact = false;
function install(installList, fn) {
	installList.count = installList.count || 0;
	installList.i = installList.i || 0;

	var config = {
		timeout: 600000,
		url: '/Install/Plugin/PluginCommandHandler.ashx',
		//params:{cmd:installList[installList.i].cmd,action:installList.action,plugin:installList.plugin,pltype:installList.pltype},
		data: { cmd: installList[installList.i].cmd, action: installList.action, plugin: installList.plugin, pltype: installList.pltype },
		async: true,
		success: function (text, options) {
			installList.count = 0;
			try {
				var result = eval('(' + text + ')');

				if (result.success) {
					fn(installList[installList.i].success);
					if (result.desc != '' && installList[installList.i].cmd == 'download')
						installList.menu = result.desc;
					installList.i = installList.i + 1;
					if (installList.i < installList.length) {
						install(installList, fn);
					}
					else {
						fn(installList.success, true);
						if (parseInt(installList.menu) > 0 && installList.action != "uninstall" && installList.action != "delete") {
							var menucount = parseInt(installList.menu);
							var frameheight = 225 + 120 * ((menucount > 3 ? 3 : menucount) - 1);
							new MaskWin().showFrame("/Admin/Plugin/PluginMenu.aspx?fresh=refreshbutton&PluginName=" + installList.plugin + "&action=" + installList.action, "更新菜单", { width: 500, height: frameheight, refresh: 'yes' });
						}
					}
				}
				else {
					//卸载插件时删除内容模型失败时重试。
					if (installList[installList.i].cmd == "installmodels" && installList.action == "uninstall" && !hasreact) {
						fn(result.desc);
						hasreact = true;
						install(installList, fn);
					}
					else {
						fn(result.desc, false);
					}
				}
			}
			catch (ex) {
				fn(ex.message, false);
			}
		},
		error: function (text, status) {
			if (status == "timeout") {
				fn("当前操作已操时！", true);
			}
			else {
				fn("应用程序错误！错误原因：" + response.text, true)
			}
		}
	}

	function reinstall() {
		if (installList.count < 10) {
			setTimeout(function () { install(installList, fn); }, 1000);
			installList.count = installList.count + 1;
		}
		else {
			fn(installList[installList.i].error, true)
		}
	}

	if (installList.i == 0) {
		var plName = installList.plugin || "";
		var start = plName.lastIndexOf("/");
		if (start > -1) {
			plName = plName.substr(start + 1, plName.length - start);
			plName = plName.replace(".zip", "");
		}
		fn(installList.start + plName);
	}

	fn(installList[installList.i].start);

	jQuery.ajax(config);
	//Ext.Ajax.request(config);
}


var RemoteInstallList = [{
	cmd: 'download',
	start: '开始下载数据',
	success: '数据下载成功',
	error: '数据下载失败'
}, {
	cmd: 'checkversion',
	start: '检测插件版本',
	success: '版本检测成功',
	error: '插件版本与当前CMS版本不符'
}, {
	cmd: 'checkfile',
	start: '开始检测数据',
	success: '数据检测成功',
	error: '数据检测失败'
}, {
	cmd: 'installdll',
	start: '开始安装dll文件',
	success: 'dll文件安装成功',
	error: 'dll文件安装失败'
}, {
	cmd: 'installdb',
	start: '开始安装数据库',
	success: '数据库安装成功',
	error: '数据库安装失败'
}, {
	cmd: 'installdbfile',
	start: '开始安装数据库访问文件',
	success: '数据库访问文件安装成功',
	error: '数据库访问文件安装失败'
}, {
	cmd: 'installcontrol',
	start: '开始安装前台控件',
	success: '前台控件安装成功',
	error: '前台控件安装失败'
}, {
	cmd: 'installskin',
	start: '开始安装模板',
	success: '模板安装成功',
	error: '模板安装失败'
}, {
	cmd: 'installmodels',
	start: '开始安装内容模型',
	success: '内容模型安装成功',
	error: '内容模型安装失败'
}, {
	cmd: 'updateconfig',
	start: '更新配置文件',
	success: '配置文件更新成功',
	error: '配置文件更新出错'
}, {
	cmd: 'reset',
	start: '重启应用程序',
	success: '应用程序启动成功',
	error: '应用程序启动出错'
}];


//RemoteInstallList.plugin="ADPlugin";
RemoteInstallList.action = "install";
RemoteInstallList.start = "开始安装插件";
RemoteInstallList.success = "插件安装成功";

var InstallList = [{
	cmd: 'checkfile',
	start: '开始检测数据',
	success: '数据检测成功',
	error: '数据检测失败'
}, {
	cmd: 'checkversion',
	start: '检测插件版本',
	success: '版本检测成功',
	error: '插件版本与当前CMS版本不符'
}, {
	cmd: 'installdll',
	start: '开始安装dll文件',
	success: 'dll文件安装成功',
	error: 'dll文件安装失败'
}, {
	cmd: 'installdb',
	start: '开始安装数据库',
	success: '数据库安装成功',
	error: '数据库安装失败'
}, {
	cmd: 'installdbfile',
	start: '开始安装数据库访问文件',
	success: '数据库访问文件安装成功',
	error: '数据库访问文件安装失败'
}, {
	cmd: 'installskin',
	start: '开始安装模板',
	success: '模板安装成功',
	error: '模板安装失败'
}, {
	cmd: 'installmodels',
	start: '开始安装内容模型',
	success: '内容模型安装成功',
	error: '内容模型安装失败'
}, {
	cmd: 'installcontrol',
	start: '开始安装前台控件',
	success: '前台控件安装成功',
	error: '前台控件安装失败'
}, {
	cmd: 'updateconfig',
	start: '更新配置文件',
	success: '配置文件更新成功',
	error: '配置文件更新出错'
}, {
	cmd: 'reset',
	start: '重启应用程序',
	success: '应用程序启动成功',
	error: '应用程序启动出错'
}];


//InstallList.plugin="ADPlugin";
InstallList.action = "install";
InstallList.start = "开始安装插件";
InstallList.success = "插件安装成功";

var UpdateList = [{
	cmd: 'checkversion',
	start: '检测插件版本',
	success: '版本检测成功',
	error: '插件版本与当前CMS版本不符'
}, {
	cmd: 'checkfile',
	start: '开始检测数据',
	success: '数据检测成功',
	error: '数据检测失败'
}, {
	cmd: 'installdll',
	start: '开始更新dll文件',
	success: 'dll文件更新成功',
	error: 'dll文件更新失败'
}, {
	cmd: 'installdb',
	start: '开始更新数据库',
	success: '数据库更新成功',
	error: '数据库更新失败'
}, {
	cmd: 'installdbfile',
	start: '开始更新数据库访问文件',
	success: '数据库访问文件更新成功',
	error: '数据库访问文件更新失败'
}, {
	cmd: 'installskin',
	start: '开始更新模板',
	success: '模板更新成功',
	error: '模板更新失败'
}, {
	cmd: 'installmodels',
	start: '开始更新内容模型',
	success: '内容模型更新成功',
	error: '内容模型更新失败'
}, {
	cmd: 'installcontrol',
	start: '开始更新前台控件',
	success: '前台控件更新成功',
	error: '前台控件更新失败'
}, {
	cmd: 'updateconfig',
	start: '更新配置文件',
	success: '配置文件更新成功',
	error: '配置文件更新出错'
}, {
	cmd: 'reset',
	start: '重启应用程序',
	success: '应用程序启动成功',
	error: '应用程序启动出错'
}];


//UpdateList.plugin="ADPlugin";
UpdateList.action = "update";
UpdateList.start = "开始更新插件";
UpdateList.success = "插件更新成功";

var RemoteUpdateList = [{
	cmd: 'download',
	start: '开始下载数据',
	success: '数据下载成功',
	error: '数据下载失败'
}, {
	cmd: 'checkversion',
	start: '检测插件版本',
	success: '版本检测成功',
	error: '插件版本与当前CMS版本不符'
}, {
	cmd: 'checkfile',
	start: '开始检测数据',
	success: '数据检测成功',
	error: '数据检测失败'
}, {
	cmd: 'installdll',
	start: '开始更新dll文件',
	success: 'dll文件更新成功',
	error: 'dll文件更新失败'
}, {
	cmd: 'installdb',
	start: '开始更新数据库',
	success: '数据库更新成功',
	error: '数据库更新失败'
}, {
	cmd: 'installdbfile',
	start: '开始更新数据库访问文件',
	success: '数据库访问文件更新成功',
	error: '数据库访问文件更新失败'
}, {
	cmd: 'installskin',
	start: '开始更新模板',
	success: '模板更新成功',
	error: '模板更新失败'
}, {
	cmd: 'installmodels',
	start: '开始更新内容模型',
	success: '内容模型更新成功',
	error: '内容模型更新失败'
}, {
	cmd: 'installcontrol',
	start: '开始更新前台控件',
	success: '前台控件更新成功',
	error: '前台控件更新失败'
}, {
	cmd: 'updateconfig',
	start: '更新配置文件',
	success: '配置文件更新成功',
	error: '配置文件更新出错'
}, {
	cmd: 'reset',
	start: '重启应用程序',
	success: '应用程序启动成功',
	error: '应用程序启动出错'
}];

//RemoteUpdateList.plugin="ADPlugin";
RemoteUpdateList.action = "update";
RemoteUpdateList.start = "开始更新插件";
RemoteUpdateList.success = "插件更新成功";


var UnInstallList = [{
	cmd: 'checkfile',
	start: '开始检测数据',
	success: '数据检测成功',
	error: '数据检测失败'
}, {
	cmd: 'installskin',
	start: '开始卸载模板',
	success: '模板卸载成功',
	error: '模板卸载失败'
}, {
	cmd: 'installcontrol',
	start: '开始卸载前台控件',
	success: '前台控件卸载成功',
	error: '前台控件卸载失败'
}, {
	cmd: 'installmodels',
	start: '开始卸载内容模型',
	success: '内容模型卸载成功',
	error: '内容模型卸载失败'
}, {
	cmd: 'installdb',
	start: '开始更新数据库',
	success: '数据库更新成功',
	error: '数据库更新失败'
}, {
	cmd: 'installdbfile',
	start: '开始卸载数据库访问文件',
	success: '数据库访问文件卸载成功',
	error: '数据库访问文件卸载失败'
}, {
	cmd: 'installdll',
	start: '开始卸载dll文件',
	success: 'dll文件卸载成功',
	error: 'dll文件卸载失败'
}, {
	cmd: 'updateconfig',
	start: '更新配置文件',
	success: '配置文件更新成功',
	error: '配置文件更新出错'
}, {
	cmd: 'reset',
	start: '重启应用程序',
	success: '应用程序启动成功',
	error: '应用程序启动出错'
}];

//UnInstallList.plugin="ADPlugin";
UnInstallList.action = "uninstall";
UnInstallList.start = "开始卸载插件";
UnInstallList.success = "插件卸载成功";



var DeleteList = [{
	cmd: 'delete',
	start: '正在处理文件',
	success: '文件处理成功',
	error: '删除失败'
}];

//DeleteList.plugin="ADPlugin";
DeleteList.action = "delete";
DeleteList.start = "删除";
DeleteList.success = "删除成功";

var InstallCTRList = [{
	cmd: 'download',
	start: '同步数据',
	success: '同步成功',
	error: '同步失败'
}, {
	cmd: 'updateconfig',
	start: '更新配置文件',
	success: '配置文件更新成功',
	error: '配置文件更新出错'
}, {
	cmd: 'reset',
	start: '重启应用程序',
	success: '应用程序启动成功',
	error: '应用程序启动出错'
}];
InstallCTRList.action = "install";
InstallCTRList.start = "开始安装控件";
InstallCTRList.success = "安装成功";


function getInstallList(action) {
	var currentList;
	switch (action) {
		case "remoteinstall":
			currentList = RemoteInstallList;
			break;
		case "remoteupdate":
			currentList = RemoteUpdateList;
			break;
		case "install":
			currentList = InstallList;
			break;
		case "update":
			currentList = UpdateList;
			break;
		case "uninstall":
			currentList = UnInstallList;
			break;
		case "delete":
			currentList = DeleteList;
			break;
		case "insctr":
			currentList = InstallCTRList;
			break;
		default:
			alert("没有设置action参数");
			return null;
	}
	currentList.i = 0;
	currentList.count = 0;
	return currentList;
}