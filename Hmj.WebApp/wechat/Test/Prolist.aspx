<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Prolist.aspx.cs" Inherits="Hmj.WebApp.wechat.Test.Prolist" %>

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
        <div class="prolistTT">
            <div class="baobaoTT">
                <h2><%=Request.QueryString["ylname"] %></h2>
            </div>
            <div class="searchBox">
                <div class="searchLeft">
                    <p>
                        <span>
                            <select class="chanpin-select" id="ptype" onchange="jz();">
                                <option>奶瓶</option>
                                <option>奶嘴</option>
                                <option>喂哺电器</option>
                                <option>储奶袋</option>
                                <option>安抚奶嘴</option>
                                <option>喂哺附件</option>
                                <option>学饮杯</option>
                                <option>餐具</option>
                            </select></span><em style="font-style: normal;">根据你筛选的条件，共找到5类产品</em>
                    </p>
                </div>
                <div class="searchRight">
                    <a href="proClass.aspx"><span>查看全部产品</span></a>
                </div>
            </div>
        </div>

        <div class="chanpinList proTop">
            <ul id="ulist">
                
            </ul>
        </div>
        <input id="yl" type="hidden" value="<%=Request.QueryString["yl"] %>" />
    </form>
</body>
</html>
<script type="text/javascript" src="js/jquery.1.11.0.min.js"></script>
<script type="text/javascript" src="js/lazy.js"></script>
<script>
    echo.init({
        offset: 100,
        throttle: 250
    });
    jz();

    function jz() {
        $.ajax({
            type: 'POST',
            url: "prolist.aspx?para=jz",
            data: { yl: $("#yl").val(),ptype:$("#ptype").val() },
            success: function (result) {
                $("#ulist").html(result);
            },
            dataType: "html"
        });
    }
</script>
