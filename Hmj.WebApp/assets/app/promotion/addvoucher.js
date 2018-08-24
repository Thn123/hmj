; (function (window, undefined, $) {

    $(document).ready(function () {


        $("#USER_STORES").select2({ width: 'resolve' });
        if ($("#USE_STORE:checked").attr("value") == "0") {
            $("#storeS").show();
        }
        else {
            $("#storeS").hide();
        }

        var _submiting1 = false;
        function GetAddCategorys() {
            if (_submiting1) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting1 = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            $.ajax({
                url: options.queryLeftUrl,
                type: "POST",
                data: { id: $("#ID").val() },
                success: function (result) {
                    hideLoadingMsg();
                    if (result.status == 0) {
                        //$(".select2-choices").find(".select2-search-choice").remove();
                        //var optsA = result.data, optsA2 = "<option></option>" + optsA;                        
                        $("#USER_STORES").html(result.data);
                        //$("select.populate").each(function () {                            
                        //    $(this).html($(this).hasClass("placeholder") ? optsA2 : optsA);
                        //});
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
        GetAddCategorys();
        //$("#ptree").treeview({
        //    url: options.queryLeftUrl,
        //    showcheck: true,
        //    cascadecheck: true,
        //    onnodeclick: TreeNode_Click,
        //});

        $("input[name='USE_STORE']").change(function () {
            if ($("#USE_STORE:checked").attr("value") == "0") {
                $("#storeS").show();
            }
            else {
                $("#storeS").hide();
            }

        });

        //var cuTDs = $("#ptree").getTreeData();
        //if (cuTDs != undefined && cuTDs[0] != undefined) {
        //    //默认加载第一个菜单数据
        //    $("#REGION_ID1").val(cuTDs[0].id);
        //    $("#h4Dict").html(cuTDs[0].text);
        //    $("#REGION_ID_Str").html(cuTDs[0].text);
        //    $("#REGION_ID").val(cuTDs[0].id);
        //}

        function TreeNode_Click(data) {
            //$("#h4Dict").html(data.text);
            //$("#REGION_ID").val(data.id);
            //$("#REGION_ID1").val(data.id);
            //$("#REGION_ID_Str").html(data.text);
            //SetRightDict();
        }

        if ($("#YES_NO:checked").attr("value") == "1") {
            loadStoreDdl();
            $("#p1").show();
            $("#p2").show();
        }
        else {
            $("#p1").hide();
            $("#p2").hide();
        }
        $('#INVALID_DATE_picker').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });

        $("#p1").hide();
        $("#p2").hide();
        //是否所有项目选中事件
        $("input[name='YES_NO']").change(function () {
            if ($("#YES_NO:checked").attr("value") == "1") {
                loadStoreDdl();
                $("#p1").show();
                $("#p2").show();
            }
            else {
                $("#p1").hide();
                $("#p2").hide();
            }
        });
        var _submiting = false;
        PostGetPStoreValueCountStr();
        function PostGetPStoreValueCountStr() {
            $.ajax({
                url: options.editUrl,
                type: "POST",
                data: {},
                success: function (result) {
                    hideLoadingMsg();
                    $("#STORES_Str").val(result);
                    loadStoreDdl();
                    loadStoreList();
                    _submiting = false;
                },
                error: function (result) {
                    hideLoadingMsg();
                    _submiting = false;
                    _showInfoMessage("页面有异常：" + result, 'error');

                }
            });
        }

        //显示门店下拉列表以及已经发放的门店
        function loadStoreDdl() {
            //当前门店面值数量字符串 ：门店编号_门店名称
            var cuAllPs = $("#STORES_Str").val();
            var cuAllPsArray = new Array();
            if (cuAllPs != "") {
                cuAllPsArray = cuAllPs.split(",");
            }
            //当前选中的店面值数量字符串 ：门店编号_门店名称
            var ids = $("#SELECT_STORES_Str").val();
            var idArray = new Array();
            if (ids != "") {
                idArray = ids.split(",");
            }

            var hl = "<select class=\"form-control\" id=\"STORE_ITMES\" name=\"STORE_ITMES\">";
            if (cuAllPs != "") {
                for (var i = 0; i < cuAllPsArray.length; i++) {
                    if (cuAllPsArray[i] != undefined) {
                        var re = cuAllPsArray[i].split("_");

                        if ($.inArray(cuAllPsArray[i], idArray) == -1)
                            hl += "<option value=\"" + cuAllPsArray[i] + "\">" + re[1] + "</option>";
                    }
                }
            }
            hl += "<select>";
            $("#store_items_div").html(hl);
        }

        function loadStoreList() {

            //当前选中的店面值数量字符串 ：门店编号_面值数量_门店名称
            var idVs = $("#SELECT_STORES_VALUE_Str").val();
            var idVArray = new Array();
            if (idVs != "") {
                idVArray = idVs.split(",");
            }
            var tbSize = $("#tableStoreS tbody tr").size();

            var proListHtml = [];
            if (idVs != "") {
                for (var i = 0; i < idVArray.length; i++) {
                    if (idVArray[i] != undefined) {
                        var newRow = "";
                        var re = idVArray[i].split("_");
                        if (i % 2 == 0) {
                            newRow = "<tr>";
                        }
                        else {
                            newRow = "<tr class='strip'>";
                        }
                        newRow += "<td style=\"width: 60%;\"><div style=\"text-align:left;\">" + re[2] + "</div></td>";
                        newRow += "<td style=\"width: 20%;\"><div style=\"text-align:left;\">" + re[1] + "张</div></td>";
                        newRow += "<td style=\"width: 20%;\"><div style=\"text-align:left;\"><a title='移除' class='abtn' href='javascript:;' onclick=\"util.Remove('" + re[0] + "_" + re[2] + "')\"><i class='fa fa-trash-o' ></i>移除</a></div></td>";
                        newRow += "</tr>";
                        if (i > 0)
                            $("#tableStoreS tbody tr:last").after(newRow);
                        else $("#tableStoreS tbody").append(newRow);
                    }
                }

            }
            else {
                $("#tableStoreS tbody").html("");
            }
        }

        //添加门店
        var _addsubmiting = false;
        $("#btnAddOneStore").click(
         function () {
             var FACE_VALUE = parseInt($("#FACE_VALUE").val());//面值金额             
             if (FACE_VALUE <= 0) {
                 _showInfoMessage("面值金额必须大于0", 'error');
                 return;
             }
             var TotalCount = parseInt($("#TotalCount").val());//面值新增数量           
             var Count1 = parseInt($("#Count1").val());//选中数量             
             var sValueCountStr = $("#SELECT_STORES_VALUE_Str").val();//当前选中的：门店编号_面值数量_门店名称

             //已经选中的总张数
             var vs = new Array();
             var selectC = 0;//已经选中的所有面值张数
             if (sValueCountStr != "") {
                 vs = sValueCountStr.split(",");
                 for (var i = 0; i < vs.length; i++) {
                     if (vs[i] != undefined) {
                         var objs = vs[i].split("_");
                         //面值张数累加
                         selectC += parseInt(objs[1]);
                     }
                 }
             }
             //判断是否超过总张数
             if (selectC + Count1 > TotalCount) {
                 _showInfoMessage("发放张数已超过总张数，请重新填写。", 'error');
                 //alert("发放张数已超过总张数，请重新填写。");
                 return;
             }

             if (Count1 <= 0 || isNaN(Count1)) {
                 _showInfoMessage("发放张数必须大于0。", 'error');
                 return;
             }

             var sValueStr = $("#SELECT_STORES_Str").val();//当前选中的：门店编号_门店名称
             var sValue = $("#STORE_ITMES").val();//下拉列表选中值
             if (sValue != undefined) {
                 var cuVC = sValue.split("_");//当前选中的：门店编号_面值数量_门店名称
                 if (sValueStr == "")
                     sValueStr = sValue;
                 else
                     sValueStr += "," + sValue;

                 //新发放的门店面值和张数
                 if (sValueCountStr == "")
                     sValueCountStr = cuVC[0] + "_" + $("#Count1").val() + "_" + cuVC[1];
                 else
                     sValueCountStr += "," + cuVC[0] + "_" + $("#Count1").val() + "_" + cuVC[1];

                 if (_addsubmiting) {
                     return;
                 }

                 _addsubmiting = true;
                 $("#SELECT_STORES_Str").val(sValueStr);
                 $("#SELECT_STORES_VALUE_Str").val(sValueCountStr);

                 var tbSize1 = $("#tableStoreS tbody tr").size();

                 var newRow = "<tr>";
                 if (tbSize1 % 2 != 0)
                     newRow = "<tr class='strip'>";

                 newRow += "<td style=\"width: 60%;\"><div style=\"text-align:left;\">" + cuVC[1] + "</div></td>";
                 newRow += "<td style=\"width: 20%;\"><div style=\"text-align:left;\">" + $("#Count1").val() + "张</div></td>";
                 newRow += "<td style=\"width: 20%;\"><div style=\"text-align:left;\"><a title='移除' class='abtn' " +
                 "href='javascript:;' onclick=\"util.Remove('" + cuVC[0] + "_" + cuVC[1] + "')\"><i class='fa fa-trash-o' ></i>" +
                 "移除</a></div></td>";
                 newRow += "</tr>";

                 if (tbSize1 > 0)
                     $("#tableStoreS tbody tr:last").after(newRow);
                 else
                     $("#tableStoreS tbody").append(newRow);

                 loadStoreDdl();
                 _addsubmiting = false;
             }
             else {
                 _showInfoMessage("请先选择产品！", 'success');
             }
         });

        //根据"门店编号_面值金额_面值数量_门店名称"去掉产品
        var _removesubmiting = false;
        util.Remove = function (sto) {
            var sIds = $("#SELECT_STORES_Str").val();//当前选中的:门店编号_门店名称            
            var sStos = sIds.split(",");

            var removeIndex = $.inArray(sto, sStos);
            sStos.splice(removeIndex, 1);
            var reSIds = sStos.join(",");
            $("#SELECT_STORES_Str").val(reSIds);

            var sIdCs = $("#SELECT_STORES_VALUE_Str").val();//当前选中的:门店编号_面值数量_门店名称    
            var sStoCs = sIdCs.split(",");

            var reStoCs = "";
            var cuSto = sto.split("_");
            for (var i = 0; i < sStoCs.length; i++) {
                var cuo = sStoCs[i].split("_");
                if (cuo[0] != cuSto[0] && cuo[2] != cuSto[1]) {
                    if (reStoCs == "")
                        reStoCs = sStoCs[i];
                    else
                        reStoCs = reStoCs + "," + sStoCs[i];
                }
            }
            $("#SELECT_STORES_VALUE_Str").val(reStoCs);

            var i = 0;
            var rIndex = 0;
            //删除行
            $("#tableStoreS tbody tr").each(function () {
                //var cuStN = $("#tbValue tbody tr:eq(" + i + ") td:eq(0)").text();
                var cuN = $("#tableStoreS tbody tr:eq(" + i + ") td:eq(0)").find("div").html();
                if (cuSto[1] == cuN)
                    rIndex = i;
                i++;
            });
            $("#tableStoreS tbody tr:eq(" + rIndex + ")").remove();

            if (_removesubmiting) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _removesubmiting = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            loadStoreDdl();
            //loadStoreList();
            _removesubmiting = false;
        }

        //新增代金券================================================
        $("#salePriceD").hide();

        function setCanSale() {
            if ($("#CAN_SALE:checked").attr("value") == "1")
                $("#salePriceD").show();
            else
                $("#salePriceD").hide();
        }

        $("input[name='CAN_SALE']").change(function () {
            setCanSale();
        });

        //保存验证  
        $("#btnAdd").click(function (e) {
            $('#EditModal').modal('show');
        });

        //保存
        $('#btnSave').click(function () {
            $('#frmSave').submit();
        });

        $('#frmSave').validator({
            rules: {
            },
            fields: {
                '#INVALID_DATE_Str': 'required',
                '#FIX_NO': 'required',
                '#StartNo': 'required;integer[+0]',
                '#FACE_VALUE': 'required;integer[+0]',
                '#TotalCount': 'required;integer[+0]',
                '#IS_TRANSFER': 'required;number;',
                '#SALE_PRICE': 'number;',
                '#YES_NO': 'required;',
                '#USE_STORE': 'required;',
            },
            valid: function (form) {
                //如果有发放门店
                if ($("#YES_NO:checked").val() == "1") {
                    if ($("#SELECT_STORES_VALUE_Str").val() == "") {
                        _showInfoMessage("请选择发放的门店！", 'error');
                        return;
                    }
                }
                //如果可以销售
                if ($("#CAN_SALE:checked").val() == "1") {
                    if ($("#SALE_PRICE").val() == "") {
                        _showInfoMessage("请填写销售价格！", 'error');
                        return;
                    }
                } 
                //如果有发放门店
                if ($("#USE_STORE:checked").val() == "0") {
                    if ($("#USER_STORES").val() == null || $("#USER_STORES").val() == undefined || $("#USER_STORES").val() == "") { 
                        _showInfoMessage("请选择使用的门店！", 'error');
                        return;
                    }
                } 
                var StartNo = parseInt($("#StartNo").val());//起始号码             
                if (StartNo <= 0) {
                    _showInfoMessage("起始号码必须大于0", 'error');
                    return;
                }
                var FACE_VALUE = parseInt($("#FACE_VALUE").val());//面值金额             
                if (FACE_VALUE <= 0) {
                    _showInfoMessage("礼券面值必须大于0", 'error');
                    return;
                }
                var TotalCount = parseInt($("#TotalCount").val());//新增数量            
                if (TotalCount <= 0) {
                    _showInfoMessage("新增数量必须大于0", 'error');
                    return;
                }

                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        $('#EditModal').modal('hide');
                        _showInfoMessage("保存成功！", 'success');
                        window.location.href = options.indexUrl;
                    }
                    else {
                        _showInfoMessage("操作失败！：" + res.message, 'error');
                        //showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                    }
                })
            }
        });

        $('#btnCancel').click(function () {
            window.location.href = options.indexUrl;
        });


    });
})(window, undefined, jQuery);