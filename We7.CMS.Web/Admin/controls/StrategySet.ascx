<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StrategySet.ascx.cs" Inherits="We7.CMS.Web.Admin.StrategySet" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<WEC:MessagePanel id="Messages" runat="server" ></WEC:MessagePanel>
  <DIV id="conbox">             
    <DL style="padding:0">
<DT style="padding:0">»IP过滤设置<br />
    <img src="/admin/images/bulb.gif" align="absmiddle"/>
    <LABEL class="block_info">通过选择预先设定的IP策略，对访问的IP进行限制。</LABEL> 
    <DL style="padding:0">
        <dd style="padding:0"><br />
            <table style="width:300px;">
                <tr>
                    <td style="font-weight:bold">
                        未选中策略：
                    </td>
                    <td>
                        &nbsp;</td>
                    <td style="font-weight:bold">
                        选中策略：
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ListBox ID="lstUnSelected" SelectionMode="Multiple" runat="server" Height="150px" Width="130px" />
                    </td>
                    <td>
                        <asp:Button ID="bttnRightAll" runat="server" Text=">>" Width="30px"  ToolTip="添加所有策略"
                            onclick="bttnRightAll_Click" UseSubmitBehavior="false" CssClass="ArrowButton"  /><br />
                        <asp:Button ID="bttnRight" runat="server" Text=">" Width="30px" 
                            onclick="bttnRight_Click" UseSubmitBehavior="false" CssClass="ArrowButton" ToolTip="添加选中策略" /><br />
                        <asp:Button ID="bttnLeft" runat="server" Text="<" Width="30px" 
                            onclick="bttnLeft_Click" UseSubmitBehavior="false" CssClass="ArrowButton"  ToolTip="移除选中策略" /><br />
                        <asp:Button ID="bttnLeftAll" runat="server" Text="<<" Width="30px" 
                            onclick="bttnLeftAll_Click" UseSubmitBehavior="false" CssClass="ArrowButton" ToolTip="移除所有策略" />
                    </td>
                    <td>            
                        <asp:ListBox ID="lstSelected" SelectionMode="Multiple" runat="server" Height="150px" Width="130px" />
                    </td>
                </tr>
                <td colspan="3" align="center"><br />
                    <asp:Button ID="bttnSave" runat="server" CssClass="button" UseSubmitBehavior="false" Text="　　保　存　　" onclick="bttnSave_Click" />
                </td>
            </table>
        </dd>
    </DL>    
    </DT>
</dl>
</DIV>
