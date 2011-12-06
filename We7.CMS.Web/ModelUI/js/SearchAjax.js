//联动控件加载二级菜单
function DoubleCascadeField2(value, myId, field2TextMapping, field2ValueMapping, field1ValueMapping, table, emptyText, type, path, nodesName, attributesName) {
    var field2 = document.getElementById(myId);		
    $.ajax({
        type: 'POST',
        url: '/ModelUI/Controls/page/QueryAjax.aspx',
        data: 'name=DoubleCascade.ascx&field1Value=' + escape(value) + "&field2TextMapping=" + escape(field2TextMapping) + "&field2ValueMapping=" + escape(field2ValueMapping) + "&field1ValueMapping=" + escape(field1ValueMapping) + "&table=" + escape(table) + "&type=" + escape(type) + "&path=" + escape(path) + "&nodesName=" + escape(nodesName) + "&attributesName=" + escape(attributesName),
        cache: false,
        complete: function(msg, textStatus) {
            var all = msg.responseText.split(",");
            field2.options.length = 0;
            if (emptyText != "") {
                field2.options.add(new Option('请选择', ''));
            }
            for (i = 0; i < all.length; i++) {
                if (all[i].length > 1) {
                    field2.options.add(new Option(all[i].split("&")[0], all[i].split("&")[1]));
                }
            }
        }
    });
}

//联动控件加载二级菜单同时也加载三级菜单
function ThreeCascadeField2(field1Id, field1Value, field2Id, field2TextMapping, field2ValueMapping, field1ValueMapping, table, myId3, field3TextMapping, field3ValueMapping, emptyText, type, path, nodesName, attributesName) {
    var field2 = document.getElementById(field2Id);
    $.ajax({
        type: 'POST',
        url: '/ModelUI/Controls/page/QueryAjax.aspx',
        data: 'name=DoubleCascade.ascx&field1Value=' + escape(field1Value) + "&field2TextMapping=" + escape(field2TextMapping) + "&field2ValueMapping=" + escape(field2ValueMapping) + "&field1ValueMapping=" + escape(field1ValueMapping) + "&table=" + escape(table) + "&type=" + escape(type) + "&path=" + escape(path) + "&nodesName=" + escape(nodesName) + "&attributesName=" + escape(attributesName),
        cache: false,
        complete: function(msg, textStatus) {           
            var all = msg.responseText.split(",");
            field2.options.length = 0;
            if (emptyText != "") {
                field2.options.add(new Option('请选择', ''));
            }
            for (i = 0; i < all.length; i++) {
                if (all[i].length > 1) {
                    field2.options.add(new Option(all[i].split("&")[0], all[i].split("&")[1]));
                }
            }
            if (all.length > 0 && all[0].length > 1) {
                ThreeCascadeField3(field1Id, field2.options[0].value, myId3, field3TextMapping, field3ValueMapping, field2ValueMapping,field1ValueMapping, table, emptyText, type, path);
            }
            else {
                var field3 = document.getElementById(myId3);
                field3.options.length = 0;
            }
        }
    });
}

//联动控件加载三级菜单
function ThreeCascadeField3(field1Id, field2value, field3Id, field3TextMapping, field3ValueMapping, field2ValueMapping, field1ValueMapping, table, emptyText, type, path, nodesName, attributesName) {
    var field3 = document.getElementById(field3Id);
    var field1Value = $("#" + field1Id + "").val();
    $.ajax({
        type: 'POST',
        url: '/ModelUI/Controls/page/QueryAjax.aspx',
        data: 'name=ThreeCascade.ascx&field1Value=' + escape(field1Value) + '&field2Value=' + escape(field2value) + "&field3TextMapping=" + escape(field3TextMapping) + "&field3ValueMapping=" + escape(field3ValueMapping) + "&field2ValueMapping=" + escape(field2ValueMapping) + "&field1ValueMapping=" + field1ValueMapping + "&table=" + escape(table) + "&type=" + escape(type) + "&path=" + escape(path) + "&nodesName=" + escape(nodesName) + "&attributesName=" + escape(attributesName),
        cache: false,
        complete: function(msg, textStatus) {           
            var all = msg.responseText.split(",");            
            field3.options.length = 0;
            if (emptyText != "") {
                field3.options.add(new Option('请选择', ''));
            }
            for (i = 0; i < all.length; i++) {
                if (all[i].length > 1) {
                    field3.options.add(new Option(all[i].split("&")[0], all[i].split("&")[1]));
                }
            }
        }
    });
}
