
; (function (window, undefined, $) {
    $(document).ready(function () {
       
        $("#btnSearch").click(function () {
            Inint();
        });


        $("#btnSave").click(function () {
            var outcid = $("#outcid").val();
            var incid = $("#incid").val();
            $.post("/Customer/DailyCheckSave.do", { outcid: outcid, incid: incid },
              function (data) {
                  if (data.status > 0) {
                      _showInfoMessage("操作成功！", "success");
                  }
                  else {
                      _showInfoMessage("操作失败！" + data.message, "error");
                  }
              },
              "json"
             );
        });
   

        $("#btnReturn").click(function () {
            window.location.href = "/Customer/Index.do";
        });

        $('#startDate').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd",
            autoclose: true
        }).on('hide', function (ev) {
            var date = $('#startDate').val() + " 05:00";
            $('#startDate').val(date)
        });
        $('#endDate').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd",
            autoclose: true
        }).on('hide', function (ev) {
            var date = $('#endDate').val() + " 05:00";
            $('#endDate').val(date)
        });

        Inint();
        function Inint() {
            var storeId = $("#STORE_Id").val();
            var startDate = $("#startDate").val();
            var endDate = $("#endDate").val();
            $.post("/Customer/DailyIncome.do", { storeId: storeId, sDate: startDate, eDate: endDate },
                     function (data) {
                         if (data.status > 0) {
                             var json = eval(data.data);
                             $("#divTable").html("");
                             var html = [];
                             html.push("<table>");
                             $(json).each(function () {
                                 if (this.PAY_AMT == "" || this.PAY_AMT == null) {
                                     html.push('<tr style="background-color:#e6e5e5">');
                                     html.push('<td colspan="2"><h4>' + this.PAYMENT_TYPE + '</h4></td>');
                                     html.push('</tr>');

                                 }
                                 else {
                                     html.push('<tr>');
                                     html.push('<td style="width:50%">' + this.PAYMENT_TYPE + '</td>');
                                     html.push('<td>' + this.PAY_AMT + '</td>');
                                     html.push('</tr>');
                                 }
                             });
                             html.push("</table>");
                             $(html.join('')).appendTo("#divTable");
                         }
                     },
                     "json"
                    );
        }

    });
})(window, undefined, jQuery);