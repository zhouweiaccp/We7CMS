<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Number.ascx.cs" Inherits="We7.Model.UI.Controls.system.Number" %>
<asp:TextBox runat="server" ID="txtInput" CssClass="number" onblur="this.className='input_blur'"
    onfous="this.className='input_focus'"></asp:TextBox>
<asp:RegularExpressionValidator ID="rvInput" runat="server" ControlToValidate="txtInput"
    Display="Dynamic" Text="*" ValidationExpression="(^(0|[1-9]\d*)\.(\d+)$)|(^\d*$)"></asp:RegularExpressionValidator>