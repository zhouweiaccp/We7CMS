
//通用模糊搜索
window.onload=LoadKeyvalue;
function LoadKeyvalue()
{
    var key=decodeURIComponent(QueryString("keyword"));
    if(key && key!="null")
    {
        var obj=document.getElementById("KeyWord");
        if(obj) obj.value=key;
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

function QueryStringFromUrl(name,url){
var qs=name+"=";
var str=url;
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

var IsPressed=false;
function doSearch(url,contentID) {

    var key = document.getElementById("KeyWord");

    if (key) {
        if (url.indexOf("?") > 0)
            url += "&keyword=" + encodeURIComponent(key.value);
        else
            url += "?keyword=" + encodeURIComponent(key.value);
    }
    
    if (contentID) {
        var countObj = document.getElementById(contentID + "_Counter");

        var count = countObj.value;
        var querystr = "";
        for (var i = 0; i < count; i++) {
            if (querystr != "") querystr = querystr + "|";
            var id = contentID + "_MyCtrl" + i;
            var value = "";
            var obj = document.getElementById(id);
            var type = obj.getAttribute("Type");

            switch (type) {
                case "DropDownList":
                    var index = obj.selectedIndex;
                    value = obj.options[index].value;
                    break;
                case "text":
                    value = obj.value;
                    break;
            }
            querystr = querystr + id + ";" + value;

        }
        
        if (url.indexOf("?") > 0)
            url += "&querystr=" + encodeURIComponent(querystr);
        else
            url += "?querystr=" + encodeURIComponent(querystr);
    }
    
    IsPressed=true;
    window.location=url;
   
}

NS4 = (document.layers) ? true : false;
function KeyPressSearch(url,event)
{
     var code = 0;
    if (NS4)
        code = event.which;
    else
        code = event.keyCode;
     //alert(code);
	if(code==13)
	{
		doSearch(url);
	}
}

  