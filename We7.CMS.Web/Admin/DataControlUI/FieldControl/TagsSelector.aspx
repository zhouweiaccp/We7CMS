<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TagsSelector.aspx.cs" Inherits="We7.CMS.Web.Admin.DataControlUI.FieldControl.TagsSelector" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>标签选择</title>
    <base target="_self" />
    <style>
        body
        {
            padding: 0;
            margin: 0;
            font-size: 12px;
            background-color: #F7F7F7;
        }
        th, td
        {
            font-size: 12px;
            font-family: 宋体 Arial;
        }
        select, input
        {
            font-size: 12px;
        }
        html,body
        {
        	overflow:hidden;
        }
    </style>

    <script type="text/javascript" src="/Admin/ajax/jquery/jquery1.2.6.pack.js"></script>

</head>
<body style="overflow:hidden">
    <form id="form1" runat="server">
    <div>
        <div style="height:130px; width:294px;overflow-x:hidden; overflow-y:auto">
        <table width="100%" id="ct">
        </table>
        </div>
        <table width="100%" id="fb" style="position: absolute; bottom: 5px;">
            <tr>
                <td align="center">
                    <button style="width: 100px;" id="ok">
                        确定</button>
                    <button id="cancel" style="width: 100px;">
                        取消</button>
                </td>
            </tr>
        </table>

        <script type="text/javascript">
        var data=<%=Tags %>;
        $(function(){
            var o=data;
            var r;
            var t=$("#ct")[0];
            for(var i=0;i<o.length;i++)
            {
                var a=o[i];
                if(i%3==0)
                {
                    r=t.insertRow(-1);
                }
                var c=document.createElement("TD");
                r.appendChild(c);
                c.innerHTML="<input id='"+a+"' name='fields' type='checkbox' value='"+a+"' />";
                c.width="20";
                c=document.createElement("TD");
                r.appendChild(c);
                c.innerHTML="<label for='"+a+"'>"+a+"</label>";
                c.width="33%";
            }
            if(i%3!=0)
            {
                i=3-i%3;
                for(var j=0;j<i;j++)
                {
                    c=document.createElement("TD");
                    r.appendChild(c);
                    c.innerHTML="&nbsp;"
                    c=document.createElement("TD");
                    r.appendChild(c);
                    c.innerHTML="&nbsp;"
                }
            }
            var si=window.dialogArguments;
            if(si&&si.length>0)
            {
                var s=si.split(",");
                for(var i=0;i<s.length;i++)
                {
                    var item=$("INPUT:checkbox[value='"+s[i]+"']");
                    if(item&&item.length>0)
                        item[0].checked=true;
                }
            }
        });
       
        $("#ok").bind("click",function()
        {
            var fields="";
            $("INPUT:checkbox[checked]").each(function(index,item){
                fields+=$(item).val()+",";
            });
            if(fields.length>0)
                fields=fields.substr(0,fields.length-1);
            window.returnValue=fields;
            window.close();
        });
        $("#cancel").bind("click",function(){
            window.close();
        });
        </script>

    </div>
    </form>
</body>
</html>
