var Storage = function(win, doc){
		var hasSupport = true,
			store = win.localStorage,
			STORE_NAME = 'localstorage',
			obj,
			support = function (){ return hasSupport },
			error = function(){ throw new Error("don't support localStorage") };

		if (store && store.getItem){
			obj = {
				set : function(key, value){
					return store.setItem(key, value);
				},
				get : function(key){
					return store.getItem(key);
				},
				del : function(key){
					return store.removeItem(key);
				}
			};
		}else{
			store = doc.documentElement;
			try{
				store.addBehavior('#default#userdata');
				store.save(STORE_NAME);
			}catch(e){
				hasSupport = false;
			}
			if (hasSupport){
				obj = {
					set : function(key, value){
						store.setAttribute(key, value);
						store.save(STORE_NAME);
					},
					get : function(key){
						store.load(STORE_NAME);
						return store.getAttribute(key);
					},
					del : function(key){
						store.removeAttribute(key);
						store.save(STORE_NAME);
					}
				};

			}
		}
		if (!obj){
			obj = {
				set:error,
				get:error,
				del:error
			};
		}
		obj.support = support;
		return obj;
	}(window, document);
	
function menuInit(objId, hover) {
    var ext='.png';
    if(isIE6()) ext='.gif';
    var img = document.getElementById(objId + "_img");
    if (hover) {
        img.style.background = 'url(/admin/theme/classic/images/' + objId + '_hover'+ext+') no-repeat';
        preloadImages('/admin/theme/classic/images/' + objId +ext );
    }
    else {
        img.style.background = 'url(/admin/theme/classic/images/' + objId + ext+') no-repeat';
        preloadImages('/admin/theme/classic/images/' + objId + '_hover'+ext);
    }
}

function isIE6()
{
    var browser=navigator.appName 
    var b_version=navigator.appVersion 
    var version=b_version.split(";"); 
	if(version.length>1){
		var trim_Version=version[1].replace(/[ ]/g,""); 
		if(browser=="Microsoft Internet Explorer" && trim_Version=="MSIE6.0") 
			return true;
		else 
			return false;
	}
	else
		return false;
}

function initMenuData()
{
    Storage.set('mainMenu','');
    Storage.set('subMenuID','menu_mainboard_1');
    Storage.set('menuID','menu_mainboard');
}

function menuHover(obj) {
    var ext='.png';
    if(isIE6()) ext='.gif';

    var img = document.getElementById(obj.id + "_img");
    if (img) {
        img.style.background = 'url(/admin/theme/classic/images/' + obj.id + '_hover'+ext+') no-repeat';
    }
}

function menuOut(obj) {
    var ext='.png';
    if(isIE6()) ext='.gif';
    var img = document.getElementById(obj.id + "_img");
    img.style.background = 'url(/admin/theme/classic/images/' + obj.id +ext+ ') no-repeat';
}

function preloadImages() {
    if (document.images) {
        if (!document.imageArray) document.imageArray = new Array();
        var i, j = document.imageArray.length,
    args = preloadImages.arguments;
        for (i = 0; i < args.length; i++) {
            if (args[i].indexOf("#") != 0) {
                document.imageArray[j] = new Image;
                document.imageArray[j++].src = args[i];
            }
        }
    }
}

function checkKey()
{
    if(event.keyCode==13)
    {
        alert('form');
        return(false);
    }
}


function menuClick(url,menuid)
{
    if(Storage.support()) 
    {
        var arr=menuid.split('_');
        if(arr!=null  && arr.length>1)
        {
            Storage.set('menuID',arr[0]+'_'+arr[1]);
            Storage.set('subMenuID',menuid);
        }
    }
    window.top.location.href=url;
}
        
wpCookies = {
// The following functions are from Cookie.js class in TinyMCE, Moxiecode, used under LGPL.

	each : function(o, cb, s) {
		var n, l;

		if (!o)
			return 0;

		s = s || o;

		if (typeof(o.length) != 'undefined') {
			for (n=0, l = o.length; n<l; n++) {
				if (cb.call(s, o[n], n, o) === false)
					return 0;
			}
		} else {
			for (n in o) {
				if (o.hasOwnProperty(n)) {
					if (cb.call(s, o[n], n, o) === false)
						return 0;
				}
			}
		}
		return 1;
	},

	getHash : function(n) {
		var v = this.get(n), h;

		if (v) {
			this.each(v.split('&'), function(v) {
				v = v.split('=');
				h = h || {};
				h[v[0]] = v[1];
			});
		}
		return h;
	},

	setHash : function(n, v, e, p, d, s) {
		var o = '';

		this.each(v, function(v, k) {
			o += (!o ? '' : '&') + k + '=' + v;
		});

		this.set(n, o, e, p, d, s);
	},

	get : function(n) {
		var c = document.cookie, e, p = n + "=", b;

		if (!c)
			return;

		b = c.indexOf("; " + p);

		if (b == -1) {
			b = c.indexOf(p);

			if (b != 0)
				return null;
		} else
			b += 2;

		e = c.indexOf(";", b);

		if (e == -1)
			e = c.length;

		return decodeURIComponent(c.substring(b + p.length, e));
	},

	set : function(n, v, e, p, d, s) {
		document.cookie = n + "=" + encodeURIComponent(v) +
			((e) ? "; expires=" + e.toGMTString() : "") +
			((p) ? "; path=" + p : "") +
			((d) ? "; domain=" + d : "") +
			((s) ? "; secure" : "");
	},

	remove : function(n, p) {
		var d = new Date();

		d.setTime(d.getTime() - 1000);

		this.set(n, '', d, p, d);
	}
};

// Returns the value as string. Second arg or empty string is returned when value is not set.
function getUserSetting( name, def ) {
	var o = getAllUserSettings();

	if ( o.hasOwnProperty(name) )
		return o[name];

	if ( typeof def != 'undefined' )
		return def;

	return '';
}

// Both name and value must be only ASCII letters, numbers or underscore
// and the shorter, the better (cookies can store maximum 4KB). Not suitable to store text.
function setUserSetting( name, value, del ) {
	var c = 'we7-settings-'+userSettings.uid, o = wpCookies.getHash(c) || {}, d = new Date();
	var n = name.toString().replace(/[^A-Za-z0-9_]/, ''), v = value.toString().replace(/[^A-Za-z0-9_]/, '');

	if ( del ) delete o[n];
	else o[n] = v;

	d.setTime( d.getTime() + 31536000000 );
	p = userSettings.url;

	wpCookies.setHash(c, o, d, p );
	wpCookies.set('we7-settings-time-'+userSettings.uid, userSettings.time, d, p );
}

function deleteUserSetting( name ) {
	setUserSetting( name, '', 1 );
}

// Returns all settings as js object.
function getAllUserSettings() {
	return wpCookies.getHash('we7-settings-'+userSettings.uid) || {};
}


jQuery(document).ready( function($) {
	// pulse
	$('.fade').animate( { backgroundColor: '#ffffe0' }, 300).animate( { backgroundColor: '#fffbcc' }, 300).animate( { backgroundColor: '#ffffe0' }, 300).animate( { backgroundColor: '#fffbcc' }, 300);

	// show things that should be visible, hide what should be hidden
	$('.hide-if-no-js').removeClass('hide-if-no-js');
	$('.hide-if-js').hide();

	// Basic form validation
	if ( ( 'undefined' != typeof wpAjax ) && $.isFunction( wpAjax.validateForm ) ) {
		$('form.validate').submit( function() { return wpAjax.validateForm( $(this) ); } );
	}

	// Move .updated and .error alert boxes
	$('div.wrap h2 ~ div.updated, div.wrap h2 ~ div.error').addClass('below-h2');
	$('div.updated, div.error').not('.below-h2').insertAfter('div.wrap h2:first');

	// screen settings tab
	$('#show-settings-link').click(function () {
		if ( ! $('#screen-options-wrap').hasClass('screen-options-open') ) {
			$('#contextual-help-link-wrap').addClass('invisible');
		}
		$('#screen-options-wrap').slideToggle('fast', function(){
			if ( $(this).hasClass('screen-options-open') ) {
				$('#show-settings-link').css({'backgroundImage':'url("images/screen-options-right.gif")'});
				$('#contextual-help-link-wrap').removeClass('invisible');
				$(this).removeClass('screen-options-open');

			} else {
				$('#show-settings-link').css({'backgroundImage':'url("images/screen-options-right-up.gif")'});
				$(this).addClass('screen-options-open');
			}
		});
		return false;
	});

	// help tab
	$('#contextual-help-link').click(function () {
		if ( ! $('#contextual-help-wrap').hasClass('contextual-help-open') ) {
			$('#screen-options-link-wrap').addClass('invisible');
		}
		$('#contextual-help-wrap').slideToggle('fast', function(){
			if ( $(this).hasClass('contextual-help-open') ) {
				$('#contextual-help-link').css({'backgroundImage':'url("images/screen-options-right.gif")'});
				$('#screen-options-link-wrap').removeClass('invisible');
				$(this).removeClass('contextual-help-open');
			} else {
				$('#contextual-help-link').css({'backgroundImage':'url("images/screen-options-right-up.gif")'});
				$(this).addClass('contextual-help-open');
			}
		});
		return false;
	});

	// check all checkboxes
	var lastClicked = false;
	$( 'table:visible tbody .check-column :checkbox' ).click( function(e) {
		if ( 'undefined' == e.shiftKey ) { return true; }
		if ( e.shiftKey ) {
			if ( !lastClicked ) { return true; }
			var checks = $( lastClicked ).parents( 'form:first' ).find( ':checkbox' );
			var first = checks.index( lastClicked );
			var last = checks.index( this );
			var checked = $(this).attr('checked');
			if ( 0 < first && 0 < last && first != last ) {
				checks.slice( first, last ).attr( 'checked', function(){
					if ( $(this).parents('tr').is(':visible') )
						return checked ? 'checked' : '';

					return '';
				});
			}
		}
		lastClicked = this;
		return true;
	} );
	$( 'thead :checkbox, tfoot :checkbox' ).click( function(e) {
		var c = $(this).attr('checked');
		if ( 'undefined' == typeof  toggleWithKeyboard)
			toggleWithKeyboard = false;
		var toggle = e.shiftKey || toggleWithKeyboard;
		$(this).parents( 'form:first' ).find( 'table tbody:visible').find( '.check-column :checkbox' ).attr( 'checked', function() {
			if ( $(this).parents('tr').is(':hidden') )
				return '';
			if ( toggle )
				return $(this).attr( 'checked' ) ? '' : 'checked';
			else if (c)
				return 'checked';
			return '';
		});
		$(this).parents( 'form:first' ).find( 'table thead:visible, table tfoot:visible').find( '.check-column :checkbox' ).attr( 'checked', function() {
			if ( toggle )
				return '';
			else if (c)
				return 'checked';
			return '';
		});
	});
});

var showNotice, adminMenu, columns;

// stub for doing better warnings
showNotice = {
	warn : function(text) {
		if ( confirm(text) )
			return true;

		return false;
	},

	note : function(text) {
		alert(text);
	}
};

(function($){
// sidebar admin menu
    adminMenu = {

	init : function() {
		$('#adminmenu div.we7-menu-toggle').each( function() {
			if ( $(this).siblings('.we7-submenu').length )
				$(this).click(function(){ adminMenu.toggle( $(this).siblings('.we7-submenu') ); });
			else
				$(this).hide();
		});
	
		$('#adminmenu li.menu-top a.menu-top').click( function() { 
		         $('#adminmenu li.menu-top').removeClass('we7-menu-open');
		        adminMenu.toggle( $(this).siblings('.we7-submenu') ); 
		 } );
		 
		$('#adminmenu li.menu-top  .we7-menu-image').click( function() { 
		        $('#adminmenu li.menu-top').removeClass('we7-menu-open');
		        adminMenu.toggle( $(this).siblings('.we7-submenu') ); 
		 } );
		//$('#adminmenu li.menu-top .we7-menu-image').click( function() { window.location = $(this).siblings('a.menu-top')[0].href; } );
		this.favorites();

		$('.we7-menu-separator').click(function(){
			if ( $('#we7content').hasClass('folded') ) {
				adminMenu.fold(1);
				setUserSetting( 'mfold', 'o' );
			} else {
				adminMenu.fold();
				setUserSetting( 'mfold', 'f' );
			}
		});

		if ( 'f' != getUserSetting( 'mfold' ) ) {
			this.restoreMenuState();
		} else {
			this.fold();
		}
		
		 if(Storage.support()) 
        {
            var currentMenu=Storage.get('menuID');
            var currentSubMenu=Storage.get('subMenuID');
            if(currentMenu!=null && currentSubMenu!=null)
            {
                $('#'+currentMenu).addClass('we7-menu-open');
                $('#'+currentMenu).addClass('we7-has-current-submenu');
                $('#'+currentMenu+'>a').addClass('we7-menu-open');
                $('#'+currentMenu+'>a').addClass('we7-has-current-submenu');
                $('#'+currentSubMenu).addClass('current');
                $('#'+currentSubMenu+'>a').addClass('current');
                var obj = document.getElementById(currentMenu);
                if(obj)
                {
                    menuHover(obj);
                }
            }
        }
	},

	restoreMenuState : function() {
		$('#adminmenu li.we7-has-submenu').each(function(i, e) {
			var v = getUserSetting( 'm'+i );
			if ( $(e).hasClass('we7-has-current-submenu') ) return true; // leave the current parent open

			if ( 'o' == v ) $(e).addClass('we7-menu-open');
			else if ( 'c' == v ) $(e).removeClass('we7-menu-open');
		});
	},

	toggle : function(el) {

		el['slideToggle'](150, function(){el.css('display','');}).parent().toggleClass( 'we7-menu-open' );

		$('#adminmenu li.we7-has-submenu').each(function(i, e) {
			var v = $(e).hasClass('we7-menu-open') ? 'o' : 'c';
			setUserSetting( 'm'+i, v );
		});

		return false;
	},
	
	close:function(off) {

		$('.memu-top').removeClass('we7-menu-open');
	},

	fold : function(off) {
		if (off) {
			$('#we7content').removeClass('folded');
			$('#adminmenu li.we7-has-submenu').unbind();
		} else {
			$('#we7content').addClass('folded');
			$('#adminmenu li.we7-has-submenu').hoverIntent({
				over: function(e){
					var m = $(this).find('.we7-submenu'), t = e.clientY, H = $(window).height(), h = m.height(), o;

					if ( (t+h+10) > H ) {
						o = (t+h+10) - H;
						m.css({'marginTop':'-'+o+'px'});
					} else if ( m.css('marginTop') ) {
						m.css({'marginTop':''})
					}
					m.addClass('sub-open');
				},
				out: function(){ $(this).find('.we7-submenu').removeClass('sub-open').css({'marginTop':''}); },
				timeout: 220,
				sensitivity: 8,
				interval: 100
			});

		}
	},

	favorites : function() {
		$('#favorite-inside').width($('#favorite-actions').width()-4);
		$('#favorite-toggle').bind( 'click', function(){
            $('#favorite-inside').removeClass('slideUp').addClass('slideDown'); 
            setTimeout(function(){if ( $('#favorite-inside').hasClass('slideDown') ) { $('#favorite-inside').slideDown(100); $('#favorite-first').addClass('slide-down'); }}, 200) 
            if ( !$('#favorite-inside').hasClass('slideDown') )
                       getSitesList();
         } );
        $(' #favorite-inside').bind( 'mouseenter', function(){$('#favorite-inside').removeClass('slideUp').addClass('slideDown'); setTimeout(function(){if ( $('#favorite-inside').hasClass('slideDown') ) { $('#favorite-inside').slideDown(100); $('#favorite-first').addClass('slide-down'); }}, 200) } );
		$('#favorite-toggle, #favorite-inside').bind( 'mouseleave', function(){$('#favorite-inside').removeClass('slideDown').addClass('slideUp'); setTimeout(function(){if ( $('#favorite-inside').hasClass('slideUp') ) { $('#favorite-inside').slideUp(100, function(){ $('#favorite-first').removeClass('slide-down'); } ); }}, 300) } );
	},
	
	loaddata:function() {
	    $.ajax({
          url: "/admin/theme/classic/MenuData.aspx?url="+location.href,
          cache: false ,
          success: function(html){
            $("#mainMenu").append(html);
            $("#waitLayer").css("display","none");
            if(Storage.support()) Storage.set('mainMenu',html);
          }
        });
	}
};

$(document).ready(function()
{

    if(location.href.toLowerCase().indexOf("admin/signin.aspx") > 0 )  
    {
        if(!isIE6() && Storage.support()) 
        {
            initMenuData();
        }
   }
    else
    {
        if(!isIE6() && Storage.support() && location.href.toLowerCase().indexOf("reload=menu") < 0) 
        {
            var menuData=Storage.get('mainMenu');
            if(menuData)
            {
                 $("#mainMenu").append(menuData);
                 $("#waitLayer").css("display","none");
            }
            else
            {
                adminMenu.loaddata();
            }
        }
        else 
            adminMenu.loaddata();
    }
});
})(jQuery);

(function($){
// show/hide/save table columns
columns = {
	init : function(page) {
		$('.hide-column-tog').click( function() {
			var column = $(this).val();
			var show = $(this).attr('checked');
			if ( show ) {
				$('.column-' + column).show();
			} else {
				$('.column-' + column).hide();
			}
			columns.save_manage_columns_state(page);
		} );
	},

	save_manage_columns_state : function(page) {
		var hidden = $('.manage-column').filter(':hidden').map(function() { return this.id; }).get().join(',');
		$.post('admin-ajax.php', {
			action: 'hidden-columns',
			hidden: hidden,
			hiddencolumnsnonce: $('#hiddencolumnsnonce').val(),
			page: page
		});
	}
}

})(jQuery);

function getSitesList()
{
    $.ajax({
                url: $('#wdUrl').val(),
                cache: false,
                success: function(html)
                {  
                      $('#favorite-inside').html(html);   
                },
                failure:function(msg,resp,status)
                {
                    alert(msg+resp+status);
                }
            });
}
