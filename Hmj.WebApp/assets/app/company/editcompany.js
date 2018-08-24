
; (function (window, undefined, $) {

    $(document).ready(function () {
        var _submiting = false;
        $('#frmEdit').validator({
            rules: {
            },
            fields: {
                '#ORG_NO': 'required',
                '#ORG_NAME': 'required',
                '#CONTACT': 'required',
                '#USER_NO': 'required',
                '#USER_PASS': 'required',
                '#USER_PASS1': 'required',
            },
            valid: function (form) {
                if (_submiting) {
                    showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                    return;
                }
                _submiting = true;
                showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage("操作成功！", 'success');
                        //alert("操作成功！");
                    }

                    else {
                        _showInfoMessage("操作失败！：" + res.message, 'error');
                        //alert("操作失败！：" + res.message);
                        //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                    }
                    hideLoadingMsg();
                    _submiting = false;
                })
            }
        });
        function tv() {
            $("#usertree").treeview(
                      {
                          url: options.getTreeUrl + '?orgId=' + ($("ID").val() != undefined ? $("ID").val() : 0),
                          showcheck: true,
                          cascadecheck: true,
                          onnodeclick: TreeNode_Click,
                          theme: "bbit-tree-no-lines" //bbit-tree-lines ,bbit-tree-no-lines,bbit-tree-arrows
                      }
             );
        }
        function TreeNode_Click(data) {
            data.expand();
        }

        tv();
        $("#btnSave").click(function () {
            if ($("#USER_PASS").val() != $("#USER_PASS1").val()) {
                _showInfoMessage("密码不一致", 'error');
                //alert("密码不一致！");
                return false;
            }
            var teeSd = $("#usertree").getTSVs();
            $("#AddIDS").val(teeSd);
            $('#frmEdit').submit();
        });

        $("#btnCancel").click(function () {
            window.location.href = options.indexUrl;
        });

    });
})(window, undefined, jQuery);