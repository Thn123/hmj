
; (function (window, undefined, $) {
    var submiting = false;
    $('#frmEdit').validator({
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
                    //_showInfoMessage('操作成功！', 'success');
                    if (window.opener) {
                        window.opener.location.reload();
                        window.close();
                    }
                    else {
                        if (parent && parent.xjDailog) {
                            parent.xjDailog.Close(false, true, null);
                        }
                        else {
                            window.close();
                        }
                    }
                }
                else {
                    _showInfoMessage('操作失败！', 'error'); 
                }
                submiting = false;
            })
        }
    });
    $('#startDate').datepicker({
        language: "zh-CN",
        format: "yyyy-mm-dd",
        autoclose: true  
    }).on('changeDate', function (ev) {
        var date = new Date($('#startDate').val());
        $('#endDate').val(formatYYYYMMDD(new Date(date.getFullYear(),date.getMonth()+1,date.getDate())));
    });
    $('#endDate').datepicker({
        language: "zh-CN",
        format: "yyyy-mm-dd",
        autoclose: true
    });

    //保存排班
    $("#btnSave").click(function (e) {
        var start = $("#startDate").val();
        var end = $("#endDate").val();
        if (start == "") {
            _showInfoMessage('请输入开始日期！', 'info');
            return false;;
        }
        if (end == "") {
            _showInfoMessage('请输入结束日期！', 'info');
            return false;
        }
        if (!checkEndTime()) {
            _showInfoMessage('结束日期必须晚于开始日期！', 'info');
            return false;
        }

        var foption = $("#selectedEmployee option:first").attr("value");
        if (foption==""||foption==null)
        {
            _showInfoMessage('请选择要排班的员工！', 'info');
            return false;
        }
        
        //选择日期相差天数
        var days = dateDiff(end, start);
        //选择星期
        var week = $("#WEEK").val();
        //选择班次
        var schedule = $("#SCHEDULE").val();
        var scheduleDate = "";
        var employee = "";
        //计算日期
        for (var i = 0; i <= days; i++)
        {
            var date = AddDays(getDate(start), i);
            //根据日期找出星期几
            var nweek =getDate(date).getDay();
            if (nweek == week)
            {
                scheduleDate += date + ",";
            }
        }
        //选择的星期不在选择的日期当中
        if (scheduleDate == "" || scheduleDate == null) {
            _showInfoMessage('所选星期不在日期范围内！', 'info');
            return false;
        }
        //选择的员工
        $("#selectedEmployee").each(function (i) {
            $(this).children("option").each(function (j) {
                employee += $(this).val()+",";
            });
        });

        //星期对应日期
        $("#SCHEDULE_DATE").val(scheduleDate);
        //班次ID
        $("#SCHEDULE_ID").val(schedule);
        //员工ID
        $("#EMPLOYEE_ID").val(employee);

        $("#frmEdit").submit();
    });
    $("#btnCancel").click(function (e) {
        if (parent && parent.xjDailog) {
            parent.xjDailog.Close(false, false, null);
        }
        else {
            window.close();
        }
    });
    //员工
    $("#selectEmployee").click(function () {
        if ($("#selectEmployee option:selected").length > 0) {
            $("#selectEmployee option:selected").each(function () {
                $("#selectedEmployee").append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option");
                $(this).remove();
            })
        }
        else {
            _showInfoMessage('请选择要排班的员工！', 'info');
            return false;
        }
    });
    //已选择人员
    $("#selectedEmployee").click(function () {
        if ($("#selectedEmployee option:selected").length > 0) {
            $("#selectedEmployee option:selected").each(function () {
                $("#selectEmployee").append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option");
                $(this).remove();
            })
        }
        else {
            _showInfoMessage('请选择要移除的员工！', 'info');
            return false;
        }
    });
    //日期比较
    function checkEndTime() {
        var startTime = $("#startDate").val();
        var start = new Date(startTime.replace("-", "/").replace("-", "/"));
        var endTime = $("#endDate").val();
        var end = new Date(endTime.replace("-", "/").replace("-", "/"));
        if (end < start) {
            return false;
        }
        return true;
    }
    //日期计算
    function dateDiff(d1, d2) {
        var day = 24 * 60 * 60 * 1000;
        try {
            var dateArr = d1.split("-");
            var checkDate = new Date();
            checkDate.setFullYear(dateArr[0], dateArr[1] - 1, dateArr[2]);
            var checkTime = checkDate.getTime();

            var dateArr2 = d2.split("-");
            var checkDate2 = new Date();
            checkDate2.setFullYear(dateArr2[0], dateArr2[1] - 1, dateArr2[2]);
            var checkTime2 = checkDate2.getTime();

            var cha = (checkTime - checkTime2) / day;
            return cha;
        } catch (e) {
            return false;
        }
    }
    //日期加上天数新日期
    function AddDays(date, days) {
        var nd = new Date(date);
        nd = nd.valueOf();
        nd = nd + days * 24 * 60 * 60 * 1000;
        nd = new Date(nd);
        var y = nd.getFullYear();
        var m = nd.getMonth() + 1;
        var d = nd.getDate();
        if (m <= 9) m = "0" + m;
        if (d <= 9) d = "0" + d;
        var cdate = y + "-" + m + "-" + d;
        return cdate;
    }
    //字符串转日期格式
    function getDate(strDate) {
        var date = eval('new Date(' + strDate.replace(/\d+(?=-[^-]+$)/,
        function (a) { return parseInt(a, 10) - 1; }).match(/\d+/g) + ')');
        return date;
    }
    //日期格式化yyyy-MM-dd
    function formatYYYYMMDD(strdate) {
        if (strdate != "" && strdate != null) {
            var date = new Date(strdate);
            var year = date.getFullYear();
            var month = date.getMonth() + 1;
            var day = date.getDate();
            if (month < 10) {
                month = "0" + month;
            }
            if (day < 10) {
                day = "0" + day;
            }
            return year + "-" + month + "-" + day;
        }
        else {
            return "";
        }
    }
    //绑定员工
    $.post(options.eNamelistUrl,null,
               function (ret) {
                   if (ret.status > 0) {
                       var data = eval(ret.data);
                       $(data).each(function (i) {
                           $("#selectEmployee").append("<option value='" + data[i].ID + "'>" + data[i].NAME + "</option>");
                       });
                   }
               },
               "json"
              );
})(window, undefined, jQuery);