<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/admin/theme/classic/content.Master"
    CodeBehind="CertificationCreate.aspx.cs" Inherits="WebEngine2007.CD.Web.Admin.ADInterface.CertificationCreate" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">

    <script type="text/javascript" language="javascript">
        function ShowMessages(info) {
            var WebMessages = document.getElementById("WebMessages");
            WebMessages.style.display = "";

            var WebContent = document.getElementById("WebContent");
            WebContent.innerText = info;
        }

        function CloseMessages() {
            var WebMessages = document.getElementById("WebMessages");
            WebMessages.style.display = "none";
        }

        function RedirectToAD(returnUrl) {
            var IframeID = document.getElementById("ifRightDetail");
            IframeID.src = returnUrl;
        }

//        //申明XMLHttpRequest对象
//        var xmlDataHttp;
//        try {
//            xmlDataHttp = new ActiveXObject("Msxml2.XMLHTTP");
//        } catch (e) {
//            try {
//                xmlDataHttp = new ActiveXObject("Microsoft.XMLHTTP");
//            } catch (e2) {
//                xmlDataHttp = false;
//            }
//        }
//        if (!xmlDataHttp && typeof XMLHttpRequest != 'undefined') {
//            xmlDataHttp = new XMLHttpRequest();
//        }

//        //准备数据，以便发送
//        function DataReadyAndSend(action) {
//            var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
//            xmlDoc.loadXML('<?xml version="1.0"?><root/>');

//            var content = xmlDoc.createElement("content");

//            var SiteIDText = document.getElementById("<%=SiteIDText.ClientID %>");
//            var SiteNameText = document.getElementById("<%=SiteNameText.ClientID %>");
//            var SiteUrlText = document.getElementById("<%=SiteUrlTextBox.ClientID %>");
//            var item = xmlDoc.createElement("item");
//            item.setAttribute("id", "SiteTextBox");
//            item.setAttribute("value", SiteIDText.value);
//            item.setAttribute("text", SiteNameText.value);
//            item.setAttribute("urltext", SiteUrlText.value);
//            content.appendChild(item);

//            var AccountIDText = document.getElementById("<%=AccountIDText.ClientID %>");
//            var AccountNameText = document.getElementById("<%=AccountNameText.ClientID %>");
//            item = xmlDoc.createElement("item");
//            item.setAttribute("id", "AccountTextBox");
//            item.setAttribute("value", AccountIDText.value);
//            item.setAttribute("text", AccountNameText.value);
//            content.appendChild(item);

//            var ADObjectText = document.getElementById("<%=ADObjectText.ClientID %>");
//            item = xmlDoc.createElement("item");
//            item.setAttribute("id", "ADObjectText");
//            item.setAttribute("value", "");
//            item.setAttribute("text", ADObjectText.value);
//            content.appendChild(item);

//            xmlDoc.documentElement.appendChild(content);
//            var ADUrlText = document.getElementById("<%=ADUrlText.ClientID %>");
//            var postUrl = ADUrlText.value + "CertificationResponse.aspx";
//            xmlDataHttp.open("POST", postUrl, true);
//            xmlDataHttp.onreadystatechange = postReady;
//            xmlDataHttp.send(xmlDoc);
//        }

//        function postReady() {
//            if (xmlDataHttp.readyState == 4) {
//                if (xmlDataHttp.status == 200) {
//                    try {
//                        var response = xmlDataHttp.responseText;
//                        var start = response.indexOf("ADFIRST ");
//                        var end = response.indexOf(" ADEND", start);

//                        var returnUrl = response.substring(start + 8, end);

//                        var IframeID = document.getElementById("ifRightDetail");
//                        IframeID.src = returnUrl;
////                        window.location = returnUrl;
//                    } catch (exception) {
//                        if (exception.description.indexOf("-1072896748") > 0) {
//                            var response = "";
//                        }
//                    }
//                } else {
//                    ShowMessages("对不起，出现错误：无法从服务器取得验证结果。");
//                }
//            }
//        }
    </script>

    <div id="WebMessages" class="MessagePanel" style="display: none">
        <table border="0">
            <tr>
                <td style="width: 15">
                    <img src="/admin/images/ico_info.gif" alt="" style="border-width: 0px;" />
                </td>
                <td id="WebContent">
                  
                </td>
            </tr>
        </table>
    </div>
    <div>
        <WEC:MessagePanel ID="Messages" runat="server">
        </WEC:MessagePanel>
    </div>
      <iframe  id="ifRightDetail" name="ifRightDetail"  frameborder="0"  width="100%" height="1500" scrolling="no"  ></iframe>
    <div style="display: none">
        <asp:TextBox ID="SiteIDText" runat="server"></asp:TextBox>
        <asp:TextBox ID="SiteNameText" runat="server"></asp:TextBox>
        <asp:TextBox ID="SiteUrlTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="AccountIDText" runat="server"></asp:TextBox>
        <asp:TextBox ID="AccountNameText" runat="server"></asp:TextBox>
        <asp:TextBox ID="ADObjectText" runat="server"></asp:TextBox>
        <asp:TextBox ID="ADUrlText" runat="server"></asp:TextBox>
    </div>
   <%-- <div style="position: absolute; top: 58%; left: 42%; height: 71px; width: 201px;">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <img alt="" id="LogingImage" src="../images/Loading_login.gif"/>
        <br />
        <br />
        广告管理证书验证并下载中....</div>--%>
</asp:Content>
