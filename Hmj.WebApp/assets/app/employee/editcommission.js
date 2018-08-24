; (function (window, undefined, $) {

    $(document).ready(function () {

        //方案详细保存验证-------------------------------------------------------------------------------
        $('#BEGIN_DATE_picker').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });

        $('#END_DATE_picker').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });

        $("#btnSaveFaxx").click(function (e) {
            $("#frmFaxx").submit();
        });

        $('#frmFaxx').validator({
            rules: {
            },
            fields: {
                '#NAME': 'required',
                '#STATUS': 'required',
                '#BEGIN_DATE_Str': 'required',
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {

                        if ($("#ID").val() == 0) {
                            _showInfoMessage("保存成功", 'success');
                            if ($("#VIWEPAGE").val() == "1") {
                                $("#ID").val(res.data);
                                $("#ID1").val(res.data);
                                $("#ID2").val(res.data);
                                $("#ID3").val(res.data);
                                $("#ID4").val(res.data);
                                $("#Faxx").removeClass("active");
                                $("#Fwxm").addClass("active");

                                $("#LiFaxx").removeClass("active");
                                $("#LiFwxm").addClass("active");

                                $("#LiFwxm").html("<a id=\"aFwxm\" href=\"#Fwxm\" data-toggle=\"tab\">服务项目</a>");
                                $("#LiCp").html("<a id=\"aCp\" href=\"#Cp\" data-toggle=\"tab\">产品</a>");
                                $("#LiHyk").html("<a id=\"aHyk\" href=\"#Hyk\" data-toggle=\"tab\">会员卡</a>");
                                $("#LiLck").html("<a id=\"aLck\" href=\"#Lck\" data-toggle=\"tab\">疗程卡</a>");
                                $("#aFwxm").click(fwxm);
                                $("#aCp").click(cp);
                                $("#aHyk").click(hyk);
                                $("#aLck").click(lck);
                                fwxm();
                            }
                            else if ($("#VIWEPAGE").val() == "2") {
                                $("#ID").val(res.data);

                                $("#Faxx").removeClass("active");
                                $("#Yjtc").addClass("active");

                                $("#LiFaxx").removeClass("active");
                                $("#LiYjtc").addClass("active");

                                $("#LiYjtc").html("<a id=\"aYjtc\" href=\"#Yjtc\" data-toggle=\"tab\">业绩提成</a>");
                            }

                        }
                        else {
                            _showInfoMessage("修改成功！", 'success');

                        }
                    }
                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');
                    }
                })
            }
        });


        //服务项目，产品，卡公用 begin=================================================================================================

        function processTcfs(value, cell, rowid) {
            var ops = [];
            //if (value != 0)
            //    ops.push("&nbsp;<select name=\"" + cell[3] + "\" style=\"color:red;\">");
            //else
            ops.push("&nbsp;<select name=\"" + cell[3] + "\">");
            ops.push("<option " + (value == 0 ? "selected=\"selected\"" : "") + " value=\"" + rowid + "_0\">选择提成方式..</option>");
            ops.push("<option " + (value == 1 ? "selected=\"selected\"" : "") + " value=\"" + rowid + "_1\">无提成</option>");
            ops.push("<option " + (value == 2 ? "selected=\"selected\"" : "") + " value=\"" + rowid + "_2\">按比例提成</option>");
            ops.push("<option " + (value == 3 ? "selected=\"selected\"" : "") + " value=\"" + rowid + "_3\">按固定金额提成</option>");
            ops.push("</select>");
            return ops.join("");
        }
        //value=\"" + value + "\"
        function processTcje(value, cell, rowid) {
            var ops = [];
            ops.push("&nbsp;<input class='discounts_input' style=\"text-align:right\" id=\"COMM_AMT_" + cell[3] + "_" + rowid + "\" type=\"text\" />");

            return ops.join("");
        }
        //value=\"" + value + "\"
        function processTcje1(value, cell, rowid) {
            var ops = [];
            ops.push("&nbsp;<input class='discounts_input' style=\"text-align:right\" id=\"COMM_DK_" + cell[3] + "_" + rowid + "\" type=\"text\" />");
            return ops.join("");
        }

        function rowBind(rowid, cell) {

            if (cell != undefined) {
                var disinput = $(this).find("input.discounts_input");
                var dropC = $(this).find("select");
                var ck = $(this).find("input:checkbox");

                //下拉列表初始化              
                if (dropC.length > 0) {
                    if (cell.cell[4] != 2) {

                        $(disinput[0]).val(parseInt(cell.cell[5]));
                        $(disinput[1]).val(parseInt(cell.cell[6]));
                        disinput.each(function () {
                            $(this).inputmask("integer", { "placeholder": "" });
                        });
                    }
                    else {
                        $(disinput[0]).val(cell.cell[5]);
                        $(disinput[1]).val(cell.cell[6]);
                        disinput.each(function () {
                            $(this).inputmask("99.9%", { "placeholder": "" });
                        });
                    }
                    dropC.change(function () {

                        //下拉框 
                        var inid = $(this).val();
                        var tar = inid.split("_");
                        var sT = tar[tar.length - 1];

                        //$(dropC).css("color", "red");

                        if (sT == 2) {
                            disinput.each(function () {
                                $(this).inputmask("99.9%", { "placeholder": "" });
                            });
                        }
                        else {
                            
                            disinput.each(function () {
                                $(this).inputmask("integer", { "placeholder": "" });
                            });
                            if (sT == 0 || sT == 1) {
                                disinput.each(function () {
                                    $(this).val(0);

                                });
                            }
                        }

                        var typeV = parseInt($(this).attr("name"));
                        if (typeV == 1) {
                            xjgridFwxm.UpdateBind(rowid, 4, sT); //更新到内部
                        }
                        else if (typeV == 2) {
                            xjgridCp.UpdateBind(rowid, 4, sT); //更新到内部 
                        }
                        else if (typeV == 3) {
                            xjgridHyk.UpdateBind(rowid, 4, sT); //更新到内部                 
                        }
                        else if (typeV == 4) {
                            xjgridLck.UpdateBind(rowid, 4, sT); //更新到内部 
                        }


                        //设置默认选中 
                        if (ck != undefined) {
                            if (cell.cell[4] != 0) {
                                cell.check_state = 1;
                                $(ck).prop("checked", true);
                            }
                            else {
                                $(ck).prop("checked", false);
                            }

                        }
                    });
                }

                if (disinput.length > 0) {
                    $(disinput[0]).change(function () {

                        var inid = this.id;
                        var tar = inid.split("_");
                        var rowid = parseInt(tar[tar.length - 1]);
                        var typeV = parseInt(tar[tar.length - 2]);

                        if (typeV == 1) {
                            xjgridFwxm.UpdateBind(rowid, 5, this.value); //更新到内部
                        }
                        else if (typeV == 2) {
                            xjgridCp.UpdateBind(rowid, 5, this.value); //更新到内部
                        }
                        else if (typeV == 3) {
                            xjgridHyk.UpdateBind(rowid, 5, this.value); //更新到内部
                        }
                        else if (typeV == 4) {
                            xjgridLck.UpdateBind(rowid, 5, this.value); //更新到内部
                        }
                    });
                    $(disinput[1]).change(function () {

                        var inid = this.id;
                        var tar = inid.split("_");
                        var rowid = parseInt(tar[tar.length - 1]);
                        var typeV = parseInt(tar[tar.length - 2]);

                        if (typeV == 1) {
                            xjgridFwxm.UpdateBind(rowid, 6, this.value); //更新到内部
                        }
                        else if (typeV == 2) {
                            xjgridCp.UpdateBind(rowid, 6, this.value); //更新到内部
                        }
                        else if (typeV == 3) {
                            xjgridHyk.UpdateBind(rowid, 6, this.value); //更新到内部
                        }
                        else if (typeV == 4) {
                            xjgridLck.UpdateBind(rowid, 6, this.value); //更新到内部
                        }
                    });
                }
            }
        }

        //服务项目，产品，卡公用 end=================================================================================================

        //服务项目列表-----------------------------------------------------------------------------------------------------
        var gridoptFwxm = {
            url: options.detailLisUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0%", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '佣金编号', name: 'PROD_ID', width: "0%", sortable: true, hide: true, align: 'left' },
                    { display: '名称', name: 'PROD_NAME', width: "45%", sortable: false, align: 'left' },
                    { display: '佣金类型', name: 'PROD_TYPE', width: "0%", sortable: false, hide: true, align: 'left' },
                    { display: '提成方式', name: 'COMM_TYPE', width: "10%", sortable: false, align: 'left', process: processTcfs },
                    { display: '提成金额/比例', name: 'COMMISSION', width: "15%", sortable: false, align: 'left', process: processTcje },
                    { display: '点客提成金额/比例', name: 'COMM_DK', width: "15%", sortable: false, align: 'left', process: processTcje1 },
                    { display: '是否选择', name: 'IsChecked', width: "0%", sortable: true, hide: true, align: 'left' },
                    { display: '服务类别', name: 'ObjDetailPro1', width: "0%", sortable: false, hide: true, align: 'left' },
            ],
            title: false,
            rp: 10,
            localpage: true,
            rowhanlder: rowBind,
            usepager: true,
            showcheckbox: true,
            autoload: false
        };
        var xjgridFwxm = new xjGrid("gridlistFwxm", gridoptFwxm);
        function fwxm() {
            xjgridFwxm.QueryByFields([
              { name: "SType", value: 1 },
              { name: "CommissionId", value: $("#ID").val() },
            ]);
        }
        $("#aFwxm").click(fwxm);

        //服务项目查询
        $("#Fwxm_Filter").click(Fwxm_Filter);

        function Fwxm_Filter() {
            var search = [null, null];
            var qText = $("#Service_Name").val();
            if (qText != "") {
                search.push(qText);
            }
            else {
                search.push(null);
            }
            search.push(null, null, null, null, null);

            var qCate = $("#SER_TYPE").val();
            if (qCate != "") {
                search.push(qCate);
            }
            else {
                search.push(null);
            }
            //0全字匹配，1模糊匹配，-1不查询
            xjgridFwxm.Fitler(search, [-1, -1, 1, -1, -1, -1, -1, -1, 0]);
        }

        //产品列表-----------------------------------------------------------------------------------------------------
        var gridoptCp = {
            url: options.detailLisUrl + "?DType=2",
            colModel: [
                      { display: '编号', name: 'ID', width: "0%", sortable: true, hide: true, align: 'left', iskey: true },
                      { display: '佣金编号', name: 'PROD_ID', width: "0%", sortable: true, hide: true, align: 'left' },
                      { display: '名称', name: 'PROD_NAME', width: "45%", sortable: false, align: 'left' },
                      { display: '佣金类型', name: 'PROD_TYPE', width: "0%", sortable: true, hide: true, align: 'left' },
                      { display: '提成方式', name: 'COMM_TYPE', width: "10%", sortable: false, align: 'left', process: processTcfs },
                      { display: '提成金额/比例', name: 'COMMISSION', width: "15%", sortable: false, align: 'left', process: processTcje },
                      { display: '点客提成金额/比例', name: 'COMM_DK', width: "15%", sortable: false, align: 'left', process: processTcje1 },
                      { display: '是否选择', name: 'IsChecked', width: "0%", sortable: true, hide: true, align: 'left' },
                      { display: '产品类别', name: 'ObjDetailPro1', width: "0%", sortable: false, hide: true, align: 'left' },
                        //{ display: '操作', name: 'ID', sortable: false, align: 'center', process: processOp } , process: processTcje
            ],
            title: false,
            rp: 10,
            localpage: true,
            rowhanlder: rowBind,
            usepager: true,
            showcheckbox: true,
            autoload: false
        };
        var xjgridCp = new xjGrid("gridlistCp", gridoptCp);
        function cp() {
            xjgridCp.QueryByFields([
             { name: "SType", value: 2 },
             { name: "CommissionId", value: $("#ID").val() },

            ]);
        }
        $("#aCp").click(cp);
        $("#Fwxm_Filter1").click(Fwxm_Filter1);

        function Fwxm_Filter1() {
            var search = [null, null];
            var qText = $("#Pro_Name").val();
            if (qText != "") {
                search.push(qText);
            }
            else {
                search.push(null);
            }
            search.push(null, null, null, null, null);
            var qCate = $("#SER_TYPE1").val();

            if (qCate != "") {
                search.push(qCate);
            }
            else {
                search.push(null);
            }
            xjgridCp.Fitler(search, [-1, -1, 1, -1, -1, -1, -1, -1, 0]);
        }
        //会员卡-----------------------------------------------------------------------------------------------------
        var gridoptHyk = {
            url: options.detailLisUrl + "?DType=3",
            colModel: [
                     { display: '编号', name: 'ID', width: "0%", sortable: true, hide: true, align: 'left', iskey: true },
                     { display: '佣金编号', name: 'PROD_ID', width: "0%", sortable: true, hide: true, align: 'left' },
                     { display: '名称', name: 'PROD_NAME', width: "45%", sortable: false, align: 'left' },
                     { display: '佣金类型', name: 'PROD_TYPE', width: "0%", sortable: true, hide: true, align: 'left' },
                     //{ display: '项目类型', name: 'SERVICE_TYPE', width: "0%", sortable: true, hide: true, align: 'left' },
                     { display: '提成方式', name: 'COMM_TYPE', width: "10%", sortable: false, align: 'left', process: processTcfs },
                     { display: '提成金额/比例', name: 'COMMISSION', width: "15%", sortable: false, align: 'left', process: processTcje },
                     { display: '点客提成金额/比例', name: 'COMM_DK', width: "15%", sortable: false, align: 'left', process: processTcje1 },
                      { display: '是否选择', name: 'IsChecked', width: "0%", sortable: true, hide: true, align: 'left' },
                      { display: '会员卡类别', name: 'ObjDetailPro1', width: "0%", sortable: false, hide: true, align: 'left' },
                       //{ display: '操作', name: 'ID', sortable: false, align: 'center', process: processOp } , process: processTcje
            ],
            title: false,
            rp: 10,
            localpage: true,
            rowhanlder: rowBind,
            usepager: true,
            showcheckbox: true,
            autoload: false
        };
        var xjgridHyk = new xjGrid("gridlistHyk", gridoptHyk);
        function hyk() {
            xjgridHyk.QueryByFields([
             { name: "SType", value: 3 },
             { name: "CommissionId", value: $("#ID").val() },
            ]);
        }
        $("#aHyk").click(hyk);
        $("#Fwxm_Filter2").click(function (e) {
            Fwxm_Filter2();
        });

        function Fwxm_Filter2() {
            var search = [null, null];
            var qText = $("#Hyk_Name").val();
            if (qText != "") {
                search.push(qText);
            }
            else {
                search.push(null);
            }
            search.push(null, null, null, null, null);
            //var qCate = $("#SER_TYPE2").val();
            //if (qCate != "") {
            //    search.push(qCate);
            //}
            //else {
            search.push(null);
            //}
            xjgridHyk.Fitler(search, [-1, -1, 1, -1, -1, -1, -1, -1, 0]);
        }
        //疗程卡-----------------------------------------------------------------------------------------------------      
        var gridoptLck = {
            url: options.detailLisUrl + "?DType=4",
            colModel: [
                     { display: '编号', name: 'ID', width: "0%", sortable: true, hide: true, align: 'left', iskey: true },
                     { display: '佣金编号', name: 'PROD_ID', width: "0%", sortable: true, hide: true, align: 'left' },
                     { display: '名称', name: 'PROD_NAME', width: "45%", sortable: false, align: 'left' },
                     { display: '佣金类型', name: 'PROD_TYPE', width: "0%", sortable: true, hide: true, align: 'left' },
                     { display: '提成方式', name: 'COMM_TYPE', width: "10%", sortable: false, align: 'left', process: processTcfs },
                     { display: '提成金额/比例', name: 'COMMISSION', width: "15%", sortable: false, align: 'left', process: processTcje },
                     { display: '点客提成金额/比例', name: 'COMM_DK', width: "15%", sortable: false, align: 'left', process: processTcje1 },
                     { display: '是否选择', name: 'IsChecked', width: "0%", sortable: true, hide: true, align: 'left' },
                     { display: '疗程卡类别', name: 'ObjDetailPro1', width: "0%", sortable: false, hide: true, align: 'left' },
                       //{ display: '操作', name: 'ID', sortable: false, align: 'center', process: processOp } , process: processTcje
            ],
            title: false,
            rp: 10,
            localpage: true,
            rowhanlder: rowBind,
            usepager: true,
            showcheckbox: true,
            autoload: false
        };
        var xjgridLck = new xjGrid("gridlistLck", gridoptLck);
        function lck() {
            xjgridLck.QueryByFields([
             { name: "SType", value: 4 },
             { name: "CommissionId", value: $("#ID").val() },
            ]);
        }
        $("#aLck").click(lck);

        $("#Fwxm_Filter3").click(Fwxm_Filter3);

        function Fwxm_Filter3() {
            var search = [null, null];
            var qText = $("#Lck_Name").val();
            if (qText != "") {
                search.push(qText);
            }
            else {
                search.push(null);
            }
            search.push(null, null, null, null, null);
            //var qCate = $("#SER_TYPE3").val();
            //if (qCate != "") {
            //    search.push(qCate);
            //}
            //else {
            search.push(null);
            //}
            xjgridLck.Fitler(search, [-1, -1, 1, -1, -1, -1, -1, -1, 0]);
        }

        var _submiting = false;
        function PostSave(postData, url, cId) {
            if (_submiting) {
                showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting = true;
            showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            $.ajax({
                url: url,
                type: "POST",
                data: { "formdata": postData, "cId": cId },
                success: function (result) {

                    if (result.status == 0) {
                        var type = $("#CUTYPE").val();
                        if (type == "1") {
                            Fwxm_Filter();
                            //fwxm();
                        }
                        else if (type == "2") {
                            Fwxm_Filter1();
                            //cp();
                        }
                        else if (type == "3") {
                            Fwxm_Filter2();
                            //hyk();
                        }
                        else if (type == "4") {
                            Fwxm_Filter3();
                            //lck();
                        }
                        _showInfoMessage("操作成功！", 'success');

                    }
                    else {
                        _showInfoMessage("操作失败：" + result.message, 'error');
                    }
                    hideLoadingMsg();
                    _submiting = false;
                },
                error: function (result) {
                    hideLoadingMsg();
                    _submiting = false;
                    _showInfoMessage("提交表单失败：" + result, 'error');

                }
            });
        }

        function formatPostData(cell) {

            //if (cell[4] == "")
            return [cell[0], cell[1], cell[3], (cell[4] == "" ? "0" : cell[4]), (cell[5] == "" ? "0" : cell[5]),  (cell[6] == "" ? "0" : cell[6]), cell[2]].join("_");
            //else
            //    return [cell[0], cell[1], cell[3], cell[4], cell[5], cell[6], cell[2]].join("_");
        }

        //保存事件============================================================================================================
        $("#btnSaveFwxm").click(function () {
            if ($("#NAME").val() == "") {
                _showInfoMessage("名称不能为空！", 'error');
                return;
            }
            var url = options.saveUrl;
            var data = xjgridFwxm.GetCheckedRowDatas(formatPostData);

            var postData = data.join(",");
            PostSave(postData, url, $("#ID1").val());
        });

        $("#btnSaveCp").click(function () {
            var url = options.saveUrl;
            var data = xjgridCp.GetCheckedRowDatas(formatPostData);
            var postData = data.join(",");
            PostSave(postData, url, $("#ID2").val());
        });

        $("#btnSaveHyk").click(function () {
            var url = options.saveUrl;
            var data = xjgridHyk.GetCheckedRowDatas(formatPostData);
            var postData = data.join(",");
            PostSave(postData, url, $("#ID3").val());
        });

        $("#btnSaveLck").click(function () {
            var url = options.saveUrl;
            var data = xjgridLck.GetCheckedRowDatas(formatPostData);
            var postData = data.join(",");
            PostSave(postData, url, $("#ID4").val());
        });

        //取消事件===================================================================================================
        function cancelM() {
            window.location.href = options.indexUrl;
        }
        $("#btnCloseFaxx").click(cancelM);
        $("#btnCloseFwxm").click(cancelM);
        $("#btnCloseCp").click(cancelM);
        $("#btnCloseHyk").click(cancelM);
        $("#btnCloseLck").click(cancelM);

        $("#btnSetFwxm").click(function (e) {
            setAllPage(1);
        });
        $("#btnSetCp").click(function (e) {
            setAllPage(2);
        });
        $("#btnSetHyk").click(function (e) {
            setAllPage(3);
        });
        $("#btnSetLck").click(function (e) {
            setAllPage(4);
        });
        function setAllPage(prod_type) {
            $("#setSelect").val(0);
            $("#COMMISSION").val(0);
            $("#COMM_DK").val(0);
            $("#CUTYPE").val(prod_type);
            $('#EditModal').modal('show');
        }

        var _submiting1 = false;
        $("#btnSave").click(function (e) {
            var type = $("#CUTYPE").val();
            var postData = "";
            if (type == "1") {
                var data = xjgridFwxm.GetCheckedRowDatas(formatPostData);
                postData = data.join(",");
            }
            else if (type == "2") {
                var data = xjgridCp.GetCheckedRowDatas(formatPostData);
                postData = data.join(",");
            }
            else if (type == "3") {
                var data = xjgridHyk.GetCheckedRowDatas(formatPostData);
                postData = data.join(",");

            }
            else if (type == "4") {
                var data = xjgridLck.GetCheckedRowDatas(formatPostData);
                postData = data.join(",");
            }

            if (postData == "") {
                alert("请先选中要设置的项。");
                return;
            }

            if ($("#setSelect").val() == "0") {
                if (_submiting1) {
                    showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                    return;
                }
                $('#tsMsg').html("确定要清空选中的数据设置吗？");
                $('#confirmModal').modal('show');
            }
            else {
                if (_submiting1) {
                    showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                    return;
                }
                _submiting1 = true;
                showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

                $.post(options.allSetUrl, {
                    type: type, cId: $("#ID").val(), tcType: $("#setSelect").val(),
                    COMMISSION: $("#COMMISSION").val(), COMM_DK: $("#COMM_DK").val(),
                    formdata: postData
                },
                  function (res) {
                      hideLoadingMsg();
                      _submiting1 = false;
                      if (res.status == 0) {
                          if (type == "1") {
                              Fwxm_Filter();
                              //fwxm();
                          }
                          else if (type == "2") {
                              Fwxm_Filter1();
                              //cp();
                          }
                          else if (type == "3") {
                              Fwxm_Filter2();
                              //hyk();
                          }
                          else if (type == "4") {
                              Fwxm_Filter3();
                              //lck();
                          }
                          $('#EditModal').modal('hide');
                          _showInfoMessage("操作成功！", 'success');
                      }
                      else {

                          _showInfoMessage("操作失败：" + res.message, 'error');
                      }
                  },
                  "json"
            );
            }
        });

        $("#btnConfirm").click(function (e) {
            $('#confirmModal').modal('hide');
            var type = $("#CUTYPE").val();
            var postData = "";
            if (type == "1") {
                var data = xjgridFwxm.GetCheckedRowDatas(formatPostData);
                postData = data.join(",");

            }
            else if (type == "2") {
                var data = xjgridCp.GetCheckedRowDatas(formatPostData);
                postData = data.join(",");
            }
            else if (type == "3") {
                var data = xjgridHyk.GetCheckedRowDatas(formatPostData);
                postData = data.join(",");
            }
            else if (type == "4") {
                var data = xjgridLck.GetCheckedRowDatas(formatPostData);
                postData = data.join(",");
            }
            _submiting1 = true;
            showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });

            $.post(options.allSetUrl, {
                type: type, cId: $("#ID").val(), tcType: $("#setSelect").val(),
                COMMISSION: $("#COMMISSION").val(), COMM_DK: $("#COMM_DK").val(),
                formdata: postData
            },
                  function (res) {

                      hideLoadingMsg();
                      _submiting1 = false;
                      if (res.status == 0) {

                          if (type == "1") {
                              fwxm();
                          }
                          else if (type == "2") {
                              cp();
                          }
                          else if (type == "3") {
                              hyk();
                          }
                          else if (type == "4") {
                              lck();
                          }
                          $('#EditModal').modal('hide');
                          _showInfoMessage("操作成功！", 'success');
                      }
                      else {
                          _showInfoMessage("操作失败：" + res.message, 'error');
                      }

                  },
                  "json"
            );
        });

        $("#COMMISSION").inputmask("integer", { "placeholder": "" });
        $("#COMM_DK").inputmask("integer", { "placeholder": "" });
        $("#setSelect").change(function (e) {
            var selectV = $("#setSelect").val();
            if (selectV != "2") {
                $("#COMMISSION").inputmask("integer", { "placeholder": "" });
                $("#COMM_DK").inputmask("integer", { "placeholder": "" });
                if (selectV == "0" || selectV == "1") { $("#COMMISSION").val(0); $("#COMM_DK").val(0); }
            }
            else {
                $("#COMMISSION").inputmask("99.9%", { "placeholder": "" });
                $("#COMM_DK").inputmask("99.9%", { "placeholder": "" });

            }
        });

    });
})(window, undefined, jQuery);