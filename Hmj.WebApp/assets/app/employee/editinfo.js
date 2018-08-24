; (function (window, undefined, $) {

    //if ($("#ISADMIN").val() == "True") {
    //    $("#storeDiv").show();
    //    $("#seeStoreDiv").show();        
    //}
    //else {
    //    $("#storeDiv").hide();
    //    $("#seeStoreDiv").hide();
    //} 
    $("#USER_TYPE").val($("#USER_TYPE1").val());

    GetEidtPost();

    var _submiting = false;
    function GetEidtPost() {
        if (_submiting) {
            //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
            return;
        }
        _submiting = true;
        //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

        $.ajax({
            url: options.getEditPost,
            type: "POST",
            data: { post_id: $("#POST_ID").val() },
            success: function (result) {
                hideLoadingMsg();
                if (result.status == 0) {

                    $("#post_ids").html(result.data);

                }
                _submiting = false;
            },
            error: function (result) {
                hideLoadingMsg();
                _submiting = false;
                _showInfoMessage("数据查询失败：" + result, 'error');
            }
        });
    }
    //基础信息保存验证-------------------------------------------------------------------------------
    $("#USER_TYPE").change(SelectUserType);
    SelectUserType();
    var _submiting1 = false;
    //根据“用户类别”获取“区域”的下拉框数据
    function SelectUserType() {
        $.ajax({
            url: options.getUserREGION,
            type: "POST",
            data: { USER_TYPE: $("#USER_TYPE").val(), STORE_ID: $("#STORE_ID1").val() },
            success: function (result) {
                hideLoadingMsg();
                if (result.status == 0) {
                    $("#STORE_ID").html(result.data);
                }
                _submiting1 = false;
            },
            error: function (result) {
                hideLoadingMsg();
                _submiting1 = false;
                _showInfoMessage("数据查询失败：" + result, 'error');
            }
        });
    }

    $("#btnSaveJcxx").click(function (e) {
        $("#POST_ID").val($("#post_ids").val());
        $("#frmJcxx").submit();
    });
    var _submiting2 = false;
    $('#frmJcxx').validator({
        rules: {
        },
        fields: {
            '#NAME': 'required',
            '#GENDER': 'required',
            '#EMPLOYEE_NO': 'required', 
            '#EMP_TYPE': 'required',
            '#IDCARD': 'idcard',
            '#POST_ID': 'required',
            '#ENTRY_DATE_Str': 'required',
            '#STORE_ID': 'required',
            '#post_ids': 'required',
            '#USER_TYPE': 'required',
        },
        valid: function (form) {
            if (_submiting2) {
                showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting2 = true;
            showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            FormSubmit(form, function (res) {
                if (res.status == 0) {
                    if ($("#ID").val() == 0) {
                        $("#ID").val(res.data);
                        $("#ID1").val(res.data);
                        $("#MODELID").val(res.data);
                        $("#jcxx").removeClass("active");
                        $("#yjfa").addClass("active");

                        $("#LiYjfa").html("<a id=\"aYjfa\" href=\"#yjfa\" data-toggle=\"tab\">薪酬与福利</a>");
                        $("#LiDlzh").html("<a href=\"#dlzh\" data-toggle=\"tab\">登录账户</a>");
                        $("#LiJcxx").removeClass("active");
                        $("#LiYjfa").addClass("active");
                        $("#LiDlzh").removeClass("active");
                    }
                    else {
                        _showInfoMessage('修改成功！', 'success');
                        window.location.href = options.indexUrl;
                    }
                    hideLoadingMsg();
                    _submiting2 = false;
                }
                else {
                    hideLoadingMsg();
                    _submiting2 = false;
                    _showInfoMessage('操作失败：' + res.message, 'error');
                }

            })
        }
    });

    $("#STATUS").change(function (e) {
        var st = $("#STATUS").val();
        if (st == 0)
            $("#lzsj").show();
        else $("#lzsj").hide();
    });

    //薪酬方案保存验证-------------------------------------------------------------------------------
    $('#frmXcyfl').validator({
        rules: {

        },
        fields: {
            "#SICK_LEAVE": "digits",
            "#ANNUAL_LEAVE": "digits",
            "#MIN_SALARY1": "number",
            "#MIN_SALARY2": "number",
            "#MIN_SALARY3": "number",
        },
        valid: function (form) {
            FormSubmit(form, function (res) {
                if (res.status == 0) {
                    if ($("#ID1").val() == "") {
                        $("#ID1").val(res.data);
                        $("#yjfa").removeClass("active");
                        $("#dlzh").addClass("active");

                        $("#LiJcxx").removeClass("active");
                        $("#LiYjfa").removeClass("active");
                        $("#LiDlzh").addClass("active");
                    }
                    else {
                        _showInfoMessage('保存成功', 'success');
                        window.location.href = "/employee/index.do";
                        //alert("保存成功！");
                    }
                }
                else {
                    _showInfoMessage("操作失败：" + res.message, 'error');
                    //alert("操作失败！：" + res.message);
                    //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                }
            })
        }
    });

    $("#btnCloseJcxx").click(function (e) {
        window.location.href = options.indexUrl;
    })

    $("#btnCloseXcyfl").click(function (e) {
        window.location.href = options.indexUrl;
    })
     
    $("#btnSaveXcyfl").click(function (e) {

        if ($("#SALARY_TYPE2:checked").val() == 2) {
            var ms = $("#MIN_SALARY1").val();
            if (ms == "") {
                _showInfoMessage("请填写固定小时工资", 'error');
                return false;
            }
            $("#SALARY_TYPEV").val(2);
            $("#MIN_SALARY").val(ms);
        }
        else if ($("#SALARY_TYPE3:checked").val() == 3) {
            var ms = $("#MIN_SALARY2").val();

            if (ms == "") {
                _showInfoMessage("请填写固定工资", 'error');
                return false;
            }
            var ci = $("#COMMISSION_ID1").val();
            if (ci == "") {
                _showInfoMessage("请选择佣金方案", 'error');
                return false;
            }
            $("#SALARY_TYPEV").val(3);
            $("#COMMISSION_ID").val(ci);
            $("#MIN_SALARY").val(ms);

        }
        else if ($("#SALARY_TYPE4:checked").val() == 4) {
            var ms = $("#MIN_SALARY3").val();
            if (ms == "") {
                _showInfoMessage("请填写保底工资", 'error');
                return false;
            }
            var ci = $("#COMMISSION_ID2").val();
            if (ci == "") {
                _showInfoMessage("请选择佣金方案", 'error');
                return false;
            }
            $("#SALARY_TYPEV").val(4);
            $("#COMMISSION_ID").val(ci);
            $("#MIN_SALARY").val(ms);
        }
        else {
            $("#SALARY_TYPEV").val(1);
        }
        $("#frmXcyfl").submit();
    });

    function rdc() {
        if ($("#SALARY_TYPE2:checked").val() == 2) {
            $("#MIN_SALARY2").val("");
            $("#COMMISSION_ID1").val("");
            $("#MIN_SALARY3").val("");
            $("#COMMISSION_ID2").val("");
            setSt(2);
        }
        else if ($("#SALARY_TYPE3:checked").val() == 3) {
            $("#MIN_SALARY1").val("");
            $("#MIN_SALARY3").val("");
            $("#COMMISSION_ID2").val("");
            setSt(3);
        }
        else if ($("#SALARY_TYPE4:checked").val() == 4) {
            $("#MIN_SALARY1").val("");
            $("#MIN_SALARY2").val("");
            $("#COMMISSION_ID1").val("");
            setSt(4);
        }
        else {
            $("#MIN_SALARY1").val("");
            $("#MIN_SALARY2").val("");
            $("#MIN_SALARY3").val("");
            $("#COMMISSION_ID1").val("");
            $("#COMMISSION_ID2").val("");
            setSt(1);
        }

    }
    function setSt(obj) {
        var st = $("#SALARY_TYPEV").val();
        var ms = $("#MIN_SALARY").val();
        var ci = $("#COMMISSION_ID").val();

        if (st == 2 && obj == 2) {
            $("#MIN_SALARY1").val(st);
        }
        else if (st == 3 && obj == 3) {
            $("#MIN_SALARY2").val(ms);
            $("#COMMISSION_ID1").val(ci);
        }
        else if (st == 4 && obj == 4) {
            $("#MIN_SALARY3").val(ms);
            $("#COMMISSION_ID2").val(ci);
        }
    }

    $("#SALARY_TYPE1").click(rdc);
    $("#SALARY_TYPE2").click(rdc);
    $("#SALARY_TYPE3").click(rdc);
    $("#SALARY_TYPE4").click(rdc);
     
    $(document).ready(function () {
        $("#LiJcxx").addClass("active");
        $("#LiYjfa").removeClass("active");
        //$("#LiDlzh").removeClass("active");

        var id = $("#ID").val();
        if (id == 0) {
            $("#LiYjfa").html("<a style=\"cursor:pointer\" onclick=\"_showInfoMessage('请先保存基础信息', 'error');\">薪酬与福利</a>");
        }
        else {
            $("#LiYjfa").html("<a id=\"aYjfa\" href=\"#yjfa\" data-toggle=\"tab\">薪酬与福利</a>");
        }

        $('#ENTRY_DATE_picker').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });

        $('#DISM_DATE_picker').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });
        rdc();

        var status = $("#STATUS").val();
        if (status == 1) {
            $("#lzsj").hide();
        }
        else $("#lzsj").show();


    });
})(window, undefined, jQuery);