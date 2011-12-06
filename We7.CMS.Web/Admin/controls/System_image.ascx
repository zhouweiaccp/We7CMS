<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="System_image.ascx.cs" Inherits="We7.CMS.Web.Admin.controls.System_image" %>
<style type="text/css">
    .style1
    {
        text-align: right;
    }
    .Navbutton
    {
        text-align: center;
    }
</style>
<div >
       <table cellspacing="0" cellpadding="4" align="left" style="width: 63%">
		                <tr style="text-align: right">
                                        <td class="style1">图片缩略图水印类型: </td>
                                        <td style="text-align: left" >
                                            <ASP:RadioButtonList ID="watermarktype" runat="server" 
                                                RepeatDirection="Horizontal" Font-Size="12px" Width="100px">
                                                <asp:ListItem Value="0">文字</asp:ListItem>
                                                <asp:ListItem Value="1">图片</asp:ListItem>
                                            </ASP:RadioButtonList>
                                        </td>
		                            </tr>
		                            <tr>
                                        <td class="style1">文字型水印的内容:</td>
                                        <td>
                                            <ASP:TextBox ID="watermarktext" runat="server" HintTitle="提示" 
                                                HintInfo="可以使用替换变量: {1}表示论坛标题 {2}表示论坛地址 {3}表示当前日期 {4}表示当前时间.例如: {3} {4}上传于{1} {2}"
                                                Width="200px" RequiredFieldType="暂无校验" CanBeNull="必填" IsReplaceInvertedComma="false"></ASP:TextBox>
                                        </td>
		                            </tr>
		                            <tr>
                                        <td class="style1">文字水印大小:</td>
                                        <td>
                                            <ASP:TextBox ID="watermarkfontsize" runat="server" Size="7" CanBeNull="必填" RequiredFieldType="数据校验"
                                                MaxLength="5"></ASP:TextBox>(单位:像素)
                                                <asp:RegularExpressionValidator ID="rlevl" runat="server" ControlToValidate="watermarkfontsize"
                                        ErrorMessage="请输入一个正整数" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                        </td>
		                            </tr>
		                            <tr>
                                        <td class="style1">文字水印字体:</td>
                                        <td>
                                            <ASP:DropDownList ID="watermarkfontname" runat="server">
                                            </ASP:DropDownList>
                                        </td>
		                            </tr>
		                            <tr>
                                        <td class="style1">图片型水印文件:</td>
                                        <td>
                                            <ASP:TextBox ID="watermarkpic" runat="server" ToolTip= "提示:附加的水印图片需存放到网站数据目录_data的watermark子目录下.注意:如果图片不存在系统将使用文字类型的水印. "
                                                Width="200px" RequiredFieldType="暂无校验" CanBeNull="必填" IsReplaceInvertedComma="false"></ASP:TextBox>
                                        </td>
		                            </tr>
		                            <tr>
                                        <td class="style1">图片水印透明度:</td>
                                
                                        <td>
                                            <ASP:TextBox ID="watermarktransparency" runat="server" ToolTip="提示：取值范围1--10 (10为不透明)."
                                                RequiredFieldType="数据校验" MaxLength="2" CanBeNull="必填" Size="5"></ASP:TextBox>
                                        </td>
		                            </tr>
		                            <tr>
                                        <td class="style1">JPG图片质量:</td>
                                
                                        <td>
                                            <ASP:TextBox ID="attachimgquality" runat="server" ToolTip="提示：本设置只适用于加水印的jpeg格式图片.取值范围 0-100, 0质量最低, 100质量最高, 默认80"
                                                Size="5" CanBeNull="必填" RequiredFieldType="数据校验" Text="80" MaxLength="3"></ASP:TextBox>
                                        </td>
		                            </tr>
		                            <tr>
                                        <td class="style1">选择水印位置:</td>
                                
                                        <td width="260" >
                                            <table cellspacing="0" cellpadding="4" width="256" >
                                                <tr class="altbg2" align="left">
                                                    <td title="请在此选择水印添加的位置(共 9 个位置可选).添加水印暂不支持动画 GIF 格式. 附加的水印图片在下面的使用的 图片水印文件 中指定.">
                                                        <asp:Literal ID="position" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
		                </tr>
		                            <tr>
                                        <td class="style1">&nbsp;</td>
                                
                                        <td width="260" >
                                            &nbsp;</td>
		                </tr>
		                            <tr>
                                        <td class="style1">自动缩小上传图片到最大限定尺寸</td>
                                
                                        <td width="260" >
                                            <asp:CheckBox ID="CuttoMaxCheckBox" runat="server" Text=" " />
                                        </td>
		                </tr>
		                            <tr>
                                        <td class="style1">上传图片的最大宽度，单位：像素</td>
                                
                                        <td width="260" >
                                            <ASP:TextBox ID="MaxWidthOfUploadedImgTextbox" runat="server" ToolTip= "提示:附加的水印图片需存放到网站数据目录_data的watermark子目录下.注意:如果图片不存在系统将使用文字类型的水印. "
                                                Width="200px" RequiredFieldType="暂无校验" CanBeNull="必填" 
                                                IsReplaceInvertedComma="false"></ASP:TextBox>
                                        </td>
		                </tr>
		                            <tr>
                                        <td class="style1">&nbsp;</td>
                                
                                        <td width="260" >
                <asp:Button ID="SaveButton" runat="server" onclick="SaveButton_Click" Text="保 存" 
                    Width="73px" />
                                        </td>
		                </tr>
		            </table>
            
    </div>