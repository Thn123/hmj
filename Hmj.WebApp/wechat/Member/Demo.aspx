<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo.aspx.cs" Inherits="Hmj.WebApp.wechat.Member.Demo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>

    </form>
</body>
</html>
<script src="js/jquery.min.js"></script>
<script src="js/Message.js"></script>
 <script>
     function alertval() {
         $.MsgBox.Alert("盛时", "正在建设中...");
     }
     $(function () {
         $("#btn_to").click(function () {
             var index = $("#rp").val();
             inte.FanTo(index);
         })
     })
     var inte = (function () {
         var n = {};
         n.Index = 0;
         //显示订单
         n.FanTo = function (index) {
             $.ajax({
                 url: "Demo.aspx?s=" + index,
                 async: true,
                 type: "get",
                 success: function (res, textStatus) {
                     var s=res.split('_')
                     $("#rp").val(s[0]);
                     if (s[1]!="") {
                         $("#total").val(s[1]);
                     }
                     
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {

                 },
                 complete: function (XMLHttpRequest, textStatus) {

                 }
             })
         };

         return n;
     })();
    </script>
