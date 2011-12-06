<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Advice_File.ascx.cs"
    Inherits="We7.CMS.Web.Admin.controls.Advice_File" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>

<script type="text/javascript">
    function onCancelHyperLinkClick() {
        window.close();
    }

    function onSaveButtonClick() {
        document.getElementById("<%=SaveButton.ClientID %>").click();
    }
    
    function openEditLayoutWindow()
    {
        var selType = document.getElementById("<%=ddlAdviceType.ClientID %>");
        if (selType && selType.value != "") {
            var url = "/admin/ContentModel/EditLayout.aspx?adviceTypeID=<%= AdviceTypeID%>&modelname=" + selType.value;
            window.open(url);
        }
        else {
            alert("请先选择反馈模型表单！如果没有下拉列表，请先新建一个反馈模型表单！");
        }
    }
 
</script>

<WEC:MessagePanel ID="Messages" runat="server">
</WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»编辑反馈模型的表单界面<br>
            <img src="/admin/images/bulb.gif" align="absmiddle" /><label class="block_info">反馈模型表单自定义</label>
            <dd>
                <table class='tree'>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="ConfigNameTextBox" runat="server">
                            </asp:Label>
                            反馈模型表单——采用内容模型进行自定义，选择一个已定义好的内容模型。
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlAdviceType" runat="server"></asp:DropDownList>
                        </td>
                        <td>
                        <a href="javascript:openEditLayoutWindow();" >编辑这个模型的表单布局</a> |  <a href="/admin/ContentModel/EditModel.aspx?action=add&type=ADVICE" target="_blank">新建反馈模型表单</a> 
                        </td>
                    </tr>
                </table>
                <div class="toolbar">
                    <a href="javascript:onSaveButtonClick();"  target="_self">保存</a>
                    <br />
                </div>
                <div style="display: none">
                    <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
                    <asp:TextBox ID="OldFileTextBox" runat="server"></asp:TextBox>
                </div>
                <div style="display: none">
                    <br />
                    <asp:Image ID="WarningImage" runat="server" ImageUrl="~/admin/Images/icon_warning.gif" />
                    <asp:Label ID="MessageLabel" runat="server">
                    </asp:Label>
                </div>
    </dl>
</div>
