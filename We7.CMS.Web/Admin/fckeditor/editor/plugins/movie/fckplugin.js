/* 
 *  FCKPlugin.js for Movie Player
 *  ------------
 */

// Register the related commands.
FCKCommands.RegisterCommand(
	'Movie',
	new FCKDialogCommand(
		'Movie',
		FCKLang['MoviePlayerDlgTitle'],
		FCKConfig.PluginsPath + 'movie/movieplayer.html',
		450, 260
	)
);
 
// Create the toolbar button.
var oMoviePlayerItem = new FCKToolbarButton(
	'Movie', 
	FCKLang['MoviePlayerBtn'], 
	FCKLang['MoviePlayerTooltip'],
	null, 
	false, true);
oMoviePlayerItem.IconPath = FCKConfig.PluginsPath + 'movie/filmreel.gif'; 
FCKToolbarItems.RegisterItem('Movie', oMoviePlayerItem);
