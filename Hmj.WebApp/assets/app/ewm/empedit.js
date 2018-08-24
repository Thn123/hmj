; (function (window, undefined, $) {
    $(document).ready(function () {

        $("#ptree").treeview({
            url: options.queryLeftUrl,
            showcheck: true,
            cascadecheck: true,
            onnodeclick: nodeClick,
        });

        function nodeClick(data) {
            $("#DeptName").val(data.text);
            $("#DeptId").val(data.value);
            $("#ParentDeptId").val(data.pid);
            $("#store").html("<font color='red'>" + data.text+"</font>");
        }

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
                        location = "employee.do";
                    }

                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');
                    }
                })
            }
        });
    });
})(window, undefined, jQuery);