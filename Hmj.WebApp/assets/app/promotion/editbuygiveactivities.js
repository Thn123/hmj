; (function (window, undefined, $) {
    $(document).ready(function () {

        //保存验证
        $('#frmSave').validator({
            rules: {
            },
            fields: {
                '#STATUS': 'required',
                '#CHANNEL': 'required',
                '#NAME': 'required',
                '#DISCOUNT_RATE': 'required',
                '#CUST_GROUP_ID': 'required',
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        if ($("#ID").val() == 0) {
                            $("#ID").val(res.data);
                            _showInfoMessage("保存成功！", 'success');
                            window.location.href = options.indexUrl;
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
            if ($("#BEGIN_DATE_Str").val() == "") {
                _showInfoMessage("请选择有效期开始时间！", 'error');
                return;
            }
            if ($("#END_DATE_Str").val() == "") {
                _showInfoMessage("请选择有效期结束时间！", 'error');
                return;
            }

            if ($("#PROMISSION_PRO_TYPE").val() == undefined) {
                _showInfoMessage("请选择购买项目！", 'error'); 
                return;
            }

            if ($("#PRO_ITMES").val() == undefined) {
                _showInfoMessage("请选择购买项目！", 'error'); 
                return;
            }

            if ($("#PROD_INFO_GIVE").val() == "") {
                _showInfoMessage("请选择赠送项目！", 'error'); 
                return;
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

        $('#YXDATE_Datepicker').datepicker();

        //取消
        $('#btnCloseFrm').click(function () {
            window.location.href = options.indexUrl;
        });


        //购买项目=======================================================================================
        //获取小分类下拉选框列表
        var _submiting2 = false;
        function PostGetProdTypes() {
            if (_submiting2) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting2 = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
            var type = $('#PROMISSION_PRO_TYPE').val();
            $.ajax({
                url: options.proTypeUrl,
                type: "POST",
                data: { "type": type },
                success: function (result) {
                    hideLoadingMsg();
                    //类别
                    //alert(result);
                    var cuAllPs = result.data;
                    var cuAllPsArray = new Array();
                    if (cuAllPs != "") {
                        cuAllPsArray = cuAllPs.split(",");
                    }
                    var selectV = "";
                    var objtype = $('#PROD_INFO').val();
                    if (objtype != "") {
                        var cuV = objtype.split("_");
                        selectV = cuV[1];
                        //alert(objtype);
                        //$("#PROMISSION_PRO_TYPE").val(cuV[0]);
                        //$("#PRO_TYPE").val(cuV[1]);
                        //$("#PRO_ITMES").val(cuV[2]);

                    }

                    var hl = "<select class=\"form-control\" id=\"PRO_TYPE\" name=\"PRO_TYPE\" onchange=\"util.ProTypeChange()\">";

                    if (cuAllPs != "") {
                        for (var i = 0; i < cuAllPsArray.length; i++) {
                            var re = cuAllPsArray[i].split("_");
                            if (selectV == re[0])
                                hl += "<option value=\"" + re[0] + "\" selected=\"selected\">" + re[1] + "</option>";
                            else
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
        //产品小类选择改变
        util.ProTypeChange = function () {
            PostGetProItems();
        }

        //显示产品小类下拉列表
        function loadProDdl() {
            //当前类别的所有产品:大类_小类_编号_名称
            var cuAllPs = $("#cuAllProInfos").val();
            var cuAllPsArray = new Array();
            if (cuAllPs != "") {
                cuAllPsArray = cuAllPs.split(",");
            }

            var selectV = "";
            var objtype = $('#PROD_INFO').val();
            if (objtype != "") {
                var cuV = objtype.split("_");
                selectV = cuV[2];
            }

            var hl = "<select class=\"form-control\" id=\"PRO_ITMES\" name=\"PRO_ITMES\">";

            if (cuAllPs != "") {
                for (var i = 0; i < cuAllPsArray.length; i++) {
                    if (cuAllPsArray[i] != undefined) {
                        var re = cuAllPsArray[i].split("_");
                        if (selectV == re[2])
                            hl += "<option value=\"" + re[2] + "\" selected=\"selected\">" + re[3] + "</option>";
                        else
                            hl += "<option value=\"" + re[2] + "\">" + re[3] + "</option>";

                    }
                }


            }
            hl += "<select>";
            $("#pro_items_div").html(hl);
            //itemChangeM();
        }


        //设置选择购买产品选中
        function SetBuySelect() {
            var objtype = $('#PROD_INFO').val();
            if (objtype != "") {
                var cuV = objtype.split("_");
                $("#PROMISSION_PRO_TYPE").val(cuV[0]);
                PostGetProdTypes();
            }
        }
        SetBuySelect();
        //赠送项目=======================================================================================
        //获取小分类下拉选框列表
        var _submiting2 = false;
        function PostGetProdTypes_GIVE() {
            if (_submiting2) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting2 = true;
            var type = $('#PROMISSION_PRO_TYPE_GIVE').val();
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

                    var hl = "<select class=\"form-control\" id=\"PRO_TYPE_GIVE\" name=\"PRO_TYPE_GIVE\" onchange=\"util.ProTypeChange_GIVE()\">";

                    if (cuAllPs != "") {
                        for (var i = 0; i < cuAllPsArray.length; i++) {
                            var re = cuAllPsArray[i].split("_");
                            hl += "<option value=\"" + re[0] + "\">" + re[1] + "</option>";
                        }
                    }
                    hl += "<select>";
                    $("#pro_type_div_GIVE").html(hl);

                    PostGetProItems_GIVE();

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
        $("#PROMISSION_PRO_TYPE_GIVE").change(function () {
            PostGetProdTypes_GIVE();
        });

        //根据大小类下拉产品类型查询所有产品
        var _submiting = false;
        function PostGetProItems_GIVE() {
            if (_submiting) {
                showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting = true;
            showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            var prodtype = $('#PROMISSION_PRO_TYPE_GIVE').val();
            var objtype = $('#PRO_TYPE_GIVE').val();

            $.ajax({
                url: options.proItemsUrl,
                type: "POST",
                data: { "prodtype": prodtype, "objtype": objtype },
                success: function (result) {
                    hideLoadingMsg();
                    $("#cuAllProInfosGive").val(result.data);//大类_编号_名称
                    loadProDdl_GIVE();//显示产品小类下拉列表                   
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
        function loadProDdl_GIVE() {
            //当前类别的所有产品:大类_编号_名称
            var cuAllPs = $("#cuAllProInfosGive").val();
            var cuAllPsArray = new Array();
            if (cuAllPs != "") {
                cuAllPsArray = cuAllPs.split(",");
            }

            //当前选中的产品:产品大类_产品编号_产品名称
            var ids = $("#PROD_INFO_GIVE").val();
            var idArray = new Array();
            if (ids != "") {
                idArray = ids.split(",");
            }

            var hl = "<select class=\"form-control\" id=\"PRO_ITMES_GIVE\" name=\"PRO_ITMES_GIVE\">";

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
            $("#pro_items_div_GIVE").html(hl);
        }

        //产品小类选择改变
        util.ProTypeChange_GIVE = function () {
            PostGetProItems_GIVE();
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
        function showSelectProdList_GIVE() {
            var prodInfoStr = $("#PROD_INFO_GIVE").val();//对应的产品：产品大类_产品编号_产品名称
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
                    proListHtml.push("<td style=\"width: 15%;\"><div style=\"text-align:left;\"><a title='移除' class='abtn' href='javascript:;' onclick=\"util.Remove_GIVE('", prodInfos[i], "')\"><i class='fa fa-trash-o' ></i>移除</a></div></td>");
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
        showSelectProdList_GIVE();


        //添加产品
        var _addsubmiting = false;
        $("#btnAddOnePro").click(
         function () {
             var sId = $("#PRO_ITMES_GIVE").val();//下拉列表选中值
             var sText = $("#PRO_ITMES_GIVE").find("option:selected").text();
             var sIds = $("#PROD_INFO_GIVE").val();//当前选中的:产品大类_产品编号_产品名称 
             if (sId != undefined) {
                 if (sIds == "")
                     sIds = $("#PROMISSION_PRO_TYPE_GIVE").val() + "_" + $("#PRO_TYPE_GIVE").val() + "_" + sId + "_" + sText;
                 else
                     sIds += "," + $("#PROMISSION_PRO_TYPE_GIVE").val() + "_" + $("#PRO_TYPE_GIVE").val() + "_" + sId + "_" + sText;

                 if (_addsubmiting) {
                     //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                     return;
                 }
                 _addsubmiting = true;
                 //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

                 $("#PROD_INFO_GIVE").val(sIds);
                 showSelectProdList_GIVE();
                 loadProDdl_GIVE();
                 _addsubmiting = false;

             }
             else {
                 _showInfoMessage("请先选择项目", 'error'); 
             }
         }
         );

        //根据"产品大类_产品编号_产品名称 "去掉产品
        var _removesubmiting = false;
        util.Remove_GIVE = function (pro) {

            var sIds = $("#PROD_INFO_GIVE").val();//当前选中的:产品大类_产品编号_产品名称             
            var sPros = sIds.split(",");//需要去掉的"产品大类_产品编号_产品名称 "

            var removeIndex = $.inArray(pro, sPros);
            sPros.splice(removeIndex, 1);
            var reSIds = sPros.join(",");
            if (_removesubmiting) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _removesubmiting = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            $("#PROD_INFO_GIVE").val(reSIds);
            showSelectProdList_GIVE();
            loadProDdl_GIVE();
            _removesubmiting = false;
        }

    });
})(window, undefined, jQuery);