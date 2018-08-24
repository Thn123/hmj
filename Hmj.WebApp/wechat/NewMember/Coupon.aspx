<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Coupon.aspx.cs" Inherits="Hmj.WebApp.wechat.NewMember.Coupon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/shengshi.css" />
    <title>会员兑礼信息</title>
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
    zhuge.track('wx_C-菜单-我的盛时-会员卡-兑礼信息-点击');
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="askbox">
            <div class="askboxcon">
                <h2>兑礼情况</h2>
            </div>
        </div>
        <div class="regMenber">
            <ul>
                <li><span class="kahaoicon" runat="server" id="cardno"><em></em></span></li>
                <li><span class="nameicon" runat="server" id="name"><em></em></span></li>
                <li><span class="Regtel_icon" runat="server" id="phone"><em></em></span></li>
                <!-- <li><span class="nandeicon"><em>先生</em></span></li>-->
                <li id="sex" runat="server"><span class="nvdeicon"><em>女士</em></span></li>

            </ul>
            <%--<p>会员积分<bdo id="jf" runat="server"></bdo>  
             会员有效期<bdo id="timeend" runat="server"></bdo>
            </p>--%>
        </div>

        <div class="jifenbox">
            <h2>兑礼礼品</h2>
            <div class="jifenboxTT">
                <ul>
                    <li><span>礼品名称</span></li>
                    <li><span>兑换积分</span></li>
                    <li><span>数量</span></li>
                    <li><span>兑礼时间</span></li>
                </ul>
            </div>

            <div class="jifenboxTT">
                <ul id="con" runat="server">
                </ul>
            </div>
        </div>
        <div class="bottomline"></div>
    </form>
</body>
</html>
