; (function (window, undefined, $) {
    $(document).ready(function () {

        //保存验证
        $('#frmSave').validator({
            rules: {
            },
            fields: {
                '#STATUS': 'required',
                '#NAME': 'required',
                '#DISCOUNT_RATE': 'required',
                '#CUST_GROUP_ID': 'required',
                '#IS_ALLDAY': 'required',
                '#IS_ALLPROD': 'required',
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        if ($("#ID").val() == 0) {
                            $("#ID").val(res.data);
                            _showInfoMessage("保存成功！", 'success'); 
                            var cuT = $("#TYPE").val();
                            window.location.href = options.indexUrl + "/" + cuT;
                            //if (cuT == 0) {
                            //    window.location.href = options.indexUrl + "/0";
                            //}
                            //else if (cuT == 1) {
                            //    window.location.href = options.indexUrl + "/1";
                            //}
                            //else if (cuT == 3) {
                            //    window.location.href = options.indexUrl + "/3";
                            //}
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
        //默认加载 是否整天选中 
        if ($("#IS_ALLDAY:checked").attr("value") == "0") {
            $("#BEGIN_TIMEDiv").show();
            $("#END_TIMEDiv").show();
        }
        else {
            $("#BEGIN_TIMEDiv").hide();
            $("#END_TIMEDiv").hide();
        }
        //是否整天选中事件
        $("input[name='IS_ALLDAY']").change(function () {
            if ($("#IS_ALLDAY:checked").attr("value") == "0") {
                $("#BEGIN_TIMEDiv").show();
                $("#END_TIMEDiv").show();
            }
            else {
                $("#BEGIN_TIMEDiv").hide();
                $("#END_TIMEDiv").hide();
            }
        });
        //默认加载 是否所有项目选中 
        if ($("#IS_ALLPROD:checked").attr("value") == "0") {
            $("#p1").show();
            $("#p2").show();
            $("#p3").show();
            $("#p4").show();
        }
        else {
            $("#p1").hide();
            $("#p2").hide();
            $("#p3").hide();
            $("#p4").hide();
        }
        //是否所有项目选中事件
        $("input[name='IS_ALLPROD']").change(function () {
            if ($("#IS_ALLPROD:checked").attr("value") == "0") {
                $("#p1").show();
                $("#p2").show();
                $("#p3").show();
                $("#p4").show();
            }
            else {
                $("#p1").hide();
                $("#p2").hide();
                $("#p3").hide();
                $("#p4").hide();
            }
        });

        //保存
        $('#btnSaveFrm').click(function () {
            if ($("#STATUS:checked").val() == undefined) {
                _showInfoMessage("请选择状态！", 'error'); 
                return;
            }
            //如果是优惠渠道  自定义不判断
            if ($("#TYPE").val() == "0") {
                if ($("#CHANNEL").val() == undefined) {
                    _showInfoMessage("请选择渠道！", 'error'); 
                    return;
                }
            }

            if ($("#IS_ALLDAY:checked").val() == undefined) {
                _showInfoMessage("请选择适用时间！", 'error'); 
                return;
            }
            if ($("#IS_ALLDAY:checked").val() == "0") {
                if ($("#BEGIN_TIME_Str").val() == "") {
                    _showInfoMessage("请选择有效期开始时间！", 'error'); 
                    return;
                }
                if ($("#END_TIME_Str").val() == "") {
                    _showInfoMessage("请选择有效期结束时间！", 'error'); 
                    return;
                }
            }
            if ($("#IS_ALLPROD:checked").val() == undefined) {
                _showInfoMessage("请选择适用项目！", 'error'); 
                return;
            }

            if($("#IS_ALLPROD:checked").val() == 0)
            {
                if ($("#PROD_INFO").val() == "") {
                    _showInfoMessage("请选择适用项目！", 'error'); 
                    return;
                }
            }
            if ($("#BEGIN_DATE_Str").val() == "") {
                _showInfoMessage("请选择有效期开始时间！", 'error'); 
                return;
            }
            if ($("#END_DATE_Str").val() == "") {
                _showInfoMessage("请选择有效期结束时间！", 'error'); 
                return;
            }
            //如果是团购活动
            if ($("#TYPE").val() == "1") {
                if ($("#SUB_TYPE").val() == undefined) {
                    _showInfoMessage("请选择类型！", 'error'); 
                    return;
                }
            }
            $('#frmSave').submit();
        });
        //开始时间
        $('#BEGIN_DATE_Str').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });
        //结束时间
        $('#END_DATE_Str').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });
        //开始时间点
        $("#BEGIN_TIME_Str").timepicker({
        });

        $('#BEGIN_TIME_BTN').click(function () {
            $('#BEGIN_TIME_Str').timepicker("show");
        });

        $("#END_TIME_Str").timepicker({
            language: "zh-CN"
        });
        //$('#END_TIME_Str').datepicker({
        //    language: "zh-CN"
        //});
        $('#END_TIME_BTN').click(function () {
            $('#END_TIME_Str').timepicker("show");
        });

        $('#YXDATE_Datepicker').datepicker();

        //取消
        $('#btnCloseFrm').click(function () {
            var cuT = $("#TYPE").val();
            //if (cuT == 0) {
            window.location.href = options.indexUrl + "/" + cuT;
            //}
            //else {
            //    window.location.href = options.indexUrl + "/1";
            //}
        });

        //获取小分类下拉选框列表
        var _submiting2 = false;
        function PostGetProdTypes() {
            if (_submiting2) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting2 = true;
            var type = $('#PROMISSION_PRO_TYPE').val();
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
            $.ajax({
                url: options.proTypeUrl,
                type: "POST",
                data: { "type": type },
                success: function (result) {
                    hideLoadingMsg();
                    //类别
                    var cuAllPs = result.data;
                    var cuAllPsArray = new Array();
                    if (cuAllPs != "") {
                        cuAllPsArray = cuAllPs.split(",");
                    }

                    var hl = "<select class=\"form-control\" id=\"PRO_TYPE\" name=\"PRO_TYPE\" onchange=\"util.ProTypeChange()\">";

                    if (cuAllPs != "") {
                        for (var i = 0; i < cuAllPsArray.length; i++) {
                            var re = cuAllPsArray[i].split("_");
                            hl += "<option value=\"" + re[0] + "\">" + re[1] + "</option>";
                        }
                    }
                    hl += "<select>";
                    $("#pro_type_div").html(hl);

                    PostGetProItems();

                    _submiting2 = false;
                },
                error: function (result) {
                    hideLoadingMsg();
                    _submiting2 = false;
                    _showInfoMessage("页面有异常：" + result, 'error'); 
                }
            });
        }
        //大类选择改变事件
        $("#PROMISSION_PRO_TYPE").change(function () {
            PostGetProdTypes();
        });

        //根据大小类下拉产品类型查询所有产品
        var _submiting = false;
        function PostGetProItems() {
            if (_submiting) {
                showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting = true;
            showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            var prodtype = $('#PROMISSION_PRO_TYPE').val();
            var objtype = $('#PRO_TYPE').val();

            $.ajax({
                url: options.proItemsUrl,
                type: "POST",
                data: { "prodtype": prodtype, "objtype": objtype },
                success: function (result) {
                    hideLoadingMsg();
                    $("#cuAllProInfos").val(result.data);//大类_小类_编号_名称
                    loadProDdl();//显示产品小类下拉列表                   
                    _submiting = false;
                },
                error: function (result) {
                    hideLoadingMsg();
                    _submiting = false;
                    _showInfoMessage("页面有异常：" + result, 'error');
                }
            });
        }

        //显示产品小类下拉列表
        function loadProDdl() {
            //当前类别的所有产品:大类_小类_编号_名称
            var cuAllPs = $("#cuAllProInfos").val();
            var cuAllPsArray = new Array();
            if (cuAllPs != "") {
                cuAllPsArray = cuAllPs.split(",");
            }

            //当前选中的产品:产品大类_小类_产品编号_产品名称
            var ids = $("#PROD_INFO").val();
            var idArray = new Array();
            if (ids != "") {
                idArray = ids.split(",");
            }

            var hl = "<select class=\"form-control\" id=\"PRO_ITMES\" name=\"PRO_ITMES\">";

            if (cuAllPs != "") {
                for (var i = 0; i < cuAllPsArray.length; i++) {
                    if (cuAllPsArray[i] != undefined) {
                        var re = cuAllPsArray[i].split("_");
                        if ($.inArray(cuAllPsArray[i], idArray) == -1)
                            hl += "<option value=\"" + re[2] + "\">" + re[3] + "</option>";
                    }
                }
            }
            hl += "<select>";
            $("#pro_items_div").html(hl);
        }

        //产品小类选择改变
        util.ProTypeChange = function () {
            PostGetProItems();
        }

        //大类转换
        function formatCP(value) {
            if (value == 0) {
                return "实物产品";
            }
            else if (value == 1) {
                return "卡";
            }
            else if (value == 2) {
                return "项目";
            }
        }

        //展示选中产品列表
        function showSelectProdList() {
            var prodInfoStr = $("#PROD_INFO").val();//对应的产品：产品大类_小类_产品编号_产品名称
            if (prodInfoStr != "") {
                var prodInfos = prodInfoStr.split(",");
                var proListHtml = [];

                //var rowid = 0;
                for (var i = 0; i < prodInfos.length; i++) {
                    var cuObj = prodInfos[i].split("_");
                    if (i % 2 == 0)
                        proListHtml.push("<tr>");
                    else
                        proListHtml.push("<tr class='strip'>");

                    proListHtml.push("<td style=\"width: 0%;\"></td>");
                    proListHtml.push("<td style=\"width: 20%;\"><div style=\"text-align:left;\">", formatCP(cuObj[0]), "</div></td>");
                    proListHtml.push("<td style=\"width: 65%;\"><div style=\"text-align:left;\">", cuObj[3], "</div></td>");
                    proListHtml.push("<td style=\"width: 15%;\"><div style=\"text-align:left;\"><a title='移除' class='abtn' href='javascript:;' onclick=\"util.Remove('", prodInfos[i], "')\"><i class='fa fa-trash-o' ></i>移除</a></div></td>");
                    proListHtml.push("</tr>");
                }

                $("#tableProds tbody").html("");
                $("#tableProds tbody").html(proListHtml.join(""));

                $("#tableProds tr").each(function (i) {
                    $(this).hover(function (e) { $(this).addClass("mhover"); }, function (e) { $(this).removeClass("mhover"); });
                });
            }
            else {
                $("#tableProds tbody").html("");
            }
        }
        showSelectProdList();


        //添加产品
        var _addsubmiting = false;
        $("#btnAddOnePro").click(
         function () {
             var sId = $("#PRO_ITMES").val();//下拉列表选中值
             var sText = $("#PRO_ITMES").find("option:selected").text();
             var sIds = $("#PROD_INFO").val();//当前选中的:产品大类_小类_产品编号_产品名称 
             if (sId != undefined) {
                 if (sIds == "")
                     sIds = $("#PROMISSION_PRO_TYPE").val() + "_" + $("#PRO_TYPE").val() + "_" + sId + "_" + sText;
                 else
                     sIds += "," + $("#PROMISSION_PRO_TYPE").val() + "_" + $("#PRO_TYPE").val() + "_" + sId + "_" + sText;

                 if (_addsubmiting) {
                     //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                     return;
                 }
                 _addsubmiting = true;
                 //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

                 $("#PROD_INFO").val(sIds);
                 showSelectProdList();
                 loadProDdl();
                 _addsubmiting = false;

             }
             else {
                 _showInfoMessage("请先选择产品！", 'success'); 
             }
         }
         );

        //根据"产品大类_产品编号_产品名称 "去掉产品
        var _removesubmiting = false;
        util.Remove = function (pro) {

            var sIds = $("#PROD_INFO").val();//当前选中的:产品大类_小类_产品编号_产品名称             
            var sPros = sIds.split(",");//需要去掉的"产品大类_小类_产品编号_产品名称 "

            var removeIndex = $.inArray(pro, sPros);
            sPros.splice(removeIndex, 1);
            var reSIds = sPros.join(",");
            if (_removesubmiting) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _removesubmiting = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            $("#PROD_INFO").val(reSIds);
            showSelectProdList();
            loadProDdl();
            _removesubmiting = false;
        }
    });
})(window, undefined, jQuery);