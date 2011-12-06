/*弹出式菜单*/
//没剑 2008-07-02
//http://regedit.cnblogs.com
/*参数说明*/
//showobj:要显示的菜单ID
//timeout：延时时间,鼠标停留/离开后延时多久开始显示/隐藏菜单
//speed：菜单显示速度，数字越大，显示越慢,默认为100
//调用示例:$("#button").DMenu("#content");
jQuery.fn.DMenu=function(showobj,timeout,speed){
	timeout=timeout?timeout:300;
	speed=speed?speed:100;
	//按钮对象
	var button=$(this);
	//延时计数器
	var timer=null;
	//隐藏的浮动层
	var hideDiv=$("<div></div>");
	//容器对象
	var Container=$("<div id=\"Container\"></div>");
	Container.hide();
	hideDiv.append(Container);
	//菜单对象
	var jqShowObj=$(showobj);
	//隐藏菜单
	jqShowObj.hide();
	//菜单显示的状态
	var display=false;
	//按钮的offset
	var offset=button.offset();
	//菜单区高
	var height=jqShowObj.height();
	//菜单区宽
	var width=jqShowObj.width();
	//按钮的高
	var btnHeight=button.height();
	//按钮的宽
	var btnWidth=button.width();
	//定位层放到最前面
	$(document.body).prepend(hideDiv);
	//放到容器中
	//Container.append(jqShowObj);

	//****显示菜单方法开始****//
	var showMenu=function(){
		//如果菜单为显示则退出操作
		if (display)
		{
			return false;
		}
		//设置容器属性
		Container.css({
			margin:"0 auto",
			width:btnWidth+"px",
			height:btnHeight+"px"
		});
		//定位隐藏层
		hideDiv.css({
			position:"absolute",
			top:offset.top+"px",
			left:offset.left+(btnWidth/2)-(width/2)+"px",
			height:height+"px",
			width:width+"px"
		}).show();
		//给容器加个黑边框
		Container.css({
			border:"1px solid #666666"
		});
		//显示定位层
		//高宽慢慢增大
		Container.animate({
			marginTop:btnHeight+4,
			height:height+4,
			width:width+4,
			opacity:'100'},speed,function(){
			//动画结束时 start//
			//显示菜单
			jqShowObj.show();
			//添加菜单入容器
			Container.append(jqShowObj);
			//去除边框
			Container.css({
				border:"0px"
			});
			//显示状态置为true
			display=true;
			//鼠标移入
			jqShowObj.mouseover(function(){
					clearTimeout(timer); 
			});
			//鼠标移开
			jqShowObj.mouseout(function(){
				hideMenu();
			});
			//动画结束时 end//
		});
	};
	//****显示菜单方法结束****//

	//****隐藏菜单方法开始****//
	var hideMenu=function(){
		clearTimeout(timer); 
		//延时隐藏菜单
		timer=setTimeout(function(){
		//显示边框
		Container.css({
			border:"1px solid #666666"
		});
		//清空容器
		Container.empty();
		//收缩容器
		Container.animate({
			  width:btnWidth,height:btnHeight,marginTop:'0', opacity: '0'
			}, speed,function(){
			//动画结束时 start//
			//隐藏容器
			Container.hide();
			//定位层隐藏
			hideDiv.hide();
			//显示状态置为false
			display=false;
			//动画结束时 end//
		});
		}, timeout); 
	};
	//****隐藏菜单方法结束****//

	//绑定按钮鼠标经过事件
	button.hover(function(e){
		//延时显示菜单
		clearTimeout(timer); 
		timer=setTimeout(function(){
			showMenu();
		}, timeout); 
	},function(){
		clearTimeout(timer); 
		//鼠标离开按钮时，如果菜单还是显示状态则隐藏
		if(display){
			timer=setTimeout(function(){
				hideMenu();
			},timeout);
		}
	});
};