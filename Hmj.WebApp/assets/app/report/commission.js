
; (function (window, undefined, $) {

    $(document).ready(function () {
        $('#BEGIN_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });
        $('#END_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });
        $('#SBEGIN_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });
        $('#SEND_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });
        $('#CBEGIN_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });
        $('#CEND_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd"
        });

        var day = new Date().Format("yyyy-MM-dd");
        $("#BEGIN_DATE").val(day);
        $("#END_DATE").val(day);
        $.cookie("BEGIN_DATE", $("#BEGIN_DATE").val());
        $.cookie("END_DATE", $("#END_DATE").val());
        //初始化日期时间
        var nowDay = new Date().Format("yyyy-MM-dd ");
        $('#btnORDER_DATE span').html(nowDay + "至" + nowDay);

        //时间范围快速查询
        //$('#btnORDER_DATE').daterangepicker(
        //    {
        //        ranges: {
        //            '今天': [moment(), moment()],
        //            '昨天': [moment().subtract('days', 1), moment().subtract('days', 1)],
        //            '本周': [moment().subtract('days', 6), moment()],
        //            //'上周': [moment().subtract('days', 13), moment()],
        //            '本月': [moment().startOf('month'), moment().endOf('month')],
        //            //'上月': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')],
        //            '今年': [moment().startOf('year'), moment().endOf('year')],
        //            '去年': [moment().subtract('year', 1).startOf('year'), moment().subtract('year', 1).endOf('year')],
        //        },
        //        opens: 'left',
        //        startDate: moment().subtract('days', 29),
        //        endDate: moment()
        //    },
        //    function (start, end) {
        //        //选择日期事件
        //        $('#btnORDER_DATE span').html(start.format('YYYY-MM-DD') + ' 至 ' + end.format('YYYY-MM-DD'));

        //        $("#BEGIN_DATE").val(start.format('YYYY-MM-DD'));
        //        $("#END_DATE").val(end.format('YYYY-MM-DD'));
        //        $.cookie("BEGIN_DATE", null);
        //        $.cookie("END_DATE", null);
        //        $.cookie("BEGIN_DATE", $("#BEGIN_DATE").val());
        //        $.cookie("END_DATE", $("#END_DATE").val());
        //        //先清空订单号查询条件
        //        $("#ORDER_NO").val("");
        //        //提交表单查询            
        //        $("#PformQuery").submit();

        //    }
        //);


        var day1 = new Date().Format("yyyy-MM-dd");
        $("#SBEGIN_DATE").val(day1);
        $("#SEND_DATE").val(day1);
        $.cookie("SBEGIN_DATE", $("#SBEGIN_DATE").val());
        $.cookie("SEND_DATE", $("#SEND_DATE").val());
        //初始化日期时间
        var nowDay = new Date().Format("yyyy-MM-dd ");
        $('#SbtnORDER_DATE span').html(nowDay + "至" + nowDay);

        //时间范围快速查询
        //$('#SbtnORDER_DATE').daterangepicker(
        //    {
        //        ranges: {
        //            '今天': [moment(), moment()],
        //            '昨天': [moment().subtract('days', 1), moment().subtract('days', 1)],
        //            '本周': [moment().subtract('days', 6), moment()],
        //            //'上周': [moment().subtract('days', 13), moment()],
        //            '本月': [moment().startOf('month'), moment().endOf('month')],
        //            //'上月': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')],
        //            '今年': [moment().startOf('year'), moment().endOf('year')],
        //            '去年': [moment().subtract('year', 1).startOf('year'), moment().subtract('year', 1).endOf('year')],
        //        },
        //        opens: 'left',
        //        startDate: moment().subtract('days', 29),
        //        endDate: moment()
        //    },
        //    function (start, end) {
        //        //选择日期事件
        //        $('#SbtnORDER_DATE span').html(start.format('YYYY-MM-DD') + ' 至 ' + end.format('YYYY-MM-DD'));
        //        $("#SBEGIN_DATE").val(start.format('YYYY-MM-DD'));
        //        $("#SEND_DATE").val(end.format('YYYY-MM-DD'));
        //        $.cookie("SBEGIN_DATE", null);
        //        $.cookie("SEND_DATE", null);
        //        $.cookie("SBEGIN_DATE", $("#SBEGIN_DATE").val());
        //        $.cookie("SEND_DATE", $("#SEND_DATE").val());
        //        //先清空订单号查询条件
        //        $("#ORDER_NO").val("");
        //        //提交表单查询
        //        $("#SformQuery").submit();

        //    }
        //);


        var day2 = new Date().Format("yyyy-MM-dd");
        $("#CBEGIN_DATE").val(day2);
        $("#CEND_DATE").val(day2);
        $.cookie("CBEGIN_DATE", $("#CBEGIN_DATE").val());
        $.cookie("CEND_DATE", $("#CEND_DATE").val());
        //初始化日期时间
        var nowDay = new Date().Format("yyyy-MM-dd ");
        $('#CbtnORDER_DATE span').html(nowDay + "至" + nowDay);

        //时间范围快速查询
        //$('#CbtnORDER_DATE').daterangepicker(
        //    {
        //        ranges: {
        //            '今天': [moment(), moment()],
        //            '昨天': [moment().subtract('days', 1), moment().subtract('days', 1)],
        //            '本周': [moment().subtract('days', 6), moment()],
        //            //'上周': [moment().subtract('days', 13), moment()],
        //            '本月': [moment().startOf('month'), moment().endOf('month')],
        //            //'上月': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')],
        //            '今年': [moment().startOf('year'), moment().endOf('year')],
        //            '去年': [moment().subtract('year', 1).startOf('year'), moment().subtract('year', 1).endOf('year')],
        //        },
        //        opens: 'left',
        //        startDate: moment().subtract('days', 29),
        //        endDate: moment()
        //    },
        //    function (start, end) {
        //        //选择日期事件
        //        $('#CbtnORDER_DATE span').html(start.format('YYYY-MM-DD') + ' 至 ' + end.format('YYYY-MM-DD'));
        //        $("#CBEGIN_DATE").val(start.format('YYYY-MM-DD'));
        //        $("#CEND_DATE").val(end.format('YYYY-MM-DD'));
        //        $.cookie("CBEGIN_DATE", null);
        //        $.cookie("CEND_DATE", null);
        //        $.cookie("CBEGIN_DATE", $("#CBEGIN_DATE").val());
        //        $.cookie("CEND_DATE", $("#CEND_DATE").val());
        //        //先清空订单号查询条件
        //        $("#ORDER_NO").val("");
        //        //提交表单查询
        //        $("#CformQuery").submit();

        //    }
        //);



        //var gridopt = {
        //    url: options.listUrl,
        //    colModel: [
        //            { display: '编号', name: 'ID', sortable: true, hide: true, align: 'left', iskey: true },
        //            { display: '日期', name: 'NAME', width: "15%", sortable: false, align: 'left' },
        //            { display: '分店号', name: 'STORE_NAME', width: "10%", sortable: false, align: 'left' },
        //            { display: '业务单号', name: 'TYPE', width: "15%", sortable: false, align: 'left' },
        //             { display: '顾客姓名', name: 'CUST_NO', width: "10%", sortable: false, align: 'left' },
        //            { display: '项目收入', name: 'MOBILE', width: "10%", sortable: false, align: 'left' },
        //             { display: '产品收入', name: 'MOBILE', width: "10%", sortable: false, align: 'left' },
        //            { display: '会员卡收入', name: 'GENDER', width: "10%", sortable: false, align: 'left' }
        //    ],
        //    sortname: "Id",
        //    sortorder: "ASC",
        //    title: false,
        //    rp: 20,
        //    localpage: true,
        //    usepager: true
        //};
        //var xjgrid = new xjGrid("gridlist", gridopt);

        var p_grid = null;
        var s_grid = null;
        var c_grid = null;

        PGRID();

        $("#PformQuery").submit(function () {
            p_grid.Query(this);
            return false;


        });

        $("#PbtnSearch").click(function () {
            $("#PformQuery").submit();
        })


        $("#SformQuery").submit(function () {
            s_grid.Query(this);
            return false;

        });

        $("#SbtnSearch").click(function () {
            $("#SformQuery").submit();
        })


        $("#CformQuery").submit(function () {
            c_grid.Query(this);
            return false;

        });

        $("#CbtnSearch").click(function () {
            $("#CformQuery").submit();
        })





        //根据地区找到地区下的门店
        PostGetProdTypes();

        $("#REGION").change(function () {

            $("#ID").val("second");
            $.cookie("REGION", $("#REGION").val());
            PostGetProdTypes();
        })

        $("#STORE").change(function () {

            $("#ID").val("second");
            $.cookie("STORE", $("#STORE").val());
            //alert($.cookie("STORE"));

        })

        var _submiting2 = false;
        function PostGetProdTypes() {
            if (_submiting2) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting2 = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
            var pid = $('#REGION').val();
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
                    var hl = "<select class=\"form-control\" id=\"STORE\" name=\"STORE\" ><option value=''>请选择门店名称</option>";

                    if (cuAllPs != "") {
                        for (var i = 0; i < cuAllPsArray.length; i++) {
                            var re = cuAllPsArray[i].split("_");
                            hl += "<option value=\"" + re[0] + "\">" + re[1] + "</option>";

                        }
                    }
                    hl += "</select>";
                    $("#STORE").html(hl);
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



        //根据地区找到地区下的门店
        PostGetProdTypes2();

        $("#REGION2").change(function () {

            $("#ID").val("second");
            $.cookie("REGION2", $("#REGION2").val());
            PostGetProdTypes2();
        })
        $("#STORE2").change(function () {

            $("#ID").val("second");
            $.cookie("STORE2", $("#STORE2").val());
            //alert($.cookie("STORE"));

        })
        var _submiting = false;
        function PostGetProdTypes2() {
            if (_submiting) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
            var pid = $('#REGION2').val();
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
                    var hl = "<select class=\"form-control\" id=\"STORE2\" name=\"STORE\"  ><option value=''>请选择门店名称</option>";

                    if (cuAllPs != "") {
                        for (var i = 0; i < cuAllPsArray.length; i++) {
                            var re = cuAllPsArray[i].split("_");
                            hl += "<option value=\"" + re[0] + "\">" + re[1] + "</option>";

                        }
                    }
                    hl += "<select>";
                    $("#STORE2").html(hl);
                    _submiting = false;
                }
                ,
                error: function (result) {
                    hideLoadingMsg();
                    _submiting = false;
                    _showInfoMessage("页面有异常：" + result, 'error');

                }
            });
        }


        //根据地区找到地区下的门店
        PostGetProdTypes3();

        $("#REGION3").change(function () {

            $("#ID").val("second");
            $.cookie("REGION3", $("#REGION3").val());
            PostGetProdTypes3();
        })
        $("#STORE3").change(function () {

            $("#ID").val("second");
            $.cookie("STORE3", $("#STORE3").val());
            //alert($.cookie("STORE"));

        })

        var _submiting3 = false;
        function PostGetProdTypes3() {
            if (_submiting3) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submiting3 = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
            var pid = $('#REGION3').val();
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
                    var hl = "<select class=\"form-control\" id=\"STORE3\" name=\"STORE\"  ><option value=''>请选择门店名称</option>";

                    if (cuAllPs != "") {
                        for (var i = 0; i < cuAllPsArray.length; i++) {
                            var re = cuAllPsArray[i].split("_");
                            hl += "<option value=\"" + re[0] + "\">" + re[1] + "</option>";

                        }
                    }
                    hl += "<select>";
                    $("#STORE3").html(hl);
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



        //根据项目大类找到小类

        $("#SERTYPE").change(function () {
            PostGetProdTypesSer();
        })

        var _submitingSer = false;
        function PostGetProdTypesSer() {
            if (_submitingSer) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submitingSer = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
            var pid = $('#SERTYPE').val();
            $.ajax({
                url: options.proSerUrl,
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
                    var hl = "<select class=\"form-control\" id=\"SER_SONTYPE\" name=\"SONTYPE\"  ><option value=''>请选择项目所属小类</option>";

                    if (cuAllPs != "") {
                        for (var i = 0; i < cuAllPsArray.length; i++) {
                            var re = cuAllPsArray[i].split("_");
                            hl += "<option value=\"" + re[0] + "\">" + re[1] + "</option>";

                        }
                    }
                    hl += "<select>";
                    $("#SER_SONTYPE").html(hl);
                    _submitingSer = false;
                }
                ,
                error: function (result) {
                    hideLoadingMsg();
                    _submitingSer = false;
                    _showInfoMessage("页面有异常：" + result, 'error');

                }
            });
        }


        //根据产品大类找到小类
        $("#P_Category").change(function () {
            PostGetProdTypesPro();
        })

        var _submitingpro = false;
        function PostGetProdTypesPro() {
            if (_submitingpro) {
                //showErrorTip("请耐心等待服务端操作结束", { left: 400, top: 450 }, true, 5000);
                return;
            }
            _submitingpro = true;
            //showLoadingMsg("正在努力地请求服务端,请稍后...", { left: 400, top: 450 });
            var pid = $('#P_Category').val();
            $.ajax({
                url: options.proUrl,
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
                    var hl = "<select class=\"form-control\" id=\"Pro_SONTYPE\" name=\"SONTYPE\"  ><option value=''>请选择项目所属小类</option>";

                    if (cuAllPs != "") {
                        for (var i = 0; i < cuAllPsArray.length; i++) {
                            var re = cuAllPsArray[i].split("_");
                            hl += "<option value=\"" + re[0] + "\">" + re[1] + "</option>";

                        }
                    }
                    hl += "<select>";
                    $("#Pro_SONTYPE").html(hl);
                    _submitingpro = false;
                }
                ,
                error: function (result) {
                    hideLoadingMsg();
                    _submitingpro = false;
                    _showInfoMessage("页面有异常：" + result, 'error');

                }
            });
        }






        $('#tab_product').on('shown.bs.tab', function (e) {
            PGRID();
        });

        $('#tab_service').on('shown.bs.tab', function (e) {
            SGRID();
        });

        $('#tab_card').on('shown.bs.tab', function (e) {
            CGRID();
        });


        function PGRID() {
            var gridopt = {
                url: options.P_QueryUrl + "?BEGIN_DATE=" + day + "&END_DATE=" + day,
                colModel: [
                        { display: '编号', name: 'ID', sortable: true, hide: true, align: 'left', iskey: true },
                          { display: '业务单编号', name: 'ORDERID', sortable: true, hide: true, align: 'left' },
                        { display: '顾客编号', name: 'CUSTID', sortable: true, hide: true, align: 'left' },
                      { display: '员工工号', name: 'EMP_NO', sortable: false, align: 'left' },
                      { display: '员工姓名', name: 'EMPNAME', sortable: false, align: 'left' },
                      { display: '手工单号', name: 'ORDERNO', sortable: false, align: 'left', process: OrderDetail },
                      { display: '产品名称', name: 'PROD_NAME', sortable: false, align: 'left' },
                      { display: '会员姓名', name: 'CUSTNAME', sortable: false, align: 'left', process: CustDetail },
                     { display: '分店名称', name: 'STORENAME', sortable: false, align: 'left' },
                     { display: '时间', name: 'DATES', sortable: false, align: 'left' },
                     { display: '数量', name: 'PROD_COUNT', sortable: false, align: 'left' },
                     { display: '金额', name: 'REAL_AMT', sortable: false, align: 'left' },
                     { display: '提成', name: 'COMMISSION', sortable: false, align: 'left' }
                ],
                title: false,
                rp: 10,
                usepager: true,
                localpage: true,

            };
            p_grid = new xjGrid("Productgridlist", gridopt);
        }

        function OrderDetail(value, cell) {
            var href = "";

            href = "../Order/OrderB.do?cid=" + cell[2] + "&oid=" + cell[1];

            return "<a href='" + href + "'>" + value + "</a>";
        }

        function CustDetail(value, cell) {
            var href = "";

            href = "../Customer/View360.do" + "?cid=" + cell[2];

            return "<a href='" + href + "'>" + value + "</a>";

        }

        function SGRID() {

            var gridopt2 = {
                url: options.S_QueryUrl + "?BEGIN_DATE=" + day1 + "&END_DATE=" + day1,
                colModel: [
                        { display: '编号', name: 'ID', sortable: true, hide: true, align: 'left', iskey: true },
                          { display: '业务单编号', name: 'ORDERID', sortable: true, hide: true, align: 'left' },
                        { display: '顾客编号', name: 'CUSTID', sortable: true, hide: true, align: 'left' },
                      { display: '员工工号', name: 'EMP_NO', sortable: false, align: 'left' },
                      { display: '员工姓名', name: 'EMPNAME', sortable: false, align: 'left' },
                      { display: '分店名称', name: 'STORENAME', sortable: false, align: 'left' },
                      { display: '单据号', name: 'ORDERNO', sortable: false, align: 'left', process: OrderDetail },
                      { display: '项目类别', name: 'SERVICE_CATEGORY', sortable: false, align: 'left' },
                     { display: '项目名称', name: 'SERVICE_NAME', sortable: false, align: 'left' },
                     { display: '表面号', name: 'CUSTNO', sortable: false, align: 'left' },
                     { display: '会员姓名', name: 'CUSTNAME', sortable: false, align: 'left', process: CustDetail },
                     { display: '是否点钟', name: 'IS_DK', sortable: false, align: 'left',process:formatyesorno },
                     { display: '时间', name: 'DATES', sortable: false, align: 'left' },
                     { display: '数量', name: 'SERVICE_COUNT', sortable: false, align: 'left' },
                     { display: '业绩', name: 'PERFORMANCE', sortable: false, align: 'left' },                   
                     { display: '提成', name: 'COMMISSION', sortable: false, align: 'left' }
                ],
                title: false,
                rp: 10,
                usepager: true,
                localpage: true,

            };
            s_grid = new xjGrid("Servicegridlist", gridopt2);

        }


        function formatyesorno(value,cell)
        {
            if (value == "False") {
                return "否";
            }
            else
            {
                return "是";
            }
        }
            


        function CGRID() {
            var gridopt3 = {
                url: options.C_QueryUrl + "?BEGIN_DATE=" + day2 + "&END_DATE=" + day2,
                colModel: [
                    { display: '编号', name: 'ID', sortable: true, hide: true, align: 'left', iskey: true },
                      { display: '业务单编号', name: 'ORDERID', sortable: true, hide: true, align: 'left' },
                        { display: '顾客编号', name: 'CUSTID', sortable: true, hide: true, align: 'left' },
                      { display: '员工编号', name: 'EMP_NO', sortable: false, align: 'left' },
                      { display: '员工姓名', name: 'EMPNAME', sortable: false, align: 'left' },
                      { display: '卡级名称', name: 'CARD_NAME', sortable: false, align: 'left' },
                      { display: '会员编号', name: 'CUSTNO', sortable: false, align: 'left' },
                      { display: '会员姓名', name: 'CUSTNAME', sortable: false, align: 'left', process: CustDetail },
                     { display: '分店名称', name: 'STORENAME', sortable: false, align: 'left' },
                     { display: '时间', name: 'DATES', sortable: false, align: 'left' },
                     { display: '数量', name: 'CARD_COUNT', sortable: false, align: 'left' },
                     { display: '金额', name: 'SALE_AMT', sortable: false, align: 'left' },
                     { display: '提成', name: 'COMMISSION', sortable: false, align: 'left' }
                ],
                title: false,
                rp: 10,
                usepager: true,
                localpage: true,


            };
            c_grid = new xjGrid("Cardgridlist", gridopt3);
        }

    });
})(window, undefined, jQuery);