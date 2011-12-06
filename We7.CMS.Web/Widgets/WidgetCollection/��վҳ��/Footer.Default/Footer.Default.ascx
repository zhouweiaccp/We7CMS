<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Footer.Default.cs" Inherits="We7.CMS.Web.Widgets.Footer_Default" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "尾部")]
    string MetaData;
</script>
<div class="<%=CssClass %>">
<div id="footer">
    <div class="wrapper">
        <div class="service">
            <a title="联系我们" href="http://www.westengine.com/about-us/contact-us/">联系我们</a> |
            <a title="案例中心" target="_blank" href="http://www.westengine.com/cases/">案例中心</a>
            | <a title="解决方案" target="_blank" href="http://www.westengine.com/solutions/">解决方案</a>
            | <a title="版权声明" target="_blank" href="http://www.westengine.com/Other/Legal-Notices/">
                版权声明</a> | <a title="关于我们" href="http://www.westengine.com/about-us/company/">关于我们</a>
            | <a title="管理登录" target="_blank" href="/Admin/Signin.aspx">管理登录</a>
        </div>
        <div class="clear">
        </div>
        <div class="copyright">
        </div>
    </div>
</div>
</div>