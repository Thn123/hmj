<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reg.aspx.cs" Inherits="Hmj.WebApp.wechat.NewMember.Reg" %>

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
        window.zhuge.load('90fc349d64e24ca8ac1bd203c490f5f8', {debug:true});
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <div class="askbox">
            <div class="askboxcon">
                <ul>
                    <li><a href="Reg.aspx?from_url=<%= HttpUtility.UrlEncode(this.FromUrl) %>" class="curbtn">新用户注册</a></li>
                    <li><a href="Band.aspx?from_url=<%= HttpUtility.UrlEncode(this.FromUrl) %>">老会员绑定</a></li>
                </ul>
            </div>
        </div>
        <div class="regbox">
            <p>
                <bdo class="name_icon"></bdo><span>
                    <input name="" id="name" type="text" placeholder="您的姓名" class="txtinput1" /></span>
            </p>
            <p style="padding: 0 10px;">
                <bdo class="xing_icon"></bdo><span>
                    <input name="" type="radio" value="" class="radiocss checked" id="xiansheng" />
                    先生 &nbsp;&nbsp;&nbsp;&nbsp;
                <input name="" type="radio" value="" class="radiocss" id="nvshi" />
                    女士 </span>
            </p>
            <p><bdo class="sheng_icon"></bdo><span><strong class="txtinput2"><em id="shuzhi">请输入生日</em><input name="" type="date" class="txtinput22" id="birthday" /></strong></span></p>
            <p>
                <bdo class="phone_icon"></bdo><span>
                    <input id="txtphone" name="" type="tel" placeholder="请输入11位手机号" class="txtinput1" style="width: 70%; float: left;" /><em id="timeid" class="yanzheng">验证</em></span>
            </p>
            <p style="height: 20px; line-height: 14px;">
                <span>
                    <label id="phone_check" style="color: red; margin-left: 40px"></label>
                </span>
            </p>
            <p>
                <bdo class="ma_icon"></bdo><span>
                    <input id="txtyzm" name="" type="tel" placeholder="手机收到的验证码" class="txtinput1" /></span>
            </p>
            <p>
                <bdo class="map_icon"></bdo>
                <span><strong>
                    <asp:DropDownList ID="ddlS" class="dropdown-select" runat="server" onchange="s(this);"></asp:DropDownList>
                </strong><strong>
                    <asp:DropDownList ID="ddlC" class="dropdown-select" runat="server">
                        <asp:ListItem Value="0">请选择城市</asp:ListItem>
                    </asp:DropDownList>
                </strong></span>
            </p>
            <h3 onclick="tz()">盛时会员俱乐部需知</h3>
            <%-- <div id="reg" class="regbtn">注册绑定</div>--%>
        </div>

        <div class="regBtn">
            <div class="submitbtn" id="reg">注册绑定</div>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    document.getElementById('xiansheng').onclick = function () {
        //alert(22);
        document.getElementById('xiansheng').className = 'radiocss checked';
        document.getElementById('nvshi').className = 'radiocss';

    }
    document.getElementById('nvshi').onclick = function () {
        //alert(22);
        document.getElementById('xiansheng').className = 'radiocss';
        document.getElementById('nvshi').className = 'radiocss checked';
    }
    function tz() {
        location.href = "http://mp.weixin.qq.com/s?__biz=MzIyMjA1MzQ0MQ==&mid=401084876&idx=1&sn=97af2c17eea741fac4e22e948e5c6bd7#rd";
    }
    var textbox = document.getElementById('birthday');
    textbox.onfocus = function (event) {
        document.getElementById('shuzhi').innerHTML = "";
    }

</script>
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
    $("#txtphone").blur(function(){
        var mobile = $("#txtphone").val()
        $.ajax({
            type: "POST",
            url: "../Business.ashx?para=CheckOld&phone=" + $("#txtphone").val(),
            async: true,
            timeout: 15000,
            dataType: "json",              
            success: function (data) {
                $("#phone_check").text(data.message);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                // $.MsgBox.Alert("盛时", "发送失败");
            }
        });
    })
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
    $("#reg").click(function () {
        if (!$(this).hasClass('Onclick')) {
            //样式不存在
            $(this).addClass('Onclick');//添加判断样式
            var select1 = document.all.<%= ddlS.ClientID %>;
            var selectvalue = select1.options[select1.selectedIndex].value; 
            var selectText= select1.options[select1.selectedIndex].text;


            var select2 = document.all.<%= ddlC.ClientID %>;
            var selectvalue2 = select2.options[select2.selectedIndex].value; 
            var selectText2= select2.options[select2.selectedIndex].text;

            if (selectvalue=="0"||selectvalue2=="0") {
                $.MsgBox.Alert("盛时", "请选择省份城市");
                $(this).removeClass('Onclick')
                return false
            }
            else if ($("#name").val() == "") {
                $.MsgBox.Alert("盛时", "请输入姓名");
                $(this).removeClass('Onclick')
                return false
            }
            else if ($("#txtphone").val().length != 11) {
                $.MsgBox.Alert("盛时", "请输入11位手机号码");
                $(this).removeClass('Onclick')
                return false
            }
            else if ($("#txtyzm").val() == "") {
                $.MsgBox.Alert("盛时", "请输入验证码");
                $(this).removeClass('Onclick')
                return false
            }
            else if($("#birthday").val()=="")
            {
                $.MsgBox.Alert("盛时", "请输入生日");
                $(this).removeClass('Onclick')
                return false
            }
            else {
                var bir = $("#birthday").val();
                var txDate = bir.substr(0,4)+ bir.substr(5,2) + bir.substr(8,2);
                //alert (txDate);
                var myDate = new Date();
                var year = myDate.getFullYear().toString();
                var mon = "";
                if (myDate.getMonth()+1<10) {
                    mon = "0"+(myDate.getMonth()+1).toString();
                }else
                {
                    mon = (myDate.getMonth()+1).toString();
                }
                var day = "";
                if (myDate.getDate()<10) {
                    day = "0"+(myDate.getDate()).toString();
                }else
                {
                    day = (myDate.getDate()).toString();
                }
                var newmyDate = year+mon+day;
                if (txDate > newmyDate) {
                    $.MsgBox.Alert("盛时", "生日不可超过当前日期");
                    $(this).removeClass('Onclick')
                    return false
                }
                var sex = "Z001";
                if (document.getElementById('xiansheng').className == 'radiocss') {
                    sex = "Z002";
                }
                $.ajax({
                    type: "POST",
                    url: "../Business.ashx?para=Reg&yzm=" + $("#txtyzm").val() + "&phone=" + $("#txtphone").val() + "&name=" + $("#name").val() + "&sf=" + selectvalue + "&city=" + selectvalue2 + "&sex=" + sex+"&sr=" + txDate,
                    async: false,
                    timeout: 15000,
                    dataType: "json",
                    beforeSend: function(xhr)
                    {
                        $("#reg").prop("disabled", true);
                    },
                    success: function (data) {
                        if (data.message == "注册成功") {
                            $.MsgBox.Alert("盛时", "注册成功！欢迎成为盛时会员", function () {
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
                    complete: function(xhr,ts)
                    {
                        $("#reg").prop("disabled", false);
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

    document.getElementById('xiansheng').onclick = function () {
        //alert(22);
        document.getElementById('xiansheng').className = 'radiocss checked';
        document.getElementById('nvshi').className = 'radiocss';

    }
    document.getElementById('nvshi').onclick = function () {
        //alert(22);
        document.getElementById('xiansheng').className = 'radiocss';
        document.getElementById('nvshi').className = 'radiocss checked';

    }
    function s(obj) {
        $.ajax({
            type: "POST",
            url: "reg.aspx?s=" + $(obj).val(),
            async: false,
            timeout: 15000,
            dataType: "html",
            success: function (data) {
                // alert(data);
                $("#ddlC").html(data);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $.MsgBox.Alert("盛时", "加载失败");
            }
        });
    }
</script>
