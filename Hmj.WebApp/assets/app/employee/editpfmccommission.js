; (function (window, undefined, $) {

    $(document).ready(function () {


        //小类选择样式改编
        util.selectStyle = function (obj) {
            if ($(obj).hasClass('badge-info')) {
                $(obj).removeClass('badge-info');
            }
            else {
                $(obj).addClass('badge-info');

                var cuTypeStr = $("#typeStr").val();
                var tS = cuTypeStr.split(",");
                if (tS.length > 0) {
                    //如果不包括
                    var cuI = $.inArray($(obj).attr("id"), tS);
                    if (cuI >= 0) {
                        tS.splice(cuI, 1)
                    }
                }

                if (tS.join(",") == "") {
                    var proType = $(obj).attr("id").split("_")[0];
                    if (proType == "1") {
                        $("#serviceitem_type").addClass('badge-info');
                    }
                    else if (proType == "2") {
                        $("#pro_type").addClass('badge-info');
                    }
                    else if (proType == "3") {
                        $("#card_type").addClass('badge-info');
                    }
                }
                $("#typeStr").val(tS.join(","));
            }
        }

        var _submiting = false;

        $("#e2").select2({ width: 'resolve' });
        //$("#e12").select2({ width: "resolve", tags: ["red", "white", "purple", "orange", "yellow"] });
        //查询类别
        function GetAddCategorys() {
            if (_submiting) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            $.ajax({
                url: options.getCCaUrl,
                type: "POST",
                data: { id: $("#ID").val() },
                success: function (result) {
                    hideLoadingMsg();
                    if (result.status == 0) { 
                        $(".select2-choices").find(".select2-search-choice").remove();
                        var optsA = result.data, optsA2 = "<option></option>" + optsA;
                        $("select.populate").each(function () {
                            var e = $(this);
                            e.html(e.hasClass("placeholder") ? optsA2 : optsA);
                        });
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
        GetAddCategorys();

        //返回
        $("#btnBack").click(function () {
            window.location.href = options.indexUrl;
        });

        //提成添加事件
        $("#addBtn_1").click(addTc);

        //添加提成方法
        function addTc() {
            if (!valDtData())
                return;
            //查询行数
            var tbSize = $("#tbValue tbody tr").size();
            var idStr = (tbSize + 1);
            //上一行的结束价格文本框
            var preRowInput = $("#tbValue tbody tr:eq(" + (tbSize - 1) + ") td:eq(3)").find("input");
            var nEndV = "";
            if (isNaN(parseInt($(preRowInput).val()))) {
                nEndV = "";
            }
            else {
                nEndV = (parseInt($(preRowInput).val()) + 1);
            }
            var newRow = "";
            if (tbSize == 1) {
                newRow = "<tr style=\"height:30px\">" +
                "<td style=\"width: 80px; \">提成区段</td>" +
                "<td style=\"width: 80px; text-align: right; color: lightgray\">" + nEndV + "</td>" +
                "<td style=\"width: 50px; text-align:center;\">至￥</td>" +
                "<td>" +
                "    <input type=\"text\" style=\"border-left: 0px; border-right: 0px;" +
                "border-top: 0px; text-align: right; width:120px\" id=\"endV_" + (tbSize + 1) + "\" />&nbsp;元" +
                "</td>" +
                "<td style=\"width: 230px; text-align: right;\">&nbsp;此区段提成比例&nbsp;</td>" +
                "<td>" +
                "      <input type=\"text\" style=\"border-left: 0px; border-right: 0px; border-top: 0px;" +
                "   text-align: right; width:120px\"" +
                " id=\"rateV_" + (tbSize + 1) + "\"/>&nbsp;%</td>" +
                "<td style=\"width: 50px; text-align:right;height:23px\">" +
                "<a title=\"移除\" id=\"rmoveBtn_1\" class=\"abtn badge badge-danger\" href=\"javascript:;\" " +
                "  style=\"color:white; font-size:10px\" ><i class=\"fa fa-minus\" ></i></a></td>" +
                "</tr>"; 
            }
            else {
                newRow = "<tr style=\"height:30px\">" +
                "<td style=\"width: 80px; \">提成区段</td>" +
                "<td style=\"width: 80px; text-align: right; color: lightgray\">" + nEndV + "</td>" +
                "<td style=\"width: 50px; text-align:center;\">至￥</td>" +
                "<td>" +
                "    <input type=\"text\" style=\" border-left: 0px; border-right: 0px;" +
                "border-top: 0px; text-align: right; width:120px\" id=\"endV_" + idStr + "\" />&nbsp;元" +
                "</td>" +
                "<td style=\"width: 230px; text-align: right;\">&nbsp;此区段提成比例&nbsp;</td>" +
                "<td>" +
                "      <input type=\"text\" style=\"border-left: 0px; border-right: 0px; border-top: 0px;" +
                "   text-align: right; width:120px\"" +
                " id=\"rateV_" + idStr + "\" />&nbsp;%</td>" +
                "<td style=\"width: 50px; text-align:right;height:23px\">" +
                "</td></tr>";

            }
            //添加一行
            $("#tbValue tbody tr:eq(" + (tbSize - 1) + ")").after(newRow);

            var cuRowInput = $("#tbValue tbody tr:eq(" + (tbSize) + ") td:eq(3)").find("input");
            //设置输入数字事件
            $(cuRowInput).keyup(function () {
                if (isNaN(parseInt($(cuRowInput).val()))) {
                    $(cuRowInput).val("");
                    $("#tbValue tbody tr:eq(" + (tbSize + 1) + ") td:eq(1)").text("");
                }
                else {
                    $("#tbValue tbody tr:eq(" + (tbSize + 1) + ") td:eq(1)").text(parseInt($(cuRowInput).val()) + 1);
                }
            });

            var cuRowInput1 = $("#tbValue tbody tr:eq(" + (tbSize) + ") td:eq(5)").find("input");
            //设置输入数字事件
            $(cuRowInput1).keyup(function () {
                if (isNaN(parseInt($(cuRowInput1).val()))) {
                    $(cuRowInput1).val("");
                }
            });

            if (tbSize == 1) {

                var cuRowInput3 = $("#tbValue tbody tr:eq(0) td:eq(3)").find("input");
                //设置输入数字事件
                $(cuRowInput3).keyup(function () {
                    if (isNaN(parseInt($(cuRowInput3).val()))) {
                        $(cuRowInput3).val("");
                        $("#tbValue tbody tr:eq(1) td:eq(1)").text("");
                    }
                    else {
                        $("#tbValue tbody tr:eq(1) td:eq(1)").text(parseInt($(cuRowInput3).val()) + 1);
                    }
                });

                $("#rmoveBtn_1").click(removeTc);
            }
        }

        $("#inputWx").keyup(function () {
            if (isNaN(parseInt($("#inputWx").val()))) {
                $("#inputWx").val("");
            }
        });

        $("#inputBl").keyup(function () {
            if (isNaN(parseInt($("#inputBl").val()))) {
                $("#inputBl").val("");
            }
        });

        //移除提成方法
        function removeTc() {
            //查询行数
            var tbSize = $("#tbValue tbody tr").size();
            if (tbSize > 1) {
                $("#tbValue tbody tr:eq(" + (tbSize - 1) + ")").remove();
            }
        }

        //验证
        function valDtData() {
            var trs = $("#tbValue tbody tr");
            var cuDStr = [];
            var cuTrDStr = "";
            var i = 0;
            var re = 0;
            $("#tbValue tbody tr").each(function () {
                var cuStN = $("#tbValue tbody tr:eq(" + i + ") td:eq(1)").text();
                var cuEdN = $("#tbValue tbody tr:eq(" + i + ") td:eq(3)").find("input").val();

                if (isNaN(parseInt(cuStN)) || isNaN(parseInt(cuEdN))) {
                    re = 1;
                }
                else {
                    if (parseInt(cuStN) > parseInt(cuEdN)) {
                        re = 2;
                    }
                }
                i++;
            });
            if (re == 1) {
                _showInfoMessage("数据不完整，请填写完整后再添加。", 'error');
                return false;
            }
            else if (re == 2) {
                _showInfoMessage("数据填写不对，请填写正确后再添加。", 'error');
                return false;
            }
            else return true;

        }

        var _submiting1 = false;

        //保存
        function SavePFMCCommission() {
          
            if (!saveValDtData()) {
                return;
            }
            if (_submiting1) {
                showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting1 = true;
            showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            $.ajax({
                url: options.savePfmUrl,
                type: "POST",
                data: { "values": getDtData(), "id": $("#ID").val(), "types": $("#typeStr").val(), "groupId": $("#groupId").val() },
                success: function (result) {
                    hideLoadingMsg();
                    _submiting1 = false;

                    if (result.status == 0) {
                        GetAddCategorys();
                        getTcData(); 
                        _showInfoMessage("保存成功！", 'success');
                    }
                    else
                        _showInfoMessage("保存失败！", 'error');

                },
                error: function (result) {
                    hideLoadingMsg();
                    _submiting1 = false;
                    _showInfoMessage("请求失败：" + result, 'error');
                }
            });
        }

        //保存验证
        function saveValDtData() { 
            $("#typeStr").val($("#e2").val());
            var typs = $("#typeStr").val();
            if (typs == "") {
                _showInfoMessage("请选择提成项！", 'error');
                return false;
            }
            var trs = $("#tbValue tbody tr");
            var cuDStr = [];
            var cuTrDStr = "";
            var i = 0;
            var re = 0;
            $("#tbValue tbody tr").each(function () {
                var cuStN = $("#tbValue tbody tr:eq(" + i + ") td:eq(1)").text();
                var cuEdN = $("#tbValue tbody tr:eq(" + i + ") td:eq(3)").find("input").val();
                var cuBlN = $("#tbValue tbody tr:eq(" + i + ") td:eq(5)").find("input").val();

                if (isNaN(parseInt(cuStN)) || isNaN(parseInt(cuEdN)) || isNaN(parseInt(cuBlN))) {
                    re = 1;
                }
                else {
                    if (parseInt(cuStN) > parseInt(cuEdN)) {
                        re = 2;
                    }
                }
                i++;
            });
            if (re == 1) {
                _showInfoMessage("数据不完整，请填写完整再保存。", 'error');
                return false;
            }
            else if (re == 2) {
                _showInfoMessage("数据填写不对，请填写正确再保存。", 'error');
                return false;
            }
            else return true;

        }

        //获取表格值列表
        function getDtData() {
            var trs = $("#tbValue tbody tr");
            var cuDStr = [];
            var cuTrDStr = "";
            var i = 0;
            $("#tbValue tbody tr").each(function () {
                cuTrDStr = $("#tbValue tbody tr:eq(" + i + ") td:eq(1)").text() + "_" +
                    $("#tbValue tbody tr:eq(" + i + ") td:eq(3)").find("input").val() + "_" +
                    $("#tbValue tbody tr:eq(" + i + ") td:eq(5)").find("input").val();
                cuDStr.push(cuTrDStr);
                i++;
            });
            return cuDStr.join(",");
        }

        //保存
        $("#btnSaveYjtc").click(function () {
            SavePFMCCommission();
        });

        var _submiting2 = false;
        //获取提成列表
        function getTcData() {
            if (_submiting2) {
                showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting2 = true;
            showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            $.ajax({
                url: options.getCpd,
                type: "POST",
                data: { "id": $("#ID").val() },
                success: function (result) {
                    hideLoadingMsg();
                    if (result.status == 0) {
                        var htmlStr = [];
                        var groupCount = 0;
                        if (result.data.length > 0) {
                            var groupId = 0;

                            for (var i = 0; i < result.data.length; i++) {
                                if (groupId != result.data[i].GROUP_ID) {
                                    groupCount++;
                                    if (groupId != 0) {
                                        htmlStr.push("</tbody>");
                                        htmlStr.push("</table>");
                                        htmlStr.push("</div>");
                                        htmlStr.push("</div>");
                                    }

                                    htmlStr.push("<div class=\"form-group col-sm-10\">");
                                    htmlStr.push("<div class=\"alert alert-dismissable alert-info\" style=\"font-size: 12px\">");
                                    htmlStr.push("<button type=\"button\" class=\"close\" style=\"font-size: 12px\" onclick=\"util.Delete('", result.data[i].GROUP_ID, "', '", result.data[i].PROD_TYPE_CATEGORY_STR, "')\">删除</button>");
                                    htmlStr.push("<h4>" + result.data[i].PROD_TYPE_CATEGORY_STR + "</h4>");
                                    htmlStr.push("<table>");
                                    htmlStr.push("<tbody>");

                                }

                                htmlStr.push("<tr style=\"height: 25px\">");
                                htmlStr.push("<td style=\"width: 78px; text-align: right;\">提成区段&nbsp;</td>");
                                htmlStr.push("<td style=\"width: 80px; text-align: right;\">" + result.data[i].SEC_BEGIN + "</td>");
                                htmlStr.push("<td style=\"width: 40px; text-align: center;\">至￥</td>");
                                htmlStr.push("<td style=\"width: 80px; text-align: right;\">" + result.data[i].SEC_END + "</td>");
                                htmlStr.push("<td style=\"width: auto;\">&nbsp;元&nbsp;</td>");
                                htmlStr.push("<td style=\"width: 230px; text-align: right;\">&nbsp;此区段提成比例&nbsp;</td>");
                                htmlStr.push("<td style=\"width: 30px; text-align: right;\">" + result.data[i].SEC_RATE + "</td>");
                                htmlStr.push("<td style=\"width: auto;\">&nbsp;%&nbsp;</td>");
                                htmlStr.push("</tr>");
                                groupId = result.data[i].GROUP_ID;
                            }

                            htmlStr.push("</tbody>");
                            htmlStr.push("</table>");
                            htmlStr.push("</div>");
                            htmlStr.push("</div>");
                            $("#addedObjs").html("");
                            $("#addedObjs").html(htmlStr.join(""));
                            $("#groupId").val(groupId);
                        }
                        else {
                            $("#addedObjs").html("");
                            $("#groupId").val("0");
                        }
                    }
                    _submiting2 = false;
                },
                error: function (result) {
                    hideLoadingMsg();
                    _submiting2 = false;
                    _showInfoMessage("请求失败：" + result, 'error');
                }
            });
        }
        getTcData();

        util.Delete = function (id, name) {
            $("#lbuserName").html(name);
            $("#hdCurrentId").val(id);
            $('#confirmModal').modal('show');
        };

        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var id = $("#hdCurrentId").val();
            $.post(options.deleteUrl, { groupId: id },
                  function (res) {
                      if (res.status == 0) {
                          getTcData();
                          GetAddCategorys();
                          _showInfoMessage("操作成功", 'success');                         
                      }
                      else {
                          _showInfoMessage("操作失败：" + res.message, 'error');

                      }
                  },
                  "json"
            );
        })
    });
})(window, undefined, jQuery);