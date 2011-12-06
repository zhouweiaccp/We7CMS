<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/Contentnomenu.Master"
    AutoEventWireup="true" CodeBehind="ProcessSign.aspx.cs" Inherits="We7.CMS.Web.Admin.ProcessSign" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<%@ Register Src="controls/ProcessHistoryList.ascx" TagName="ProcessHistoryList"
    TagPrefix="uc1" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script type="text/javascript" src="/Admin/Ajax/jquery/jquery-1.3.2.min.js"></script>
    <script type="text/javascript">
        function onSaveClick(act) {
            var saveButton = document.getElementById("<%=SaveButton.ClientID %>");
            var actionInput = document.getElementById("<%=ActionTextBox.ClientID %>");
            actionInput.value = act;
            saveButton.click();
        }
        
        function selectIdea(obj)
        {
            var v=obj.value;
            if(v)
            {
                $.ajax({
                    url:"/Admin/Ajax/KTSelect.aspx",
                    data:{f:'SignTemplate',k:v},
                    success:function(text,status){
                        $("#<%=DescriptionTextBox.ClientID %>").text(text);
                    }
                });
            }
        }
    </script>
<table border="0" cellpadding="5" width="100">
<tr>
<td style="" valign="top" >
    <div id="breadcrumb">
        <h2 class="title">
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/logo_info.gif" />
            您正在审批：
            <a href="<%=HrefAddress%>" target="_blank">
                <asp:Label ID="ArticleTitleLabel" runat="server" Text="">
                </asp:Label></a>
                <p><span class="summary">
                    <asp:Label ID="SummaryLabel" runat="server" Text="">
                    </asp:Label></p>
                </span>
        </h2>
        <WEC:MessagePanel ID="Messages" runat="server">
        </WEC:MessagePanel>
        <div runat="server" id="SignPanelDiv">
        <div>
            <p style="margin-left: 20px;">
                签字项：<br />
                <asp:TextBox ID="ApproveTitle" runat="server" Columns="40" TextMode="SingleLine"></asp:TextBox>
            </p>
        </div>
        <div>
            <p style="margin-left: 20px;">
                签字意见：<br />
                <asp:TextBox ID="DescriptionTextBox" runat="server" Columns="40" Rows="5" TextMode="MultiLine"></asp:TextBox>
                <br />
                <asp:DropDownList ID="ddlSignTemplate" runat="server" onclick="selectIdea(this)">
                </asp:DropDownList>
            </p>
        </div>
        <div>
            <p style="margin-left: 20px;">
                签字日期：
                <asp:Label ID="ApproveDate" runat="server" Text=""></asp:Label>
                <br />
                签字人名：
                <asp:Label ID="ApproveName" runat="server" Text=""></asp:Label>
            </p>
        </div>
        <div class="toolbar">
            <li class="smallButton4">
                <asp:HyperLink ID="SaveHyperLink" NavigateUrl="javascript:onSaveClick('1');" runat="server">
                通过审核
                </asp:HyperLink>
            </li>
            <li class="smallButton4">
                <asp:HyperLink ID="BackHyperLink" NavigateUrl="javascript:onSaveClick('0');" runat="server">
                退回重审</asp:HyperLink>
            </li>
            <li class="smallButton4">
                <asp:HyperLink ID="HyperLink1" NavigateUrl="javascript:onSaveClick('2');" runat="server">
                退回编辑</asp:HyperLink>
            </li>
        </div>
        </div>
        <div>
            <uc1:ProcessHistoryList ID="ProcessHistoryList1" runat="server" />
            <br />
        </div>
        <div class="hidden">
            <asp:Button ID="SaveButton" runat="server" Text="" OnClick="SaveButton_Click" />
            <asp:TextBox ID="ActionTextBox" runat="server"></asp:TextBox>
        </div>
    </div>
</td>
<td valign="top" style="border-left:solid 1px #ccc;width:60%;padding-left:15px">
<iframe id="iframeContent" frameborder="0"  width="100%" scrolling="no" ></iframe>
</td>
</tr>
</table>

   <script language="javascript" type="text/javascript">
        function initIframeSrc()
        {
            var url="ArticleView.aspx?id="+QueryString('id');
             if(document.getElementById("iframeContent"))
                    document.getElementById("iframeContent").src=url;
        }
        initIframeSrc();
        MaxTheWindow();
   </script>
</asp:Content>
