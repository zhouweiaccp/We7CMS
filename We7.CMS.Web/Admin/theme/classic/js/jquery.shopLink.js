(function($){    
    $.fn.shopLink= function(){
        $("a.shopLink").click(function(e){
            var url = $(this).attr("href");
            $.colorbox({width:"100%", height:"100%", iframe:true,href:url});
            return false;
        });
    }    
})(jQuery);