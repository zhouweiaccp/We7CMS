//联动事件
function getSubcate(select) {
    var parentID = select.options[select.selectedIndex].value;
    var name = select.options[select.selectedIndex].text;
    var field2Name = select.id.replace('Field1DropDownList', 'Field2DropDownList');
    if (parentID) {
        DoubleCascadeField2(parentID, field2Name, 'Name', 'KeyWord', 'ParentID', 'Category', '请选择', 'db', '', '', ''
                , function() {
                    subcateChange(field2Name);
                });
    }
}
//二级改变方法
function subcateChange(selectId) {
    var keyword = "";
    var select = document.getElementById(selectId);
    var field2HiddenName = select.id.replace('Field2DropDownList', 'Field2Hidden');
    if (select.selectedIndex > -1) {
        keyword = select.options[select.selectedIndex].text;
    }
    $("#" + field2HiddenName).val(keyword);
}

// 联动控件加载二级菜单    v1.0  2011-1-12   moye
// 参数说明：
//      value   :   查询的值
//      myId    :   联动select的id
//      field2TextMapping   :   绑定select的text值
//      field2ValueMapping  :   绑定select的value值
//      field1ValueMapping  :   查询的字段名
//      table               :   表名
//      emptyText           :   为空显示值
//      type                :   xml/db
//      path                :   路径
//      nodesName           :   节点名
//      attributesName      :   属性名
//      callback            :   回调方法
function DoubleCascadeField2(value, myId, field2TextMapping, field2ValueMapping, field1ValueMapping, table,
                                                                emptyText, type, path, nodesName, attributesName, callback) {
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
            if (typeof callback != 'undefined' && callback instanceof Function) {
                callback();
            }
        }
    });
}