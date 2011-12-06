<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModelEditor.aspx.cs" MasterPageFile="~/Admin/theme/classic/content.Master"
    Inherits="We7.CMS.Web.Admin.Addins.ModelEditor" %>

<%@ Register Src="~/ModelUI/Panel/system/EditorPanel.ascx" TagName="EditorPanel"
    TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <link rel="stylesheet" type="text/css" href="../theme/classic/css/article.css" media="screen" />

    <script type="text/javascript" src="/Admin/Ajax/My97DatePicker/WdatePicker.js"></script>

    <script type="text/javascript" language="javascript">
        function iframeAutoFit() {
            try {
                if (window != parent) {
                    var a = parent.document.getElementsByTagName("IFRAME");
                    for (var i = 0; i < a.length; i++) //author:meizz
                    {
                        if (a[i].contentWindow == window) {
                            var h1 = 0, h2 = 0;
                            a[i].parentNode.style.height = a[i].offsetHeight + "px";
                            a[i].style.height = "10px";
                            if (document.documentElement && document.documentElement.scrollHeight) {
                                h1 = document.documentElement.scrollHeight;
                            }
                            if (document.body) h2 = document.body.scrollHeight;
                            var h = Math.max(h1, h2);
                            if (document.all) { h += 4; }
                            if (window.opera) { h += 1; }
                            a[i].style.height = a[i].parentNode.style.height = h + h * 0.3 + "px";
                        }
                    }

                    if (parent.closeBar) parent.closeBar();
                }
            }
            catch (ex) { }
        }
        if (window.attachEvent) {
            window.attachEvent("onload", iframeAutoFit);
        }
        else if (window.addEventListener) {
            window.addEventListener('load', iframeAutoFit, false);
        }
    </script>

    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="../Images/icons_article.gif" />
        <asp:Label ID="NameLabel" runat="server">
        </asp:Label>
        <span class="summary"><span id="navChannelSpan"></span>
            <asp:Label ID="SummaryLabel" runat="server" Text=" ">
            </asp:Label>
        </span>
    </h2>
    <div id="position">
        <asp:Literal ID="PagePathLiteral" runat="server"></asp:Literal>
    </div>
    <div id="mycontent">
        <uc1:EditorPanel ID="ucEditor" runat="server" PanelName="edit" />
    </div>
</asp:Content>
