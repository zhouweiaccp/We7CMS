/*
 * Ext JS Library 2.0
 * Copyright(c) 2006-2007, Ext JS, LLC.
 * licensing@extjs.com
 * 栏目树：栏目管理
 * http://extjs.com/license
 */

Ext.onReady(function (rootID) {

    //    Ext.state.Manager.setProvider(
    //            new Ext.state.SessionProvider({state: Ext.appState}));

    // turn on quick tips
    Ext.QuickTips.init();

    // create our layout
    //debugger
    // shorthand
    var Tree = Ext.tree;
    var win;
    var tree = new Tree.TreePanel({
        el: 'tree-div',
        width: 170,
        useArrows: true,
        autoScroll: true,
        animate: true,
        enableDD: true,
        border: true,
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
    tree.on('click', onPopwin);
    tree.on('contextmenu', menuShow);

    var sm = tree.getSelectionModel();


    function expandMe(node) {
        node.toggle();
    }


    //菜单显示
    function menuShow(node) {
        //debugger
        node.select();
        if (node == tree.getRootNode()) {
            var menuR = new Ext.menu.Menu({
                id: 'rootMenu',
                items: [
                {
                    text: '新建一级栏目',
                    handler: onCreateItemClick
                }, '-',
                {
                    text: '刷新',
                    handler: freshTree
                }, '-',
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
                '/admin/ajax/ChannelNodesGet.aspx',
               {
                   success: function (result, request) {
                       var responseArray = result.responseText;
                       content = '<div style="width:150px;height:100px;padding-left:30px;padding-top:10px"><b>栏目信息概览</b><br>' + responseArray +
                       '</div> ';
                       createMenuContent(content, node)
                   },
                   failure: function (result, request) {
                       var responseArray = result.responseText;
                       alert(responseArray);
                   }
               },
                'type=detail&node=' + node.id
            );

        }
    }

    //右菜单显示内容
    function createMenuContent(content, node) {

        var optionMenu = new Ext.menu.Menu({
            id: 'optionMenu', // the menu's id we use later to assign as submenu
            items: [content, '-',
            {
                text: '<u>编辑栏目信息</u>',
                xtype: node.text,
                handler: onPopwin
            }]
        });

        var menuC = new Ext.menu.Menu({
            id: 'mainMenu',
            items: [
            {
                icon: '/admin/ajax/Ext2.0/resources/images/logo_info.gif',
                text: '概要 >>',
                iconCls: 'calendar',
                menu: optionMenu // <-- submenu by reference
            },
            {
                text: '栏目属性',
                icon: '/admin/ajax/Ext2.0/resources/images/list-items.gif',
                xtype: node.text,
                handler: onPopwin
            },
            {
                text: '栏目标签',
                xtype: node.text,
                icon: '/admin/ajax/Ext2.0/resources/images/users.gif',
                handler: onPopwin
            },
            {
                text: '栏目权限',
                xtype: node.text,
                icon: '/admin/ajax/Ext2.0/resources/images/icon_key.gif',
                handler: onPopwin
            }, '-',
            {
                text: '新建子栏目',
                xtype: node.text,
                handler: onCreateItemClick
            },
            {
                text: '删除栏目',
                xtype: node.text,
                handler: onDelChannelClick
            }, '-',
            {
                text: '刷新',
                xtype: node.text,
                handler: freshTree
            }, '-',
            {
                text: '栏目文章',
                xtype: node.text,
                handler: onOpenArticlesClick
            }
            ]
        });
        menuC.show(node.ui.getAnchor());
    }


    function expandTree() {
        tree.expandAll();
    }

    function collapseTree() {
        tree.collapseAll();
        root.expand();
    }

    //点击新建栏目 
    function onCreateItemClick(item) {
        var isDemoSite = document.getElementById("hdDemoSite").value;
//        if (isDemoSite === "true") {
//            alert("对不起，此演示站点您没有该操作权限！");
//            return;
//        }
        // create the window on the first click and reuse on subsequent clicks
        var n = sm.getSelectedNode();
        var id = n.id;
        if (id == 'root') id = '';

        var tabContainer = document.getElementById('rightTabs');

        window.ifRightDetail.location.href = "ChannelEdit.aspx?pid=" + formatToGUID(id);
    }

    //点击栏目文章
    function onOpenArticlesClick(item) {
        var n = sm.getSelectedNode();
        var url = "addins/Articlelist.aspx";
        if (QueryString('wap') == '1')
            url = url + "?wap=1";
        else
            url = url + "?web=1";
        window.ifRightDetail.location.href = url + "&oid=" + formatToGUID(n.id);
    }

    //点击栏目文件夹
    function onOpenFolderClick(item) {
        var n = sm.getSelectedNode();
        var url = "folder.aspx";
        window.ifRightDetail.location.href = url + "?channelID=" + formatToGUID(n.id);
    }


    //删除栏目
    function onDelChannelClick(item) {
        var isDemoSite = document.getElementById("hdDemoSite").value;
        if (isDemoSite === "true") {
            alert("对不起，演示站点禁止删除栏目！");
            return;
        }
        var n = sm.getSelectedNode();
        if (confirm('您确认要把栏目“' + n.text + '” 及栏目下的文章全部删除吗？此操作不可恢复，请确认！')) {
            Ext.MessageBox.show({
                msg: '发送请求到服务器，请稍候..',
                progressText: '删除...',
                width: 300,
                wait: true,
                waitConfig: { interval: 200 },
                icon: 'ext-mb-download', //custom class in msg-box.html
                animEl: 'mb7'
            });

            Ext.lib.Ajax.request(
                'POST',
                '/admin/ajax/ChannelTreeDel.aspx',
               {
                   success: function (result, request) {
                       var responseArray = result.responseText;
                       if (responseArray == '0')
                           Ext.MessageBox.hide();
                       else {
                           alert(responseArray);
                           Ext.MessageBox.hide();
                       }

                   },
                   failure: function (result, request) {
                       var responseArray = result.responseText;
                       alert(responseArray);
                   }
               },
                'node=' + n.id
            );
            n.remove();
        }

    }

    //弹出窗口：栏目编辑
    function onPopwin(item) {
        var isDemoSite = document.getElementById("hdDemoSite").value;
//        if (isDemoSite === "true") {
//            alert("对不起，此演示站点您没有该操作权限！");
//            return;
//        }
        // tabs for the center
        var index = 0;
        if (item.text == '栏目属性') index = 1;
        if (item.text == '栏目标签') index = 4;
        if (item.text == '栏目权限') index = 5;

        var n = sm.getSelectedNode();

        var tabContainer = document.getElementById('rightTabs');
        //        if(tabContainer) tabContainer.innerHTML="";

        var id = n.id;
        if (id == 'root' || id == '00000000_1111_0000_0000_000000000000') return; //顶级栏目

        //         debugger;

        window.ifRightDetail.location.href = "ChannelEdit.aspx?id=" + formatToGUID(id) + "&tab=" + index;
    }

    //按钮事件：刷新树节点
    Ext.get('freshNodeTreeButton').on('click', function () {
        //       freshParentNode(); 
        freshNode();
    });

    Ext.get('freshNodeTextButton').on('click', function () {
        var newText = document.getElementById("newNodeText");
        if (newText)
            freshNodeText(newText.value);
    });

    function freshNodeText(newTitle) {
        var node = sm.getSelectedNode();
        if (node)
            node.setText(newTitle);
    }

    function freshTree() {
        //        debugger;
        var node = sm.getSelectedNode();

        if (node.attributes.children != false && node != tree.getRootNode())
            node = sm.getSelectedNode().parentNode;
        node.attributes.children = false;
        node.reload();
    }

    function freshNode() {
        //        debugger;
        var node = sm.getSelectedNode();
        node.expand();

        var newText = document.getElementById("newNodeText").value;
        var newID = document.getElementById("newNodeID").value;

        var nodeSon = new Tree.TreeNode({ id: newID, text: newText, leaf: true });
        node.appendChild(nodeSon);

        ////        node.attributes.children = false; 
        ////        node.reload();

        nodeSon.select();
    }

    function freshParentNode() {
        //        debugger;
        var node = sm.getSelectedNode();
        if (node) {
            if (node != tree.getRootNode())
                node = node.parentNode;
            node.attributes.children = false;
            node.reload();
        }
    }

    // some functions to determine whether is not the drop is allowed
    function hasNode(t, n) {
        return (t.attributes.type == 'fileCt' && t.findChild('id', n.id)) ||
            (t.leaf === true && t.parentNode.findChild('id', n.id));
    };

    function isSourceCopy(e, n) {
        var a = e.target.attributes;
        return n.getOwnerTree() == tree && !hasNode(e.target, n) &&
           ((e.point == 'append' && a.type == 'fileCt') || a.leaf === true);
    };

    function isReorder(e, n) {
        return n.parentNode == e.target.parentNode && e.point != 'append';
    };

    // handle drag over and drag drop
    tree.on('nodedragover', function (e) {
        var n = e.dropNode;
        var a = e.target.attributes;
        //        return isSourceCopy(e, n) || isReorder(e, n);
        //        return a.leaf === true;
    });

    tree.on('beforenodedrop', function (e) {
        var n = e.dropNode;
        if (!(e.dropNode.parentNode == e.target.parentNode && e.point != 'append')) {
            return confirm('您确认要把栏目“' + n.text + '” 移动到 “' + e.target.text + "” 下吗？");         //关键点在于回调函数    
        }
    });

    tree.on('beforemovenode', function (tree, node, oldParent, newParent, index) {
        return saveTree(node.id, newParent.id, index);
    });

    function showResult(btn) {
        Ext.example.msg('Button Click', 'You clicked the {0} button', btn);                         //这里有一个{0}看起来可以在这里种使用模板
    };

    //数据回传到服务器
    function saveTree(node, newParent, index) {

        var action = true;
        Ext.MessageBox.show({
            msg: '保存数据到服务器，请稍候..',
            progressText: '保存...',
            width: 300,
            wait: true,
            waitConfig: { interval: 200 },
            icon: 'ext-mb-download', //custom class in msg-box.html
            animEl: 'mb7'
        });


        Ext.lib.Ajax.request(
            'POST',
            '/admin/ajax/ChannelDataSave.aspx',
            {
                success: function (result, request) {
                    var responseArray = result.responseText;
                    if (responseArray == '0') {
                        Ext.MessageBox.hide();
                    }
                    else {
                        Ext.MessageBox.hide();
                        //                       Ext.Msg.show({
                        //                           title:'错误',
                        //                           msg: responseArray,
                        //                           buttons: Ext.Msg.OK,
                        //                           animEl: 'elId',
                        //                           icon: Ext.MessageBox.ERROR
                        //                        });
                        alert(responseArray);
                    }

                },
                failure: function (result, request) {
                    var responseArray = result.responseText;
                    alert(responseArray);
                }
            },
            'node=' + node + '&newParent=' + newParent + '&index=' + index
        );

        return action;
    }


    function formatToGUID(id) {
        if (id == '') return id;
        var GUIDId = id.replace('_', '-').replace('_', '-').replace('_', '-').replace('_', '-');
        if (GUIDId.substr(0, 1) != "{") GUIDId = "{" + GUIDId + "}";
        return GUIDId;
    }

});

