<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApiDemo.aspx.cs" Inherits="Hmj.WebApp.wechat.Member.ApiDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery-1.10.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ssoform").submit();
        });
    </script>
</head>
<body>
    <form action="http://www.censh.com/customer/account/sso" method="post" id="ssoform">
        <input type="hidden" name="callback" value="http://wechat.censh.com/wechat/Member/Callback.aspx" />
        <input type="hidden" name="channel" value="wx" />
        <input type="hidden" name="clear" value="" />
        <input type="hidden" name="redirect" value="" />
        <input type="hidden" name="ticket" id="ticket" runat="server" value="" />
        <input type="hidden" name="unique" id="unique" runat="server" value="" />
        <%--<input type="submit" name="nam" value="单点登录" />--%>
    </form>
</body>
</html>

