
; (function (window, undefined, $) {
   
        $('#BEGIN_DATE_picker').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });
        $('#END_DATE_picker').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });

   

        var submiting = false;
        $('#frmEdit').validator({
            rules: {
            },
            fields: {
                '#PROD_SKU': 'required',
                '#PROD_NAME': 'required',
                '#PROD_TYPE': 'required',
                '#PROD_PRICE': 'required',
                '#BEGIN_DATE': 'required',
                '#END_DATE': 'required',
                '#PROD_PERIOD': 'required',
                '#IS_STAR': 'required',
                '#IS_SALES': 'required',
                '#IS_ALLSTORE': 'required',
                '#IS_CARDPAY': 'required',
                '#IS_DISCOUNT': 'required',
                '#PROD_STATUS': 'required'
               

                //'#Store_ID': 'required;idcard',

            },
            valid: function (form) {
                if (submiting) {
                    return;
                }
                submiting = true;
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        alert("操作成功！");
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
                        showErrorTip("操作失败！：" + res.message, { left: 100, top: 10 }, true, 5000);
                    }
                    submiting = false;
                })
            }
        });
        $("#btnSave").click(function (e) {
            $("#frmEdit").submit();
        });
        $("#btnCancel").click(function (e) {
            if (parent && parent.xjDailog) {
                parent.xjDailog.Close(false, false, null);
            }
            else {
                window.close();
            }
        })
        function cancelM() {
            window.location.href = options.indexUrl;
        }
        $("#btnClose").click(cancelM);

    

        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0].replace("/", "-").replace("/", "-");
            }
            else {
                return "-";
            }
        }



    //门店
        var sto_grid = null;
        $('#tab_store').on('show.bs.tab', function (e) {
            var id = $("#ID").val();
            if (id == "" || id == "0") {
                alert("请先保存产品基础信息！");
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

        //保存适用门店
        $("#btnStoreSave").click(function () {

            //$("select[@name=STORE_INFO_discounts]").each(function (i) {
            //    alert(i);
            //});
            var id = $("#ID").val();
            var url = options.Sto_SaveUrl + "/" + id;
            var data = sto_grid.GetCheckedRowDatas(formatPostStoreData);
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

        function InitStoGrid(id) {
            var gridopt = {
                url: options.Sto_QueryUrl + "/" + id,
                colModel: [
                        { display: '编号', name: 'ID', hide: true, iskey: true },
                        { display: '门店编号', name: 'STORE_NO', align: 'left' },
                        { display: '门店名称', name: 'NAME', align: 'left' },
                        { display: '非繁忙时价格', name: 'PRICE', align: 'left', process: formatDis},
                        { display: '繁忙时价格', name: 'BUSY_PRICE', align: 'left', process: formatDis2 },
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

        function rowStoreBind(rowid, cell) {
            var disinput = $(this).find("input.discounts_input");
            if (disinput.length > 0) { 
                disinput.blur(UpdateBind)
            }

            //var disinput = $(this).find("select.form-control option").val();
            //alert(disinput);
        }
        function UpdateBind() {
            var inid = this.id;
            var tar = inid.split("_");
            var rowid = parseInt(tar[tar.length - 1]);
         
            if (tar[2] == "PRICE") {
                sto_grid.UpdateBind(rowid, 3, this.value); //更新到内部
            }
            else if (tar[2] == "BUSYPRICE") {
                sto_grid.UpdateBind(rowid, 4, this.value); //更新到内部
            }
        }


        function formatPostStoreData(cell) {
            return [cell[0], cell[3], cell[4]].join("_");
        }
        function formatDis(value, cell, rowid) {
            return "<input type='text' class='discounts_input' id='STORE_INFO_PRICE_" + rowid + "' value='" + value + "'/>";
        }

        function formatDis2(value, cell, rowid) {
            return "<input type='text' class='discounts_input' id='STORE_INFO_BUSYPRICE_" + rowid + "' value='" + value + "'/>";
        }



    //设置选择购买产品选中
        function SetBuySelect() {
            var objtype = $('#BCATE').val();
           
            var id = $('#ID').val();
            
            if (objtype != "" && id > 0 && objtype != undefined) {
                //alert(objtype);
                var s = objtype.split("_");
                $("#P_Category").val(s[0]);
                PostGetProdTypes();
            }
        
        }
        SetBuySelect();

        $("#P_Category").change(function () {
            PostGetProdTypes2();
        })

        var _submiting2 = false;
        function PostGetProdTypes() {
            if (_submiting2) {
                showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting2 = true;
            showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
            var pid = $('#P_Category').val();

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

                    var selectV = "";
                    var objtype = $('#BCATE').val();//中医调理_中医调理1

                    if (objtype != "") {
                        var cuV = objtype.split("_");
                        selectV = cuV[2];
                      
                    }


                    var hl = "<select class=\"form-control\" id=\"PRO_TYPE\" name=\"PRO_TYPE\" ><option value=''>请选择产品所属小类</option>";

                    if (cuAllPs != "") {
                        for (var i = 0; i < cuAllPsArray.length; i++) {
                            var re = cuAllPsArray[i].split("_");
                           
                            if (selectV == re[1])
                                hl += "<option value=\"" + re[0] + "\" selected=\"selected\">" + re[1] + "</option>";
                            else
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

                    //var selectV = "";
                    //var objtype = $('#BCATE').val();//中医调理_中医调理1

                    //if (objtype != "") {
                    //    var cuV = objtype.split("_");
                    //    selectV = cuV[2];

                    //}


                    var hl = "<select class=\"form-control\" id=\"PRO_TYPE\" name=\"PRO_TYPE\" ><option value=''>请选择产品所属小类</option>";

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
                    $("#PRO_TYPE").html(hl);
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