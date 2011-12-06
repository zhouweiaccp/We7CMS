<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DialogInput.aspx.cs" Inherits="We7.CMS.Web.Admin.Ajax.DialogInput" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查询输入</title>

    <script type="text/javascript">
        function getValue()
        {
            return "<%=Value %>";
        }        
        function setValue(value)
        {
           var el= window.dialogArguments;
           el.value=value;
           window.close();
        }       
        function cancel()
        {
            window.close();
        }
        
        //设置风险
        function setRisk(risk,solution)
        {
            var el= window.dialogArguments;
            var list=el.form.getElementsByTagName("input");
            for(var i=0;i<list.length;i++)
            {
                if(list[i]["related"]=="risk")
                {
                    list[i].value=risk;
                }
                 if(list[i]["related"]=="solution")
                {
                    list[i].value=solution;
                }
            }
            window.close();
            window.open
        }
    </script>

</head>
<body style="padding: 0; margin: 0;">
    <iframe id="frame" runat="server" src="RiskInfo.aspx" width="100%" height="100%">
    </iframe>
</body>
</html>
