<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/content.Master" AutoEventWireup="true"
    Codebehind="About.aspx.cs" Inherits="We7.CMS.Web.Admin.About" Title="Untitled Page" %>

<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <br />
        <asp:Image ID="Image1" runat="server"  ImageUrl="~/admin/Images/we7logo.png" /><br />
    <br />
    <div>
        <table class="content" width="100%">
            <tr>
                <td style="width: 154px">
                    产品版本:</td>
                <td style="width: 486px">
                    <asp:Label ID="lblVersion" runat="server" Height="23px" Width="141px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 154px">
                    官方论坛:</td>
                <td style="width: 486px">
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://bbs.we7.cn/" Width="271px">http://bbs.we7.cn/</asp:HyperLink></td>
            </tr>
            <tr>
                <td style="width: 154px">
                    技术网址:</td>
                <td style="width: 486px">
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="http://www.we7.cn/" Width="186px">http://www.we7.cn/</asp:HyperLink></td>
            </tr>
            <tr>
                <td style="width: 154px">
                    公司网址:</td>
                <td style="width: 486px">
                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="http://www.westengine.com"
                        Width="313px">http://www.westengine.com/</asp:HyperLink></td>
            </tr>
            <tr>
                <td style="width: 154px">
                    热线电话:</td>
                <td style="width: 486px">
                    010 - 82609944/54, 158 1110 6161
                </td>
            </tr>
            <tr>
                <td style="width: 154px">
                </td>
                <td style="width: 486px">
                </td>
            </tr>
        </table>
        <br />
        &nbsp;Copyright 2001-2008 版权所有 &nbsp;西部动力（北京）科技有限公司<br />
        <span class="font8"></span><span class="font4"> </span>
    </div>
</asp:Content>
