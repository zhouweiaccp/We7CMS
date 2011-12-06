<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Thumbnail.aspx.cs" Inherits="We7.CMS.Web.Admin.cgi_bin.controls.Thumbnail" %>

<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>生成缩略图</title>

    <script type="text/javascript">
        function setValue(el,v)
        {
            opener.document.getElementById("thumb_"+el).src=v;
            opener.document.getElementById(el).value=v;
            window.close();
        }
        function delImage(existPath,fn) {
            if(confirm("您要删除缩略图”"+fn+"“吗？"))
            {
                var IDTextBox = document.getElementById("<%=IDTextBox.ClientID %>");
                IDTextBox.value = existPath;
                var ShowButton = document.getElementById("<%=DeleteButton.ClientID %>");
                ShowButton.click();
            }
        }
        
        function checkImageSet() {
            var smallSize = document.getElementById("<%=SizesDropDownList.ClientID %>");
            if (!smallSize.value || smallSize.value == '0') {
                alert("您还没有设定生成缩略图的尺寸！");
                smallSize.focus();
                return false;
            }

            var smallCut = document.getElementById("<%=CutTypeDropDownList.ClientID %>");
            if (!smallCut.value) {
                alert("您还没有设定裁切缩略图的模式！");
                smallCut.focus();
                return false;
            }
            var ddl = document.getElementById("<%=SizesDropDownList.ClientID %>")
            var index = ddl.selectedIndex;
            var Text = ddl.options[index].text;
            var list = Text.split('：');
            if (list.length > 1) {
                var imageSize = list[1];
            }
            var imagePathTextBox = document.getElementById("<%=ImagePathTextBox.ClientID %>");
            var imagePath = imagePathTextBox.value;
            if (imagePath.indexOf(imageSize) != -1) {
                var messages = "您确定覆盖原来对应的缩略图？";
                var ifConfirm = window.confirm(messages);
                if (ifConfirm) {
                    return true;
                }
                else {
                    return false;
                }
            }
            return true;
        }

    function saveResult()
    {
        document.getElementById("<%=txt_width2.ClientID %>").value=  document.getElementById("txt_width").value;
        document.getElementById("<%=txt_height2.ClientID %>").value=document.getElementById("txt_height").value;
        document.getElementById("<%=txt_top2.ClientID %>").value=document.getElementById("txt_top").value;
        document.getElementById("<%=txt_left2.ClientID %>").value= document.getElementById("txt_left").value;
        document.getElementById("<%=txt_DropWidth2.ClientID %>").value=document.getElementById("txt_DropWidth").value;
        document.getElementById("<%=txt_DropHeight2.ClientID %>").value=document.getElementById("txt_DropHeight").value;
         document.getElementById("<%=txt_Zoom2.ClientID %>").value=document.getElementById("txt_Zoom").value;
        return checkImageSet(); 
    }
    </script>

    <script type="text/javascript" src="/admin/ajax/jquery/jquery.latest.min.js"></script>

    <script type="text/javascript" src="/Admin/Ajax/jquery/ui.core.packed.js"></script>

    <script type="text/javascript" src="/Admin/Ajax/jquery/ui.draggable.packed.js"></script>

    <script type="text/javascript" src="/Admin/cgi-bin/Article.js"></script>

    <script type="text/javascript" src="/Admin/cgi-bin/cookie.js"></script>

    <script type="text/javascript" src="/Admin/cgi-bin/Article.js"></script>

    <script type="text/javascript" src="/Admin/Ajax/jquery/Thumbnail.js">
    </script>

    <link href="/Admin/Ajax/jquery/css/resizeImage.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/Admin/theme/classic/css/article.css"
        media="screen" />
    <link rel="shortcut icon" href="/favicon.ico" />
    <link rel="stylesheet" href="../theme/classic/css/global.css" type="text/css" media="all" />
    <link rel="stylesheet" href="../theme/classic/css/we7-admin.css" type="text/css"
        media="all" />
    <link rel="stylesheet" href="../theme/classic/css/colors-fresh.css" type="text/css"
        media="all" />
    <link rel="stylesheet" type="text/css" href="../theme/classic/css/main.css" media="screen" />
    <style>
        td
        {
            padding: 0;
            margin: 0;
            font-size: 12px;
            color: #666;
        }
        h3
        {
            color: #EB976B;
            font-size: 14px;
            padding: 0;
            margin: 0;
            line-height: 150%;
        }
    </style>
</head>
<body class="we7-admin" style="padding:0; margin:0;">
    <form id="form1" runat="server">
    <div>
        <div id="main">
            <WEC:MessagePanel ID="Messages" runat="server">
            </WEC:MessagePanel>
            <table style="width: 95%">
                <tr>
                    <td style="width: 430px; text-align: center">
                        <div id="resourceFrame">
                            <div id="bg_image">
                                <img id="o_image" src="<%=OriginalImagePath %>" />
                            </div>
                            <div id="drop">
                                <img id="drop_image" src="<%=OriginalImagePath %>" />
                            </div>
                        </div>
                        <center>
                            <table style="width: 200px; display:none;" >
                                <tr>
                                    <td id="Min">
                                        <img alt="缩小" src="/admin/images/Minc.gif" style="width: 19px; height: 19px" id="moresmall"
                                            class="smallbig" />
                                    </td>
                                    <td>
                                        <div id="bar">
                                            <div class="child">
                                            </div>
                                        </div>
                                    </td>
                                    <td id="Max">
                                        <img alt="放大" src="/admin/images/Maxc.gif" style="width: 19px; height: 19px" id="morebig"
                                            class="smallbig" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </td>
                    <td style="vertical-align: top">
                        <h3>
                            原图上传(支持jpg,gif格式):</h3>
                        <asp:FileUpload ID="ImageFileUpload" runat="server" Width="339px" />
                        <asp:Button ID="UploadImage" runat="server" Text="上传图片" UseSubmitBehavior="false"  class="Btn" OnClick="SaveUploadImage" />
                        <br />
                        原图尺寸：宽<label id="width" class="Hidden">
                            <%=this.width %></label>px 高：<label id="height" class="Hidden"><%=this.height %>px</label><br />
                        <asp:DropDownList ID="SizesDropDownList" runat="server" AutoPostBack="false">
                        </asp:DropDownList>
                        <asp:DropDownList ID="CutTypeDropDownList" runat="server" onblur="saveImageCutToCookies(this)">
                            <asp:ListItem Selected="True" Value=''>--请选择裁切模式--</asp:ListItem>
                            <asp:ListItem Value="W">按比例仅收缩宽到标准</asp:ListItem>
                            <asp:ListItem Value="H">按比例仅收缩高到标准</asp:ListItem>
                            <asp:ListItem Value="Cut">指定高宽裁减,不变形</asp:ListItem>
                            <asp:ListItem Value="HW">缩放到指定宽高,可能变形</asp:ListItem>
                            <asp:ListItem Value="CustomerCut">手动：按左图框定裁切</asp:ListItem>
                        </asp:DropDownList>
                
                        <div>
                        <asp:CheckBox ID="AddWatermarkCheckbox" runat="server" Text="加水印" /></div>
                      
                        <asp:Button ID="GenerateButton" CssClass="DelBtn" runat="server" UseSubmitBehavior="false" onfocus="this.blur()" OnClientClick="if(!saveResult())return;"
                            OnClick="GenarateButton_ServerClick" Text="生成缩略图" />
                        <input class="DelBtn" type="button" value="选择缩略图" name="SelectButton" id="SelectButton"
                            onclick="setValue('Thumbnail','<%=ThumbnailPath %>')" />
                        <div style="display:none">
                            图片实际宽度：
                            <input type="text" id="txt_width" runat="server" /><br />
                            图片实际高度：
                            <input type="text" id="txt_height" runat="server" /><br />
                            距离顶部：
                            <input type="text" id="txt_top" runat="server" /><br />
                            距离左边：
                            <input type="text" id="txt_left" runat="server" /><br />
                            截取框的宽：<input type="text" id="txt_DropWidth" runat="server" /><br />
                            截取框的高：
                            <input type="text" id="txt_DropHeight" runat="server" />
                            <br />
                            放大倍数：
                            <input type="text" id="txt_Zoom" runat="server" /><br />
                        </div>
                        <div style="margin: 0 auto; padding:10px 0;">
                            <img src="<%=ThumbnailPath %>" />
                        </div>
                    </td>
                </tr>
            </table>
            <div style="display: none">
                <asp:TextBox ID="IDTextBox" runat="server"></asp:TextBox>
                <asp:Button ID="DeleteButton" runat="server" Text="Save" />
                <asp:TextBox ID="ImagePathTextBox" runat="server"></asp:TextBox>
            </div>
            <div style="display: none">
                <asp:TextBox ID="txt_width2" runat="server"></asp:TextBox><br />
                <asp:TextBox ID="txt_height2" runat="server"></asp:TextBox><br />
                <asp:TextBox ID="txt_top2" runat="server"></asp:TextBox><br />
                <asp:TextBox ID="txt_left2" runat="server"></asp:TextBox><br />
                <asp:TextBox ID="txt_DropWidth2" runat="server"></asp:TextBox><br />
                <asp:TextBox ID="txt_DropHeight2" runat="server"></asp:TextBox><br />
                <asp:TextBox ID="txt_Zoom2" runat="server"></asp:TextBox>
            </div>
        </div>
    </form>
</body>
</html>
