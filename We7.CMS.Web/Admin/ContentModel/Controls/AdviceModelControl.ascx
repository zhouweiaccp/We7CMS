<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdviceModelControl.ascx.cs"
    Inherits="We7.CMS.Web.Admin.ContentModel.Controls.AdviceModelControl" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<WEC:MessagePanel ID="Msg" runat="server" Width="900px">
</WEC:MessagePanel>
<div>
    <img src="/admin/images/bulb.gif" align="absmiddle" />
    <label class="block_info">
        <asp:Literal runat="server" ID="InfoLiteral" Text="反馈控件用于反馈录入与反馈信息显示"></asp:Literal></label>
</div>
<br />
<div class="clear">
</div>
<div>
    <div id="conbox">
        <dl>
            <dt>»生成反馈控件<br>
                <dd>
                    <div>
                        <asp:Button ID="btnGenarate" runat="server" Text="生成反馈控件" CssClass="Btn" OnClick="btnGenarate_Click" />
                    </div>
                </dd>
            </dt>
        </dl>
    </div>
</div>
