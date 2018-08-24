; (function (window, undefined, $) {

    $(document).ready(function () {
        //初始化日期时间
        var day = new Date().Format("yyyy-MM-dd");
        //$("#ORDER_START_DATE").val(day);
        //$("#ORDER_END_DATE").val(day);
        var gridopt = {
            url: options.listUrl + "?ORDER_START_DATE=" + day + "&ORDER_END_DATE="+day+"&ORDER_STATUS=0",
            colModel: [
                    { display: '编号', name: 'ID', width: "5%", sortable: true, hide: true, align: 'left', iskey: true },
                    { display: '顾客ID', name: 'CUST_ID', width: "9%", sortable: false,hide: true,align: 'left' },
                    { display: '订单日期', name: 'CREATE_DATE', width: "9%", sortable: false, align: 'left', process: formatDate },
                    { display: '订单号', name: 'ORDER_NO', width: "10%", sortable: false, align: 'left',process:formatOrder },
                    { display: '状态', name: 'ORDER_STATUSS', width: "8%", sortable: false, align: 'left' },
                    { display: '状态', name: 'ORDER_STATUS', width: "6%", sortable: false, hide: true, align: 'left' },
                    { display: '顾客姓名', name: 'CUST_NAME', width: "9%", sortable: false, align: 'left' },
                    { display: '订单项目', name: 'PRODUCT_NAME', width: "22%", sortable: false, align: 'left' },
                    { display: '应付金额', name: 'PAY_AMT', width: "9%", sortable: false, align: 'left', process: formatRMB },
                    { display: '付款方式', name: 'PAYMENT_TYPE', width: "10%", sortable: false, align: 'left' },
                    { display: '操作', name: 'ID', width: "15%", sortable: false, align: 'left    ', process: processOp }
            ],
            sortname: "Id",
            sortorder: "ASC",
            title: false,
            rp: 10,
            usepager: true,
            showcheckbox: true
        };
        var xjgrid = new xjGrid("gridlist", gridopt);
        function processOp(value, cell) {
            var ops = [];
           
            if (cell[4] == "未付款")
            {
                ops.push("&nbsp;<a title='订单付款' class='abtn' href='javascript:;'  onclick=\"util.Pay('", value, "','", cell[1], "')\"><i class='fa fa-search' ></i> 付款</a>");
            }
            else if (cell[4] == "已付款")
            {
                ops.push("&nbsp;<a title='业绩分配' class='abtn' href='javascript:;'  onclick=\"util.EmpPer('", value, "','", cell[1], "')\"><i class='fa fa-ticket' ></i> 业绩分配</a>");
                ops.push("&nbsp;<a title='作废' class='abtn' href='javascript:;'  onclick=\"util.View('", value, "','", cell[1], "')\"><i class='fa fa-ban' ></i> 作废</a>");
            }
            return ops.join("");
        }
        function formatDate(value, cell) {
            if (value) {
                return value.split(" ")[0]
            }
            else {
                return "-"
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
            if(cents<10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((value.length - (1 + i)) / 3) ; i++)
                value = value.substring(0, value.length - (4 * i + 3)) + ',' +
                value.substring(value.length - (4 * i + 3));
            return (((sign) ? '' : '-') + '<b>￥</b>' + value + '.' + cents);
        }
        //订单详细
        function formatOrder(value, cell)
        {
            var href = "";
            if (cell[4] == "未付款") {
                //转到支付页面
                href = "/Order/OrderD.do?cid="+cell[1]+"&oid="+cell[0];
            }
            else if (cell[4] == "已付款") {
                //转到明细页面
                //转到明细页面 已作废
                href = "/Order/OrderB.do?cid=" + cell[1] + "&oid=" + cell[0];
            }
            else if (cell[4] == "已作废")
            {
                //转到明细页面 已作废
                href = "/Order/OrderB.do?cid=" + cell[1] + "&oid=" + cell[0];
            }
            return "<a href='" + href + "'>" + value + "</a>";
        }
        $("#formQuery").submit(function () {
            xjgrid.Query(this);
            return false;
        });

        $("#btnAdd").click(function (e) {
            //新增订单
            window.location.href = "/Order/OrderD.do";
        });
        $("#btnCard").click(function (e) {
            //购卡
            window.location.href = "/Order/OrderCard.do";
        });
        
        //退款
        util.Refund = function (id) {
            var url = options.editUrl + "/" + id;
            window.xjDailog.Open(url, {
                width: 680,
                height: 570,
                caption: '编辑客户信息',
                theme: "simple", //默认主题
                onclose: function (userstate) {
                    xjgrid.Reload();
                } //窗口关闭时执行的回调函数  只有窗口通过函数主动关闭时才会触发。
            });
        };
        //作废订单
        util.Cancell = function (id, name) {
            //$("#lbOrderNo").html(name);
            //$("#hdCurrentId").val(id);
            //$('#confirmModal').modal('show');
            window.location.href = "/Order/OrderB.do?cid=" + cid + "&oid=" + oid;
        };

        //查看订单
        util.View = function (oid, cid) {
            window.location.href = "/Order/OrderB.do?cid="+cid+"&oid="+oid;
        };

        //付款
        util.Pay = function (oid, cid) {
            window.location.href = "/Order/Payment.do?cid=" + cid + "&oid=" + oid;
        };

        //业绩分配
        util.EmpPer = function (oid, cid) {
            window.location.href = "/Order/EmpPer.do?cid=" + cid + "&oid=" + oid;
        };

        ////初始化日期时间
        //var nowDay = new Date().Format("yyyy-MM-dd ");     
        //$('#btnORDER_DATE span').html(nowDay + "至" + nowDay);

        ////时间范围快速查询
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
        //        $("#ORDER_START_DATE").val(start.format('YYYYMMDD'));
        //        $("#ORDER_END_DATE").val(end.format('YYYYMMDD'));
        //        //先清空订单号查询条件
        //        $("#ORDER_NO").val("");
        //        //提交表单查询
        //        $("#formQuery").submit();
       
        //    }     
        //);
        $('#ORDER_START_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd",
            autoclose: true
        });
        $('#ORDER_END_DATE').datepicker({
            language: "zh-CN",
            format: "yyyy-mm-dd",
            autoclose: true
        });
        //订单状态选择事件
        $("#ORDER_STATUS").change(function (e) {
            var status = $("#ORDER_STATUS").val();
            if (status == "0") {
                $("#btnCombine").show();
            }
            else {
                $("#btnCombine").hide();
            }
            //先清空订单号查询条件
            $("#ORDER_NO").val("");
            //提交表单查询
            $("#formQuery").submit();
         });
         
         //查询按钮事件
         $("#btnSearch").click(function()
         {
             //先清空其他条件
             //$("#ORDER_START_DATE").val("");
             //$("#ORDER_END_DATE").val("");
             //$("#ORDER_STATUS").val(null);
             //提交表单查询
             $("#formQuery").submit();
         });

        //合并订单
         $("#btnCombine").click(function () {
             var data = xjgrid.GetCheckedRowDatas(formatPostData);
             if (data == null || data == "")
             {
                 _showInfoMessage("请您先选择要合并的订单！", "info");
                 return false;
             }
             var count = data.toString().split(',').length;
             if (count <= 1)
             {
                 _showInfoMessage("合并的订单必须两个以上！", "info");
                 return false;
             }
             window.location.href = "/Order/OrderCombine.do/" + data;
         });

         function formatPostData(cell) {
             return [cell[0]].join("_");
         }

    });
})(window, undefined, jQuery);