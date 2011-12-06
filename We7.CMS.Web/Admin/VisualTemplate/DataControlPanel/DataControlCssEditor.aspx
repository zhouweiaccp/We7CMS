<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataControlCssEditor.aspx.cs" Inherits="We7.CMS.Web.Admin.VisualTemplate.DataControlCssEditor" %>

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
        body
        {
            padding: 0;
            margin: 0;
            font-family: 宋体 Arial;
            background-color: #F7F7F7;
        }
    </style>
    <script type="text/javascript">
        function store()
        {
            document.getElementById("hdKey").value="";
            document.getElementById("hdValue").value="";
            var r=window.showModalDialog('FieldControl/storeCSSCopy.htm','<%=Request["style"] %>','scrollbars=no;resizable=no;help=no;status=no;center=1; dialogHeight=130px;dialogwidth=270px;');
            if(r)
            {
                document.getElementById("hdValue").value=r.v;
                document.getElementById("hdKey").value=r.t;
                return true;
            }
            return false;
        }    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td>
                    <asp:TextBox ID="CssTextBox" runat="server" TextMode="MultiLine" Height="415px" Width="100%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="SaveButton" runat="server" UseSubmitBehavior="false" Text="　　保存　　"
                                    OnClick="SaveButton_Click" />
                            </td>
                            <td>
                                <asp:Button ID="bttnStore" OnClick="bttnStore_Click" OnClientClick="if(!store())return;" runat="server" UseSubmitBehavior="false" Text="　　归库　　" />
                                <asp:HiddenField ID="hdValue" runat="server" />
                                <asp:HiddenField ID="hdKey" runat="server" />
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
