
function addBind(handler,mode,model,name,file)
{
     $.ajax( 
    { 
        type:   "get",
        url: "/admin/Template/action/addBind.ashx",
        data:"handler=" + handler + "&mode=" + mode+"&model=" + model+ "&filename=" + file,  
        success:   function(msg)
        {   
                if(msg!='0')
                     alert("无法指定模板："+msg);
                else
                {
                    var id= MathRand();
                    id="MYLI"+id;
                    var liTag="<LI id='"+id +"'><IMG class=Icon height=16 src='/admin/images/icon_globe.gif' width=16>"+name+"<A class=Del title='删除指定 "+name+"?'  href=\"javascript:removeBind('"+handler+"','"+mode+"','"+model+"','"+file+"','"+id+"');\" >[删除]</A> </LI>";
                    $('#bindList').append(liTag);
                 }
        },
        failure:function(msg,resp,status)
        {
            alert(msg);
            alert(resp);
            alert(status);
        }
    } 
    ); 
}
        
function removeBind(handler,mode,model,file,theLI)
{
     $.ajax( 
    { 
        type:   "get", 
        url:   "/admin/Template/action/delBind.ashx?handler=" + handler + "&mode=" + mode+"&model=" + model+ "&filename=" + file,  
        datatype: "html ", 
        success:   function(msg)
        {   
                if(msg=='0')
                     $('#'+theLI).remove();
                else
                     alert("无法删除指定："+msg);
        },
        failure:function(msg,resp,status)
        {
            alert(msg);
            alert(resp);
            alert(status);
        }  
    } 
    ); 
}

function MathRand() 
{ 
var Num=""; 
for(var i=0;i<5;i++) 
{ 
    Num+=Math.floor(Math.random()*10); 
} 
return Num;
} 