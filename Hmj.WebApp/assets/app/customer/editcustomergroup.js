; (function (window, undefined, $) {
    $(document).ready(function () {

        var _submiting = false;
        //保存验证
        $('#frmSave').validator({
            rules: {
            },
            fields: {
                //'#STATUS': 'required',
                //'#CHANNEL': 'required',
                '#NAME': 'required',
                //'#DISCOUNT_RATE': 'required',
                //'#CUST_GROUP_ID': 'required',
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
                        if ($("#ID").val() == 0) {
                            _showInfoMessage("保存成功", 'success');
                            $("#ID").val(res.data);
                            window.location.href = options.indexUrl;
                        }
                        else {
                            _showInfoMessage("修改成功", 'success');
                            window.location.href = options.indexUrl;
                        }
                    }
                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');
                        //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                    }
                    hideLoadingMsg();
                    _submiting = false;
                })
            }
        });


        //保存
        $('#btnSaveFrm').click(function () {
            $('#frmSave').submit();
        });

        //筛选条件选择改变
        $("#CUST_GROUP_CONDITION").change(function () {
            SetConditionTitle();
        });
        //设置查询条件的title
        function SetConditionTitle() {

            var titlesHtml = [];

            var conditionHtml = [];

            var cuV = $("#CUST_GROUP_CONDITION").val();

            if (cuV == 1) {
                titlesHtml.push("<div class=\"col-sm-4\">");
                titlesHtml.push("<label id=\"c2\" class=\"control-label\" style=\"text-align: left\">属性</label>");
                titlesHtml.push("</div>");
                titlesHtml.push("<div class=\"col-sm-4\">");
                titlesHtml.push("<label id=\"c3\" class=\"control-label\" style=\"text-align: left\">条件</label>");
                titlesHtml.push("</div>");
                titlesHtml.push("<div class=\"col-sm-4\">");
                titlesHtml.push("<label id=\"c4\" class=\"control-label\" style=\"text-align: left\">值</label>");
                titlesHtml.push("</div>");

                conditionHtml.push("<div class=\"col-sm-4\" id=\"conDiv1\">");
                conditionHtml.push("</div>");
                conditionHtml.push("<div class=\"col-sm-4\" id=\"conDiv2\">");
                conditionHtml.push("</div>");
                conditionHtml.push("<div class=\"col-sm-4\" id=\"conDiv3\">");
                conditionHtml.push("</div>");
            }
            else if (cuV == 2) {
                titlesHtml.push("<div class=\"col-sm-4\">");
                titlesHtml.push("<label id=\"c2\" class=\"control-label\" style=\"text-align: left\">消费时间</label>");
                titlesHtml.push("</div>");
                titlesHtml.push("<div class=\"col-sm-4\">");
                titlesHtml.push("<label id=\"c3\" class=\"control-label\" style=\"text-align: left\">消费次数</label>");
                titlesHtml.push("</div>");
                titlesHtml.push("<div class=\"col-sm-4\">");
                titlesHtml.push("<label id=\"c4\" class=\"control-label\" style=\"text-align: left\">值</label>");


                conditionHtml.push("<div class=\"col-sm-4\" id=\"conDiv1\">");
                conditionHtml.push("</div>");
                conditionHtml.push("<div class=\"col-sm-4\" id=\"conDiv2\">");
                conditionHtml.push("</div>");
                conditionHtml.push("<div class=\"col-sm-4\" id=\"conDiv3\">");
                conditionHtml.push("</div>");

            }
            else if (cuV == 3) {
                titlesHtml.push("<div class=\"col-sm-4\">");
                titlesHtml.push("<label id=\"c2\" class=\"control-label\" style=\"text-align: left\">媒体类型</label>");
                titlesHtml.push("</div>");
                titlesHtml.push("<div class=\"col-sm-4\">");
                titlesHtml.push("<label id=\"c3\" class=\"control-label\" style=\"text-align: left\">活动名称</label>");
                titlesHtml.push("</div>");
                titlesHtml.push("<div class=\"col-sm-4\">");
                titlesHtml.push("<label id=\"c4\" class=\"control-label\" style=\"text-align: left\">是否参与</label>");


                conditionHtml.push("<div class=\"col-sm-4\" id=\"conDiv1\">");
                conditionHtml.push("</div>");
                conditionHtml.push("<div class=\"col-sm-4\" id=\"conDiv2\">");
                conditionHtml.push("</div>");
                conditionHtml.push("<div class=\"col-sm-4\" id=\"conDiv3\">");
                conditionHtml.push("</div>");
            }

            $("#conditionTitle").html("");
            $("#conditionTitle").html(titlesHtml.join(""));

            $("#conditionValue").html("");
            $("#conditionValue").html(conditionHtml.join(""));


            SetCondition(cuV);
        }

        SetConditionTitle();

        //显示查询条件
        //function showGroupCondition() {
        //    var Group_Id = $("#ID").val();
        //    var STRSQL = $("#STRSQL").val();

        //}

        //根据大类得到条件
        function SetCondition(type) {
            $.ajax({
                url: options.getConditions,
                type: "POST",
                data: { "type": type },
                success: function (result) {
                    //基本条件
                    if (type == 1) {

                        var con1 = "<select class=\"form-control\" id=\"Condition1\" name=\"Condition1\">";
                        var dts = result.data;

                        for (var i = 0; i < dts.length; i++) {
                            if (dts[i].Code == "1") {
                                con1 += "<option value=\"" + dts[i].Value + "\">" + dts[i].Name + "</option>";
                            }
                        }
                        con1 += "</select>";

                        $("#conDiv1").html(con1);

                        SetChildrenCondition(1, "GENDER", 1);//初始化第一个选项条件

                        $("#Condition1").change(function () {
                            SetChildrenCondition(1, $("#Condition1").val(), 1);
                        });

                    }
                    else if (type == 2)//消费状况
                    {
                        var con1 = "<select class=\"form-control\" id=\"Condition1\" name=\"Condition1\">";
                        var con2 = "<select class=\"form-control\" id=\"Condition2\" name=\"Condition2\">";

                        var dts = result.data;

                        for (var i = 0; i < dts.length; i++) {
                            if (dts[i].Code == "1") {
                                con1 += "<option value=\"" + dts[i].Value + "\">" + dts[i].Name + "</option>";
                            }
                            else if (dts[i].Code == "2") {
                                con2 += "<option value=\"" + dts[i].Value + "\">" + dts[i].Name + "</option>";
                            }
                        }
                        con1 += "</select>";
                        con2 += "</select>";

                        $("#conDiv1").html(con1);
                        $("#conDiv2").html(con2);

                        var con3 = "<input class=\"form-control\" id=\"S_VALUE\" name=\"S_VALUE\" type=\"text\" value=\"\" /> ";
                        $("#conDiv3").html(con3);
                    }
                    else {// 活动记录
                        var con1 = "<select class=\"form-control\" id=\"Condition1\" name=\"Condition1\">";
                        var con2 = "<select class=\"form-control\" id=\"Condition2\" name=\"Condition2\">";

                        var dts = result.data;

                        for (var i = 0; i < dts.length; i++) {
                            if (dts[i].Code == "1") {
                                con1 += "<option value=\"" + dts[i].Value + "\">" + dts[i].Name + "</option>";
                            }
                            else if (dts[i].Code == "2") {
                                con2 += "<option value=\"" + dts[i].Value + "\">" + dts[i].Name + "</option>";
                            }
                        }
                        con1 += "</select>";
                        con2 += "</select>";

                        $("#conDiv1").html(con1);
                        $("#conDiv2").html(con2);
                    }

                },
                error: function (result) {
                    alert("页面有异常：" + result);
                }
            });
        }

        //根据属性获得查询条件
        function SetChildrenCondition(ptype, ctype, cIndex) {

            $.ajax({
                url: options.getChildreConds,
                type: "POST",
                data: { "ptype": ptype, "ctype": ctype, "conIndex": cIndex },
                success: function (result) {
                    var dts = result.data;
                    var vs = dts.split(",");
                    //基本条件
                    if (ptype == 1) {
                        //条件1
                        if (cIndex == 1) {
                            if (vs[0] == "2") {
                                var con2 = "<select class=\"form-control\" id=\"Condition2\" name=\"Condition2\">";
                                for (var i = 1; i < vs.length; i++) {
                                    var cuVC = vs[i].split("_");

                                    con2 += "<option value=\"" + cuVC[0] + "\">" + cuVC[1] + "</option>";
                                }

                                con2 += "</select>";
                                $("#conDiv2").html(con2);
                            }
                            //值
                            SetChildrenCondition(1, ctype, 2);
                        }
                            //值
                        else if (cIndex == 2) {
                            //2dropdownlist
                            if (vs[0] == "2") {
                                var con3 = "<select class=\"form-control\" id=\"S_VALUE\" name=\"S_VALUE\">";
                                for (var i = 1; i < vs.length; i++) {
                                    var cuVC = vs[i].split("_");
                                    con3 += "<option value=\"" + cuVC[0] + "\">" + cuVC[1] + "</option>";
                                }
                                con3 += "</select>";
                                $("#conDiv3").html(con3);
                            }
                                //1txt  
                            else if (vs[0] == "1") {
                                var con3 = "<input class=\"form-control\" id=\"S_VALUE\" name=\"S_VALUE\" type=\"text\" value=\"\" /> ";
                                $("#conDiv3").html(con3);
                                $("#S_VALUE").attr("placeholder", vs[1]);
                            }
                                //3date  
                            else if (vs[0] == "3") {
                                var con3 = "<input type=\"text\" id=\"S_VALUE\" class=\"form-control mask\" data-inputmask=\"'alias': 'yyyy-mm-dd'\">";
                                $("#conDiv3").html(con3);
                                $("#S_VALUE").inputmask("yyyy-mm-dd", { "placeholder": "yyyy-mm-dd" });
                                //$("#S_VALUE").attr("placeholder", vs[1]);
                            }
                        }
                    }
                        //消费状况
                    else {

                    }

                },
                error: function (result) {
                    _showInfoMessage("页面有异常：" + result, 'error');
                }
            });

        }

        //添加筛选条件
        $('#btnAddOneCon').click(function () {

            var value1 = $("#CUST_GROUP_CONDITION").val();//筛选条件下拉列表选中值  --大类
            var name1 = $("#CUST_GROUP_CONDITION").find("option:selected").text();//筛选条件下拉列表选中名称   --大类

            var value2 = $("#Condition1").val();//筛选条件下拉列表选中值 --条件1
            var name2 = $("#Condition1").find("option:selected").text();//筛选条件下拉列表选中名称  --条件1

            var value3 = $("#Condition2").val();//筛选条件下拉列表选中值  --条件2
            var name3 = $("#Condition2").find("option:selected").text();//筛选条件下拉列表选中名称  --条件2

            var valueR3 = $("#Condition2").val();//筛选条件下拉列表选中值  --条件2
            var nameR3 = $("#Condition2").find("option:selected").text();//筛选条件下拉列表选中名称  --条件2

            var nv = $("#S_VALUE").val();//最后的值 --值

            var names = $("#GROUP_CONDITIONS_NAME").val();//当前选中的名称  --选中名称
            var values = $("#GROUP_CONDITIONS_VALUE").val();//当前选中的值   --选中值


            var cuName = name1 + "_" + name2 + "_" + name3 + "_and_" + nv; //var cuName = "空括号_" + name1 + "_" + name2 + "_" + name3 + "_空括号_and_" + nv;
            var cuValue = value1 + "_" + value2 + "_" + value3 + "_and_" + nv;// "空括号_" + value1 + "_" + value2 + "_" + value3 + "_空括号_and_" + nv;

            var sNas = new Array();
            var sNas1 = new Array();

            var sVas = new Array();
            if (names != "") {
                var selectNames = names.split(",");
                for (var i = 0; i < selectNames.length; i++) {

                    if (selectNames[i] != "") {
                        //处理选中条件显示的内容数组--重新编号
                        var cuo = selectNames[i].split("_");
                        cuo.splice(0, 1, i + 1);
                        var cuStr = cuo.join("_");
                        sNas.push(cuStr);

                        //处理是否重复的准备数组
                        var cuo1 = selectNames[i].split("_");
                        cuo1.splice(0, 1);
                        var cuStr1 = cuo1.join("_");
                        sNas1.push(cuStr1);
                    }
                }



                var selectValues = values.split(",");
                for (var i = 0; i < selectValues.length; i++) {

                    if (selectValues[i] != "") {
                        //处理选中条件显示的值数组--重新编号
                        var cuo = selectValues[i].split("_");
                        cuo.splice(0, 1, i + 1);
                        var cuStr = cuo.join("_");
                        sVas.push(cuStr);
                    }
                }
            }

            var removeIndex = $.inArray(cuName, sNas1);
            if (removeIndex >= 0) {
                _showInfoMessage("不能添加重复条件", 'error');
                return;
            }

            var len = sNas.length + 1;

            //编号_括号_条件1_条件2_条件3_括号_值
            //value1 + "_" + value2 + "_" + value3 + "_and_" + n
            if (names == "")
                names = len + "_" + cuName;
            else
                names = sNas.join(",") + "," + len + "_" + cuName;

            if (values == "")
                values = len + "_" + cuValue;
            else
                values = sVas.join(",") + "," + len + "_" + cuValue;
            $("#GROUP_CONDITIONS_NAME").val(names);
            //$("#GROUP_CONDITIONS_VALUE").val(values);//存sql语句
            var StrValues = values.split(",");
            var strSql = "";

            $("#GROUP_CONDITIONS_VALUE").val(values);
            if (value1 == 1)//基本条件
            {
                var sqlwhere = $("#GROUP_CONDITIONS_SQL").val();
                strSql = " Convert(nvarchar(1000)," + value2 + ") " + value3 + " '" + nv + "' ";
                sqlwhere += strSql;
                $("#GROUP_CONDITIONS_SQL").val(sqlwhere);
            }
            else if (value1 == 2)//消费状况
            {
                //多长时间内 当前时间减去多少天
                //固定时间范围
                //CUST_GROUP_TYPE
                var Type = $("#CUST_GROUP_TYPE").val();
                if (Type == 1)//动态分组
                {
                    strSql = " A.ID in(select CUST_ID from ORDER_HEAD where TRANS_DATE>=Convert(nvarchar(10),DateAdd(dd,-'" + parseInt(value2) + "',getdate()),120) and TRANS_DATE<=Convert(nvarchar(10),getdate(),120) group by CUST_ID having COUNT(1) " + value3 + " '" + nv + "') ";
                }
                else//静态分组
                {
                    var myDate = new Date();
                    var DateTo = myDate.getFullYear() + "-" + (parseInt(myDate.getMonth()) + parseInt('1')) + "-" + myDate.getDate();     //获取当前日期
                    var DateFrom = getNewDay(DateTo, value2);
                    strSql = " A.ID in(select CUST_ID from ORDER_HEAD where TRANS_DATE>='" + DateFrom + "' and TRANS_DATE<='" + DateTo + "' group by CUST_ID having COUNT(1) " + value3 + " '" + nv + "') ";
                }
                var sqlwhere = $("#GROUP_CONDITIONS_SQL").val();
                sqlwhere += strSql;
                $("#GROUP_CONDITIONS_SQL").val(sqlwhere);
            }
            showConditionList();
        });


        function getNewDay(dateTemp, days) {
            var dateTemp = dateTemp.split("-");
            var nDate = new Date(dateTemp[1] + '-' + dateTemp[2] + '-' + dateTemp[0]); //转换为MM-DD-YYYY格式    
            var millSeconds = Math.abs(nDate) - (days * 24 * 60 * 60 * 1000);  //（加上days天 ：var millSeconds = Math.abs(nDate) - (days * 24 * 60 * 60 * 1000);）
            var rDate = new Date(millSeconds);
            var year = rDate.getFullYear();
            var month = rDate.getMonth() + 1;
            if (month < 10) month = "0" + month;
            var date = rDate.getDate();
            if (date < 10) date = "0" + date;
            return (year + "-" + month + "-" + date);
        }


        //展示选中条件的值：编号_括号_大类_条件1_条件2_括号_or/and_值
        function showConditionList() {
            var nameStrs = $("#GROUP_CONDITIONS_NAME").val();//当前选中的值 
            var namesHtml = [];
            if (nameStrs != "") {
                var names = nameStrs.split(",");

                for (var i = 0; i < names.length; i++) {
                    var cuObj = names[i].split("_");
                    namesHtml.push("<tr style=\"height:10px\">");

                    namesHtml.push("<td style=\"width: 15%;\">按 <span class=\"label label-success\">", cuObj[1], "</span> 筛选</td>");

                    namesHtml.push("<td style=\"width: 10%;\">");
                    //namesHtml.push("<select class=\"form-control\" style=\" height:28px;\" id=\"startStr" + i + "\" name=\"startStr" + i + "\" onchange=\"util.selectChangeM('" + names[i] + "',1,this);\">");

                    //if (cuObj[1] == "空括号") {
                    //    namesHtml.push("<option value=\"空括号\" selected=\"true\"></option>");
                    //}
                    //else {
                    //    namesHtml.push("<option value=\"空括号\"></option>");
                    //}

                    //if (cuObj[1] == "(") {
                    //    namesHtml.push("<option value=\"(\" selected=\"true\">(</option>");
                    //}
                    //else {
                    //    namesHtml.push("<option value=\"(\">(</option>");
                    //}
                    //编号_大类_条件1_条件2_or/and_值
                    //编号_括号_大类_条件1_条件2_括号_or/and_值
                    //namesHtml.push("</select>");
                    namesHtml.push("</td>");

                    namesHtml.push("<td style=\"width: 15%;\"><div style=\"text-align:left;\">", cuObj[2], "</div></td>");
                    namesHtml.push("<td style=\"width: 10%;\"><div style=\"text-align:left;\">", cuObj[3], "</div></td>");
                    namesHtml.push("<td style=\"width: 20%;\"><div style=\"text-align:left;\">", cuObj[5], "</div></td>");

                    namesHtml.push("<td style=\"width: 10%;\">");
                    //namesHtml.push("<select class=\"form-control\" style=\" height:28px;\" id=\"endStr" + i + "\" name=\"endStrendStr" + i + "\" onchange=\"util.selectChangeM('" + names[i] + "',2,this);\">");

                    //if (cuObj[5] == "空括号") {
                    //    namesHtml.push("<option value=\"空括号\" selected=\"true\"></option>");
                    //}
                    //else {
                    //    namesHtml.push("<option value=\"空括号\"></option>");
                    //}
                    //if (cuObj[5] == ")") {
                    //    namesHtml.push("<option value=\")\" selected=\"true\">)</option>");
                    //}
                    //else {
                    //    namesHtml.push("<option value=\")\">)</option>");
                    //}
                    //namesHtml.push("</select>");
                    namesHtml.push("</td>");

                    namesHtml.push("<td style=\"width: 10%;\"><div style=\"text-align:left;\"><a title='移除' href='javascript:;' onclick=\"util.Remove('", names[i], "')\"><span class=\"label label-danger\">移除</span></a></div></td>");
                    namesHtml.push("<td style=\"width: 10%;\">");
                    if (i != names.length - 1) {
                        namesHtml.push("<select class=\"form-control\" style=\" height:28px;\" id=\"tj" + i + "\" name=\"tj" + i + "\" onchange=\"util.selectChangeR('" + names[i] + "',this);\">");
                        if (cuObj[4] == "or") {
                            namesHtml.push("<option value=\"and\">并且</option>");
                            namesHtml.push("<option value=\"or\" selected=\"true\">或者</option>");
                        }
                        else {
                            namesHtml.push("<option value=\"and\" selected=\"true\">并且</option>");
                            namesHtml.push("<option value=\"or\">或者</option>");
                        }
                        namesHtml.push("</select>");
                    }

                    namesHtml.push("</td>");
                    namesHtml.push("</tr>");
                }
            }
            $("#tblCondition tbody").html("");
            $("#tblCondition tbody").html(namesHtml.join(""));
        }

        //括号选择事件
        util.selectChangeM = function (cuV, type, obj) {
            var nameStrs = $("#GROUP_CONDITIONS_NAME").val();//当前选中的名称
            var valueStrs = $("#GROUP_CONDITIONS_VALUE").val();//当前选中的值 
            if (nameStrs != "") {
                var names = nameStrs.split(",");

                var removeIndex = $.inArray(cuV, names);
                var cuNV = names[removeIndex].split("_");
                //编号_括号_大类_条件1_条件2_括号_or/and_值
                if (type == 1)
                    cuNV.splice(1, 1, $(obj).val());
                else if (type == 2)
                    cuNV.splice(5, 1, $(obj).val());

                names.splice(removeIndex, 1, cuNV.join("_"));

                var newNameStrs = names.join(",");
                $("#GROUP_CONDITIONS_NAME").val(newNameStrs);

                var values = valueStrs.split(",");
                var cuRI = values[removeIndex].split("_");
                //编号_括号_大类_条件1_条件2_括号_or/and_值
                if (type == 1)
                    cuRI.splice(1, 1, $(obj).val());
                else if (type == 2)
                    cuRI.splice(5, 1, $(obj).val());

                values.splice(removeIndex, 1, cuRI.join("_"));

                var newValueStrs = values.join(",");
                $("#GROUP_CONDITIONS_VALUE").val(newValueStrs);

                //alert(newValueStrs);

                showConditionList();
            }
        }
        //并且或者选择事件
        util.selectChangeR = function (cuV, obj) {
            var nameStrs = $("#GROUP_CONDITIONS_NAME").val();//当前选中的名称
            var valueStrs = $("#GROUP_CONDITIONS_VALUE").val();//当前选中的值 
            if (nameStrs != "") {
                var names = nameStrs.split(",");
                var removeIndex = $.inArray(cuV, names);
                var cuNV = names[removeIndex].split("_");

                //编号_括号_大类_条件1_条件2_括号_or/and_值
                cuNV.splice(6, 1, $(obj).val());
                names.splice(removeIndex, 1, cuNV.join("_"));

                var newNameStrs = names.join(",");
                $("#GROUP_CONDITIONS_NAME").val(newNameStrs);

                var values = valueStrs.split(",");
                var cuRI = values[removeIndex].split("_");

                //编号_括号_大类_条件1_条件2_括号_or/and_值
                cuRI.splice(6, 1, $(obj).val());
                values.splice(removeIndex, 1, cuRI.join("_"));

                var newValueStrs = values.join(",");
                $("#GROUP_CONDITIONS_VALUE").val(newValueStrs);

                showConditionList();
            }
        }

        //移除选中的条件
        util.Remove = function (con) {
            var nameStrs = $("#GROUP_CONDITIONS_NAME").val();//当前选中的名称   
            var valueStrs = $("#GROUP_CONDITIONS_VALUE").val();//当前选中的值 

            if (nameStrs != "") {
                //名称
                var names = nameStrs.split(",");
                var removeIndex = $.inArray(con, names);
                names.splice(removeIndex, 1);
                var newNameStrs = names.join(",");
                $("#GROUP_CONDITIONS_NAME").val(newNameStrs);

                //值
                var values = valueStrs.split(",");
                values.splice(removeIndex, 1);
                var newValueStrs = values.join(",");
                $("#GROUP_CONDITIONS_VALUE").val(newValueStrs);

                showConditionList();
            }
        }

        //取消
        $('#btnCloseFrm').click(function () {
            window.location.href = options.indexUrl;
        });

        //顾客列表
        var gridopt = {
            url: options.showGroupCustomer,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '顾客名称', name: 'NAME', width: "10%", sortable: false, align: 'left' },
                    { display: '手机号码', name: 'MobilePhone', width: "15%", sortable: false, align: 'left' },
                    { display: '管理柜台', name: 'STORE_NAME', width: "10%", sortable: false, align: 'left' },
                    { display: '最后一次购买时间', name: 'LAST_BUY_DATE', width: "15%", sortable: false, align: 'left', process: formatDate },
                    { display: '近三个月消费频次', name: 'BUY_COUNT', width: "15%", sortable: false, align: 'left' },
                    { display: '近三个月消费金额', name: 'BUY_MONEY', width: "15%", sortable: false, align: 'left' },
                    //{ display: '操作', name: 'ID', width: "10%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 5,
            usepager: true,
            showcheckbox: false,
            autoload: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);

        //
        if ($("#ID").val() != 0) {

        }

        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0].replace("/", "-").replace("/", "-");
            }
            else {
                return "-"
            }
        }

        //function processOp(value, cell) {
        //    var ops = [];
        //    ops.push("&nbsp;&nbsp;<a title='删除顾客分组' class='abtn' href='javascript:;'><i class='fa fa-trash-o' ></i>删除</a>");
        //    return ops.join("");
        //}
        //显示已选择的分组客户
        $('#btnShowGC').click(function () {

            //if ($("#GROUP_CONDITIONS_VALUE").val() == "") {
            //    _showInfoMessage("请先选择查询条件", 'error');
            //    return;
            //}
            xjgrid.QueryByFields([
                { name: "GROUP_CONDITIONS_VALUE", value: $("#GROUP_CONDITIONS_VALUE").val() },
                { name: "CUST_GROUP_TYPE", value: $("#CUST_GROUP_TYPE").val() },
                { name: "STRSQL", value: $("#STRSQL").val() },
                { name: "GROUP_CONDITIONS_SQL", value: $("#GROUP_CONDITIONS_SQL").val() },
            ]);
        });




        //显示客户
        showSelectCustList();
        function showSelectCustList() {
            //显示查询条件
            showGroupCondition();
           
           
        }

        function showGroupCondition() {
            var Group_ID = $("#ID").val();
            $.ajax({
                url: options.showGroupCondition,
                type: "Get",
                data: { "Group_ID": Group_ID },
                success: function (result) {
                    if (result.status == 1) {
                        var json = eval(result.data[0]);
                        var Ht = eval(result.data[1]);
                        $(json).each(function (i) {
                            var names = "";//$("#GROUP_CONDITIONS_NAME").val();//当前选中的名称  --选中名称
                            var values = "";//$("#GROUP_CONDITIONS_VALUE").val();//当前选中的值   --选中值
                            var value1, count1, value2, value3;
                            var name1, name2, name3;
                            var valueR3;
                            var nameR3;
                            var nv;
                            var cuName;
                            var cuValue;
                            var ListCondition;
                            for (var j = 0; j < json.length; j++) {
                                ListCondition = Ht[json[j].FILTER_CONDITION];
                                value1 = json[j].FILTER_CONDITION;//筛选条件下拉列表选中值  --大类
                                count1 = $("#CUST_GROUP_CONDITION option").length;
                                for (var i = 0; i < count1; i++) {
                                    if ($("#CUST_GROUP_CONDITION ").get(0).options[i].value == value1) {
                                        name1 = $("#CUST_GROUP_CONDITION ").get(0).options[i].text;
                                        break;
                                    }
                                }
                                value2 = json[j].ATTRIBUTE;//筛选条件下拉列表选中值 --条件1
                                for (var i = 0; i < ListCondition.length; i++) {
                                    if (ListCondition[i].Code == "1" && ListCondition[i].Value == value2) {
                                        name2 = ListCondition[i].Name;//筛选条件下拉列表选中名称  --条件1
                                        break;
                                    }
                                }

                                value3 = json[j].OPERATORS;//筛选条件下拉列表选中值  --条件2
                                for (var i = 0; i < ListCondition.length; i++) {
                                    if (ListCondition[i].Code == "2" && ListCondition[i].Value == value3) {
                                        name3 = ListCondition[i].Name;//筛选条件下拉列表选中名称  --条件2
                                        break;
                                    }
                                }

                                var valueR3 = value3;//筛选条件下拉列表选中值  --条件2
                                var nameR3 = name3;//筛选条件下拉列表选中名称  --条件2

                                var nv = json[j].FIXED_VALUE;//最后的值 --值

                                var cuName = name1 + "_" + name2 + "_" + name3 + "_and_" + nv; //var cuName = "空括号_" + name1 + "_" + name2 + "_" + name3 + "_空括号_and_" + nv;
                                var cuValue = value1 + "_" + value2 + "_" + value3 + "_and_" + nv;// "空括号_" + value1 + "_" + value2 + "_" + value3 + "_空括号_and_" + nv;

                                var len = j + 1;
                                if (names == "")
                                    names = len + "_" + cuName;
                                else
                                    names = names + "," + len + "_" + cuName;
                                //编号_条件1_条件2_条件3_值
                                //value1 + "_" + value2 + "_" + value3 + "_and_" + n

                                if (values == "")
                                    values = len + "_" + cuValue;
                                else
                                    values = values + "," + len + "_" + cuValue;

                            }

                            $("#GROUP_CONDITIONS_NAME").val(names);
                            $("#GROUP_CONDITIONS_VALUE").val(values);
                            showConditionList();

                        });
                    }
                    xjgrid.QueryByFields([
                    { name: "GROUP_CONDITIONS_VALUE", value: $("#GROUP_CONDITIONS_VALUE").val() },
                    { name: "CUST_GROUP_TYPE", value: $("#CUST_GROUP_TYPE").val() },
                    { name: "STRSQL", value: $("#STRSQL").val() },
                    { name: "GROUP_CONDITIONS_SQL", value: $("#GROUP_CONDITIONS_SQL").val() },
                    { name: "Group_Id", value: $("#ID").val() },
                    { name: "Group_Type", value: $("#CUST_GROUP_TYPE").val() },
                    ]);
                }


            });
        }


        $('#ExportTXT').click(function () {
            //$.ajax({
            //    url: options.exportFile,
            //    type: "Get",
            //    data: { "Group_ID": Group_ID ,"type":1},
            //    success: function (result) {
            //    }
            //});
        });
    
    });
})(window, undefined, jQuery);