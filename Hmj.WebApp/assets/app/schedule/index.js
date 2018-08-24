; (function (window, undefined, $) {

    $(document).ready(function () {

        $('#frmEdit').validator({
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        _showInfoMessage('操作成功！', 'success');
                        $('#confirmModal').modal('hide');
                        //当前日期
                        var bDate = new Date($("#scheduleDate").val());
                        //刷新视图
                        InintView(bDate);
                       
                    }
                    else {
                        _showInfoMessage('操作失败！', 'error');
                        return false;
                    }
                })
            }
        });

        //加载视图
        function InintView(bDate) {
            //yyyy/m/d
            var bStrDate = formatDateString(bDate);
            $("#gridlist #tableShedule thead tr").remove();
            $("#gridlist #tableShedule tbody tr").remove();

            //下月份日期
            var eStrDate = formatDateString(new Date(bDate.getFullYear(), bDate.getMonth() + 1, bDate.getDate()));
            num = dateDiff(eStrDate, bStrDate);

            var html = [];
            html.push('<tr>');
            html.push('<td style="width: 5%;">姓名</td>');
            for (var i = 0; i <= num; i++) {
                var adate = new Date(bDate.getFullYear(), bDate.getMonth(), bDate.getDate() + i);
                // alert(formatWeek(formatDateString(adate)));
                //alert(adate + "%" + formatWeek(formatDateString(adate)));
                html.push('<td class="tEName"><div>' + adate.getDate() + '</div><div>' + formatWeek(formatDateString(adate)) + '</div></td>');
            }
            html.push('</tr>');
            $(html.join("")).appendTo("#gridlist #tableShedule thead");

            //检索员工姓名
            $.post(options.ENameList, { SCHEDULE_DATE: bStrDate },
                    function (ret) {
                        if (ret.status > 0) {
                            var data = eval(ret.data);
                            //排班信息
                            getSchedule(data, bDate, num);
                        }
                    },
                    "json"
                   );
        }

        //当前日期
        var bDate = new Date($("#scheduleDate").val());
        //初始化视图
        InintView(bDate);
        //取得排班信息
        function getSchedule(obj,bDate,nums)
        {
            var bStrDate =formatDateString(bDate);
            $.post(options.listUrl, { SCHEDULE_DATE: bStrDate },
                     function (ret) {
                         if (ret.status > 0) {
                             var data = eval(ret.data);
                             var strHtml = [];
                             for (var i = 0; i < obj.length; i++) {
                                 //循环姓名
                                 strHtml.push('<tr>');
                                 strHtml.push('<td class="tEName" style="">' + obj[i].EMPLOYEE_NAME + '</td>');
                                 var style = "";
                                 //循环日期
                                 for (var n = 0; n <= nums; n++)
                                 {
                                     var adate =formatDateString(new Date(bDate.getFullYear(), bDate.getMonth(), bDate.getDate() + n));
                                     var aStrDate = formatYYYYMMDD(adate);
                                     for (var j = 0; j < data.length; j++)
                                     {
                                         if (aStrDate == formatDate(data[j].SCHEDULEDATE, '') && data[j].EMPLOYEE_ID == obj[i].EMPLOYEE_ID) {
                                             style = ' itemid=' + data[j].ID + "&" + data[j].SCHEDULE_TYPE + "&" + aStrDate + "&" + obj[i].EMPLOYEE_NAME + ' ';
                                             style += ' onclick="eventclick(this)" title=' + data[j].SCHEDULE_NAME + "：" + data[j].BEGINTIME + "-" + data[j].ENDTIME + ' ';
                                             style += ' class="tEvent" style = "background-color:' + data[j].COLOR + '" ';
                                             break;
                                         }
                                         else {
                                             style = "";
                                         }    
                                     }
                                     strHtml.push('<td ' + style + '></td>');
                                 }
                                 strHtml.push('</tr>');
                             }
                             $(strHtml.join("")).appendTo("#gridlist #tableShedule tbody");
                         }
                     },
                     "json"
                    );
        }
        //格式化星期
        function formatWeek(date)
        {
            var mdate = new Date(date);
            var week = mdate.getDay();
            switch (week)
            {
                case 0:week = "日";break;
                case 1:week = "一";break;
                case 2:week = "二";break;
                case 3:week = "三";break;
                case 4:week = "四";break;
                case 5:week = "五";break;
                case 6:week = "六";break;
                default:"";break;
            }
            return week;
        }
        // 返回某个日期对应的月份的天数 
        function GetMonthDayCount(date) {
            switch (date.getMonth() + 1) {
                case 1: case 3: case 5: case 7: case 8: case 10: case 12:
                    return 31;
                case 4: case 6: case 9: case 11:
                    return 30;
            }
            //feb: 
            date = new Date(date);
            var lastd = 28;
            date.setDate(29);
            while (date.getMonth() == 1) {
                lastd++;
                AddDays(date, 1);
            }
            return lastd;
        }
        //日期计算
        function dateDiff(d1, d2) {
            var day = 24 * 60 * 60 * 1000;
            try {
                var dateArr = d1.split("/");
                var checkDate = new Date();
                checkDate.setFullYear(dateArr[0], dateArr[1] - 1, dateArr[2]);
                var checkTime = checkDate.getTime();

                var dateArr2 = d2.split("/");
                var checkDate2 = new Date();
                checkDate2.setFullYear(dateArr2[0], dateArr2[1] - 1, dateArr2[2]);
                var checkTime2 = checkDate2.getTime();

                var cha = (checkTime - checkTime2) / day;
                return cha;
            } catch (e) {
                return false;
            }
        }

        function formatYYYYMMDD(strdate)
        {
            if (strdate != "" && strdate != null) {
                var date = new Date(strdate);
                var year = date.getFullYear();
                var month = date.getMonth() + 1;
                var day = date.getDate();
                if (month < 10) {
                    month = "0" + month;
                }
                if (day < 10)
                {
                    day = "0" + day;
                }
                return year + "/" + month + "/" + day;
            }
            else {
                return "";
            }
        }
        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0]
            }
            else {
                return "-"
            }
        }
        //日期格式化字符串 yyyy/m/d
        function formatDateString(date)
        {
            if (date != "" && date != null) {
                var year = date.getFullYear();
                var month = date.getMonth() + 1;
                var day = date.getDate();
                return year + "/" + month + "/" + day;
            }
            else {
                return "";
            }
        }
        $('#scheduleDate').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd",
            autoclose: true
        }).on('changeDate', function (ev) {     
            InintView(ev.date);
        });

        //单选班次事件
        $("input[name=SCHEDULE_ID]").click(function () {
            InintRadio($(this));
        });

        $("#btnAdd").click(function (e) {
            var url = options.editUrl;
            window.xjDailog.Open(url, {
                width: 600,
                height: 570,
                caption: '排班',
                theme: "simple", //默认主题
                onclose: function (userstate) {
                    var date = new Date($("#scheduleDate").val());
                    //重新加载刷新视图
                    InintView(date);
                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            });
        });

        //修改排班
        $("#btnSave").click(function () {
            $('#BEGINTIME').removeAttr("disabled");
            $('#ENDTIME').removeAttr("disabled");

            $("#frmEdit").submit();
            $('#BEGINTIME').attr("disabled", "true");
            $('#ENDTIME').attr("disabled", "true");
        });
    });
   
})(window, undefined, jQuery);
//单选班次事件
function InintRadio(obj) {
    var date = $(obj).attr("itemid").split('&');
    $("#BEGINTIME").val(date[0]);
    $("#ENDTIME").val(date[1]);

    var title = $(obj).attr("title");
    if (title == "自定义") {
        if ($.trim($("#zdyBeginTime").val()) != "")
        {
            $("#BEGINTIME").val($("#zdyBeginTime").val());
            $("#ENDTIME").val($("#zdyEndTime").val());
        } 
        $('#BEGINTIME').removeAttr("disabled");
        $('#ENDTIME').removeAttr("disabled");
        $('#BEGINTIME').timepicker();
        $('#ENDTIME').timepicker();
    }
    else {  
        $('#BEGINTIME').attr("disabled", "true");
        $('#ENDTIME').attr("disabled", "true");
    }
}

//排班Event单击事件
function eventclick(obj) {
    //EID,SID,DATE,NAME
    if(obj==""||obj==null)
    {
        return false;
    }

    var data = $(obj).attr("itemid").split('&');
    $("#ESchId").val(data[0]);//排班ID
    $("#spanEName").html("&nbsp;&nbsp;[" + data[3] + "-" + data[2] + "]");

    $("#selectedEmployee").each(function (i) {
        $(this).children("option").each(function (j) {
            employee += $(this).val() + ",";
        });
    });

    var objTitle = $(obj).attr("title");
 
    $("input[name=SCHEDULE_TYPE]").each(function (i) {
        if ($(this).val() == data[1]) {
            $(this).prop("checked", 'checked');
            var title = objTitle.split('：')[0];
            var time = objTitle.split('：')[1].split('-');
 
            //InintRadio($(this)); 
            $("#BEGINTIME").val(time[0]);
            $("#ENDTIME").val(time[1]);

            $("#zdyBeginTime").val("");
            $("#zdyEndTime").val("");
            if (title == "自定义") {
                $("#zdyBeginTime").val(time[0]);
                $("#zdyEndTime").val(time[1]);
                $('#BEGINTIME').removeAttr("disabled");
                $('#ENDTIME').removeAttr("disabled");
                $('#BEGINTIME').timepicker();
                $('#ENDTIME').timepicker();
            }
            else {
                $('#BEGINTIME').attr("disabled", "true");
                $('#ENDTIME').attr("disabled", "true");
            }
        }
    });

    // $('#confirmModal').modal('show');
    $('#confirmModal').modal({
        keyboard: false,
        placement:"left"
    });

}



