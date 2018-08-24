; (function (window, undefined, $) {

    $(document).ready(function () {
        var submiting = false;
        $('#frmMakeUpCardSave').validator({
            rules: {
            },
            fields: {
                //'#CARD_NO': 'required'
            },
            valid: function (form) {
                if (submiting) {
                    return;
                }
                submiting = true;
                FormSubmit(form, function (res) {
                    if (res.status > 0) {
                        _showInfoMessage("操作成功！", "success");
                        this.parent.window.location.href = "/Customer/view360.do?cid=" + $("#CUST_ID").val();
                    }
                    else {
                        _showInfoMessage("操作失败！"+res.message, "error");
                    }
                    submiting = false;
                })
            }
        });


        $("#btnCancel").click(function () {
            if (parent && parent.xjDailog) {
                parent.xjDailog.Close(false, false, null);
            }
            else {
                window.close();
            }
        });

        //补卡
        $("#btnReissue").click(function () {
            //if ($.trim($("#OLD_PASSWORD").val()) == "") {
            //    _showInfoMessage("请输入旧密码！", "info");
            //    return false;
            //}
            if ($.trim($("#CARD_NO").val()) == "") {
                _showInfoMessage("请输入新卡号！", "info");
                return false;
            }
            //if ($.trim($("#PASSWORD").val()) == "") {
            //    _showInfoMessage("请输入新密码！", "info");
            //    return false;
            //}
            $("#hideStatus").val(3);
            $('#frmMakeUpCardSave').submit();
        });

        //解冻
        $("#btnThaw").click(function (e) {
            $("#lbcaozuo").html("解冻");
            $('#confirmModal').modal('show');
            $("#hideStatus").val(2);
            //$('#frmMakeUpCardSave').submit();
        });

        //挂失
        $("#btnLost").click(function () {
            $("#lbcaozuo").html("挂失");
            $('#confirmModal').modal('show');
            $("#hideStatus").val(1);
            //$('#frmMakeUpCardSave').submit();
        });
        //进行处理
        $("#btnConfirm").click(function () {
            $('#frmMakeUpCardSave').submit();
        });

    });

})(window, undefined, jQuery);