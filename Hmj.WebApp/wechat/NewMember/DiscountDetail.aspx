<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiscountDetail.aspx.cs" Inherits="Hmj.WebApp.wechat.NewMember.discountDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8">
<meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" /> 
<meta content="yes" name="apple-mobile-web-app-capable" /> 
<meta content="black" name="apple-mobile-web-app-status-bar-style" /> 
<meta content="telephone=no" name="format-detection" />
<link type="text/css" href="css/quan.css" rel="stylesheet" />
    <title>优惠券详情</title>
    <script>(function (c, f) { var s = c.document; var b = s.documentElement; var m = s.querySelector('meta[name="viewport"]'); var n = s.querySelector('meta[name="flexible"]'); var a = 0; var r = 0; var l; var d = f.flexible || (f.flexible = {}); if (m) { var e = m.getAttribute("content").match(/initial\-scale=([\d\.]+)/); if (e) { r = parseFloat(e[1]); a = parseInt(1 / r) } } else { if (n) { var j = n.getAttribute("content"); if (j) { var q = j.match(/initial\-dpr=([\d\.]+)/); var h = j.match(/maximum\-dpr=([\d\.]+)/); if (q) { a = parseFloat(q[1]); r = parseFloat((1 / a).toFixed(2)) } if (h) { a = parseFloat(h[1]); r = parseFloat((1 / a).toFixed(2)) } } } } if (!a && !r) { var p = c.navigator.appVersion.match(/android/gi); var o = c.navigator.appVersion.match(/iphone/gi); var k = c.devicePixelRatio; if (o) { if (k >= 3 && (!a || a >= 3)) { a = 3 } else { if (k >= 2 && (!a || a >= 2)) { a = 2 } else { a = 1 } } } else { a = 1 } r = 1 / a } b.setAttribute("data-dpr", a); if (!m) { m = s.createElement("meta"); m.setAttribute("name", "viewport"); m.setAttribute("content", "initial-scale=" + r + ", maximum-scale=" + r + ", minimum-scale=" + r + ", user-scalable=no"); if (b.firstElementChild) { b.firstElementChild.appendChild(m) } else { var g = s.createElement("div"); g.appendChild(m); s.write(g.innerHTML) } } function i() { var t = b.getBoundingClientRect().width; if (t / a > 750) { t = 750 * a } var u = t / 10; b.style.fontSize = u + "px"; d.rem = c.rem = u } c.addEventListener("resize", function () { clearTimeout(l); l = setTimeout(i, 300) }, false); c.addEventListener("pageshow", function (t) { if (t.persisted) { clearTimeout(l); l = setTimeout(i, 300) } }, false); if (s.readyState === "complete") { s.body.style.fontSize = 12 * a + "px" } else { s.addEventListener("DOMContentLoaded", function (t) { s.body.style.fontSize = 12 * a + "px" }, false) } i(); d.dpr = c.dpr = a; d.refreshRem = i })(window, window["lib"] || (window["lib"] = {}));
</script>
    
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
    <form id="form1" runat="server">
   <div class="quan-fang-box">
<img src="images/quan/banner01.jpg"/>
<div class="quan_list quan_fale" style="top:0.6rem;">
   <ul style="display:block;">
      <li>
        <a href="#"><div class="quanbg">
          <div class="quanLeft">
          <h2><%=DiscountName %><span><%=DiscoutnNo %></span></h2>
          <p><strong>适用范围：</strong><%=DiscountMs %><br/>
          <strong>适用门店：</strong><%=DiscountStore %></p>
          <em></em>
          </div>
          <div class="quanRight">
            <h2><em>￥</em><%=DiscountPrice %></h2>
            <p class="padd"></p>
            <p>有效期至<br/><%=DiscountTime %></p>
          </div>
        </div></a>
      </li>
   </ul>
    
</div>
</div>

<div class="hexiao-erweima-box">
  <img src="<%=Qcode %>"/>
</div>
    </form>
</body>
</html>
<script type="text/javascript" src="js/jquery.min.js"></script>