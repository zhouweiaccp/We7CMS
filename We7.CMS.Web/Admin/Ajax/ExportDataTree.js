/*
 * Ext JS Library 2.0
 * Copyright(c) 2006-2007, Ext JS, LLC.
 * 
 * 栏目树：月统计
 * http://extjs.com/license
 */

Ext.onReady(function(rootID) {
    // turn on quick tips
    Ext.QuickTips.init();

    var Tree = Ext.tree;
    var win;

    var tree = new Tree.TreePanel({
        el: 'tree-div',
        width: 170,
        autoScroll: true,
        animate: true,
        enableDD: false,
        containerScroll: true,
        loader: new Tree.TreeLoader({
            dataUrl: '/admin/ajax/ChannelNodesGet.aspx'
        })
    });

    var rootID = 'root';
    if (QueryString('wap') == '1') {
        rootID = '00000000_1111_0000_0000_000000000000';
        ThisSiteName = ThisSiteName + "wap站";
    }

    // set the root node
    var root = new Tree.AsyncTreeNode({
        text: ThisSiteName,
        draggable: false,
        cls: 'root',
        icon: '/admin/ajax/Ext2.0/resources/images/site.gif',
        id: rootID
    });
    tree.setRootNode(root);

    // render the tree
    tree.render();
    root.expand();

    //Add the context menu code
    tree.on('click', onOpenArticlesClick);
    tree.on('contextmenu', menuShow);

    var sm = tree.getSelectionModel();


    //菜单显示 
    function menuShow(node) {
        node.select();
        if (node == tree.getRootNode()) {
            var menuR = new Ext.menu.Menu({
                id: 'rootMenu',
                items: [
               {
                   text: '子栏目全部展开',
                   handler: expandTree
               },
                {
                    text: '子栏目全部折叠',
                    handler: collapseTree
                }
                ]
            });
            menuR.show(node.ui.getAnchor());
        }
        else {
            var content = '<div style="width:300px;height:250px;padding-left:30px;padding-top:10px">Choose a Theme</div> ';
            Ext.lib.Ajax.request(
                'POST',
                '../ajax/ChannelNodesGet.aspx',
               {
                   success: function(result, request) {
                       var responseArray = result.responseText;
                       content = '<div style="width:250px;height:180px;padding-left:30px;padding-top:10px"><b>栏目信息概览</b><br>' + responseArray +
                       '</div> ';
                       createMenuContent(content, node)
                   },
                   failure: function(result, request) {
                       var responseArray = result.responseText;
                       alert(responseArray);
                   }
               },
                'type=detail&node=' + node.id
            );

        }
    }

    function expandMe(node) {
        node.toggle();
    }

    //点击栏目文章
    function onOpenArticlesClick(item) {
        var n = sm.getSelectedNode();
        var url = "/admin/manage/ExportDataHandler.aspx";
        if (QueryString('wap') == '1')
            url = url + "?wap=1";
        else
            url = url + "?web=1";
        if (n == tree.getRootNode()) {
            document.getElementById("ifRightDetail").src= url;
        }
        else {
            document.getElementById("ifRightDetail").src = url + "&oid=" + formatToGUID(n.id);
        }
    }

    function expandTree() {
        tree.expandAll();
    }

    function collapseTree() {
        tree.collapseAll();
        root.expand();
    }

    function formatToGUID(id) {
        if (id == '') return id;
        var GUIDId = id.replace('_', '-').replace('_', '-').replace('_', '-').replace('_', '-');
        if (GUIDId.substr(0, 1) != "{") GUIDId = "{" + GUIDId + "}";
        return GUIDId;
    }

});
