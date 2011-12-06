<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdviceDetail.aspx.cs" Inherits="We7.CMS.Web.Admin.AdviceDetail"
    MasterPageFile="~/admin/theme/classic/content.Master" %>

<%@ Register Src="~/ModelUI/Panel/System/SimpleEditorPanel.ascx" TagName="ModelDetails"
    TagPrefix="uc2" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<%@ Register Src="../manage/controls/ProcessHistoryList.ascx" TagName="ProcessHistoryList"
    TagPrefix="uc1" %>
<%@ Register Assembly="FCKeditor.net" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">

    <script type="text/javascript">
        function TransactButtonClick() {
            var submitBtn = document.getElementById("<%=TransactButton.ClientID %>");
            submitBtn.click();
        }
        function ToOtherButtonClick() {
            var toOtherButtonBtn = document.getElementById("<%=ToOtherButton.ClientID %>");
            toOtherButtonBtn.click();
        }
        function selectUserID() {     
            var oSel = document.getElementById("<%=ddlToOtherHandleUserID.ClientID %>");
            var oSelText = oSel.options[oSel.selectedIndex].text;
            if (oSel.value != "" && confirm("您确定要转交给 " + oSelText + " 办理吗？")) {
                onAdviceListCallback(oSel.value);
            }
            else {
            }
        }
        function AdminHandClick() {
            var adminHandButton = document.getElementById("<%=AdminHandButton.ClientID %>");
            adminHandButton.click();
        }
        function onAdviceListCallback(v) {
         
            var userID = document.getElementById("<%=UserIDTextBox.ClientID %>");
            userID.value = v;

            var adviceTag = document.getElementById("<%=txtAdviceTag.ClientID %>");
            var adviceTagValue = document.getElementById("adviceTag").value;
            adviceTag.value = adviceTagValue;

            var toOtherButton = document.getElementById("<%=ToOtherButton.ClientID %>");
            toOtherButton.click();
        }
        function AuditReportButtonClick() {
            var auditReportButtonBtn = document.getElementById("<%=AuditReportButton.ClientID %>");
            auditReportButtonBtn.click();
        }
        function ReportButtonClick() {
            var reportButtonBtn = document.getElementById("<%=ReportButton.ClientID %>");
            reportButtonBtn.click();
        }
        function ReturnButtonClick() {
            var returnButtonBtn = document.getElementById("<%=ReturnButton.ClientID %>");
            returnButtonBtn.click();
        }
        function SelectedAdviceTag(id) {
            var ddl = document.getElementById(id);
            document.getElementById("adviceTag").value = ddl.options[ddl.selectedIndex].value;

        }
        function AddNewAdviceTag() {
            var newWin = window.showModalDialog("AdviceTag.aspx?newTag=Yes", window, "dialogHeight:   520px;   dialogWidth:   700px;   dialogTop:   458px;   dialogLeft:   166px;   edge:   Raised;   center:   Yes;   help:   Yes;   resizable:   Yes;   status:   Yes;");
            window.location.href = window.location.href;
            window.location.reload;

        }
    </script>

    <style>
        .leftcolumn
        {
            width: 100px;
            border: solid red 1px;
        }
        .leftcolumn strong
        {
            width: 100px;
        }
        .rightcolumn
        {
            text-align: left;
        }
        th
        {
            width: 100px;
        }
    </style>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_comment.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="详细信息">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="">
            </asp:Label>
        </span>
    </h2>
    <div id="position">
        <asp:Literal ID="PagePathLiteral" runat="server"></asp:Literal>
    </div>
    <div>
        <WEC:MessagePanel ID="Messages" runat="server">
        </WEC:MessagePanel>
        <br />
        <div>
            <asp:Panel ID="CtrlContainer" runat="server">
            </asp:Panel>
        </div>
        <table style="height: 604px; width: 100%" runat="server" id="actionTable">
            <tr>
                <td style="font-size: 16px;">
                    <b>
                        <asp:Label ID="TitleLabel" runat="server"></asp:Label></b>
                </td>
                <td>
                    <b>办理踪迹</b>
                </td>
            </tr>
            <tr runat="server" id="Departmenttr" align="left" valign="top">
                <td valign="top" align="left">
                    <table style="height: 613px;">
                        <tr>
                            <td class="style7">
                                <hr />
                                <div style="display: block; width: 500px;">
                                </div>
                                <asp:PlaceHolder runat="server" ID="ModelDetails"></asp:PlaceHolder>
                                <%--<uc2:ModelDetails ID="ModelDetails" runat="server" PanelName="adminView">
                                </uc2:ModelDetails>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <hr />
                                    反馈类别： </b>
                                <asp:DropDownList ID="ddlAdviceTag" runat="server">
                                </asp:DropDownList>
                                <%--<a id="newTag" runat="server" onclick="AddNewAdviceTag()" style="cursor: hand;">添加新反馈标签:</a>--%>
                                <asp:Label ID="lbAdviceTag" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trToOtherHandleUser" runat="server">
                            <td>
                                <b>
                                    <hr />
                                    办理人： </b>
                                <asp:DropDownList ID="ddlToOtherHandleUserID" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trHandleRemark" runat="server">
                            <td valign="top">
                                <hr />
                                <table width="100%">
                                    <tr>
                                        <td valign="middle">
                                            <b>办理备注： </b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Width="360px" Height="80px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trPriority" runat="server">
                            <td valign="top">
                                <hr />
                                <table>
                                    <tr>
                                        <td valign="middle">
                                            <b>邮件优先级： </b>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblPriority" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="普通优先级" Value="Normal"></asp:ListItem>
                                                <asp:ListItem Text="低优先级" Value="Low"></asp:ListItem>
                                                <asp:ListItem Text="高优先级" Value="High" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <hr />
                                    回复内容：</b>
                                <div id="replyDiv" runat="server">
                                    <br />
                                </div>
                                <FCKeditorV2:FCKeditor ID="ReplyContentTextBox" ToolbarSet="Basic" runat="server"
                                    Height="340px" Width="450px" BasePath="/admin/fckeditor/">
                                </FCKeditorV2:FCKeditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="style5">
                                <div id="toAdviceDiv">
                                    <b>
                                        <hr />
                                        <asp:Label ID="toAdviceLabel" runat="server"></asp:Label></b><br />
                                    <asp:TextBox ID="ToAdviceTextBox" runat="server" Columns="28" Height="100px" TextMode="MultiLine"
                                        Visible="false" Width="440px"></asp:TextBox></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="toolbar">
                                <br />
                                <li class="smallButton8">
                                    <asp:HyperLink ID="TransactHyperLink" NavigateUrl="javascript:TransactButtonClick();"
                                        runat="server">保存，并完成办理</asp:HyperLink>
                                </li>
                                <li class="smallButton8">
                                    <asp:HyperLink ID="AdminHandHyperLink" NavigateUrl="javascript:AdminHandClick();"
                                        runat="server">标记为已处理</asp:HyperLink>
                                </li>
                                <li class="smallButton8">
                                    <asp:HyperLink ID="ToOtherHyperLink" NavigateUrl="javascript:selectUserID();" runat="server">转交办理</asp:HyperLink>
                                </li>
                                <li class="smallButton8">
                                    <asp:HyperLink ID="AuditReportHyperLink" NavigateUrl="javascript:AuditReportButtonClick();"
                                        runat="server">办理完毕，上报审核</asp:HyperLink>
                                </li>
                                <li class="smallButton4">
                                    <asp:HyperLink ID="ReportHyperLink" NavigateUrl="javascript:ReportButtonClick();"
                                        runat="server">审核通过</asp:HyperLink></li>
                                <li class="smallButton4">
                                    <asp:HyperLink ID="ReturnHyperLink" NavigateUrl="javascript:ReturnButtonClick();"
                                        runat="server">退回重办</asp:HyperLink></li>
                                <li  style=" vertical-align:middle;">                                                                                                                                                                        
                                    &nbsp;&nbsp;<asp:CheckBox ID="chbSendEmail" Text="" TextAlign="left" Checked="true" runat="server" /><font runat="server" id="fontSendEmail" >将处理结果邮件通知用户</font>
                                    </li>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 100%; height: 100%;" valign="top">
                    <br />
                    <div runat="server" style="font-size: medium; overflow-y: scroll; padding: 5px; margin-left: 0px;
                        margin-right: 10px; margin-top: 10px; border: solid 1px #eee">
                        <uc1:ProcessHistoryList ID="ProcessHistoryList1" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <div style="display: none">
            <asp:Button ID="TransactButton" runat="server" Text="Transact" OnClick="TransactButton_Click" />
            <asp:Button ID="ToOtherButton" runat="server" Text="ToOther" OnClick="ToOtherButton_Click" />
            <asp:Button ID="AuditReportButton" runat="server" Text="AuditReport" OnClick="AuditReportButton_Click" />
            <asp:Button ID="ReportButton" runat="server" Text="Report" OnClick="ReportButton_Click" />
            <asp:Button ID="ReturnButton" runat="server" Text="Return" OnClick="ReturnButton_Click" />
            <asp:Button ID="AdminHandButton" runat="server" Text="Return" OnClick="AdminHandButton_Click" />
            <asp:TextBox ID="UserIDTextBox" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtAdviceTag" runat="server"></asp:TextBox>
            <input type="text" id="adviceTag" value="noTag" />
        </div>
    </div>
</asp:Content>
