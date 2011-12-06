<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Article_image.ascx.cs" Inherits="We7.CMS.Web.Admin.controls.Article_image" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<script type="text/javascript">
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
//        debugger;
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
    var topst = $("#drop_image").css("top");
    topst = reNumber(parseInt(topst));
    //alert(topst);
    var leftst = $("#drop_image").css("left");
    leftst = reNumber(parseInt(leftst));
    //alert(leftst);
    document.getElementById("<%=txt_width2.ClientID %>").value = document.getElementById("txt_width").value;
    document.getElementById("<%=txt_height2.ClientID %>").value = document.getElementById("txt_height").value;
    document.getElementById("txt_top").value = topst;
    document.getElementById("<%=txt_top2.ClientID %>").value = document.getElementById("txt_top").value;
    //document.getElementById("<%=txt_top2.ClientID %>").value = parseInt(topst);
    document.getElementById("txt_left").value = leftst;
    document.getElementById("<%=txt_left2.ClientID %>").value = document.getElementById("txt_left").value;
    //document.getElementById("<%=txt_left2.ClientID %>").value = parseInt(leftst);
    document.getElementById("<%=txt_DropWidth2.ClientID %>").value = document.getElementById("txt_DropWidth").value;
    document.getElementById("<%=txt_DropHeight2.ClientID %>").value = document.getElementById("txt_DropHeight").value;
    document.getElementById("<%=txt_Zoom2.ClientID %>").value = document.getElementById("txt_Zoom").value;
    return checkImageSet(); 
}
function reNumber(num) {
    if (num > 0) {
        num = -num;
    }
    if (num < 0) {
        num = Math.abs(num);
    }
    return num;
 }
</script>

<script src="/admin/Ajax/jquery/ui1.8.1/ui/minified/jquery.ui.core.min.js" type="text/javascript"></script>
<script src="/admin/Ajax/jquery/ui1.8.1/ui/minified/jquery.ui.widget.min.js" type="text/javascript"></script>
<script src="/admin/Ajax/jquery/ui1.8.1/ui/minified/jquery.ui.mouse.min.js" type="text/javascript"></script>
<script src="/admin/Ajax/jquery/ui1.8.1/ui/minified/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="<%=AppPath%>/Ajax/jquery/resizeImage.js"></script>
    <link href="<%=AppPath%>/Ajax/jquery/css/resizeImage.css" rel="stylesheet" type="text/css" />
<div id="main">
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
    <div class="conbox">
        <div id="conbox">
            <dl>
                <dt>» 设置文章缩略图<br>
                    <img src="/admin/images/bulb.gif" align="absmiddle" /><label class="block_info">文章缩略图可以用在头条新闻、wap或图片新闻的缩略图阵列展示中。打开cookie功能，可以自动保存上次设置状态。</label>
                    <dd>     
                    <table style="width:95%">
                    <tr>
                        <td style="width:430px;text-align:center">
                    <div id="resourceFrame">
                        <div id="bg_image">
                            <img id="o_image" src="<%=OriginalImagePath %>" />
                        </div>
                        <div id="drop">
                            <img id="drop_image" src="<%=OriginalImagePath %>" />
                        </div>
                    </div>
                    <center>
                    <table style="width:200px;">
                        <tr>
                            <td id="Min">
                                    <img alt="缩小" src="/admin/images/Minc.gif" style="width: 19px; height: 19px"
                                        id="moresmall" class="smallbig" />
                            </td>
                            <td>
                                <div id="bar">
                                    <div class="child">
                                    </div>
                                </div>
                            </td>
                            <td id="Max">
                                    <img alt="放大" src="/admin/images/Maxc.gif" style="width: 19px; height: 19px"
                                        id="morebig" class="smallbig" />
                            </td>
                        </tr>
                    </table>
                    </center>
                    </td>
                    <td style="vertical-align:top">
                     <h3>
                            原图上传(支持jpg,gif格式):</h3>
                            <dd>
                                <asp:FileUpload ID="ImageFileUpload" runat="server" Width="339px" />
                                    <input class="Btn" type="submit" value="上传图片" id="UploadImage" runat="server" onserverclick="UploadImage_ServerClick" >
                            <br />
                       原图尺寸：宽<label id="width" class="Hidden">
            <%=this.width %></label>px  高：<label id="height" class="Hidden"><%=this.height%>px</label><br />

                            <asp:DropDownList ID="SizesDropDownList" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                            <asp:DropDownList ID="CutTypeDropDownList" runat="server"  
                                        onblur="saveImageCutToCookies(this)"  >
                                <asp:ListItem Selected="True" Value=''>--请选择裁切模式--</asp:ListItem>
                                <asp:ListItem Value="W" >按比例仅收缩宽到标准</asp:ListItem>
                                <asp:ListItem Value="H" >按比例仅收缩高到标准</asp:ListItem>
                                <asp:ListItem Value="Cut" >指定高宽裁减,不变形</asp:ListItem>
                                <asp:ListItem Value="HW" >缩放到指定宽高,可能变形</asp:ListItem>
                                <asp:ListItem Value="CustomerCut">手动：按左图框定裁切</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <asp:CheckBox ID="AddWatermarkCheckbox" runat="server" Text="加水印" />
                            <br />
                           <input class="DelBtn" type="submit" value="生成缩略图" name="GenarateButton" id="GenerateButton"
                                runat="server" onclick="return saveResult()" onserverclick="GenarateButton_ServerClick" />
                          <div style="display:none">           
                         图片实际宽度： <input type="text" id="txt_width" /><br />
                         图片实际高度： <input type="text" id="txt_height" /><br />
                         距离顶部： <input type="text" id="txt_top" /><br />
                         距离左边： <input type="text" id="txt_left" /><br />
                         截取框的宽：<input type="text" id="txt_DropWidth" /><br />
                         截取框的高： <input type="text" id="txt_DropHeight" />  <br />
                         放大倍数：  <input type="text" id="txt_Zoom" value="1" /><br />
                        </div>
                        </dd>
                        </td>
                       </tr>
                    </table>          
                    </dd>
            </dl>
             <h3 style="margin:0 20px;">缩略图列表:</h3>
            <dl>
                    <dd>
                    <table cellpadding="10" cellspacing="0">
                    <asp:Repeater runat="server" ID="ImagesRepeater">
                    <ItemTemplate>
                            <tr>
                                <td  style="text-align:left;border-bottom:dotted 1px #aaa;width:200px;">
                                    <img id="Img1" alt="" src="<%# Eval("ImagePath") %>" style="max-width:200px;max-height:200px" />
                                </td>
                                <td style="text-align:left;border-bottom:dotted 1px #aaa;width:200px;">
                                    <%# Eval("Name") %> ,标签： <%# Eval("Tag") %>,文件： <%# Eval("FileName") %>
                                </td>
                                <td style="text-align:left;border-bottom:dotted 1px #aaa;width:200px;">
                                 <a href="javascript:delImage('<%# Eval("ImagePath") %>','<%# Eval("FileName") %>');">删除</a>
                                </td>
                            </tr>
                    </ItemTemplate>
                    </asp:Repeater>
               </table>
           </dd>
          </dl>
        </div>
    </div>

<script  type="text/javascript">
    loadSettingFromCookies('<%=this.ClientID %>');
    changeFrame(document.getElementById('<%=SizesDropDownList.ClientID %>'));
</script>

<div style="display: none">
    <asp:TextBox ID="IDTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="DeleteButton" runat="server" Text="Save" OnClick="DeleteButton_Click" />
    <asp:TextBox ID="ImagePathTextBox" runat="server"></asp:TextBox>
</div>
<div style="display:none">
    <asp:TextBox ID="txt_width2" runat="server"></asp:TextBox><br />
    <asp:TextBox ID="txt_height2" runat="server"></asp:TextBox><br />
    <asp:TextBox ID="txt_top2" runat="server" ></asp:TextBox><br />
    <asp:TextBox ID="txt_left2" runat="server" ></asp:TextBox><br />
     <asp:TextBox ID="txt_DropWidth2" runat="server" ></asp:TextBox><br />
    <asp:TextBox ID="txt_DropHeight2" runat="server" ></asp:TextBox><br />
    <asp:TextBox ID="txt_Zoom2" runat="server" ></asp:TextBox>
    <input type="hidden" id="reName" value="" />
</div>