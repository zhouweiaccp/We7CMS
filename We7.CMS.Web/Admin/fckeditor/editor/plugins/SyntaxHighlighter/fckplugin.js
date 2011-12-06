/*
 * 把SyntaxHighlighter做为FCKeditor的语法显示插件
 * Author : Garfeild < garfield0601@gmail.com >
 * since  : 2007-10-19
 * Modified by Metsky.com[2010-05-26]
*/

// Register the related commands.
//FCKCommands.RegisterCommand( 'SyntaxHighlighter', new FCKDialogCommand("SyntaxHighlighter",FCKLang.DlgSyntaxHighlighterTitle,FCKConfig.Plugins.Items['SyntaxHighlighter'].Path + 'index.html', 540, 540 ) ) ;
FCKCommands.RegisterCommand( 'SyntaxHighlighter', new FCKDialogCommand("SyntaxHighlighter",FCKLang['DlgSyntaxHighLighterTitle'],FCKConfig.PluginsPath + 'SyntaxHighlighter/index.html', 600, 540 ) ) ;

// Create the "SyntaxHighlighter" toolbar button.
var oHighLighterItem		= new FCKToolbarButton( 'SyntaxHighlighter', FCKLang['DlgSyntaxHighLighterTitle'] ) ;
oHighLighterItem.IconPath	= FCKConfig.PluginsPath + 'SyntaxHighlighter/logo.gif' ;

FCKToolbarItems.RegisterItem( 'SyntaxHighlighter', oHighLighterItem );// 'SyntaxHighlighter' is the name used in the Toolbar config.

var FCKHighLighter = new Object();
var CSS_PATH 	  = FCKConfig.BasePath + "css/";//css路径

var usingTag = "div";
var usingFlag = "SyntaxHighlighter";

FCKHighLighter.Add = function( value ){
	FCKSelection.Delete();
	var oDiv  = FCK.CreateElement(usingTag);
	oDiv.className = usingFlag;
	oDiv.innerHTML = value;
}

// 双击事件处理代码
FCKHighLighter.OnDoubleClick = function( div ){
	var oDiv = div;
	var vTag=usingTag.toUpperCase();

	// 循环的作用看一下代码就知道了，是为了选择高亮代码的最顶层元素
	while (oDiv.parentNode){
		if (oDiv.tagName == vTag && oDiv.className == usingFlag)
			break;
		oDiv = oDiv.parentNode;
	}

	if(oDiv.tagName == vTag && oDiv.className == usingFlag) {
		FCKSelection.SelectNode( oDiv ) ;
		FCKCommands.GetCommand( 'SyntaxHighlighter' ).Execute() ;
	}
}

// 添加双击事件
FCK.RegisterDoubleClickHandler( FCKHighLighter.OnDoubleClick, usingTag ) ;		// 双击灰色栏

// 单击事件处理代码
FCKHighLighter._ClickListener = function( e )
{
	var oDiv = e.target;

	// 循环的作用看一下代码就知道了，是为了选择高亮代码的最顶层元素
	while (oDiv.parentNode){
		if (oDiv.tagName == usingTag && oDiv.className == usingFlag)
			break;
		oDiv = oDiv.parentNode;
	}

	if ( oDiv.tagName == usingTag && oDiv.className == usingFlag )
		FCKSelection.SelectNode( oDiv ) ;
}

FCKHighLighter._SetupClickListener = function (){
	if (FCKBrowserInfo.IsGecko)
		FCK.EditorDocument.addEventListener( 'click', FCKHighLighter._ClickListener, true ) ;
}

// 添加单击事件
FCK.Events.AttachEvent( 'OnAfterSetHTML', FCKHighLighter._SetupClickListener ) ;

// 添加右键菜单
FCK.ContextMenu.RegisterListener( {
	AddItems : function( menu, tag, tagName )
	{
		if (!tag)
			return;

		var oDiv = tag;

		// 循环的作用看一下代码就知道了，是为了选择高亮代码的最顶层元素
		while (oDiv.parentNode){
			if (oDiv.tagName == usingTag && oDiv.className == usingFlag)
				break;
			oDiv = oDiv.parentNode;
		}

		// under what circumstances do we display this option
		if ( oDiv.tagName == usingTag && oDiv.className == usingFlag )//&& (tag._FCKHighLighter || tag.parentElement._FCKHighLighter) ) 
		{
			FCKSelection.SelectNode( oDiv ) ;
			// when the option is displayed, show a separator  the command
			menu.AddSeparator() ;
			// the command needs the registered command name, the title for the context menu, and the icon path
			menu.AddItem( 'SyntaxHighlighter', FCKLang['DlgSyntaxHighLighterProperty'], oHighLighterItem.IconPath ) ;
		}
	}}
);
