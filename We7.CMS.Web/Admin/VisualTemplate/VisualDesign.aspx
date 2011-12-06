<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualDesign.aspx.cs" Inherits="We7.CMS.Web.Admin.VisualTemplate.VisualDesign" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>可视化设计</title>
    <link href="/Scripts/jQuery/jQueryUI/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jQuery/jquery.js" type="text/javascript"></script>
    <link href="/Scripts/jQuery/plugin/colorpicker/css/colorpicker.css" rel="stylesheet"
        type="text/css" />
    <link href="/admin/ajax/jquery/colorbox/colorbox.css"  rel="stylesheet" type="text/css" />
    <script src="/Scripts/jQuery/plugin/colorpicker/colorpicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        if ($.browser.msie) {
            var version = $.browser.version;
            if (parseFloat(version) < 7.0) {
                location.href = "nosupport.html";
            }
        }
    </script>
    <script src="/Scripts/ckfinder/ckfinder.js?20110422004" type="text/javascript"></script>
    <script src="/Scripts/jQuery/jQueryUI/jquery-ui.js" type="text/javascript"></script>
    <!--<script src="/Scripts/jQuery/plugin/jquery.scrollfollow.js" type="text/javascript"></script>-->
    <script src="/Scripts/Common.js" type="text/javascript"></script>
    <script src="/Admin/VisualTemplate/Scripts/writeCapture.js" type="text/javascript"></script>
    <script src="/Admin/VisualTemplate/Scripts/jquery.drag.js" type="text/javascript"></script>
    <script src="/Admin/VisualTemplate/Scripts/vdmain.js?20110901002" type="text/javascript"></script>
    <script src="/Admin/VisualTemplate/Scripts/adminpanel.js?20110622001" type="text/javascript"></script>
    <script src="Scripts/TemplateBuilder.js?20110614001" type="text/javascript"></script>
    <script src="/admin/cgi-bin/DialogHelper.js" type="text/javascript"></script>    
    <script src="/admin/ajax/jquery/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    
    <link id="test" rel="stylesheet" type="text/css" href="style/VisualDesign.Simple.css?20110509001"
        media="screen" />
    <link id="Link1" rel="stylesheet" type="text/css" href="style/FloatPanel.css?20110901001"
        media="screen" />
    <style type="text/css">
        #LayoutList .layoutskin
        {
            width: 120px;
            height: 120px;
            float: left;
            margin: 10px 10px 10px 10px;
            border-width: 1px;
            cursor: pointer;
            text-align: center;
        }
        #LayoutList .layoutskin img
        {
            width: 120px;
            height: 100px;
        }
        #LayoutList .layoutskin:hover
        {
            border: dashed 1px blue;
        }
        .clear
        {
            clear: both;
            height: 0px;
        }
        .selectskin
        {
            border: solid 1px red;
        }
        #tooboxhandle:hover
        {
            cursor: move;
        }
        #tooboxhandle > a:hover
        {
            cursor: pointer;
        }
        .colorpicker
        {
            z-index: 100000;
        }
        .publishInfo
        {
            width: 100%;
            height: 40px;
            font-family: "宋体";
            font-size: 16px;
            font-weight: bold;
            list-style: 40px;
            color: #666666;
            border: 1px dashed #CCC;
            background: #d4ffc0;
            padding: 5px 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        var CommonData = {};
        CommonData.IsDemoSite =<%=IsDemoSite %>;
    </script>
    <div id="showtoolwarp" style="display: none; bottom: 0px; left: 1px!important; left: 18px;
        width: 200px; border: 1px solid #333; padding: 5px; font-size: 14px; background-color: #B6EAFD;
        position: fixed; z-index: 99999;">
        We7部件工具箱
        <%--<a hidefocus="" href="###" title="关闭" class="window_close" id="A1" style="display: block;">
        </a>--%>
        <a href="javascript:void(0);" id="btnmax" style="display: block;" class="window_restore">
        </a>
    </div>
    <div id="warp">
        <div id="toolbar">
            <div class="rightBar">
                <asp:Image ID="LogoImage" runat="server" Style="vertical-align: bottom;" ImageUrl="~/admin/Images/icons_look.gif" />
                正在设计：
                <asp:Label ID="NameLabel" runat="server" Text="模板编辑"></asp:Label>
                <span class="summary">文件
                    <asp:Label ID="SummaryLabel" runat="server" Text=""> </asp:Label>
                </span>
                <asp:DropDownList ID="PrevewDropDownList" runat="server">
                </asp:DropDownList>
                <span class="summary">
                    <input type="checkbox" checked="checked" id="usedata" />
                    <label for="usedata">
                        使用例子数据</label>
                </span><span style="vertical-align: bottom; padding: 0; margin: 0" id="Priview"><a>
                    <img src="images/preview-button.png" border="0" /></a> </span>
                <br />
                <a class="publish" id="btnPublish" title="发布">
                    <img src="images/publish-button.png" border="0" /></a> <a id="changelayout" title="更换模板">
                        <img src="images/theme-button.png" border="0" /></a> <a href="javascript:void(0);"
                            id="pagebglayout">
                            <img src="images/bk-button.png" border="0" /></a> <a href="javascript:customcss();"
                                id="bt_custom_css" title="自定义样式">
                                <img src="images/bk-uxstyle.png" border="0" />
                            </a><a href="javascript:AddHtmlWidget();" target="_self" id="AddHtmlWidget" title="添加静态部件">
                                <img src="images/html-Widget.png" border="0" />
                            </a>
            </div>
        </div>
        <div id="messagelabel" class="publishInfo" style="display: none;">
        </div>
        <div id="LayoutList" class="panelContent">
            <div class="items">
            </div>
            <div class="clear">
            </div>
            <div style="background-color: #DADFE4; color: Green; font-weight: bold;">
                <p id="layoutbar">
                    <img class="submit" src="images/save-button.png" style="margin: 0 10px 0 10px; cursor: pointer;" /><img
                        style="margin: 0 10px 0 10px; cursor: pointer;" src="images/cancel-button.png"
                        class="cancle"></p>
            </div>
        </div>
        <div id="admin_panle_container" style="font-size: 12px; display: none;" class="panelContent">
            <table style="width: 100%; border: none;" cellpadding="5">
                <tr>
                    <td style="width: 350px; min-height: 100px; vertical-align: top">
                        <div id="admin_pagebgcolor">
                            <fieldset style="overflow: hidden; margin-top: 10px;" id="Fieldset1">
                                <legend style="padding: 0pt 10px; margin-left: 25px; font-size: 13px; color: rgb(0, 0, 0);">
                                    页面设置</legend>
                                <div class="bgitem">
                                    <select id="sourcetarget">
                                        <option value="page">网页背景</option>
                                    </select>
                                    <input type="text" id="bgcolor" name="bgcolor" readonly="readonly" />
                                    <span id="btnbgtransparent" style="color: Blue; cursor: pointer;">[透明]</span>
                                </div>
                                <div id="container_setting">
                                    <div>
                                        页面宽度:
                                        <select id="pagecontainwidth">
                                            <option value="900">默认(900PX)</option>
                                            <option value="1200">超宽幅(1200PX)</option>
                                            <option value="1100">超宽幅(1100PX)</option>
                                            <option value="1000">宽幅(1000PX)</option>
                                            <option value="900">宽幅(900PX)</option>
                                            <option value="800">中幅(800PX)</option>
                                            <option value="700">中幅(700PX)</option>
                                            <option value="600">小幅(600PX)</option>
                                        </select>
                                    </div>
                                    <div>
                                        <label for="customersize">
                                            自定义宽度</label>
                                        <input type="checkbox" id="customersize" name="customersize" />
                                        <input type="text" id="cssize" />
                                    </div>
                                    <div>
                                        页面位置:
                                        <select id="containcentersel">
                                            <option value="auto">居中</option>
                                            <option value="0">居左</option>
                                        </select>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </td>
                    <td style="vertical-align: top">
                        <fieldset style="overflow: hidden; margin-top: 10px; width: 350px" id="info">
                            <legend style="padding: 0pt 10px; margin-left: 25px; font-size: 13px; color: rgb(0, 0, 0);">
                                背景图片</legend>
                            <div>
                                <span>背景图片:</span>
                                <input type="text" name="bgimageurl" id="bgimageurl" readonly="readonly" />
                                <span title="清空背景图" id="btnclearbgimage" style="color: Blue; cursor: pointer;">[清空]</span>
                                <a href="javascript:void(0)" title="从服务器选择背景图" id="selectImage">[选择]</a>
                            </div>
                            <div>
                                <span>背景定位:</span>
                                <select id="bgposition">
                                    <option value="left top">左上</option>
                                    <option value="center top">中上</option>
                                    <option value="right top">右上</option>
                                    <option value="left bottom">左下</option>
                                    <option value="center bottom">中下</option>
                                    <option value="right bottom">右下</option>
                                </select>
                            </div>
                            <div>
                                <span>背景重复:</span>
                                <select id="bgrepeat">
                                    <option value="repeat">重复</option>
                                    <option value="no-repeat">不重复</option>
                                    <option value="repeat-x">水平重复</option>
                                    <option value="repeat-y">垂直重复</option>
                                </select>
                            </div>
                            <div class="onlybg">
                                <span>背景滚动:</span>
                                <select id="bgscroll">
                                    <option value="scroll">滚动</option>
                                    <option value="fixed">不滚动</option>
                                </select>
                            </div>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <input type="button" id="pagebgsave" value="确定" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="main">
            <div id="toolwarp" class="window  window_current" style="z-index: 24; left: 537px;
                top: 92px; width: 291px; height: 495px; display: block; position: absolute;"
                _olddisplay="block">
                <div class="window_outer" style="height: 455px;">
                    <div style="z-index: 24;" class="window_inner" id="window_inner_3">
                        <div class="window_bg_container">
                            <div class="window_bg window_center">
                            </div>
                            <div class="window_bg window_t">
                            </div>
                            <div class="window_bg window_rt">
                            </div>
                            <div class="window_bg window_r">
                            </div>
                            <div class="window_bg window_rb">
                            </div>
                            <div class="window_bg window_b">
                            </div>
                            <div class="window_bg window_lb">
                            </div>
                            <div class="window_bg window_l">
                            </div>
                            <div class="window_bg window_lt">
                            </div>
                        </div>
                        <div class="window_content" id="widgetList">
                            <div id="tooboxhandle" class="window_titleBar" id="window_titleBar_3">
                                <%--<a hidefocus="" href="javascript:void(0);" title="关闭" class="window_close" id="window_closeButton_3"
                                    style="display: block;"></a>--%>
                                <a hidefocus="" href="javascript:void(0);" title="最大化" class="window_max" id="window_maxButton_3">
                                </a><a hidefocus="" href="javascript:void(0);" title="还原" class="window_restore"
                                    id="window_restoreButton_3"></a><a hidefocus="" href="javascript:void(0);" id="btnmin"
                                        title="最小化" class="window_min" id="window_minButton_3" style="display: block;">
                                </a><a hidefocus="" href="javascript:void(0);" title="退出全屏" class="window_restore_full"
                                    id="window_restorefullButton_3"></a><a hidefocus="" href="javascript:void(0);" title="全屏"
                                        class="window_fullscreen" id="window_fullButton_3"></a><a hidefocus="" href="javascript:void(0);"
                                            title="刷新" class="window_refresh" id="window_refreshButton_3"></a><a hidefocus=""
                                                href="javascript:void(0);" title="浮动" class="window_pinUp" id="window_pinUpButton_3">
                                            </a><a hidefocus="" href="javascript:void(0);" title="钉住" class="window_pinDown"
                                                id="window_pinDownButton_3"></a>
                                <div class="window_title titleText" id="window_title_3">
                                    We7部件工具箱</div>
                            </div>
                            <div class="window_bodyArea" id="window_body_3" style="width: 291px; height: 476px;">
                                <a id="widgettab" title="部件">
                                    <img src="images/content-button-focus.png" border="0" /></a> <a id="layouttab" title="布局">
                                        <img src="images/layout-button.png" border="0" /></a> <a id="shoptab" title="商城部件下载">
                                            <img src="images/shop-button.png" border="0" /></a>
                                <%--<a href="http://shop.we7.cn/" target="_blank" title="访问We7插件商店获取更多部件" style="float:right;">更多部件</a>--%>
                                <%--<a id="showtabs" title="折叠/打开"><img src="images/hide-tool.png" style=" border:0;" /></a>--%>
                                <div id="toolbody" style="overflow-y: hidden; max-height: 376px;" class="loginArea">
                                    <div id="widgets" style="max-height: 376px; overflow-y: auto;">
                                    </div>
                                    <div id="layout" style="display: none; max-height: 316px; overflow-y: hidden;">
                                        <ul style="list-style: none;">
                                            <li class="layoutitem dragitem litem1" data='{path:"/admin/visualtemplate/layoutcolumn/Column1.ascx"}'>
                                                <span>100%</span> </li>
                                            <li class="layoutitem dragitem litem2" data='{path:"/admin/visualtemplate/layoutcolumn/Column2-1.ascx"}'>
                                                <span>25%+75%</span> </li>
                                            <li class="layoutitem dragitem litem3" data='{path:"/admin/visualtemplate/layoutcolumn/Column2-2.ascx"}'>
                                                <span>75%+25%</span> </li>
                                            <li class="layoutitem dragitem litem4" data='{path:"/admin/visualtemplate/layoutcolumn/Column2-3.ascx"}'>
                                                <span>50%+50%</span> </li>
                                            <li class="layoutitem dragitem litem5" data='{path:"/admin/visualtemplate/layoutcolumn/Column3-1.ascx"}'>
                                                <span>25%+25%+50%</span> </li>
                                            <li class="layoutitem dragitem litem6" data='{path:"/admin/visualtemplate/layoutcolumn/Column3-2.ascx"}'>
                                                <span>33%*3</span> </li>
                                            <li class="layoutitem dragitem litem7" data='{path:"/admin/visualtemplate/layoutcolumn/Column4-1.ascx"}'>
                                                <span>25%*4</span> </li>
                                            <li class="layoutitem dragitem litem8" data='{path:"/admin/visualtemplate/layoutcolumn/Column5-1.ascx"}'>
                                                <span>20%*5</span> </li>
                                        </ul>
                                    </div>
                                    <div id="shopproducts" style="max-height: 376px; overflow-y: auto;">
                                        <%--<ul>
                                            <li height="auto"><a href="/Shop/goods/131cb2ae_389e_4874_9268_ab16ec702286.html?u=moyerock"
                                                class="shopLink">
                                                <img src="http://shop.we7.cn/_data/2011/07/07/Thumbnail/201107071016083511149116052_thumb.jpg" />
                                            </a>
                                            RSS新闻订阅 
                                            <input type="button" value="一键安装" />
                                                <hr />
                                            </li>
                                            <li height="auto"><a href="http://shop.we7.cn/Shop/goods/eba354e2_4093_4c55_acff_dd82ca45a9c0.html?u=moyerock"
                                                class="shopLink">
                                                <img src="http://shop.we7.cn/_data/2011/07/07/Thumbnail/20110707151716692721827094_thumb.jpg" />
                                            </a>
                                            未来三天天气预报
                                                <input type="button" value="一键安装" />
                                                <hr />
                                            </li>
                                        </ul>--%>                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="pageedit" style="width: 100%;">
                <iframe id="pageframe" name="pageframe" frameborder="0" height="100%" width="100%"
                    scrolling="no"></iframe>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#Priview").click(function () {
                var url = $("#" + "<%=PrevewDropDownList.ClientID %>").val();
                if (url == "" || !url || url == "undefined") {
                    alert("请选择一个路径!");
                    return;
                }
                if ($("#usedata").attr("checked") || $("#usedata").attr("checked") == "checked") {
                    url = setUrlParam(url, "virtualdata", "virtualdata");
                }
                window.open(url);
            });

            //绑定事件
            $("#widgettab").click(function () {
                //点击widget按钮,切换到widget面板,同时修改可视化页面的body的class为pageEdit
                $("#layout").hide();
                $("#widgets").show();
                $("#shopproducts").hide();
                $("#layouttab").find("img").attr("src", "images/layout-button.png");
                $("#shoptab").find("img").attr("src", "images/shop-button.png");
                $(this).find("img").attr("src", "images/content-button-focus.png");
                //TODO::
                $("#pageframe").contents().find("body").removeClass("zeLayoutMode ").addClass("zeContentMode");

            });
            $("#layouttab").click(function () {
                //点击layout按钮,切换到layout面板,同时修改可视化页面的body的class为layoutEdit
                $("#widgets").hide();
                $("#layout").show();
                $("#shopproducts").hide();
                $("#widgettab").find("img").attr("src", "images/content-button.png");
                $("#shoptab").find("img").attr("src", "images/shop-button.png");
                $(this).find("img").attr("src", "images/layout-button-focus.png");

                $("#pageframe").contents().find("body").removeClass("zeContentMode ").addClass("zeLayoutMode ");

            });
            $("#shoptab").click(function () {
                $("#layout").hide();
                $("#widgets").hide();
                $("#shopproducts").show();
                $("#widgettab").find("img").attr("src", "images/content-button.png");
                $("#layouttab").find("img").attr("src", "images/layout-button.png");
                $(this).find("img").attr("src", "images/shop-button-focus.png");

                $("#pageframe").contents().find("body").removeClass("zeLayoutMode ").addClass("zeContentMode");
            });
            $("#showtabs").toggle(function () {
                $("#toolbody").hide();
                $(this).find("img").attr("src", "images/show-tool.png");

            }, function () {
                $("#toolbody").show();
                $(this).find("img").attr("src", "images/hide-tool.png");
            });
            According({ el: "#widgets" });
            SetContentUrl();
        });


        $("#pageframe").load(function () {
            resizeContent();
        });

        //         //调用函数
        //        var pagestyle = function() {
        //            var rframe = $("#pageframe");
        //            //ie7默认情况下会有上下滚动条，去掉上下15像素
        //            var h = $(window).height() - rframe.offset().top ;
        //            rframe.height(h);
        //            var w=$(window).width()-rframe.offset().left;
        //            rframe.width(w);
        //        }
        //        //注册加载事件
        //        $("#pageframe").load(pagestyle);
        //        //注册窗体改变大小事件   
        //        $(window).resize(pagestyle);

        //设置文件路径
        function SetContentUrl() {
            var file = Request.parameter("file");
            var folder = Request.parameter("folder");
            var state = Request.parameter("state");
            var virtualdata = Request.parameter("virtualdata");
            if (!file) {
                alert("文件名不正确！");
                return;
            }
            if (!folder) {
                folder = "";
            }
            if (!state) {
                state = "";
            }
            if (!virtualdata) {
                virtualdata = "";
            }
            var url = "/Admin/VisualTemplate/PageEditor.aspx";
            url = setUrlParam(url, "file", file);
            url = setUrlParam(url, "folder", folder);
            url = setUrlParam(url, "virtualdata", virtualdata);
            url = setUrlParam(url, "state", state);
            $("#pageframe").attr("src", url);
        }

        function According(options) {
            var defaults = { header: 'h3' };
            options = $.extend({}, defaults, options);
            $(options.el).find(options.header).live("click", function () {
                var nextNode = $(this).next();
                if (nextNode.css("display") != "none") {
                    nextNode.hide();
                }
                else {
                    nextNode.show();
                }
            });
        }

        //自定义样式页
        function customcss() {
            var url = "/Admin/VisualTemplate/UxStyleEditor/ThemeUxStyleEditor.aspx";
            window.showModalDialog(url, "自定义样式", 'scrollbars=no;resizable=no;help=no;status=no;dialogHeight=480px;dialogwidth=750px;');

        }
        //新建HTML部件页
        function AddHtmlWidget() {
            var url = "/Admin/VisualTemplate/HtmlWidgets/HtmlWidgets.aspx";
            var result = window.showModalDialog(url, "新建HTML部件页", 'scrollbars=no;resizable=no;help=no;status=no;dialogHeight=480px;dialogwidth=750px;');
            if (result != null && result != '' && result != undefined) {
                LoadWidgetList();
            }

        }

        var timerObj = null;

        function finish() {
            $("body").height(getHeight());
            $("#toolwarp").draggable({ handle: "#tooboxhandle", iframeFix: true, containment: 'parent', scroll: false }).disableSelection();
            if (timerObj == undefined)
                timerObj = window.setInterval("finish();", 1000);
        }

        function getHeight() {
            var iframeid = window.parent.document.getElementById("pageframe"); //iframe id
            if (document.getElementById) {
                if (iframeid && !window.opera) {
                    if (iframeid.contentDocument && iframeid.contentDocument.body.offsetHeight) {
                        return iframeid.contentDocument.body.offsetHeight;
                    }
                    else if (iframeid.Document && iframeid.Document.body.scrollHeight) {
                        return iframeid.Document.body.scrollHeight;
                    }
                }
            }
            return 1000;
        }

        $("#pageframe").load(function () { finish() })
       
    </script>
</body>
</html>
