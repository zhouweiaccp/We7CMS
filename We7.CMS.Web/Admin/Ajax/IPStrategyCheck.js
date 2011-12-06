function checkKey(name, el) {
    new Ajax.Request("/Admin/Ajax/IPStrategyCheck.aspx", {
        method: "get",
        parameters: "key=" + encodeURI(name) + "&type=1&r=" + new Date().getTime(),
        onSuccess: function(result) {
            el.style.display = "none";
            if (result.responseText == "true") {
                el.style.display = "";
            }
        },
        onFailure: function(result) {
            alert("检测出错" + result.message);
        }
    });
}

function checkName(name, el) {
    new Ajax.Request("/Admin/Ajax/IPStrategyCheck.aspx", {
    method: "get",
    parameters: "key=" + encodeURI(name) + "&type=0&r=" +new Date().getTime(),
    onSuccess: function(result) {
            el.style.display = "none";
            if (result.responseText == "true") {
                el.style.display = "";
            }
        },
        onFailure: function(result) {
            alert("检测出错" + result.message);
        }
    });
}

function checkKey2(name, el) {
    var flag = false;
    new Ajax.Request("/Admin/Ajax/IPStrategyCheck.aspx", {
        method: "get",
        asynchronous: false,
        parameters: "key=" + encodeURI(name) + "&type=1&r=" + new Date().getTime(),
        onSuccess: function(result) {
        
            el.style.display = "none";
            if (result.responseText == "true") {
                el.style.display = "";
                flag = true;
            }
        },
        onFailure: function(result) {
            alert("检测出错" + result.message);
        }
    });
    return flag;
}

function checkName2(name, el) {
    var flag = false;
    new Ajax.Request("/Admin/Ajax/IPStrategyCheck.aspx", {
        method: "get",
        asynchronous: false,
        parameters: "key=" + encodeURI(name) + "&type=0&r=" + new Date().getTime(),
        onSuccess: function(result) {
            el.style.display = "none";
            if (result.responseText == "true") {
                el.style.display = "";
                flag = true;
            }
        },
        onFailure: function(result) {
            alert("检测出错" + result.message);
        }
    });
    return flag;
}