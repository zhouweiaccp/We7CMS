function setCookie(name,value,expiry,path,domain,secure) {
	var nameString = name + "=" + value;
	var expiryString = (expiry == null) ? "" : " ;expires = " + expiry.toGMTString();
	var pathString = (path == null) ? "" : " ;path = " + path;
	var domainString = (path == null) ? "" : " ;domain = " + domain;
	var secureString = (secure) ? ";secure" : "";
	document.cookie = nameString + expiryString + pathString + domainString + secureString;
}

function clearCookie() {  
	var arr = ["sendtype", "year", "mon", "day", "hour", "min", "member", "group", "userstr", "content", "key", "desc"];
	var i;
	var ts;
	for(i = 0; i < arr.length; i++) {
		ts = arr[i];
		deleteCookie(ts);
	}
}

function getCookie(name) {
	var CookieFound = false;
	var start = 0;
	var end = 0;
	var CookieString = document.cookie;
	var i = 0;

	while (i <= CookieString.length) {
		start = i ;
		end = start + name.length;
		if (CookieString.substring(start, end) == name) {
			CookieFound = true;
			break;
		}
		i++;
	}

	if (CookieFound){
		start = end + 1;
		end = CookieString.indexOf(";",start);
		if (end < start)
			end = CookieString.length;
		return unescape(CookieString.substring(start, end));
	}
	return "";
}

function deleteCookie(name){
	var expires = new Date();
	expires.setTime (expires.getTime() - 1);

	setCookie(name, "Delete Cookie", expires, null, null, false);
}