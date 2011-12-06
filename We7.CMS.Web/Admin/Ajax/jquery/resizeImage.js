//----------------------------------------------------------------
// <copyright file="resizeImage.js" >
//    Copyright (c) All rights reserved.
// </copyright>
//----------------------------------------------------------------

//缩放代码
function bigSmall() {
	var size = $(this).attr("id") == "morebig" ? 0.01 : -0.01;
	var value = parseFloat($("#txt_Zoom").val());
	var temp = value + size;
	//debugger;
	if (temp <= 2 && temp >= 0) {
		$("#txt_Zoom").val((value + size).toString());
		var width = parseInt($("#width").text());
		var height = parseInt($("#height").text());
		// $("#o_image").css({ width:parseInt(width*temp)+ "px", height:parseInt(height*temp) + "px" });
		$("#bg_image").css({ width: parseInt((width * temp)) + "px", height: parseInt((height * temp)) + "px" });
		$("#o_image").css({ width: parseInt((width * temp)) + "px", height: parseInt((height * temp)) + "px" });
		$("#drop_image").css({ width: parseInt((width * temp)) + "px", height: parseInt((height * temp)) + "px" });
		$("#txt_width").val($("#o_image").css("width").replace(/px/, ""));
		$("#txt_height").val($("#o_image").css("height").replace(/px/, ""));
		$(".child").css({ left: parseInt($(".child").eq(0).css("left").replace(/px/, "")) + size * 100 + "px" });
	}
}
//初始化
$(document).ready(
          function () {
          	//debugger;
          	var width = parseInt($("#width").text())//图片的原长宽
          	var height = parseInt($("#height").text());
          	//将图片长宽输入textbox中
          	$("#txt_DropWidth").val($("#drop").css("width").replace("px", ""));
          	$("#txt_DropHeight").val($("#drop").css("height").replace("px", ""));

          	$("#drop_image").css({ left: "-101px", top: "-101px" }); //将截取框内的图片移动到适合位置，注意截取框的1px边框
          	//设置div的拖动功能
          	$("#bg_image").draggable({ cursor: 'move',
          		drag: function (e, ui) {
          			//debugger;
          			var self = $(this).data("draggable");
          			var drop_image = $("#drop_image");
          			var top = $("#drop_image").css("top").replace(/px/, ""); //取出截取框到顶部的距离
          			var left = $("#drop_image").css("left").replace(/px/, ""); //取出截取框到左边的距离
          			drop_image.css({ left: (parseInt(self.position.left) - 101) + "px", top: (parseInt(self.position.top) - 101) + "px" }); //同时移动内部的图片
          			//drop_image.style.backgroundPosition = (self.position.left - parseInt(left)-1) + 'px ' + (self.position.top - parseInt(top)-1) + 'px';
          			$("#txt_left").val(99 - parseInt($(this).css("left")));
          			$("#txt_top").val(99 - parseInt($(this).css("top")));
          		}

          	});

          	$("#drop_image").draggable(
                            { cursor: 'move',
                            	drag: function (e, ui) {
                            		var self = $(this).data("draggable");
                            		var divimage = $("#bg_image");
                            		//divimage.style.backgroundPosition = parseInt((self.position.left))*300 + 'px ' + parseInt((self.position.top))*300 + 'px';
                            		divimage.css({ left: (parseInt(self.position.left) + 101) + "px", top: (parseInt(self.position.top) + 101) + "px" }); //同时移动div

                            		$("#txt_left").val(99 - parseInt($("#bg_image").css("left")));
                            		$("#txt_top").val(99 - parseInt($("#bg_image").css("top")));
                            	}
                            });
          	$("#bg_image").css({ opacity: 0.3, backgroundColor: "#fff", width: width + "px", height: height + "px" });
          	$("#txt_top").val("100");
          	$("#txt_left").val("100");
          	$("#txt_width").val(width);
          	$("#txt_height").val(height);
          	$(".smallbig").click(bigSmall);
          	//缩放的代码
          	$(".child").draggable(
              {
              	cursor: "move", containment: $("#bar"),
              	drag: function (e, ui) {
              		var left = parseInt($(this).css("left"));
              		var value = 1 + (left - 100) / 100;
              		$("#txt_Zoom").val(value);
              		$("#o_image,bg_image").css({ width: parseInt(width * value) + "px", height: parseInt(height * value) + "px" });
              		$("#bg_image").css({ width: parseInt(width * value) + "px", height: parseInt(height * value) + "px" });
              		$("#drop_image").css({ width: parseInt(width * value) + "px", height: parseInt(height * value) + "px" });
              		$("#txt_width").val($("#o_image").css("width").replace(/px/, ""));
              		$("#txt_height").val($("#o_image").css("height").replace(/px/, ""));
              	}
              });

          }
        );

function changeFrame(v) {
	var s = v.options[v.selectedIndex].text;
	var size = s.split('：')[1];
	if (size) {
		var list = size.split('*');
		if (list.length > 1) {
			var width = list[0];
			var height = list[1];
		}
		var top = (420 - height) / 2;
		var left = (420 - width) / 2;
		$("#drop").css({ width: parseInt((width)) + "px", height: parseInt((height)) + "px" });
		$("#txt_DropWidth").val(width);
		$("#txt_DropHeight").val(height);
		//debugger;
		saveImageSizeToCookies(v);
	}
}