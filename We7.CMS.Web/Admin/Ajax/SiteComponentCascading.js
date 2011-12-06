
Ext.onReady(function(){
    var ifCreating= document.getElementById("IfCreatingTextBox");
    var IfFinished= document.getElementById("IfFinishedTextBox");
    if(ifCreating.value=="0" || IfFinished.value=="1")
    {return;}

    Ext.QuickTips.init();
    
    var siteReader = new Ext.data.ArrayReader({}, [
       {name: 'ID'},
       {name: 'Name'},
       {name: 'Description'}
    ]);

    var componentReader = new Ext.data.ArrayReader({}, [
       {name: 'ID'},
       {name: 'Name'},
       {name: 'Description'},
       {name: 'Version'},
       {name: 'Publisher'}
    ]);

    var siteSM=new Ext.grid.RowSelectionModel({singleSelect: true});
    var siteGrid = new Ext.grid.GridPanel({
        store: new Ext.data.Store({
            reader: siteReader,
            data: mySiteData
        }),
        cm: new Ext.grid.ColumnModel([
            new Ext.grid.RowNumberer(),
            {id:'ID',header: "站点ID", width: 160, sortable: true, dataIndex: 'ID', hidden:true},
            {header: "站点名称", width: 80, sortable: true, dataIndex: 'Name'},
            {header: "站点描述", width: 120, sortable: false, dataIndex: 'Description'}
        ]),
		sm: siteSM,
        viewConfig: {
            forceFit:true
        },
        width:600,
        height:200,
        title:'目前已有站点列表',
        iconCls:'icon-grid',
        renderTo: 'SitesGrid'
    });
   
    siteGrid.getSelectionModel().on('rowselect', function(sm, rowIdx, r) {
	    var store=new Ext.data.Store({
                reader: componentReader
            });
        
        var componentSM = new Ext.grid.CheckboxSelectionModel();
                
        var cm=new Ext.grid.ColumnModel([
                componentSM,
                {id:'ID',header: "站点ID", width: 40, sortable: false, dataIndex: 'ID', hidden:true},
                {header: "组件名称", width: 60, sortable: false, dataIndex: 'Name'},
                {header: "组件版本", width: 60, sortable: false, dataIndex: 'Version'},
                {header: "组件描述", width: 260, sortable: false, dataIndex: 'Description'},
                {header: "发布者", width: 120, sortable: false, dataIndex: 'Publisher'}
            ]);

        var componentGrid = new Ext.grid.GridPanel({
            store: store,
            cm: cm,
            sm: componentSM,
            width:600,
            height:200,
            frame:true,
            title:'已选站点 {'+r.data.company+'} 的组件列表',
            iconCls:'icon-grid'
        });  

        store.loadData(myComponentData);
        store.filter('ID',r.data.ID);
        var getDiv=document.getElementById("ComponentsGrid");
        getDiv.innerText=""; 
        var getLabel=document.getElementById("SelectSiteIDTextBox");
        getLabel.innerText=r.data.ID; 
        var selectComponent=document.getElementById("SelectComponentText");
        selectComponent.setAttribute("value","");
//        debugger;
        componentGrid.getSelectionModel().on('rowselect', function(smC, rowIdxC, rC){
            var temp=selectComponent.value;
            if(temp!="")
            {selectComponent.setAttribute("value",temp+"|"+rC.data.Name + ":" + rC.data.Version);}
            else
            {selectComponent.setAttribute("value",rC.data.Name + ":" + rC.data.Version);}
        });  
         
        componentGrid.render('ComponentsGrid');
        componentGrid.getSelectionModel().selectRange(0,1); 
        componentGrid.getSelectionModel().lock(); 
    }); 

    siteSM.selectFirstRow();
});
