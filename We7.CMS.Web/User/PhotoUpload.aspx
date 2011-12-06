<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhotoUpload.aspx.cs" MasterPageFile="~/User/DefaultMaster/content.Master"
    Inherits="We7.CMS.Web.User.PhotoUpload" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
   
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
    <script src="/Admin/Ajax/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/article.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/cookie.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/tags.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <form runat="server" id="form1">
    <div>
        <WEC:MessagePanel ID="Messages" runat="server">
        </WEC:MessagePanel>
    </div>
    <div class="realRight ml10">
        <div class="mybox">
            <div class="mytit">
                个人资料</div>
            <div class="con">
                <div class="pCenter" id="pCenter">
                    <div class="at_tab_t">
                        <a href="/Plugins/ShopPlugin/UI/UserInfo.aspx" class="at_tab_i">基本资料</a>
                        <a href="/Plugins/ShopPlugin/UI/ContractInfo.aspx" class="at_tab_i">联系方式</a>
                        <a href="/Plugins/ShopPlugin/UI/PhotoUpload.aspx" class="at_tab_i at_current">头像照片</a>
                    </div>
                    <form id="form2" runat="server">
                    <div class="at_tab_c">
                        <div class="at_tab_l">
                            <table class="basicInfor">
						 <tbody>
							 <tr>
								 <td width="250px" valign="top">
								 <h2>正在使用的头像:</h2>
								 <p class="mt5 headimg clearfix">
								 <span class="big fc9">
								  <asp:Image id="imgPhoto" width="120px" height="120px" runat="server"></asp:Image><br>120*120</span>
								 </p>
								 </td>
								 <td>
								 <p class="mb5"><strong>上传我的新头像</strong><span class="orange1 ml5">上传真人头像，获得更多关注！</span></p>
								    <asp:FileUpload runat="server" ID="fuPhoto"></asp:FileUpload><asp:Button ID="bttnUpload" runat="server" Text="上传" />
								 <p class="mt5"><strong>上传说明:</strong></p>
								 <p class="fc9 mt5">图片格式：支持jpg/png/jpeg/gif格式的图片<br>图片大小：大小不要超过1M<br>操作说明：头像保存后，您需要刷新一下本页面(按F5键)，才能查看最新的头像效果。</p>
								 <p class="mt5"><strong>头像照片示例:</strong></p>
								 <dl class="Exaimg clearfix mt5">
								   <dt><img src="/Plugins/ShopPlugin/UI/css/img/hmm.jpg"></dt>
								   <dd class="ml10 mt5"><strong>本人真实照片</strong><p class="fc9">清晰、正面、免冠<br>近期拍摄<br>登记照、生活照</p></dd>
								 </dl>
								 </td>
							 </tr>
						 </tbody>
						</table>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
