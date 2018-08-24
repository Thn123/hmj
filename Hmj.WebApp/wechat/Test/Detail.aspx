<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Hmj.WebApp.wechat.Test.Detail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
    <meta name="apple-mobile-web-app-title" content="">
    <!-- uc强制竖屏 -->
    <meta name="screen-orientation" content="portrait">
    <!-- UC强制全屏 -->
    <meta name="full-screen" content="yes">
    <!-- UC应用模式 -->
    <meta name="browsermode" content="application">
    <!-- QQ强制竖屏 -->
    <meta name="x5-orientation" content="portrait">
    <!-- QQ强制全屏 -->
    <meta name="x5-fullscreen" content="true">
    <!-- QQ应用模式 -->
    <meta name="x5-page-mode" content="app">
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/mian.css" />
    <title>TT家族</title>
</head>
<body>
    <form id="form1" runat="server">
        <%if (t != null)
          { %>
        <div class="shopImg">
            <div class="swiper-container" id="swiperbanner" style="height: auto;">
                <div class="swiper-wrapper">
                    <%if(t.TopImg1!=null){ %>
                    <div class="swiper-slide">
                        <img src="<%=WebUrl+t.TopImg1 %>"></div>
                    <%} %>
                    <%if(t.TopImg2!=null){ %>
                    <div class="swiper-slide">
                        <img src="<%=WebUrl+t.TopImg2 %>"></div>
                    <%} %>
                     <%if(t.TopImg3!=null){ %>
                    <div class="swiper-slide">
                        <img src="<%=WebUrl+t.TopImg3 %>"></div>
                    <%} %>
                     <%if(t.TopImg4!=null){ %>
                    <div class="swiper-slide">
                        <img src="<%=WebUrl+t.TopImg4 %>"></div>
                    <%} %>
                </div>
                <div class="swiper-pagination"></div>
            </div>
        </div>
        <div class="prodetailInfo">
            <h2><%=t.Name %></h2>
            <p><%=t.Desc %></p>
            <span>价格：<em><%=t.Price %></em>元</span>
        </div>

        <div class="prodetail">
            <%=t.Detail %>
        </div>

        <div class="footerbar">
            <ul>
                <li><a href="javascript:buy();"><span>线上购买</span></a></li>
                <li class="cur"><a href="city.aspx"><span>实体店购买</span></a></li>
            </ul>
        </div>
        <%} %>
    </form>
</body>
</html>
<script type="text/javascript" src="js/jquery.1.11.0.min.js"></script>
<script type="text/javascript" src="js/swiper.js"></script>
<script>
    var mySwiper = new Swiper('#swiperbanner', { loop: true, autoplay: 4500, pagination: '.swiper-pagination' });

    var tipsflag = true;
    function tips(text) {
        if (tipsflag == true) {
            var tishiDiv = document.createElement('div');
            tishiDiv.className = "motify";
            document.body.appendChild(tishiDiv);
            tipsflag = false;
        }
        $('.motify').html(text).show();
        setTimeout(function () { $('.motify').fadeOut(); }, 500);
    }
    function buy() {
        tips('敬请期待！');
    }
</script>
