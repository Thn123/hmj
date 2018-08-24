<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Hmj.WebApp.wechat.Test.Index" %>

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
        <div class="page">
            <div class="checkBao">
                <div class="checkBaoBtn">
                    <select class="dropdown-select" id="nl">
                        <option value="-1">请选择宝宝月龄</option>
                        <option value="0">0-6个月</option>
                        <option value="1">6-12个月</option>
                        <option value="2">12-18个月</option>
                        <option value="3">18个月以上</option>
                    </select></div>
                <a href="javascript:;" onclick="tz();">
                    <div class="xiayibu"></div>
                </a>
            </div>
        </div>
    </form>
</body>
</html>
<script type="text/javascript" src="js/jquery.1.11.0.min.js"></script>
<script>
    function tz() {
        if ($("#nl").val() == -1) {
            alert("请选择宝宝月龄");
        } else {
            location = "prolist.aspx?yl=" + $("#nl").val() + "&ylname=" + $("#nl").find("option:selected").text();
        }
    }
</script>
