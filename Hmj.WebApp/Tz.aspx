<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tz.aspx.cs" Inherits="Hmj.WebApp.Tz" %>

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<title></title>
</head>
<body>
<center>
<span id="totalSecond">3</span>秒后自动返回商铺网页</br>
<span><a href="<%= url%>">手动跳转到商铺网页</a></span>
</center>
</body>
<script language="javascript" type="text/javascript">
    var second = document.getElementById('totalSecond').innerText;
    var int = setInterval("redirect()", 1000);
    function redirect(){
        if (second < 0){
            clearInterval(int);
            location.href = '<%= url%>';
        }else{
            document.getElementById('totalSecond').innerText = second--;
        }
    }
</script>
</html>
