/**
*拖拽添加控件到iframe中
*
*/

/*
*默认参数
*/
DragNewWidget.prototype.defaultOptions = {
    placeholder:"placehodler" ,     //占位符CLASS
    frame:"pageframe",       //frame的id
    dragWidget:".dragitem",
    helper:function(){return $('<div class="helper" style="height:20px; width:150px; z-index:100000;background-color:#A00;color:Yellow; border:solid 1px Yellow; text-align:center">要拖我到哪里啊？</div>');},
    columns: ".RadDockZone",         //可拖拽的列
    items: ">.RadDock"
};
/**
*插入控件
*/
function DragNewWidget(options) {
    
    var Me = this;
    //合并参数
    this.options = $.extend({}, this.defaultOptions, options);
    this.frame = this.options.frame;
    this.columns = this.options.columns;
    this.items = this.options.items;
    //操作placeholder
    var _createPlaceholder = function () {
   
        $("#" + Me.frame).contents().find("body").append('<div style="border:dashed 1px gray;background-color:#FFFF00;width:100%;height:20px;" class="' + Me.options.placeholder + '"></div>');
        
    };

    var _getPlaceholder = function () { return $("#" + Me.frame).contents().find("." + Me.options.placeholder); };

    var _deletePlaceholder = function () { $("#" + Me.frame).contents().find("." + Me.options.placeholder).remove(); };

    //计算鼠标是否在iframe中
    var _hoverFrame = function (event) {

        var pageX = event.pageX;
        var pageY = event.pageY;
        var left = $("#" + Me.frame).offset().left;
        var top = $("#" + Me.frame).offset().top;
        var width = $("#" + Me.frame).outerWidth();
        var height = $("#" + Me.frame).outerHeight();

        if (pageX >= left && pageX <= left + width && pageY >= top && pageY <= top + height) {
            return true;
        }
        else {
            return false;
        }
    };
        //计算鼠标是否在中容器中
    var checkMouseOver = function (event, column) {

        var pageX = event.pageX - $("#" + Me.frame).offset().left;
        var pageY = event.pageY - $("#" + Me.frame).offset().top;
        var left = $(column).offset().left;
        var top = $(column).offset().top;
        var width = $(column).outerWidth();
        var height = $(column).outerHeight();

        if (pageX >= left && pageX <= left + width && pageY >= top && pageY <= top + height) {
            return true;
        }
        else {
            return false;
        }
    }
    /**
    *检查鼠标在DOM元素的上半还是下半还是不在
    *@返回1表示在上，2在下，0则不在
    */
   var getAbsolutePosition = function (element) {
        element = $(element).get(0);
        var r = { x: element.offsetLeft, y: element.offsetTop };

        if (element.offsetParent) {
            var tmp = getAbsolutePosition(element.offsetParent);
            r.x += tmp.x;
            r.y += tmp.y;
        }
        return r;
    };
    var checkElement = function (event, element) {

        var pageY = event.pageY-$("#"+Me.frame).offset().top;
        var height = $(element).outerHeight();
        var top = $(element).offset().top;
       
        if (pageY >= top && pageY <= top + height) {

            if (pageY <= top + (height / 2)) {
                return 1;
            }
            else {
                return 2;
            }
        } else {
            if (pageY < top) {
                return -1;
            }
            else {
                return 0;
            }
        }
    }

    this.refresh = function () {

        $(Me.options.dragWidget).draggable({
            iframeFix: true,
            revert: 'invalid',
            //containment:"document",
            appendTo:"body",
            helper: Me.options.helper,
            start: function (event, ui) {

            },
            drag: function (event, ui) {
                ui.item = this;
                //1.判断鼠标是否在iframe中,如果在则添加占位符

                if (_hoverFrame(event)) {

                    //2.计算在哪个列中
                    //1.遍历所有容器,查看鼠标中心位于哪个column中,可以为套嵌

                    var cols = $("#" + Me.frame).contents().find(Me.columns);
                    var overCols = [];
                    var hoverCol;
                    //获取鼠标
                    for (var i = 0; i < cols.length; i++) {
                        //console.log($("#frame1").contents().find("#layout_col_1").offset().top);
                        var hover = checkMouseOver(event, cols[i]);
                        if (hover) {
                            //在容器中
                            overCols.push(cols[i]);
                        }
                    }
                    //2.可能为套嵌容器,判断最终在哪个容器:哪个比较高度比较低就在该上
                    var tempHeight = 1000000; //一个比较大的值 用于初始比较

                    for (var i = 0; i < overCols.length; i++) {
                        //比较
                        if ($(overCols[i]).outerHeight() < tempHeight) {
                            hoverCol = overCols[i];
                            tempHeight = $(overCols[i]).outerHeight();
                        }
                    }

                    if (hoverCol != null) {

                        //4.判断在哪个节点
                        var currentNode = null;
                        var postion = 0;
                        var children = $(Me.items, hoverCol);

                        for (var i = 0; i < children.length; i++) {
                            var p = checkElement(event, children[i]);
                            if (p > 0) {
                                currentNode = children[i];
                                postion = p;
                                break;
                            }
                        }

                        if (_getPlaceholder().size() == 0) {
                            //创建
                            _createPlaceholder();
                        }
                        var placeholder = _getPlaceholder();
                        if (currentNode != null) {
                            if (postion == 1) {
                                $(placeholder).insertBefore(currentNode);
                            }
                            else if (postion == 2) {
                                $(placeholder).insertAfter(currentNode);
                            }
                            else {
                                $(placeholder).insertAfter(currentNode);
                            }
                        }
                        else {
                            placeholder.appendTo(hoverCol);
                        }
                    }
                }
                else {
                    //不在区域内则删除占位符
                    $(_getPlaceholder()).remove(); //.remove();
                }
            },
            stop: function (event, ui) {
                ui.item = this;

                if ($(_getPlaceholder()).size() > 0) {

//                    //存在占位符
                    ui.placeholder=$(_getPlaceholder());
                      Me.options.add(event,ui);
               }
            }
        });
    }
    this.refresh();
}
