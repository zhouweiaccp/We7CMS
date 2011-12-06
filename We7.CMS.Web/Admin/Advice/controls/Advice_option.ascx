<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Advice_Option.ascx.cs" Inherits="We7.CMS.Web.Admin.controls.Advice_Option" %>
  <%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>

    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»反馈类型基本属性<br />
            <img src="/admin/images/bulb.gif" align="absmiddle" />
            <label class="block_info">
                此处对反馈类型基本属性进行修改与编辑！&nbsp;&nbsp;更多帮助，<a href="http://help.we7.cn/library/58.html" target="_blank">请参照 We7 QA 站点帮助</a></label>
        </dt>
        <dd style="width: 650px">
            <div style="float: left;">
                <table class="personalForm">
                    <tr>
                        <td style="width:18%">
                            类型名称：
                        </td>
                        <td class="formValue">
                            <asp:TextBox ID="AdviceNameText" runat="server" Columns="35"  Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" >
                            备注：
                        </td>
                        <td class="formValue">
                            <asp:TextBox ID="RemarkText" runat="server" TextMode="MultiLine" Columns="28" 
                                Height="100px" Width="250px"></asp:TextBox>
                            <br />
                            &nbsp;
                        </td>
                    </tr>
					<asp:Panel  ID="ExtraProperties" runat="server">
						<tr>
							<td>
								类型创建人：
							</td>
							<td class="formValue">
								<asp:Label ID="AdviceCreatedText" runat="server" Text="" Width="250"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								创建时间：
							</td>
							<td class="formValue">
								<asp:Label ID="StartTimeText" runat="server" Text="" Width="250"></asp:Label>
							</td>
						</tr>
					</asp:Panel>
                    <tr>
                        <td>
                        </td>
                        <td>
                           <input class="Btn" id="SubmitButton" runat="server" type="submit" value="保存当前信息"
                                onserverclick="SubmitButton_Click" />
                                &nbsp;&nbsp;
                           <input class="Btn" type="Button" value="返回列表" onclick="javascript:location.href='/admin/advice/AdviceTypes.aspx'" />
                        </td>
                    </tr>
                </table> 
            </div>
        </dd>
    </dl>
</div>
