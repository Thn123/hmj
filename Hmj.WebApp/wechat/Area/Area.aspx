<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Area.aspx.cs" Inherits="Hmj.WebApp.wechat.Area.Area" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="initial-scale=1.0,user-scalable=no,maximum-scale=1,width=device-width" name="viewport" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <script src="js/jquery-1.7.2-min.js"></script>
    <title>区域活动</title>
    <style type="text/css">
        * {
            margin: 0;
            padding: 0;
        }

        a:link, a:visited {
            color: #666;
            text-decoration: none;
        }

        a:hover {
            color: #333;
            text-decoration: none;
        }

        .areaList {
            background: #f2f2f2;
            width: 100%;
            max-width: 750px;
            margin: 0 auto;
        }

            .areaList .soubox {
                width: 100%;
                height: 50px;
                border-bottom: 1px solid #ddd;
                display: block;
            }

                .areaList .soubox span {
                    float: left;
                    width: 80%;
                }

                .areaList .soubox .txt {
                    border: 1px solid #ddd;
                    background: url(search_icon.png) no-repeat 10px center #fff;
                    background-size: 15px;
                    border-radius: 20px;
                    height: 30px;
                    -webkit-appearance: none;
                    width: 100%;
                    line-height: 30px;
                    display: block;
                    margin: 10px;
                    text-indent: 30px;
                }

                .areaList .soubox bdo {
                    float: left;
                    width: 20%;
                    text-align: center;
                    font-size: 16px;
                    line-height: 50px;
                }

            .areaList .listbox {
                background: #fff;
                display: block;
            }

                .areaList .listbox ul li {
                    background: url(jiantouRight.png) no-repeat 97% center;
                    background-size: 10px 17px;
                    display: block;
                    font-size: 18px;
                    padding: 10px 12px;
                    line-height: 30px;
                    border-bottom: 1px solid #eee;
                }

                    .areaList .listbox ul li a {
                        display: block;
                        text-align: left;
                        text-decoration: none;
                        color: #333;
                    }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="areaList">
            <div class="soubox">
                <span>
                    <input id="sou" name="" type="text" class="txt"></span>
                <a href="#" onclick="ser()"><bdo>搜索</bdo></a>
            </div>
            <div id="con" class="listbox" runat="server">
            </div>

        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    function ser() {
        var sou = document.getElementById("sou").value;
        $.ajax({
            type: "POST",
            url: "Area.aspx?para=" + sou,
            async: false,
            timeout: 15000,
            dataType: "html",
            success: function (data) {
                $("#con").html(data);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("网络错误");
            }
        });
    }
</script>
