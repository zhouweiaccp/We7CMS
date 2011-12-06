function vbFieldItem() {
	var _type, _title, _name, _max, _min, _required, _value, _desc, _len, _defaultValue,_data;
	this.getType = function() { return _type; }
	this.setType = function(t) { _type = t; }
	this.getName = function() { return _name; }
	this.setName = function(n) { _name = n; }
	this.getTitle = function() { return _title; }
	this.setTitle = function(n) { _title = n; }
	this.getDescription = function() { return _desc; }
	this.setDescription = function(d) { _desc = d; }
	this.setMax = function(m) { _max = m; }
	this.setMin = function(m) { _min = m; }
	this.getMax = function() { return _max; }
	this.getMin = function() { return _min; }
	//debugger
	this.getRequired = function() { return _required; }
	this.setRequired = function(r) { _required = r; }
	this.getValue = function() { return _value; }	
	this.setValue = function(v) { _value = v; }
	this.getLength = function() { return _len; }	
	this.setLength = function(v) { _len = v; }
	this.setDefaultValue = function(v) { _defaultValue = v; }
	this.getDefaultValue = function() { return _defaultValue; }
	this.setData = function(v) { _data = v; }
	this.getData = function() { return _data; }
}

function vbBaseItem() {    
    var me = this;
	var _field = null;
	var _messageDiv = null;
	var _uniqueID = null;
	
	this.createHtmlObject = function(id) {
		throw "Not implement";
	}
	
	this.refresh=function(id){	    
	}
	
	this.setField = function(f) {
		_field = f;
	}
		
	this.getField = function() {
		return _field;
	}
	
	this.setMessageDIV = function(d) {
	    _messageDiv = d;
	}
	
	this.getMessageDIV = function() {
	    return _messageDiv;
	}
	
	this.getValue = function () {
		throw "Not implement";
	}
	
	this.setValue = function(v) {
	    throw "Not implement";
	}
	
	this.getUniqueID = function() {
	    return _uniqueID;
	}
	
	this.setUniqueID = function(v) {
	    _uniqueID = v;
	}
	
	this.showMessage = function(m) {
	    if(this.getMessageDIV() != null) {
	       this.getMessageDIV().innerHTML = m;
	    }
	}
	
	this.validateEx = function() {
	    return true;
	}
	
	this.validate = function() {
//	    if((me.getField().getRequired()) && (me.getValue() == null || me.getValue() == "")) {
//	        //me.showMessage("此参数是必须填写的。");
//	        return false;
//	    }
	    return me.validateEx();
	}
	
	this.requiredValidate = function() {
	    if((me.getField().getRequired()) && (me.getValue() == null || me.getValue() == "")) {
	        return false;
	    }
	    return true;
	}
}

function vbFactory() {
    var me = this;
    var items = new Array();
    var index = 0;
    var _name;
    var _id;
    var _description;
    var _ctr;
    var _dc;

    var counter = 0;
    var creatorNames = new Array();
    var creators = new Array();

    this.addCreator = function(name, func) {       
        creatorNames[counter] = name;
        creators[counter] = func;
        counter++;
    }

    this.addItem = function(it) {
        //debugger;
        it.setUniqueID(it.getField().getName());
        items[index] = it;
        index++;
    }

    this.getItem = function(i) {
        return items[i];
    }

    this.getLength = function() {
        return index;
    }

    this.getID = function() {
        return _id;
    }

    this.setID = function(v) {
        _id = v;
    }

    this.getCtr = function() {
        return _ctr;
    }

    this.setCtr = function(v) {
        _ctr = v;
    }
    this.getDc = function() {
        return _dc;
    }
    this.setDc = function(v) {
        _dc = v;
    }

    this.clear = function() {
        index = 0;
        items = new Array();
        _id = null;
    }
    
    
    this.createItem = function(field) {
        var i;
        var creator;
        var item;
        for (i = 0; i < counter; i++) {           
            if (creatorNames[i] == field.getType()) {//遍历找到控件的创建类型
                creator = creators[i];
                item = eval("new " + creator + "()");//创建对应类型的控件input
                item.setField(field); 
                this.addItem(item);
                return item;
            }
        }
        return null;
    }

    this.createHtmlObject = function(dvContainer) {
        //必填属性table
        tableBase = dvContainer.find("table:first").get(0);
        //高级选项table
        tableAdvance = dvContainer.find("table:last").get(0);
        //遍历当前构造出的对象的内容
        for (var i = 0; i < me.getLength(); i++) {
            //这一步将得到已经创建好的各种类型的控件
            var item = me.getItem(i)
            //创建表格行对象
            var tr;
            //判断是否是必填属性，如果是则创建到必填属性table，否则创建到高级选项table
            if (item.getField().getRequired()) {
                tr = tableBase.insertRow(-1);
            }
            else {
                tr = tableAdvance.insertRow(-1);
            }
            var td1 = tr.insertCell(0);
            var td2 = tr.insertCell(1);
            var div = document.createElement("span");
            item.setMessageDIV(div);
            //当前属性的lable标签(描述)
            td1.title = item.getField().getDescription();
            td1.style.cursor = "pointer";
            td1.align = "left";
            td2.align = "left";
            td1.innerHTML = item.getField().getTitle() + "：";
            td2.appendChild(item.createHtmlObject(item.getField().getName(), _dc));
            td2.appendChild(item.getMessageDIV());
        }
        return dvContainer;
    }
    
    	
	
	this.reloadData = function(data) {
	     var dc;
	     try
	     {
            dc=data;
	        this.loadBaseItems();
	        _name=dc.name;
	        _description=dc.description;
	        _ctr=dc.control;
	        me.setDc(dc);
	        
	        for(var i=0;i<dc.params.length;i++)
	        {
	            var node=dc.params[i];
	            var f = new vbFieldItem();	            
	            f.setName(node.name);
	            f.setType(node.type);
                f.setTitle(node.title);
                f.setDescription(node.description);
                f.setMax(node.maximum);
                f.setMin(node.minimum);
                f.setLength(node.length);
                f.setDefaultValue(node.defaultValue);
                f.setRequired(node.required=="true"||node.required=="True"||node.required=="TRUE");                
                this.getItem(i).setField(f);
	        }
	     }
	     catch(e)
	     {
	        alert(e.message);
	        return;
	     }                
	}
	
	this.refresh=function(data){	
	    this.reloadData(data);   
	    for(var i=0; i<me.getLength(); i++) {
            var item = me.getItem(i);
            if(item.refresh)
            {         
                item.refresh(item.getField().getName(),_dc);
            }
        }
	}

	this.loadData = function(data) {
	    var dc;
	    try {
	        dc = data;
	        this.loadBaseItems();
	        _name = dc.name;
	        _description = dc.description;
	        _ctr = dc.control;
	        me.setDc(dc);
	        for (var i = 0; i < dc.params.length; i++) {
	            var node = dc.params[i];
	            var f = new vbFieldItem();
	            f.setName(node.name);
	            f.setType(node.type);
	            f.setTitle(node.title);
	            f.setDescription(node.description);
	            f.setMax(node.maximum);
	            f.setMin(node.minimum);
	            f.setLength(node.length);
	            f.setDefaultValue(node.defaultValue);
	            f.setData(node.data);
	            f.setRequired(node.required || node.required == "true" || node.required == "True" || node.required == "TRUE");
	            me.createItem(f);
	        }
	    }
	    catch (e) {
	        alert(e.message);
	        return;
	    }
	}
	
	//公共属性
	this.loadBaseItems = function() {
	    //CssFile属性

	}
	
	this.getName = function () {
	    return _name;
	}
	
	this.getDescription = function () {
	    return _description;
	}
	
	//loadDefault;
	this.loadValue = function(obj) {
	    //Step 1: Validate
	    //	    if(obj == null || 
	    //	        obj.attributes["tag"] == null ||
	    //	        obj.attributes["tag"].value != "wec" ||
	    //	        obj.childNodes.length != 2) {
	    //	        return;
	    //	        throw "不合法的控件格式。";
	    //	    }
	    //	    var div1 = obj.childNodes[0];
	    //	    var div2 = obj.childNodes[1];
	    //	    if(div1.attributes["tag"] == null ||
	    //	        div1.attributes["tag"].value.toLowerCase() != me.getCtr().replace('.','_').toLowerCase()) {
	    //	        return;
	    //	        //throw "控件类型不匹配。";
	    //	    }
	    //	    if(div2.attributes["tag"] == null ||
	    //	        div2.attributes["tag"].value != "content" ||
	    //	        div2.childNodes.length == 0) {
	    //	        return;
	    //	        //throw "缺少内容节点。";
	    //	    }
	    var div = obj; // div2.childNodes[0];
	    var tagname = div.tagName.toLowerCase();
	    if (tagname.indexOf(":") > 0) tagname = tagname.substring(tagname.indexOf(":") + 1);
	    if (tagname != me.getCtr().replace('.', '_').toLowerCase()) {
	        //return;
	        throw "控件类型不匹配。";
	    }
	    if (div.attributes["id"] == null) {
	        //return;
	        throw "缺少ID属性。";
	    }
	    _id = div.attributes["id"].value;

	    //Step 2: Retrieve the values
	    for (var i = 0; i < me.getLength(); i++) {
	        var item = me.getItem(i);
	        var name = item.getField().getName().toLowerCase();
	        var at = div.attributes[name];
	        if (at != null) {
	            item.setValue(at.value);
	        }
	    }
	}

	this.loadDC = function(obj) {
	    if (!obj) return;
	    _id = obj.id;
	    for (var i = 0; i < me.getLength(); i++) {
	        var item = me.getItem(i);
	        var name = item.getField().getName();
	        //兼容全小写和原来的
	        var lowerName = name.toLowerCase();
	        var upperName = name.toUpperCase();
	        if (obj[lowerName] != undefined) {
	            item.setValue(obj[lowerName]);
	        }
	        else {
	            item.setValue(item.getField().getDefaultValue());
	        }
	    }
	}
	
	this.loadValue2 = function(obj) {
	      /*
	      // obj example
	      //---------------------------------
            <DIV tag="wec" style="border:solid gray 1px dotted" xmlns:wec=\"http://www.We7.com\">
                <DIV tag="testcontrol">Test1</DIV>
                <DIV tag="content">
                    <?xml:namespace prefix = wec />
                    <wec:testcontrol id=Test1 xmlns:wec="http://We7"
                        runat="server" Name="1" Name2="2">
                    </wec:testcontrol>
                </DIV>
            </DIV>
        //-----------------------------------
        */
	
	    //Step 1: Validate
	    if(obj == null || 
	        obj.attributes["tag"] == null ||
	        obj.attributes["tag"].value != "wec" ||
	        obj.childNodes.length != 2) {
	        return;
//	        throw "不合法的控件格式。";
	    }
	    var div1 = obj.childNodes[0];
	    var div2 = obj.childNodes[1];
	    if(div1.attributes["tag"] == null ||
	        div1.attributes["tag"].value.toLowerCase() != me.getCtr().replace('.','_').toLowerCase()) {
	        return;
	        //throw "控件类型不匹配。";
	    }
	    if(div2.attributes["tag"] == null ||
	        div2.attributes["tag"].value != "content" ||
	        div2.childNodes.length == 0) {
	        return;
	        //throw "缺少内容节点。";
	    }
	    var div = div2.childNodes[0];
	    if(div.tagName.toLowerCase() != me.getCtr().replace('.','_').toLowerCase()) {
	        return;
	        //throw "控件类型不匹配。";
	    }
	    if(div.attributes["id"] == null) {
	        return;
	        //throw "缺少ID属性。";
	    }
	    _id = div.attributes["id"].value;
	    
        for(var i=0; i<me.getLength(); i++) {
             var item = me.getItem(i);
             var name = item.getField().getName().toLowerCase();
             var at = div.attributes[name];
             if(at != null) {
                item.setValue(at.value);
             }
        }
	}	
	
	this.loadDefaultValues = function() {
	    for(var i=0; i<me.getLength(); i++) {
             var item = me.getItem(i);
             var dv = item.getField().getDefaultValue();
             if(dv) {
                item.setValue(dv);
             }
        }
	}

	this.validate = function() {
	   for(var i=0; i<me.getLength(); i++) {
	        var item = me.getItem(i);
	        if(!item.validate()) {
	        	alert(item.getField().getTitle()+" 属性格式不正确！");
	            var txt=document.getElementById(item.getField().getName());
	            if(txt) txt.focus();
	            return false;
	        }
	        if(!item.requiredValidate()) {
	        	alert(item.getField().getTitle()+" 属性不能为空！");
	            var txt=document.getElementById(item.getField().getName());
	            if(txt) txt.focus();
	            return false;
	        }
	    }
	    return true;
	}


	this.getProperties = function() {	   
	    var result = {};
	    if (_id != null) {
	        result.id = _id;
	        var dc=me.getDc();
	        result.fileName = dc.fileName;	    
	    }
	    for (var i = 0; i < me.getLength(); i++) {
	        var item = me.getItem(i);
	        result[item.getField().getName()] = item.getValue();
	    }
	    //debugger
	    return result;
	}
	
	this.getControl = function() {
	    var dc=me.getDc();
	    var txt="",imgstr="", ctr=me.getCtr().replace(".","_").toLowerCase();
	    imgstr='<IMG xmlns:wec="http://www.WestEngine.com" tag="wec" class="wec" controlName="';
	    imgstr+=me.getCtr()+'"  ';
	    
	    txt='<wec:'+ctr;
	    txt+=" id='"+_id+"'";
	    txt+=" runat='server'";
	    for(var i=0; i<me.getLength(); i++) {
	        var item = me.getItem(i);
	        txt+=" "+item.getField().getName().toLowerCase()+"='"+item.getValue()+"'";
	    }
	    txt+=" control='"+me.getCtr()+"' "
	    txt+=" filename='"+dc.fileName+"' "
	    txt+="></wec:"+ctr+">";
	    txt=HtmlEncode(txt);
	    
	    var icon=dc.demoUrl;
        $.ajax({
                url:'../Ajax/CreateControlIcon.ashx',
                async: false,
                data:{id:_id,control:me.getCtr()},
                success:function(result,op)
                {
                    icon=result ;
                },
                error:function(xh,st,op){
                    alert("创建控件图标出错！");
                }
        });
        
	    if(!icon) 
	        icon="/admin/images/s.jpg";
	        
	    imgstr+=' control="'+txt+'" filename="'+dc.fileName+'"  src="'+icon+'"  />';
	    return imgstr;
	}

	
	this.getControl2 = function() {
	    var txt="",ctr=me.getCtr().replace(".","_").toLowerCase();
	    txt='<div contenteditable="false" tag="wec" xmlns:wec="http://www.WestEngine.com" style="border-right: gray 1px dotted; border-top: gray 1px dotted; border-left: gray 1px dotted; width: 100px; border-bottom: gray 1px dotted; height: 50px; background-color: #aaffaa">';
	    txt+='<div tag="'+ctr+'">'+'控件:'+_name+_id+'</div>';
	    txt+='<div tag="content">';
	    txt+='<wec:'+ctr;
	    txt+=" id='"+_id+"'";
	    txt+=" runat='server'";
	    for(var i=0; i<me.getLength(); i++) {
	        var item = me.getItem(i);
	        txt+=" "+item.getField().getName().toLowerCase()+"='"+item.getValue()+"'";
	    }
	    txt+=" control='"+me.getCtr()+"' "
	    txt+="></wec:"+ctr+"></div></div>";
	    return txt;
	}
}

//BUILDER集合
var items = new Array();
function AllvbFactory() {
    this.setI = function(Index, Item) {                               
        items[Index] = Item;
    }
    this.getI = function(i) {    
        return items[i];
    }
}
var AllBUILDER = new AllvbFactory();


function CreateBuilder() {
    return BUILDER;
}

var BUILDER = new vbFactory();
BUILDER.Event = {};
BUILDER.Event.add = function(cmd, fn) {
    if (BUILDER.Event[cmd] == null) {
        BUILDER.Event[cmd] = new Array();
    }
    BUILDER.Event[cmd].push(fn);
}
BUILDER.Event.dispatch = function(cmd) {
    var o = {}
    for (var i = 1; i < arguments.length; i++) {
        o[i - 1] = arguments[i];
    }
    if (BUILDER.Event[cmd] != null) {
        for (var i = 0; i < BUILDER.Event[cmd].length; i++)
            BUILDER.Event[cmd][i](o);
    }
}


