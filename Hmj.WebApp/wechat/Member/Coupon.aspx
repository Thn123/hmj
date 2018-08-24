<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Coupon.aspx.cs" Inherits="Hmj.WebApp.wechat.Member.Coupon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/mian.css" />
    <title>会员兑礼信息</title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="topnav">
            <h2>兑礼情况</h2>
        </div>
        <div class="menber">
            <ul>
                <li><span class="kahaoicon" runat="server" id="cardno"><em></em></span></li>
                <li><span class="nameicon" runat="server" id="name"><em></em></span></li>
                <li><span class="telicon" runat="server" id="phone"><em></em></span></li>
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
