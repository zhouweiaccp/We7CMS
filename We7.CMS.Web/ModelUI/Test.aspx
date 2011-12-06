<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="We7.Model.UI.Test" %>

<%@ Register Src="ModelUI/Container/we7/ArticleEditor.ascx" TagName="ArticleEidtor"
    TagPrefix="We7" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    {name:'评论- 列表',desc:'评论-列表',dir:'CommentsList',items:[{name:'评论添加列表一体_DIV布局POST提交
    ',description:'',author:'',version:'1.0',created:'2009-12-03 00:00:00',demoUrl:'Admin/images
    /s.jpg',control:'CommentsList.Simple',ctrDir:'CommentsList',remark:'',type:'',fileName:'
    /Widgets/CommentsList/Page/CommentsList.Simple.ascx',parameters: [{name:'Wrapper',title:'外观',description:'设置当前部件的外围样式
    ',type:'KeyValueSelector',required:false,maximum:0,minium:0,length:0,data:'vswrapper',defaultValue:''},
    {name:'HeadTitle',title:'头部名称',description:'模板名称 ',type:'String',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'NavigationUrl',title:'导航地址',description:'导航地址 ',type:'String',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'BindColumnID',title:'指定栏目',description:'获取指定栏目的ID。用于页面导向。 ',type:'Channel',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'ISValidate',title:'是否生成验证码',description:'使用验证码 ',type:'Boolean',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'ISSiginSelect',title:'是否必须登录',description:'默认不需要登录 ',type:'Boolean',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'AllowPager',title:'允许翻页',description:'超出单页评论数，显示翻页链接 ',type:'Boolean',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'PageSize',title:'每页评论数',description:'每页评论数目。必须是整数。 ',type:'Number',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:'10'},
    {name:'DateFormat',title:'日期显示格式',description:'例子显示为“日期：2007-10-20”，不填为类似“15分钟前”格式
    ',type:'String',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:'yyyy-MM-dd'}],files:[],styles:[]}]}
    
    {path:'/Widgets/ArticlePagedList/Page/ArticlePagedList.Pager.ascx', parts:[ {name:'基本信息',
    params:[{name:'Wrapper',title:'外观',description:'定义部件最外围样式',type:'KeyValueSelector',required:true,maximum:0,minium:0,length:0,data:'vswrapper',defaultValue:''},
    {name:'PagerPath',title:'控件路径',description:'',type:'String',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'NavigationUrl',title:'',description:'',type:'',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'HeadTitle',title:'',description:'',type:'',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''}]},
    {name:'分页控件', params:[]}, {name:'文章列表', params:[{name:'ArticleList-MaxTitleLength',title:'标题最大长度',description:'',type:'Number',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'ArticleList-MaxDescLength',title:'简介最大长度',description:'',type:'Number',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'ArticleList-ImageTag',title:'图片标签',description:'',type:'String',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'ArticleList-OrderStr',title:'排序字段',description:'',type:'Order',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'ArticleList-OwnerID',title:'栏目ID',description:'栏目ID',type:'Channel',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'ArticleList-IncludeChildren',title:'是否包含子栏目',description:'',type:'Boolen',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
    {name:'ArticleList-MustContainsImage',title:'必须含有图片',description:'',type:'Boolen',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''}]}]}
    
     {path:'/Widgets/ArticlePagedList/Page/ArticlePagedList.Pager.ascx',
      parts:[
      {name:'基本信息',
       params:[{name:'NavigationUrl',title:'',description:'',type:'',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'HeadTitle',title:'',description:'',type:'',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'PagerPath',title:'控件路径',description:'',type:'String',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'Wrapper',title:'外观',description:'部件最外围样式',type:'KeyValueSelector',required:true,maximum:0,minium:0,length:0,data:'vswrapper',defaultValue:''}]},
      {name:'文章列表',
       params:[{name:'ArticleList-MaxDescLength',title:'简介最大长度',description:'',type:'Number',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-MaxTitleLength',title:'标题最大长度',description:'',type:'Number',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-Tag',title:'标签',description:'',type:'',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-PageSize',title:'页数',description:'',type:'PageSize',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-DisablePager',title:'禁止分页',description:'',type:'Boolean',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-ImageTag',title:'图片标签',description:'',type:'String',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-IsTop',title:'置顶信息',description:'',type:'Boolean',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-IncludeChildren',title:'是否包含子栏目',description:'',type:'Boolen',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-OwnerID',title:'栏目ID',description:'栏目ID',type:'Channel',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-DisableColumn',title:'禁止栏目查询',description:'',type:'Boolean',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-DateFormat',title:'日期格式',description:'',type:'String',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-OrderStr',title:'排序字段',description:'',type:'Order',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''},
               {name:'ArticleList-MustContainsImage',title:'必须含有图片',description:'',type:'Boolen',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''}]}]}
                        
                        
   {path:'/Widgets/Login/Page/Login.Ex.ascx',parts:[
        {name:'基本信息',
            params:[{name:'NavigationUrl',title:'导航地址',description:'定义用于包装器的导航地址',type:'String',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''},
                    {name:'HeadTitle',title:'头部标题',description:'定义用于包装器的头部标题',type:'String',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''},
                    {name:'Wrapper',title:'外观',description:'定义部件最外围样式',type:'KeyValueSelector',required:true,maximum:0,minium:0,length:0,data:'vswrapper',defaultValue:''}]}]}
                    
                    
                    
                    {path:'/Widgets /ArticleList/Page/ArticleList.Image.ascx',parts:[{name:'基本信息',params: [{name:'Wrapper',title:'外观',description:'定义部件最外围样式 ',type:'KeyValueSelector',required:true,maximum:0,minium:0,length:0,data:'vswrapper',defaultValue:''}, {name:'NavigationUrl',title:'导航地址',description:'定义用于包装器的导航地址 ',type:'String',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'HeadTitle',title:'头部标题',description:'定义用于包装器的头部标题 ',type:'String',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''}]}, {name:'文章列表',params:[{name:'ArticleList-MaxTitleLength',title:'标题最大长度 ',description:'',type:'Number',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-MaxDescLength',title:'简介最大长度 ',description:'',type:'Number',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-ImageTag',title:'图片标签 ',description:'',type:'KeyValueSelector',required:false,maximum:0,minium:0,length:0,data:'thumbnail',defaultValue:''}, {name:'ArticleList-DisableColumn',title:'禁止栏目查询 ',description:'',type:'Boolean',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-OwnerID',title:'栏目ID',description:'栏目 ID',type:'Channel',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-IncludeChildren',title:'包含子栏目 ',description:'',type:'Boolean',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-OrderStr',title:'排序字段 ',description:'',type:'OrderFields',required:true,maximum:0,minium:0,length:0,data:'Title| 标题,Index|顺序号,Created|创建日期,Updated|更新时间',defaultValue:''}, {name:'ArticleList-PageSize',title:'页数 ',description:'',type:'Number',required:true,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-DisablePager',title:'禁止分页 ',description:'',type:'Boolean',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-Days',title:'时间段',description:'按日计算 ',type:'Number',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-DateFormat',title:'日期格式 ',description:'',type:'String',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-MustContainsImage',title:'必须含有图片 ',description:'',type:'Boolean',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-IsTop',title:'置顶信息 ',description:'',type:'Boolean',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-IsKeyword',title:'关键字查询 ',description:'',type:'Boolean',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''}, {name:'ArticleList-Tag',title:'标签 ',description:'',type:'Tags',required:false,maximum:0,minium:0,length:0,data:'',defaultValue:''}]}]}
    
    <asp:RadioButtonList RepeatColumns="5" RepeatDirection="Horizontal"
    </form>
    <p>
        &nbsp;</p>
</body>
</html>
