function showhint(iconid, str)
{
	var imgUrl='../images/hint.gif';
	if (iconid != 0)
	{
		imgUrl = '../images/warning.gif';
	}
	document.write('<div style="background:url(' + imgUrl + ') no-repeat 20px 10px;border:1px dotted #DBDDD3; background-color:#FDFFF2; margin-bottom:10px; padding:10px 10px 10px 56px; text-align: left; font-size: 12px;">');
	document.write(str + '</div><div style="clear:both;"></div>');
}

function showloadinghint(divid, str)
{
	if (divid=='')
	{
		divid='PostInfo';
	}
	document.write('<div id="' + divid + ' " style="display:none;position:relative;border:1px dotted #DBDDD3; background-color:#FDFFF2; margin:auto;padding:10px" width="90%"  ><img border="0" src="../images/ajax_loading.gif" /> ' + str + '</div>');
}


function CheckByName(form,tname,noname)
{
  for (var i=0;i<form.elements.length;i++)
    {
	    var e = form.elements[i];
	    if(!e.name) continue;
	    if(e.name.indexOf(tname)>=0)
		{
		   if(noname!="")
           {
              if(e.name.indexOf(noname)>=0) ;
              else
              {
                 e.checked = form.chkall.checked;
                // alert(e.name+' '+form.chkall.checked);
              }
             
		   }	  
		   else
		   {
		      e.checked = form.chkall.checked;   
		   }
	    }
	}
}


function isMaxLen(o)
{
	var nMaxLen=o.getAttribute? parseInt(o.getAttribute("maxlength")):"";
	if(o.getAttribute && o.value.length>nMaxLen)
	{
		o.value=o.value.substring(0,nMaxLen)
	}
}
    

//显示提示层
function showhintinfo(obj, objleftoffset,objtopoffset, title, info , objheight, showtype ,objtopfirefoxoffset)
{
   
   var p = getposition(obj);
   
   if((showtype==null)||(showtype =="")) 
   {
       showtype =="up";
   }
   document.getElementById('hintiframe'+showtype).style.height= objheight + "px";
   document.getElementById('hintinfo'+showtype).innerHTML = info;
   document.getElementById('hintdiv'+showtype).style.display='block';
   
   if(objtopfirefoxoffset != null && objtopfirefoxoffset !=0 && !isie())
   {
        document.getElementById('hintdiv'+showtype).style.top=p['y']+parseInt(objtopfirefoxoffset)+"px";
   }
   else
   {
        if(objtopoffset == 0)
        { 
			if(showtype=="up")
			{
				 document.getElementById('hintdiv'+showtype).style.top=p['y']-document.getElementById('hintinfo'+showtype).offsetHeight-40+"px";
			}
			else
			{
				 document.getElementById('hintdiv'+showtype).style.top=p['y']+obj.offsetHeight+5+"px";
			}
        }
        else
        {
			document.getElementById('hintdiv'+showtype).style.top=p['y']+objtopoffset+"px";
        }
   }
   document.getElementById('hintdiv'+showtype).style.left=p['x']+objleftoffset+"px";
}



//隐藏提示层
function hidehintinfo()
{
    document.getElementById('hintdivup').style.display='none';
    document.getElementById('hintdivdown').style.display='none';
}



//得到字符串长度
function getLen( str) 
{
   var totallength=0;
   
   for (var i=0;i<str.length;i++)
   {
     var intCode=str.charCodeAt(i);   
     if (intCode>=0&&intCode<=128)
     {
        totallength=totallength+1; //非中文单个字符长度加 1
	 }
     else
     {
        totallength=totallength+2; //中文字符长度则加 2
     }
   } 
   return totallength;
}   
   


function getposition(obj)
{
	var r = new Array();
	r['x'] = obj.offsetLeft;
	r['y'] = obj.offsetTop;
	while(obj = obj.offsetParent)
	{
		r['x'] += obj.offsetLeft;
		r['y'] += obj.offsetTop;
	}
	return r;
}

  

function cancelbubble(obj)
{
    //<textarea style="width:400px"></textarea>
    //var log = document.getElementsByTagName('textarea')[0];
	var all = obj.getElementsByTagName('*');
	
	for (var i = 0 ; i < all.length; i++)
	{
	    //log.value +=  all[i].nodeName +":" +all[i].id + "\r\n";
		all[i].onmouseover = function(e)
		{
    		if (e) //停止事件冒泡
	    	    e.stopPropagation();
		    else
			    window.event.cancelBubble = true;
			
			obj.style.display='block';
			//this.style.border = '1px solid white';
			//log.value = '鼠标现在进入的是： ' + this.nodeName + "_" + this.id;
		};
		
		all[i].onmouseout = function(e)
		{
		    if (e) //停止事件冒泡
			    e.stopPropagation();
		    else
			    window.event.cancelBubble = true;
			
	 
			if(this.nodeName == "DIV")
			{
			    obj.style.display='none';
			}
//			else
//			{
//			    obj.style.display='none';
//			}
			//this.style.border = '1px solid white';
			//log.value = '鼠标现在离开的是：' + this.nodeName + "_" + this.id;
	    };
	}

}