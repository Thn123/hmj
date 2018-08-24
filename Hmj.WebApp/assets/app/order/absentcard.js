; (function (window, undefined, $) {
    $(document).ready(function () {

        var submiting = false;
        $('#formSave').validator({
            valid: function (form) {
                if (submiting) {
                    return;
                }
                submiting = true;
                FormSubmit(form, function (res) {
                    if (res.status > 0) {
                        _showInfoMessage("操作成功！", "success");
                        window.location.href = "/Customer/View360.do?cid=" + $("#hideCUST_ID").val();

                    }
                    else {
                        _showInfoMessage("操作失败！" + res.message, "error");
                    }
                    submiting = false;
                })
            }
        });

        $('#BEGIN_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd",
            autoclose: true
        }).on('changeDate', function (ev) {

        });
        $('#END_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd",
            autoclose: true
        });


        $("#PAYCARD_TYPE").change(function () {

            var prodName = $("#PAYCARD_TYPE").find("option:selected").text();
           
            $("#hidePROD_NAME").val(prodName);
        });

        $("#btnSave").click(function () {

            $("#formSave").submit();
        });
        $("#btnReturn").click(function () {
            window.location.href = "/Customer/View360.do?cid=" + $("#hideCUST_ID").val();
        });
        util.keyPress = function (ob) {
            if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
                ob.value = ob.t_value;
            else ob.t_value = ob.value;
            if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
                ob.o_value = ob.value;
        }
        util.onBlur = function (ob) {
            if (!ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?|\.\d*?)?$/))
                ob.value = ob.o_value;
            else {
                if (ob.value.match(/^\.\d+$/))
                    ob.value = 0 + ob.value;
                if (ob.value.match(/^\.$/))
                    ob.value = 0; ob.o_value = ob.value
            };
        };
        util.keyUpCash = function (ob) {
            if (!ob.value.match(/^[\+\-]?\d*?\.?\d*?$/))
                ob.value = ob.t_value;
            else ob.t_value = ob.value;
            if (ob.value.match(/^(?:[\+\-]?\d+(?:\.\d+)?)?$/))
                ob.o_value = ob.value;
            try {
                var but_amt = $("#spanBalance").text();
                //var vipcard = $("#CARD_ID").val();
                //if (vipcard == "" || vipcard == null) {
                //    _showInfoMessage("请您先选择会员卡级！", "info");
                //    ob.value = 0;
                //    return false;
                //}
                var paycard_type = $("#PAYCARD_TYPE").val();
                if (paycard_type == "" || paycard_type == null) {
                    _showInfoMessage("请您先选择退卡方式！", "info");
                    ob.value = 0;
                    $("#CASH_AMT").val(but_amt);
                    return false;
                }
                if (parseFloat(ob.value) > parseFloat(but_amt)) {
                    _showInfoMessage("退卡金额不能大于卡内余额！", "info");
                    ob.value = but_amt;
                    $("#CASH_AMT").val(0);
                    $("#CARD_CASH_AMT").val(but_amt);
                    return false;
                }
                var cash_amt = parseFloat(but_amt) - parseFloat(ob.value);
                if (!isNaN(cash_amt)) {
                    $("#CASH_AMT").val(cash_amt);
                }

            }
            catch (ex) {
                _showInfoMessage("金额填写有误！" + ex.message, "error");
            }
        };

    });
})(window, undefined, jQuery);
