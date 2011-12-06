$(document).ready(function() {
    postClickData(getUrl());
});

function getUrl() {
    return window.location.href;
}

function postClickData(url) {
    $.ajax({
        type: 'POST',
        url: '/Admin/Ajax/ClickRecord.aspx',
        data: 'action=add&url=' + escape(url),
        cache: false,
        complete: function(msg, textStatus) {           
        }
    });
}