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
        //卡包信息选项卡
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

        //优惠券选项卡
        $("#tab_coupon").click(function (e) {
            var ifr = $("#coupon_ifr");
            var cardtype = $(this).attr("abbr");
            var url = options.couponUrl;
            //alert(url);
          
            if (ifr.length == 0) {               
                $("#coupon").append("<iframe name='coupon_ifr' width='100%' height='" + bodyheight + "px' id='coupon_ifr' src='" + url + "' frameborder='0' />");
            }
            else {
                ifr[0].src = url;
            }
        });

        //会员积分选项卡
        $("#tab_points").click(function (e) {
            var ifr = $("#points_ifr");
            var cardtype = $(this).attr("abbr");
            var url = options.pointsUrl;
            //alert(url);
          
            if (ifr.length == 0) {               
                $("#points").append("<iframe name='points_ifr' width='100%' height='" + bodyheight + "px' id='points_ifr' src='" + url + "' frameborder='0' />");
            }
            else {
                ifr[0].src = url;
            }
        });

        //会员礼券/促销选项卡
        $("#tab_certificate").click(function (e) {
            var ifr = $("#certificate_ifr");
            var cardtype = $(this).attr("abbr");
            var url = options.certificateUrl;
            //alert(url);

            if (ifr.length == 0) {
                $("#certificate").append("<iframe name='certificate_ifr' width='100%' height='" + bodyheight + "px' id='certificate_ifr' src='" + url + "' frameborder='0' />");
            }
            else {
                ifr[0].src = url;
            }
        });

        //会员附件选项卡
        $("#tab_files").click(function (e) {
            var ifr = $("#files_ifr");
            var cardtype = $(this).attr("abbr");
            var url = options.filesUrl;
            //alert(url);

            if (ifr.length == 0) {
                $("#files").append("<iframe name='files_ifr' width='100%' height='" + bodyheight + "px' id='files_ifr' src='" + url + "' frameborder='0' />");
            }
            else {
                ifr[0].src = url;
            }
        });

        //发卡
        $("#btnFaCard").click(function (e) {
            var id = $("#hideCid").val();
            var name = $("#spanCustName").html();
            var phone = $("#spanMobile").html();
            var url = "/Customer/FaCard.do?cid=" + id + "&name=" + name + "&phone=" + phone;
            window.xjDailog.Open(url, {
                width: 520,
                height: 350,
                caption: '发卡',
                theme: "simple", //默认主题
                onclose: function (userstate) {
                    window.location.href = "/Customer/view360.do?cid=" + id ;

                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            });
        });
        //补卡 挂失
        $("#btnMakeUpCard").click(function () {
            var status = $("#hideStatus").val();
            if (status=="0") {
                _showInfoMessage('该会员还没有发卡！', 'info');
                return false;
            }
            var id = $("#hideCid").val();
            var url = "/Customer/MakeUpCard.do/" + id;
            window.xjDailog.Open(url, {
                width: 600,
                height: 450,
                caption: '发卡',
                theme: "simple", //默认主题
                onclose: function (userstate) {
                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            });
        });
        //购卡/充值
        $("#btnBuyCard").click(function () {
            var status = $("#hideStatus").val();
            if (status == "0") {
                _showInfoMessage('该会员还没有发卡！', 'info');
                return false;
            }
            var id = $("#hideCid").val();
            window.location.href = "/Order/BuyCard.do/"+id;
        });
        //转账
        $("#btnTransfer").click(function () {
            window.location.href = "/Customer/CustomerTransfer.do?cno=" + $("#hideCard_No").val();
        });
        //退卡
        $("#btnAbsentCard").click(function () {
            var status = $("#hideStatus").val();
            if (status == "0") {
                _showInfoMessage('该会员还没有发卡！', 'info');
                return false;
            }
            var id = $("#hideCid").val();
            window.location.href = "/Order/AbsentCard.do/" + id;
        });
        //积分兑换
        $("#btnExchange").click(function () {
            var id = $("#hideCid").val();
            window.location.href = "/Customer/MemberPoints.do/"+id;
        });

    });

    _utils.UploadCallback = function (fileid, url,id) {        
            $("#IMAGE_SHOW").attr("src", url);
            $("#IMAGE_ID").val(id);
    };

    $("#exec1").click(function ()
    {
        window.location.href = "../mot/execute.do";
    })
    $("#exec2").click(function () {
        window.location.href = "../mot/execute.do";
    })
    $("#exec3").click(function () {
        window.location.href = "../mot/execute.do";
    })
    $("#exec4").click(function () {
        window.location.href = "../mot/execute.do";
    })
})(window, undefined, jQuery);