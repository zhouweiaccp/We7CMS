<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/theme/classic/content.Master"
    AutoEventWireup="true" CodeBehind="EditLayout.aspx.cs" Inherits="We7.CMS.Web.Admin.ContentModel.EditLayout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <style>
        .rTop
        {
            width: 100px;
            text-align: center;
            font-size: small;
            line-height: 25px;
            border: 1px solid #999;
            position: fixed;
            right: 80px;
            bottom: 250px;
            border-bottom-color: #333;
            border-right-color: #333;
            margin: 5px;
        }
        .opl
        {
            padding: 2px 0 2px 0;
        }
    </style>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="内容模型页面布局"></asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="包括录入表单的自定义与管理用列表的自定义">
            </asp:Label>
        </span>
    </h2>
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
    <link href="/Admin/Ajax/jquery/ui1.8.1/css/ui-lightness/jquery-ui-1.8.1.custom.css"
        rel="stylesheet" type="text/css" />
    <link media="screen" rel="stylesheet" href="<%=AppPath%>/ajax/jquery/colorbox/colorbox.css" />
    <link href="/Admin/ContentModel/css/formbuilder.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/ContentModel/js/Common.js" type="text/javascript"></script>
    <script src="/Scripts/jQuery/jQueryUI/1.8.5/js/jquery-ui-1.8.5.custom.min.js" type="text/javascript"></script>
    <script src="<%=AppPath%>/cgi-bin/article.js" type="text/javascript"></script>
    <script src="<%=AppPath%>/cgi-bin/cookie.js" type="text/javascript"></script>
    <script src="<%=AppPath%>/cgi-bin/tags.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Admin/cgi-bin/CheckBrowser.js"></script>
    <script src="<%=AppPath%>/ajax/jquery/colorbox/jquery.colorbox-min.js"></script>
    <script type="text/javascript" src="/scripts/we7/we7.loader.js">
      we7("span[rel=xml-hint]").help();
      we7('.tipit').tip();

      we7.beforeUnload("表单已经发生改变，您尚未保存，确定离开吗?", IsChange);

        function IsChange() {
            var tab = we7.queryString("tab");
            if(we7.isUndef(tab) || tag.length==0)
            tab=1;
            var t = '';
            $("#formFields>li").each(function (i) { if (!we7.isUndef($(this).attr("myid"))) t += $(this).attr("myid") + "," });
            var isChange = (t != $("#myId").val());
            if ((tab == 1 || tab == 3) && isChange)
                return true;
            else return false
        }
    </script>
    <div id="mycontent">
        <div class="Tab menuTab">
            <ul class="Tabs">
                <asp:Label runat="server" ID="MenuTabLabel"></asp:Label>
            </ul>
        </div>
        <div class="clear">
        </div>
        <div id="rightWrapper">
            <div id="container" style="display: table">
                <asp:PlaceHolder runat="server" ID="ContentHolder"></asp:PlaceHolder>
            </div>
        </div>
    </div>
    <div class="rTop" id="rTop">
        <ul>
            <li class="opl"><a href="javascript:void(0);" onclick="CreateModelTable()">创建表</a></li>
            <li class="opl"><a href="javascript:void(0);" onclick="CreateWidget()">创建部件</a></li>
            <li class="opl"><a href="javascript:void(0);" onclick="CreateLayout()">自定义布局</a></li>
            <li class="opl"><a href="javascript:void(0);" onclick="AddLeftMenu()">添加至左侧菜单</a></li>
        </ul>
    </div>
    <script type="text/javascript" language="javascript">
        function newForm(url, title, w, h) {
            $.colorbox({ width: w, height: h, href: url, iframe: true,
                overlayClose: false, escKey: false,
                onClosed: function () {
                }
            });
        }
        function CreateModelTable() {
            we7.loading("操作中");
            var url = "/Admin/ContentModel/ajax/ContentModel.asmx/CreateModelTable";
            var modelName = we7.queryString("modelname");
            var data = { model: modelName };
            var msg;
            $.ajax({
                url: url,
                data: jsonToString(data),
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (json) {
                    var result = stringToJSON(json);
                    if (result.success) {
                        we7.info("创建成功！");
                       
                    }
                    else {
                        we7.info("创建失败！错误信息：" + result.msg, { autoHide: true, hideTimeout: 6000 });
                    }
                },
                failure: function () {
                    we7.info("应用程序错误!", { autoHide: true, hideTimeout: 6000 });
                }
            });
        }

        function CreateLayout() {
            newForm('/Admin/ContentModel/EditDetail.aspx?t=layout&modelname='+we7.queryString("modelname"), '编辑布局', "80%", "100%");
        }
        function CreateWidget() {
            newForm('/Admin/ContentModel/EditDetail.aspx?t=widget&modelname='+we7.queryString("modelname"), '编辑部件', "80%", "80%");
        }

        function CloseForm()
        {
         $.fn.colorbox.close();
         we7.loading("添加成功,即将刷新左侧菜单..", {autoHide:true,hideTimeout:2000});
         setTimeout(function(){ 
            if (location.href.indexOf("?") > 0)
                location.href = location.href + '&reload=menu';
            else
                location.href = location.href + '?reload=menu';
                },2000);
       
        }
    function AddLeftMenu()
    {
      var url='/admin/manage/AddMenu.aspx?modelname=' + we7.queryString("modelname") + '&nomenu=nomenu';
      $.colorbox({ width: "60%",height: "95%", href: url, iframe: true,
                overlayClose: false, escKey: false});
    }

        function iframeAutoFit() {
            try {
                if (window != parent) {
                    var a = parent.document.getElementsByTagName("IFRAME");
                    for (var i = 0; i < a.length; i++) {
                        if (a[i].contentWindow == window) {
                            var h1 = 0, h2 = 0;
                            a[i].parentNode.style.height = a[i].offsetHeight + "px";
                            a[i].style.height = "10px";
                            if (document.documentElement && document.documentElement.scrollHeight) {
                                h1 = document.documentElement.scrollHeight;
                            }
                            if (document.body) h2 = document.body.scrollHeight;
                            var h = Math.max(h1, h2);
                            if (document.all) { h += 4; }
                            if (window.opera) { h += 1; }
                            a[i].style.height = a[i].parentNode.style.height = h + "px";
                        }
                    }
                }
            }
            catch (ex) { }
        }
        if (window.attachEvent) {
            window.attachEvent("onload", iframeAutoFit);
        }
        else if (window.addEventListener) {
            window.addEventListener('load', iframeAutoFit, false);
        }
    </script>
    <!---formbuilder-->
    <script src="/Admin/ContentModel/js/FormBuilder/Jquery.extend.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/We7.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/Control.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/Field.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/TextBox.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/CheckBox.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/CheckBoxList.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/DropdownList.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/FileUpload.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/Password.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/RadioButton.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/RadioButtonList.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/TextArea.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/SelectInput.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/CategoryDropdownList.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/GovSN.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/DepartmentEx.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/Editor.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/XMLSelect.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/GUID.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/Number.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/DateTime.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/DateTimeLabel.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/ChannelSelect.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/Thumbnail.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/Label.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/RelationSelect.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/Attachment.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/ThumbnailEx.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/TypeProperty.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/ChildrenTable.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/ChildrenTableHorizontal.js" type="text/javascript"></script>
    <script src="/Admin/ContentModel/js/FormBuilder/MultiUploadify.js" type="text/javascript"></script>
</asp:Content>
