
; (function (window, undefined, $) {
    var submiting = false;
    $('#frmEdit').validator({
        rules: {
        // 自定义规则，如果通过返回true，否则返回错误消息
            cusidcard: function (element, param, field) {
                var idtype = $("#IDTYPE").val();
                if (idtype == param[0]) {
                    return /^.+$/.test($(element).val()) || "必填项";
                }
                return true;
            }
        },
        fields: {
            '#CUST_NO': 'required',
            '#NAME': 'required',
            //'#IDCARD': 'idcard',
            '#MOBILE': 'required',
            '#EMAIL': 'email',
        },
        valid: function (form) {
            if (submiting) {
                return;
            }
            submiting = true;
            FormSubmit(form, function (res) {
                if (res.status == 0) {
                    //alert("操作成功！");
                    _showInfoMessage("操作成功！", 'success');
                    var flag=$("#flag").val();

                    if (flag == "1") {
                        window.location.href = "/Customer/Index.do";
                    }
                  
                }
                else {
                    showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                }
                submiting = false;
            })
        }
    });
    $('#birthday_picker').datepicker({
        language: "zh-CN",
        format: "yyyy-mm-dd"
    });
    $("#IDCARD").blur(function(e){
        //身份证？可推算出性别和生日
        var v = this.value;
        if (/^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$/.test(v))
        {
            var lastchar = v.substr(v.length - 1);
            if(lastchar=="X" || lastchar=="x")
            {
                $("#GENDER")[0].checked = true;
            }
            else
            {
                var lcInt = parseInt(lastchar);
                if((lcInt %2) ==0)
                {
                    $("#GENDER")[0].checked = true;
                }
                else
                {
                    $("#GENDER")[1].checked = true;
                }
            }
        }
    });
    $("#btnSave").click(function (e) {
        $("#frmEdit").submit();
    });
    $("#btnCancel").click(function (e) {
        //if (parent && parent.xjDailog) {
        //    parent.xjDailog.Close(false, false, null);
        //}
        //else {
        //    window.close();
        //}
    });
    $("#btnClose").click(function () {
        window.location.href = "/Customer/Index.do";
    });

    if ($("#SOURCE").val() == "1") {
        $("#show").show();
    }
    else {
        $("#show").hide();
    }

    $("#SOURCE").change(function () {
        var v = $("#SOURCE").val();
        if (v == "1") {
            $("#show").show();

        }
        else {
            $("#show").hide();
        }
    })
})(window, undefined, jQuery);