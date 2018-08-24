
; (function (window, undefined, $) {
    $(document).ready(function () {
        $("#btnSearchOut").click(function () {
            var cardNo = $("#CardNoOut").val();
            var pwd = $("#Password").val();
            if ($.trim(cardNo) == "") {
                _showInfoMessage("请输入要转出的会员卡号！", "info");
                return false;
            }
            //if ($.trim(pwd) == "") {
            //    _showInfoMessage("请输入密码！", "info");
            //    return false;
            //}

            $.post("/Customer/getCard.do", { cardNo: cardNo, pwd: pwd },
                  function (data) {
                      if (data.status > 0) {
                          var json = eval(data.data);
                          $("#outcid").val(json.ID);
                          $("#CUST_NO_OUT").html(json.MOBILE);
                          $("#CUST_NAME_OUT").html(json.NAME);
                          $("#PAR_AMT_OUT").html(formatPrice(json.PAR_AMT));
                          $("#BALANCE_OUT").html(formatPrice(json.BALANCE));
                      }
                      else {
                          $("#outcid").val("0");
                          $("#CUST_NO_OUT").html("");
                          $("#CUST_NAME_OUT").html("");
                          $("#PAR_AMT_OUT").html("");
                          $("#BALANCE_OUT").html("");
                          _showInfoMessage(data.message, "info");
                      }
                  },
                  "json"
                 );

        });

        $("#btnSearchIn").click(function () {
            var cardNo = $("#CardNoIn").val();

            if ($.trim(cardNo) == "") {
                _showInfoMessage("请输入要转入的会员卡号！", "info");
                return false;
            }

            $.post("/Customer/getCard.do", { cardNo: cardNo, pwd: "" },
                  function (data) {
                      if (data.status > 0) {
                          var json = eval(data.data);
                          $("#incid").val(json.ID);
                          $("#CUST_NO_IN").html(json.MOBILE);
                          $("#CUST_NAME_IN").html(json.NAME);
                          $("#PAR_AMT_IN").html(formatPrice(json.PAR_AMT));
                          $("#BALANCE_IN").html(formatPrice(json.BALANCE));
                      }
                      else {
                          $("#incid").val("0");
                          $("#CUST_NO_IN").html("");
                          $("#CUST_NAME_IN").html("");
                          $("#PAR_AMT_IN").html("");
                          $("#BALANCE_IN").html("");
                          _showInfoMessage(data.message, "info");
                      }
                  },
                  "json"
                 );
        });

        $("#btnSave").click(function () {
            var outcid = $("#outcid").val();
            var incid = $("#incid").val();
            if (outcid == "")
            {
                _showInfoMessage("请您先检索转出会员卡号！", "info");
                return false;
            }
            if (incid == "")
            {
                _showInfoMessage("请您先检索转入会员卡号！", "info");
                return false;
            }
            $.post("/Customer/CardTransferSave.do", { outcid: outcid, incid: incid },
              function (data) {
                  if (data.status > 0) {
                      _showInfoMessage("操作成功！", "success");
                      $("#btnSearchOut").click();
                      $("#btnSearchIn").click();
                  }
                  else {
                      _showInfoMessage("操作失败！"+data.message, "error");
                  }
              },
              "json"
             );
        });
        //价格格式化
       function formatPrice(value) {
            value = value.toString().replace(/\$|\,/g, '');
            if (isNaN(value))
                value = "0";
            sign = (value == (value = Math.abs(value)));
            value = Math.floor(value * 100 + 0.50000000001);
            cents = value % 100;
            value = Math.floor(value / 100).toString();
            if (cents < 10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((value.length - (1 + i)) / 3) ; i++)
                value = value.substring(0, value.length - (4 * i + 3)) + ',' +
                value.substring(value.length - (4 * i + 3));
            return (((sign) ? '' : '-') + '' + value);
       };

       $("#btnReturn").click(function () {
           window.location.href = "/Customer/Index.do";
       });

    });
})(window, undefined, jQuery);