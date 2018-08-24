<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ewm.aspx.cs" Inherits="Hmj.WebApp.wechat.NewMember.Ewm" %>
<html>
<head>
<meta charset="utf-8">
<title>盛时钟表管家</title>
<meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" /> 
<meta content="yes" name="apple-mobile-web-app-capable" /> 
<meta content="black" name="apple-mobile-web-app-status-bar-style" /> 
<meta content="telephone=no" name="format-detection" />
<link type="text/css" rel="stylesheet" href="css/shengshi.css" />
<style type="text/css">
.guanjia-info-admin{ background:#563b7f;overflow:hidden; height:100px;} 
.guanjia_erweima{ display:block; padding:0px 10px 10px;}
.guanjia_erweima img{ width:50%; float:none; margin:0 auto; display:block; height:auto}
.guanjia_erweima p{ font-size:14px; color:#999; text-align:center; line-height:40px; border-bottom:none;}
.guanjia-infoadmin{ position:absolute; left:50%; top:55px; width:80px; height:80px; margin-left:-40px; display:block;}
.guanjia-infoadmin img{ width:100%; height:100%; border-radius:100%;position:absolute; left:0; top:0; right:0;}
.guanjia-info22{ padding-top:40px; display:block; padding-bottom:30px;}
.guanjia-info22 h2{ text-align:center; font-size:18px; color:#333; line-height:30px; font-family:Arial, Helvetica, sans-serif;}
.guanjia-info22 p{ line-height:30px; font-size:14px; color:#999; padding:0 10px; text-align:center;}
.pinpai{padding:5px; border-top:1px solid #ddd; position:relative; margin-bottom:0px; display:block;}
.pinpai strong{ position:absolute; left:50%; width:80px; height:30px; background:#fff; margin-top:-16px; margin-left:-40px; font-size:1.4rem; color:#666; text-align:center;}
.pinpai p{ line-height:22px; font-size:12px; color:#999; padding:10px; text-align:center;}
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
<div class="guanjia-info-admin"></div>

<div class="guanjia-infoadmin" id="guanjiaurl" runat="server">
</div>

<div class="guanjia-info22" runat="server" id="guanjia">
</div>

 <div class="pinpai">
    <strong>擅长品牌</strong>
    <p id="pp" runat="server"></p>
   </div>
<div class="pinpai">
    <strong>个人简介</strong>
    <p id="jj" runat="server"></p>
   </div>


 <div class="guanjia_erweima" id="ewmurl" runat="server">
  </div>


</body>
</html>