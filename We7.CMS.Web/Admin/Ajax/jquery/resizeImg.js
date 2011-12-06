//----------------------------------------------------------------
// <copyright file="resizeImg.js" >
//    Copyright (c) All rights reserved.
// </copyright>
//----------------------------------------------------------------

        //缩放代码
        function bigSmall()
        {
          var size=$(this).attr("id")=="morebig"?0.01:-0.01;
          var value=parseFloat($("#txt_Zoom").val());
          var temp=value+size;
          //debugger;
          if(temp<=2)
          {
          $("#txt_Zoom").val((value+size).toString());
          var width=parseInt($("#width").text());
          var height=parseInt($("#height").text());
        // $("#o_image").css({ width:parseInt(width*temp)+ "px", height:parseInt(height*temp) + "px" });
          $("#photo").css({ width: parseInt((width * temp)) + "px", height: parseInt((height * temp)) + "px" });
         //$("#drop_image").css({ width:parseInt((width*temp))+ "px", height:parseInt((height*temp)) + "px" });
          $("#txt_width").val($("#photo").css("width").replace(/px/, ""));
         $("#txt_height").val($("#photo").css("height").replace(/px/, ""));
         $(".child").css({left:parseInt($(".child").eq(0).css("left").replace(/px/,""))+size*100+"px"});
         }
        }
        //初始化
    function changeFrame(v) {
        var s=v.options[v.selectedIndex].text;
        var size=s.split('：')[1];
        if (size) {
            var list = size.split('*');
	        if (list.length > 1) {
		        var width = list[0];
		        var height = list[1];
	        }
            var top= (420-height)/2;
            var left =(420-width)/2;
            //$("#txt_DropWidth").val(width);
            //$("#txt_DropHeight").val(height);
            $("#w").val(width);
            $("#h").val(height);
            $.extend($.imgAreaSelect, {
                animate: function(fx) {
                    var start = fx.elem.start, end = fx.elem.end, now = fx.now,
            curX1 = Math.round(start.x1 + (end.x1 - start.x1) * now),
            curY1 = Math.round(start.y1 + (end.y1 - start.y1) * now),
            curX2 = Math.round(start.x2 + (end.x2 - start.x2) * now),
            curY2 = Math.round(start.y2 + (end.y2 - start.y2) * now);
                    fx.elem.ias.setSelection(curX1, curY1, curX2, curY2);
                    fx.elem.ias.update();
                },
                prototype: $.extend($.imgAreaSelect.prototype, {
                    animateSelection: function(x1, y1, x2, y2, duration) {
                        var fx = $.extend($('<div/>')[0], {
                            ias: this,
                            start: this.getSelection(),
                            end: { x1: x1, y1: y1, x2: x2, y2: y2 }
                        });

                        if (!$.imgAreaSelect.fxStepDefault) {
                            $.imgAreaSelect.fxStepDefault = $.fx.step._default;
                            $.fx.step._default = function(fx) {
                                return fx.elem.ias ? $.imgAreaSelect.animate(fx) :
                        $.imgAreaSelect.fxStepDefault(fx);
                            };
                        }

                        $(fx).animate({ cur: 1 }, duration, 'swing');
                    }
                })
            });

            $(function() {
                ias = $('img#photo').imgAreaSelect({ fadeSpeed: 400, handles: true,
                    instance: true
                });

                $('.sizeDropClass').change(function() {
                    // If nothing's selected, start with a tiny area in the center
                if (!ias.getSelection().width)
                    ias.setOptions({ show: true, x1: 199, y1: 149, x2: width, y2: height });
                ias.animateSelection(100, 75, parseInt(100) + parseInt(width), parseInt(75) + parseInt(height), 'slow');
                });
            });
           
            saveImageSizeToCookies(v);
        }
    }

  