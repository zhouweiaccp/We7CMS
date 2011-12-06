// JScript 文件

FCKCommands.RegisterCommand('insertvideo', new FCKDialogCommand('insertvideo', FCKLang.InsertMusic, FCKPlugins.Items['insertvideo'].Path + 'insertvideo.html', 370, 250));
var insertcodeItem = new FCKToolbarButton('insertvideo', FCKLang.InsertMusic);
insertcodeItem.IconPath = FCKPlugins.Items['insertvideo'].Path + 'insertmusic.gif';
FCKToolbarItems.RegisterItem('insertvideo', insertcodeItem);