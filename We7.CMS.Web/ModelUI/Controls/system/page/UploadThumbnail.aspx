<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadThumbnail.aspx.cs"
    Inherits="We7.Model.UI.Controls.system.page.UploadThumbnail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>生成缩略图</title>
    <link rel="stylesheet" type="text/css" href="/admin/Ajax/css/jquery.imagecropper.css" />
    <link rel="stylesheet" type="text/css" href="/admin/theme/classic/css/we7-admin.css" />
   <script type="text/javascript" src="<%=AppPath%>/Ajax/jquery/jquery-1.3.2.js"></script>
    <script type="text/javascript" src="<%=AppPath%>/Ajax/jquery/ui.core.js"></script>

    <script type="text/javascript" src="<%=AppPath%>/Ajax/jquery/ui.draggable.js"></script>

    <script type="text/javascript" src="<%=AppPath%>/Ajax/jquery/jquery.imagecropper.js"></script>

    <script type="text/javascript">

        $(document).ready(function() {
            var imageCropper = $('#imgBackground').imageCropper({
                callbacks: {
                    dragging: updateStatus,
                    zoomed: updateStatus
                }
            });
            updateStatus();

            $('#btnCropImage').click(function() {
                $('#imgCroppedImage').css('display', 'block');
            });

            $('#btnResetSettings').click(function() {
                imageCropper.settings.imagePath = $('#txtImgPath').val();
                imageCropper.settings.zoomLevel = parseFloat($('#txtZoomLevel').val());
                imageCropper.settings.left = parseInt($('#txtLeft').val());
                imageCropper.settings.top = parseInt($('#txtTop').val());
                imageCropper.settings.width = parseInt($('#txtWidth').val());
                imageCropper.settings.height = parseInt($('#txtHeight').val());
                imageCropper.reset();
            });

            function updateStatus() {
                $('#txtImgPath').val(imageCropper.settings.imagePath);
                $('#txtZoomLevel').val(imageCropper.settings.zoomLevel);
                $('#txtLeft').val(imageCropper.settings.left);
                $('#txtTop').val(imageCropper.settings.top);
                $('#txtWidth').val(imageCropper.settings.width);
                $('#txtHeight').val(imageCropper.settings.height);
            }
        });
        
            function changeFrame(v)
    {
        var s=v.options[v.selectedIndex].text;
        var size=s.split('：')[1];
        if(size)
        {
            var list = size.split('*');
	        if (list.length > 1) {
		        var width = list[0];
		        var height = list[1];
	        }
            var top= (420-height)/2;
            var left =(420-width)/2;
            $("#txtWidth").val(width);
            $("#txtHeight").val(height);
           $('#btnResetSettings').click();
        }
    }

         function Step()
       {  
        var imgDivs = $("#EditorPane>div");
       // $('#EditorPane').css('display', 'block');
        $("#EditorPane").show();           
       }
        function Ok()
        {
        var sData = dialogArguments;
        sData.ShowImgList()
         window.close();
        }
     
       window.onunload = function()
       { var sData = dialogArguments;
         sData.ShowImgList()
       }
    </script>

    <style type="text/css">
        a:link
        {
            color: #2366a8;
        }
        a:visited
        {
            color: #2366a8;
        }
        A:link, A:visited
        {
            -moz-outline: none;
            text-decoration: underline;
        }
        a:hover, a:active
        {
        }
        a img
        {
            border: 2px solid #FFFFFF;
        }
        a:hover img
        {
            border: 2px solid #81C1E7;
        }
        .style5
        {
        }
        .style6
        {
            width: 49px;
        }
        .style8
        {
            width: 183px;
            height: 222px;
        }
        .style9
        {
            height: 222px;
        }
        .style10
        {
            height: 17px;
        }
    </style>
</head>
<base target="_self">
<body>
    <form id="form1" runat="server">
    <div>
        <div style="display: none">
            <table>
                <tr>
                    <td>
                        image path:
                    </td>
                    <td>
                        <input type="text" value="" id="txtImgPath" />
                    </td>
                </tr>
                <tr>
                    <td>
                        zoom level:
                    </td>
                    <td>
                        <input type="text" value="" id="txtZoomLevel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        left:
                    </td>
                    <td>
                        <input type="text" value="" name="txtLeft" id="txtLeft" />
                    </td>
                </tr>
                <tr>
                    <td>
                        top:
                    </td>
                    <td>
                        <input type="text" value="" name="txtTop" id="txtTop" />
                    </td>
                </tr>
                <tr>
                    <td>
                        width:
                    </td>
                    <td>
                        <input type="text" value="" name="txtWidth" id="txtWidth" />
                    </td>
                </tr>
                <tr>
                    <td>
                        height:
                    </td>
                    <td>
                        <input type="text" value="" name="txtHeight" id="txtHeight" />
                    </td>
                </tr>
            </table>
            <input type="button" value="Reset Settings" id="btnResetSettings" />
        </div>
    <table width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td width="100%">
                <table border="0" cellpadding="3" cellspacing="1" width="100%" align="center" style="background-color: #b9d8f3; height:368px">
                    <tr bgcolor='#F4FAFF'>
                        <td nowrap="nowrap" align="left" class="style8" valign="top">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style5" colspan="2" style="font-size: large; font-weight: bolder">
                                        选择图片
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style6">
                                        &nbsp;
                                    </td>
                                    <td style="font-size: 12px">
                                        <asp:LinkButton ID="LinkButtonlocalHost" runat="server" OnClick="LinkButtonlocalHost_Click">本地电脑上传</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%; height: 101px;">
                                <tr> 
                                    <td class="style10" colspan="2" style="font-size: large; font-weight: bolder">  <font size="2">编辑图片</font></td>
                                      
                                </tr>
                                <tr>
                                    
                                    <td style="font-size: 12px">
                                        <asp:DropDownList ID="SizesDropDownList" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size: 12px">
                                        <asp:DropDownList ID="CutTypeDropDownList" runat="server">
                                            <asp:ListItem Selected="True" Value=''>--请选择裁切模式--</asp:ListItem>
                                            <asp:ListItem Value="W">按比例仅收缩宽到标准</asp:ListItem>
                                            <asp:ListItem Value="H">按比例仅收缩高到标准</asp:ListItem>
                                            <asp:ListItem Value="Cut">指定高宽裁减,不变形</asp:ListItem>
                                            <asp:ListItem Value="HW">缩放到指定宽高,可能变形</asp:ListItem>
                                            <asp:ListItem Value="CustomerCut">手动：按左图框定裁切</asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                        <asp:CheckBox ID="AddWatermarkCheckbox" runat="server" Text="加水印" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="GenerateButton" runat="server" Text="确定" OnClick="GenerateButton_Click" />
                        </td>
                        <td align="center" class="style9" valign="top" align="left">
                            &nbsp;<asp:Panel ID="localHostPaneUpload" runat="server" Visible="true">
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="left">
                                            <asp:FileUpload ID="ImageFileUpload" runat="server" Width="191px" />
                                            <input class="Btn" type="submit" value="上传图片" id="UploadImage" runat="server" onserverclick="UploadImage_ServerClick" >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="ImagesRepeater" runat="server">
                                                <ItemTemplate>
                                                    <div style="float: left; text-align: center">
                                                        <div style="width: 102px; height: 102px; margin: 6px; padding: 2px;">
                                                            <a href="<%=Request.RawUrl%>&imgEdit=<%# GetImgEditNmae(Container.DataItem.ToString())%>">
                                                                <img src="<%#Container.DataItem%>" style="width: 100px; height: 100px" /></a></div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div id="EditorPane" style="display: none">
                                <asp:Image ID="imgBackground" ImageUrl="~/Admin/Ajax/Images/Hydrangeas.jpg" Height="300"
                                    Width="360" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>   </div>
    </form>
</body>
</html>
