
function addTag(tagName) {
    tagName = $.trim(tagName);
    if (tagName.length > 0) {
        we7.loading("操作中..");
        $.ajax({
            type: "get",
            url: "/admin/ajax/TagAjax.aspx?op=add&id=" + $("#IDHidden").val() + "&type=" + $("#TagTypeHidden").val() + "&name=" + encodeURIComponent(tagName),
            datatype: "json ",
            success: function (json) {
                json = eval('(' + json + ')');
                if (json.success) {
                    if (tagName.length > 10)
                        tagName = tagName.substring(0, 10) + "..";
                    $('#tagList').append(FormatTag(tagName));
                    $("#tagNameInput").val('');
                    $("#tagNameInput").focus();
                    we7.status("操作成功!");
                }
                else {
                    we7.status("添加失败 " + tagName + ",错误消息：" + json.msg , { autoHide: true, hideTimeout: 6000 });

                }
            },
            failure: function (msg, resp, status) {
                alert(msg);
                alert(resp);
                alert(status);
            }
        });
    } else {
        $("#tagNameInput").focus();
    }
}

function FormatTag(tagName) {
    var a = $("<A class=a_del title=\"" + tagName + "\"   href=\"javascript:void(0);\"  >[x]</A>");
    a.bind("click", function (event) {
        removeTag(tagName, event);
    });
    return $("<LI><IMG class=Icon height=16 src=\"/admin/images/icon_globe.gif\" width=16>" + tagName + "  </LI>").append(a);
}

function removeTag(tagName, event) {
    tagName = $.trim(tagName);
    if (tagName.length > 0) {
        we7.loading("操作中..");
        $.ajax({
            type: "get",
            url: "/admin/ajax/TagAjax.aspx",
            data: "op=del&id=" + $("#IDHidden").val() + "&type=" + $("#TagTypeHidden").val() + "&name=" + encodeURIComponent(tagName),
            datatype: "json ",
            success: function (json) {
                json = eval('(' + json + ')');
                if (json.success) {
                    $(event.target).parent().remove();
                    we7.status("操作成功!");
                }
                else
                    we7.status("删除失败 " + tagName + ",错误消息：" + json.msg , { autoHide: true, hideTimeout: 6000 });
            }

        });
    }
}

function GetTags() {

    var loading = $("<br/> <img src='/admin/images/blue-loading.gif' />");
    $(".usefulTags").append(loading);
    $.ajax({
        type: "get",
        url: "/admin/ajax/TagAjax.aspx",
        data: "op=list&pi=" + $("#pi").val(),
        datatype: "json ",
        success: function (json) {
            loading.remove();
            if (we7.isStr(json) && json) {
                json = eval('(' + json + ')');
                if (json.success) {
                    if (we7.isObj(json.Data) && json.Data) {
                        $(".usefulTags").append("<br/><br/>");
                        $.each(json.Data, function (i) {
                            $(".usefulTags").append(we7.formatStr("<a href=\"javascript:addTag('{0}')\" title=\"为文章添加标签 {0}？\"  >{0}</a> ", json.Data[i].Identifier));
                        });
                        $("#pi").val((parseInt($("#pi").val()) + 1));
                    }
                }
                else {
                    we7.status("加载失败,错误消息：" + json.msg, { autoHide: true, hideTimeout: 6000 });
                }
            }
            else
                we7.status("请求无响应，或者异常，请查看错误日志");
        }
    });
}

function KeyPressAdd(obj) {
    if (event.keyCode == 13 || event.keyCode == 0) {
        addTag(obj.value);
        return;
    }
}    