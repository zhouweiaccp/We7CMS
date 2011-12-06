function updateKTSelect(obj,el,f)
{
    el=document.getElementsByName(el);
    k=obj.value;
    if(el&&k)
    {
        el=el[0];
        $.ajax({
            url:'/Admin/Ajax/KTSelect.aspx',
            data:{f:f,k:k},
            success:function(text,status,code){
                el.value=text;
            }
        });
    }
}