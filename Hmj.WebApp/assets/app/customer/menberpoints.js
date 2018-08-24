; (function (window, undefined, $) {

    $(document).ready(function () {

        var submiting = false;
        $('#frmFaCardSave').validator({
            rules: {
            },
            fields: {
                '#CARD_NO': 'required'
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
                        _showInfoMessage("操作失败！" + res.message, "error");
                    }
                    submiting = false;
                })
            }
        });

        $("#btnSave").click(function () {
            $('#frmFaCardSave').submit();
        });
        $("#btnCancel").click(function () {
            if (parent && parent.xjDailog) {
                parent.xjDailog.Close(false, false, null);
            }
            else {
                window.close();
            }
        });
    });

})(window, undefined, jQuery);