; (function (window, undefined, $) {
    $(document).ready(function () {


        $("#btnSave").click(function (e) {
            $("#frmSave").submit();
        });

        $('#frmSave').validator({
            rules: {
            },
            fields: {
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage("操作成功！", 'success');
                        location = "Index.do";
                    }

                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');
                    }
                })
            }
        });


    });
})(window, undefined, jQuery);