<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="daohang.aspx.cs" Inherits="Hmj.WebApp.wechat.Area.daohang" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width,minimum-scale=1.0" name="viewport" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <link type="text/css" href="css/index.css" rel="stylesheet" />
    <title>活动查询</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="daohang">

            <p>选择城市查询该城市区域活动</p>
            <div class="carbtn" style="margin-bottom: 0; padding-bottom: 0;">
                <ol style="text-align: left;">
                    <h5>省份</h5>
                    <div class="daohangli selecticon" style="width: 90%; display: none;">
                        <asp:DropDownList ID="DropDownList3" runat="server" name=""
                            class="dropdown-select" AutoPostBack="True"
                            OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="daohangli selecticon">
                        <asp:DropDownList ID="DropDownList1" class="dropdown-select"
                            runat="server" onchange="show();">
                            <asp:ListItem>请选择城市</asp:ListItem>
                            <asp:ListItem>上海</asp:ListItem>
                            <asp:ListItem>北京</asp:ListItem>
                            <asp:ListItem>成都</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </ol>
                <ol style="text-align: left;">
                    <h5>城市</h5>
                    <div class="daohangli selecticon">
                        <asp:DropDownList ID="DropDownList2" runat="server" class="dropdown-select"
                            onchange="show2();">
                            <asp:ListItem>请选择城市</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </ol>
            </div>
        </div>
        <asp:Button ID="Button4" class="btn" Width="100%" Style="display: none;" runat="server" Text="" OnClick="Button4_Click" />
        <asp:Button ID="Button5" class="btn" Width="100%" Style="display: none;" runat="server" Text="" OnClick="Button5_Click" />
        <asp:Button ID="Button3" Style="display: none;" runat="server" Text="变化" class="yanzheng"
            OnClick="Button3_Click" />
        <style type="text/css">
            html, body {
                margin: 0;
                padding: 0;
            }

            .iw_poi_title {
                color: #CC5522;
                font-size: 14px;
                font-weight: bold;
                overflow: hidden;
                padding-right: 13px;
                white-space: nowrap;
            }

            .iw_poi_content {
                font: 12px arial,sans-serif;
                overflow: visible;
                padding-top: 4px;
                white-space: -moz-pre-wrap;
                word-wrap: break-word;
            }
        </style>
        <script type="text/javascript" src="http://api.map.baidu.com/api?key=&v=1.1&services=true"></script>
        <!--百度地图容器-->
        <div style="width: 320px; height: 200px; margin: 0 auto; display: none;" id="dituContent"></div>

        <script type="text/javascript">
            //创建和初始化地图函数：
            function initMap() {
                createMap(); //创建地图
                setMapEvent(); //设置地图事件
                addMapControl(); //向地图添加控件
                addMarker(); //向地图中添加marker
            }

            //创建地图函数：


            //地图事件设置函数：
            function setMapEvent() {
                map.enableDragging(); //启用地图拖拽事件，默认启用(可不写)
                map.enableScrollWheelZoom(); //启用地图滚轮放大缩小
                map.enableDoubleClickZoom(); //启用鼠标双击放大，默认启用(可不写)
                map.enableKeyboard(); //启用键盘上下左右键移动地图
            }

            //地图控件添加函数：
            function addMapControl() {
                //向地图中添加缩放控件
                var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_ZOOM });
                map.addControl(ctrl_nav);
            }

            //标注点数组

            //创建marker
            function addMarker() {
                for (var i = 0; i < markerArr.length; i++) {
                    var json = markerArr[i];
                    var p0 = json.point.split("|")[0];
                    var p1 = json.point.split("|")[1];
                    var point = new BMap.Point(p0, p1);
                    var iconImg = createIcon(json.icon);
                    var marker = new BMap.Marker(point, { icon: iconImg });
                    var iw = createInfoWindow(i);
                    var label = new BMap.Label(json.title, { "offset": new BMap.Size(json.icon.lb - json.icon.x + 10, -20) });
                    marker.setLabel(label);
                    map.addOverlay(marker);
                    label.setStyle({
                        borderColor: "#808080",
                        color: "#333",
                        cursor: "pointer"
                    });

                    (function () {
                        var index = i;
                        var _iw = createInfoWindow(i);
                        var _marker = marker;
                        _marker.addEventListener("click", function () {
                            this.openInfoWindow(_iw);
                        });
                        _iw.addEventListener("open", function () {
                            _marker.getLabel().hide();
                        })
                        _iw.addEventListener("close", function () {
                            _marker.getLabel().show();
                        })
                        label.addEventListener("click", function () {
                            _marker.openInfoWindow(_iw);
                        })
                        if (!!json.isOpen) {
                            label.hide();
                            _marker.openInfoWindow(_iw);
                        }
                    })()
                }
            }
            //创建InfoWindow
            function createInfoWindow(i) {
                var json = markerArr[i];
                var iw = new BMap.InfoWindow("<b class='iw_poi_title' title='" + json.title + "'>" + json.title + "</b><div class='iw_poi_content'>" + json.content + "</div>");
                return iw;
            }
            //创建一个Icon
            function createIcon(json) {
                var icon = new BMap.Icon("http://app.baidu.com/map/images/us_mk_icon.png", new BMap.Size(json.w, json.h), { imageOffset: new BMap.Size(-json.l, -json.t), infoWindowOffset: new BMap.Size(json.lb + 5, 1), offset: new BMap.Size(json.x, json.h) })
                return icon;
            }

            initMap(); //创建和初始化地图

            function show() {
                document.getElementById("Button3").click();

            }

            function show2() {
                document.getElementById("Button4").click();

            }

            function show4() {
                document.getElementById("Button5").click();
            }

        </script>



        <div class="zhuangulist" runat="server" id="se">
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    var odiv = document.getElementById('se').getElementsByTagName('div');
    for (var i = 0; i < odiv.length; i++) {
        odiv[i].index = i;
        odiv[i].onclick = function () {
            for (var j = 0; j < odiv.length; j++) {
                odiv[j].className = "nordiv";
            }

            odiv[this.index].className = "curdiv";
        }
    }
</script>
