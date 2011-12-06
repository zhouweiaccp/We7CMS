<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoreSelect.aspx.cs" Inherits="We7.Model.UI.Controls.system.page.morselect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>选择更多</title>
     <script language="javascript">
     

    function selected_info()
    { 
     var str=GetCheckBoxListValue('ChbList');
     var strc=str.substring(0,str.length-1)
     var sData = dialogArguments;
     var txtid=sData.document.getElementById("<%=Clientid%>")
        txtid.value= strc;
     window.close();
     }
   function GetCheckBoxListValue(objID)
 {
    var v = new Array();
    var CheckBoxList = document.getElementById(objID);
        if(CheckBoxList.tagName=="TABLE")
       {
        for(i=0;i<CheckBoxList.rows.length;i++)    
            for(j=0;j<CheckBoxList.rows[i].cells.length;j++)
        if(CheckBoxList.rows[i].cells[j].childNodes[0])
                    if(CheckBoxList.rows[i].cells[j].childNodes[0].checked==true)
                         v.push(CheckBoxList.rows[i].cells[j].childNodes[1].innerText);
       }
        if(CheckBoxList.tagName=="label")
          {
          for(i=0;i<CheckBoxList.childNodes.length;i++)
             if(CheckBoxList.childNodes[i].tagName == "INPUT")
                 if(CheckBoxList.childNodes[i].checked==true)
                 {
                     i++;
                     v.push(CheckBoxList.childNodes[i].innerText);
                 }            
           }
    
    return  v.toString();
}
 </script>

</head>
<body  >
    <form id="form1" runat="server">
    <div align="center">
        <table style="width:100%;">
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
        <asp:CheckBoxList ID="ChbList" runat="server" RepeatColumns="4"  >
        </asp:CheckBoxList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
<input runat="server" type="button" onclick="selected_info()" value="确定" />
    </div>
    </form>
</body>
</html>
