<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="Hmj.WebApp.wechat.NewMember.Order" %>

<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/shengshi.css">
    <title>会员基本信息</title>
    
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
    <div class="askbox">
        <div class="askboxcon">
            <h2>订单情况</h2>
        </div>
    </div>
    <div class="regMenber">
        <ul style="margin: 0; padding: 0;">
            <li><span class="kahaoicon" runat="server" id="cardno"><em></em></span></li>
            <li><span class="nameicon" runat="server" id="name"><em></em></span></li>
            <li><span class="Regtel_icon" runat="server" id="phone"><em></em></span></li>
            <!-- <li><span class="nandeicon"><em>先生</em></span></li>-->
            <li id="sex" runat="server"><span class="nvdeicon"><em>女士</em></span></li>
        </ul>
        <%--<div class="huiyuanInfo"><span>会员有效期<bdo>2016.10.1</bdo></span>会员积分<bdo>27261</bdo></div>--%>
    </div>



    <div class="jifenbox">

        <div class="jifentab">
            <ol class="curbtn" onclick="javascript:Hoverbtn(1);" id="dd1">销售订单</ol>
            <ol onclick="javascript:Hoverbtn(2);" id="dd2">网上商城</ol>
            <ol onclick="javascript:Hoverbtn(3);" id="dd3">维修订单</ol>
            <ol onclick="javascript:Hoverbtn(4);" id="dd4">预约订单</ol>
        </div>
        
            <div class="dis" id="conb1">
                <div class="jifenboxTT">
                    <ul>
                        <li ><span>品牌</span></li>
                        <li ><span>金额</span></li>
                        <li ><span>数量</span></li>
                        <li ><span>时间</span></li>
                    </ul>
                </div>

                <div class="jifenboxTT">
                    <ul id="con1" runat="server">
                    </ul>
                </div>
            </div>

            <div class="undis" id="conb2">
                <div class="jifenboxTT">
                    <ul>
                        <li ><span>品牌</span></li>
                        <li ><span>金额</span></li>
                        <li ><span>数量</span></li>
                        <li ><span>时间</span></li>
                    </ul>
                </div>

                <div class="jifenboxTT">
                    <ul id="con2" runat="server">
                    </ul>
                </div>
            </div>

            <div class="undis" id="conb3">
                <div class="jifenboxTT">
                    <ul>
                        <li ><span>品牌</span></li>
                        <li ><span>状态</span></li>
                        <li ><span>数量</span></li>
                        <li ><span>时间</span></li>
                    </ul>
                </div>

                <div class="jifenboxTT">
                    <ul id="con4" runat="server">
                    </ul>
                </div>
            </div>

            <div class="undis" id="conb4">
                <div class="jifenboxTT">
                    <ul>
                        <li ><span>品牌</span></li>
                        <li ><span>状态</span></li>
                        <li ><span>数量</span></li>
                        <li ><span>时间</span></li>
                    </ul>
                </div>

                <div class="jifenboxTT">
                   <ul id="con3" runat="server">
                    </ul>
                </div>
            </div>

    </div>



</body>
</html>
<script type="text/javascript">
    zhugeclick('wx_C-菜单-我的盛时-会员卡-订单情况-点击')
    function g(o) {
        return document.getElementById(o);
    }
    function Hoverbtn(n) {
        for (var i = 1; i <= 4; i++) {
            g('dd' + i).className = '';
            g('conb' + i).className = 'undis';
            if (i==1) {
                zhugeclick('wx_C-菜单-我的盛时-会员卡-订单情况-销售订单-点击');
            }
            if (i == 2) {
                zhugeclick('wx_C-菜单-我的盛时-会员卡-订单情况-网上商城-点击');
            }
            if (i == 3) {
                zhugeclick('wx_C-菜单-我的盛时-会员卡-订单情况-维修订单-点击');
            }
            if (i == 4) {
                zhugeclick('wx_C-菜单-我的盛时-会员卡-订单情况-预约订单-点击');
            }
        }
        g('conb' + n).className = 'dis';
        g('dd' + n).className = 'curbtn';
    }
    function zhugeclick(msg) {
        zhuge.track(msg);
    }
</script>
