function dialogHelper (doc) {

    var DOC = doc;
    var me = this;
    var topDIV = null;
    var leftDIV = null;
    var contentDIV = null;
    var rightDIV = null;
    var bottomDIV = null;   
    var documentWidth = 1024;
    var documentHeight = 768;
    var width = 700;
    var height = 400;
    var parameter = null;
    var isIE = navigator.appName == "Microsoft Internet Explorer";
    
    
    if( isIE) {
        documentWidth = doc.body.offsetWidth;
        documentHeight = doc.body.offsetHeight;
    }
    else {
        documentWidth = window.innerWidth;
        documentWidth = window.innerHeight;
    }

    this.setParameter = function(p) {
        parameter = p;  
    }
    
    this.getParameter = function() {
        return parameter;
    }

    this.setDocumentWidth = function(w) {
        me.documentWidth = w;
    }
    
    this.getDocumentWidth = function() {
        return me.documentWidth;
    }
    
    this.getDocumentHeight = function() {
        return me.documentHeight;
    }
    
    this.setDocumentHeight = function(h) {
        me.documentHeight = h;
    }
    
    this.setWidth = function(w) {
        me.width = w;
    }
    
    this.getWidth = function() {
        return me.width;
    }
    
    this.getHeight = function() {
        return me.height;
    }
    
    this.setHeight = function(h) {
        me.height = h;
    }
    
    this.getURL = function() {
        return me.URL;
    }
    
    this.setURL = function(url) {
        me.URL = url;
        if(frame) {
            frame.src = "about:blank";
            frame.src = url;
        }
    }
    
    this.refresh = function() {       
        var W = width;
        var H = height;
        var DW = documentWidth;
        var DH = documentHeight;
        var TH = me.MAX(0, (DH - H)/2);
        var TW = me.MAX(0, (DW - W)/2);  
        
        me.adjustDIV(topDIV, 0, 0, DW, TH);
        me.adjustDIV(leftDIV, 0, TH, TW, H);
        me.adjustDIV(contentDIV, TW, TH, W, H);
        me.adjustDIV(rightDIV, TW + W, TH, TW, H);
        me.adjustDIV(bottomDIV, 0, TH + H, DW , TH);
    }

    this.show = function() {
        me.showDIVs(true);
    }

    this.hide = function() {
        me.showDIVs(false);
    }
    
    this.create = function() {    
        topDIV = me.createDIV("both", 9991);
        leftDIV = me.createDIV("none", 9992);
        contentDIV = me.createDIV("none", 9993);
        rightDIV = me.createDIV("none", 9994);
        bottomDIV = me.createDIV("both", 9995);
            
        me.processOpacity(topDIV);
        me.processOpacity(leftDIV);
        me.processOpacity(rightDIV);
        me.processOpacity(bottomDIV);        
        
        frame = DOC.createElement("IFRAME");
        frame.style.width = "100%";
        frame.style.height = "100%";
        frame.src = me.URL;
        me.showDIVs(false);
  
        DOC.body.appendChild(topDIV);
        DOC.body.appendChild(leftDIV);
        DOC.body.appendChild(contentDIV);
        DOC.body.appendChild(rightDIV);
        DOC.body.appendChild(bottomDIV);
        contentDIV.appendChild(frame);

        me.refresh();
    }   
        
    this.free = function() {
        contentDIV.removeChild(frame);
        DOC.removeChild(topDIV);
        DOC.removeChild(leftDIV);
        DOC.removeChild(contentDIV);
        DOC.removeChild(rightDIV);
        DOC.removeChild(bottomDIV);
        
        topDIV = null;
        leftDIV = null;
        contentDIV = null;
        rightDIV = null;
        bottomDIV = null;
    }   
    
    this.showDIV = function(div, isShow) {
        div.style.display = isShow ? "block" : "None";
    }
    
    this.showDIVs = function(isShow) {
        me.showDIV(topDIV, isShow);
        me.showDIV(leftDIV, isShow);
        me.showDIV(contentDIV, isShow);
        me.showDIV(rightDIV, isShow);
        me.showDIV(bottomDIV, isShow);
    }   
    
    this.createDIV = function(c, z) {
        var div = DOC.createElement("DIV");
        div.style.position = "absolute";
        div.style.backgroundColor = "black";
        div.style.zIndex = z;
        div.style.clear = c;
        div.style.display = "none";
        return div;
    }
    
    this.processOpacity = function(div) {
       if(isIE) {
            div.style.filter = "alpha(opacity=45);";
       }
       else {
        div.style.MozOpacity = 75;
       }
    }

    this.adjustDIV = function(div, l, t, w, h) {
        div.style.left = l + "px";
        div.style.top = t + "px";
        div.style.width = w + "px";
        div.style.height = h + "px";
   }  
   
   this.MAX = function(v1, v2) {
        return v1 > v2 ? v1 : v2;
    }
    
    this.create();
    this.hide();
}



var __DD = null;
var __DF = null;
var __isShowing = false;

function showDialog(url, func, para) {
    if(!__DD) {
        __DD = new dialogHelper(document);
    }
    __DF = func;
    __DD.setURL(url);
    __DD.setParameter(para);
    __DD.show();
    __isShowing = true;
}

function showDialogEx(url, func, width,height,para) {
    if(!__DD) {
        __DD = new dialogHelper(document);
    }
    __DF = func;
    __DD.setURL(url);
    __DD.setWidth(width);
    __DD.setHeight(height); 
   __DD.setParameter(para); 
    __DD.show();
    __isShowing = true;
}

function getParameter() {
    return __DD.getParameter();    
}

function handleDialogCallback(v, t) {
    if(__DD) {
        __DD.hide();
        __isShowing = false;
        if(__DF) {
            __DF(v, t);
        }
    }
}

function closeDialog(v, t) {

    if(v == null) {
        window.returnValue = null;
    }
    else {
        window.returnValue = v + ";" + t;
    }
    var doc = window.parent;
    if(doc == null) {
        doc = top;
    }
    if(doc) {
        if(doc && doc.handleDialogCallback) {
            doc.handleDialogCallback(v, t);            
        }
    }
    if(window.opener) {
        window.close();
    }
}

function MaxTheWindow()
{
    window.moveTo(-4,-4)
    window.resizeTo(screen.availWidth+9,screen.availHeight+9)
}

/*
*
* 判断浏览器类型
*
*/
function getOs()
{
    if(navigator.userAgent.indexOf("MSIE")>0)
    {
         return "MSIE";
    }
    else if(isFirefox=navigator.userAgent.indexOf("Firefox")>0)
    {
         return "Firefox";
    }
    else if(isSafari=navigator.userAgent.indexOf("Safari")>0)
    {
         return "Safari";
    }
    else if(isCamino=navigator.userAgent.indexOf("Camino")>0)
    {
         return "Camino";
    }
    else if(isMozilla=navigator.userAgent.indexOf("Gecko/")>0)
    {
         return "Gecko";
    }
    else
    {
    return "";
    }
}
   
function showNotice(objDiv)
{
   if(objDiv)
   {
        if(objDiv.style.display=="none")
        {
            if(getOs()=="MSIE")
             {
                objDiv.style.display = "";
             }
             else
             {
                objDiv.style.display = "table";
             } 
        }
        else
            objDiv.style.display="none";
   } 
}
/**********************************************
*使用模式窗口代替iframe的解决方案 
*thehim 2008-3-1
*function showDialog(url, func, para)
* url:窗口地址
***********************************************/

var __WE7DOCFUNC = null;

function weShowModelDialog(url, func, para, width, height) {

    var mask=document.getElementById("mask");
    if(mask)
        mask.style.visibility='visible';
    //var ret=window.showModalDialog(url,window,"dialogWidth:700px;dialogHeight:500px;center:yes;status:no;scroll:auto;help:no;");
    var ret;
    if(func)
        __WE7DOCFUNC=func;
        
    if(width && height)
    {
        ret=popupDialog(url,width,height);
    }
    else
    {
        ret=popupDialog(url,700,500);
    }
    
    
//    if(ret !=null && ret !="null" && func){
//        var arry = new Array();
//        arry = ret.split(",");
//        func(arry[0],arry[1],para);
//    }
    if(mask)
        mask.style.visibility='hidden';
    return ret;
}

function weCloseDialog(v, t){
   
    if(v == null) {
        window.returnValue = null;
    }
    else {
        window.returnValue = v + ";" + t;
    }
    
    var isMSIE= (navigator.appName == "Microsoft Internet Explorer");  //判断浏览器  
    var doc=null;
    if(isMSIE)
        doc=window.dialogArguments;
    else
        doc = window.opener;
        
     if(doc) {
        if(doc && doc.weHandleDialogCallback) {
            doc.weHandleDialogCallback(v, t);            
        }
    }
    
   window.close();
}

function weHandleDialogCallback(v, t) {
    if(__WE7DOCFUNC) {
        __WE7DOCFUNC(v, t);
    }
}
/*
兼容浏览器的模态窗口
*/
function popupDialog(url,width,height){  
     
    var x = parseInt(screen.width / 2.0) - (width / 2.0);   
    var y = parseInt(screen.height / 2.0) - (height / 2.0);  
    
    var isMSIE= (navigator.appName == "Microsoft Internet Explorer");  //判断浏览器  

    if (isMSIE) {            
        var retval = window.showModalDialog(url, self, "dialogWidth:"+width+"px; dialogHeight:"+height+"px; dialogLeft:"+x+"px; dialogTop:"+y+"px; status:no; directories:yes;scrollbars:auto;Resizable=no; "  );  
        return retval;
   } 
   else {  
        var win = window.open(url, "mcePopup", "top=" + y + ",left=" + x + ",scrollbars=yes,dialog=yes,modal=yes,width=" + width + ",height=" + height + ",resizable=no" );  
        eval('try { win.resizeTo(width, height); } catch(e) { }');  
        win.focus(); 
    
   }  
}

/*获取URL参数值 */

function QueryString(name){
    var qs=name+"=";
    var str=location.search;
    if(str.length>0){
    begin=str.indexOf(qs);
    if(begin!=-1){
    begin+=qs.length;
    end=str.indexOf("&",begin);
    if(end==-1)end=str.length;
    return(str.substring(begin,end));
    }
    }
    return "";
}

function MM_jumpMenu(targ,selObj,restore){ 
  eval(targ+".location='"+selObj.options[selObj.selectedIndex].value+"'");
  if (restore) selObj.selectedIndex=0;
}

function MM_openMenu(selObj,restore){ 
 if(selObj.options[selObj.selectedIndex].value!=""){
      eval("window.open('"+selObj.options[selObj.selectedIndex].value+"');");
      if (restore) selObj.selectedIndex=0;
  }
}

function QueryString(name){
var qs=name+"=";
var str=location.search;
if(str.length>0){
begin=str.indexOf(qs);
if(begin!=-1){
begin+=qs.length;
end=str.indexOf("&",begin);
if(end==-1)end=str.length;
return(str.substring(begin,end));
}
}
return null;
}