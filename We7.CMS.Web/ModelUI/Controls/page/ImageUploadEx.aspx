<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageUploadEx.aspx.cs"
    Inherits="We7.CMS.Web.Admin.ContentModel.Controls.Page.ImageUploadEx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>上传图片</title>
    <link rel="stylesheet" type="text/css" href="/ModelUI/Controls/page/js/jquery.imgareaselect/css/imgareaselect-default.css" />

    <script type="text/javascript" src="/ModelUI/Controls/page/js/jquery.imgareaselect/scripts/jquery-1.4.2.min.js"></script>

    <script type="text/javascript" src="/ModelUI/Controls/page/js/jquery.imgareaselect/scripts/jquery.imgareaselect.js"></script>

    <script type="text/javascript" src="/Admin/Ajax/jquery/ui1.8.1/jquery-ui-1.8.1.custom.min.js"></script>

    <script type="text/javascript">
    $(function(){
    
        //$("#imgBig").attr("src","F:\Documents and Settings\Administrator\My Documents\My Pictures\css兼容图片1.gif");
        $('#imgBig').imgAreaSelect({handles: true,persistent:true,x1:0,y1:0,x2:100,y2:100,resizable:false});
//        $("#fuImage").change(function(){
//            $("#imgBig").attr("src",$(this).val());      
//            $("#imgBig").imgAreaSelect({x1:120,y1:90,x2:300,y2:200,handles:true});
//        });
//        
//        $("#fuThumbnail").change(function(){
//            $("#imgThumbnail").attr("src",$(this).val());
//        });
    });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table style="position:absolute;">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <div id="bImg" style="display: block; width: 500px; height: 450px; background: red;
                                overflow: hidden;">
                                <img id="imgBig" src="1.gif" onselectstart="return false;" />
                            </div>
                            <div>
                                <table style="width: 200px;">
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
                           </div>
                        </td>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td>
                                        上传图片：
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="fuImage" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        生成缩略图
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    生成方式
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGType" runat="server">
                                                        <asp:ListItem Text="固定大小"></asp:ListItem>
                                                        <asp:ListItem Text="固定高"></asp:ListItem>
                                                        <asp:ListItem Text="固定宽"></asp:ListItem>
                                                        <asp:ListItem Text="等比例缩小"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    尺寸
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGSize" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="button" value="生成" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        使用本地缩略图
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="fuThumbnail" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        缩略图
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="display: block; width: 150px; height: 150px; background: red; overflow: hidden"">
                                            <img id="imgThumbnail" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="bttnOrign" runat="server" Text="使用原图" />
                                        <asp:Button ID="bttnThumbnail" runat="server" Text="使用缩略图" /><br />
                                        <asp:Button ID="bttnBoth" runat="server" Text="使用原图&缩略图" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
