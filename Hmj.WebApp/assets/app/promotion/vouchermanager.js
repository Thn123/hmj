; (function (window, undefined, $) {

    $(document).ready(function () {
        //if ($("#userType").val() == "0")
        //    $("#btnAdd").show();
        //else $("#btnAdd").hide();

        //是否可转让
        function formatTRANSFER(value, cell) {
            if (value == "True") {
                return "是";
            }
            else
                return "否";
        }

        //适用顾客 未使用，已使用，已过期，已作废
        function formatState(value, cell) {
            if (value == 0) {
                return "未使用";
            }
            else if (value == 1) {
                return "已使用";
            }
            else if (value == 2) {
                return "已过期";
            }
            else if (value == 3) {
                return "已作废";
            }
        }

        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0].replace("/", "-").replace("/", "-");
            }
            else {
                return "-";
            }
        }

        //金额格式化
        function formatRMB(value, cell) {
            value = value.toString().replace(/\$|\,/g, '');
            if (isNaN(value))
                value = "0";
            sign = (value == (value = Math.abs(value)));
            value = Math.floor(value * 100 + 0.50000000001);
            cents = value % 100;
            value = Math.floor(value / 100).toString();
            if (cents < 10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((value.length - (1 + i)) / 3) ; i++)
                value = value.substring(0, value.length - (4 * i + 3)) + ',' +
                value.substring(value.length - (4 * i + 3));
            return (((sign) ? '' : '-') + '<b>￥</b>' + value + '.' + cents);
        }

        $('#INVALID_DATE_picker').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });

        $('#INVALID_DATE_picker1').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });

        //渠道优惠券
        var gridopt = {
            url: options.listUrl,
            colModel: [
                    { display: '编号', name: 'ID', width: "0", sortable: false, hide: true, align: 'left', iskey: true },
                    { display: '券号', name: 'VOUCHER_NO', width: "10%", sortable: false, align: 'left' },
                    { display: '面值', name: 'FACE_VALUE', width: "10%", sortable: false, align: 'left', process: formatRMB },
                    { display: '是否可转让', name: 'IS_TRANSFER', width: "10%", sortable: false, align: 'left', process: formatTRANSFER },
                    { display: '是否可销售', name: 'CAN_SALE', width: "10%", sortable: false, align: 'left', process: formatTRANSFER },
                    { display: '销售价格', name: 'SALE_PRICE', width: "10%", sortable: false, align: 'left' },
                    { display: '发放门店', name: 'STORE_NAME', width: "15%", sortable: false, align: 'left' },
                    { display: '创建时间', name: 'CREATE_DATE', width: "9%", sortable: false, align: 'left', process: formatDate },
                    { display: '失效日期', name: 'INVALID_DATE', width: "9%", sortable: false, align: 'left', process: formatDate },
                    { display: '状态', name: 'STATUS', width: "7%", sortable: false, align: 'left', process: formatState },
                    { display: '指定顾客', name: 'CUST_NAME', width: "10%", sortable: false, align: 'left' },
                    //{ display: '操作', name: 'ID', width: "10%", sortable: false, align: 'center', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            showcheckbox: false,
            autoload: false
        };
        var xjgrid = new xjGrid("gridlist", gridopt);

        //function processOp(value, cell) {
        //    var ops = [];
        //    if (cell[10] == "" && cell[4] != "True") {
        //        ops.push("&nbsp;<a title='发放' class='abtn' href='javascript:;' onclick=\"util.Edit('", cell[1], "')\"><i class='fa fa-edit' ></i>发放</a>");
        //    }
        //    return ops.join("");
        //}
        xjgrid.Query($("#formQuery")[0]);

        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        //保存
        $('#btnAdd').click(function () {
            window.location.href = options.addVoucher;
        });
        //发放和作废代金券======================   
        //util.Edit = function (pno) {
        //    $.post(options.getVM, {
        //        objNo: pno
        //    },
        //           function (ret) {
        //               if (ret && ret.status == 0) {
        //                   $("#djq").html(ret.data.VOUCHER_NO);
        //                   $("#VOUCHER_NO1").val(ret.data.VOUCHER_NO);
        //               }
        //               else {
        //                   _showInfoMessage("数据库中没有此代金券，请刷新重试！", 'error');
        //               }
        //           },
        //           "json"
        //     );
        //    $('#EditModal1').modal('show');
        //}

        //根据姓名/电话检索
        $("#CUST_NAME").select2({
            placeholder: "输入姓名、电话查询",
            minimumInputLength: 2,
            width: 'resolve',
            ajax: {
                url: options.queryMemberUrl,
                dataType: 'json',
                quietMillis: 100,
                data: function (term, page) {
                    return {
                        q: term, //search term
                        page_limit: 10, // page size
                        page: page // page number
                    };
                },
                results: function (data, page) {
                    var strJson = eval("(" + data.data + ")");
                    return { results: strJson, more: false };
                }
            },
            formatResult: custFormatResult,
            formatSelection: custFormatSelection,
            dropdownCssClass: "bigdrop",
            formatNoMatches: function (m) {
                return "<span style='color:#7B7B7B;font-size:12px;' >未查询到相关信息</span>";
            },
            escapeMarkup: function (m) { return m; }
        });

        function custFormatResult(obj) {
            var markup = "<table class='movie-result'><tr>";
            markup += "<td class='movie-info'><div class='movie-title'>" + obj.title + "</div>";
            if (obj.mobile !== undefined) {
                markup += "<div class='movie-synopsis'>" + obj.mobile + "</div>";
            }
            markup += "</td></tr></table>"
            return markup;
        }

        //选择顾客 
        function custFormatSelection(obj) {
            $("#CUST_ID").val(obj.id);
            return obj.title;
        }

        //保存
        $('#btnSave1').click(function () {
            if ($("#CUST_ID").val() == "") {
                _showInfoMessage("请选择发放客户！", 'error');
                return;
            }
            $('#frmSave1').submit();
        });

        //保存验证
        $('#frmSave1').validator({
            rules: {
            },
            fields: {
                '#INVALID_DATE_Str1': 'required',
            },
            valid: function (form) {
                FormSubmit(form, function (res) {
                    if (res.status == 0) {
                        $('#EditModal1').modal('hide');
                        _showInfoMessage("保存成功！", 'success');
                        xjgrid.Reload();
                    }
                    else {
                        _showInfoMessage("操作失败：" + res.message, 'error');
                    }
                })
            }
        });
    });
})(window, undefined, jQuery);