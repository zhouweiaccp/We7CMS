<%@ Page Language="C#" MasterPageFile="~/User/DefaultMaster/content.Master"  AutoEventWireup="true" CodeBehind="SendValidateMail.aspx.cs" Inherits="We7.CMS.Web.User.SendValidateMail" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
<script type="text/javascript" src="/Admin/Ajax/jquery/ui1.8.1/jquery-1.4.2.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <div>
            <table>
                            <tr>
                                <td>
                                    您注册的用户名:
                                </td>
                                <td>
                                    <input id="loginName" />
                                </td>
                            </tr>
                            <tr>
                                <td>你注册时填写的邮箱：</td>
                                <td><input id="email" />   <label id="msg" style="color:Red"></label></td>
                            </tr>
                            <tr>
                            <td></td>
                                <td >
                                    <input id="btnSendMail"   type="button" value="发送验证邮件" />
                                    <br />  <br />
                                     <label id="successMsg" style="color:Green;font-weight:bold"></label>
                                </td>
                            </tr>
                        </table>
    </div>
    <script type="text/javascript">
    $(function(){
        $("#btnSendMail").click(function(){
             if($.trim($("#loginName").val())=="")
             {
                $("#msg").text("用户名不能为空！");
                return;
             }      
             if($.trim($("#email").val())=="")
             {
                $("#msg").text("您邮箱还没有填呢！");
                return;
             }

             $("#msg").text('');  
             $("#successMsg").text('');  
                        
             $.ajax({
                url:'/user/Action/SendMail.ashx',
                data:"loginName="+$("#loginName").val()+"&email="+$("#email").val(),
                type:'get',
               success:  function(msg)
                {   
                    if(msg=='0')
                         $("#successMsg").text("：）已成功发送邮件到信箱 "+$("#email").val()+"，请到邮箱查收，并按邮件中提示进行操作 ！");
                    else
                        $('#msg').text("发送验证邮件失败："+msg);
                },
                failure:function(){
                     $("#msg").text("发送验证邮件失败！");
                },
                error:function(){
                     $("#msg").text("发送验证邮件失败！");
                }
             });
         });
    });
    </script>
</asp:Content>