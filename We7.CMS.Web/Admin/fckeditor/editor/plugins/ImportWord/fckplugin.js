// JScript 文件

FCKCommands.RegisterCommand('ImportWord', new FCKDialogCommand('ImportWord', FCKLang.ImportWord, FCKPlugins.Items['ImportWord'].Path + 'ImportWord.aspx', 400, 300));
var ImportWordeItem = new FCKToolbarButton('ImportWord', FCKLang.ImportWord);
ImportWordeItem.IconPath = FCKPlugins.Items['ImportWord'].Path + 'ImportWord.gif';
FCKToolbarItems.RegisterItem('ImportWord', ImportWordeItem);