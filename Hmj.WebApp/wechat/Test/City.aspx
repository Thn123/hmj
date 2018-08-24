<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="City.aspx.cs" Inherits="Hmj.WebApp.wechat.Test.City" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8">
<meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" /> 
<meta content="yes" name="apple-mobile-web-app-capable" /> 
<meta content="black" name="apple-mobile-web-app-status-bar-style" />
<meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
<meta name="apple-mobile-web-app-title" content=""><!-- uc强制竖屏 -->
<meta name="screen-orientation" content="portrait"><!-- UC强制全屏 -->
<meta name="full-screen" content="yes"><!-- UC应用模式 -->
<meta name="browsermode" content="application"><!-- QQ强制竖屏 -->
<meta name="x5-orientation" content="portrait"><!-- QQ强制全屏 -->
<meta name="x5-fullscreen" content="true"><!-- QQ应用模式 -->
<meta name="x5-page-mode" content="app">
<meta content="telephone=no" name="format-detection" />
<link type="text/css" rel="stylesheet" href="css/mian.css"/>
<title>我要汤美星</title>
</head>
<body class="cityshopbg">
    <form id="form1" runat="server">
   <div class="quyuSearch">
    <ol class="curbg"><span><select class="city-select" onchange="" id="cityOption"><option>城市</option></select></span></ol>
    <ol><span><select class="city-select" id="areaOption"><option>区域</option></select></span></ol>
    <ol><span><select class="city-select"><option>智能排序</option></select></span></ol>
</div>
 
 <div class="cityList">
    <ul>
       <li>
           <p><strong>Tommee Tippee 仙霞路店</strong></p>
           <p>地址：仙霞路99号尚嘉中心3层318号</p>
           <div class="zuobiao">
              <span>300m</span>
           </div>
       </li>
        <li>
            <p><strong>Tommee Tippee 仙霞路店</strong></p>
            <p>地址：仙霞路99号尚嘉中心3层318号</p>
            <div class="zuobiao">
                <span>430m</span>
            </div>
        </li>
        <li>
            <p><strong>Tommee Tippee 仙霞路店</strong></p>
            <p>地址：仙霞路99号尚嘉中心3层318号</p>
            <div class="zuobiao">
                <span>3620m</span>
            </div>
        </li>
        <li>
            <p><strong>Tommee Tippee 仙霞路店</strong></p>
            <p>地址：仙霞路99号尚嘉中心3层318号</p>
            <div class="zuobiao">
                <span>2130m</span>
            </div>
        </li>
        <li>
            <p><strong>Tommee Tippee 仙霞路店</strong></p>
            <p>地址：仙霞路99号尚嘉中心3层318号</p>
            <div class="zuobiao">
                <span>22230m</span>
            </div>
        </li>
        <li>
            <p><strong>Tommee Tippee 仙霞路店</strong></p>
            <p>地址：仙霞路99号尚嘉中心3层318号</p>
            <div class="zuobiao">
                <span>30m</span>
            </div>
        </li>
        <li>
            <p><strong>Tommee Tippee 仙霞路店</strong></p>
            <p>地址：仙霞路99号尚嘉中心3层318号</p>
            <div class="zuobiao">
                <span>12230m</span>
            </div>
        </li>
    </ul>
 </div>
    </form>
</body>
</html>
<script type="text/javascript" src="js/jquery.1.11.0.min.js"></script>
<script>

    var diqu = [['上海', ['黄浦区', '徐汇区', '长宁区', '静安区', '普陀区', '虹口区']], ['北京', ['朝阳区', '丰台区', '东城区', '西城区', '海淀区', '石景山区']]];
    var cityOption = areaOption = '';
    for (var i = 0; i < diqu.length; i++) {
        cityOption += '<option>' + diqu[i][0] + '</option>';
    }
    $('#cityOption').append(cityOption);


    $('#cityOption').change(function () {
        $('#areaOption').aspx();
        if ($(this).val() != '城市') {
            var cityName = $('#cityOption').val();
            for (var i = 0; i < diqu.length; i++) {
                if (diqu[i][0] == cityName) {
                    var id = i;
                    areaOption = '';
                    for (var j = 0; j < diqu[id][1].length; j++) {
                        areaOption += '<option>' + diqu[id][1][j] + '</option>';
                    }
                }
            }
            $('#areaOption').aspx(areaOption);
        } else {
            alert('请选择城市');
        }
    })


    $('.cityList ul li').on('click', function () {
        $(this).addClass('curbg').siblings().removeClass('curbg');
    })
</script>

