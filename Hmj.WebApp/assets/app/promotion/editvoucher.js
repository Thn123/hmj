; (function (window, undefined, $) {
    $(document).ready(function () {

        //保存验证
        $('#frmSave').validator({
            rules: {
            },
            fields: {
                '#STATUS': 'required',
                '#NAME': 'required',
                '#CUST_GROUP_ID': 'required',
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        if ($("#ID").val() == 0) {
                            $("#ID").val(res.data); 
                            _showInfoMessage("保存成功！", 'success');
                            var cuT = $("#TYPE").val();
                            window.location.href = options.indexUrl + "/" + cuT;
                        }
                        else {
                            _showInfoMessage("修改成功！", 'success');
                           
                        }
                    }
                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');  
                        //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                    }
                })
            }
        });

        //保存
        $('#btnSaveFrm').click(function () {
            if ($("#STATUS:checked").val() == undefined) {
                _showInfoMessage("请选择状态！", 'error');
                
                return;
            }

            if ($("#VOUCHER_MONEY").val() == "") {
                _showInfoMessage("请选择代金券面值！", 'error');  
                return;
            }
            if (!_validateResult) {
                alert(_validateMsg);                
                return;
            }
            $('#frmSave').submit();
        });

        //取消
        $('#btnCloseFrm').click(function () {
            var cuT = $("#TYPE").val();
            window.location.href = options.indexUrl + "/" + cuT;

        });

        //顾客选择
        $("#CUST_GROUP_ID").change(function () {
            validateMoney();
        });

        //面值选择
        $("#VOUCHER_MONEY").change(function () {
            validateMoney();
        });
        validateMoney();
        //代金券验证方法
        var _validateResult = false;
        var _validateMsg = "";
        var _submiting = false;
        function validateMoney() {

            if (_submiting) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            $.ajax({
                url: options.validateUrl,
                type: "POST",
                data: { "cgi": $("#CUST_GROUP_ID").val(), "vm": $("#VOUCHER_MONEY").val() },
                success: function (result) {
                    var msgStr = result.data;
                    var msgs = msgStr.split("_")
                    if (msgs[0] != "") {
                        $("#cgiDiv").html(msgs[0]);
                    }
                   
                    if (result.status == 0) {
                        _validateResult = false;
                        _validateMsg = "代金券不够，还剩 " + msgs[1] + " 张";
                        alert("代金券不够，还剩 " + msgs[1] + " 张");
                    }
                    else if (result.status == -1) {
                        _validateMsg = "代金券不够";
                        _validateResult = false;
                    }
                    else {
                        _validateResult = true;
                        _validateMsg = "";
                    }
                    _submiting = false;
                },
                error: function (result) {
                    //hideLoadingMsg();
                    _submiting = false;
                    _showInfoMessage("页面有异常：" + result, 'error');
                    
                }
            });
        }
    });
})(window, undefined, jQuery);