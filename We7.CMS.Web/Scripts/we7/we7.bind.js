/// <reference path="../jQuery/jquery-1.4.2.js" />
/// <reference path="jquery.jqGrid.min.js" />
/// <reference path="we7.core.js" />

//[TODO:当 height 太小时的删除确认？]
//[TODO:如果只想让行删除，不想让行编辑？]
(function () {
	jQuery.jgrid.no_legacy_api = true;
	jQuery.jgrid.useJSON = true;

	function we7bind(elem, data, options) {       // elem [, data], options     elem 为 jQuery 对象，且 selector 必须为 ID 选择器 的 table
		options = options || {};
		var self = this, fire = $(this), elemId = elem.selector.substr(1), pagerId = 'jqGridPager_' + elemId;
		var definedData = !!(options.colNames && options.colModel),                                               // NOTE: definedData 表示是否已在前台定义好模型
            dataUrl, urlRegex = /^(http|\/admin).+/;

		if (we7.isStr(data)) {
			if (urlRegex.test(data)) {      // capatibal with url(so we consider the datatype as Json)
				dataUrl = data;
				options.datatype = "json";
			} else {
				options.datastr = data;
				options.datatype = "jsonstring";
			}
		} else if (we7.isArr(data)) {
			options.data = data;
			options.datatype = "local";
			definedData = true;
		} else {
			options = $.extend(data || {}, options);
		}

		if (!options.pager) {
			$(we7.formatStr('<div id="{0}" class="we7-jqGrid-Pager" />', pagerId)).insertAfter(elem);                  // class= we7-jqGrid-Pager
		}

		var defaultOptions = {
			/* View */
			altRows: true
				, gridview: true
				, height: "auto"
				, hidegrid: false
				, autoencode: true
				, mtype: 'POST'

			/* Search */
				, ignoreCase: true

			/* Sort */
				, sortorder: 'desc'

			/* Pager */
				, viewrecords: true
				, rowNum: 20
				, rowList: [10, 20, 30, 50]
				, pager: '#' + pagerId              // 创建一个 Pager 由于 jqGrid 内置脚本，pager 必须为一个ID选择器（而非 jQuery 对象或其他选择器）
				, pagerpos: 'right'
				, recordpos: 'left'

			/* Parameter Names */
				, prmNames: {
					page: "_page"
					, rows: "_rows"
					, sort: "_sort"
					, order: "_sord"
					, nd: '_t'
					, search: '_search'			// 实际上没有什么用途（用于标识是否搜索，其可以从搜索条件的有无而自动判断）
					, id: '_id'					// 删除和编辑时的 ID 参数名
					, oper: '_oper'
					, editoper: '3'
					, deloper: '1'
					, totalrows: '_tr'			// 实际上没有什么用途（要请求的行用 rows 表示）
					, filter: '_condition'		// 添加的名称，过滤规则
				}
				, serializeGridData: function (d) {
					var ds = d["_sord"];
					if (ds) {
						d["_sord"] = ds.toLowerCase() === "desc" ? 1 : 0;
					}
					delete d["_search"];
					return d;
				}

				, jsonReader: {
					id: '_gridID'
					, repeatitems: false
					, page: "currPage"			// pageSize
					, total: "totalPages"
					, records: "totalRecords"
				}

			/* we7.bind */
				, addEditColumn: true
				, editColumnHeader: '操作'
				, deletableRow: false

		};
		function triggerEvent(type, data) {
			var event = $.Event();
			event.type = type;
			fire.trigger(event, data);
			return event;
		}
		options = $.extend(defaultOptions, options);
		if (!options.ajaxDelOptions) {
			options.ajaxDelOptions = {};
		}
		options.loadError = function () {
			triggerEvent("onLoadError", [].slice.apply(arguments));
		};

		$.extend(this, {
			destroy: function () {
				var eBefore = triggerEvent("onBeforeDestroy");
				if (eBefore.isDefaultPrevented()) { return false; }
				$.jgrid.GridDestroy(elemId);
				triggerEvent("onDestroy");
			}
			, refresh: function () {
				var data = {}, eBefore = triggerEvent("onBeforeLoad", [data, true]); 	// (data, force)
				if (eBefore.isDefaultPrevented()) { return false; }
				$grid.trigger("reloadGrid");
			}
		});
		$.each("onBeforeLoad,onBeforeDelete,onBeforeEdit,onBeforeDestroy,onLoad,onLoadError,onDelete,onEdit,onDestroy".split(','), function (i, event) {
			self[event] = function (fn) {
				fire.bind(event, fn);
			};
			if (we7.isFunc(options[event])) {
				fire.bind(event, options[event]);
			}
		});
		self.onLoadError(function (xhr, status, err) {
			we7.log('#bxhr01\n' + (err ? (err.message ? err.message : '[bind error] http status:' +err.toString()) : ('unknow error:' + err.toString() + ' [' + status + ']')));
		});
		var editableGrid = false, modelCreated = false, templated = false, editableRow = false,
			$grid, $gridview, self = this, lastSel, dataRendered, _search, _searchModels;

		function buildTemplate() {
			if (!options.rowTemplate) {					//	 未定义模板，启用服务器模板？
				return;
			}
			var colRegex = /\<td[^>]*\>(((?!\<\/?td).)+)\<\/td\>/gi, fieldRegex = /^\$\{([a-z0-9_]+)\}$/i, colMRegex = /\$\{([a-z0-9_]+)\}/gi,
				colModel = [], cols, searches = [], st;

			while (cols = colRegex.exec(options.rowTemplate)) {
				var c = $(cols[0]), cm, cmK, field, colContent = cols[1], attr_e, attr_s;
				field = fieldRegex.exec(colContent);
				cm = {
					header: jQuery.trim(c.attr('header') || '')
					, sorttype: c.attr('sorttype') || 'text'
					, sortOnload: c.attr('sortOnload') || false
					, width: c.attr('width') || parseInt(c.css("width")) || undefined
				};
				attr_e = c.attr('editable');
				if (attr_e){
					if (attr_e !== "false") {
						cm.editable = true;
						if (attr_e !== "true") {
							cm.edittype = attr_e;
						} else {
							cm.edittype = 'text';
						}
					} else {
						cm.editable = false;
					}
				} else {
					cm.editable = false;
				}
				if (field) {
					cm.name = field[1];
				} else {
					cm.index = c.attr('sortkey') || undefined;
					cm.name = 'we7col_' + Math.floor(Math.random() * 1000);
					cmK = c.attr("editkey");
					if (cmK) {
						cm.editKey = cmK;
					} else {
						cm.editable = false;
					}
				}
				if (!we7.isUndef(cm.index)) {
					cm.sortable = true;
				} else {
					cm.sortable = false;
				}
				colModel.push(cm);

				while (st = colMRegex.exec(colContent)) {		// 搜索标记
					searches.push(st[1]);
				}
			}
			if (colModel.length) {
				options.colModel = colModel;
				_searchModels = searches.join(',');
			}
		}

		function mergeModel(models) {
			function findAll(arr, fn) {
				var ids = [];
				$.each(arr, function (i, a) {
					if (fn.call(arr, a, i) === true) {
						ids.push(i);
					}
				});
				return ids.length ? ids : null;
			}
			// what about no template ?
			var med, uiReged = !!jQuery.fn.datepicker, colModel = we7(options.colModel) ? we7(options.colModel).clone() : [];
			if (!options.colModel) { options.colModel = [] }
			$.each(models, function (i, model) {
				med = findAll(colModel, function (item) { return item.name === model.name });   // 在本地寻找正在处理的服务器 model
				if (med && med.length > 0) {
					$.each(med, function (j, mj) {						// 将原 model 中的所有当前 model 与服务器版本合并
						var m = colModel[mj];
						if (m.editable) {								// TODO注意：如果一行内多列使用相同 colModel，则更新时导致其数据不同步（解决办法是重新呈现行）
							model.editable = true;
							if (m.edittype) {
								model.edittype = m.edittype;
							} else {
								model.edittype = 'text';
							}
						}
						if (model.sortable) {
							model.sortable = true;
							m.sorttype && (model.sorttype = m.sorttype);
						}
						!!m.header && (model.header = m.header);
						!!m.sortOnload && (model.sortOnload = m.sortOnload);
						options.colModel[mj] = model;
					});
				} else {
					if (options.rowTemplate) {
						model.hidden = true;
					}
					options.colModel.push(model);
				}
			});
			
			editableRow = !!(we7.findInArray(options.colModel, function (item) { return item.editable && !item.hidden }));
			if (options.deletableRow || editableRow) {
				editableGrid = true;
				options.editurl = '/admin/ajax/BusinessSubmit/JsonForCondition.ashx';
				options.serializeRowData = wrapperAjaxData;
				options.serializeDelData = wrapperAjaxData;

				if (options.addEditColumn) {
					options.colModel.push({
						header: options.editColumnHeader				// colName
						, name: '_action'								// name, index
						, sortable: false
						, width: 50
					});
				}
				if(editableRow){
					options.ondblClickRow = editRow;
				}
			}
			var ename, sortStart = we7.findInArray(options.colModel, function (item) { return !!item.sortOnLoad });
			if (sortStart) {
				options.sortname = sortStart.name;
				options.sortorder = sortStart.sortOnLoad === true ? "desc" : sortStart.sortOnLoad; 	// 支持 sortOnLoad: desc, sortOnLoad: asc
			}

			options.colNames = [];
			$.each(options.colModel, function (i, elem) {
				if (!options.we7Models) { options.we7Models = {} }
				ename = elem.name.substr(0, 7) === "we7col_" ? elem.editKey : elem.name;
				if (elem.editable && !we7.isUndef(options.we7Models[ename])) {			//在 bindOption 中直接定义，在这里读出
					elem.editoptions = $.extend(elem.editoptions || {}, options.we7Models[ename]);
					if (elem.edittype === "date") {				// 做 date 支持
						elem.edittype = 'text';
						if (!uiReged) {
							we7.load([{
								name: "jqueryui"
								, src: "/Scripts/jQuery/jQueryUI/jQuery-ui.js"
							}, {
								name: '_jqueryui_css'
								, src: "/Scripts/jQuery/jQueryUI/css/jQuery-ui.css"
							}]);
							uiReged = true;
						}
						if (!elem.editoptions) { elem.editoptions = {}; }
						var diE = elem.editoptions.dataInit;
						elem.editoptions.dataInit = function (ipt) {
							if (diE) { diE.apply(this, Array.prototype.slice.apply(arguments)) }
							var cancel_fn = function (e) {
								e.preventDefault();
								e.stopPropagation();
							};
							setTimeout(function () {
								$(ipt).datepicker();
								$(ipt).focus(function () {				// TODO: 解决显示日历控件问题
									$(this).datepicker("show");
								}).keydown(cancel_fn).bind("paste", cancel_fn).bind("contextmenu", cancel_fn);
							}, 50);
						};
					}
				}

				if (elem.sortable && we7.isUndef(elem.index)) {
					elem.index = elem.name;
				}
				options.colNames.push(elem.header || elem.name || "");
				try {
					!(we7.isUndef(elem.header)) && delete elem.header;
					!(we7.isUndef(elem.sortOnLoad)) && delete elem.sortOnLoad;
					!(we7.isUndef(elem.identify)) && delete elem.identify;
				} catch (e) { }
				options.colModel[i] = elem;
			});
			delete options.we7Models;

			if (_searchModels) {
				$.each(_searchModels.split(','), function (i, s) {
					var j = we7.indexOfArray(options.colModel, function (item) { return this.name === s });
					if (j > -1) {
						options.colModel[j].searchable = true;
					}
				});
				_searchModels = undefined;
			}
		}

		var _tmpled = false;
		function injectRow(rowid, id) {
			var el;
			if (modelCreated) {
				if (!_tmpled) {
					_tmpled = true;
					jQuery.template('we7_tmpl', options.rowTemplate);
				}
				el = jQuery('<p>').append(jQuery.tmpl("we7_tmpl", this));
				return rowid === true ? el.text() : el.html();
			} else {
				return false; 									// 使用原有逻辑
			}
		}

		function wrapperAjaxData(d) {								// 当 ajax 方式为 POST 时处理 data [不为 GET 请求处理]
			if (!editableGrid) { return null; } 						// 不允许编辑时
			var _ps = [], newP = [];
			for (var _p in d) {
				if (d.hasOwnProperty(_p)) {
					if (_p.substr(0, 1) != '_') {					// 筛选内置参数
						_ps.push(_p);
						newP.push((new BindCondition(_p, bindVerb.equals, d[_p])).toParam());
						//newP.push(_p + '=' + d[_p]); 				  // 原值已编码，此处无需再编码
					}
				}
			}
			for (var i = 0; i < _ps.length; i++) {
				delete d[_ps[i]];
			}

			try {
				d["_sort"] && delete d["_sort"]; 				// 这些参数为查询时使用[更新、删除等操作时不需要]
				d["_sord"] && delete d["_sord"];
				d["_c"] && delete d["_c"];
				d["_f"] && delete d["_f"];
			}
			catch (ex) { }

			if (newP.length) {
				d["_c"] = newP.join('|'); 	// 使用 condition 一样的编码机制
			}
			d["_tb"] = options.postData["_tb"];
			d = processDataBeforeReq(d);
			return d;
		}

		jQuery.jgrid._operateEditUI = function (id, edit) {
			var spans = $("tr#" + id).find("td:last").find("span"); 	//[aria-describedby$=_action]:first
			if (edit) {
				if (!we7.isUndef(lastSel)) {
					jQuery.jgrid._operateEditUI(lastSel, false);
				}
				lastSel = id;
				spans.show();
				spans.filter(":eq(0)").hide();
				if (options.deletableRow) {
					spans.filter(":eq(1)").hide();
				}
			} else {
				$grid.jqGrid('restoreRow', id);
				spans.hide();
				spans.filter(":eq(0)").show();
				if (options.deletableRow) {
					spans.filter(":eq(1)").show();
				}
				lastSel = undefined;
			}
			spans = undefined;
		}
		function editRow(id, args) {
			if (!editableGrid) { return; }
			options.addEditColumn && jQuery.jgrid._operateEditUI(id, true);
			args = args || {};
			var rowData = dataRendered && dataRendered.rows ? we7.findInArray(dataRendered.rows, function (item) { return item[options.jsonReader.id] === id }) : undefined;
			var fnOld = args.onsuccess, eBefore = triggerEvent("onBeforeEdit", [id,rowData]);
			if (eBefore.isDefaultPrevented()) { return false; }
			args.onsuccess = function () {
				var cbArgs = [].slice.apply(arguments);
				if (fnOld) {
					fnOld.apply(this, cbArgs);
				}
				triggerEvent("onEdit", [id,rowData].push(cbArgs));
			};
			$grid.jqGrid('editRow', id, false, args.onedit, args.onsuccess, args.url, args.data, args.onsave, args.onerror, args.onrestore);
		}
		function delRow(id) {
			var eid = $('#gbox_' + elemId);
			var rowData = dataRendered && dataRendered.rows ? we7.findInArray(dataRendered.rows, function (item) { return item[options.jsonReader.id] === id }) : undefined;
			var eBefore = triggerEvent("onBeforeDelete", [id, rowData]);
			if (eBefore.isDefaultPrevented()) { return false; }

			$grid.jqGrid('delGridRow', id, { reloadAfterSubmit: true, jqModal: false, serializeDelData: wrapperAjaxData, left: (eid.width() / 2 - 120), top: (Math.max((eid.height()) / 2 - 80, 20)), ajaxDelOptions: { beforeSend: function (xhr, settings) { settings.success = function (data, status) { data = JSON.parse(data); if (data.code == 200) { triggerEvent("onDelete", [data, id, rowData, this.data]) } } } } });
		}
		jQuery.jgrid._editRow = editRow;
		jQuery.jgrid._delRow = delRow;
		function searchGrid(keyword) {		// 此搜索仅对由服务器提供数据源的 grid 有效[ 因为依赖dataRendered ]
			var allData = $.extend({}, dataRendered), searchResult = [];
			if (!allData.totalRecords) {
				return; 					// 没有数据
			}
			var matchIndex = 0, models = options.colModel
			if (keyword.length) {		   // 直接将服务器返回的实际数据用于搜索，未处理当实际数据比“要显示的数据”更多的情况
				keyword = keyword.toLowerCase()
				$.each(allData.rows, function (i, row) {
					matchIndex = 0;
					var rowStr;
					if (modelCreated) {
						rowStr = injectRow.apply(row, [true])
						matchIndex = rowStr ? rowStr.toLowerCase().indexOf(keyword) : -1;
						if (matchIndex > -1) {
							row._keyMatch = matchIndex;
							searchResult.push(row);
						}
					} else {
						$.each(row, function (j, val) {
							var model = we7.findInArray(models, function (item) { return item.name === j; });
							if (model && model.hidden) { return; } 	// TODO：联合服务器的搜索、限制字段搜索
							matchIndex++;
							if (!we7.isUndef(val) && val.toString().toLowerCase().indexOf(keyword) > -1) {
								row._keyMatch = matchIndex;
								searchResult.push(row);
								return false;
							}
						});
					}
				});
				searchResult.sort(function (first, second) {
					return first._keyMatch - second._keyMatch;
				});
				$.each(searchResult, function (i) { delete searchResult[i]._keyMatch; });
				allData.rows = searchResult;
			}
			$grid.jqGrid('clearGridData')[0].addJSONData(allData);
		}

		var ajaxOptions, preProcessData = function (data) {
			var idCol = "ID"; 										// 为了提高效率省去了计算过程 we7.findInArray(models, function (item) { return !!item.identify });
			if (idCol) {
				if (data && data.rows && data.rows.length) {
					$.each(data.rows, function (i, row) {
						data.rows[i]._gridID = row.ID.replace('{', '-_-').replace('}', '_-_'); 					// HACK: 为了提高效率直接写了 ID，而通用做法是 data.rows[i][idCol.name] 
					});
				}
			}
			return data;
		},
		processDataBeforeReq = function (data) {
			data._id = data._id.replace('-_-', '{').replace('_-_', '}');
			return data;
		},
		loadComplete = function (d) {		// 对数据进行格式化处理
			d.cols && (delete d.cols);
		},
		gridComplete = function (data) {		// ！重新加载数据完成也会激发

			function getTd(i) {
				var td = $("<td>");
				$($gridview.rows.namedItem(ids[i])).append(td);
				return td;
			}
			if (!modelCreated && !definedData) {
				mergeModel(data.cols);
				if (options.rowTemplate) {
					modelCreated = true;
				}
			} else if ($grid) {
				if (editableGrid) {
					var ids = $grid.jqGrid('getDataIDs');
					for (var i = 0; i < ids.length; i++) {
						var eid = '#' + elemId, cl = ids[i], width=0, html=''
							, be = '<span class="ui-icon ui-icon-pencil" title="编辑" style="cursor:pointer;float:left;" onclick="jQuery.jgrid._operateEditUI (\'{1}\',true);jQuery.jgrid._editRow(\'{1}\');" ></span>'
							, de = '<span class="ui-icon ui-icon-trash" title="删除" style="cursor:pointer;float:left;" onclick="jQuery.jgrid._delRow(\'{1}\')" ></span>'
							, se = '<span class="ui-icon ui-icon-disk" title="保存" style="cursor:pointer;float:left;display:none" onclick=\"jQuery(\'{0}\').saveRow(\'{1}\');jQuery.jgrid._operateEditUI (\'{1}\',false);" ></span>'
							, ce = '<span class="ui-icon ui-icon-arrowreturnthick-1-w" title="取消编辑" style="cursor:pointer;float:left;display:none" onclick=\"jQuery.jgrid._operateEditUI (\'{1}\',false);" ></span>';
						be = we7.formatStr(be, eid, cl);
						de = we7.formatStr(de, eid, cl);
						se = we7.formatStr(se, eid, cl);
						ce = we7.formatStr(ce, eid, cl);
						
						if(editableRow){
							width += 40;
							html += be;
						}
						
						if (options.deletableRow) {
							!editableRow && (width += 20);
							html += de;
						}
						
						if(editableRow){
							html += se + ce;
						}
						
						getTd(i).css("text-align","center").append($('<div>').css({"margin":"5px auto","width": width+"px"}).html(html));
					}
				}

				if (!_search) {
					_search = true;
					var titleBar = $('#gview_' + elemId).find("div.ui-jqgrid-titlebar:first");
					var searchBar = $('<div>查找：</div>').addClass("jqgrid-we7search"), searchInput = $('<input type="text" maxlength="30" style="border-style:inset" />').addClass("jqgrid-we7sinput"), searchTimer;

					titleBar.append(searchBar);
					searchInput.keyup(function () {
						var kw = this.value;
						searchTimer = setTimeout(function () {
							searchGrid(kw);
						}, 680);
					}).keydown(function () {
						if (searchTimer) {
							try { clearTimeout(searchTimer); searchTimer = undefined; }
							catch (ex) { }
						}
					}).appendTo(searchBar);
				}
			}
		},
		initGrid = function (initData) {
			options.gridComplete = gridComplete;
			initData && gridComplete(initData);
			options.loadComplete = loadComplete;
			initData && loadComplete(initData);

			$grid = $(elem).jqGrid(options);
			$gridview = $grid[0];
			initData && $gridview.addJSONData(initData); 	//初次载入时添加数据
			dataRendered = initData;
			self.grid = $grid;
		};

		buildTemplate(); 	// options.rowTemplate 意指已经 DOM 中定义的模板
		if (dataUrl) {
			options.postData && (options.postData["_rows"] = options.rowNum);
			ajaxOptions = {
				url: dataUrl
			   , type: 'POST'					// 约定的，所有请求均使用 POST [其他参数也使用]
			   , dataType: 'json'
			   , data: options.postData
			   , success: function (data, textStatus, xhr) {
			   	if (!data || (data && data.code != 200)) {
			   		triggerEvent("onLoadError", [xhr, textStatus, data]);
			   		return;
			   	}
			   	$.extend(options, {
			   		url: dataUrl
						, loadBeforeSend: function (xhr, s) {	// 事件对应 $.ajax 的 beforeSend 事件，也就是 ajax 连接已打开（URL已随请求发出），但数据尚未发送时
							var eBefore = triggerEvent("onBeforeLoad", [s]);
							if (eBefore.isDefaultPrevented()) { return false; }
							if (!definedData) {
								definedData = true;
								return false; 					// 第一次载入，取消继续
							}
						}
						, beforeProcessing: function (data, status, xhr) {
							var isError = false;
							if (!data || (data.code && data.code != 200)) {
								isError = true;
								triggerEvent("onLoadError", [xhr, status, data]);
							} else {
								data = preProcessData(data);
							}
							dataRendered = data;
						}
						, rowInjector: injectRow
			   	});
			   	data = preProcessData(data);
			   	initGrid(data, options);
			   }
			   , error: function (xhr, status, err) {
			   	triggerEvent("onLoadError", [xhr, textStatus, err]);
			   }
			};
			var bLoadNow = triggerEvent("onBeforeLoad", [ajaxOptions]);
			if (bLoadNow === false) { return false; }
			$.ajax(ajaxOptions);
			if (options.postData) {
				delete options.postData._sort;
				delete options.postData._sord;
				delete options.postData._rows;
			}
		} else {
			var bLoadNow = triggerEvent("onBeforeLoad");
			if (bLoadNow === false) { return false; }
			initGrid(data, options);
		}
	}

	/* BindOption Framework */
	var bindVerb = {
		contains: 1
		, greaterThan: 2
		, greaterOrEquals: 3
		, lessThan: 4
		, lessOrEqual: 5
		, equals: 6
		, notEqual: 7
		, notContain: 8
		, isNull: 9
		, notNull: 10

		/* extends contains */
		, beginWith: 11
		, endWidth: 12
		, notBeginWidth: 13
		, notEndWidth: 14
	};
	function BindCondition(src, operator, target) {
		this.source = src;
		this.operator = operator;
		this.target = target;
		this.toParam = function () {
			var operator = this.operator, target = this.target;
			switch (operator) {
				case bindVerb.isNull:
				case bindVerb.notNull:
					target = '';
					break;
				case bindVerb.beginWith:
				case bindVerb.notBeginWidth:
					if (operator === bindVerb.beginWidth) {
						operator = bindVerb.contains;
					} else {
						operator = bindVerb.notContain;
					}
					target = "%" + target;
					break;
				case bindVerb.endWidth:
				case bindVerb.notEndWidth:
					if (operator === bindVerb.endWidth) {
						operator = bindVerb.contains;
					} else {
						operator = bindVerb.notEndWidth;
					}
					target += "%";
					break;
				default:
					break;
			}

			return we7.formatStr('{0}@{1}@{2}', this.source, operator, target);
		};
		return this;
	}
	BindCondition.parse = function (expr) {
		we7.log('method not implemented: we7.BindCondition.parse');
	};
	function BindOption(obj) {
		var self = this;
		this.tableName = (obj && obj.tableName) || '';
		this.fields = (obj && obj.fields) || ''; 			// 定义一个 obj{Created:{}, ... } 这些对象的内容即为编辑用作 options
		this.conditions = (obj && obj.conditions) || [];
		this.sortField = (obj && obj.sortField) || '';
		this.sortOrder = (obj && obj.sortOrder) || 1;
		this.toURI = function () {
			function encode(p) {
				var ret = [];
				for (var e in p) {
					if (p.hasOwnProperty(e)) {
						ret.push(encodeURIComponent(e) + '=' + encodeURIComponent(p[e]));
					}
				}
				return ret.join('&');
			}
			return encode(this.toParam());
		};
		this.toParam = function () {
			var params = {}, cd = [], order = self.sortOrder;
			if (we7.isStr(order)) {
				if (self.sortOrder.toLowerCase() === "desc") {
					order = 1;
				} else {
					order = 0;
				}
			}

			params["_tb"] = self.tableName;
			params["_f"] = we7.isStr(self.fields) ? self.fields : (function () {
				var f = [];
				$.each(self.fields, function (k, v) {
					f.push(k);
				});
				return f.join(',');
			})();
			if (self.sortField) {
				params["_sort"] = self.sortField;
				params["_sord"] = order;
			}

			$.each(self.conditions, function (i, condition) {
				if (condition) {
					cd.push(condition.toParam());
				}
			});
			params["_c"] = cd.join('|');
			return params;
		};
		return this;
	}

	we7.BindOption = BindOption;
	we7.BindCondition = BindCondition;
	we7.bindVerb = bindVerb;

	we7.bind = function (elem, we7bindData, options) {
		if (!we7.isObj(we7bindData) || we7.isUndef(we7bindData.tableName)) {
			throw new Error('Unknow bind data');
		}
		var baseUrl = '/admin/ajax/BusinessSubmit/JsonForCondition.ashx';
		!options.postData && (options.postData = {});
		$.extend(options.postData, we7bindData.toParam());
		if (we7.isObj(we7bindData.fields)) {
			options.we7Models = we7bindData.fields;
		}
		var metaScript = $('<div type="text/we7tmpl" />').append(elem.find('tr:first').children());
		var rowTemplate = we7.browser.ff ? unescape(metaScript.html()) : metaScript.html();
		rowTemplate = jQuery.trim(rowTemplate.replace('<tbody><tr>', '').replace('</tr></tbody>', ''));
		if (rowTemplate) {
			options.rowTemplate = rowTemplate;
		}
		if (we7bindData.sortField) {
			options.sortname = options.sortname || we7bindData.sortField;
			options.sortorder = we7.isUndef(options.sortorder) ? we7bindData.sortOrder : options.sortorder;
		}
		metaScript = undefined;
		elem.empty();  /*TODO:这样就决定了对应 table 仅能调用一次。如何来保证后续绑定仍能进行？而不光依赖在 destroy 中处理？*/
		elem.show();
		return new we7bind(elem, baseUrl, options);
	};
	we7.bindJSON = function (elem, json, options) {
		return new we7bind(elem, json, options);
	};

	window.we7.extend({
		"bind": function (binding, options) {
			return we7.bind(this.jquery, binding, options);
		},
		"bindJSON": function (json, options) {
			return we7.bindJSON(this.jquery, json, options);
		}
	});

})();