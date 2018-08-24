<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reg.aspx.cs" Inherits="Hmj.WebApp.wechat.Member.Reg" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/mian.css" />
    <script src="js/jquery-1.10.1.min.js"></script>
    <script src="js/Message2.js"></script>
    <title>会员注册</title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="topbox">
            <ul>
                <li><a href="Reg.aspx"><span class="curbtn">新用户注册</span></a></li>
                <li><a href="Band.aspx"><span>老会员绑定</span></a></li>
            </ul>
        </div>

        <div class="regbox">
            <p>
                <bdo class="name_icon"></bdo><span>
                    <input name="" id="name" type="text" placeholder="您的姓名" class="txtinput" /></span>
            </p>
            <p style="padding: 0 10px;">
                <bdo class="xing_icon"></bdo><span>
                    <input name="" type="radio" value="" class="radiocss checked" id="xiansheng" />
                    先生 &nbsp;&nbsp;&nbsp;&nbsp;
                <input name="" type="radio" value="" class="radiocss" id="nvshi" />
                    女士 </span>
            </p>

            <%--            <p>
                <bdo class="sheng_icon"></bdo><span>
                    <input type="date" class="txtinput" placeholder="请输入生日" data-clear-btn="false" name="date-1" id="frmMain_txt_SB_SERVERTIME" value="" /></span>
            </p>--%>
            <p><bdo class="sheng_icon"></bdo><span><strong class="txtinput2"><em id="shuzhi">请输入生日</em><input name="" type="date" class="txtinput22" id="birthday" /></strong></span></p>
            <p>
                <bdo class="phone_icon"></bdo><span>
                    <input id="txtphone" name="" type="tel" placeholder="请输入11位手机号" class="txtinput" style="width: 70%; float: left;" /><em id="timeid" class="yanzheng">验证</em></span>
            </p>
            <p>
                <bdo class="ma_icon"></bdo><span>
                    <input id="txtyzm" name="" type="text" placeholder="手机收到的验证码" class="txtinput" /></span>
            </p>
            <p>
                <bdo class="map_icon"></bdo>
                <span><strong>
                    <asp:DropDownList ID="ddlS" class="dropdown-select" runat="server" onchange="s(this);"></asp:DropDownList>
                </strong><strong>
                    <asp:DropDownList ID="ddlC" class="dropdown-select" runat="server">
                        <asp:ListItem>请选择城市</asp:ListItem>
                    </asp:DropDownList>
                </strong></span>
            </p>
            <h3 onclick="tz()">盛时会员俱乐部需知</h3>
            <div id="reg" class="regbtn">注册绑定</div>
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
    $("#reg").click(function () {
        var select1 = document.all.<%= ddlS.ClientID %>;
        var selectvalue = select1.options[select1.selectedIndex].value; 
        var selectText= select1.options[select1.selectedIndex].text;


        var select2 = document.all.<%= ddlC.ClientID %>;
        var selectvalue2 = select2.options[select2.selectedIndex].value; 
        var selectText2= select2.options[select2.selectedIndex].text;

        if (selectvalue=="0"||selectvalue2=="0") {
            $.MsgBox.Alert("盛时", "请选择省份城市");
        }
        if ($("#name").val() == "") {
            $.MsgBox.Alert("盛时", "请输入姓名");
        }
        else if ($("#txtphone").val().length != 11) {
            $.MsgBox.Alert("盛时", "请输入11位手机号码");
        }
        else if ($("#txtyzm").val() == "") {
            $.MsgBox.Alert("盛时", "请输入验证码");
        }
        else if($("#frmMain_txt_SB_SERVERTIME").val()=="")
        {
            $.MsgBox.Alert("盛时", "请输入生日");
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
            }
            var sex = "Z001";
            if (document.getElementById('xiansheng').className == 'radiocss') {
                sex = "Z002";
            }
            $.ajax({
                type: "POST",
                url: "../Business.ashx?para=Reg&yzm=" + $("#txtyzm").val() + "&phone=" + $("#txtphone").val() + "&name=" + $("#name").val() + "&sf=" + selectvalue + "&city=" + selectvalue2 + "&sex=" + sex+"&sr=" + txDate,
                async: true,
                timeout: 15000,
                dataType: "json",
                success: function (data) {
                    if (data.message == "注册成功") {
                        $.MsgBox.Alert("盛时", "注册成功！欢迎成为盛时会员", function () {
                            location = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/ka.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect";
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
     
    var textbox = document.getElementById('birthday');
    textbox.onfocus = function (event) {
        document.getElementById('shuzhi').innerHTML=""; 
    } 

</script>
