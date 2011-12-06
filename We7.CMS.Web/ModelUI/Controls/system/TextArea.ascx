<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextArea.ascx.cs" Inherits="CModel.Controls.system.TextArea" %>
<asp:TextBox runat="server" ID="txtInput" TextMode="MultiLine" onblur="this.className='input_blur'"
    onfous="this.className='input_focus'"></asp:TextBox>