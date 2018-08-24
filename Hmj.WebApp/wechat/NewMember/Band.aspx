<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Band.aspx.cs" Inherits="Hmj.WebApp.wechat.NewMember.Band" %>

<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/shengshi.css">
    <script src="js/jquery.min.js"></script>
    <script src="js/Message.js"></script>
    <title>盛时</title>
    <script type="text/javascript">

        var _fromUrl = '<%= this.FromUrl %>';

        window.zhuge = window.zhuge || [];
        window.zhuge.methods = "_init debug identify track trackLink trackForm page".split(" ");
        window.zhuge.factory = function (b) {
            return function () {
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
        window.zhuge.load = function (b, x) {
            if (!document.getElementById("zhuge-js")) {
                var a = document.createElement("script");
                var verDate = new Date();
                var verStr = verDate.getFullYear().toString()
                    + verDate.getMonth().toString() + verDate.getDate().toString();

                a.type = "text/javascript";
                a.id = "zhuge-js";
                a.async = !0;
                a.src = (location.protocol == 'http:' ? "http://www.censh.com/js/zhuge.min.js" : 'https://www.censh.com/js/zhuge.min.js');
                a.onerror = function () {
                    window.zhuge.identify = window.zhuge.track = function (ename, props, callback) {
                        if (callback && Object.prototype.toString.call(callback) === '[object Function]') callback();
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
    <div class="askbox">
        <div class="askboxcon">
            <ul>
                <li><a href="Reg.aspx?from_url=<%= HttpUtility.UrlEncode(this.FromUrl) %>">新用户注册</a></li>
                <li><a href="Band.aspx?from_url=<%= HttpUtility.UrlEncode(this.FromUrl) %>" class="curbtn">老会员绑定</a></li>
            </ul>
        </div>
    </div>

    <div class="regbox">
        <p>
            <bdo class="phone_icon"></bdo><span>
                <input name="" type="tel" id="txtphone" placeholder="请输入11位手机号" class="txtinput1" style="width: 76%; float: left;"><em id="timeid" class="yanzheng">验证</em></span>
        </p>
        <p>
            <bdo class="ma_icon"></bdo><span>
                <input name="" id="txtyzm" type="tel" placeholder="手机收到的验证码" class="txtinput1"></span>
        </p>

        <h3 onclick="tz()">盛时会员俱乐部需知</h3>
    </div>
    <div class="regBtn">
        <div class="submitbtn" id="bangding">立即绑定</div>
    </div>
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
        var mobile = $("#txtphone").val()
        var pattern = /^1[34578]\d{9}$/;
        if (pattern.test(mobile) == false) {
            $.MsgBox.Alert("盛时", "请输入11位手机号码");
            return false;
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
        if (!$(this).hasClass('Onclick')) {
            //样式不存在
            $(this).addClass('Onclick');//添加判断样式
            //setTimeout(function () { $(this).removeClass('Onclick') }, 5000);//延迟5秒后，把判断样式删除。

            if ($("#txtphone").val().length != 11) {
                $.MsgBox.Alert("盛时", "请输入11位手机号码");
                $(this).removeClass('Onclick')
                return false;
            }
            else if ($("#txtyzm").val() == "") {
                $.MsgBox.Alert("盛时", "请输入验证码");
                $(this).removeClass('Onclick')
                return false;
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "../Business.ashx?para=BD2&yzm=" + $("#txtyzm").val() + "&phone=" + $("#txtphone").val(),
                    async: false,
                    timeout: 15000,
                    dataType: "json",
                    beforeSend: function (xhr) {
                    },
                    success: function (data) {
                        if (data.message == "绑定成功") {
                            $.MsgBox.Alert("盛时", "绑定成功！欢迎进入盛时会员俱乐部", function () {
                                //location = "getcard.aspx?cardid=pcIiEjhKodSTiLBTApX3WkjY10HA";
                                var _url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/ka.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect";
                                
                                if (_fromUrl != '') {
                                    _url = _fromUrl;
                                }
                                location.href = _url;
                            });
                        } else {
                            $.MsgBox.Alert("盛时", data.message);
                        }


                    },
                    complete: function (xhr, ts) {
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        //alert("提交失败");
                        $.MsgBox.Alert("盛时", "提交失败");
                    }
                });
            }
            $(this).removeClass('Onclick')
        }
    });
</script>
<script type="text/javascript">
    function tz() {
        location.href = "http://mp.weixin.qq.com/s?__biz=MzIyMjA1MzQ0MQ==&mid=401084876&idx=1&sn=97af2c17eea741fac4e22e948e5c6bd7#rd";
    }
</script>
