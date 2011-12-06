/*
 * Ext JS Library 2.0
 * Copyright(c) 2006-2007, Ext JS, LLC.
 * 
 * 栏目树：文章管理
 * http://extjs.com/license
 */

Ext.onReady(function(rootID){
 // turn on quick tips
    Ext.QuickTips.init();

    var Tree = Ext.tree;
    var win;
     
    var tree = new Tree.TreePanel({
        el: 'tree-div',
        width: 170,
        autoScroll:true,
        animate:true,
        enableDD:false,
        containerScroll: true, 
        loader: new Tree.TreeLoader({
            dataUrl:'../ajax/ChannelNodesGet.aspx'
        })
    });

    var rootID='root';
    if(QueryString('wap')=='1') 
   {
        rootID='00000000_1111_0000_0000_000000000000';
        ThisSiteName=ThisSiteName+"wap站";
   } 

    // set the root node
    var root = new Tree.AsyncTreeNode({
        text: ThisSiteName,
        draggable:false,
        cls : 'root',
        icon: '../ajax/Ext2.0/resources/images/site.gif', 
        id:rootID
    });
    tree.setRootNode(root);

    // render the tree
    tree.render();
    root.expand();

     //Add the context menu code 
    tree.on('click', onOpenArticlesClick);
    tree.on('contextmenu', menuShow); 

   var sm = tree.getSelectionModel();   
   

    function expandMe(node) {
        node.toggle();
    }
    
        
    //菜单显示 
    function menuShow(node){
        node.select();
        if(node==tree.getRootNode())
        {
             var menuR =  new Ext.menu.Menu({
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
        else
        {
            var content='<div style="width:300px;height:250px;padding-left:30px;padding-top:10px">Choose a Theme</div> ';
            Ext.lib.Ajax.request(
                'POST',
                '../ajax/ChannelNodesGet.aspx',
               {
                   success: function ( result, request) {
                        var responseArray = result.responseText;
                       content='<div style="width:250px;height:180px;padding-left:30px;padding-top:10px"><b>栏目信息概览</b><br>'+responseArray+
                       '</div> '; 
                       createMenuContent(content,node)
                    },
                    failure: function ( result, request) {
                        var responseArray = result.responseText;
                        alert( responseArray); 
                    }
               },
                'type=detail&node='+node.id
            );
                        
        }
    }
   
    //右菜单显示内容
    function createMenuContent(content,node){

        var menuC =  new Ext.menu.Menu({
        id: 'mainMenu',
        items: [
            {
                text: '发布新文章',
                icon: '../ajax/Ext2.0/resources/images/Article.gif',  
                xtype :node.text,
                tag:node,
                handler: onCreateItemClick
            },
            {
                text: '栏目文章',
                icon: '../ajax/Ext2.0/resources/images/list-items.gif',  
                xtype :node.text,
                handler: onOpenArticlesClick
            },'-',
            {
                text: '栏目设置',
                icon: '../ajax/Ext2.0/resources/images/Cosmetic.gif', 
                xtype :node.text,
                handler: onPopwin
            }
            ]
          });
        menuC.show(node.ui.getAnchor());
   } 
   
    function expandTree()
   {
        tree.expandAll();
   }
    
   function collapseTree()
   {
        tree.collapseAll();
        root.expand();
   }
    
    //点击新建文章 
    function onCreateItemClick(item){
        // create the window on the first click and reuse on subsequent clicks
        var n = sm.getSelectedNode();
        var id=n.id;
        if(id=='root') id='';
        var url="../addins/ArticleEdit.aspx";
         if(QueryString('wap')=='1')  
                url=url+"?wap=1";
          else
               url=url+"?web=1";   
        var tabContainer=document.getElementById('rightTabs');
        if(window!=parent)
            window.parent.location.href=url+"&oid="+formatToGUID(id); 
        else
            window.location.href=url+"&oid="+formatToGUID(id); 
     } 
     
  //点击栏目文件夹
   function onOpenFolderClick(item){
        var n = sm.getSelectedNode();
         var url="../folder.aspx";
        window.ifRightDetail.location.href=url+"?iframe=1&channelID="+formatToGUID(n.id); 
   }
   
         //弹出窗口：栏目编辑
    function onPopwin(item){
     // tabs for the center
        var index=1;
        if(item.text=='栏目属性') index=1;
        if(item.text=='栏目标签') index=4;
        if(item.text=='栏目权限') index=5;
                
        var n = sm.getSelectedNode();
         
        var tabContainer=document.getElementById('rightTabs');
      
         var id=n.id;
         if(id=='root' || id=='00000000_1111_0000_0000_000000000000') return;//顶级栏目
         
         window.ifRightDetail.location.href="../ChannelEdit.aspx?id="+formatToGUID(id) +"&tab="+index; 
      } 
      
   //点击栏目文章
   function onOpenArticlesClick(item){

        var n = sm.getSelectedNode();
        var url="../addins/Articlelist.aspx";
         if(QueryString('wap')=='1')  
                url=url+"?wap=1&type=0";
          else
              url = url + "?web=1&type=0";   
               
//         var navChannelSpan=document.getElementById("navChannelSpan");
         if(n==tree.getRootNode())
         {
                window.ifRightDetail.location.href=url;
//                navChannelSpan.innerHTML=""; 
         } 
         else 
         {
                 window.ifRightDetail.location.href=url+"&oid="+formatToGUID(n.id); 
//                 navChannelSpan.innerHTML="当前栏目："+ getChannelFullPath("",n);
          }
   }
           
  function getChannelFullPath(path,node)
  {
        var fullPath=node.text;
        if(path!="") fullPath= fullPath + " >  " + path;
        if(node.parentNode == tree.getRootNode())
        {
            return fullPath;
        }
        else
        {
            return getChannelFullPath(fullPath,node.parentNode);
         }
  }
  
  function formatToGUID(id)
   {
        if(id=='') return id;
        var GUIDId=id.replace('_','-').replace('_','-').replace('_','-').replace('_','-');
        if(GUIDId.substr(0,1)!="{") GUIDId="{"+GUIDId+"}"; 
        return  GUIDId;
   }  
     
});
