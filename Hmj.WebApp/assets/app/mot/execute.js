
; (function (window, undefined, $) {
    var submiting = false;
    $('#form1').validator({
        rules: {
        },
        fields: {
        },
        valid: function (form) {
            if (submiting) {
                return;
            }
            submiting = true;
            FormSubmit(form, function (res) {
                if (res.status == 0) {
                    _showInfoMessage(res.message, 'success');
                   // alert("操作成功！");
                }
                else {
                    _showInfoMessage(res.message, 'error');
                }
                submiting = false;
            })
        }
    });
    //$("#btnSave").click(function (e) {
    //    $("#form1").submit();
    //});
})(window, undefined, jQuery);