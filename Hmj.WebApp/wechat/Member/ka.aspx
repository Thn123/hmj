<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ka.aspx.cs" Inherits="Hmj.WebApp.wechat.Member.ka" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/mian.css" />
    <title>盛时</title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="kaimg">
            <div runat="server" id="imagecon"></div>
            <div class="kaimgtxt" id="cardno" runat="server"></div>
        </div>
        <div class="kabox">
            <div class="kahao">
                <span id="name" runat="server"></span>
                <span id="phone" runat="server"></span>
                <span id="endtime" runat="server"></span>
                <span id="jf" runat="server"></span>
            </div>
            <ul>
                <li>
                    <div class="iconbox">
                        <bdo class="kaicon01">积分商城</bdo>
                    </div>
                    <div class="navbox" id="conjfsc" runat="server">
                        <span><a href="#">数码<br />
                            产品</a></span>
                        <span><a href="#">生活<br />
                            日用</a></span>
                        <span><a href="#">旅游<br />
                            健身</a></span>
                        <span><a href="#">维修<br />
                            服务</a></span>
                    </div>
                </li>
                <li>
                    <div class="iconbox">
                        <bdo class="kaicon02">电子钱包</bdo>
                    </div>
                    <div class="navbox">
                        <span><a onclick="alertval()">电池<br />
                            清洗</a></span>
                        <span><a onclick="alertval()">维修<br />
                            保养</a></span>
                        <span><a onclick="alertval()">生日<br />
                            礼券</a></span>
                    </div>
                </li>
                <li>
                    <div class="iconbox">
                        <bdo class="kaicon03">会员卡状况</bdo>
                    </div>
                    <div class="navbox">
                        <span><a href="https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/Member/Member.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect">基本<br />
                            信息</a></span>
                        <span><a href="https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/Member/Coupon.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect">兑礼<br />
                            情况</a></span>
                        <span><a href="https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/Member/Order.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect">订单<br />
                            情况</a></span>
                    </div>
                </li>
            </ul>
        </div>
    </form>
</body>
</html>
<script src="js/jquery-1.10.1.min.js"></script>
<script src="js/Message2.js"></script>
<script type="text/javascript">
    function alertval() {
        $.MsgBox.Alert("盛时", "正在建设中...");
    }
</script>
