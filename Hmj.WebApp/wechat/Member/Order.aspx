<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="Hmj.WebApp.wechat.Member.Order" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/mian.css" />
    <title>会员订单信息</title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="topnav">
            <h2>订单情况</h2>
        </div>
        <div class="menber">
            <ul>
                <li><span class="kahaoicon" runat="server" id="cardno"><em></em></span></li>
                <li><span class="nameicon" runat="server" id="name"><em></em></span></li>
                <li><span class="telicon" runat="server" id="phone"><em></em></span></li>
                <!-- <li><span class="nandeicon"><em>先生</em></span></li>-->
                <li id="sex" runat="server"><span class="nvdeicon"><em>女士</em></span></li>
            </ul>
<%--            <p>会员积分<bdo id="jf" runat="server"></bdo>  
             会员有效期<bdo id="timeend" runat="server"></bdo>
            </p>--%>
        </div>

        <div class="jifenbox">
            <div class="jifentab">
                <ol class="curbtn" onclick="javascript:Hoverbtn(1);" id="dd1">销售订单</ol>
                <ol onclick="javascript:Hoverbtn(2);" id="dd2">网上商城</ol>
                <ol onclick="javascript:Hoverbtn(4);" id="dd4">预约订单</ol>
                <ol onclick="javascript:Hoverbtn(3);" id="dd3">维修订单</ol>
            </div>
            <div class="dis" id="conb1">
                <div class="jifenboxTT">
                    <ul>
                        <li ><span>品牌</span></li>
                        <li ><span>金额</span></li>
                        <li style="width:12.5%"><span>数量</span></li>
                        <li style="width:12.5%"><span>折扣</span></li>
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
                        <li style="width:12.5%"><span>数量</span></li>
                        <li style="width:12.5%"><span>折扣</span></li>
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
            <%--            <div class="jifenboxTT">
                <ul>
                    <li><span>品牌名</span></li>
                    <li><span>购买时间</span></li>
                    <li><span>购买柜台</span></li>
                    <li><span>金额</span></li>
                </ul>
            </div>

            <div class="jifenboxTT">
                <ul id="con" runat="server">
                </ul>
            </div>--%>
        </div>
<%--        <div class="bottomline"></div>--%>
    </form>
</body>
</html>
<script type="text/javascript">
    function g(o) {
        return document.getElementById(o);
    }
    function Hoverbtn(n) {
        for (var i = 1; i <= 4; i++) {
            g('dd' + i).className = '';
            g('conb' + i).className = 'undis';
        }
        g('conb' + n).className = 'dis';
        g('dd' + n).className = 'curbtn';
    }
</script>
