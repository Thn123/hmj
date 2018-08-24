<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Band.aspx.cs" Inherits="Hmj.WebApp.wechat.Member.Band" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/mian.css" />
    <script src="js/jquery-1.10.1.min.js"></script>
    <script src="js/Message2.js"></script>
    <title>会员绑定</title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="topbox">
            <ul>
                <li><a href="Reg.aspx"><span>新用户注册</span></a></li>
                <li><a href="Band.aspx"><span class="curbtn">老会员绑定</span></a></li>
            </ul>
        </div>

        <div class="regbox">
            <p>
                <bdo class="phone_icon"></bdo><span>
                    <input name="" type="tel" id="txtphone" placeholder="请输入11位手机号" class="txtinput" style="width: 70%; float: left;" /><em id="timeid" class="yanzheng">验证</em></span>
            </p>
            <p>
                <bdo class="ma_icon"></bdo><span>
                    <input name="" id="txtyzm" type="text" placeholder="手机收到的验证码" class="txtinput" /></span>
            </p>

            <h3 onclick="tz()">盛时会员俱乐部需知</h3>
            <div class="regbtn" id="bangding">直接绑定</div>
        </div>
        <div class="bgall" id="petshow">
        <div class="bgcon">
            <div class="bgclosed" id="bgclosed"></div>
            <h2>盛时会员俱乐部需知</h2>
            <div class="guizecon">
                <p>
                    1.什么超级会员体系
 超级会员服务针对2144游戏平台忠实用户的分级服务，玩家成为超级会员后，享有专属的服务特权和游戏特权。
                </p>
                <p>
                    2.如何成为超级会员
当满足以下条件时，即可激活对应的超级会员，联系客服QQ，我们会分配专属大客户经理客服即可享受非一般的特权。
单服单月：
                </p>
                <p>1、单笔充值2000RMB以上；</p>
                <p>2、累计充值3500RMB以上；</p>
                <p>
                    1.什么超级会员体系
 超级会员服务针对2144游戏平台忠实用户的分级服务，玩家成为超级会员后，享有专属的服务特权和游戏特权。
                </p>
                <p>
                    2.如何成为超级会员
当满足以下条件时，即可激活对应的超级会员，联系客服QQ，我们会分配专属大客户经理客服即可享受非一般的特权。
单服单月：
                </p>
                <p>1、单笔充值2000RMB以上；</p>
                <p>2、累计充值3500RMB以上；</p>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    var num = 60;
    var timerrr;
    var flag = true;
    var daojishi = document.getElementById('timeid');
    function dao() {
        if (num > 0) {
            num -= 1;
            daojishi.innerHTML = num;
            if (num < 10) {
                daojishi.innerHTML = "0" + num;
            }
        }
        if (num == 0) {
            daojishi.innerHTML = "验证";
            num = 60;
            flag = true;
            clearInterval(timerrr);
        }

    }
    $("#timeid").click(function () {
        if ($("#txtphone").val().length != 11) {
            $.MsgBox.Alert("盛时", "请输入11位手机号码");
        }
        else if (flag == true) {
            $.ajax({
                type: "POST",
                url: "../Business.ashx?para=SendDX&phone=" + $("#txtphone").val(),
                async: true,
                timeout: 15000,
                dataType: "json",
                success: function (data) {
                    dao();
                    timerrr = setInterval('dao()', 1000);
                    flag = false;
                    $.MsgBox.Alert("盛时", data.message);

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $.MsgBox.Alert("盛时", "发送失败");
                }
            });
        }
    })

    $(function () {
        //if ($.browser.msie) {
        //    $('input:checkbox').click(function () {
        //        this.blur();
        //        this.focus();
        //    });
        //};
        $("#isty").change(function () {
            if ($("#isty").prop("checked") == true) {
                $("#Div1").hide();
                $("#bangding").show();
            }
            else {
                $("#Div1").show();
                $("#bangding").hide();
            }
        });
    });

    $("#bangding").click(function () {
        if ($("#txtphone").val().length != 11) {
            $.MsgBox.Alert("盛时", "请输入11位手机号码");
        }
        else if ($("#txtyzm").val() == "") {
            $.MsgBox.Alert("盛时", "请输入验证码");
        }
        else {
            $.ajax({
                type: "POST",
                url: "../Business.ashx?para=BD2&yzm=" + $("#txtyzm").val() + "&phone=" + $("#txtphone").val(),
                async: true,
                timeout: 15000,
                dataType: "json",
                success: function (data) {
                    if (data.message == "绑定成功") {
                        $.MsgBox.Alert("盛时", "绑定成功！欢迎进入盛时会员俱乐部", function () {
                            //location = "getcard.aspx?cardid=pcIiEjhKodSTiLBTApX3WkjY10HA";
                            location = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/Ka.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect";
                        });
                    } else {
                        $.MsgBox.Alert("盛时", data.message);
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert("提交失败");
                    $.MsgBox.Alert("盛时", "提交失败");
                }
            });
        }
    });
</script>
<script type="text/javascript">
    function shuoming() {
        document.getElementById('petshow').style.display = 'block';
    }
    document.getElementById('bgclosed').onclick = function () {
        document.getElementById('petshow').style.display = 'none';
    }
    function tz() {
        location.href = "http://mp.weixin.qq.com/s?__biz=MzIyMjA1MzQ0MQ==&mid=401084876&idx=1&sn=97af2c17eea741fac4e22e948e5c6bd7#rd";
    }
</script>