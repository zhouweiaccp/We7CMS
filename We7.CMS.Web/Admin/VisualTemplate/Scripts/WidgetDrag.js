/// <reference path="jquery-1.4.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.5.custom.min.js" />
function checkMouseOver(pageX, pageY, left, top, width, height) {
    if (pageX >= left && pageX <= left + width && pageY >= top && pageY <= top + height) {
        return true;
    }
    else {
        return false;
    }
 };
 
 
function getAbsolutePosition(element) {
    element=$(element).get(0);
    var r = { x: element.offsetLeft, y: element.offsetTop };
  
    if (element.offsetParent) {
      var tmp = getAbsolutePosition(element.offsetParent);
      r.x += tmp.x;
      r.y += tmp.y;
    }
    return r;
 };
function DragWidget(options) {

    this.options = options;
    this.Placeholder = $(this.options.iframeId).contents().find("#placeholder");

    var iframeLeft = $(this.options.iframeId).offset().left;
    var iframeTop = $(this.options.iframeId).offset().top;
    var This=this;
    //this.columns=$(this.options.iframeId).contents().find(".columns");

    $(this.options.items).draggable({
        iframeFix: true,
        revert: true,
        helper: "clone",
        drag: function (event, ui) {
            if (checkMouseOver(event.pageX, event.pageY, iframeLeft, iframeTop, $(This.options.iframeId).width(), $(This.options.iframeId).height())) {
                //进入容器
                var el = ui.helper.get(0);
                var pageX = event.pageX - parseInt(iframeLeft);
                var pageY = event.pageY - parseInt(iframeTop);
                var cols = $(This.options.iframeId).contents().find(This.options.columns);
                var overCols = new Array();       //保存在Over的容器
                $(cols).each(function (i) {
                    var p = getAbsolutePosition(this);
                    if (checkMouseOver(pageX, pageY, p.x, p.y, $(this).width(), $(this).height())) {
                        overCols.push(cols[i]);
                    }

                });
                var overColsCount = overCols.length;
                var overCol = null;
                var tempHeight = 100000;
                if (overColsCount == 0) {
                    //return;
                }
                else if (overColsCount == 1) {
                    overCol = overCols[0];
                }
                else if (overColsCount > 1) {
                    for (var i = 0; i < overColsCount; i++) {
                        var colHeight = $(overCols[i]).height();

                        if (colHeight < tempHeight) {
                            overCol = overCols[i];
                            tempHeight = colHeight;
                        }
                    }
                };
                if (overCol != null) {
                    //判断在该column哪个位置
                    var top = parseInt($(el).offset().top-iframeTop);

                    var currentNode = null;
                    var children = $(This.options.its, overCol);
                    for (var i = 0; i < children.length; i++) {
                       
                        if (top < parseInt($(children[i]).offset().top)) {
                            currentNode = children[i];
                          
                            break;
                        }
                    }

                };

                if (currentNode) {

                  
                    //插入下方
                   
                    This.Placeholder.insertAfter(currentNode).height("20").css("display", "");
                }
                else {
                    //没有元素
                   
                    $(This.options.iframeId).contents().find("#placeholder").appendTo($(overCol)).css("height", 20).show();
                }
            }
            else {
                $(This.options.iframeId).contents().find("#placeholder").appendTo($(This.options.iframeId).contents().find("body")).hide();
            }
        },
        stop: function (event, ui) {
            var placehodler = $(This.options.iframeId).contents().find("#placeholder");
            if (placehodler.css("display") != "none") {
                try {
                    if(options.stop!=null)
                    {
                        options.stop(this);
                    }
                 
                 
                    $(placehodler).appendTo($(This.options.iframeId).contents().find("body")).hide();
                }
                catch (err) {
                    alert(err);
                }
            }
        }
    });
}
function initWidget(){
    var widgets = new DragWidget({
        iframeId:"#pageframe",
        items:"li",
        columns: ".RadDockZone",
        its:">.RadDock",
        stop:function(item){
         //参数
         var data=$(item).attr("data");
         
         data=unescape(data);
         
         var params=stringToJSON(data);
         
         params.id=generatorId(params.Control);
 
         var loading=$('<img src="/Admin/VisualTemplate/images/ajax-loader.gif"/>');
         var placehodler = $(this.iframeId).contents().find("#placeholder");
       
         
         loading.insertBefore(placehodler);
         var Me=this;
         //插入
         WidgetController.add(params,function(id,json){
            if (json.Success) {
                     var head = json.Data.head;
                     if (head) {
                         head = StringUtil.Base64Decode(head);
                        $(Me.iframeId).contents().find("head").append(head);

                     }
                     var body = json.Data.body;
                     if (body) {
                         body = StringUtil.Base64Decode(body);
                         writeCapture.replaceWith(loading, body);
                     }
                      document.getElementById("pageframe").contentWindow.refresh ();
                     //document.getElementById("pageframe").contentWindow.AddWidget(id+"_widget");
                     loading.remove();

                 }
                 else {
                     alert(json.Message);
                     loading.remove();
                 }
                 resizeContent();
         });
        }
    });
}

//绑定事件
function BindEvent()
{
 
}
/**
*widget操作
*/
var WidgetController = {
    /**
    **添加
    */
    add: function (params,callback) {
        var paramsString = jsonToString(params);
        var id=params.id;
        paramsString = StringUtil.Base64Encode(paramsString);
        $.ajax(
         {
             async: true,
             type: "POST",
             url: "/Admin/VisualTemplate/RenderWidget.aspx?action=add&state=design1&params=" + paramsString,
             dataType: "json",
             success: function (json) {
                 //进行处理
                 callback(id,json);
             }
         });
    }
   ,edit:function(data)
   {
    var folder=Request.parameter("folder");
    var template=Request.parameter("file");
      data = stringToJSON(data);
      
       builder.showBuilder(data.data.atts.File,template,folder,data.data.atts);
   }
};


var builder = null;
$(function() {
    builder = new TemplateBuilder({
        ShowListSuccess: function(result) {
            var data = jsonToString(result);
        },
        ShowListFailure: function() {alert("ShowListFailure"); },
        ShowBuilderSuccess: function(result) {
        var ctr=result.ctr;
        var temp=ctr.split(".")[0];
        var controlPath="/We7Controls/"+temp+"/Page/"+ctr+".ascx";
         var contrilId=tempControlid;
         var controlType="system";
         var attributes=StringUtil.Base64Encode(jsonToString(result));
         
         EditWidgetSuccess(controlPath,controlType,attributes,contrilId);
           
        },
        ShowBuilderFailure: function() { alert("ShowBuilderFailure"); }
    });
});
