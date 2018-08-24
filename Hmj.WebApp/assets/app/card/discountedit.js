
; (function (window, undefined, $) {
    $(document).ready(function () {

        var submiting = false;
        $('#frmEdit').validator({
            rules: {
            },
            fields: {
                '#NAME': 'required',
                '#PAR_AMT': 'required;number;range[0+]',
                '#PRICE': 'required;number;range[0+]',
                '#DISCOUNT_RATE': 'required;number;range[0~1]',
                '#PERIOD': 'required;integer[+0]',
            },
            valid: function (form) {
                if (submiting) {
                    return;
                }
                submiting = true;
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        //alert("操作成功！");
                        _showInfoMessage("操作成功！", 'success');
                        window.location.href = "/Card/discount.do";
                        $("#ID").val(res.data);
                    }
                    else {
                        showErrorTip("操作失败！：" + res.message, { left: 400, top: 450 }, true, 5000);
                    }
                    submiting = false;
                })
            }
        });
        $("#btnSave").click(function (e) {
            $("#frmEdit").submit();
        });

        function cancelM() {
            window.location.href = options.indexUrl;
        }
        $("#btnCancel").click(cancelM);


        //服务
        var s_grid = null;
        var p_grid = null;
        var sto_grid = null;
        $('#tab_service').on('show.bs.tab', function (e) {
            var id = $("#ID").val();
            if (id == "" || id == "0") {
                alert("请先保存卡片信息！");
                e.preventDefault();
                return false;
            }
        });
        $('#tab_service').on('shown.bs.tab', function (e) {
            var id = $("#ID").val();
            if (id != "" && id != "0") {
                if (s_grid == null) {
                    InitSGrid(id);
                }
            }
        });

        $('#tab_product').on('show.bs.tab', function (e) {
            var id = $("#ID").val();
            if (id == "" || id == "0") {
                alert("请先保存卡片信息！");
                e.preventDefault();
                return false;
            }
        });
        $('#tab_product').on('shown.bs.tab', function (e) {
            var id = $("#ID").val();
            if (id != "" && id != "0") {
                if (p_grid == null) {
                    InitPGrid(id);
                }
            }
        });


        $('#tab_store').on('show.bs.tab', function (e) {
            var id = $("#ID").val();
            if (id == "" || id == "0") {
                alert("请先保存卡片信息！");
                e.preventDefault();
                return false;
            }
        });
        $('#tab_store').on('shown.bs.tab', function (e) {
            var id = $("#ID").val();
            if (id != "" && id != "0") {
                if (sto_grid == null) {
                    InitStoGrid(id);
                }

            }
        });

        $("#service_frm_q").submit(function () {
            s_grid.Query(this);
            return false;
        })

        $("#S_Filter").click(function () {
            $("#service_frm_q").submit();
        });


        $("#product_frm_q").submit(function () {
            p_grid.Query(this);
            return false;
        })

        $("#P_Filter").click(function () {
            $("#product_frm_q").submit();
        });

        //$("#S_Filter").click(function (e) {
        //    var search = [null];
        //    var qText = $("#S_QTEXT").val();
        //    if (qText != "") {
        //        search.push(qText);
        //    }
        //    else {
        //        search.push(null);
        //    }
        //    search.push(null,null);        
        //    var bCate = $("#TYPE").val();
        //    //alert(bCate);
        //    if (bCate != "") {
        //        search.push(bCate);
        //    }
        //    else {
        //        search.push(null);
        //    }

        //    var sCate = $("#PRO_TYPE").val();
        //    if (sCate != "") {
        //        search.push(sCate);
        //    }
        //    else {
        //        search.push(null);
        //    }
        //    s_grid.Fitler(search, [-1, 1, -1, -1, 0]);
        //});


        //$("#P_Filter").click(function (e) {
        //    var search = [null];
        //    var qText = $("#P_QTEXT").val();
        //    if (qText != "") {
        //        search.push(qText);
        //    }
        //    else {
        //        search.push(null);
        //    }
        //    search.push(null, null);
        //    var qCate = $("#P_Category").val();
        //    if (qCate != "") {
        //        search.push(qCate);
        //    }
        //    else {
        //        search.push(null);
        //    }
        //    p_grid.Fitler(search, [-1, 1, -1, -1, 0]);
        //});


        $("#btnServiceSave").click(function () {
            var id = $("#ID").val();
            var url = options.S_SaveUrl + "/" + id;
            var data = s_grid.GetCheckedRowDatas(formatPostData);
            var postData = data.join(",");
            PostSave(postData, url);
        });
        $("#btnProductSave").click(function () {
            var id = $("#ID").val();
            var url = options.P_SaveUrl + "/" + id;
            var data = p_grid.GetCheckedRowDatas(formatPostData);
            var postData = data.join(",");
            PostSave(postData, url);
        });
        //保存适用门店
        $("#btnStoreSave").click(function () {

            //$("select[@name=STORE_INFO_discounts]").each(function (i) {
            //    alert(i);
            //});
            var id = $("#ID").val();
            var url = options.Sto_SaveUrl + "/" + id;
            var data = sto_grid.GetCheckedRowDatas(formatPostStoreData);
            //alert(data);
            //var cbxids = data.toString().split(',');
            //var newdata = [];
            //for (var i = 0; i < cbxids.length; i++)
            //{
            //    var obj = "#STORE_INFO_cb_" + (parseInt(cbxids[i])-1).toString();
            //    var _cbx_sales = $(obj).parent("div").parent("td").parent("tr").children("td").eq(3).find("select[NAME$=STOREYES_INFO]").children('option:selected').val();
            //    var _cbx_used = $(obj).parent("div").parent("td").parent("tr").children("td").eq(4).find("select[NAME$=STOREYES_INFO]").children('option:selected').val();

            //    newdata.push([cbxids[i], _cbx_sales, _cbx_used].join("_"));
            //}
            //alert(newdata);
            //STORE_INFO_cb_21
            //return false;
            var postData = data.join(",");
            PostSave(postData, url);
        });



        var _submiting = false;
        function PostSave(postData, url) {
            if (_submiting) {
                showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting = true;
            showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
            $.ajax({
                url: url,
                type: "POST",
                data: { "formdata": postData },
                success: function (result) {
                    hideLoadingMsg();
                    if (result.status == 0) {
                        alert("操作成功！");
                    }
                    else {
                        showErrorTip("操作失败！：" + res.message, { left: 400, top: 450 }, true, 5000);
                    }
                    _submiting = false;
                },
                error: function (result) {
                    hideLoadingMsg();
                    _submiting = false;
                    alert("提交表单失败：" + result);
                }
            });
        }
        function formatPostData(cell) {
            return [cell[0], cell[3]].join("_");
        }

        function formatPostStoreData(cell) {
            return [cell[0], cell[3], cell[4], cell[5]].join("_");
            // return [cell[0]];
        }
        function InitSGrid(id) {
            var gridopt = {
                url: options.S_QueryUrl + "/" + id,
                colModel: [
                        { display: '编号', name: 'ItemId', hide: true, iskey: true },
                        { display: '名称', name: 'ItemName', width: '40%', align: 'left' },
                        { display: '类别', name: 'Cate', width: '30%', align: 'left' },
                        { display: '折扣', name: 'Discounts', align: 'left', process: formatDis },
                        { display: '是否选择', name: 'IsChecked', hide: true },
                        { display: '分类ID', name: 'CategoryID', hide: true }
                ],
                title: false,
                rp: 10,
                usepager: true,
                localpage: true,
                rowhanlder: rowBind,
                showcheckbox: true
            };
            s_grid = new xjGrid("SERVICE_INFO", gridopt);
        }
        function InitPGrid(id) {
            var gridopt = {
                url: options.P_QueryUrl + "/" + id,
                colModel: [
                        { display: '编号', name: 'ItemId', hide: true, iskey: true },
                        { display: '名称', name: 'ItemName', align: 'left' },
                         { display: '类别', name: 'Cate', width: '30%', align: 'left' },
                        { display: '折扣', name: 'Discounts', align: 'left', process: formatDis2 },
                        { display: '是否选择', name: 'IsChecked', hide: true }
                ],
                title: false,
                rp: 10,
                localpage: true,
                rowhanlder: rowBind,
                usepager: true,
                showcheckbox: true
            };
            p_grid = new xjGrid("PRODUCT_INFO", gridopt);
        }
        function InitStoGrid(id) {
            var gridopt = {
                url: options.Sto_QueryUrl + "/" + id,
                colModel: [
                        { display: '编号', name: 'ID', hide: true, iskey: true },
                        { display: '门店编号', name: 'STORE_NO', align: 'left' },
                        { display: '门店名称', name: 'NAME', align: 'left' },
                        { display: '允许销售', name: 'IS_SALES', align: 'left', process: formatDis3 },
                          { display: '销售最大数', name: 'SALE_MAX', align: 'left', width: "10%", process: formatDis5 },
                        { display: '允许使用', name: 'IS_USERD', align: 'left', process: formatDis4 },
                        { display: '是否选择', name: 'IsChecked', hide: true }
                ],
                title: false,
                rp: 10,
                localpage: true,
                rowhanlder: rowStoreBind,
                usepager: true,
                showcheckbox: true
            };
            sto_grid = new xjGrid("STORE_INFO", gridopt);
        }

        function rowBind(rowid, cell) {
            var disinput = $(this).find("input.discounts_input");
            if (disinput.length > 0) {
                disinput.blur(UpdateBind)
            }
        }


        function rowStoreBind(rowid, cell) {
            var disinput = $(this).find("select");
            var disinput2 = $(this).find("input.discounts_input");
            if (disinput.length > 0) {
                disinput.blur(UpdateBind)
            }
            if (disinput2.length > 0) {
                disinput2.blur(UpdateBind)
            }

            //var disinput = $(this).find("select.form-control option").val();
            //alert(disinput);
        }
        function UpdateBind() {
            var inid = this.id;
            var tar = inid.split("_");
            var rowid = parseInt(tar[tar.length - 1]);
            if (tar[0] == "SERVICE") {//服务列表
                s_grid.UpdateBind(rowid, 3, this.value); //更新到内部
            }
            else if (tar[0] == "PRODUCT") {//产品列表
                p_grid.UpdateBind(rowid, 3, this.value); //更新到内部
            }
            else if (tar[0] == "SALES") {
                //alert(this.value);
                sto_grid.UpdateBind(rowid, 3, this.value); //更新到内部
            }
            else if (tar[0] == "USED") {
                sto_grid.UpdateBind(rowid, 5, this.value); //更新到内部
            }
            else if (tar[0] == "SALEMAX") {
                sto_grid.UpdateBind(rowid, 4, this.value); //更新到内部
            }

        }
        function formatDis(value, cell, rowid) {
            return "<input type='text' class='discounts_input' id='SERVICE_INFO_discounts_" + rowid + "' value='" + value + "'/>";
        }
        function formatDis2(value, cell, rowid) {
            return "<input type='text' class='discounts_input' id='PRODUCT_INFO_discounts_" + rowid + "' value='" + value + "'/>";
        }
        function formatDis5(value, cell, rowid) {
            return "<input type='text' class='discounts_input' id='SALEMAX_INFO_discounts_" + rowid + "' value='" + value + "'/>";
        }

        function formatDis3(value, cell, rowid) {
            if (value == "True") {

                return "<select type='text' class='form-control'  id='SALES_INFO_discounts_" + rowid + "'  ><option  value='1' selected='true'>是</option><option  value='0' >否</option> </select>";
            }
            else

                return "<select type='text' class='form-control' id='SALES_INFO_discounts_" + rowid + "'><option  value='1'>是</option><option  value='0' selected='true'>否</option> </select>";


        }

        function formatDis4(value, cell, rowid) {
            if (value == "True") {
                return "<select type='text' class='form-control'  id='USED_INFO_discounts_" + rowid + "' ><option  value='1' selected='true'>是</option><option  value='0' >否</option> </select>";
            }
            else

                return "<select type='text' class='form-control' id='USED_INFO_discounts_" + rowid + "'><option  value='1'>是</option><option  value='0' selected='true'>否</option> </select>";
        }


        _utils.UploadCallback = function (fileid, url, id) {
            $("#IMAGE_SHOW").attr("src", url);
            $("#IMAGE_ID").val(id);
        };
    });


    $("#BCATE").change(function () {
        PostGetProdTypes();
    })

    var _submiting2 = false;
    function PostGetProdTypes() {
        if (_submiting2) {
            //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
            return;
        }
        _submiting2 = true;
        //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
        var pid = $('#BCATE').val();
        $.ajax({
            url: options.proTypeUrl,
            type: "POST",
            data: { "PID": pid },
            success: function (result) {

                hideLoadingMsg();
                //大类
                var cuAllPs = result.data;
                var cuAllPsArray = new Array();
                if (cuAllPs != "") {
                    cuAllPsArray = cuAllPs.split(",");

                }
                var hl = "<select class=\"form-control\" id=\"PRO_TYPE\" name=\"SCATE\" ><option value=''>请选择项目小类</option>";

                if (cuAllPs != "") {
                    for (var i = 0; i < cuAllPsArray.length; i++) {
                        var re = cuAllPsArray[i].split("_");
                        hl += "<option value=\"" + re[0] + "\">" + re[1] + "</option>";

                    }
                }
                hl += "<select>";
                $("#PRO_TYPE").html(hl);
                _submiting2 = false;
            }
            ,
            error: function (result) {
                hideLoadingMsg();
                _submiting2 = false;
                _showInfoMessage("页面有异常：" + result, 'error');

            }
        });
    }



    $("#P_Category").change(function () {
        PostGetProdTypes2();
    })


    var _submiting3 = false;
    function PostGetProdTypes2() {
        if (_submiting3) {
            showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
            return;
        }
        _submiting3 = true;
        showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
        var pid = $('#P_Category').val();

        $.ajax({
            url: options.proTypeUrl2,
            type: "POST",
            data: { "PID": pid },
            success: function (result) {

                hideLoadingMsg();
                //大类
                var cuAllPs = result.data;

                var cuAllPsArray = new Array();
                if (cuAllPs != "") {
                    cuAllPsArray = cuAllPs.split(",");

                }

                var hl = "<select class=\"form-control\" id=\"PSCATE\" name=\"PSCATE\" ><option value=''>请选择产品小类</option>";
                if (cuAllPs != "") {
                    for (var i = 0; i < cuAllPsArray.length; i++) {
                        var re = cuAllPsArray[i].split("_");

                        //if (selectV == re[1])
                        //    hl += "<option value=\"" + re[0] + "\" selected=\"selected\">" + re[1] + "</option>";
                        //else
                        hl += "<option value=\"" + re[0] + "\">" + re[1] + "</option>";

                    }
                }
                hl += "<select>";
                $("#PSCATE").html(hl);
                _submiting3 = false;
            }
            ,
            error: function (result) {
                hideLoadingMsg();
                _submiting3 = false;
                _showInfoMessage("页面有异常：" + result, 'error');

            }
        });
    }




})(window, undefined, jQuery);