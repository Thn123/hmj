; (function (window, undefined, $) {

    $(document).ready(function () {

        $("#btnBooking").click(function (e) {
            var url = options.newBookingUrl;
            window.location.href = url;//跳转至预约视图
//            window.xjDailog.Open(url, {
//                width: 680,
//                height: 570,
//                caption: '新建预约',
//                theme: "simple", //默认主题
//                onclose: function (userstate) {
//                    xjgrid.Reload();
//                   
//                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
//            });
        });
        $("#btnOrder").click(function (e) {
            var url = options.newOrderUrl;
            window.location.href = url;

        });

        //==============顾客360选项卡切换==================
        var bodyheight;
        $(document).ready(function (e) {
            var mainheigth = $(document).height();
            var pos = $("#tabcontent").offset();
            bodyheight = mainheigth - pos.top + 100;
           
        })
       
        "<iframe name='detail_ifr' width='100%' height='100%' id='detail_ifr' src='about:blank' frameborder='0' />";
        $("#tab_activities").click(function (e) {
            
        });
        //详细信息选项卡
        $("#tab_detail").click(function (e) {
            
            var ifr = $("#detail_ifr");
            var cardtype = $(this).attr("abbr");
            var url = options.detailUrl;
            //alert(url);
          
            if (ifr.length == 0) {               
                $("#detail").append("<iframe name='detail_ifr' width='100%' height='" + bodyheight + "px' id='detail_ifr' src='" + url + "' frameborder='0' />");
            }
            else {
                ifr[0].src = url;
            }

        });
        //预约选项卡
        $("#tab_booking").click(function (e) {
            
            var ifr = $("#booking_ifr");
            var cardtype = $(this).attr("abbr");
            var url = options.bookingUrl;
            //alert(url);
          
            if (ifr.length == 0) {
                $("#booking").append("<iframe name='booking_ifr' width='100%' height='" + bodyheight + "px' id='booking_ifr' src='" + url + "' frameborder='0' />");
            }
            else {
                ifr[0].src = url;
            }
            
        });
        //订单选项卡
        $("#tab_order").click(function (e) {
            var ifr = $("#order_ifr");
            var cardtype = $(this).attr("abbr");
            var url = options.orderUrl;
            //alert(url);
          
            if (ifr.length == 0) {               
                $("#order").append("<iframe name='order_ifr' width='100%' height='" + bodyheight + "px' id='order_ifr' src='" + url + "' frameborder='0' />");
            }
            else {
                ifr[0].src = url;
            }
        });
        //备忘录选项卡
        $("#tab_remark").click(function (e) {
            var ifr = $("#remark_ifr");
            var cardtype = $(this).attr("abbr");
            var url = options.remarkUrl;
            //alert(url);
          
            if (ifr.length == 0) {               
                $("#remark").append("<iframe name='remark_ifr' width='100%' height='" + bodyheight + "px' id='remark_ifr' src='" + url + "' frameborder='0' />");
            }
            else {
                ifr[0].src = url;
            }
        });
        //会员权益选项卡
        $("#tab_card").click(function (e) {
            var ifr = $("#card_ifr");
            var cardtype = $(this).attr("abbr");
            var url = options.cardUrl;
            //alert(url);
          
            if (ifr.length == 0) {               
                $("#card").append("<iframe name='card_ifr' width='100%' height='" + bodyheight + "px' id='card_ifr' src='" + url + "' frameborder='0' />");
            }
            else {
                ifr[0].src = url;
            }
        });

    });

    _utils.UploadCallback = function (fileid, url,id) {        
            $("#IMAGE_SHOW").attr("src", url);
            $("#IMAGE_ID").val(id);
        }; 
})(window, undefined, jQuery);