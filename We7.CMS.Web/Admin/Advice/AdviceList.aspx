<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdviceList.aspx.cs"
 MasterPageFile="~/admin/theme/classic/content.Master" Inherits="We7.CMS.Web.Admin.AdviceList" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
    <%@ Register Src="../Advice/controls/AdviceListControl.ascx" TagName="AdviceListControl"
    TagPrefix="uc1" %>
<asp:content id="We7Content" contentplaceholderid="MyContentPlaceHolder" runat="server">

    <script type="text/javascript">
   </script>
        <uc1:AdviceListControl ID="AdviceListControl" runat="server" />
</asp:content>
