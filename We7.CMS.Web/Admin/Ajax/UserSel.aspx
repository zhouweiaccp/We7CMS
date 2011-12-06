<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSel.aspx.cs" Inherits="We7.CMS.Web.Admin.Ajax.UserSel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>

    <script type="text/javascript">    
    function updateValue()
    {
       var el= window.dialogArguments;
       el.value="";
       var l=document.getElementsByName("chk");
       for(var i=0;i<l.length;i++)
       {
          if(l[i].checked)
            el.value+=l[i].value+";";
       }
       if(el.value.length)
            el.value=el.value.substr(0,el.value.length-1);
       
       window.close();
    }
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <%
                int index = 0;
                for (int i = 0; i < Rows; i++)
               { %>
            <tr>
                <% for (int j = 0; j < Columns; j++)
                   {
                       if (index < AccountItems.Count)
                       { %>
                <td>
                    <input type="checkbox" name="chk" value="<%=AccountItems[index].LastName %>" <%=ContainVal(AccountItems[index].LastName)?"checked":"" %> /><label><%=AccountItems[index].LastName%></label>
                </td>
                <%}
                           else
                           { %>
                <td>
                    &nbsp;
                </td>
                <%} index++;
                       }
                %>
            </tr>
            <%  
                } %>
                <tr>
                    <td colspan="<%=Columns %>">
                        <input type="button" value="选择" onclick="updateValue()" />
                    </td>
                </tr>
        </table>
    </div>
    </form>
</body>
</html>
