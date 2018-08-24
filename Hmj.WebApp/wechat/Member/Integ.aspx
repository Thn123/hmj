<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Integ.aspx.cs" Inherits="Hmj.WebApp.wechat.Member.Integ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/mian.css" />
    <title>会员积分明细</title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="topnav">
            <h2>积分情况</h2>
        </div>
        <div class="menber">
            <ul>
                <li><span class="kahaoicon" runat="server" id="cardno"><em></em></span></li>
                <li><span class="nameicon" runat="server" id="name"><em></em></span></li>
                <li><span class="telicon" runat="server" id="phone"><em></em></span></li>
                <!-- <li><span class="nandeicon"><em>先生</em></span></li>-->
                <li id="sex" runat="server"><span class="nvdeicon"><em>女士</em></span></li>

            </ul>
            <p>会员积分<bdo id="jf" runat="server"></bdo> </p>
            <%-- 会员有效期<bdo id="timeend" runat="server"></bdo>
            </p>--%>
        </div>

        <div class="jifenbox">
            <h2>积分明细</h2>
            <div class="jifenboxTT">
                <ul>
                    <li><span>品牌名</span></li>
                    <li><span>购买金额</span></li>
                    <li><span>购买时间</span></li>
                    <li><span>产生积分</span></li>
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
