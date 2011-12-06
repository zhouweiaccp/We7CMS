    var Sys = {};
    var ua = navigator.userAgent.toLowerCase();
    if (window.ActiveXObject)
        Sys.ie = ua.match(/msie ([\d.]+)/)[1];
    if(Sys.ie == "6.0" ||Sys.ie == "7.0" )
    {
        alert("您即将访问We7模板编辑器，但您的浏览器版本太旧了，如想体验我们全新的拖拽技术，建议您下载以下浏览器的任一款：360极速双核版、搜狗双核版、傲游双核版、QQ浏览器双核版、火狐、chrome及IE8/IE9等，谢谢您对互联网新技术的支持！");
        
        history.go(-1);
    }   