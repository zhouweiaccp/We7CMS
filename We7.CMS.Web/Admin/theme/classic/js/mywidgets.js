
// DOM ready!

$(function(){

  // Use the cookie plugin
  
  $.fn.EasyWidgets({

    behaviour : {
      useCookies : true
    },
      i18n : {
      editText : '<img src="./edit.png" alt="Edit" width="16" height="16" />',
      closeText : '<li class="closeImg"><img src="/admin/images/blank.gif" alt="关闭" width="25" height="25" /></li>',
      collapseText : '<li class="collapseImg"><img src="/admin/images/blank.gif" alt="折叠" width="25" height="25" /></li>',
      cancelEditText : '<img src="./edit.png" alt="Edit" width="16" height="16" />',
      extendText : '<li class="extendImg"><img src="/admin/images/blank.gif" alt="展开" width="25" height="25" /></li>',
      closeTitle : '关闭',
      confirmMsg : '您确认要关闭此挂件吗？',
      extendTitle : '展开',
      collapseTitle : '折叠'
    }

  });
  
});