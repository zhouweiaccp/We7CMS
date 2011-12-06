
function Controller() {    
}    
   
Controller.prototype.init = function()    
{    
    //创建用户树组件    
//    this.createUserTree();    
}    
   
//创建用户树组件    
Controller.prototype.createUserTree = function()    
{    
//    ServiceProvider.getUsers(this.handleUsers);    
}    
   

   
/**===============UI事件处理函数=======================*/   
Controller.prototype.onSelectUser = function(node, event)    
{    
    alert(node.id);    
}    

function showTree()
{

    var controller = new Controller();    
    Ext.onReady(controller.init, controller);    
    var div_tree=document.getElementById("tree-div");
    alert(div_tree);
    if(div_tree)
        div_tree.display="block";
}

Ext.BLANK_IMAGE_URL = '../ajax/Ext2.0/resources/images/default/s.gif';

