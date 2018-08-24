<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Member.aspx.cs" Inherits="Hmj.WebApp.wechat.NewMember.Member" %>

<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/shengshi.css">
    <script src="js/jquery.min.js"></script>
    <script src="js/Message.js"></script>
    <title>会员基本信息</title>
    
      <script type="text/javascript">
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
                <h2>会员基本信息</h2>
            </div>
        </div>

        <div class="regMenber otherinfo">
            <ul>
                <li><span class="kahaoicon" runat="server" id="cardno"><em></em></span></li>
                <li><span class="nameicon" runat="server" id="name"><em></em></span></li>
                <li><span class="Regtel_icon" runat="server" id="phone"><em></em></span></li>
                <!-- <li><span class="nandeicon"><em>先生</em></span></li>-->
                <li id="sex" runat="server"><span class="nvdeicon"><em>女士</em></span></li>
            </ul>

            <p id="bir" runat="server"></p>
            <p id="create" runat="server"></p>
            <p id="guitai" runat="server"></p>
            <p>
                <strong>所在城市</strong> <span><kbd>
                    <asp:DropDownList ID="ddlS" class="dropdown-select" runat="server" onchange="s(this);"></asp:DropDownList></kbd><kbd>
                        <asp:DropDownList ID="ddlC" class="dropdown-select" runat="server">
                            <asp:ListItem>请选择城市</asp:ListItem>
                        </asp:DropDownList></kbd></span>
            </p>
            <p>
                <strong>联系地址</strong> <span>
                     <input name="" id="addval" value="" type="text" placeholder="<%=address %>" class="txtinput">
            </p>
        </div>

        <div class="hudong" id="checkid">
            <h2>互动渠道愿意接受</h2>
            <ul>
                <li><bdo id="radsj" runat="server">
                    <input name="" type="radio" value="手机" class="radiocss"></bdo><bdo>手机</bdo></li>
                <li><bdo id="raddx" runat="server">
                    <input name="" type="radio" value="短信" class="radiocss"></bdo><bdo>短信</bdo></li>
                <li><bdo id="radyj" runat="server">
                    <input name="" type="radio" value="邮件" class="radiocss"></bdo><bdo>邮件</bdo></li>
                <li><bdo id="radlp" runat="server">
                    <input name="" type="radio" value="礼品" class="radiocss"></bdo><bdo>礼品</bdo></li>
                <li><bdo id="radzy" runat="server">
                    <input name="" type="radio" value="直邮" class="radiocss"></bdo><bdo>直邮</bdo></li>
                <li><bdo id="radhd" runat="server">
                    <input name="" type="radio" value="活动" class="radiocss"></bdo><bdo>活动</bdo></li>
            </ul>
        </div>

        <div class="regBtn">
            <div class="submitbtn" id="submit">保存提交</div>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    var danxuan = document.getElementById('checkid').getElementsByTagName('input');
    for (var i = 0; i < danxuan.length; i++) {
        danxuan[i].index = i;
        danxuan[i].flag = true;
        danxuan[i].onclick = function () {
            if (danxuan[this.index].flag == true) {
                danxuan[this.index].className = 'radiocss checked2';
                danxuan[this.index].flag = false;

            } else {
                danxuan[this.index].className = 'radiocss';
                danxuan[this.index].flag = true;
            }
        }
    }
    document.getElementById('submit').onclick = function () {
        var curzhi = "";
        for (var i = 0; i < danxuan.length; i++) {
            if (danxuan[i].className == 'radiocss checked2') {
                curzhi += danxuan[i].value + ',';
            }
        }
        alert(curzhi);
    }
</script>
<script type="text/javascript">

    function s(obj) {
        $.ajax({
            type: "POST",
            url: "Member.aspx?s=" + $(obj).val(),
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


    var danxuan = document.getElementById('checkid').getElementsByTagName('input');
    for (var i = 0; i < danxuan.length; i++) {
        danxuan[i].index = i;
        danxuan[i].flag = true;
        danxuan[i].onclick = function () {
            if (danxuan[this.index].flag == true) {
                danxuan[this.index].className = 'radiocss checked2';
                danxuan[this.index].flag = false;

            } else {
                danxuan[this.index].className = 'radiocss';
                danxuan[this.index].flag = true;
            }

        }
    }
    document.getElementById('submit').onclick = function () {
        var curzhi = "";
        for (var i = 0; i < danxuan.length; i++) {
            if (danxuan[i].className == 'radiocss checked2') {
                curzhi += danxuan[i].value + ',';
            }
        }

        
        var select1 = document.all.<%= ddlS.ClientID %>;
        var selectvalue = select1.options[select1.selectedIndex].value; 
        var selectText= select1.options[select1.selectedIndex].text;


        var select2 = document.all.<%= ddlC.ClientID %>;
        var selectvalue2 = select2.options[select2.selectedIndex].value; 
        var selectText2= select2.options[select2.selectedIndex].text;
        var addval;
        if ($("#addval").val()=="") {
            addval = "<%= address%>";
        }else
        {
            addval = $("#addval").val();
        }

        if (selectvalue=="0"||selectvalue2=="0") {
            $.MsgBox.Alert("盛时", "请选择省份城市");
        }else{
            var urlval = "../Business.ashx?para=Update&cardno=" + "<%= cardnoval%>" + "&name="
                    + "<%= nameval%>" + "&sf=" + selectvalue + "&cs=" 
                    + selectvalue2 + "&add=" + addval+"&sex=" + $("#sexval").html()+"&xz="+curzhi;
            $.ajax({
                type: "POST",
                url:urlval ,
                async: false,
                timeout: 15000,
                dataType: "json",
                success: function (data) {
                    if (data.message == "修改成功") {
                      
                        zhuge.track('wx_C-菜单-我的盛时-会员卡-基本信息-保存提交-点击', {
                            '所有信息': urlval//事件自定义属性
                        }, function(){
                            //location.href = 'http://www.xxx.com/b.html';//执行打点的后续操作,如页面跳转
                        });
                        $.MsgBox.Alert("盛时", "修改个人信息成功", function () {
                            location = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/NewMember/Member.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect";
                        });
                    } else {
                        $.MsgBox.Alert("盛时", data.message);
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(URL);
                    //alert("提交失败");
                    $.MsgBox.Alert("盛时", "提交失败");
                }
            });
        }
    }

    function zhugeclick(msg) {
        zhuge.track(msg);
    }
</script>
