<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ka.aspx.cs" Inherits="Hmj.WebApp.wechat.NewMember.ka" %>

<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <title>盛时会员卡</title>
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/shengshi.css" />
    
      <script type="text/javascript">
    window.zhuge = window.zhuge || [];
    window.zhuge.methods = "_init debug identify track trackLink trackForm page".split(" ");
    window.zhuge.factory = function(b) {
        return function() {
            var a = Array.prototype.slice.call(arguments);
            a.unshift(b);
            window.zhuge.push(a);
            return window.zhuge;
        }
    };
    for (var i = 0; i < window.zhuge.methods.length; i++) {
        var key = window.zhuge.methods[i];
        window.zhuge[key] = window.zhuge.factory(key);
    }
    window.zhuge.load = function(b, x) {
        if (!document.getElementById("zhuge-js")) {
            var a = document.createElement("script");
            var verDate = new Date();
            var verStr = verDate.getFullYear().toString()
                + verDate.getMonth().toString() + verDate.getDate().toString();

            a.type = "text/javascript";
            a.id = "zhuge-js";
            a.async = !0;
            a.src = (location.protocol == 'http:' ? "http://www.censh.com/js/zhuge.min.js" : 'https://www.censh.com/js/zhuge.min.js');
             a.onerror = function(){  
             window.zhuge.identify = window.zhuge.track = function(ename, props, callback){                   
               if(callback && Object.prototype.toString.call(callback) === '[object Function]')callback();               
                };     
            };
            var c = document.getElementsByTagName("script")[0];
            c.parentNode.insertBefore(a, c);
            window.zhuge._init(b, x)
        }
    };
    window.zhuge.load('90fc349d64e24ca8ac1bd203c490f5f8', { debug: true });

    zhuge.identify('1');
    </script>
</head>

<body>
    <div class="menberBox">
        <div id="imagecon" runat="server"></div>
        <div class="menberKahao">
            <p id="cardno" runat="server"></p>
        </div>
    </div>
    <div class="menberInfo">
        <p><span>积分<br />
            <em runat="server" id="jf"></em></span><img id="nickname" runat="server" src="images/renwu.jpg">
            <strong runat="server" id="name"></strong><bdo runat="server"  id="phone"></bdo><bdo id="endtime" runat="server"></bdo></p>
    </div>


    <div class="menberNav">
        <div class="menNavlist" id="conjfsc" runat="server" >
        
          
        </div>

        <div class="menNavlist">
            <h2>电子钱包</h2>
            <div class="swiper-container">
                <div class="swiper-wrapper">
                    
                      <div class="swiper-slide"><a href="https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/Discount.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect" onclick="zhugeclick('wx_C-菜单-我的盛时-会员卡-旅游健身-点击')">
                          <span class="icon12" ></span>
                        <p>维修保养</p></a>
                    </div>
                </div>
            </div>
        </div>

        <div class="menNavlist">
            <h2>会员卡状况</h2>
            <ol><a href="https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/Member.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect"  onclick="zhugeclick('wx_C-菜单-我的盛时-会员卡-基本信息-点击')"><span class="icon21"></span>
                <p>基本信息</p></a>
            </ol>
            <ol><a href="https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/Coupon.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect"  onclick="zhugeclick('wx_C-菜单-我的盛时-会员卡-兑礼信息-点击')"><span class="icon22"></span>
                <p>兑礼信息</p></a>
            </ol>
            <ol><a href="https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/Order.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect"  onclick="zhugeclick('wx_C-菜单-我的盛时-会员卡-订单情况-点击')"><span class="icon23"></span>
                <p>订单情况</p></a>
            </ol>
        </div>
    </div>
</body>
</html>
<script src="js/jquery.min.js"></script>
<script src="js/swiper.min.js"></script>
<script src="js/Message.js"></script>
 <script>
     var swiper = new Swiper('.swiper-container', {
         slidesPerView: 4,
         paginationClickable: true
     });
    function alertval() {
        $.MsgBox.Alert("盛时", "正在建设中...");
    }
    function zhugeclick(msg) {
        zhuge.track(msg);
    }
    zhugeclick('wx_C-菜单-我的盛时-会员卡-点击');
    </script>
