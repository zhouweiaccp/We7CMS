<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataControlModelEditor.aspx.cs"
    Inherits="We7.CMS.Web.Admin.DataControlUI.DataControlModelEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>样式编辑</title>
    <base target="_self" />
    <style type="text/css">
        input, selectd
        {
            margin: 0;
        }
        body, td
        {
            font-size: 12px;
        }
        .style1
        {
            width: 67px;
        }
        body
        {
            padding: 0;
            margin: 0;
            font-family: 宋体 Arial;
            background-color: #F7F7F7;
        }
    </style>

    <script type="text/javascript" src="/Admin/Ajax/jquery/jquery-1.3.2.min.js"></script>

    <script type="text/javascript">
        function store()
        {
            document.getElementById("hdKey").value="";
            document.getElementById("hdValue").value="";
            var r=window.showModalDialog('FieldControl/storeCtrCopy.htm','<%=Request["ctr"] %>','scrollbars=no;resizable=no;help=no;status=no;center=1; dialogHeight=170px;dialogwidth=430px;');
            if(r)
            {
                if(r.t)
                {
                    document.getElementById("hdValue").value=r.v.name+"|"+r.v.key+"|"+r.v.desc;
                }
                document.getElementById("hdKey").value=r.t;
                return true;
            }
            return false;
        }    
    </script>

    <script type="text/javascript">
        $(function(){
            var data=<%=Data %>
            
            for(var i=0;i<data.length;i++)
            {
                var item=data[i];
                $("#menu").append($("<div></div>").append(item.label).data("exp",item.exp).click(function(){
                    AppendText($(this).data("exp"));
                }).css("margin","2px").css("border","solid 1px #333399").css("padding","3px 0;").css("text-align","center").css("cursor","pointer").hover(function(){
                    $(this).css("background","#FFFFFF");
                },function(){
                     $(this).css("background","");
                }));
            }
        });
        
        function AppendText(str)
        {
            var ubb=$("#<%=CtrCodeTextBox.ClientID %>")[0];
            ubb.focus();
            if(document.selection)
            {
            document.selection.createRange().text=str;
            }
            else
            {
                 var ubbLength=ubb.value.length;
                 var cursorPos=(ubb.value.substr(0,ubb.selectionStart)+str).length;
                 ubb.value=ubb.value.substr(0,ubb.selectionStart)+str+ubb.value.substring(ubb.selectionStart,ubb.value.length);
                 ubb.setSelectionRange(cursorPos,cursorPos);
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td valign ="top">
                    <asp:TextBox ID="CtrCodeTextBox" runat="server" TextMode="MultiLine" Height="415px"
                        Width="100%"></asp:TextBox>
                </td>
                <td valign="top" width="150">
                    <div style="border:solid 1px #333399; text-align:center; padding:3px 0; background:#eeeeee; color:black; font-weight:bold; margin:2px;">可用字段(点击插入)</div>
                    <div id="menu" style="height:390px;overflow-y:auto;">
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="SaveButton" runat="server" Text="　　保存　　" OnClick="SaveButton_Click" />
                            </td>
                            <td>
                                <asp:Button ID="StoreButton" OnClientClick="return store();" runat="server" Text="　　归库　　"
                                    OnClick="StoreButton_Click" />
                                <asp:HiddenField ID="hdKey" runat="server" />
                                <asp:HiddenField ID="hdValue" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="msgLabel" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
