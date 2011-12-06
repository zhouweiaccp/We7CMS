function checkadvice(el,id){
    var pwd= window.prompt("请输入查询密码","");
    var url=$(el).attr("href");
    $.ajax({
        url:"/Admin/Ajax/CheckAdvicePwd.ashx",
        data:{id:id,pwd:pwd},
        method:"post",
        asyn:false,
        success:function(text,state,req){
            if(text=="true"){
                window.location=url;
            }
            else{
                alert("密码错误！");
            }
        },
        failure:function(text,state,req){
            alert("应用程序错误");
        },
        error:function(text,state,req){
            alert("应用程序错误，请与管理员联系");
        }
    });
    return false;
}