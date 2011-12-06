window.onload = function()
{
	var copyP = document.createElement( 'p' ) ;
	copyP.className = 'copyright' ;
	copyP.innerHTML = '&copy; 2007-2008 Frederico Caldeira Knabben (<a href="http://www.fredck.com" target="_blank">FredCK.com</a>). All rights reserved.<br /><br />' ;
	document.body.appendChild( document.createElement( 'hr' ) ) ;
	document.body.appendChild( copyP ) ;
	
	window.top.SetActiveTopic( window.location.pathname ) ;
}