<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="shoubiao.aspx.cs" Inherits="Hmj.WebApp.wechat.NewMember.shoubiao" %>

<html>
<head>
    <meta charset="utf-8">
    <title>盛时钟表管家</title>
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width"
        name="viewport" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/shengshi.css" />
    <style type="text/css">
        .erweimaMask
        {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            height: 100%;
            width: 100%;
            background: rgba(0,0,0,0.8);
            display: none;
             z-index:22;
        }
        .erweimaImg
        {
            position: absolute;
            left: 0;
            top: 0%;
            width: 100%;
            display: block;
        }
        .erweimaImg img
        {
            width: 85%;
            margin: 0 auto;
            padding-top: 0px;
            display: block;
        }
        .erweimaImg p
        {
            background: url(img/wenzi.png) no-repeat center center;
            background-size: 90% auto;
            padding: 6rem 0;
            display: block;
            text-align: center;
        }
        .changAnBtn
        {
            border: 1px solid #fff;
            width: 20rem;
            height: 3rem;
            color: #fff;
            line-height: 3rem;
            border-radius: 10px;
            margin: 0 auto;
            text-align: center;
            font-size: 1.6rem;
        }
        .erweiDel
        {
            position: absolute;
            right: 120px;
            top: 117px;
            width: 20px;
            height: 25px;
            background: url(img/shanchu2.png) no-repeat center center;
            background-size: 20px;
            display: block;
            z-index:21;
        }
        .menberIndexNav strong.red
        {
            color: #ff0000;
        }
    </style>
    
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
    </script>
</head>
<body>
    <div class="menberBox" style="padding: 0; height: auto;">
        <img src="img/banner.jpg">
        <div class="kefuBox">
            <div class="touxiang" id="gjtx" runat="server">
                <img src="images/meinv.jpg">
            </div>
            <p id="name" runat="server">
            </p>
            <div class="wujiaoxing">
                <em></em><em></em><em></em><em></em><em></em>
            </div>
        </div>
    </div>
    <div class="erjiNav" style="display: none;">
        <ol>
            累计顾客服务数<strong id="ljcount" runat="server"></strong></ol>
        <ol>
            绑定顾客数<strong id="bdcount" runat="server"></strong></ol>
        <ol>
            今日访客数<strong id="fkcount" runat="server"></strong></ol>
    </div>
    <input type="hidden" id="HfName" runat="server" name="HfName" value="" />
  <%--  <div class="noticeBox" onclick="alertval()">
        <p style="color: rgba(94, 93, 95, 0.47)">
            您有5条最新消息，点击查看吧！</p>
    </div>--%>
    <div class="menberIndexNav">
        <ul>
         <%--   <li><a href="http://wechat.censh.com/wechat/GJ/duihua/duihua.html" onclick="zhugeclick('wx_C-菜单-钟表管家-私人管家-点击')">
                <div>
                    <bdo class="icon01"></bdo><strong style="color: #7745C3">私人管家</strong>
                </div>
            </a></li>--%>
       <%--     <li><a href="http://www.censh.com/seckilling/">
                <div>
                    <bdo class="icon04"></bdo><strong style="color: #7745C3">秒杀拍卖</strong>
                </div>
            </a></li>--%>
<%--            <li><a href="https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/Store/daohang.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect">--%>
            <li><a href="https://www.censh.com/stores" onclick="zhugeclick('wx_C-菜单-钟表管家-附件门店-点击')">
                <div>
                    <bdo class="icon03"></bdo><strong style="color: #7745C3">附近门店</strong>
                </div>
            </a></li>
        <%--    <li>
                <div onclick="alertval()">
                    <bdo class="icon02"></bdo><strong style="color: rgba(94, 93, 95, 0.47)">微商城</strong>
                </div>
            </li>--%>
            <li><a href="https://www.censh.com/watch/?from=singlemessage&isappinstalled=0" onclick="zhugeclick('wx_C-菜单-钟表管家-选表预约-点击')">
                <div>
                    <bdo class="icon05"></bdo><strong style="color: #7745C3">选表预约</strong>
                </div>
            </a></li>
            <li><a href="http://wechat.censh.com/wechat/Repair/zhishi.html" onclick="zhugeclick('wx_C-菜单-钟表管家-维修保养-点击')">
                <div>
                    <bdo class="icon06"></bdo><strong style="color: #7745C3">保养知识</strong>
                </div>
            </a></li>
            <li id="shengshixue"><a onclick="zhugeclick('wx_C-菜单-钟表管家-腕表趣闻-点击')">
                <div>
                    <bdo class="icon07"></bdo><strong style="color: #7745C3">腕表趣闻</strong>
                </div>
                </a>
            </li>
            <li><a href="http://www.censh.com/shop" onclick="zhugeclick('wx_C-菜单-钟表管家-在线商城-点击')">
                <div>
                    <bdo class="icon08"></bdo><strong style="color: #7745C3">在线商城</strong>
                </div>
            </a></li>
            <li>
                <div onclick="Confirmval()">
                    <bdo class="icon09"></bdo><strong style="color: #7745C3">解除绑定</strong>
                </div>
            </li>
        </ul>
    </div>
    <div class="erweimaMask">
        <div class="erweiDel">
        </div>
        <div class="erweimaImg">
            <img src="img/erweima.png">
        </div>
    </div>
</body>
</html>

<script src="js/jquery.min.js"></script>

<script src="js/swiper.min.js"></script>

<script src="js/Message.js"></script>

<script>
    function alertval() {
        $.MsgBox.Alert("盛时", "正在建设中...");
    }
    function getewm(id) {
        location = "Ewm.aspx?Id=" + id;
    }
    function Confirmval() {
        var name = $("#HfName").val();
        alert
        if (name.length == 0) {
            $.MsgBox.Alert("盛时", "您还未绑定，请至就近门店扫描管家二维码进行绑定！");
        } else {

            var altname = "您将解除与你的钟表管家" + name + "的一对一咨询服务关系，请您慎重确认！";

            $.MsgBox.Confirm("盛时", altname, function myfunction() {
                $.ajax({
                    type: "POST",
                    url: "../Business.ashx?para=GetJb",
                    async: true,
                    timeout: 15000,
                    dataType: "json",
                    success: function (data) {
                        zhuge.track('wx_C-菜单-钟表管家-解除绑定-点击', {
                            'name': name//事件自定义属性
                        }, function () {
                           // location.href = 'http://www.xxx.com/b.html';//执行打点的后续操作,如页面跳转
                        });
                        $.MsgBox.Alert("盛时", data.message, function loc() {
                            location = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/Shoubiao.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect";
                        });
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $.MsgBox.Alert("盛时", "解绑失败");
                    }
                });
            });
        }
    }
    $('.erweiDel').on('click', function () {

        $('.erweimaMask').fadeOut();
    })

    $('#shengshixue').on('click', function () {

        $('.erweimaMask').fadeIn();
    })
    function zhugeclick(msg) {
        zhuge.track(msg);
    }
    zhugeclick('wx_C-菜单-钟表管家-点击')
</script>

