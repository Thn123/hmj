<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Member.aspx.cs" Inherits="Hmj.WebApp.wechat.Member.Member" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" rel="stylesheet" href="css/mian.css" />
    <title>会员基本信息</title>
    <script src="js/jquery-1.10.1.min.js"></script>
    <script src="js/Message2.js"></script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="topnav">
            <h2>会员基本信息</h2>
        </div>
        <div class="menber otherinfo">
            <ul>
                <li><span class="kahaoicon" runat="server" id="cardno"><em></em></span></li>
                <li><span class="nameicon" runat="server" id="name"><em></em></span></li>
                <li><span class="telicon" runat="server" id="phone"><em></em></span></li>
                <!-- <li><span class="nandeicon"><em>先生</em></span></li>-->
                <li id="sex" runat="server"><span class="nvdeicon"><em>女士</em></span></li>
            </ul>
            <p id="bir" runat="server"></p>
            <p id="create" runat="server"></p>
            <p id="guitai" runat="server"></p>
        </div>

        <div class="regbox" style="padding: 0">
            <p style="padding: 0 5px 5px;">
                <bdo class="mapding"></bdo><span><strong>
                    <%--                    <select name="" class="dropdown-select">
                        <option>省份</option>
                        <option>上海</option>
                        <option>浙江</option>
                    </select>--%>
                    <asp:DropDownList ID="ddlS" class="dropdown-select" runat="server" onchange="s(this);"></asp:DropDownList>
                </strong><strong>
                    <%--                        <select name="" class="dropdown-select">
                            <option>城市</option>
                            <option>徐汇区</option>
                            <option>上海</option>
                        </select>--%>
                    <asp:DropDownList ID="ddlC" class="dropdown-select" runat="server">
                        <asp:ListItem>请选择城市</asp:ListItem>
                    </asp:DropDownList>
                </strong></span>
            </p>
            <p style="padding: 0 5px 5px;">
                <bdo class="map_icon"></bdo><span>
                    <input name="" id="addval" value="" type="text" placeholder="<%=address %>" class="txtinput"></span>
            </p>
        </div>

        <div class="hudong" id="checkid">
            <h2>互动渠道愿意接受</h2>
            <ul>
                <li><bdo id="radsj" runat="server">
                    <input name="" type="radio" value="手机" class="radiocss"></bdo><bdo class="telicon">手机</bdo></li>
                <li><bdo id="raddx" runat="server">
                    <input name="" type="radio" value="短信" class="radiocss"></bdo><bdo class="duanicon">短信</bdo></li>
                <li><bdo id="radyj" runat="server">
                    <input name="" type="radio" value="邮件" class="radiocss"></bdo><bdo class="emailicon">邮件</bdo></li>
                <li><bdo id="radlp" runat="server">
                    <input name="" type="radio" value="礼品" class="radiocss"></bdo><bdo class="lipinicon">礼品</bdo></li>
                <li><bdo id="radzy" runat="server">
                    <input name="" type="radio" value="直邮" class="radiocss"></bdo><bdo class="zhiyouicon">直邮</bdo></li>
                <li><bdo id="radhd" runat="server">
                    <input name="" type="radio" value="活动" class="radiocss"></bdo><bdo class="huoicon">活动</bdo></li>
            </ul>
        </div>

        <div class="regbtn" id="submit">保存提交</div>
    </form>
</body>
</html>
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
                        $.MsgBox.Alert("盛时", "修改个人信息成功", function () {
                            location = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx32364024f2c86185&redirect_uri=http://wechat.censh.com/wechat/Member/Member.aspx&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect";
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
</script>
